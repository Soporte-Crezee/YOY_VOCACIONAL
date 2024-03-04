using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POV.Core.PadreTutor.Implements
{
    public class UrlHlp
    {
        private const string LOGIN = "~/Auth/Login.aspx";
        private const string LOGOUT = "~/Auth/Logout.aspx";
        private const string DEFAULT = "~/Default.aspx";
        private const string EDITAR_PASS = "~/Pages/EditarContrasena.aspx";
        private const string EDITAR_PERFIL = "~/Pages/EditarPerfil.aspx";
        private const string EXPEDIENTE = "~/Pages/ExpedienteAlumnoTutor.aspx";
        private const string RESULTADOHABITOS = "~/Pages/Reportes/ResultadoPruebaHabitosTutorado.aspx";
        private const string RESULTADODOMINOS = "~/Pages/Reportes/ResultadoPruebaDominosTutorado.aspx";
        private const string RESULTADOTERMAN = "~/Pages/Reportes/ResultadoPruebaTermanTutorado.aspx";
        private const string RESULTADOSACKS = "~/Pages/Reportes/ResultadoPruebaSacksTutorado.aspx";
        private const string RESULTADOCLEAVER = "~/Pages/Reportes/ResultadoPruebaCleaverTutorado.aspx";
        private const string RESULTADOCHASIDE = "~/Pages/Reportes/ResultadoPruebaChasideTutorado.aspx";
        private const string RESULTADOALLPORT = "~/Pages/Reportes/ResultadoPruebaAllportTutorado.aspx";
        private const string RESULTADOKUDER = "~/Pages/Reportes/ResultadoPruebaKuderTutorado.aspx";
        private const string RESULTADOROTTER = "~/Pages/Reportes/ResultadoPruebaRotterTutorado.aspx";
        private const string RESULTADORAVEN = "~/Pages/Reportes/ResultadoPruebaRavenTutorado.aspx";
        private const string RESULTADOFRASES = "~/Pages/Reportes/ResultadoPruebaFrasesVocacionalesTutorado.aspx";
        private const string RESULTADOZAVIC = "~/Pages/Reportes/ResultadoPruebaZavicTutorado.aspx";
        private const string INVITAR_TUTORADO = "~/Pages/Tutorados.aspx";
        private const string PAQUETE_PREMIUM = "~/ComprasPortal/AdquirirPaquete.aspx";
        private const string COMPRA_CREDITO = "~/ComprasPortal/AdquirirCredito.aspx";
        private const string IMAGEN_PERFIL = "~/Files/ImagenUsuario/ImagenPerfil.aspx?r={0}&img={1}&usr={2}";
        private const string ConfirmarTutor = "~/Pages/ConfirmarTutor.aspx";
        private const string ACEPTAR_TERMINOS = "~/Pages/AceptarTerminos.aspx";


        public static string GetDefaultURL() 
        {
            return Path(DEFAULT);
        }
        public static string GetLoginURL()
        {
            return Path(LOGIN);
        }

        public static string GetEditarPassURL()
        {
            return Path(EDITAR_PASS);
        }

        public static string GetLogoutURL()
        {
            return Path(LOGOUT);
        }

        public static string GetEditarPerfilURL()
        {
            return Path(EDITAR_PERFIL);
        }

        public static string GetExpedienteURL()
        {
            return Path(EXPEDIENTE);
        }

        public static string GetResultadoHabitosURL()
        {
            return Path(RESULTADOHABITOS);
        }

        public static string GetResultadoDominosURL()
        {
            return Path(RESULTADODOMINOS);
        }

        public static string GetResultadoTermanURL()
        {
            return Path(RESULTADOTERMAN);
        }

        public static string GetResultadoSACKSURL()
        {
            return Path(RESULTADOSACKS);
        }

        public static string GetResultadoCleaverURL()
        {
            return Path(RESULTADOCLEAVER);
        }

        public static string GetResultadoChasideURL()
        {
            return Path(RESULTADOCHASIDE);
        }

        public static string GetResultadoAllportURL()
        {
            return Path(RESULTADOALLPORT);
        }

        public static string GetResultadoKuderURL()
        {
            return Path(RESULTADOKUDER);
        }

        public static string GetResultadoRotterURL() 
        {
            return Path(RESULTADOROTTER);
        }

        public static string GetResultadoRavenURL()
        {
            return Path(RESULTADORAVEN);
        }

        public static string GetResultadoFrasesVocacionalesURL()
        {
            return Path(RESULTADOFRASES);
        }

        public static string GetResultadoZavicURL()
        {
            return Path(RESULTADOZAVIC);
        }

        public static string GetInvitacionURL() 
        {
            return Path(INVITAR_TUTORADO);
        }

        public static string GetPaquetesPremiumURL() 
        {
            return Path(PAQUETE_PREMIUM);
        }

        public static string GetCompraCreditoURL() 
        {
            return Path(COMPRA_CREDITO);
        }

        public static string GetImagenPerfilURL(string tipo, long usocialID)
        {
            Random rand = new Random((int)DateTime.Now.Ticks);
            return Path(IMAGEN_PERFIL, rand.Next(1, 1000), tipo, usocialID);
        }

        public static string GetConfirmarTutorURL()
        {
            return Path(ConfirmarTutor);
        }


        public static string GetAceptarTerminosURL()
        {
            return Path(ACEPTAR_TERMINOS);
        }

        #region auxiliares
        private static string Path(string virtualPath)
        {
            string s = VirtualPathUtility.ToAbsolute(virtualPath);
            return s;
        }

        private static string Path(string virtualPath, params object[] args)
        {
            return Path(string.Format(virtualPath, args));
        }
        #endregion
    }
}