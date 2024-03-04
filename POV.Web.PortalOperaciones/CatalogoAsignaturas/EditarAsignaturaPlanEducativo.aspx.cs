using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Web.PortalOperaciones.Helper;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.Reactivos.BO;
using POV.Reactivos.Service;
using POV.Web.PortalOperaciones.AppCode.Page;
using POV.Seguridad.BO;
using POV.Modelo.Estandarizado.BO;
using POV.Modelo.Estandarizado.Service;
namespace POV.Web.PortalOperaciones.CatalogoAsignaturas
{
    public partial class EditarAsignaturaPlanEducativo : PageBase
    {
        private AreaAplicacionCtrl areaAplicacionCtrl;
        private MateriaCtrl materiaCtrl;

        #region *** propiedades de clase ***
        private PlanEducativo LastObject
        {
            set { Session["lastPlanEducativo"] = value; }
            get { return Session["lastPlanEducativo"] as PlanEducativo; }
        }

        private Materia LastMateria
        {
            set { Session["lastMateria"] = value; }
            get { return Session["LastMateria"] as Materia; }
        }

        private string ClaveOriginal
        {
            set { Session["claveOriginal"] = value; }
            get { return Session["claveOriginal"] as string; }
        }

        private List<Materia> ListMaterias
        {
            get
            {
                return Session["listMateriasPlan"] != null ? Session["listMateriasPlan"] as
                    List<Materia> : null;
            }
            set { Session["listMateriasPlan"] = value; }
        }

        private DataTable DtMateriasEstatus
        {
            get
            {
                return Session["DtMateriasPlan"] != null ? Session["DtMateriasPlan"] as
                    DataTable : null;
            }
            set { Session["DtMateriasPlan"] = value; }
        }
        #endregion

        public EditarAsignaturaPlanEducativo() 
        {
            areaAplicacionCtrl = new AreaAplicacionCtrl();
            materiaCtrl = new MateriaCtrl();
        }

        #region *** eventos de pagina ***
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (LastObject != null)
                {
                    TxtIDAsignatura.Enabled = false;
                    CargarDesdeTabla();
                    DataToUser(LastObject);
                    FillBack();
                }
                else
                {
                    LastMateria = null;
                    ClaveOriginal = null;
                    txtRedirect.Value = string.Empty;
                    Response.Redirect("~/CatalogoAsignaturas/EditarPlanEducativo.aspx", true);
                }

            }
        }

        protected void BtnGuardar_OnClick(Object sender, EventArgs e)
        {
            DoModif();
        }

        protected void BtnCancelar_OnClick(object sender, EventArgs e)
        {
            LastMateria = null;
            ClaveOriginal = null;
            Response.Redirect("~/CatalogoAsignaturas/EditarPlanEducativo.aspx", true);
        }
        #endregion

        #region *** validaciones ***
        private string ValidateData()
        {
            string sError = string.Empty;

            string clave = TxtClaveAsignatura.Text.Trim();
            string descripcion = TxtTituloAsignatura.Text.Trim();

            if (string.IsNullOrEmpty(clave))
                sError += ", Clave de la materia";

            if (string.IsNullOrEmpty(descripcion))
                sError += ", Descripción de la materia";

            if (cbGradoAsignatura.SelectedIndex == 0)
                sError += ", Grado de asignatura";

            if (cbAreaConocimiento.SelectedIndex == 0)
                sError += ", Area de conocimiento";

            if (sError.Length > 0)
                sError = "Los siguientes campos no pueden ser vacios: " + sError.Substring(2);
            return sError;
        }

        private string ValidateInfoMateria()
        {
            string sError = string.Empty;
            Materia mat = new Materia();
            mat.Clave = TxtClaveAsignatura.Text.Trim();

            if (mat.Clave != ClaveOriginal)
            {
                DataSet ds = materiaCtrl.Retrieve(ConnectionHlp.Default.Connection, mat);
                if (ds.Tables["Materia"].Rows.Count > 0)
                {
                    Materia matTemp = materiaCtrl.LastDataRowToMateria(ds);
                    if (matTemp.MateriaID != LastMateria.MateriaID)
                        sError = "Una asignatura similar ya se encuentra registrada, por favor verifique";
                    else
                        LastMateria.Clave = TxtClaveAsignatura.Text;
                }
                else
                {
                    Materia materiaExistente = ListMaterias.FirstOrDefault(item => item.Clave == mat.Clave);
                    if (materiaExistente != null)
                    {
                        sError = "Una asignatura similar ya se encuentra registrada, por favor verifique";
                    }
                    else
                    {
                        LastMateria.Clave = TxtClaveAsignatura.Text;
                    }

                }
            }
            LastMateria.Titulo = TxtTituloAsignatura.Text.Trim();
            LastMateria.Grado = Convert.ToByte(cbGradoAsignatura.SelectedValue);
            LastMateria.AreaAplicacion = GetAreaAplicativaFromUI();

            return sError;
        }

        #endregion

        #region *** Data to UserInterface ***
        private void DataToUser(PlanEducativo planEducativo)
        {
            ClaveOriginal = LastMateria.Clave;
            TxtIDAsignatura.Text = LastMateria.MateriaID.ToString();
            TxtClaveAsignatura.Text = LastMateria.Clave.ToString();
            TxtTituloAsignatura.Text = LastMateria.Titulo.ToString();
            cbAreaConocimiento.SelectedValue = LastMateria.AreaAplicacion.AreaAplicacionID.ToString();
            cbGradoAsignatura.SelectedValue = LastMateria.Grado.ToString();
        }

        private void CargarDesdeTabla()
        {
            cbAreaConocimiento.DataSource = configurarCbAreaConocimiento();
            cbAreaConocimiento.DataTextField = "Descripcion";
            cbAreaConocimiento.DataValueField = "AreaAplicacionID";
            cbAreaConocimiento.DataBind();

            loadGrados(LastObject.NivelEducativo);
        }

        private void loadGrados(NivelEducativo nivelEducativo)
        {
            if (nivelEducativo.NumeroGrados != null)
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(new DataTable());
                ds.Tables[0].Columns.Add("Grado", typeof(string));
                ds.Tables[0].Columns.Add("Nombre", typeof(string));

                DataRow row = ds.Tables[0].NewRow();
                row["Grado"] = DBNull.Value;
                row["Nombre"] = "Seleccionar";
                ds.Tables[0].Rows.InsertAt(row, 0);

                for (int i = 1; i <= nivelEducativo.NumeroGrados.Value; i++)
                {
                    DataRow newRow = ds.Tables[0].NewRow();
                    newRow["Grado"] = i.ToString();
                    newRow["Nombre"] = i + "°";
                    ds.Tables[0].Rows.Add(newRow);
                }
                cbGradoAsignatura.DataSource = ds;
                cbGradoAsignatura.DataTextField = "Nombre";
                cbGradoAsignatura.DataValueField = "Grado";
                cbGradoAsignatura.DataBind();
            }
        }

        private DataSet configurarCbAreaConocimiento()
        {
            DataSet ds = new DataSet();
            ds = areaAplicacionCtrl.Retrieve(ConnectionHlp.Default.Connection, new AreaAplicacion());
            DataRow row = ds.Tables["AreaAplicacion"].NewRow();
            row["AreaAplicacionID"] = DBNull.Value;
            row["Descripcion"] = "Seleccionar";
            ds.Tables["AreaAplicacion"].Rows.InsertAt(row, 0);
            return ds;
        }
        #endregion

        #region *** UserInterface to Data ***
        private AreaAplicacion GetAreaAplicativaFromUI()
        {
            AreaAplicacion areaAplicativa = new AreaAplicacion();
            int areaAplicativaID = 0;

            int.TryParse(cbAreaConocimiento.SelectedValue, out areaAplicativaID);

            if (areaAplicativaID > 0)
                areaAplicativa.AreaAplicacionID = areaAplicativaID;
            DataSet ds = areaAplicacionCtrl.Retrieve(ConnectionHlp.Default.Connection, areaAplicativa);
            areaAplicativa = areaAplicacionCtrl.LastDataRowToAreaAplicacion(ds);
            return areaAplicativa;
        }
        #endregion

        #region *** metodos auxiliares ***
        private void DoModif()
        {
            String validateMessage = ValidateData();

            if (validateMessage.Length < 1)
            {
                String validateDataMessage = ValidateInfoMateria();
                if (validateDataMessage.Length < 1)
                {
                    Materia materiaExistente = ListMaterias.FirstOrDefault(item => item.Clave == ClaveOriginal);

                    foreach (DataRow data in DtMateriasEstatus.Rows)
                    {
                        if (data["Clave"].ToString() == ClaveOriginal)
                        {
                            data["Clave"] = LastMateria.Clave;
                            break;
                        }
                    }

                    Response.Redirect("~/CatalogoAsignaturas/EditarPlanEducativo.aspx", true);
                }
                else
                {
                    txtRedirect.Value = string.Empty;
                    ShowMessage(validateDataMessage, MessageType.Error);
                }
            }
            else
            {
                txtRedirect.Value = string.Empty;
                ShowMessage(validateMessage, MessageType.Error);
            }
        }

        private void FillBack()
        {
            lnkBack.NavigateUrl = "~/CatalogoAsignaturas/EditarPlanEducativo.aspx";
        }
        #endregion

        #region AUTORIZACION DE LA PAGINA

        protected override void AuthorizeUser()
        {
            //ejemplificando la ejecucion
            List<Permiso> permisos = userSession.CurrentPrivilegios.GetPermisos();

            bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOPLANEDUCATIVO) != null;
            bool lectura = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POCONSULTARPLANEDUCATIVO) != null;
            bool edit = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POEDITARPLANEDUCATIVO) != null;
            bool delete = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POELIMINARPLANEDUCATIVO) != null;

            if (!acceso)
                redirector.GoToHomePage(true);
            if (!lectura)
                redirector.GoToHomePage(true);
            if (!edit)
                redirector.GoToHomePage(true);
            if (!delete)
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