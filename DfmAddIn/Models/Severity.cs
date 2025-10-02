using System.Runtime.Serialization;

namespace Mechly.DfmAddIn.Models
{
    [DataContract]
    public enum Severity_e
    {
        [EnumMember]
        Low,

        [EnumMember]
        Medium,

        [EnumMember]
        High
    }
}