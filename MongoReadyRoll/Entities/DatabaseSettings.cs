using System.Collections.Generic;

namespace MongoReadyRoll.Entities
{
    public class DatabaseSettings
    {
        public MigrationLogSettings MigrationLogSettings { get; set; }
        public IEnumerable<DatabaseSetting> Settings { get; set; }
    }
}