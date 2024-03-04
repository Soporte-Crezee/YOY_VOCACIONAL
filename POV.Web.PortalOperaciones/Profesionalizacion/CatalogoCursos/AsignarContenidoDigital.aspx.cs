using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using POV.ContenidosDigital.BO;
using POV.Profesionalizacion.BO;
using POV.Profesionalizacion.Service;
using Framework.Base.DataAccess;
using POV.Web.PortalOperaciones.AppCode.Page;
using POV.Web.PortalOperaciones.Helper;
using POV.Seguridad.BO;
using POV.ContenidosDigital.Service;
using System.Data;
using Framework.Base.Exceptions;

namespace POV.Web.PortalOperaciones.Profesionalizacion.CatalogoCursos
{
    public partial class AsignarContenidoDigital : PageBase
    {
        private IDataContext dctx = ConnectionHlp.Default.Connection;
        private CursoCtrl cursoCtrl;
        private ContenidoDigitalCtrl contenidoDigitalCtrl;
        private TipoDocumentoCtrl tipoDocumentoCtrl;

        #region *** propiedades de la clase ***

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

        private Curso SS_Curso
        {
            set { Session["lastCurso"] = value; }
            get { return Session["lastCurso"] != null ? Session["lastCurso"] as Curso : null; }
        }

        #endregion

        public AsignarContenidoDigital()
        {
            cursoCtrl = new CursoCtrl();
            contenidoDigitalCtrl = new ContenidoDigitalCtrl();
            tipoDocumentoCtrl = new TipoDocumentoCtrl();
        }

        #region *** eventos de pagina ***
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (SS_Curso == null)
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

                            AAgrupadorContenidoDigital aAgrupadorContenidoDigital = cursoCtrl.RetrieveComplete(dctx, SS_Curso);
                            aAgrupadorContenidoDigital = ((Curso)aAgrupadorContenidoDigital).AgrupadoresContenido[0];

                            cursoCtrl.InsertContenidoCurso(dctx, aAgrupadorContenidoDigital, contenidoDigital);
                            ShowMessage("El contenido digital se ha asignado con éxito", MessageType.Information);
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

                        AAgrupadorContenidoDigital aAgrupadorContenidoDigital = cursoCtrl.RetrieveComplete(dctx, SS_Curso);
                        aAgrupadorContenidoDigital = ((Curso)aAgrupadorContenidoDigital).AgrupadoresContenido[0];

                        cursoCtrl.DeleteContenidoCurso(dctx, aAgrupadorContenidoDigital, contenidoDigital);
                        ShowMessage("El contenido digital se ha eliminado con éxito", MessageType.Information);
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

            if (txtClave.Text.Trim().Length > 30)
                sError += "La clave no debe ser mayor a 30 caracteres";

            if (txtNom.Text.Trim().Length > 200)
                sError += "El nombre no debe ser mayor a 200 caracteres";

            if (txtInstitucion.Text.Trim().Length > 200)
                sError += "La institucion de origen no debe ser mayor a 200 caracteres";

           
            return sError;
        }

        #endregion

        #region *** Data To User Interface ***

        private void DataToUserInterface()
        {
            Curso curso = cursoCtrl.RetrieveComplete(dctx, new Curso { AgrupadorContenidoDigitalID = SS_Curso.AgrupadorContenidoDigitalID }) as Curso;

            txtID.Text = curso.AgrupadorContenidoDigitalID.ToString();
            txtNombre.Text = curso.Nombre;
            txtTema.Text = curso.TemaCurso.Nombre;

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
            Curso curso = cursoCtrl.RetrieveComplete(dctx, new Curso { AgrupadorContenidoDigitalID = SS_Curso.AgrupadorContenidoDigitalID }) as Curso;

            ConfigureGridResultsAsignados(curso.AgrupadoresContenido[0].ContenidosDigitales);
        }

        private void ConfigureGridResultsAsignados(List<ContenidoDigital> contenidos)
        {
            DataTable dtContenidosAsignados = new DataTable();
            dtContenidosAsignados.Columns.Add("ContenidoDigitalID", typeof(long));
            dtContenidosAsignados.Columns.Add("Clave", typeof(string));
            dtContenidosAsignados.Columns.Add("Nombre", typeof(string));
            dtContenidosAsignados.Columns.Add("InstitucionOrigen", typeof(string));
            dtContenidosAsignados.Columns.Add("TipoDocumentoNombre", typeof(string));

            foreach (ContenidoDigital contenidoDigital in contenidos)
            {
                ContenidoDigital contenido = contenidoDigitalCtrl.RetrieveComplete(dctx, contenidoDigital);
                if (contenido != null)
                {
                    DataRow dr = dtContenidosAsignados.NewRow();
                    dr[0] = contenidoDigital.ContenidoDigitalID;
                    dr[1] = contenidoDigital.Clave;
                    dr[2] = contenido.Nombre;
                    dr[3] = contenido.InstitucionOrigen.Nombre;
                    dr[4] = contenido.TipoDocumento.Nombre;
                    dtContenidosAsignados.Rows.Add(dr);
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

            if (txtClave.Text.Trim().Length > 0)
                contenido.Clave = txtClave.Text.Trim();

            if (txtNom.Text.Trim().Length > 0)
                contenido.Nombre = "%" + txtNom.Text.Trim() + "%";

            if (txtInstitucion.Text.Trim().Length > 0)
                contenido.InstitucionOrigen.Nombre = txtInstitucion.Text.Trim();

            if (GetTipoDocumentoFromUI().TipoDocumentoID != null)
                contenido.TipoDocumento = GetTipoDocumentoFromUI();

            return contenido;
        }
        #endregion

        #region *** metodos auxiliares ***

        private void FillBack()
        {
            if (HttpContext.Current.Request.UrlReferrer != null)
                lnkBack.NavigateUrl = HttpContext.Current.Request.UrlReferrer.PathAndQuery;
            else
                lnkBack.NavigateUrl = "BuscarCurso.aspx";
        }
       
        #endregion
        
        #region *****Message Showing*****
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

        #region AUTORIZACION DE LA PAGINA

        protected override void AuthorizeUser()
        {
            //ejemplificando la ejecucion
            List<Permiso> permisos = userSession.CurrentPrivilegios.GetPermisos();

            bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOCURSOS) != null;
            bool edit = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POEDITARCURSOS) != null;

            if (!acceso)
                redirector.GoToHomePage(true);
            if (!edit)
                redirector.GoToHomePage(true);
        }

        #endregion
    }
}