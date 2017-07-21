using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CerberusExtensions
{ 
    public static class Extensions
    {
        public static Int32 ToInt(this Enum e)
        {
            return Convert.ToInt32(e);
        }
    }
}
