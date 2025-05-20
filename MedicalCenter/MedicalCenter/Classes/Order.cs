using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalCenter.Classes
{
    public class Order
    {
        public object Order_number { get; set; }
        public object Date { get; set; }
        public object Time { get; set; }
        public object ID_Doctor { get; set; }
        public object ID_Client { get; set; }
        public object ID_Coupon { get; set; }
        public object ID_Procedures { get; set; }
        public object Name_Procedures { get; set; }
        public object Name_Doctor { get; set; }
        public object Middle_name_Doctor { get; set; }
        public object Price { get; set; }

        public object Status { get; set; } // 'Pending', 'Completed'
        public int ID_DoctorValue { get; set; }
        public int ID_ClientValue { get; set; }

    }
}
