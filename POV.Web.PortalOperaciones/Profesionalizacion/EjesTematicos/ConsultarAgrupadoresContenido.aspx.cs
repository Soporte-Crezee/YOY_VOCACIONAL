using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Framework.Base.Exceptions;
using Framework.Base.DataAccess;
using POV.Web.PortalOperaciones.Helper;
using POV.Web.PortalOperaciones.AppCode.Page;
using POV.Seguridad.BO;
using POV.Profesionalizacion.BO;
using POV.Profesionalizacion.Service;

namespace POV.Web.PortalOperaciones.Profesionalizacion.EjesTematicos
{
    public partial class ConsultarAgrupadoresContenido : PageBase
    {
        private IDataContext dctx = ConnectionHlp.Default.Connection;

        public DataTable DtAgrupadoresSituacion
        {
            set { Session["dtAgrupadoresSituacion"] = value; }
            get { return Session["dtAgrupadoresSituacion"] != null ? Session["dtAgrupadoresSituacion"] as DataTable : null; }
        }

        private EjeTematico LastEjeTematico
        {
            set { Session["lastEjeTematico"] = value; }
            get { return Session["lastEjeTematico"] != null ? Session["lastEjeTematico"] as EjeTematico : null; }
        }

        private SituacionAprendizaje LastSituacionAprendizaje
        {
            get { return Session["lastSituacionAprendizaje"] != null ? Session["lastSituacionAprendizaje"] as SituacionAprendizaje : null; }
            set { Session["lastSituacionAprendizaje"] = value; }
        }

        private AAgrupadorContenidoDigital LastAgrupador
        {
            get { return Session["lastAgrupador"] != null ? Session["lastAgrupador"] as AAgrupadorContenidoDigital : null; }
            set { Session["lastAgrupador"] = value; }
        }

        private string GridViewSortDirection
        {
            get { return ViewState["SortDirection"] as string ?? "DESC"; }
            set { ViewState["SortDirection"] = value; }
        }

        private SituacionAprendizajeCtrl situacionAprendizajeCtrl;
        private EjeTematicoCtrl ejeTematicoCtrl;

        private AgrupadorContenidoDigitalCtrl agrupadorContenidoDigitalCtrl;

        public ConsultarAgrupadoresContenido()
        {
            situacionAprendizajeCtrl = new SituacionAprendizajeCtrl();
            agrupadorContenidoDigitalCtrl = new AgrupadorContenidoDigitalCtrl();
            ejeTematicoCtrl = new EjeTematicoCtrl();
        }

        #region *** eventos de pagina ***
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (LastEjeTematico != null && LastEjeTematico.EjeTematicoID != null
                    && LastSituacionAprendizaje != null && LastSituacionAprendizaje.SituacionAprendizajeID != null)
                {
                    LoadAgrupadoresSituacion();
                }
                else
                {
                    Response.Redirect("ConfigurarSituacionesAprendizaje.aspx", true);
                }

                
            }
        }

        protected void grdAgrupadores_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName.Trim())
            {
                case "config":
                    {
                        try
                        {
                            long id = long.Parse(e.CommandArgument.ToString());

                            SituacionAprendizaje situacionAprendizaje = situacionAprendizajeCtrl.LastDataRowToSituacionAprendizaje(situacionAprendizajeCtrl.Retrieve(dctx, LastEjeTematico, LastSituacionAprendizaje));

                            AgrupadorCompuesto agrupadorCompuesto = agrupadorContenidoDigitalCtrl.RetrieveSimple(dctx, situacionAprendizaje.AgrupadorContenidoDigital) as AgrupadorCompuesto;

                            AAgrupadorContenidoDigital agrupadorHijo = agrupadorCompuesto.AgrupadoresContenido.FirstOrDefault(item => item.AgrupadorContenidoDigitalID == id);

                            LastAgrupador = agrupadorHijo;
                            Response.Redirect("ConfigurarContenidosAgrupador.aspx", true);
                        }
                        catch (Exception ex)
                        {
                            ShowMessage(ex.Message, MessageType.Error);
                        }
                        break;
                    }
                case "Sort":
                    {
                        break;
                    }
                default: { ShowMessage("Comando no encontrado", MessageType.Error); break; }

            }
        }

        protected void grdAgrupadores_Sorting(object sender, GridViewSortEventArgs e)
        {

            if (DtAgrupadoresSituacion != null)
            {
                DataView dataView = new DataView(DtAgrupadoresSituacion);
                dataView.Sort = e.SortExpression + " " + GetSortDirection();
                DtAgrupadoresSituacion = (dataView.ToTable());
                grdAgrupadores.DataSource = DtAgrupadoresSituacion;
                grdAgrupadores.DataBind();
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

        protected void BtnCancelar_OnClick(object sender, EventArgs e)
        {
            LimpiarObjetosSesion();
            Response.Redirect("ConfigurarSituacionesAprendizaje.aspx", true);
        }
        #endregion

        #region *** validaciones ***
        //METODOS PARA VALIDACIONES
        #endregion

        #region *** Data to UserInterface ***
        private void LoadAgrupadoresSituacion()
        {
            DataTable dtAgrupadores = new DataTable();
            dtAgrupadores.Columns.Add("AgrupadorContenidoDigitalID");
            dtAgrupadores.Columns.Add("Nombre");
            dtAgrupadores.Columns.Add("Predeterminado");

            EjeTematico ejeTematico = ejeTematicoCtrl.RetrieveComplete(dctx, new EjeTematico { EjeTematicoID = LastEjeTematico.EjeTematicoID });

            SituacionAprendizaje situacionAprendizaje = situacionAprendizajeCtrl.LastDataRowToSituacionAprendizaje(situacionAprendizajeCtrl.Retrieve(dctx, LastEjeTematico, new SituacionAprendizaje { SituacionAprendizajeID = LastSituacionAprendizaje.SituacionAprendizajeID }));

            AgrupadorCompuesto agrupadorCompuesto = agrupadorContenidoDigitalCtrl.RetrieveSimple(dctx, situacionAprendizaje.AgrupadorContenidoDigital) as AgrupadorCompuesto;

            txtNivelEducativo.Text = ejeTematico.AreaProfesionalizacion.NivelEducativo.Descripcion;
            txtGrado.Text = ejeTematico.AreaProfesionalizacion.Grado.ToString() + "°";
            txtAsignatura.Text = ejeTematico.AreaProfesionalizacion.Nombre;
            txtBloque.Text = ejeTematico.MateriasProfesionalizacion.FirstOrDefault().Nombre;
            txtNombreEjeTematico.Text = ejeTematico.Nombre;
            txtNombreSituacion.Text = situacionAprendizaje.Nombre;

            foreach (AAgrupadorContenidoDigital agrupadorHijo in agrupadorCompuesto.AgrupadoresContenido)
            {
                DataRow dr = dtAgrupadores.NewRow();
                dr[0] = agrupadorHijo.AgrupadorContenidoDigitalID;
                dr[1] = agrupadorHijo.Nombre;
                dr[2] = agrupadorHijo.EsPredeterminado.Value ? "Si" : "No";

                dtAgrupadores.Rows.Add(dr);
            }
            if (dtAgrupadores.Rows.Count > 0)
            {
                DataView dv = new DataView(dtAgrupadores);
                dv.Sort = "Predeterminado DESC";
                dtAgrupadores = dv.ToTable();
            }

            DtAgrupadoresSituacion = dtAgrupadores;
            grdAgrupadores.DataSource = dtAgrupadores;
            grdAgrupadores.DataBind();
        }
        #endregion

        #region *** UserInterface to Data ***
        //METODOS DE CONVERSION DE UI A OBJETO CONCRETO
        #endregion

        #region *** metodos auxiliares ***
        private void LimpiarObjetosSesion()
        {
            LastAgrupador = null;
        }
        #endregion

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

        #region AUTORIZACION DE LA PAGINA

        protected override void AuthorizeUser()
        {
            //NOTA. EJEMEPLO DE DEFINICION
            //ejemplificando la ejecucion
            List<Permiso> permisos = userSession.CurrentPrivilegios.GetPermisos();

            bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOSITUACIONES) != null;
            bool edit = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POEDITARSITUACIONES) != null;

            if (!acceso)
                redirector.GoToHomePage(true);
            if (!edit)
                redirector.GoToHomePage(true);
        }
        #endregion
    }
}