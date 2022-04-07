using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kassa.Classes
{
    class User
    {
        public int ID { set; get; }
        public string Vorname { set; get; }
        public string Nachname { set; get; }
        public DateTime Einstellungsdatum { set; get; }
        public string Rechte { set; get; }

        public string FullName
        {
            get
            {
                return Vorname + " " + Nachname;
            }
        }
    }
}
