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
using System.Text;

namespace POV.Web.HerramientasDocente.Pages.Pruebas
{
    public partial class RegistrarSumarioGeneralFrasesVocacionalesUI : PageBase
    {
        #region propiedades de la clase
        DataSet ds;
        DataTable dt;
        DataSet dsOrganizacion;
        DataTable dtOrganizacion;

        DataSet dsPerspectiva;
        DataTable dtPerspectiva;

        DataSet dsFuentes;
        DataTable dtFuentes;

        private string QS_Alumno
        {
            get { return this.Request.QueryString["num"]; }
        }

        public Boolean SS_Nuevo
        {
            get { return (Boolean)this.Session["Nuevo"]; }
            set { this.Session["Nuevo"] = value; }
        }
        RegistroPruebaDinamicaCtrl registroPruebaDinamicaCtrl = new RegistroPruebaDinamicaCtrl();
        #endregion

        #region eventos de la página
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SS_Nuevo = false;
                FillBack();
                var obj = FillSumarioGeneralFrasesVocacionales();
                if (obj == null)
                {
                    SS_Nuevo = true;
                    ResultadoPruebaDinamicaCtrl resultCtrl = new ResultadoPruebaDinamicaCtrl();
                    accordion.Visible = true;
                    ds = resultCtrl.RetrieveResultadorPruebaFrasesVocacionales(ConnectionHlp.Default.Connection, new Alumno { AlumnoID = int.Parse(QS_Alumno) }, new PruebaDinamica { PruebaID = 20 });

                    string strFilter = string.Empty;
                    strFilter = "Descripcion IN ('Expectativas ante la vida vocacional','Toma de decisiones','Autoconcepto','Identificaciones')";
                    //dsOrganizacion = new DataSet();
                    if (strFilter.Trim().Length > 0)
                    {
                        dsOrganizacion = Criterios(strFilter);

                        dtOrganizacion = dsOrganizacion.Tables[0].DefaultView.ToTable(true, "Descripcion");
                        dtOrganizacion.DefaultView.Sort = "Descripcion ASC";
                        rptrClasificadorOrganizacion.DataSource = dtOrganizacion;
                        rptrClasificadorOrganizacion.DataBind();
                    }

                    strFilter = "Descripcion IN ('Actitudes hacia el estudio','Actitudes hacia el trabajo','Expectativas de los demás')";
                    //dsPerspectiva = new DataSet();
                    if (strFilter.Trim().Length > 0)
                    {
                        dsPerspectiva = Criterios(strFilter);

                        dtPerspectiva = dsPerspectiva.Tables[0].DefaultView.ToTable(true, "Descripcion");
                        dtPerspectiva.DefaultView.Sort = "Descripcion ASC";
                        rptrClasificadorPerspectiva.DataSource = dtPerspectiva;
                        rptrClasificadorPerspectiva.DataBind();
                    }

                    strFilter = "Descripcion IN ('Barreras para la elección temores, ansiedades y necesidades','Elaboración de duelos')";
                    //dsFuentes = new DataSet();
                    if (strFilter.Trim().Length > 0)
                    {
                        dsFuentes = Criterios(strFilter);

                        dtFuentes = dsFuentes.Tables[0].DefaultView.ToTable(true, "Descripcion");
                        dtFuentes.DefaultView.Sort = "Descripcion ASC";
                        rptrClasificadorFuentes.DataSource = dtFuentes;
                        rptrClasificadorFuentes.DataBind();
                    }

                }
                else
                {
                    accordion.Visible = false;
                    BtnGuardar.Visible = false;
                    DataToUserInterfaceDisable(obj);
                }

            }
        }

        protected void rptrClasificadorOrganizacion_DataBinding(object sender, EventArgs e)
        {

        }

        protected void rptrClasificadorOrganizacion_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string strClasificador = ((DataRowView)(e.Item.DataItem)).Row.ItemArray[0].ToString();

                Repeater childRepeater = (Repeater)e.Item.FindControl("rptrReactivosOrganizacion");

                string strFilter = "Descripcion like " + "'" + strClasificador + "'";

                var dtFilter = dsOrganizacion.Tables[0].DefaultView.Table.Select(strFilter).CopyToDataTable();

                childRepeater.DataSource = dtFilter;
                childRepeater.DataBind();
            }
        }

        protected void rptrClasificadorPerspectiva_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string strClasificador = ((DataRowView)(e.Item.DataItem)).Row.ItemArray[0].ToString();

                Repeater childRepeater = (Repeater)e.Item.FindControl("rptrReactivosPerspectiva");

                string strFilter = "Descripcion like " + "'" + strClasificador + "'";

                var dtFilter = dsPerspectiva.Tables[0].DefaultView.Table.Select(strFilter).CopyToDataTable();

                childRepeater.DataSource = dtFilter;
                childRepeater.DataBind();
            }
        }

        protected void rptrClasificadorPerspectiva_DataBinding(object sender, EventArgs e)
        {

        }

        protected void rptrClasificadorFuentes_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string strClasificador = ((DataRowView)(e.Item.DataItem)).Row.ItemArray[0].ToString();

                Repeater childRepeater = (Repeater)e.Item.FindControl("rptrReactivosFuentes");

                string strFilter = "Descripcion like " + "'" + strClasificador + "'";

                var dtFilter = dsFuentes.Tables[0].DefaultView.Table.Select(strFilter).CopyToDataTable();

                childRepeater.DataSource = dtFilter;
                childRepeater.DataBind();
            }
        }

        protected void rptrClasificadorFuentes_DataBinding(object sender, EventArgs e)
        {

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

        protected void BtnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("BuscarAlumnosFrasesVocacionalesUI.aspx");
        }
        #endregion

        #region métodos auxiliares

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
                Response.Redirect("BuscarAlumnosFrasesVocacionalesUI.aspx");
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
            if (txtOrganizacionPersonalidad.Text.Trim().Length <= 0)
                sError += " ,Organizacio de Personalidad";
            if (txtPerspectivaOpciones.Text.Trim().Length <= 0)
                sError += " ,Perspectiva de las opciones profesionales";
            if (txtFuentesConflicto.Text.Trim().Length <= 0)
                sError += " ,Fuentes de conflicto";

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

        private DataSet Criterios(string filter)
        {
            DataSet dsCriterio = new DataSet();
            DataView dataView = new DataView(ds.Tables[0]);
            dataView.RowFilter = filter;
            filter = string.Empty;
            dsCriterio.Tables.Clear();
            dsCriterio.Tables.Add(dataView.ToTable());
            return dsCriterio;
        }

        #region *** User Interface to Data ***
        private SumarioGeneralFrasesVocacionales UserInterfaceToData()
        {
            SumarioGeneralFrasesVocacionales obj = new SumarioGeneralFrasesVocacionales();

            obj.Prueba = new PruebaDinamica();
            obj.Alumno = new Alumno();

            obj.Prueba.PruebaID = 20;
            obj.Alumno.AlumnoID = int.Parse(QS_Alumno);
            obj.SumarioOrganizacionPersonalidad = txtOrganizacionPersonalidad.Text.Trim();
            obj.SumarioPerspectivaOpciones = txtPerspectivaOpciones.Text.Trim();
            obj.SumarioFuentesConflicto = txtFuentesConflicto.Text.Trim();
            obj.FechaRegistro = DateTime.Now;
            return obj;
        }

        private SumarioGeneralFrasesVocacionales FillSumarioGeneralFrasesVocacionales()
        {
            bool res = true;
            SumarioGeneralFrasesVocacionales sumario = new SumarioGeneralFrasesVocacionales();
            sumario.Prueba = new PruebaDinamica() { PruebaID = 20 };
            sumario.Alumno = new Alumno() { AlumnoID = long.Parse(QS_Alumno) };
            return registroPruebaDinamicaCtrl.LastDataRowToSumarioGeneralFrasesVocacionales(registroPruebaDinamicaCtrl.Retrieve(ConnectionHlp.Default.Connection, sumario)); ;
        }
        #endregion

        #region *** Data to User Interface ***
        private void DataToUserInterfaceDisable(SumarioGeneralFrasesVocacionales obj)
        {
            obj.Prueba = new PruebaDinamica();
            obj.Alumno = new Alumno();

            txtOrganizacionPersonalidad.Text = obj.SumarioOrganizacionPersonalidad;
            txtPerspectivaOpciones.Text = obj.SumarioPerspectivaOpciones;
            txtFuentesConflicto.Text = obj.SumarioFuentesConflicto;

            txtOrganizacionPersonalidad.Enabled = false;
            txtPerspectivaOpciones.Enabled = false;
            txtFuentesConflicto.Enabled = false;
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
                lnkBack.NavigateUrl = "~/BuscarAlumnosFrasesVocacionalesUI.aspx";
            }
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

        protected override void AuthorizeUser()
        {

        }
    }
}