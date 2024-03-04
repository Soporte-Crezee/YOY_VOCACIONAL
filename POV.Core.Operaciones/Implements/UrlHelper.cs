using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace POV.Core.Operaciones
{
    public class UrlHelper
    {
        private const string LOGIN = "~/Auth/Login.aspx";
        private const string LOGOUT = "~/Auth/Logout.aspx";
        private const string DEFAULT = "~/Default.aspx";

        private const string ERROR = "~/Errors/Error.aspx";

        private const string REGISTRARNUEVAESCUELA = "~/Escuelas/NuevaEscuela.aspx";
        private const string EDITARESCUELA = "~/Escuelas/EditarEscuela.aspx";
        private const string CONSULTARESCUELAS = "~/Escuelas/BuscarEscuelas.aspx";

        private const string CONSULTARDIRECTORES = "~/Directores/BuscarDirectores.aspx";
        private const string EDITARDIRECTOR = "~/Directores/EditarDirector.aspx";
        private const string REGISTRARNUEVODIRECTOR = "~/Directores/RegistrarDirector.aspx";

        private const string CONSULTARCICLOSESCOLARES = "~/CiclosEscolares/BuscarCicloEscolar.aspx";
        private const string EDITARCICLOESCOLAR = "~/CiclosEscolares/EditarCicloEscolar.aspx";

        private const string CONFIGURARTRAMA = "~/Configuracion/ConfigurarTrama.aspx";
        private const string EDITARVARIABLETRAMA = "~/Configuracion/EditarVariableTrama.aspx";

        private const string CONSULTARCONTRATOS = "~/Contratos/BuscarContrato.aspx";
        private const string CONFIGURARRECURSOCICLO = "~/Contratos/Recursos/ConfigurarRecursosCiclo.aspx";

        private const string CONSULTARASISTENCIAS = "~/Profesionalizacion/CatalogoAsistencias/BuscarAsistencias.aspx";
        private const string EDITARASISTENCIA = "~/Profesionalizacion/CatalogoAsistencias/EditarAsistencia.aspx";
        private const string REGISTRARASISTENCIA = "~/Profesionalizacion/CatalogoAsistencias/RegistrarAsistencia.aspx";

        private const string CONSULTARTEMAASISTENCIAS = "~/Profesionalizacion/CatalogoTemaAsistencia/BuscarTemaAsistencia.aspx";
        private const string EDITARTEMAASISTENCIA = "~/Profesionalizacion/CatalogoTemaAsistencia/EditarTemaAsistencia.aspx";
        private const string REGISTRARTEMAASISTENCIA = "~/Profesionalizacion/CatalogoTemaAsistencia/RegistrarTemaAsistencia.aspx";

        private const string CONSULTARTEMACURSOS = "~/Profesionalizacion/CatalogoTemaCurso/BuscarTemaCurso.aspx";
        private const string EDITARTEMACURSO = "~/Profesionalizacion/CatalogoTemaCurso/EditarTemaCurso.aspx";
        private const string REGISTRARTEMACURSO = "~/Profesionalizacion/CatalogoTemaCurso/RegistrarTemaCurso.aspx";

        private const string CONSULTAREJESTEMATICOS = "~/Profesionalizacion/EjesTematicos/BuscarEjeTematico.aspx";

        private const string CONFIGURARSITUACIONAPRENDIZAJE ="~/Profesionalizacion/EjesTematicos/ConfigurarSituacionesAprendizaje.aspx";

        public static string GetLoginURL() { return Path(LOGIN); }
        public static string GetLogoutURL() { return Path(LOGOUT); }

        public static string GetConsultarEscuelasURL() { return Path(CONSULTARESCUELAS); }
        public static string GetEditarEscuelaURL() { return Path(EDITARESCUELA); }
        public static string GetRegistrarNuevaEscuelaURL() { return Path(REGISTRARNUEVAESCUELA); }

        public static string GetConsultarDirectoresURL() { return Path(CONSULTARDIRECTORES); }
        public static string GetEditarDirectorURL() { return Path(EDITARDIRECTOR); }
        public static string GetRegistrarNuevoDirectorURL() { return Path(REGISTRARNUEVODIRECTOR); }

        public static string GetConsultarCiclosEscolaresURL() { return Path(CONSULTARCICLOSESCOLARES); }
        public static string EditarCicloEscolarURL() { return Path(EDITARCICLOESCOLAR); }

        public static string GetConfigurarTramaURL() { return Path(CONFIGURARTRAMA); }
        public static string GetEditarVariableTramaURL() { return Path(EDITARVARIABLETRAMA); }

        public static string GetConsultarContratosURL() { return Path(CONSULTARCONTRATOS); }
        public static string GetConfigurarRecursosCicloURL() { return Path(CONFIGURARRECURSOCICLO); }

        public static string GetConsultarAsistenciasURL() { return Path(CONSULTARASISTENCIAS); }
        public static string GetEditarAsistenciaURL() { return Path(EDITARASISTENCIA); }
        public static string GetRegistrarAsistenciaURL() { return Path(REGISTRARASISTENCIA); }

        public static string GetConsultarTemaAsistenciasURL() { return Path(CONSULTARTEMAASISTENCIAS); }
        public static string GetEditarTemaAsistenciaURL() { return Path(EDITARTEMAASISTENCIA); }
        public static string GetRegistrarTemaAsistenciaURL() { return Path(REGISTRARTEMAASISTENCIA); }

        public static string GetConsultarTemaCursosURL() { return Path(CONSULTARTEMACURSOS); }
        public static string GetEditarTemaCursoURL() { return Path(EDITARTEMACURSO); }
        public static string GetRegistrarTemaCursoURL() { return Path(REGISTRARTEMACURSO); }

        public static string GetConsultarEjesTematicosURL() { return Path(CONSULTAREJESTEMATICOS); }

        public static string GetConfigurarSituacionesAprendizajeURL() { return Path(CONFIGURARSITUACIONAPRENDIZAJE); }

        #region inicio
        public static string GetDefaultURL() { return Path(DEFAULT); }
        #endregion

        #region auxiliares
        private static string Path(string virtualPath)
        {
            return VirtualPathUtility.ToAbsolute(virtualPath);
        }

        private static string Path(string virtualPath, params object[] args)
        {
            return Path(string.Format(virtualPath, args));
        }
        #endregion    
    }
}
