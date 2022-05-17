﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kassa.Classes
{
    class Rechnung
    {
        public int ID { get; set; }
        public int User { get; set; }
        public int Anzahl { get; set; }
        public double Preis { get; set; }
        public DateTime RechnungsDatum { get; set; }

        public string Datum
        {
            get
            {
                string datum = RechnungsDatum.ToString("dd MMMM yyyy");
                return datum;
            }
        }
    }
}