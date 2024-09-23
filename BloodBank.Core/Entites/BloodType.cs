using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BloodBank.Core.Entites
{
    public enum BloodType
    {
        [EnumMember(Value ="A+")]
        APositive,
        [EnumMember(Value = "A-")]
        ANegative,
        [EnumMember(Value = "B+")]
        BPositive,
        [EnumMember(Value = "B-")]
        BNegative,
        [EnumMember(Value = "O+")]
        OPositive,
        [EnumMember(Value = "O-")]
        ONegative,
        [EnumMember(Value = "AB+")]
        ABPositive,
        [EnumMember(Value = "AB-")]
        AbNegative,

    }
}
