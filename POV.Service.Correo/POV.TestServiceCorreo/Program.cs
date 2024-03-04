using POV.Envio.Correo;
using POV.Licencias.BO;
using POV.Seguridad.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace POV.TestServiceCorreo
{
    class Program
    {
        static void Main(string[] args)
        {            
            ServiceEnvioCorreo e = new ServiceEnvioCorreo();
            e.Action();
            //e.getDataUsuarioLicenciaToList();
            //object sender=new object();
            //double intervalo = 1;
            //Timer tmServicio = null;
            //double conversionmddia = intervalo * 3600000; // 1 hora en tiempo real
            //tmServicio = new Timer(conversionmddia);
            //tmServicio.Elapsed += new ElapsedEventHandler(e.tmServicio_Elapsed);
            //ElapsedEventArgs ee;
            //e.tmServicio_Elapsed(sender, ElapsedEventArgs ee);
            //e.enviarCorreo();
            System.Console.WriteLine("Hola");
            Console.ReadKey();
        }
    }
}
