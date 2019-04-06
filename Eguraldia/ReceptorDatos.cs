using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.IO;
using System.Web;
using System.Windows.Forms;

namespace Eguraldia
{
    static class ReceptorDatos
    {
        public static string obtenerDatos()
        {
            WebClient client = new WebClient();

            if (WebRequest.DefaultWebProxy != null)
            {
                client.Proxy = WebRequest.DefaultWebProxy;
                client.Credentials = CredentialCache.DefaultCredentials;
                client.Proxy.Credentials = CredentialCache.DefaultNetworkCredentials;
            }

            string downloadString = client.DownloadString("http://194.30.12.29/aldia/meteo/estmeteo.shtml");

            return downloadString;
        }
    }
}
