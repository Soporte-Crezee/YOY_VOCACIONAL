using POV.Administracion.Service;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.Comun.Service;
using POV.Logger.Service;
using POV.Seguridad.BO;
using POV.Seguridad.Utils;
using POV.Web.PortalUniversidad.AppCode.Page;
using POV.Web.PortalUniversidad.Helper;
using Framework.Base.Exceptions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using POV.CentroEducativo.Services;
using System.Data;
using POV.Licencias.BO;
using System.Collections;
using POV.Licencias.Service;
using POV.Prueba.Diagnostico.Dinamica.BO;
using POV.Modelo.BO;
using Framework.Base.DataAccess;
using POV.Prueba.Diagnostico.Dinamica.Service;
using POV.Modelo.Service;
using POV.Modelo.Context;

namespace POV.Web.PortalUniversidad.Carreras
{
    public partial class NuevaCarrera : PageBase
    {
        private GrupoCicloEscolarCtrl grupoCicloEscolarCtrl;
        private IDataContext dctx = ConnectionHlp.Default.Connection;
        private PruebaDinamicaCtrl pruebaDinamicaCtrl;
        private ModeloCtrl modeloCtrl;
        #region *** propiedades de clase ***
        public Carrera CarreraUpd
        {
            get { return Session["CarreraUpd"] != null ? Session["CarreraUpd"] as Carrera : null; }
            set { Session["CarreraUpd"] = value; }
        }

        public Carrera LastObject
        {
            get { return Session["LastCarrera"] != null ? (Carrera)Session["LastCarrera"] : null; }
            set { Session["LastCarrera"] = value; }
        }

        private List<Carrera> Session_Carreras
        {
            get { return (List<Carrera>)this.Session["LIST_CARRERA"]; }
            set { this.Session["LIST_CARRERA"] = value; }
        }
        #endregion

        public NuevaCarrera()
        {
            grupoCicloEscolarCtrl = new GrupoCicloEscolarCtrl();
            pruebaDinamicaCtrl = new PruebaDinamicaCtrl();
            modeloCtrl = new ModeloCtrl();
        }

        #region *** eventos de pagina ***
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (userSession == null || !userSession.IsLogin())
                    redirector.GoToLoginPage(true);

                if (userSession.CurrentEscuela == null || userSession.CurrentCicloEscolar == null)
                    redirector.GoToError(true);

                if (!IsPostBack)
                {
                    LoadAreasConocimiento();
                    if (CarreraUpd != null)
                    {
                        lblAccion.Text = "Editar";
                        lnkBack.NavigateUrl = "~/Carreras/BuscarCarreras.aspx";
                        LoadUpdate();
                    }
                }
            }
            catch (Exception ex)
            {

                this.ShowMessage(ex.Message, MessageType.Error);
            }
        }
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (CarreraUpd != null)
                    DoUpdate();
                else
                    DoInsert();
            }
            catch (Exception ex)
            {

                this.ShowMessage(ex.Message, MessageType.Error);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (CarreraUpd != null)
                redirector.GoToConsultarCarreras(false);
            else
                redirector.GoToVincularCarreras(false);
        }
        #endregion

        #region *** validaciones ***
        private void ValidateData()
        {
            string sError = string.Empty;

            //Valores Requeridos.
            if (txtDescripcion.Text.Trim().Length <= 0)
                sError += " ,Descripcion";
            if (txtNombre.Text.Trim().Length <= 0)
                sError += " ,Nombre";
            if (ddlAreaConocimiento.SelectedIndex == -1 || ddlAreaConocimiento.SelectedValue.Length <= 0)
                sError += " ,Area Conocimiento";


            if (sError.Trim().Length > 0)
            {
                sError = sError.Substring(2);
                throw new Exception(string.Format("Los siguientes parámetros son requeridos:{0}", sError));
            }

            //Valores con Incorrectos.
            if (txtDescripcion.Text.Trim().Length > 254)
                sError += " ,Descripcion";
            if (txtNombre.Text.Trim().Length > 50)
                sError += " ,Nombre";

            if (sError.Trim().Length > 0)
            {
                sError = sError.Substring(2);
                throw new Exception(string.Format("Los siguientes parámetros son inválidos :{0}", sError));
            }
        }
        #endregion

        #region *** UserInterface to Data ***
        private Carrera UserInterfaceToData()
        {
            Carrera carrera = new Carrera();
            carrera.NombreCarrera = txtNombre.Text.Trim();
            carrera.Descripcion = txtDescripcion.Text.Trim();
            carrera.ClasificadorID = int.Parse(ddlAreaConocimiento.SelectedValue);
            carrera.Activo = true;
            return carrera;
        }
        #endregion

        #region *** metodos auxiliares ***
        private void DoInsert()
        {
            try
            {
                try
                {
                    ValidateData();

                }
                catch (Exception ex)
                {
                    this.ShowMessage(ex.Message, MessageType.Error);
                    return;
                }
                var objeto = new object();
                var contexto = new Contexto(objeto);
                LastObject = UserInterfaceToData();

                UniversidadCtrl universidadCtrl = new UniversidadCtrl(contexto);
                var universidadSave = universidadCtrl.RetrieveWithRelationship(userSession.CurrentUniversidad, true).FirstOrDefault();

                if (universidadSave.Carreras.FirstOrDefault(x => x.CarreraID == LastObject.CarreraID) == null) 
                    {
                        universidadSave.Carreras.Add(LastObject);
                    }
                universidadCtrl.Update(universidadSave);
                contexto.Commit(objeto);
                
                contexto.Dispose();

                // Se recarga las relaciones de la universidad-carrera
                UniversidadCtrl upSEssion = new UniversidadCtrl(null);
                userSession.CurrentUniversidad = upSEssion.RetrieveWithRelationship(universidadSave, false).FirstOrDefault();
                LastObject = null;
                redirector.GoToConsultarCarreras(false);
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message, MessageType.Error);
            }
        }

        private void ConsultarCarreras(Carrera filter)
        {
            CarreraCtrl carreraCtrl = new CarreraCtrl(null);
            List<Carrera> listaCarreras = carreraCtrl.Retrieve(filter, false);
            Session_Carreras = listaCarreras;
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

        #region Update
        private void LoadUpdate()
        {
            CarreraCtrl carreraCtrl = new CarreraCtrl(null);
            Carrera carreraUpd = carreraCtrl.Retrieve(new Carrera { CarreraID = CarreraUpd.CarreraID }, false).First();
            DataToUserInterface(carreraUpd);
        }

        private void DataToUserInterface(Carrera carrera)
        {
            txtNombre.Text = carrera.NombreCarrera;
            txtDescripcion.Text = carrera.Descripcion;
            ddlAreaConocimiento.SelectedValue = carrera.ClasificadorID.ToString();
        }

        private Carrera UserInterfaceToDataUpd()
        {
            Carrera carrera = new Carrera();
            carrera.NombreCarrera = txtNombre.Text.Trim();
            carrera.Descripcion = txtDescripcion.Text.Trim();
            carrera.ClasificadorID = int.Parse(ddlAreaConocimiento.SelectedValue);
            carrera.Activo = true;
            return carrera;
        }

        private void DoUpdate()
        {
            try
            {
                try
                {
                    ValidateData();

                }
                catch (Exception ex)
                {
                    this.ShowMessage(ex.Message, MessageType.Error);
                    return;
                }
                var objeto = new object();
                var contexto = new Contexto(objeto);
                LastObject = UserInterfaceToDataUpd();

                UniversidadCtrl universidadCtrl = new UniversidadCtrl(contexto);
                var universidadSave = universidadCtrl.RetrieveWithRelationship(userSession.CurrentUniversidad, true).FirstOrDefault();

                // Eliminar la realacion con la carrera anterior
                CarreraCtrl carreraCtrlAnterior = new CarreraCtrl(contexto);
                Carrera carreraSeleccionadoAnterior = carreraCtrlAnterior.Retrieve(CarreraUpd, true).FirstOrDefault();
                if (universidadSave.Carreras.FirstOrDefault(x => x.CarreraID == carreraSeleccionadoAnterior.CarreraID) != null)
                {
                    universidadSave.Carreras.Remove(carreraSeleccionadoAnterior);
                }
                

                // Crear relacion con la nueva carrera
                if (universidadSave.Carreras.FirstOrDefault(x => x.CarreraID == LastObject.CarreraID) == null)
                {
                    universidadSave.Carreras.Add(LastObject);
                }
                universidadCtrl.Update(universidadSave);
                contexto.Commit(objeto);

                contexto.Dispose();


                // Se recarga las relaciones de la universidad-carrera
                UniversidadCtrl upSEssion = new UniversidadCtrl(null);
                userSession.CurrentUniversidad = upSEssion.RetrieveWithRelationship(universidadSave, false).FirstOrDefault();

                // Eliminar la relacion carrera-universidad
                CarreraCtrl carreraCtrl = new CarreraCtrl(null);
                var carreraSave = carreraCtrl.RetrieveWithRelationship(CarreraUpd, true).FirstOrDefault();
                if (carreraSave.Universidades.Count == 0)
                {
                    carreraCtrl.Delete(carreraSave);
                }

                LastObject = null;
                CarreraUpd = null;
                redirector.GoToConsultarCarreras(false);
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message, MessageType.Error);
            }
        }
        #endregion
        #endregion

        #region AUTORIZACION DE LA PAGINA

        protected override void AuthorizeUser()
        {
            //ejemplificando la ejecucion
            List<Permiso> permisos = userSession.CurrentPrivilegiosUniversidad.GetPermisos();

            bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PUACCESOCARRERAS) != null;
            bool creacion = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PUACCESOCARRERAS) != null;

            if (!acceso)
                redirector.GoToHomePage(true);
            if (!creacion)
                redirector.GoToHomePage(true);
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