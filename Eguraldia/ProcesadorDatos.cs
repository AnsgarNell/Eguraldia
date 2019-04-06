using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Globalization;

namespace Eguraldia
{
    static class ProcesadorDatos
    {
        internal static DatosTiempo procesar(string data)
        {
            string datos = GetSubString(data, "<input type=\"hidden\" id=\"opacua\" value=\"Nombre de la Estación: OPACUA", "\">");
            
            // Si no hay conexión abortar
            if (datos == null)
                return null;

            try
            {
                // Obtenemos los datos
                DatosTiempo datosTiempo = new DatosTiempo();
                string fecha = GetSubString(datos, "Fecha: ", "\r\n");
                datosTiempo.fecha = DateTime.Parse(fecha);
                string estado = GetSubString(datos, "ESTADO DEL TIEMPO : ", " <br>");
                datosTiempo.estado = estado;
                string temperatura = GetSubString(datos, "TEMPERATURA AIRE : ", " ºC<br>");
                datosTiempo.temperatura = Double.Parse(temperatura, CultureInfo.InvariantCulture);
                return datosTiempo;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private static string GetSubString(string data, string strFirst, string strLast)
        {
            int first = data.IndexOf(strFirst) + strFirst.Length;
            int last = data.IndexOf(strLast, first);
            if ((first > 0) && (last > first))
            {
                string str2 = data.Substring(first, last - first);
                return str2;
            }
            else
                return null;
        }

        internal static string procesarCarreteras(HtmlElement body)
        {
            string datos = GetSubString(body.InnerText, "A-2128PTO. OPAKUA", "\r\n");
            return datos;
        }
    }
}
