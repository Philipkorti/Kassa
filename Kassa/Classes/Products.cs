﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kassa.Classes
{
    class Products : ICloneable
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public double Preis { get; set; }
        public int InStock { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public double Gesamtpreis
        {
            get
            {
                return Preis * InStock;
            }
        }
    }
}
