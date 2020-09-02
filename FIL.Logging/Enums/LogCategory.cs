using System.Runtime.Serialization;

namespace FIL.Logging.Enums
{
    public enum LogCategory
    {
        [EnumMember]
        Info = 0,

        [EnumMember]
        Debug = 1,

        [EnumMember]
        Warn = 2,

        [EnumMember]
        Error = 3,

        [EnumMember]
        Fatal = 4,

        [EnumMember]
        Trace = 5
    }
}