using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BloodBank.Core.Entites
{
    public enum Gender
    {
        [EnumMember(Value ="Male")]
        Male,
        [EnumMember(Value = "Female")]
        Female,

    }
}
