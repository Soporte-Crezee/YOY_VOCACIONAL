using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Framework.Base.DataAccess;
using POV.Web.PortalOperaciones.Helper;
using POV.Profesionalizacion.Service;
using POV.ContenidosDigital.Service;
using Framework.Base.Exceptions;
using POV.Web.PortalOperaciones.AppCode.Page;
using System.Data;
using POV.Profesionalizacion.BO;
using POV.ContenidosDigital.BO;
using POV.Seguridad.BO;

namespace POV.Web.PortalOperaciones.Profesionalizacion.CatalogoAsistencias
{
    public partial class AsignarContenidoAsistencia : PageBase
    {

        #region *** Propiedades***
        private IDataContext dctx = ConnectionHlp.Default.Connection;
        private AsistenciaCtrl asistenciaCtrl;
        private ContenidoDigitalCtrl contenidoDigitalCtrl;
        private TipoDocumentoCtrl tipoDocumentoCtrl;
        private DataTable SS_Contenidos
        {
            set { this.Session["DsContenidoDigitalAsignados"] = value; }
            get { return (DataTable)this.Session["DsContenidoDigitalAsignados"]; }
        }

        private DataSet DsContenidoDigital
        {
            set { Session["DsContenidoDigital"] = value; }
            get { return Session["DsContenidoDigital"] != null ? Session["DsContenidoDigital"] as DataSet : null; }
        }

        private Asistencia SS_Asistencia
        {
            set { Session["lastAsistencia"] = value; }
            get { return Session["lastAsistencia"] != null ? Session["lastAsistencia"] as Asistencia : null; }
        }

        #endregion

        public AsignarContenidoAsistencia()
        {
            asistenciaCtrl = new AsistenciaCtrl();
            contenidoDigitalCtrl = new ContenidoDigitalCtrl();
            tipoDocumentoCtrl = new TipoDocumentoCtrl();
        }
        #region *** Eventos de Pagina ***
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (SS_Asistencia == null)
                {
                    txtRedirect.Value = "BuscarCurso.aspx";
                    ShowMessage("No se ha seleccionado un elemento", MessageType.Error);
                    return;
                }
                LoadTiposDocumento();
                FillBack();
                DataToUserInterface();
                LoadContenidosAsignados();
            }

        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            string validateMessage = ValidateData();
            if (string.IsNullOrEmpty(validateMessage))
            {
                ContenidoDigital contenido = UserIntefaceToData();
                DsContenidoDigital = contenidoDigitalCtrl.Retrieve(dctx, contenido);
                LoadContenidosDisponibles(DsContenidoDigital);

            }
            else
            {
                ShowMessage(validateMessage.Substring(2), MessageType.Warning);
            }
        }
        protected void grdContenidos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName.Trim())
            {
                case "AsigContenidos":
                    {
                        try
                        {
                            ContenidoDigital contenidoDigital = new ContenidoDigital();
                            contenidoDigital.ContenidoDigitalID = Convert.ToInt64(e.CommandArgument.ToString());

                            AAgrupadorContenidoDigital aAgrupadorContenidoDigital = asistenciaCtrl.RetrieveComplete(dctx, SS_Asistencia);
                            aAgrupadorContenidoDigital = ((Asistencia)aAgrupadorContenidoDigital).AgrupadoresContenido[0];

                            asistenciaCtrl.InsertContenidoAsistencia(dctx, aAgrupadorContenidoDigital, contenidoDigital);
                            ShowMessage("El contenido se ha asignado con éxito", MessageType.Information);
                            LoadContenidosAsignados();

                        }
                        catch (Exception ex)
                        {
                            ShowMessage(ex.Message, MessageType.Error);
                        }
                        break;
                    }
                case "DesAsigContenidos":
                    {
                        ContenidoDigital contenidoDigital = new ContenidoDigital();
                        contenidoDigital.ContenidoDigitalID = Convert.ToInt64(e.CommandArgument.ToString());

                        AAgrupadorContenidoDigital aAgrupadorContenidoDigital = asistenciaCtrl.RetrieveComplete(dctx, SS_Asistencia);
                        aAgrupadorContenidoDigital = ((Asistencia)aAgrupadorContenidoDigital).AgrupadoresContenido[0];

                        asistenciaCtrl.DeleteContenidoAsistencia(dctx, aAgrupadorContenidoDigital, contenidoDigital);
                        ShowMessage("El contenido se ha eliminado con éxito", MessageType.Information);
                        LoadContenidosAsignados();
                        break;
                    }
                case "Sort":
                    {
                        break;
                    }
                default: { ShowMessage("Comando no encontrado", MessageType.Error); break; }
            }
        }
        #endregion
       
        #region *** Data To User Interface ***

        private void DataToUserInterface()
        {
            Asistencia asistencia = asistenciaCtrl.RetrieveComplete(dctx, new Asistencia { AgrupadorContenidoDigitalID = SS_Asistencia.AgrupadorContenidoDigitalID }) as Asistencia;

            txtID.Text = asistencia.AgrupadorContenidoDigitalID.ToString();
            txtNombre.Text = asistencia.Nombre;
            txtTema.Text = asistencia.TemaAsistencia.Nombre;

        }

        private void LoadTiposDocumento()
        {
            DataSet ds = tipoDocumentoCtrl.Retrieve(dctx, new TipoDocumento { Activo = true });
            DDLTipoDocumento.DataSource = ds;
            DDLTipoDocumento.DataValueField = "TipoDocumentoID";
            DDLTipoDocumento.DataTextField = "Nombre";
            DDLTipoDocumento.DataBind();
            DDLTipoDocumento.Items.Insert(0, new ListItem("SELECCIONE...", "-1"));
        }

        /// <summary>
        ///  Carga los datos de contenidos disponibles en el gridview de la interfaz grafica
        /// </summary>
        /// <param name="ds">DataSet con la informacion del los resultados de la busqueda</param>
        private void LoadContenidosDisponibles(DataSet ds)
        {
            grdConenidosDisponibles.DataSource = ConfigureGridResultsDisponibles(ds);
            grdConenidosDisponibles.DataBind();
        }

        private DataSet ConfigureGridResultsDisponibles(DataSet ds)
        {

            if (!ds.Tables[0].Columns.Contains("NombreTipoDocumento"))
                ds.Tables[0].Columns.Add("NombreTipoDocumento");

            List<TipoDocumento> tiposDocumento = new List<TipoDocumento>();

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                TipoDocumento ar = tiposDocumento.SingleOrDefault(a => a.TipoDocumentoID == int.Parse(row["TipoDocumentoID"].ToString()));
                if (ar != null)
                {
                    row["NombreTipoDocumento"] = ar.Nombre;
                }
                else
                {
                    TipoDocumento nivel = new TipoDocumento { TipoDocumentoID = int.Parse(row["TipoDocumentoID"].ToString()) };
                    nivel = tipoDocumentoCtrl.LastDataRowToTipoDocumento(tipoDocumentoCtrl.Retrieve(dctx, nivel));
                    row["NombreTipoDocumento"] = nivel.Nombre;
                    tiposDocumento.Add(nivel);
                }

            }
            return ds;
        }

        /// <summary>
        /// Carga los datos de contenidos asignados en el gridview de la interfaz grafica
        /// </summary>
        private void LoadContenidosAsignados()
        {
            Asistencia asistencia = asistenciaCtrl.RetrieveComplete(dctx, new Asistencia { AgrupadorContenidoDigitalID = SS_Asistencia.AgrupadorContenidoDigitalID }) as Asistencia;

            ConfigureGridResultsAsignados(asistencia.AgrupadoresContenido[0].ContenidosDigitales);
        }

        private void ConfigureGridResultsAsignados(List<ContenidoDigital> contenidos)
        {
            DataTable dtContenidosAsignados = new DataTable();
            dtContenidosAsignados.Columns.Add("ContenidoDigitalID", typeof(long));
            dtContenidosAsignados.Columns.Add("Nombre", typeof(string));
            dtContenidosAsignados.Columns.Add("InstitucionOrigen", typeof(string));
            dtContenidosAsignados.Columns.Add("TipoDocumentoNombre", typeof(string));
            if (contenidos != null)
            {
                foreach (ContenidoDigital contenidoDigital in contenidos)
                {
                    ContenidoDigital contenido = contenidoDigitalCtrl.RetrieveComplete(dctx, contenidoDigital);
                    if (contenido != null)
                    {
                        DataRow dr = dtContenidosAsignados.NewRow();
                        dr[0] = contenidoDigital.ContenidoDigitalID;
                        dr[1] = contenido.Nombre;
                        dr[2] = contenido.InstitucionOrigen.Nombre;
                        dr[3] = contenido.TipoDocumento.Nombre;
                        dtContenidosAsignados.Rows.Add(dr);
                    }
                }
            }
            SS_Contenidos = dtContenidosAsignados;
            grdConenidosAsignados.DataSource = dtContenidosAsignados;
            grdConenidosAsignados.DataBind();
        }

        #endregion
        #region *** User Interface To Data ***
        private TipoDocumento GetTipoDocumentoFromUI()
        {
            TipoDocumento tipo = null;

            int tipoDocumentoID = 0;
            string valorTipoDocumento = DDLTipoDocumento.SelectedValue;

            if (int.TryParse(valorTipoDocumento, out tipoDocumentoID))
            {
                if (tipoDocumentoID < 0)
                    tipo = new TipoDocumento();
                else
                    tipo = new TipoDocumento { TipoDocumentoID = tipoDocumentoID };
            }
            return tipo;
        }

        private ContenidoDigital UserIntefaceToData()
        {
            ContenidoDigital contenido = new ContenidoDigital();
            contenido.InstitucionOrigen = new InstitucionOrigen();

            long id = 0;
            if (long.TryParse(txtidentificador.Text.Trim(), out id))
                contenido.ContenidoDigitalID = id;

            if (txtNom.Text.Trim().Length > 0)
            {
                contenido.Nombre = txtNom.Text.Trim();
                contenido.Nombre = !string.IsNullOrEmpty(contenido.Nombre) ? string.Format("%{0}%", contenido.Nombre) : null;
            }

            if (txtInstitucion.Text.Trim().Length > 0)
            {
                contenido.InstitucionOrigen.Nombre = txtInstitucion.Text.Trim();
                contenido.InstitucionOrigen.Nombre = !string.IsNullOrEmpty(contenido.InstitucionOrigen.Nombre) ? string.Format("%{0}%", contenido.InstitucionOrigen.Nombre) : null;
            }

            if (GetTipoDocumentoFromUI().TipoDocumentoID != null)
                contenido.TipoDocumento = GetTipoDocumentoFromUI();

            return contenido;
        }
        #endregion
        #region *** validaciones ***
        public string ValidateData()
        {
            string sError = string.Empty;
            long id = 0;

            if (!string.IsNullOrEmpty(txtidentificador.Text.Trim()))
            {
                bool result = long.TryParse(txtidentificador.Text.Trim(), out id);
                if (!result)
                    sError += "El identificador debe ser numérico";
            }

            if (txtNom.Text.Trim().Length > 200)
                sError += "El nombre no debe ser mayor a 200 caracteres";

            if (txtInstitucion.Text.Trim().Length > 200)
                sError += "La institución de origen no debe ser mayor a 200 caracteres";

            //if (txtEtiqueta.Text.Trim().Length > 50)
            //    sError += "La etiqueta no debe ser mayor a 50 caracteres";

            return sError;
        }

        #endregion
        #region *** Metodos auxiliares ***

        private void FillBack()
        {
            if (HttpContext.Current.Request.UrlReferrer != null)
                lnkBack.NavigateUrl = HttpContext.Current.Request.UrlReferrer.PathAndQuery;
            else
                lnkBack.NavigateUrl = "~";
        }
        #endregion
        #region *****Message  Showing*****
        /// <summary>
        /// Desplega el mensaje de error/advertencia/información en la UI
        /// </summary>
        /// <param name="message">Mensaje a desplegar</param>
        /// <param name="typeNotification">1: Error, 2: Advertencia, 3: Información</param>
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
        #region *** AUTORIZACION DE LA PAGINA ***

        protected override void AuthorizeUser()
        {
            //ejemplificando la ejecucion
            List<Permiso> permisos = userSession.CurrentPrivilegios.GetPermisos();

            bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOASISTENCIA) != null;
            bool edit = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POEDITARASISTENCIA) != null;

            if (!acceso)
                redirector.GoToHomePage(true);
            if (!edit)
                redirector.GoToHomePage(true);
        }

        #endregion
    }
}