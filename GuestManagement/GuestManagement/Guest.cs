using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuestManagement
{
    partial class Guest
    {
        private string name, gender, phone, address, moneyType, usrEmail;
        private double money;
        public string Name { get { return name; } set { name = value; } }
        public string Gender { get { return gender; } set { gender = value; } }
        public string Phone { get { return phone; } set { phone = value; } }
        public string Address { get { return address; } set { address = value; } }
        public string MoneyType { get { return moneyType; } set { moneyType = value; } }
        public double Money { get { return money; } set { money = value; } }
        public string UserEmail { get { return usrEmail; } set { usrEmail = value; } }

        public Guest() { }
        public Guest(string name, string gender, string phone, string address, string moneyType, double money, string usrEmail)
        {
            this.name = name;
            this.gender = gender;
            this.phone = phone;
            this.address = address;
            this.moneyType = moneyType;
            this.money = money;
            this.usrEmail = usrEmail;
        }
    }
}
