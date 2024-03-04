using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Framework.Base.DataAccess;
using POV.Seguridad.BO;
using POV.Seguridad.Service;
using POV.Core.RedSocial.Interfaces;
using POV.Core.RedSocial.Implement;
using POV.CentroEducativo.BO;
using GP.SocialEngine.BO;
using GP.SocialEngine.Service;
using POV.CentroEducativo.Service;
using POV.Licencias.Service;
using POV.Licencias.BO;
using POV.Web.PortalSocial.AppCode;

namespace POV.Web.PortalSocial
{
    public partial class SeleccionarEscuela : System.Web.UI.Page
    {
        #region Members
        private LicenciaEscuelaCtrl licenciaEscuelaCtrl;
        private DocenteCtrl docenteCtrl;
        private EscuelaCtrl escuelaCtrl;
        private UsuarioCtrl usuarioCtrl;
        private SocialHubCtrl socialHubCtrl;
        private IUserSession userSession;
        private IRedirector redirector;
        private UsuarioSocialCtrl usuarioSocialCtrl;
        private GrupoSocialCtrl grupoSocialCtrl;
        private CicloEscolarCtrl cicloEscolarCtrl;
        private readonly IDataContext dctx = ConnectionHelper.Default.Connection;
        private ContratoCtrl contratoCtrl;
        #endregion

        public SeleccionarEscuela()
        {
            licenciaEscuelaCtrl = new LicenciaEscuelaCtrl();
            userSession = new UserSession();
            redirector = new Redirector();
            socialHubCtrl = new SocialHubCtrl();
            grupoSocialCtrl = new GrupoSocialCtrl();
            usuarioSocialCtrl = new UsuarioSocialCtrl();
            usuarioCtrl = new UsuarioCtrl();
            docenteCtrl = new DocenteCtrl();
            escuelaCtrl = new EscuelaCtrl();
            cicloEscolarCtrl = new CicloEscolarCtrl();
            contratoCtrl = new ContratoCtrl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.HplLogout.NavigateUrl = UrlHelper.GetLogoutURL();

                if (userSession.IsLogin() && !userSession.IsAlumno() && userSession.LicenciasDocente != null && userSession.LicenciasDocente.Count > 0)
                {
                    if (userSession.LicenciasDocente.Count == 1)
                    {
                        SeleccionarEscuelaActual(userSession.LicenciasDocente.First());
                        return;
                    }
                    List<EscuelaListItem> items = new List<EscuelaListItem>();

                    Dictionary<string, string> escuelas = new Dictionary<string,string>();

                    foreach (LicenciaEscuela licenciaEscuela in userSession.LicenciasDocente)
                    {
                        EscuelaListItem item = new EscuelaListItem();
                        Escuela escuela = escuelaCtrl.LastDataRowToEscuela(escuelaCtrl.Retrieve(dctx, licenciaEscuela.Escuela));
                        item.Escuela = escuela.NombreEscuela;
                        item.Turno = escuela.Turno.ToString();
                        item.LicenciaID = licenciaEscuela.LicenciaEscuelaID.ToString();

                        items.Add(item);
                    }

                    this.RptEscuelas.DataSource = items;
                    this.RptEscuelas.DataBind();
                    
                }
                else
                    redirector.GoToLoginPage(true);
            }
        }

        protected void Escuelas_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "SELECT_LICENCIA")
            {
                long licenciaEscuelaID;

                if (long.TryParse(e.CommandArgument.ToString(), out licenciaEscuelaID))
                {
                    LicenciaEscuela licenciaEscuela = userSession.LicenciasDocente.First(item => item.LicenciaEscuelaID == licenciaEscuelaID);

                    SeleccionarEscuelaActual(licenciaEscuela);
                }
                
            }
        }

        /// <summary>
        /// Selecciona una escuela para iniciar en el portal social
        /// </summary>
        /// <param name="licenciaEscuela"></param>
        private void SeleccionarEscuelaActual(LicenciaEscuela licenciaEscuela)
        {
            #region privilegios de usuario en sesion
            UsuarioPrivilegiosCtrl usuarioPrivilegiosCtrl = new UsuarioPrivilegiosCtrl();

            UsuarioPrivilegios usuarioPrivilegios = new UsuarioEscolarPrivilegios();
            usuarioPrivilegios.Usuario = userSession.CurrentUser;
            (usuarioPrivilegios as UsuarioEscolarPrivilegios).Escuela = licenciaEscuela.Escuela;
            (usuarioPrivilegios as UsuarioEscolarPrivilegios).CicloEscolar = licenciaEscuela.CicloEscolar;

            usuarioPrivilegios = usuarioPrivilegiosCtrl.RetrieveComplete(dctx, usuarioPrivilegios);


            userSession.UsuarioPrivilegios = usuarioPrivilegios;

            #endregion

            LicenciaDocente licenciaDocente = (LicenciaDocente)licenciaEscuela.ListaLicencia.First(item => item.Tipo == ETipoLicencia.DOCENTE);
            LblErrorEscuela.Text = "";
            UsuarioSocial usuarioSocial = usuarioSocialCtrl.LastDataRowToUsuarioSocial(usuarioSocialCtrl.Retrieve(dctx, licenciaDocente.UsuarioSocial));
            DataSet dsSocialHub = socialHubCtrl.RetrieveSocialHubUsuario(dctx, new SocialHub {
                SocialProfileType = ESocialProfileType.USUARIOSOCIAL, 
                SocialProfile = new UsuarioSocial {
                    UsuarioSocialID = usuarioSocial.UsuarioSocialID
                } });
            GrupoCicloEscolarCtrl grupoCicloEscolarCtrl = new GrupoCicloEscolarCtrl();

            List<Materia> materias = grupoCicloEscolarCtrl.RetrieveMateriasDocente(dctx, licenciaDocente.Docente, new GrupoCicloEscolar { Escuela = licenciaEscuela.Escuela, CicloEscolar = licenciaEscuela.CicloEscolar });
            if (dsSocialHub.Tables[0].Rows.Count > 0 && materias.Count > 0)
            {
                CicloContratoCtrl cicloContratoCtrl = new CicloContratoCtrl();
                CicloContrato cicloContrato = cicloContratoCtrl.RetrieveComplete(dctx, licenciaEscuela.Contrato, new CicloContrato { CicloEscolar = licenciaEscuela.CicloEscolar });

                List<PruebaContrato> listaPruebas = cicloContrato.RecursoContrato.PruebasContrato.ToList();
                //seleccion de la prueba asignada al contrato
                //Remover esta consulta cuando se integre con el motor de pruebas
                if (listaPruebas.Count > 0)
                {
                    PruebaContrato pruebaFelderPivote = listaPruebas.FirstOrDefault(item => item.TipoPruebaContrato == ETipoPruebaContrato.Pivote && item.Activo.Value);
                    if (pruebaFelderPivote != null)
                        userSession.PruebaAsignada = pruebaFelderPivote.Prueba;
                    else
                        userSession.PruebaAsignada = null;
                }
                userSession.CurrentDocente = docenteCtrl.LastDataRowToDocente(docenteCtrl.Retrieve(dctx, licenciaDocente.Docente));
                userSession.CurrentEscuela = escuelaCtrl.LastDataRowToEscuela(escuelaCtrl.Retrieve(dctx, licenciaEscuela.Escuela));
                userSession.CurrentUsuarioSocial = usuarioSocial;
                userSession.CurrentCicloEscolar = cicloEscolarCtrl.LastDataRowToCicloEscolar(cicloEscolarCtrl.Retrieve(dctx, licenciaEscuela.CicloEscolar));
                userSession.SocialHub = socialHubCtrl.LastDataRowToSocialHub(dsSocialHub);
                userSession.ModulosFuncionales = licenciaEscuela.ModulosFuncionales;

                redirector.GoToHomePage(true);
            }
            else
            {
                userSession.SocialHub = null;
                 userSession.CurrentDocente = null;
                userSession.CurrentEscuela = null;
                userSession.CurrentUsuarioSocial = null;
                userSession.CurrentCicloEscolar = null;
                userSession.PruebaAsignada = null;
                userSession.ModulosFuncionales = null;
                LblErrorEscuela.Text = "ACCESO INCORRECTO: No tienes grupos asignados en la escuela seleccionada.";
            }

            
        }

        protected void Escuelas_DataBound(object sender, RepeaterItemEventArgs e)
        {
            if (this.RptEscuelas.Items.Count < 1)
            {
                if (e.Item.ItemType == ListItemType.Footer)
                {
                    Label lblFooter = (Label)e.Item.FindControl("LblSinEscuelas");
                    lblFooter.Visible = true;
                }
            }
        }
    }
}