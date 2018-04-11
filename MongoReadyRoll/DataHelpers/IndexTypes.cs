using System.ComponentModel;

namespace MongoReadyRoll.DataHelpers
{
    public enum IndexTypes
    {
        [Description("index")]
        SingleField,
        [Description("2dsphere")]
        TwoDSphere
    }
}
