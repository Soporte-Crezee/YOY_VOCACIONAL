using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GP.SocialEngine.BO;
using GP.SocialEngine.Service;
using POV.Core.RedSocial.Interfaces;
using POV.Core.RedSocial.Implement;
using POV.Web.PortalSocial.AppCode;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using System.Data;
using POV.Expediente.BO;
using POV.Expediente.Service;
using POV.CentroEducativo.BO;
using POV.Licencias.BO;
using POV.Licencias.Service;
using System.Collections;
using POV.Prueba.Diagnostico.Dinamica.BO;
using POV.CentroEducativo.Services;

namespace POV.Web.PortalSocial.Social
{
    public partial class MisIntereses : System.Web.UI.Page
    {
        
        private IUserSession userSession;
        private IRedirector redirector;
        private ExpedienteEscolarCtrl expedienteEscolarCtrl;
        private readonly IDataContext dctx = ConnectionHelper.Default.Connection;

        #region *** propiedades de clase ***
        public DataSet DsIntereses
        {
            set { Session["dsIntereses"] = value; }
            get { return Session["dsIntereses"] != null ? Session["dsIntereses"] as DataSet : null; }
        }

        private string GridViewSortDirection
        {
            get { return ViewState["SortDirection"] as string ?? "DESC"; }
            set { ViewState["SortDirection"] = value; }
        }
        #endregion

        public MisIntereses()
        {
            userSession = new UserSession();
            redirector = new Redirector();
            expedienteEscolarCtrl = new ExpedienteEscolarCtrl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (userSession.IsLogin())
                {
                    EFAlumnoCtrl alumnoCtrl = new EFAlumnoCtrl(null);
                    Alumno alumno = alumnoCtrl.Retrieve(new Alumno { AlumnoID = userSession.CurrentAlumno.AlumnoID }, false).FirstOrDefault();
                        
                    if ((bool)alumno.CorreoConfirmado)
                        CargarDatos();
                    else
                        redirector.GoToHomeAlumno(true);
                }
                else
                    redirector.GoToLoginPage(true);
            }
        }

        private void CargarDatos()
        {
            UsuarioSocial usuarioSocial = userSession.CurrentUsuarioSocial;
            
            #region ***LLenar información de usuario***
            this.LblNombreUsuario.Text = usuarioSocial.ScreenName;           
            this.ImgUser.ImageUrl = UrlHelper.GetImagenPerfilURL("normal",(long)  usuarioSocial.UsuarioSocialID);
            #endregion

            ArrayList arrAreaConocimiento = new ArrayList();
            Alumno alumno = userSession.CurrentAlumno;
            GrupoCicloEscolar grupoCicloEscolar = userSession.CurrentGrupoCicloEscolar;
            Contrato contrato = userSession.Contrato;

            ContratoCtrl contratoCtrl = new ContratoCtrl();
            List<PruebaContrato> pruebas = contratoCtrl.RetrievePruebasAsignadoContrato(dctx, contrato, new CicloContrato { CicloEscolar = grupoCicloEscolar.CicloEscolar });
            PruebaContrato pruebaPivoteContrato = pruebas.FirstOrDefault(item => item.TipoPruebaContrato == ETipoPruebaContrato.Pivote);

            var pruebaDinamica = new PruebaDinamica(); //(PruebaDinamica)pruebaPivoteContrato.Prueba;

            List<InteresAspirante> interesesAspirante = expedienteEscolarCtrl.RetrieveInteresesAspirante(dctx, alumno, pruebaDinamica).Distinct().ToList();

            divIntereses.Controls.Add(new LiteralControl("<div class='row' >"));
                for (var interes = 0; interes < interesesAspirante.Count; interes++)
                {
                    if(interes > 0 && interes % 3 == 0)divIntereses.Controls.Add(new LiteralControl("</div> <div class='row' >"));    
                    divIntereses.Controls.Add
                        (new LiteralControl(@"
                            <div class='col-xs-12 col-md-4 espacio_opcion' >
                                <div class='card card-block box_shadow'>
                                    <h4 class='card-title'>" + interesesAspirante[interes].NombreInteres + @"</h4>
                                </div>
                            </div>"
                        )
                    );
                }
            divIntereses.Controls.Add(new LiteralControl("</div>"));
        }

        private DataSet ConfigureGridResults(DataSet ds)
        {
            if (!ds.Tables[0].Columns.Contains("NombreInteres"))
                ds.Tables[0].Columns.Add("NombreInteres");
            List<InteresAspirante> modelos = new List<InteresAspirante>();

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                InteresAspirante ar = modelos.SingleOrDefault(a => a.InteresID == int.Parse(row["InteresID"].ToString()));
                if (ar != null)
                {
                    row["NombreInteres"] = ar.NombreInteres;
                }
            }
            return ds;
        }

        private void LoadIntereses(DataSet dsIntereses)
        {
            grdIntereses.DataSource = ConfigureGridResults(dsIntereses);
            grdIntereses.DataBind();
        }

        protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName.Trim())
            {
                case "Sort":
                    {
                        break;
                    }
                default: { ShowMessage("Comando no encontrado", MessageType.Error); break; }
            }
        }

        protected void grd_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataSet dataSet = DsIntereses;
            if (dataSet != null)
            {
                DataView dataView = new DataView(dataSet.Tables[0]);
                dataView.Sort = e.SortExpression + " " + GetSortDirection();
                DsIntereses.Tables.Clear();
                DsIntereses.Tables.Add(dataView.ToTable());
                grdIntereses.DataSource = DsIntereses;
                grdIntereses.DataBind();
            }
        }

        private string GetSortDirection()
        {
            switch (GridViewSortDirection)
            {
                case "ASC":
                    GridViewSortDirection = "DESC";
                    break;

                case "DESC":
                    GridViewSortDirection = "ASC";
                    break;
            }

            return GridViewSortDirection;
        }


        #region *****Message  Showing*****
        /// <summary>
        /// Desplega el mensaje de error/advertencia/información en la UI
        /// </summary>
        /// <param name="message">Mensaje a desplegar</param>
        /// <param name="messageType">Tipo de mensaje</param>
        private void ShowMessage(string message, MessageType messageType)
        {
            string type = string.Empty;

            switch (messageType)
            {
                case MessageType.Error:
                    type = "1";
                    break;
                case MessageType.Information:
                    type = "3";
                    break;
                case MessageType.Warning:
                    type = "2";
                    break;
            }

            ShowMessage(message, type);
        }
        /// <summary>
        /// Desplega el mensaje de error/advertencia/información en la UI
        /// </summary>
        /// <param name="message">Mensaje a desplegar</param>
        /// <param name="typeNotification">1: Error, 2: Advertencia, 3: Información</param>
        private void ShowMessage(string message, string typeNotification)
        {
            //Se ubican los controles que manejan el desplegado de error/advertencia/información
            if (Page.Master == null) return;
            Control m = Page.Master.FindControl("hdnLastMessage");
            Control t = Page.Master.FindControl("hdnShowMessage");

            if (m == null)
                throw new Exception("No se pudo desplegar correctamente el error.\nNo se encontró el control 'hdnLastMessage' en la MasterPage.\nEl error original es:\n" + message);
            if (t == null)
                throw new Exception("No se pudo desplegar correctamente el error.\nNo se encontró el control 'hdnShowMessage' en la MasterPage.\nEl error original es:\n" + message);

            if (m.GetType() != typeof(HiddenField) || t.GetType() != typeof(HiddenField))
                throw new Exception("No se pudo desplegar correctamente el error.\nAlguno de los controles de la MasterPage para el manejo de errores no es HiddenField.\nEl error original es:\n" + message);

            //Si el HiddenField del mensaje de error ya tiene un mensaje guardado, se da un 'enter' y se concatena el nuevo mensaje (errores acumulados)
            //En caso contrario, se pone el encabezado y se concatena el nuevo mensaje
            if (((HiddenField)m).Value != null && ((HiddenField)m).Value.Trim().CompareTo("") != 0)
                ((HiddenField)m).Value += "<br />";


            ((HiddenField)m).Value += message.Replace("\n", "<br />");
            ((HiddenField)t).Value = typeNotification;
        }
        #endregion
    }
}