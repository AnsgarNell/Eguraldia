using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eguraldia
{
    class DatosTiempo
    {
        public DateTime fecha { get; set; }
        public string estado { get; set; }
        public double temperatura { get; set; }
        public string estadoCarretera { get; set; }

        public override string ToString()
        {
            return fecha + " " + estado + " " + temperatura + " °C " + estadoCarretera;
        }
    }
}
