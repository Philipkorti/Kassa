using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kassa.Classes
{
    class Kundenanzeige
    {
        public int KundenID { get; set; }
        public string Vorname { get; set; }
        public string Nachname { get; set; }
        public int Punkte { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }

        public string Fullname
        {
            get { return Vorname + " " + Nachname; }
        }
    }
}
