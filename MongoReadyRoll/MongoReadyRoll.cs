using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoReadyRoll.DataHelpers;
using MongoReadyRoll.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MongoReadyRoll
{
    public class MongoReadyRoll
    {
        private const int FilePathLength = 5;

        private readonly DatabaseSettings _dbSettings;
        public MongoReadyRoll()
        {
            using (StreamReader reader = new StreamReader("DatabaseSettings.json"))
            {
                var json = reader.ReadToEnd();
                _dbSettings = JsonConvert.DeserializeObject<DatabaseSettings>(json);
            }
        }

        public async Task Execute()
        {
            var migrationLogDb = GetMigrationLogDatabase();

            var fileEntries = await GetFiles(migrationLogDb);
            foreach (var jsonFile in fileEntries)
            {
                Console.WriteLine("Start processing " + jsonFile.FilePath);
                using (var file = File.OpenText(jsonFile.FilePath))
                using (var reader = new JsonTextReader(file))
                {
                    try
                    {
                        var allData = (JObject)JToken.ReadFrom(reader);

                        var collectionName = allData.SelectToken("CollectionName").Value<string>();
                        var setting = _dbSettings.Settings.First(x => x.CollectionName == collectionName);

                        var jsonData = allData.SelectToken("Data");
                        var client = new MongoClient(setting.ConnectionString);
                        var database = client.GetDatabase(setting.DatabaseName);

                        var migrationType = allData.SelectToken("MigrationType").Value<string>();

                        switch (migrationType)
                        {
                            case "data":
                                Console.WriteLine("Upserting Data");
                                await ScriptHelper.Execute(database, jsonData, collectionName, setting.Identifier);
                                break;
                            case "index":
                                Console.WriteLine("Upserting Indexes");
                                var indexHelper = new IndexHelper();
                                await indexHelper.Execute(database, jsonData, collectionName);
                                break;
                            default:
                                Console.WriteLine("Migration type not supported.");
                                break;
                        }

                        await SaveLogEntry(migrationLogDb, jsonFile);
                    }
                    catch (MongoException me)
                    {
                        Console.WriteLine("An Error related to MongoDb occurred: " + me.Message);
                        return;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("An Error occurred: " + e.Message);
                        return;
                    }
                }
                Console.WriteLine("Done processing " + jsonFile.FilePath);
            }
            Console.WriteLine("MongoReadyRoll completed!");
        }

        private async Task<IEnumerable<JsonFileProperty>> GetFiles(IMongoDatabase migrationLogDb)
        {
            Console.WriteLine("Finding all files for rolling in Data");
            var fileEntries = Directory.GetFiles("Data").ToList();
            Console.WriteLine($"Found {fileEntries.Count} ready roll scripts.");

            var executedScripts = await GetExecutedScripts(migrationLogDb);

            var files = new List<JsonFileProperty>();
            foreach (var fileEntry in fileEntries)
            {
                if (executedScripts.Any(x => x.ScriptFilename == fileEntry))
                {
                    continue;
                }

                var json = new JsonFileProperty
                {
                    FilePath = fileEntry
                };
                var indexOfEndOfUnixTimeStamp = fileEntry.IndexOf("-", StringComparison.Ordinal);
                json.TimeStamp = int.Parse(fileEntry.Substring(FilePathLength, indexOfEndOfUnixTimeStamp - FilePathLength));
                files.Add(json);
            }
            var orderedFiles = files.OrderBy(x => x.TimeStamp).ToList();
            
            Console.WriteLine($"Found {orderedFiles.Count} new scripts to run.");

            return orderedFiles;
        }

        private async Task<List<MigrationLog>> GetExecutedScripts(IMongoDatabase migrationLogDb)
        {
            var collection = migrationLogDb.GetCollection<MigrationLog>(_dbSettings.MigrationLogSettings.CollectionName);
            var executedScripts = await collection.Find(FilterDefinition<MigrationLog>.Empty).ToListAsync();
            return executedScripts;
        }

        private async Task SaveLogEntry(IMongoDatabase migrationLogDb, JsonFileProperty jsonFile)
        {
            var migrationLogs = migrationLogDb.GetCollection<MigrationLog>(_dbSettings.MigrationLogSettings.CollectionName);

            var newLog = new MigrationLog
            {
                Id = ObjectId.GenerateNewId().ToString(),
                ExecutionDate = DateTime.UtcNow,
                ScriptFilename = jsonFile.FilePath
            };

            await migrationLogs.InsertOneAsync(newLog);
        }

        private IMongoDatabase GetMigrationLogDatabase()
        {
            var client = new MongoClient(_dbSettings.MigrationLogSettings.ConnectionString);
            var database = client.GetDatabase(_dbSettings.MigrationLogSettings.DatabaseName);
            return database;
        }
    }
}