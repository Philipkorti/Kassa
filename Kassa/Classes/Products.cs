using System;
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
        public string Lieferung { get; set; }

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
        public string Angekommen
        {
            get
            {
                DateTime date = DateTime.Now;
                DateTime dateTime;
                string datestring = date.ToString("dd MM yyyy");
                string dateTimestring; 
                if (DateTime.TryParse(Lieferung, out dateTime))
                {
                    dateTimestring = dateTime.ToString("dd MM yyyy");
                    if (dateTime > date)
                    {
                        return "Nicht Angekommen";
                    }
                    else
                    {
                        if (datestring == dateTimestring)
                        {
                            return "Kommt Heute";
                        }
                        else
                        {
                            return "Angekommen";
                        }
                        
                    }
                }
                else
                {
                    return "";
                }
            }
        }
    }
}
