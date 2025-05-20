using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration; // Для ConfigurationManager
using System.IO;            // Для File

namespace MedicalCenter.Classes
{
    public class Coupon
    {
        public object ID_Coupon { get; set; }
        public object Date { get; set; }
        public object time { get; set; }
        public object ID_Doctor { get; set; }
        public object Ordered { get; set; }
    }
}
