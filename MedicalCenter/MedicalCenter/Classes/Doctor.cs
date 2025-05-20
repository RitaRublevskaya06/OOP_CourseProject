using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalCenter.Classes
{
    public class Doctor
    {
        public object ID_Doctor { get; set; }
        public object Name_Doctor { get; set; }
        public object Surname_Doctor { get; set; }
        public object Middle_name_Doctor { get; set; }

        public object DoctorLogin { get; set; }
        public object DoctorPassword { get; set; }

        public object Spezialization { get; set; }
        public object Study { get; set; }
        public object Work_experience { get; set; }
        public object Photo_Doctor { get; set; }
        public byte[] Photo_Doctor_byte { get; set; }
    }
}
