using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Threading;
using System.Windows.Forms.DataVisualization.Charting;

namespace Eguraldia
{
    public partial class Form1 : Form
    {
        DatosTiempo anterior = new DatosTiempo();
        string data;
        string dataCarretera;
        List<DatosTiempo> puntos = new List<DatosTiempo>();

        public Form1()
        {
            InitializeComponent();
            webBrowser1.Navigate("http://194.30.12.29/pda/estadocarreteras/vialidad_invernal.asp");
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            data = ReceptorDatos.obtenerDatos();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            DatosTiempo datosTiempo = ProcesadorDatos.procesar(data);
            datosTiempo.estadoCarretera = dataCarretera;
            if (datosTiempo == null)
            {
                writeError("Datos no válidos");
            }
            else
            {
                if (datosTiempo.fecha != anterior.fecha)
                {
                    anterior = datosTiempo;
                    chart1.Series[0].Points.AddXY(datosTiempo.fecha.TimeOfDay + "\n" + datosTiempo.estado + "\n" + datosTiempo.estadoCarretera, datosTiempo.temperatura);
                    if (datosTiempo.estado.Contains("NIEVE"))
                        chart1.Series[0].Points.Last().Color = Color.Blue;
                    else
                        chart1.Series[0].Points.Last().Color = Color.Red;
                    puntos.Add(datosTiempo);
                    write("Recibido: " + datosTiempo.ToString());
                }
                else
                    write("Datos duplicados " + datosTiempo.ToString());
            }
        }

        private void writeError(string p)
        {
            write("ERROR: " + p);
        }

        private void write(string p)
        {
            txtBoxInfo.Text += DateTime.Now + " " + p + "\n";
            txtBoxInfo.SelectionStart = txtBoxInfo.Text.Length;
            txtBoxInfo.ScrollToCaret();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            bWorkerObtener.RunWorkerAsync();
        }

        ToolTip tooltip = new ToolTip();

        void chart1_MouseMove(object sender, MouseEventArgs e)
        {
            var pos = e.Location;
            tooltip.RemoveAll();

            var results = chart1.HitTest(pos.X, pos.Y, false, ChartElementType.DataPoint);
            foreach (var result in results)
            {
                if (result.ChartElementType == ChartElementType.DataPoint)
                {
                    var prop = result.Object as DataPoint;
                    if (prop != null)
                    {
                        tooltip.Show(prop.AxisLabel + "\n" + prop.YValues[0] + " ºC", chart1, pos.X, pos.Y - 55);
                    }
                }
            }
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            timer1.Interval = 1000 * 60 * 5;
            dataCarretera = ProcesadorDatos.procesarCarreteras(webBrowser1.Document.Body);
            bWorkerObtener.RunWorkerAsync();     
        }
    }
}
