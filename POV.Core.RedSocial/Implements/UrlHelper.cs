using System;
using System.Web;

namespace POV.Core.RedSocial.Implement
{
	public class UrlHelper
	{
		private const string LOGIN = "~/Auth/Login.aspx";
        private const string LOGINASPIRANTE = "~/Auth/Login.aspx?u=asp";
        private const string LOGINORIENTADOR = "~/Auth/Login.aspx?u=ori";
		private const string LOGOUT = "~/Auth/Logout.aspx";
		private const string DEFAULT = "~/Default.aspx";
		private const string PAGE404 = "~/404.aspx";
		private const string EDITAR_PASS = "~/CuentaUsuario/EditarContrasena.aspx";
		private const string IMAGEN_PERFIL = "~/Files/ImagenUsuario/ImagenPerfil.aspx?r={0}&img={1}&usr={2}";

		private const string ALUMNO_NOTICIAS = "~/PortalAlumno/Noticias.aspx";
		private const string ALUMNO_MURO = "~/PortalAlumno/Muro.aspx?u={0}";
		private const string ALUMNO_REACTIVOS = "~/PortalAlumno/MisReactivos.aspx?u={0}";
		private const string GRUPO_NOTICIAS_DOCENTES = "~/PortalGrupo/NoticiasDocentes.aspx";
		private const string GRUPO_NOTICIAS_MIDOCENTE = "~/PortalGrupo/MiDocente.aspx?u={0}";
		private const string GRUPO_LISTA = "~/PortalGrupo/Alumnos.aspx";

		private const string DOCENTE_INICIO = "~/PortalDocente/Inicio.aspx";
		private const string DOCENTE_CAMBIAR_ESCUELA = "~/SeleccionarEscuela.aspx";
		private const string DOCENTE_GRUPO_MURO = "~/PortalDocente/Muro.aspx?gs={0}";
		private const string DOCENTE_GRUPO_NOTICIAS = "~/PortalDocente/Noticias.aspx?gs={0}";
		private const string DOCENTE_ALUMNOS = "~/PortalDocente/Alumnos.aspx?gs={0}";
		private const string DOCENTE_REPORTES = "~/PortalDocente/Reportes/Reportes.aspx?gs={0}";
        private const string ORIENTADOR_CALENDAR = "~/PortalDocente/OrientacionVocacional.aspx";

		private const string SOCIAL_MENSAJES = "~/Social/Mensajes.aspx";
		private const string SOCIAL_NOTIFICACIONES = "~/Social/NotificacionesSocial.aspx";
		private const string SOCIAL_VER_PUBLICACION = "~/Social/VerPublicacion.aspx?n={0}";
		private const string SOCIAL_EDITAR_PERFIL = "~/Social/EditarPerfil.aspx";
		private const string SOCIAL_PERFIL = "~/Social/Perfil.aspx?u={0}";
		private const string SOCIAL_REPORTES_ABUSO = "~/Social/ReportesAbuso.aspx";
		private const string SOCIAL_VER_REPORTE_ABUSO = "~/Social/VerReporteAbuso.aspx";
        private const string SOCIAL_ESTADO_PROCESO = "~/PortalAlumno/Noticias.aspx";
        private const string SOCIAL_MIS_INTERESES = "~/Social/MisIntereses.aspx";
        private const string SELECCIONAR_CARRERA = "~/PortalAlumno/SeleccionarCarrera.aspx";
        private const string SOCIAL_EXPEDIENTE = "~/PortalAlumno/ExpedienteAlumno.aspx";
        private const string SOCIAL_REPORTE = "~/PortalAlumno/Reportes/ExpedienteAlumnoReport.aspx";
        private const string SOCIAL_REPORTE_HABITOS = "~/PortalAlumno/Reportes/ReporteAlumnoResultadoHabitos.aspx";
        private const string SOCIAL_REPORTE_DOMINOS = "~/PortalAlumno/Reportes/ReporteAlumnoResultadoDominos.aspx";
        private const string SOCIAL_REPORTE_TERMAN = "~/PortalAlumno/Reportes/ReporteAlumnoResultadoTerman.aspx";
        private const string SOCIAL_REPORTE_KUDER = "~/PortalAlumno/Reportes/ReporteAlumnoResultadoKuder.aspx";
        private const string SOCIAL_REPORTE_SACKS = "~/PortalAlumno/Reportes/ReporteAlumnoResultadoSACKS.aspx";
        private const string SOCIAL_REPORTE_CLEAVER = "~/PortalAlumno/Reportes/ReporteAlumnoResultadoCleaver.aspx";
        private const string SOCIAL_REPORTE_ALLPORT = "~/PortalAlumno/Reportes/ReporteAlumnoResultadoAllport.aspx";
        private const string SOCIAL_REPORTE_CHASIDE = "~/PortalAlumno/Reportes/ReporteAlumnoResultadoChaside.aspx";
        private const string SOCIAL_REPORTE_ROTTER = "~/PortalAlumno/Reportes/ReporteAlumnoResultadoRotter.aspx";
        private const string SOCIAL_REPORTE_RAVEN = "~/PortalAlumno/Reportes/ReporteAlumnoResultadoRaven.aspx";
        private const string SOCIAL_REPORTE_FRASES = "~/PortalAlumno/Reportes/ReporteAlumnoResultadoFrasesVocacionales.aspx";
        private const string SOCIAL_REPORTE_ZAVIC = "~/PortalAlumno/Reportes/ReporteAlumnoResultadoZavic.aspx";
        private const string SOCIAL_REPORTE_ESTILOS = "~/PortalAlumno/Reportes/ReporteAlumnoResultadoEstilosdeAprendizaje.aspx";
        private const string SOCIAL_REPORTE_INTMUL = "~/PortalAlumno/Reportes/ReporteAlumnoResultadoInteligenciasMultiples.aspx";
        private const string SOCIAL_REPORTE_INVINT = "~/PortalAlumno/Reportes/ReporteAlumnoResultadoInventariodeIntereses.aspx";
        private const string SOCIAL_REPORTE_INVHERRERA = "~/PortalAlumno/Reportes/ReporteAlumnoResultadoInventarioHerrera.aspx";
        private const string SOCIAL_REPORTE_SUCESOS = "~/PortalAlumno/Reportes/ReporteAlumnoResultadoSucesosdeVida.aspx";
        private const string SOCIAL_REPORTE_LUSCHER = "~/PortalAlumno/Reportes/ReporteAlumnoResultadoPruebaLuscher.aspx";
        private const string SOCIAL_REPORTE_BATERIABULLYING = "~/PortalAlumno/Reportes/ReporteAlumnoResultadoBullying.aspx";

        private const string SOCIAL_PRUEBAS = "~/PortalAlumno/Pruebas.aspx";

        private const string ValidarBateriaBullying = "~/Auth/ValidarBateriaBullying.aspx";

		private const string REACTIVOS = "~/Reactivos/BuscarReactivos.aspx";
		private const string MURO_REACTIVO = "~/Reactivos/MuroReactivo.aspx?id={0}";

        //Visualizar eje Tematico
        private const string EJE_ACTIVIDAD = "~/ContenidosDigitales/VerContenidoDigital.aspx?src=eje&ejeid={0}&contid={1}";


		private const string RESOLVER_REACTIVO = "~/Reactivos/ResolverReactivo.aspx";
		private const string SUGERENCIAS_REACTIVO = "~/Reactivos/Sugerencias.aspx";
		private const string SUSCRIBIR_REACTIVO = "~/Reactivos/Suscribir.aspx?id={0}";
		private const string ValidarDiagnostica = "~/Auth/ValidarDiagnostica.aspx";

		private const string ACEPTAR_TERMINOS = "~/CuentaUsuario/AceptarTerminos.aspx";
		private const string Diagnostica = "~/Auth/PruebaDiagnostica.aspx";
		private const string ConfirmarAlumno = "~/CuentaUsuario/ConfirmarAlumno.aspx";
		private const string ConfirmarMaestro = "~/CuentaUsuario/ConfirmarMaestro.aspx";

		private const string ACTIVIDADES_ALUMNOS = "~/AptitudesSobresalientes/RealizarActividadesUI.aspx";
        private const string ACTIVIDADES_DOCENTES_ALUMNOS = "~/AptitudesSobresalientes/RealizarActividadesDocenteUI.aspx";
		private const string MANEJADOR_TAREAS = "~/AptitudesSobresalientes/ManejadorTareasUI.aspx?accion={0}";
        private const string REGISTRAR_NUEVO_ASPIRANTE = "/Aspirantes/NuevoAspirante.aspx";
        private const string Blog = "/PortalAlumno/ViewBlog.aspx";
        private const string CARRERAS_EVENTOS_UNIVERSIDAD = "~/PortalAlumno/CarrerasEventosUniversidadUI.aspx";

        private const string ACTIVAR_USUARIO = "~/CuentaUsuario/ActivarUsuario.aspx";

		public static string GetDiagnosticaURL()
		{

			return Diagnostica;
		}
		public static string GetLoginURL()
		{
			return Path(LOGIN);
		}

        public static string GetLoginURLOrientador()
        {
            return Path(LOGINORIENTADOR);
        }

        public static string GetLoginURLAspirante()
        {
            return Path(LOGINASPIRANTE);
        }

		public static string GetValidarDiagnosticaURL()
		{
			return Path(ValidarDiagnostica);
		}
		public static string GetConfirmarAlumnoURL()
		{
			return Path(ConfirmarAlumno);
		}
		public static string GetConfirmarMaestroURL()
		{
			return Path(ConfirmarMaestro);
		}
		public static string GetLogoutURL()
		{
			return Path(LOGOUT);
		}
		public static string Get404URL()
		{
			return Path(PAGE404);
		}
		public static string GetAceptarTerminosUrl()
		{
			return Path(ACEPTAR_TERMINOS);
		}
		public static string GetEditarPassURL()
		{
			return Path(EDITAR_PASS);
		}

		public static string GetImagenPerfilURL(string tipo, long usocialID)
		{
			Random rand = new Random((int) DateTime.Now.Ticks);
			return Path(IMAGEN_PERFIL, rand.Next(1, 1000), tipo, usocialID);
		}
		#region inicio
		public static string GetDefaultURL()
		{
			return Path(DEFAULT);
		}
		#endregion

		#region portal alumno
		public static string GetAlumnoNoticiasURL()
		{
			return Path(ALUMNO_NOTICIAS);
		}
		public static string GetAlumnoMuroURL(long usocialID)
		{
			return Path(ALUMNO_MURO, usocialID);
		}
		public static string GetAlumnoReactivoURL(long usocialID)
		{
			return Path(ALUMNO_REACTIVOS, usocialID);
		}
		public static string GetGrupoNoticiasDocentesURL()
		{
			return Path(GRUPO_NOTICIAS_DOCENTES);
		}
		public static string GetGrupoNoticiasMiDocenteURL(long usocialID)
		{
			return Path(GRUPO_NOTICIAS_MIDOCENTE, usocialID);
		}
        public static string GetGrupoListaURL()
        {
            return Path(GRUPO_LISTA);
        }

        public static string GetCarrerasEventosUniversidadURL()
        {
            return Path(CARRERAS_EVENTOS_UNIVERSIDAD);
        }

        
		#endregion

		#region portal docente
		public static string GetDocenteInicioURL()
		{
			return Path(DOCENTE_INICIO);
		}
		public static string GetDocenteCambiarEscuelaURL()
		{
			return Path(DOCENTE_CAMBIAR_ESCUELA);
		}
		public static string GetDocenteGrupoMuroURL(long grupoID)
		{
			return Path(DOCENTE_GRUPO_MURO, grupoID);
		}
		public static string GetDocenteGrupoNoticiasURL(long grupoID)
		{
			return Path(DOCENTE_GRUPO_NOTICIAS, grupoID);
		}
		public static string GetDocenteGrupoAlumnosURL(long grupoID)
		{
			return Path(DOCENTE_ALUMNOS, grupoID);
		}
        public static string GetCalendarOrientadorURL()
        {
            return Path(ORIENTADOR_CALENDAR);
        }
		public static string GetDocenteGrupoReportesURL(long grupoID)
		{
			return Path(DOCENTE_REPORTES, grupoID);
		}
		#endregion

		#region social
		public static string GetNotificacionesURL()
		{
			return Path(SOCIAL_NOTIFICACIONES);
		}
		public static string GetVerPublicacionURL(string notificacionID)
		{
			return Path(SOCIAL_VER_PUBLICACION, notificacionID);
		}
		public static string GetMensajesURL()
		{
			return Path(SOCIAL_MENSAJES);
		}
		public static string GetPerfilURL(long usocialID)
		{
			return Path(SOCIAL_PERFIL, usocialID);
		}
		public static string GetEditarPerfilURL()
		{
			return Path(SOCIAL_EDITAR_PERFIL);
		}
		public static string GetReportesAbusoURL()
		{
			return Path(SOCIAL_REPORTES_ABUSO);
		}
		public static string GetVerReporteAbusoURL()
		{
			return Path(SOCIAL_VER_REPORTE_ABUSO);
		}

        public static string GetEstadoProcesoURL()
        {
            return Path(SOCIAL_ESTADO_PROCESO);
        }
        public static string GetMisInteresesURL()
        {
            return Path(SOCIAL_MIS_INTERESES);
        }

        public static string GetSeleccionarCarreraURL()
        {
            return Path(SELECCIONAR_CARRERA);
        }

        public static string GetExpedienteURL()
        {
            return Path(SOCIAL_EXPEDIENTE);
        }

        public static string GetReporteURL()
        {
            return Path(SOCIAL_REPORTE);
        }

        public static string GetReporteHabitosURL()
        {
            return Path(SOCIAL_REPORTE_HABITOS);
        }

        public static string GetReporteDominosURL()
        {
            return Path(SOCIAL_REPORTE_DOMINOS);
        }

        public static string GetReporteTermanURL()
        {
            return Path(SOCIAL_REPORTE_TERMAN);
        }

        public static string GetReporteKuderURL()
        {
            return Path(SOCIAL_REPORTE_KUDER);
        }

        public static string GetREporteSACKSURL()
        {
            return Path(SOCIAL_REPORTE_SACKS);
        }

        public static string GetReporteCleaverURL()
        {
            return Path(SOCIAL_REPORTE_CLEAVER);
        }

        public static string GetReporteAllportURL()
        {
            return Path(SOCIAL_REPORTE_ALLPORT);
        }

        public static string GetReporteChasideURL()
        {
            return Path(SOCIAL_REPORTE_CHASIDE);
        }

        public static string GetReporteRotterURL() 
        {
            return Path(SOCIAL_REPORTE_ROTTER);
        }

        public static string GetReporteRavenURL()
        {
            return Path(SOCIAL_REPORTE_RAVEN);
        }

        public static string GetReporteFrasesVocacionalesURL()
        {
            return Path(SOCIAL_REPORTE_FRASES);
        }
        public static string GetReporteZavicURL()
        {
            return Path(SOCIAL_REPORTE_ZAVIC);
        }
        public static string GetReporteEstilosDeAprendizajeURL()
        {
            return Path(SOCIAL_REPORTE_ESTILOS);
        }

        public static string GetReporteInteligenciasMultiplesURL()
        {
            return Path(SOCIAL_REPORTE_INTMUL);
        }

        public static string GetReporteInventarioDeInteresesURL() 
        {
            return Path(SOCIAL_REPORTE_INVINT);
        }

        public static string GetReporteInventarioHerreraURL() 
        {
            return Path(SOCIAL_REPORTE_INVHERRERA);
        }

        public static string GetReporteSucesosdeVidaURL()
        {
            return Path(SOCIAL_REPORTE_SUCESOS);
        }
        public static string GetReportePruebaLuscherURL()
        {
            return Path(SOCIAL_REPORTE_LUSCHER);
        }
        public static string GetPruebasURL()
        {
            return Path(SOCIAL_PRUEBAS);
        }

        public static string GetActivarUsuarioURL()
        {
            return Path(ACTIVAR_USUARIO);
        }

        public static string GetReporteBateriaBullyingURL()
        {
            return Path(SOCIAL_REPORTE_BATERIABULLYING);
        }

        public static string GetValidarBateriaBullyingURL()
        {
            return Path(ValidarBateriaBullying);
        }
        #endregion

		#region reactivos
		public static string GetReactivosURL()
		{
			return Path(REACTIVOS);
		}
		public static string GetMuroReactivoURL(string reactivoID)
		{
			return Path(MURO_REACTIVO, reactivoID);
		}
		public static string GetSuscribirURL(string reactivoID)
		{
			return Path(SUSCRIBIR_REACTIVO, reactivoID);
		}
		public static string GetResolverReactivoURL()
		{
			return Path(RESOLVER_REACTIVO);
		}
		public static string GetSugerenciasURL()
		{
			return Path(SUGERENCIAS_REACTIVO);
		}
		#endregion

		#region grupo alumno

		#endregion

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

		#region Aptitudes Sobresalientes
		public static string GetActividadesAlumnoURL()
		{
			return Path(ACTIVIDADES_ALUMNOS);
		}

        public static string GetActividadesDocenteURL()
        {
            return Path(ACTIVIDADES_DOCENTES_ALUMNOS);
        }

        public static string GetTareaEjeTematicoURL(string ejeID, string contID)
        {
            return Path(EJE_ACTIVIDAD, ejeID, contID);
        }

        public static string GetRegistrarNuevoAspiranteURL()
        {
            return Path(REGISTRAR_NUEVO_ASPIRANTE);
        }

        public static string GetBlogURL()
        {
            return Path(Blog);
        }
		
		#endregion		
	}
}
