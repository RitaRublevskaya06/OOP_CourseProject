using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalCenter.Classes
{
    public class Procedures
    {
        public object ID_Procedures { get; set; }
        public object Name_Procedures { get; set; }
        public object Decription { get; set; }
        public object Price { get; set; }
        public object Photo { get; set; }
        public object Spezialization { get; set; }
        public byte[] Photo_Procedures_byte { get; set; }

    }
}
