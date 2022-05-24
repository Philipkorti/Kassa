using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kassa.Classes
{
    class VerkaufProdukte
    {
        public int VerkaufID { get; set; }
        public int ProduktID { get; set; }
        public string ProduktName { get; set; }
        public int Anzahl { get; set; }
        public double Gesamtpreis { get; set; }
    }
}
