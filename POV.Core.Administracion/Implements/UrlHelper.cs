using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace POV.Core.Administracion
{
    public class UrlHelper
    {
        private const string LOGIN = "~/Auth/Login.aspx";
        private const string LOGOUT = "~/Auth/Logout.aspx";
        private const string DEFAULT = "~/Default.aspx";
        private const string CONFIRMAR_DIRECTOR = "~/CuentaUsuario/ConfirmarDirector.aspx";

        private const string ACEPTAR_TERMINOS = "~/CuentaUsuario/AceptarTerminos.aspx";
        private const string SELECCIONAR_ESCUELA = "~/SeleccionarEscuela.aspx";
        private const string EDITAR_PASS = "";
        private const string REPORTES = "~/Reportes/VisorReportes.aspx";

        private const string ERROR = "~/Errors/Error.aspx";
        private const string EDITAR_DOCENTE = "~/Docentes/EditarDocente.aspx";
        private const string EDITAR_COSTOPRODUCTO = "~/Productos/EditarProducto.aspx";
        private const string EDITAR_UNIVERSIDAD = "~/Universidades/EditarUniversidad.aspx";
        private const string EDITAR_ESPECIALISTA = "~/Especialistas/EditarEspecialista.aspx";
        private const string REGISTRAR_DOCENTE = "~/Docentes/RegistrarDocente.aspx";
        private const string CONSULTAR_DOCENTE = "~/Docentes/BuscarDocentes.aspx";
        private const string CONSULTAR_PRODUCTO = "~/Productos/BuscarProductos.aspx";
        private const string CONSULTAR_UNIVERSIDAD = "~/Universidades/BuscarUniversidad.aspx";
        private const string CONSULTAR_ESPECIALISTA = "~/Especialistas/BuscarEspecialista.aspx";
        private const string ASIGNAR_DOCENTE_GRUPO = "~/Grupos/GestionarDocenteGrupo.aspx";
        private const string CONSULTAR_ALUMNOS_GRUPO = "~/Grupos/AlumnosAsignadosGrupo.aspx";
        private const string EDITAR_GRUPO = "~/Grupos/EditarGrupo.aspx";
        private const string CONSULTAR_GRUPOS = "~/Grupos/BuscarGrupos.aspx";
		private const string CONSULTAR_PRUEBAS_ASIGNADAS_GRUPO = "~/Grupos/ConsultarPruebasAsignadasGrupo.aspx";
		private const string ASIGNAR_PRUEBA_GRUPO = "~/Grupos/AsignarPruebaGrupo.aspx";

		private const string GESTIONAR_ALUMNO = "~/Grupos/GestionarAlumnosGrupo.aspx";
		private const string CONSULTAR_ALUMNOS = "~/Alumnos/BuscarAlumnos.aspx";
        private const string EDITAR_ALUMNO = "~/Alumnos/EditarAlumno.aspx";
		
        private const string SELECCIONAR_PERFIL = "~/SeleccionarPerfil.aspx";

        private const string REPORTE_ALUMNOS_DIAGNOSTICO = "~/Alumnos/ReporteResultadosDiagnosticoPorEscuela.aspx";

        public static string GetLoginURL() { return Path(LOGIN); }
        public static string GetLogoutURL() { return Path(LOGOUT); }
        public static string GetAceptarTerminosURL() { return Path(ACEPTAR_TERMINOS); }
        public static string GetConfirmarDirectorURL() { return Path(CONFIRMAR_DIRECTOR); }
        public static string GetEditarPassURL() { return Path(EDITAR_PASS); }
        public static string GetSeleccionarEscuelaURL() { return Path(SELECCIONAR_ESCUELA); }
        public static string GetReportURL() { return Path(REPORTES); }


        public static string GetErrorURL() { return Path(ERROR); }
        public static string GetEditarDocenteURL() { return Path(EDITAR_DOCENTE); }
        public static string GetEditarCostoProductoURL() { return Path(EDITAR_COSTOPRODUCTO); }
        public static string GetEditarUniversidadURL() { return Path(EDITAR_UNIVERSIDAD); }
        public static string GetEditarEspecialistaURL() { return Path(EDITAR_ESPECIALISTA); }
        public static string GetRegistrarDocenteURL() { return Path(REGISTRAR_DOCENTE); }
        public static string GetConsultarDocenteURL() { return Path(CONSULTAR_DOCENTE); }
        public static string GetConsultarProductoURL() { return Path(CONSULTAR_PRODUCTO); }
        public static string GetConsultarUniversidadURL() { return Path(CONSULTAR_UNIVERSIDAD); }
        public static string GetConsultarEspecialistaURL() { return Path(CONSULTAR_ESPECIALISTA); }

        public static string GetAsignarDocenteGrupoURL() { return Path(ASIGNAR_DOCENTE_GRUPO); }
        public static string GetConsultarAlumnosGrupoURL() { return Path(CONSULTAR_ALUMNOS_GRUPO); }
        public static string GetEditarGrupoURL() { return Path(EDITAR_GRUPO); }
        public static string GetConsultarGruposURL() { return Path(CONSULTAR_GRUPOS); }
		public static string GetConsultarPruebasAsignadasGrupoURL() { return Path(CONSULTAR_PRUEBAS_ASIGNADAS_GRUPO); }
		public static string GetAsignarPruebaGrupoURL() { return Path(ASIGNAR_PRUEBA_GRUPO); }
        
        public static string GetConsultarAlumnosURL() { return Path(CONSULTAR_ALUMNOS); }
        public static string GetEditarAlumnoURL() { return Path(EDITAR_ALUMNO); }
        public static string GetGestionarAlumnosGrupoURL() { return Path(GESTIONAR_ALUMNO); }
        public static string GetReporteAlumnosDiagnosticoURL() { return Path(REPORTE_ALUMNOS_DIAGNOSTICO); }

        public static string GetSeleccionarPerfilURL() { return Path(SELECCIONAR_PERFIL); }
        
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
