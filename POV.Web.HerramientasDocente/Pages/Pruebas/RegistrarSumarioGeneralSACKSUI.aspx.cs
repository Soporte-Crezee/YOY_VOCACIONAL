using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using POV.AppCode.Page;
using POV.Comun.BO;
using POV.ConfiguracionActividades.BO;
using POV.ServiciosActividades.Controllers;
using POV.Content.MasterPages;
using POV.ConfiguracionActividades.Services;
using POV.Prueba.Diagnostico.Dinamica.Service;
using POV.Web.Helper;
using System.Data;
using POV.CentroEducativo.BO;
using POV.Prueba.Diagnostico.Dinamica.BO;
using Framework.Base.Exceptions;
using System.Web.UI;
using POV.Prueba.BO;
using System.Web;

namespace POV.Web.HerramientasDocente.Pages.Pruebas
{
    public partial class RegistrarSumarioGeneralSACKSUI : PageBase
    {
        DataSet ds;
        DataTable dt;

        private string QS_Alumno
        {
            get { return this.Request.QueryString["num"]; }
        }

        public Boolean SS_Nuevo {
            get { return (Boolean)this.Session["Nuevo"]; }
            set { this.Session["Nuevo"] = value; }
        }
        RegistroPruebaDinamicaCtrl registroPruebaDinamicaCtrl = new RegistroPruebaDinamicaCtrl();
 
        #region eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SS_Nuevo = false;
                FillBack();
                var obj = FillSumarioGeneralSACKS();
                if (obj == null)
                {
                    SS_Nuevo = true;
                    ResultadoPruebaDinamicaCtrl resultCtrl = new ResultadoPruebaDinamicaCtrl();
                    accordion.Visible = true;
                    ds = resultCtrl.RetrieveResultadorPruebaSACKS(ConnectionHlp.Default.Connection, new Alumno { AlumnoID = int.Parse(QS_Alumno) }, new PruebaDinamica { PruebaID = 12 });
                    dt = ds.Tables[0].DefaultView.ToTable(true, "Descripcion");
                    dt.DefaultView.Sort = "Descripcion ASC";
                    rptrClaificador.DataSource = dt;
                    rptrClaificador.DataBind();
                }
                else
                {
                    accordion.Visible = false;
                    BtnGuardar.Visible = false;
                    DataToUserInterfaceDisable(obj);
                }
                    
            }
        }

        #endregion


        protected void rptrClaificador_DataBinding(object sender, EventArgs e)
        {

        }

        protected void rptrClaificador_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string strClasificador = ((DataRowView)(e.Item.DataItem)).Row.ItemArray[0].ToString();

                Repeater childRepeater = (Repeater)e.Item.FindControl("rptrReactivos");

                string strFilter = "Descripcion like " + "'" + strClasificador + "'";

                var dtFilter = ds.Tables[0].DefaultView.Table.Select(strFilter).CopyToDataTable();

                childRepeater.DataSource = dtFilter;
                childRepeater.DataBind();
            }
        }

        protected void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (SS_Nuevo)
                    DoInsert();

                //Response.Redirect("BuscarAlumnosSACKSUI.aspx");
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message, MessageType.Error);
            }

        }

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

                var obj = UserInterfaceToData();
                registroPruebaDinamicaCtrl.Insert(ConnectionHlp.Default.Connection, obj);
                Response.Redirect("BuscarAlumnosSACKSUI.aspx");
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message, MessageType.Error);
            }
        }

        private void ValidateData()
        {
            string sError = string.Empty;

            // valores requeridos
            if (txtMadurez.Text.Trim().Length <= 0)
                sError += " ,Madurez";
            if (txtConflictosExpresados.Text.Trim().Length <= 0)
                sError += " ,Conflictos expresados";
            if (txtNivelRealidad.Text.Trim().Length <= 0)
                sError += " ,NivelRealidad";

            if (sError.Trim().Length > 0)
            {
                sError = sError.Substring(2);
                throw new Exception(string.Format("Los siguientes parámetros son requeridos : {0}", sError));
            }

            if (sError.Trim().Length > 0)
            {
                sError = sError.Substring(2);
                throw new Exception(string.Format("Los siguientes parámetros son inválidos : {0}", sError));
            }
        }

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

        protected override void AuthorizeUser()
        {

        }

        #region *** User Interface to Data ***
        private SumarioGeneralSacks UserInterfaceToData()
        {
            SumarioGeneralSacks obj = new SumarioGeneralSacks();

            obj.Prueba = new PruebaDinamica();
            obj.Alumno = new Alumno();

            obj.Prueba.PruebaID = 12;
            obj.Alumno.AlumnoID = int.Parse(QS_Alumno);
            obj.SumarioMadurez = txtMadurez.Text.Trim();
            obj.SumarioNivelRealida = txtNivelRealidad.Text.Trim();
            obj.SumarioConflictoExpresados = txtConflictosExpresados.Text.Trim();
            obj.FechaRegistro = DateTime.Now;
            return obj;
        }
        #endregion
        

        #region *** Data to User Interface ***
        private void DataToUserInterfaceDisable(SumarioGeneralSacks obj)
        {
            obj.Prueba = new PruebaDinamica();
            obj.Alumno = new Alumno();

            txtMadurez.Text = obj.SumarioMadurez;
            txtNivelRealidad.Text = obj.SumarioNivelRealida;
            txtConflictosExpresados.Text = obj.SumarioConflictoExpresados;

            txtMadurez.Enabled = false;
            txtNivelRealidad.Enabled = false;
            txtConflictosExpresados.Enabled = false;
        }
        #endregion

        private void FillBack()
        {
            if (HttpContext.Current.Request.UrlReferrer != null)
            {
                lnkBack.NavigateUrl = HttpContext.Current.Request.UrlReferrer.PathAndQuery;
            }
            else
            {
                lnkBack.NavigateUrl = "~/BuscarAlumnosSACKSUI.aspx";
            }
        }

        protected void BtnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("BuscarAlumnosSACKSUI.aspx");
        }

        private SumarioGeneralSacks FillSumarioGeneralSACKS()
        {
            bool res = true;
            SumarioGeneralSacks sumario = new SumarioGeneralSacks();
            sumario.Prueba = new PruebaDinamica(){PruebaID=12};
            sumario.Alumno = new Alumno(){AlumnoID=long.Parse(QS_Alumno)};
            return registroPruebaDinamicaCtrl.LastDataRowToSumarioGeneralSacks(registroPruebaDinamicaCtrl.Retrieve(ConnectionHlp.Default.Connection, sumario));;
        }

    }
}