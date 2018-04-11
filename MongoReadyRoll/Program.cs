using System.Threading.Tasks;

namespace MongoReadyRoll
{
    class Program
    {
        static void Main()
        {
            Task.Run(async () =>
            {
                var mongoroll = new MongoReadyRoll();
                await mongoroll.Execute();
            }).GetAwaiter().GetResult();
        }
    }
}
