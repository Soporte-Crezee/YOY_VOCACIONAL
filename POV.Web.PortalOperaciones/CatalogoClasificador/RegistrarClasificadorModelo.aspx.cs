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
using POV.Core.Operaciones;
using POV.Core.Operaciones.Interfaces;
using POV.Core.Operaciones.Implements;
using POV.Modelo.BO;
using POV.Modelo.Service;
using POV.Modelo.Estandarizado.BO;
using POV.Comun.BO;
using POV.Operaciones.Service;
using System.Configuration;

namespace POV.Web.PortalOperaciones.CatalogoClasificador
{
    public partial class RegistrarClasificadorModelo : System.Web.UI.Page
    {
        #region Propiedades
        private IDataContext dctx = ConnectionHlp.Default.Connection;
        private ModeloCtrl modeloCtrl;

        AModelo LastObject
        {
            get { return Session["lastModelo"] != null ? Session["lastModelo"] as ModeloDinamico : null; }
            set { Session["lastModelo"] = value; }
        }

        private List<PropiedadClasificador> ListaPropiedades
        {
            get { return Session["ListaPropiedades"] != null ? Session["ListaPropiedades"] as List<PropiedadClasificador> : null; }
            set { Session["ListaPropiedades"] = value; }
        }

        public RegistrarClasificadorModelo()
        {
            modeloCtrl = new ModeloCtrl();
        }
        #endregion

        #region *****> Eventos de Pagina <*****
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ListaPropiedades = new List<PropiedadClasificador>();                
                AgregarPropiedades();
            }

        }

        protected void BtnGuardar_OnClick(object sender, EventArgs e)
        {
            string sError = ValidateClasificador();
            if (string.IsNullOrEmpty(sError))
            {
                sError = ValidarPropiedadClasificador();
                if (string.IsNullOrEmpty(sError))
                {
                    GuardarDatosClasificador();
                }
                else
                {
                    txtRedirect.Value = "";
                    ShowMessage("Errores en los campos: " + sError.Substring(2), MessageType.Error);
                }
            }
            else
            {
                txtRedirect.Value = "";
                ShowMessage("Errores en los campos: " + sError.Substring(2), MessageType.Error);
            }

        }

        protected void BtnCancelar_OnClick(object sender, EventArgs e)
        {
            ListaPropiedades = null;
            Response.Redirect("BuscarClasificadorModelo.aspx", true);
        }

        protected void RptPropiedades_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

        }
        #endregion

        #region *****> Métodos Auxiliares <*****
        private void AgregarPropiedades()
        {
            PropiedadClasificador propiedadClas = new PropiedadClasificador();

            List<PropiedadPersonalizada> ListaPropiedadesProp = new List<PropiedadPersonalizada>();
            ListaPropiedadesProp = modeloCtrl.RetrieveListaPropiedadesModeloDinamico(dctx, LastObject as ModeloDinamico);

            foreach (PropiedadPersonalizada propiedad in ListaPropiedadesProp)
            {
                propiedadClas = GetPropiedadFromUI();
                propiedadClas.Propiedad = new PropiedadPersonalizada() { PropiedadID = propiedad.PropiedadID};
                propiedadClas.Descripcion = propiedad.Nombre;
                ListaPropiedades.Add(propiedadClas);
            }
                        
            LoadListaPropiedades(ListaPropiedades);
        }

        private PropiedadClasificador GetPropiedadFromUI()
        {
            PropiedadClasificador propiedad = new PropiedadClasificador();
            propiedad.Activo = true;
            propiedad.FechaRegistro = DateTime.Now;

            return propiedad;
        }

        private void LoadListaPropiedades(List<PropiedadClasificador> propiedades)
        {
            DataTable dtPropiedades = new DataTable();

            dtPropiedades.Columns.Add("propiedadID", typeof(string));
            dtPropiedades.Columns.Add("nombrePropiedad", typeof(string));
            dtPropiedades.Columns.Add("descripcion", typeof(string));
            
            foreach (PropiedadClasificador propiedad in propiedades)
            {                
                DataRow dr = dtPropiedades.NewRow();
                dr[0] = propiedad.Propiedad.PropiedadID;
                dr[1] = propiedad.Descripcion;                                
                dtPropiedades.Rows.Add(dr);
            }
            
            RptPropiedades.DataSource = dtPropiedades;
            RptPropiedades.DataBind();
            RptPropiedades.Visible = true;
        }

        private Clasificador GetClasificadorModeloFromUI()
        {
            Clasificador clasificador = new Clasificador();
            clasificador.Nombre = txtNombre.Text.Trim();
            clasificador.Descripcion = txtDescripcion.Text.Trim();
            clasificador.FechaRegistro = DateTime.Now;
            clasificador.Activo = true;

            return clasificador;
        }

        private void GuardarDatosClasificador()
        {
            try
            {
                Clasificador clasificador = GetClasificadorModeloFromUI();
                modeloCtrl.InsertClasificador(dctx, LastObject as ModeloDinamico, clasificador);

                Clasificador classi = new Clasificador();
                classi = modeloCtrl.LastDataRowToClasificador(modeloCtrl.RetrieveClasificador(dctx, clasificador, LastObject as ModeloDinamico));
                foreach (PropiedadClasificador propiedad in ListaPropiedades)
                {
                    modeloCtrl.InsertPropiedadClasificador(dctx, classi, propiedad);
                }

                ListaPropiedades = null;

                txtRedirect.Value = "BuscarClasificadorModelo.aspx";
                ShowMessage("Registro exitoso.", MessageType.Information);
            }
            catch (Exception ex)
            {
                txtRedirect.Value = "";
                ShowMessage("Error: " + ex.Message, MessageType.Error);
            }
        }
        #endregion

        #region *** validaciones ***
        private string ValidateClasificador()
        {
            string sError = string.Empty;

            if (string.IsNullOrEmpty(txtNombre.Text.Trim()))
                sError = ", Nombre del Clasificador requerido";

            if (sError.Length > 0)
                return sError;

            if (txtNombre.Text.Trim().Length > 200)
                sError += ", Nombre de la Propiedad excede 200 caracteres ";

            if (txtDescripcion.Text.Trim().Length > 1000)
                sError += ", Descripción de la Propiedad excede 1000 caracteres ";

            return sError;
        }

        private string ValidarPropiedadClasificador()
        {
            string sError = string.Empty;            
            
            foreach (RepeaterItem itemEquipment in RptPropiedades.Items)
            {
                TextBox textBox = (TextBox)itemEquipment.FindControl("txtDescripcionPropiedad");
                if (string.IsNullOrEmpty(textBox.Text.Trim()))
                    sError = "";
                else
                {
                    if (textBox.Text.Trim().Length > 1000)
                        sError += ", Descripción en Propiedad excede 1000 caracteres";
                    else
                    {
                        int i = ListaPropiedades.FindIndex(x => x.Propiedad.PropiedadID == int.Parse(((Label)itemEquipment.FindControl("lblPropiedadID")).Text));
                        ListaPropiedades[i].Descripcion = textBox.Text.Trim();
                    }                    
                }
            }
            
            return sError;
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
       
    }
}