using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POV.WS.SendingEmailsStudents;
using System.Timers;
using System.Configuration;

namespace WS.SendingMailsStudents
{
    class Program
    {
        static Timer timer = null;
        static SendingEmailsStudents sendMail = null;
        static void Main(string[] args)
        {
            double intervaloMilisegundos = Convert.ToDouble(ConfigurationManager.AppSettings["IntervaloMilisegundos"]);
            double duracion = Convert.ToDouble(ConfigurationManager.AppSettings["Duracion"]);

            double tiempoEjecucion = duracion * intervaloMilisegundos;
            timer = new Timer(tiempoEjecucion);
            sendMail = new SendingEmailsStudents();
            sendMail.Action();
            //timer.Elapsed += timer_Elapsed;
            //timer.Start();
            Console.Read();

        }

        static void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("Proceso Iniciado");
            sendMail.Action();
            Console.WriteLine("Proceso Finalizado");
            timer.Close();
            timer.Dispose();

            GC.Collect();
        }
    }
}
