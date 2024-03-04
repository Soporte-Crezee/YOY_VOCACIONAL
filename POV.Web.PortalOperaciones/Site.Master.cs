using System;
using System.Collections.Generic;
using System.Linq;
using POV.Seguridad.BO;
using POV.Web.PortalOperaciones.AppCode.Page;

namespace POV.Web.PortalOperaciones
{
    public partial class Site : MasterPageBase
    {

        public Site() : base()
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!userSession.IsLogin())
                {
                    redirector.GoToLoginPage(true);
                }
                else
                {
                    string usuario = userSession.CurrentUser.NombreUsuario;
                    lblAnio.Text = DateTime.Now.ToString("yyyy");
                    this.LblNombreUsuario.Text = usuario;
                }
            }
        }

        protected override void AuthorizeUser()
        {
            //ejemplificando la ejecucion
            List<Permiso> permisos = userSession.CurrentPrivilegios.GetPermisos();

            bool accesoEscuelas = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOESCUELAS) != null;
            bool accesoPruebaDiag = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOPRUEBA) != null;
            bool accesoReactivosDiag = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOREACTIVODIAGNOSTICO) != null;
            bool accesoPreguntasIni = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOPREGUNTASINICIALES) != null;
            bool accesoTipoInteligencia = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOTIPOINTELIGENCIA) != null;
            bool accesoNivelTaxonomico = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESONIVELTAXONOMICO) != null;
            bool accesoContratos = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOCONTRATO) != null;
            bool accesoUsuarios = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOUSUARIO) != null;
            bool accesoPerfiles = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOPERFILES) != null;
            bool accesoReactivosEst = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOREACTIVOESTANDARIZADO) != null;
            bool accesoNivelComplejidad = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESONIVELCOMPLEJIDAD) != null;
            bool accesoPlanesEducativos = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOPLANEDUCATIVO) != null;
            bool accesoDirectores = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESODIRECTORES) != null;
            bool accesoPaises = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOPAIS) != null;
            bool accesoEstados = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOESTADOS) != null;
            bool accesoCiudades = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOCIUDADES) != null;
            bool accesoLocalidades = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOLOCALIDADES) != null;
            bool accesoCicloEscolar = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOCICLOESCOLAR) != null;
            bool accesoCatPrueba = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOPRUEBA) != null;
            bool accesoConfigTrama = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOCONFIGURARTRAMA) != null;
            bool accesoAreaEst = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOAREASESTANDARIZADAS) != null;
            bool accesoReactivoFinal = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOREACTIVOSFINAL) != null;
            bool accesoReactivosDinamico = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOREACTIVOSDINAMICO) != null;
            bool accesoCatModelos = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOMODELOSPRUEBA) != null;
            //Modulo Aptitudes
            bool accesoTipoAptitud = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOTIPOAPTITUD) != null;
            bool accesoConfiguracionGeneral = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOCONFIGURACIONPLATAFORMA) != null;
            bool accesoTemas = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOTEMA) != null;
            LoadMenuProfesionalizacion(permisos);


            if (accesoPaises)
            {
                opcion_Comun.Visible = true;
                menu_PaisesCatalogo.Visible = true;
            }

            if (accesoEstados)
            {
                opcion_Comun.Visible = true;
                menu_EstadosCatalogo.Visible = true;
            }

            if (accesoCiudades)
            {
                opcion_Comun.Visible = true;
                menu_CiudadesCatalogo.Visible = true;
            }

            if (accesoCatPrueba)
            {
                opcion_Catalogo_Pruebas.Visible = true;

            }

            if (accesoContratos)
            {
                opcion_Contratos.Visible = true;
                opcion_CargaAlumnos.Visible = true;
                opcion_ReportesCarga.Visible = true;
                opcion_Formatos.Visible = true;
            }
        }

        private void LoadMenuProfesionalizacion(List<Permiso> permisos)
        {
            bool accesoEjes = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOEJETEMATICO) != null;
            bool accesoAreasProfesionalizacion = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOAREASPROFESIONALIZACION) != null;
            bool accesoContenidos = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOCONTENIDOSDIGITALES) != null;
            bool accesotiposDocumento = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOTIPODOCUMENTO) != null;
            bool accesoAsistencias = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOASISTENCIA) != null;
            bool accesoTemasAsistencia = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOTEMAASISTENCIA) != null;
            bool accesoCursos = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOCURSOS) != null;
            bool accesoTemasCursos = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOTEMASCURSOS) != null;
        }
    }
}