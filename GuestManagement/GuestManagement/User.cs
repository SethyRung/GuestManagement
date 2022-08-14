using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuestManagement
{
    public partial class User
    {
        private string emale, pass, MName, FName, type;
        private DateTime date;

        public User() { }
        public User(string emale, string pass, string MName, string FName, string type, DateTime date)
        {
            this.emale = emale;
            this.pass = pass;
            this.MName = MName;
            this.FName = FName;
            this.type = type;
            this.date = date;
        }

        public string Email 
        {
            get => emale;
            set => emale = value;
        }
        public string Password
        {
            get => pass;
            set => pass = value;
        }
        public string MaleName
        {
            get => MName;
            set => MName = value;
        }
        public string FemaleName
        {
            get => FName;
            set => FName = value;
        }
        public string Type
        {
            get => type;
            set => type = value;
        }
        public DateTime Date
        {
            get => date;
            set => date = value;
        }
    }
}
