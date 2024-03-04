using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Framework.Base.Exceptions;
using POV.Administracion.Service;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.Core.Universidades.Implements;
using POV.Core.Universidades.Interfaces;
using POV.Licencias.BO;
using POV.Licencias.Service;
using POV.Seguridad.BO;
using POV.Seguridad.Service;
using POV.Web.PortalUniversidad.Helper;
using POV.Web.PortalUniversidad.AppCode.Page;
using POV.CentroEducativo.Services;
using System.Collections;
using POV.Prueba.Diagnostico.Dinamica.BO;
using Framework.Base.DataAccess;
using POV.Prueba.Diagnostico.Dinamica.Service;
using POV.Modelo.Service;
using POV.Modelo.BO;
using POV.Modelo.Context;

namespace POV.Web.PortalUniversidad.Carreras
{
    public partial class BuscarCarreras : CatalogPage
    {
        #region propiedades de la clase
        public List<Carrera> ListCarrera
        {
            get { return Session["LISTA_CARRERAS"] != null ? Session["LISTA_CARRERAS"] as List<Carrera> : null; }
            set { Session["LISTA_CARRERAS"] = value; }
        }

        public Carrera CarreraUpd
        {
            get { return Session["CarreraUpd"] != null ? Session["CarreraUpd"] as Carrera : null; }
            set { Session["CarreraUpd"] = value; }
        }
        #endregion

        private IDataContext dctx = ConnectionHlp.Default.Connection;
        private PruebaDinamicaCtrl pruebaDinamicaCtrl;
        private ModeloCtrl modeloCtrl;

        public BuscarCarreras()
        {
            pruebaDinamicaCtrl = new PruebaDinamicaCtrl();
            modeloCtrl = new ModeloCtrl();
        }

        #region *** eventos de pagina ***
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                CarreraUpd = null;
                if (userSession != null && userSession.CurrentUniversidad != null && userSession.CurrentCicloEscolar != null)
                {
                    if (!IsPostBack)
                    {
                        LoadAreasConocimiento();
                        LoadCarreras(userSession.CurrentUniversidad);
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

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                var listaFilter = ListCarrera;

                if (txtNombre.Text != string.Empty)
                {
                    listaFilter = listaFilter.Where(x => x.NombreCarrera == txtNombre.Text).ToList();
                }
               
                if (!String.IsNullOrEmpty(ddlAreaConocimiento.SelectedValue))
                    listaFilter = listaFilter.Where(x => x.ClasificadorID == int.Parse(ddlAreaConocimiento.SelectedValue)).ToList();

                if (String.IsNullOrEmpty(txtNombre.Text) && String.IsNullOrEmpty(ddlAreaConocimiento.SelectedValue))
                    listaFilter = ListCarrera;

                grdCarreras.DataSource = listaFilter;
                grdCarreras.DataBind();
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message, MessageType.Error);
            }
        }
        protected void grdCarreras_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                switch (e.CommandName)
                {
                    case "editar":
                        CarreraUpd = new Carrera { CarreraID = long.Parse(e.CommandArgument.ToString()) };
                        redirector.GoToEditarCarrera(true);
                        
                        break;
                    case "eliminar":
                        CarreraUpd = new Carrera { CarreraID = long.Parse(e.CommandArgument.ToString()) };
                        DoDelete(CarreraUpd);
                        CarreraUpd = null;
                        LoadCarreras(userSession.CurrentUniversidad);
                        break;
                }
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
        }
        #endregion


        #region *** Cargar Carreras ***
        private void LoadCarreras(Universidad universidad)
        {
            ListCarrera = userSession.CurrentUniversidad.Carreras.ToList();
            LlenarCarrerasUniversidad(ListCarrera);            
        }

        private void LlenarCarrerasUniversidad(List<Carrera> lista)
        {
            grdCarreras.DataSource = lista.GroupBy(test => test.CarreraID).Select(grp => grp.First()).ToList();
            grdCarreras.DataBind();
        }
        #endregion

        #region *** metodos auxiliares ***
        private void DoDelete(Carrera asignacionCarrera)
        {
            try
            {
                #region *** Desvincular Carrera-Universidad
                var objeto = new object();
                var contexto = new Contexto(objeto);

                UniversidadCtrl universidadCtrl = new UniversidadCtrl(contexto);
                var universidadSave = universidadCtrl.RetrieveWithRelationship(userSession.CurrentUniversidad, true).FirstOrDefault();

                CarreraCtrl carreraCtrl = new CarreraCtrl(contexto);
                Carrera carreraSeleccionado = carreraCtrl.Retrieve(asignacionCarrera, true).FirstOrDefault();
                if (universidadSave.Carreras.FirstOrDefault(x => x.CarreraID == carreraSeleccionado.CarreraID) != null)
                {
                    universidadSave.Carreras.Remove(carreraSeleccionado);
                }
                universidadCtrl.Update(universidadSave);
                contexto.Commit(objeto);

                // Se recarga las relaciones de la universidad
                UniversidadCtrl upSEssion = new UniversidadCtrl(null);
                userSession.CurrentUniversidad = upSEssion.RetrieveWithRelationship(universidadSave, false).FirstOrDefault();

                contexto.Dispose();

                #endregion
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message, MessageType.Error);
            }
        }

        private void LoadAreasConocimiento()
        {
            ddlAreaConocimiento.DataSource = GetAreasConocimiento();
            ddlAreaConocimiento.DataValueField = "ClasificadorID";
            ddlAreaConocimiento.DataTextField = "Nombre";
            ddlAreaConocimiento.DataBind();
            ddlAreaConocimiento.Items.Insert(0, new ListItem("Seleccionar área de conocimiento", ""));
        }

        private DataSet GetAreasConocimiento()
        {
            ArrayList arrAreaConocimiento = new ArrayList();
            Escuela escuela = userSession.CurrentEscuela;
            CicloEscolar cicloEscolar = userSession.CurrentCicloEscolar;
            Contrato contrato = new Contrato() { ContratoID = 2 };

            ContratoCtrl contratoCtrl = new ContratoCtrl();
            List<PruebaContrato> pruebas = contratoCtrl.RetrievePruebasAsignadoContrato(dctx, contrato, new CicloContrato { CicloEscolar = cicloEscolar });
            PruebaContrato pruebaPivoteContrato = pruebas.FirstOrDefault(item => item.TipoPruebaContrato == ETipoPruebaContrato.Pivote);

            var pruebaDinamica = (PruebaDinamica)pruebaPivoteContrato.Prueba;
            PruebaDinamica pruebaDinamicaModelo = pruebaDinamicaCtrl.RetrieveComplete(dctx, pruebaDinamica, false);
            DataSet DsClasificadores = modeloCtrl.RetrieveClasificador(dctx, new Clasificador { Activo = true }, new ModeloDinamico { ModeloID = pruebaDinamicaModelo.Modelo.ModeloID });

            return DsClasificadores;
        }

        private string EscapeLike(string valueWithoutWildcards)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < valueWithoutWildcards.Length; i++)
            {
                char c = valueWithoutWildcards[i];
                if (c == '*' || c == '%' || c == '[' || c == ']')
                    sb.Append("[").Append(c).Append("]");
                else if (c == '\'')
                    sb.Append("''");
                else
                    sb.Append(c);
            }
            return sb.ToString();
        }

        #endregion

        #region AUTORIZACION DE LA PAGINA


        protected void grdCarreras_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (RenderEdit)
                {
                    ImageButton btnEdit = (ImageButton)e.Row.FindControl("btnEdit");
                    btnEdit.Visible = true;
                }

                if (RenderDelete)
                {
                    ImageButton btnDelete = (ImageButton)e.Row.FindControl("btnDel");
                    btnDelete.Visible = true;
                }
            }
        }

        private bool RenderEdit = false;
        private bool RenderDelete = false;

        protected override void DisplayCreateAction()
        {
            PnlCreate.Visible = true;
        }

        protected override void DisplayReadAction()
        {

            grdCarreras.Visible = true;
        }

        protected override void DisplayUpdateAction()
        {
            RenderEdit = true;
        }

        protected override void DisplayDeleteAction()
        {
            RenderDelete = true;
        }

        protected override void AuthorizeUser()
        {
            //ejemplificando la ejecucion
            List<Permiso> permisos = userSession.CurrentPrivilegiosUniversidad.GetPermisos();

            bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PUACCESOCARRERAS) != null;
            bool lectura = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PUACCESOCARRERAS) != null;
            bool creacion = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PUACCESOCARRERAS) != null;
            bool delete = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PUACCESOCARRERAS) != null;
            bool edit = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PUACCESOCARRERAS) != null;

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