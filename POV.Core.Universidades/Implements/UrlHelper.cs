using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace POV.Core.Universidades
{
    public class UrlHelper
    {
        private const string LOGIN = "~/Auth/Login.aspx";
        private const string LOGOUT = "~/Auth/Logout.aspx";
        private const string DEFAULT = "~/Default.aspx";
        private const string CONFIRMAR_UNIVERSIDAD = "~/CuentaUsuario/ConfirmarUniversidad.aspx";

        private const string ACEPTAR_TERMINOS = "~/CuentaUsuario/AceptarTerminos.aspx";
        private const string SELECCIONAR_ESCUELA = "~/SeleccionarEscuela.aspx";
        private const string EDITAR_PASS = "~/Pages/EditarContrasena.aspx";
        private const string REPORTES = "~/Reportes/VisorReportes.aspx";

        private const string ERROR = "~/Errors/Error.aspx";
        private const string EDITAR_ORIENTADOR = "~/Orientadores/EditarOrientador.aspx";
        private const string EDITAR_CARRERA = "~/Carreras/NuevaCarrera.aspx";
        private const string EDITAR_ESPECIALISTA = "~/Especialistas/EditarEspecialista.aspx";
        private const string REGISTRAR_ORIENTADOR = "~/Orientadores/RegistrarOrientador.aspx";
        private const string CONSULTAR_ORIENTADOR = "~/Orientadores/BuscarOrientadores.aspx";
        private const string VINCULAR_EXPEDIENTE = "~/Pages/VincularExpediente.aspx";
        private const string CONSULTAR_ESPECIALISTA = "~/Especialistas/BuscarEspecialista.aspx";
        private const string ASIGNAR_ORIENTADOR_GRUPO = "~/Grupos/GestionarOrientadorGrupo.aspx";
        private const string CONSULTAR_ALUMNOS_GRUPO = "~/Grupos/AlumnosAsignadosGrupo.aspx";
        private const string EDITAR_GRUPO = "~/Grupos/EditarGrupo.aspx";
        private const string CONSULTAR_GRUPOS = "~/Grupos/BuscarGrupos.aspx";
		private const string CONSULTAR_PRUEBAS_ASIGNADAS_GRUPO = "~/Grupos/ConsultarPruebasAsignadasGrupo.aspx";
		private const string ASIGNAR_PRUEBA_GRUPO = "~/Grupos/AsignarPruebaGrupo.aspx";

		private const string GESTIONAR_ALUMNO = "~/Grupos/GestionarAlumnosGrupo.aspx";
		private const string CONSULTAR_ALUMNOS = "~/Alumnos/BuscarAlumnos.aspx";
        private const string EDITAR_UNIVERSIDAD = "~/Pages/EditarPerfil.aspx";
		
        private const string SELECCIONAR_PERFIL = "~/SeleccionarPerfil.aspx";

        private const string REPORTE_ALUMNOS_DIAGNOSTICO = "~/Alumnos/ReporteResultadosDiagnosticoPorEscuela.aspx";

        private const string CONSULTAR_CARRERAS = "~/Carreras/BuscarCarreras.aspx";
        private const string VINCULAR_CARRERAS = "~/Carreras/VincularCarreras.aspx";

        private const string CONSULTAR_EVENTOS = "~/Eventos/BuscarEventos.aspx";
        private const string EDITAR_EVENTO = "~/Eventos/EditarEvento.aspx";
        
        public static string GetLoginURL() { return Path(LOGIN); }
        public static string GetLogoutURL() { return Path(LOGOUT); }
        public static string GetAceptarTerminosURL() { return Path(ACEPTAR_TERMINOS); }
        public static string GetConfirmarUniversidadURL() { return Path(CONFIRMAR_UNIVERSIDAD); }
        public static string GetEditarPassURL() { return Path(EDITAR_PASS); }
        public static string GetSeleccionarEscuelaURL() { return Path(SELECCIONAR_ESCUELA); }
        public static string GetReportURL() { return Path(REPORTES); }


        public static string GetErrorURL() { return Path(ERROR); }
        public static string GetEditarOrientadorURL() { return Path(EDITAR_ORIENTADOR); }
        public static string GetVincularExpedienteURL() { return Path(VINCULAR_EXPEDIENTE); }
        public static string GetEditarCarreraURL() { return Path(EDITAR_CARRERA); }
        public static string GetEditarEspecialistaURL() { return Path(EDITAR_ESPECIALISTA); }
        public static string GetRegistrarOrientadorURL() { return Path(REGISTRAR_ORIENTADOR); }
        public static string GetConsultarOrientadorURL() { return Path(CONSULTAR_ORIENTADOR); }
        public static string GetConsultarEspecialistaURL() { return Path(CONSULTAR_ESPECIALISTA); }

        public static string GetAsignarOrientadorGrupoURL() { return Path(ASIGNAR_ORIENTADOR_GRUPO); }
        public static string GetConsultarAlumnosGrupoURL() { return Path(CONSULTAR_ALUMNOS_GRUPO); }
        public static string GetEditarGrupoURL() { return Path(EDITAR_GRUPO); }
        public static string GetConsultarGruposURL() { return Path(CONSULTAR_GRUPOS); }
		public static string GetConsultarPruebasAsignadasGrupoURL() { return Path(CONSULTAR_PRUEBAS_ASIGNADAS_GRUPO); }
		public static string GetAsignarPruebaGrupoURL() { return Path(ASIGNAR_PRUEBA_GRUPO); }
        
        public static string GetConsultarAlumnosURL() { return Path(CONSULTAR_ALUMNOS); }
        public static string GetEditarUniversidadURL() { return Path(EDITAR_UNIVERSIDAD); }
        public static string GetGestionarAlumnosGrupoURL() { return Path(GESTIONAR_ALUMNO); }
        public static string GetReporteAlumnosDiagnosticoURL() { return Path(REPORTE_ALUMNOS_DIAGNOSTICO); }

        public static string GetSeleccionarPerfilURL() { return Path(SELECCIONAR_PERFIL); }

        public static string GetConsultarCarrerasURL() { return Path(CONSULTAR_CARRERAS); }
        public static string GetVincularCarrerasURL() { return Path(VINCULAR_CARRERAS); }

        public static string GetConsultarEventosURL() { return Path(CONSULTAR_EVENTOS); }
        public static string GetEditarEventoURL() { return Path(EDITAR_EVENTO); }

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
