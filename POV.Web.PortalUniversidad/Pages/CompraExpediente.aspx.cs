using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Administracion.BO;
using POV.Administracion.Services;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.Comun.BO;
using POV.Comun.Service;
using POV.Licencias.BO;
using POV.Licencias.Service;
using POV.Modelo.BO;
using POV.Modelo.Service;
using POV.Prueba.Diagnostico.Dinamica.BO;
using POV.Prueba.Diagnostico.Dinamica.Service;
using POV.Seguridad.BO;
using POV.Web.PortalUniversidad.AppCode.Page;
using POV.Web.PortalUniversidad.Helper;
using POV.Web.PortalUniversidad.Logic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace POV.Web.PortalUniversidad.Pages
{
    public partial class CompraExpediente : CatalogPage
    {
        private ModeloCtrl modeloCtrl;
        private PruebaDinamicaCtrl pruebaDinamicaCtrl;
        private readonly IDataContext dctx = ConnectionHlp.Default.Connection;
        private EstadoCtrl estadoCtrl;
        private InfoAlumnoUsuarioCtrl infoAlumnoUsuarioCtrl;
        private CostoProductoCtrl costoProductoCtrl;

        #region variables de session
        private List<InfoAlumnoUsuario> Session_listaSeleccionados
        {
            get { return Session["Session_listaSeleccionados"] as List<InfoAlumnoUsuario>; }
            set { Session["Session_listaSeleccionados"] = value; }
        }

        private string Session_cantidadExpedientes
        {
            get { return Session["Session_cantidadExpedientes"] as string; }
            set { Session["Session_cantidadExpedientes"] = value; }
        }

        private string Session_costoPorExpediente
        {
            get { return Session["Session_costoPorExpediente"] as string; }
            set { Session["Session_costoPorExpediente"] = value; }
        }
        #endregion
        
        public CompraExpediente() 
        {
            modeloCtrl = new ModeloCtrl();
            pruebaDinamicaCtrl = new PruebaDinamicaCtrl();
            estadoCtrl = new EstadoCtrl();
            infoAlumnoUsuarioCtrl = new InfoAlumnoUsuarioCtrl();
            costoProductoCtrl = new CostoProductoCtrl(null);
        }

        #region Evento de la pagina
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (userSession != null && userSession.CurrentUniversidad != null && userSession.CurrentCicloEscolar != null)
                {
                    if (!IsPostBack)
                    {
                        LblNombreUsuario.Text = userSession.CurrentUser.NombreUsuario;
                        LoadAreaConocimiento();
                        LoadEstado();
                    }
                }
                else 
                {
                    redirector.GoToLoginPage(true);
                }
            }
            catch (Exception ex) 
            {
                this.ShowMessage(ex.Message, MessageType.Error);
            }
        }

        protected void btnBuscarExpedientes_Click(object sender, EventArgs e)
        {
            try
            {
                DivInfoEncontrada.Visible = true;
                
                LoadExpedientesDisponibles();
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message, MessageType.Error);
            }
        }

        protected void ComprarPaquete_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCantidadExpedintes.Text))
            {
                int cantidad= 0;
                if (int.TryParse(txtCantidadExpedintes.Text, out cantidad) && !string.IsNullOrEmpty(Session_costoPorExpediente))
                {
                    int cantidadExpedientes = int.Parse(txtCantidadExpedintes.Text);
                    int cantidadExpedientesExistencia = int.Parse(Session_cantidadExpedientes);

                    if (cantidadExpedientes > cantidadExpedientesExistencia)
                        this.ShowMessage("La cantidad no puede ser mayor a la cantidad de expedientes disponibles.", MessageType.Error);
                    else
                    {
                        if (cantidadExpedientes < 1)
                            this.ShowMessage("La cantidad no puede ser menor que 1.", MessageType.Error);
                        else
                        {
                            string total = (double.Parse(Session_costoPorExpediente) * double.Parse(txtCantidadExpedintes.Text)).ToString().Trim();

                            Session["PayPalPaymentRequestName"] = "Compra de expedientes";
                            Session["payment_amt"] = total;
                            Session["currency_code"] = PaypalSettings.PayPalCURRENCYCODE;

                            Session_cantidadExpedientes = txtCantidadExpedintes.Text.Trim();

                            Response.Redirect("~/Checkout/CheckoutStart.aspx", true);
                        }
                    }
                }
                else
                {
                    this.ShowMessage("La cantidad tiene un formato incorrecto.", MessageType.Error);
                }
            }
            else {
                this.ShowMessage("Introducir la cantidad de expedientes a comprar.", MessageType.Error);
            }
        }
        #endregion

        #region Metodos Auxiliares
        private void LoadAreaConocimiento() 
        {
            ddlSearchAreasConocimiento.DataSource = GetAreasConocimientoAlumno();
            ddlSearchAreasConocimiento.DataValueField = "ClasificadorID";
            ddlSearchAreasConocimiento.DataTextField = "Nombre";
            ddlSearchAreasConocimiento.DataBind();
            ddlSearchAreasConocimiento.Items.Insert(0, new ListItem("Seleccionar", ""));
        }

        private DataSet GetAreasConocimientoAlumno() 
        {
            ArrayList arrAreaconoimiento = new ArrayList();
            Escuela escuela = userSession.CurrentEscuela;
            CicloEscolar cicloEscolar = userSession.CurrentCicloEscolar;
            Contrato contrato = userSession.Contrato;

            ContratoCtrl contratoCtrl = new ContratoCtrl();
            List<PruebaContrato> pruebas = contratoCtrl.RetrievePruebasAsignadoContrato(dctx, contrato, new CicloContrato { CicloEscolar = cicloEscolar });
            PruebaContrato pruebaPivoteContrato = pruebas.FirstOrDefault(item => item.TipoPruebaContrato == ETipoPruebaContrato.Pivote);

            var pruebaDinamica = (PruebaDinamica)pruebaPivoteContrato.Prueba;
            PruebaDinamica pruebaDinamicaModelo = pruebaDinamicaCtrl.RetrieveComplete(dctx, pruebaDinamica, false);
            DataSet DsClasificadores = modeloCtrl.RetrieveClasificador(dctx, new Clasificador { Activo = true }, new ModeloDinamico { ModeloID = pruebaDinamicaModelo.Modelo.ModeloID });

            return DsClasificadores;
        } 

        private void LoadEstado() 
        {
            ddlSearchEstado.DataSource = GetEstado();
            ddlSearchEstado.DataTextField = "Nombre";
            ddlSearchEstado.DataValueField = "EstadoID";
            ddlSearchEstado.DataBind();
            ddlSearchEstado.Items.Insert(0, new ListItem("Seleccionar", ""));
        }
        
        private DataSet GetEstado() 
        {
            DataSet dsEstado = estadoCtrl.Retrieve(dctx, new Estado());
            return dsEstado;
        }

        private void LoadExpedientesDisponibles()
        {
            Session_cantidadExpedientes = string.Empty;
            Session_costoPorExpediente = string.Empty;
            Session_listaSeleccionados = null;

            List<InfoAlumnoUsuario> getNoExpediente = GetDataInfoExpediente().GroupBy(test => test.AlumnoID).Select(grp => grp.First()).ToList();

            List<Alumno> alumnosUniversidad = userSession.CurrentUniversidad.Alumnos.ToList();

            getNoExpediente = (from item in getNoExpediente
                               join item2 in alumnosUniversidad
                                       on item.AlumnoID equals item2.AlumnoID into g
                                       where !g.Any()
                                       select item).ToList();


            List<InfoAlumnoUsuario> selecciones = new List<InfoAlumnoUsuario>();
            foreach (InfoAlumnoUsuario item in getNoExpediente)
            {
                selecciones.Add(item);
            }

            var cantidadExpedientes = getNoExpediente.Count().ToString();
            var costoProducto = new CostoProducto();

            if (getNoExpediente.Count != 0)
            {
                tExpedientes.Visible = true;
                EmptyDiv.Visible = false;
                ComprarPaquetePremium.Visible = true;
                lblCantidad.Text = cantidadExpedientes.ToString();
                //List<CostoProducto> productoDisponibles = costoProductoCtrl.Retrieve(new CostoProducto { Nombre = "Expediente" }, false);
                List<CostoProducto> productoDisponibles = costoProductoCtrl.Retrieve(new CostoProducto(), false);

                foreach (CostoProducto item in productoDisponibles)
                {
                    if (item.Producto.TipoProducto == ETipoProducto.Expediente)
                        costoProducto = item;
                }

                if (costoProducto.FechaFin == null)
                {
                    string precioExpediente = costoProducto.Precio.ToString();
                    lblPrecio.Text = precioExpediente;
                    Session_costoPorExpediente = precioExpediente;
                }
            }
            else
            {
                costoProducto.Precio = 0;
                tExpedientes.Visible = false;
                EmptyDiv.Visible = true;
                ComprarPaquetePremium.Visible = false;
            }
            Session_listaSeleccionados = selecciones;
            Session_cantidadExpedientes = getNoExpediente.Count().ToString();
        }

        private List<InfoAlumnoUsuario> GetDataInfoExpediente() 
        {
            InfoAlumnoUsuario infoExpediente = InterfaceToFiltroExpediente();
            List<InfoAlumnoUsuario> listaInfoExpediente = infoAlumnoUsuarioCtrl.Retrieve(dctx, infoExpediente).Distinct().ToList();
            return listaInfoExpediente;
        }

        private InfoAlumnoUsuario InterfaceToFiltroExpediente() 
        {
            InfoAlumnoUsuario infoExpediente = new InfoAlumnoUsuario();
            infoExpediente.clasificador = new Clasificador();
            infoExpediente.estado = new Estado();
            if(!String.IsNullOrEmpty(ddlSearchAreasConocimiento.SelectedValue))
                infoExpediente.clasificador.ClasificadorID=int.Parse(ddlSearchAreasConocimiento.SelectedValue);
            if (!String.IsNullOrEmpty(ddlSearchEstado.SelectedValue))
                infoExpediente.estado.EstadoID = int.Parse(ddlSearchEstado.SelectedValue);
            if (!String.IsNullOrEmpty(txtEscuelaSearch.Text.Trim()))
                infoExpediente.Escuela = txtEscuelaSearch.Text.Trim();
            if (!String.IsNullOrEmpty(ddlSearchNivel.SelectedValue))
                infoExpediente.Grado = (EGrado)Convert.ChangeType(ddlSearchNivel.SelectedValue, typeof(byte));

            return infoExpediente;
        }
        #endregion

        #region Message Showing
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

        #region Autorizacion de la pagina
        private bool RenderEdit = false;
        private bool RenderDelete = false;

        protected override void AuthorizeUser()
        {
            //ejemplificando la ejecucion
            List<Permiso> permisos = userSession.CurrentPrivilegiosUniversidad.GetPermisos();
            bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PUACCESOXPEDIENTE) != null;
            bool lectura = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PUACCESOXPEDIENTE) != null;
            bool creacion = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PUACCESOXPEDIENTE) != null;
            bool delete = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PUACCESOXPEDIENTE) != null;
            bool edit = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PUACCESOXPEDIENTE) != null;

            if (!acceso)
                redirector.GoToHomePage(true);
            if (lectura)
                DisplayReadAction();
            if (creacion)
                DisplayCreateAction();
            if (delete)
                DisplayDeleteAction();
            if (edit)
                DisplayUpdateAction();
        }

        protected override void DisplayCreateAction()
        {
            
        }

        protected override void DisplayReadAction()
        {
            tExpedientes.Visible = true;
        }

        protected override void DisplayUpdateAction()
        {
            RenderEdit = true;
        }

        protected override void DisplayDeleteAction()
        {
            RenderDelete = true;
        }
        #endregion
    }
}