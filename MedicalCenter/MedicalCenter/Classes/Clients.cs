using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration; 
using System.IO;    

namespace MedicalCenter.Classes
{
    public class Clients
    {
        public object ID_Clients { get; set; }
        public object Name_Client { get; set; }
        public object Surname_Client { get; set; }
        public object Order_number { get; set; }
        public object Email { get; set; }
        public object Login { get; set; }
        public object Password { get; set; }
        public byte[] Image { get; set; } = Array.Empty<byte>();

    }
}
