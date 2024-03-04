using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Prueba.BO;
using POV.Prueba.Diagnostico.Dinamica.BO;
using POV.Prueba.Diagnostico.Service;
using POV.Reactivos.BO;
using POV.Seguridad.BO;
using POV.Web.PortalOperaciones.AppCode.Page;
using POV.Web.PortalOperaciones.Helper;

namespace POV.Web.PortalOperaciones.Pruebas
{
    public partial class ConfigurarBancoReactivos : PageBase
    {
        #region ******Propiedades*******

        private CatalogoPruebaCtrl catalogoPruebaCtrl = new CatalogoPruebaCtrl();
        private IDataContext dctx = null;

        private APrueba LastObject
        {
            set { Session["lastPruebas"] = value; }
            get { return Session["lastPruebas"] != null ? Session["lastPruebas"] as APrueba : null; }
        }
        private ABancoReactivo LastBancoReactivos
        {
            set { Session["lastBancoReactivos"] = value; }
            get { return Session["lastBancoReactivos"] != null ? Session["lastBancoReactivos"] as ABancoReactivo : null; }
        }
        private List<ReactivoBanco> ListaReactivosBanco
        {
            set { Session["listReactivosBanco"] = value; }
            get { return Session["listReactivosBanco"] != null ? Session["listReactivosBanco"] as List<ReactivoBanco> : null; }
        }
        private List<Reactivo> ListaReactivosModelo
        {
            set { Session["listReactivosModelo"] = value; }
            get { return Session["listReactivosModelo"] != null ? Session["listReactivosModelo"] as List<Reactivo> : null; }
        }
        private DataSet DsReactivosModelo
        {
            set { Session["DsReactivosModelo"] = value; }
            get { return Session["DsReactivosModelo"] != null ? Session["DsReactivosModelo"] as DataSet : null; }
        }
        private int CountReactivos
        {
            set { Session["countReactivos"] = value; }
            get { return Session["countReactivos"] != null ? (int)Session["countReactivos"] : 0; }
        }
        private int CountSelectedReactivos
        {
            set { Session["countSelectedReactivos"] = value; }
            get { return Session["countSelectedReactivos"] != null ? (int)Session["countSelectedReactivos"] : 0; }
        }
        #endregion

        #region ********eventos********
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    if (LastObject == null)
                    {
                        txtRedirect.Value = "BuscarPruebas.aspx";
                        ShowMessage("Ocurrió un problema al consultar el banco de la prueba", MessageType.Error);
                    }
                    else
                    {
                        LastBancoReactivos = null;
                        ListaReactivosBanco = null;
                        ListaReactivosModelo = null;
                        DsReactivosModelo = null;
                        CountReactivos = 0;
                        CountSelectedReactivos = 0;
                        dctx = ConnectionHlp.Default.Connection;
                        LoadPrueba();
                        LoadReactivosModelo();
                        LoadBancoReactivos();
                    }
                }
            }
            catch (Exception ex)
            {
                txtRedirect.Value = "BuscarPruebas.aspx";
                ShowMessage("Ocurrió un problema al consultar el banco de la prueba", MessageType.Error);
            }
        }
        protected void btnAsignarTodos_Click(object sender, EventArgs e)
        {
            try
            {
                AddReactivosSeleccionados();
            }
            catch (Exception ex)
            {

                this.ShowMessage(ex.Message, MessageType.Error);
            }
        }
        protected void lnkbtnCopiarReactivo_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateEstadoSeleccionReactivosBanco();
                List<Reactivo> reactivos = new List<Reactivo>();

                using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
                {
                    HiddenField hdnRowReactivoID = (HiddenField)row.FindControl("hdnReactivoID");
                    Label lblRowClave = (Label)row.FindControl("lblClave");
                    Reactivo reactivo = new Reactivo { ReactivoID = Guid.Parse(hdnRowReactivoID.Value) };
                    string sError;
                    if (ValidateReactivoSeleccionado(reactivo, out sError))
                    {
                        reactivo = ListaReactivosModelo.FirstOrDefault(x => x.ReactivoID == reactivo.ReactivoID);
                        reactivos.Add(reactivo);
                    }
                    else
                    {
                        throw new Exception(string.Format("Ocurrió un error al asignar el reactivo:{0}. {1}", lblRowClave.Text, sError));
                    }
                }
                AddReactivosBanco(reactivos);
                grdBancoReactivos.DataSource = ListaReactivosBanco.Where(x => x.Activo == true).ToList();
                grdBancoReactivos.DataBind();

                CountReactivos = ListaReactivosBanco.Count(x => x.Activo == true);
                txtTotalReactivos.Text = CountReactivos.ToString();
            }
            catch (Exception ex)
            {

                this.ShowMessage(ex.Message, MessageType.Error);
            }
        }
        protected void btnBuscarReactivos_Click(object sender, EventArgs e)
        {
            try
            {
                LoadReactivosModelo();
                UpdateEstadoSeleccionReactivosBanco();
                txtClaveReactivo.Text = string.Empty;
                txtNombreReactivo.Text = string.Empty;
            }
            catch (Exception ex)
            {


            }
        }
        protected void rdUsarTotal_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rdUsarTotal.Checked)
                {
                    CountSelectedReactivos = 0;
                    rdUsarAleatorio.Checked = false;
                    rdUsarSeleccionados.Checked = false;
                    plSeleccionAleatoria.Visible = false;
                    plSeleccionEspecifica.Visible = false;
                    txtTotalReactivosSeleccionados.Text = CountSelectedReactivos.ToString();
                    txtTotalReactivosAleatorios.Text = string.Empty;
                    foreach (GridViewRow row in grdBancoReactivos.Rows)
                    {
                        CheckBox checkrow = (CheckBox)row.FindControl("chkUsar");
                        checkrow.Checked = true;
                    }
                    UpdateEstadoSeleccionReactivosBanco();
                }

            }
            catch (Exception ex)
            {


            }
        }
        protected void rdUsarAleatorio_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rdUsarAleatorio.Checked)
                {
                    plSeleccionAleatoria.Visible = true;
                    rdUsarTotal.Checked = false;
                    rdUsarSeleccionados.Checked = false;
                    plSeleccionEspecifica.Visible = false;
                    CountSelectedReactivos = 0;
                    txtTotalReactivosSeleccionados.Text = CountSelectedReactivos.ToString();
                    foreach (GridViewRow row in grdBancoReactivos.Rows)
                    {
                        CheckBox checkrow = (CheckBox)row.FindControl("chkUsar");
                        checkrow.Checked = false;
                    }
                    UpdateEstadoSeleccionReactivosBanco();
                }
            }
            catch (Exception ex)
            {


            }
        }
        protected void rdUsarSeleccionados_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rdUsarSeleccionados.Checked)
                {
                    plSeleccionEspecifica.Visible = true;
                    rdUsarAleatorio.Checked = false;
                    rdUsarTotal.Checked = false;
                    plSeleccionAleatoria.Visible = false;
                    CountSelectedReactivos = 0;
                    txtTotalReactivosAleatorios.Text = string.Empty;
                    UpdateEstadoSeleccionReactivosBanco();
                    CountSelectedReactivos = ListaReactivosBanco.Count(x => x.Activo == true && x.EstaSeleccionado == true);

                    txtTotalReactivosSeleccionados.Text = CountSelectedReactivos.ToString();
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void lnkbtnQuitar_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {

                    UpdateEstadoSeleccionReactivosBanco();
                    using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
                    {
                        HiddenField hdnRowReactivoID = (HiddenField)row.FindControl("hdnReactivoID");
                        HiddenField hdnRowReactivoBancoID = (HiddenField)row.FindControl("hdnReactivoBancoID");
                        HiddenField hdnRowReactivoOriginalID = (HiddenField)row.FindControl("hdnReactivoOriginalID");
                        CheckBox chkUsar = (CheckBox)row.FindControl("chkUsar");
                        ReactivoBanco reactivobanco = new ReactivoBanco
                            {
                                ReactivoBancoID = !string.IsNullOrEmpty(hdnRowReactivoBancoID.Value) ? long.Parse(hdnRowReactivoBancoID.Value) : (long?)null,
                                Reactivo = new Reactivo { ReactivoID = Guid.Parse(hdnRowReactivoID.Value) },
                                EstaSeleccionado = chkUsar.Checked,
                                ReactivoOriginal = new Reactivo { ReactivoID = Guid.Parse(hdnRowReactivoOriginalID.Value) }
                            };

                        RemoveReactivoBanco(reactivobanco);
                        ReorderReactivosBanco();
                        grdBancoReactivos.DataSource = ListaReactivosBanco.Where(x => x.Activo == true).ToList();
                        grdBancoReactivos.DataBind();
                        txtTotalReactivos.Text = CountReactivos.ToString();
                    }

                }
                catch (Exception ex)
                {


                }
            }
            catch (Exception)
            {

            }
        }
        protected void btnSubir_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                UpdateEstadoSeleccionReactivosBanco();
                UpReactivoBanco();
            }
            catch (Exception ex)
            {


            }
        }
        protected void btnBajar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                UpdateEstadoSeleccionReactivosBanco();
                DownReactivoBanco();
            }
            catch (Exception ex)
            {


            }
        }
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                DoUpdate();
                
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message,MessageType.Error);
               
            }
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                ClearSession();
                Response.Redirect("BuscarPruebas.aspx");
            }
            catch
            {


            }

        }

        protected void chkTodos_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkTodos = (CheckBox)sender;

            bool valor = chkTodos.Checked;

            UpdateEstadoSeleccionReactivosBanco();
            foreach (DataRow dr in DsReactivosModelo.Tables[0].Rows)
            {
                dr["Usar"] = valor;
            }

            grdReactivos.DataSource = DsReactivosModelo;
            
            grdReactivos.DataBind();
            ((CheckBox)grdReactivos.HeaderRow.FindControl("chkTodos")).Checked = valor;
        }
        #endregion

        #region *******metodos auxiliares*******

        private void LoadPrueba()
        {
            if (LastObject != null)
            {
                LastBancoReactivos = null;

                LastObject = catalogoPruebaCtrl.RetrieveComplete(dctx, LastObject,true);
                txtClavePrueba.Text = LastObject.Clave;
                txtNombrePrueba.Text = LastObject.Nombre;
                txtModeloPrueba.Text = LastObject.Modelo.Nombre;
            }
        }
        private void LoadReactivosModelo()
        {
            dctx = ConnectionHlp.Default.Connection;

            Reactivo filtro = new Reactivo { Clave = !string.IsNullOrEmpty(txtClaveReactivo.Text) ? txtClaveReactivo.Text.Trim() : null, NombreReactivo = !string.IsNullOrEmpty(txtNombreReactivo.Text) ? string.Format("%{0}%", txtNombreReactivo.Text.Trim()) : null };
            switch (LastObject.TipoPrueba)
            {
                case ETipoPrueba.Dinamica:
                    filtro.TipoReactivo = ETipoReactivo.ModeloGenerico;
                    filtro.Caracteristicas = new CaracteristicasModeloGenerico();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

          
            List<Reactivo> ls = catalogoPruebaCtrl.RetrieveListaReactivos(dctx, LastObject.Modelo, filtro, false);

            ls = ls.OrderBy(x => x.Clave).ToList();

            ListaReactivosModelo = new List<Reactivo>();

            if (DsReactivosModelo == null || DsReactivosModelo.Tables[0] == null || DsReactivosModelo.Tables[0].Columns.Count <= 0)
            {
                DsReactivosModelo = new DataSet("ReactivosModelo");
                DsReactivosModelo.Tables.Add(new DataTable());
                if (!DsReactivosModelo.Tables[0].Columns.Contains("Usar")) { DsReactivosModelo.Tables[0].Columns.Add(new DataColumn("Usar", typeof(bool))); }
                if (!DsReactivosModelo.Tables[0].Columns.Contains("ReactivoID")) { DsReactivosModelo.Tables[0].Columns.Add(new DataColumn("ReactivoID", typeof(Guid))); }
                if (!DsReactivosModelo.Tables[0].Columns.Contains("Clave")) { DsReactivosModelo.Tables[0].Columns.Add(new DataColumn("Clave", typeof(string))); }
                if (!DsReactivosModelo.Tables[0].Columns.Contains("NombreReactivo")) { DsReactivosModelo.Tables[0].Columns.Add(new DataColumn("NombreReactivo", typeof(string))); }
                if (!DsReactivosModelo.Tables[0].Columns.Contains("TipoReactivo")) { DsReactivosModelo.Tables[0].Columns.Add(new DataColumn("TipoReactivo", typeof(byte))); }

            }
            DsReactivosModelo.Tables[0].Rows.Clear();
            foreach (Reactivo reactivo in ls)
            {
                if (reactivo.Activo == true && reactivo.Asignado==false)
                {
                    DataRow row = DsReactivosModelo.Tables[0].NewRow();
                    row["Usar"] = false;
                    row["ReactivoID"] = reactivo.ReactivoID;
                    row["Clave"] = reactivo.Clave;
                    row["NombreReactivo"] = reactivo.NombreReactivo;
                    row["TipoReactivo"] = (byte)reactivo.TipoReactivo;
                    DsReactivosModelo.Tables[0].Rows.Add(row);
                    ListaReactivosModelo.Add(reactivo);
                }
            }
            grdReactivos.DataSource = DsReactivosModelo;
            grdReactivos.DataBind();

        }
        private void LoadBancoReactivos()
        {
            LastBancoReactivos = catalogoPruebaCtrl.RetrieveBancoReactivosPrueba(dctx, LastObject);
            LastBancoReactivos.ListaReactivosBanco = LastBancoReactivos.ListaReactivosBanco.OrderBy(x => x.Orden).ToList();

            if (ListaReactivosBanco == null) ListaReactivosBanco = new List<ReactivoBanco>();

            CountReactivos = 0;
            CountSelectedReactivos = 0;
            foreach (ReactivoBanco reactivoBanco in LastBancoReactivos.ListaReactivosBanco)
            {
                if (reactivoBanco.Activo == true)
                {
                    ListaReactivosBanco.Add(reactivoBanco);
                    CountReactivos++;
                    if ((bool)reactivoBanco.EstaSeleccionado) { CountSelectedReactivos++; }
                }
            }
            chkOrdenamiento.Checked = !LastBancoReactivos.EsSeleccionOrdenada.Value;
            chkPorGrupo.Checked = (bool)LastBancoReactivos.EsPorGrupo;
            

            if (LastBancoReactivos.TipoSeleccionBanco != null)
                switch (LastBancoReactivos.TipoSeleccionBanco)
                {
                    case ETipoSeleccionBanco.TOTAL:
                        rdUsarTotal.Checked = true;
                        txtTotalReactivos.Text = CountReactivos.ToString();
                        break;
                    case ETipoSeleccionBanco.ALEATORIA:
                        rdUsarAleatorio.Checked = true;
                        plSeleccionAleatoria.Visible = true;
                        txtTotalReactivosAleatorios.Text = LastBancoReactivos.NumeroReactivos.ToString();
                        break;
                    case ETipoSeleccionBanco.NUMERO_ESPECIFICO:
                        rdUsarSeleccionados.Checked = true;
                        plSeleccionEspecifica.Visible = true;
                        txtTotalReactivosSeleccionados.Text = CountSelectedReactivos.ToString();
                        break;
                    default:
                        {
                            rdUsarAleatorio.Checked = false;
                            rdUsarTotal.Checked = false;
                            rdUsarSeleccionados.Checked = false;
                        }
                        break;
                }
            txtReactivosPorPagina.Text = LastBancoReactivos.ReactivosPorPagina.ToString();
            txtTotalReactivos.Text = CountReactivos.ToString();
            grdBancoReactivos.DataSource = ListaReactivosBanco;
            grdBancoReactivos.DataBind();

        }
        private bool ValidateReactivoSeleccionado(Reactivo reactivo, out string error)
        {
            error = string.Empty;
            if (ListaReactivosBanco.Any(reactivoBanco => reactivoBanco.Activo == true && reactivo.ReactivoID == reactivoBanco.ReactivoOriginal.ReactivoID))
            {
                error = " El reactivo se encuentra en el banco de reactivos ";
                return false;
            }
            return true;
        }
        private void AddReactivosBanco(IEnumerable<Reactivo> reactivos)
        {
            foreach (Reactivo reactivo in reactivos)
            {

                ReactivoBanco reactivoBanco = new ReactivoBanco
                    {
                        Activo = true,
                        Orden = CountReactivos + 1,
                        Reactivo = reactivo,
                        ReactivoOriginal = reactivo,
                        EstaSeleccionado = rdUsarTotal.Checked
                    };
                ListaReactivosBanco.Add(reactivoBanco);
                CountReactivos = ListaReactivosBanco.Count(x => x.Activo == true);
            }
        }
        private void RemoveReactivoBanco(ReactivoBanco reactivo)
        {
            int index = -1;
            if (reactivo.ReactivoBancoID == null)
            {
                index = ListaReactivosBanco.FindIndex(x => (x.Activo == true && x.ReactivoOriginal.ReactivoID == reactivo.ReactivoOriginal.ReactivoID));
                ListaReactivosBanco.RemoveAt(index);
            }
            else
            {
                ReactivoBanco reactivoBanco = ListaReactivosBanco.FirstOrDefault(x => x.ReactivoBancoID == reactivo.ReactivoBancoID);
                reactivoBanco.Activo = false;
            }
            if ((bool)reactivo.EstaSeleccionado)
            {
                CountSelectedReactivos--;
            }
            CountReactivos = ListaReactivosBanco.Count(x => x.Activo == true);
        }
        private void ReorderReactivosBanco()
        {
            int order = 1;
            foreach (ReactivoBanco reactivoBanco in ListaReactivosBanco)
            {
                if (reactivoBanco.Activo == true)
                {
                    reactivoBanco.Orden = order;
                    order++;
                }

            }
        }
        private void UpdateEstadoSeleccionReactivosBanco()
        {
            foreach (GridViewRow row in grdBancoReactivos.Rows)
            {
                HiddenField hdnRowReactivoID = (HiddenField)row.FindControl("hdnReactivoID");
                CheckBox chkUsar = (CheckBox)row.FindControl("chkUsar");
                ReactivoBanco reactivo = ListaReactivosBanco.FirstOrDefault(x => (x.Activo == true && x.Reactivo.ReactivoID == Guid.Parse(hdnRowReactivoID.Value)));
                if (reactivo != null)
                {
                    reactivo.EstaSeleccionado = chkUsar.Checked;
                }
            }
        }
        private void AddReactivosSeleccionados()
        {
            UpdateEstadoSeleccionReactivosBanco();

            List<Reactivo> reactivos = new List<Reactivo>();
            foreach (GridViewRow row in grdReactivos.Rows)
            {
                CheckBox chkRowUsar = (CheckBox)row.FindControl("chkUsar");
                if (chkRowUsar.Checked)
                {
                    HiddenField hdnRowReactivoID = (HiddenField)row.FindControl("hdnReactivoID");
                    Reactivo reactivo = new Reactivo { ReactivoID = Guid.Parse(hdnRowReactivoID.Value) };
                    string sError;
                    if (ValidateReactivoSeleccionado(reactivo, out sError))
                    {
                        reactivo = ListaReactivosModelo.FirstOrDefault(x => x.ReactivoID == reactivo.ReactivoID);
                        reactivos.Add(reactivo);
                    }
                    else
                    {
                        Label lblRowClave = (Label)row.FindControl("lblClave");
                        throw new Exception(string.Format("Ocurrió un error al asignar el reactivo:{0}. {1}", lblRowClave.Text, sError));
                    }
                }
            }
            AddReactivosBanco(reactivos);

            grdBancoReactivos.DataSource = ListaReactivosBanco.Where(x => x.Activo == true).ToList();
            grdBancoReactivos.DataBind();
            txtTotalReactivos.Text = CountReactivos.ToString();
        }
        private void ValidateBancoReactivos()
        {

            bool conf = false;
            int count = ListaReactivosBanco.Count(x => x.Activo == true);
            if (count <= 0) throw new Exception("Por favor, agrega al menos un reactivo para el banco de reactivos");

            if (rdUsarAleatorio.Checked)
            {
                conf = true;
                if (txtTotalReactivosAleatorios.Text.Trim().Length <= 0) throw new Exception("El número de reactivos para la selección aleatoria es obligatorio ");
                int aux;
                if (!int.TryParse(txtTotalReactivosAleatorios.Text.Trim(), out aux)) throw new Exception("Por favor, escribe un número entero válido");
                if (aux < 1) throw new Exception("El número de reactivos debe ser mayor que cero");
                if (aux > count) throw new Exception(string.Format("El número de reactivos para la selección aleatoria no corresponde al número de reactivos agregados al banco. \n Por favor,escribe un valor igual o menor que {0} ", count));
            }
            else if (rdUsarTotal.Checked)
                conf = true;
            else if (rdUsarSeleccionados.Checked)
            {
                conf = true;
                if (ListaReactivosBanco.Count(x => x.Activo == true && x.EstaSeleccionado == true) <= 0)
                    throw new Exception("Por favor,selecciona al menos un reactivo del banco de reactivos ");
            }
            if (!conf) throw new Exception("El campo opción de selección es obligatorio");
            int val;
            if (!int.TryParse(txtReactivosPorPagina.Text.Trim(), out val)) throw new Exception("Por favor, escribe un número entero válido en el campo reactivos por página");
            else if (txtReactivosPorPagina.Text == string.Empty) 
                throw new Exception("El campo reactivos por pagina es obligatorio");
            else if (int.Parse(txtReactivosPorPagina.Text) < 1)
                throw new Exception("El campo reactivos por pagina debe ser mayor a 0");

        }
        private ABancoReactivo UserInterfaceToData()
        {
            ABancoReactivo aBanco = null;
            switch (LastObject.TipoPrueba)
            {
                case ETipoPrueba.Dinamica:
                    aBanco = new BancoReactivosDinamico
                        {
                            Prueba = LastObject,
                            BancoReactivoID = LastBancoReactivos.BancoReactivoID
                        };
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            aBanco.Activo = true;
            aBanco.FechaRegistro = LastBancoReactivos.FechaRegistro;
            aBanco.EsSeleccionOrdenada = !chkOrdenamiento.Checked;
            aBanco.EsPorGrupo = chkPorGrupo.Checked;
            aBanco.ReactivosPorPagina = int.Parse(txtReactivosPorPagina.Text);
          
            if (rdUsarAleatorio.Checked)
            {
                aBanco.TipoSeleccionBanco = ETipoSeleccionBanco.ALEATORIA;
                aBanco.NumeroReactivos = int.Parse(txtTotalReactivosAleatorios.Text);
            }
            else if (rdUsarTotal.Checked)
            {
                aBanco.TipoSeleccionBanco = ETipoSeleccionBanco.TOTAL;
                aBanco.NumeroReactivos = ListaReactivosBanco.Count(x => x.Activo == true);
            }
            else if (rdUsarSeleccionados.Checked)
            {
                aBanco.TipoSeleccionBanco = ETipoSeleccionBanco.NUMERO_ESPECIFICO;
                aBanco.NumeroReactivos = ListaReactivosBanco.Count(x => x.Activo == true && x.EstaSeleccionado == true);
            }

            aBanco.ListaReactivosBanco = ListaReactivosBanco;
            return aBanco;
        }
        private void DoUpdate()
        {
            try
            {
                UpdateEstadoSeleccionReactivosBanco();
                try
                {
                    ValidateBancoReactivos();
                }
                catch (Exception ex)
                {
                    this.ShowMessage(ex.Message, MessageType.Error);
                    return;
                }
                ABancoReactivo aBanco = UserInterfaceToData();
                dctx = ConnectionHlp.Default.Connection;
                catalogoPruebaCtrl.UpdateBancoReactivosPrueba(dctx, aBanco, LastBancoReactivos);
                ClearSession();
                Response.Redirect("BuscarPruebas.aspx");
            }
            catch (Exception ex)
            {
                Logger.Service.LoggerHlp.Default.Error(this, ex);
                throw new Exception("Ocurrió un error al configurar el banco de reactivos");
            }
        }
        private void UpReactivoBanco()
        {

            if (hdnBancoSelectedRow.Value != string.Empty)
            {
                GridViewRow rowSelected = (from GridViewRow row in grdBancoReactivos.Rows let hdnRowAux = (HiddenField)row.FindControl("hdnReactivoID") where hdnBancoSelectedRow.Value == hdnRowAux.Value select row).FirstOrDefault();
                if (rowSelected != null)
                {
                    int rowindex = rowSelected.RowIndex;
                    if (rowindex > 0)
                    {
                        GridViewRow rowAux = grdBancoReactivos.Rows[rowindex - 1];
                        HiddenField hdnRowAuxReactivoID = (HiddenField)rowAux.FindControl("hdnReactivoID");

                        ReactivoBanco reactivoBanco = new ReactivoBanco { Reactivo = new Reactivo { ReactivoID = Guid.Parse(hdnBancoSelectedRow.Value) } };
                        ReactivoBanco reactivoBancoAux = new ReactivoBanco { Reactivo = new Reactivo { ReactivoID = Guid.Parse(hdnRowAuxReactivoID.Value) } };
                        reactivoBanco = ListaReactivosBanco.FirstOrDefault(x => (x.Activo == true && x.Reactivo.ReactivoID == reactivoBanco.Reactivo.ReactivoID));
                        reactivoBancoAux = ListaReactivosBanco.FirstOrDefault(x => x.Activo == true && x.Reactivo.ReactivoID == reactivoBancoAux.Reactivo.ReactivoID);
                        if (reactivoBanco != null && reactivoBancoAux != null)
                        {
                            int index = ListaReactivosBanco.FindIndex(x => (x.Activo == true && x.Reactivo.ReactivoID == reactivoBanco.Reactivo.ReactivoID));
                            int indexAux = ListaReactivosBanco.FindIndex(x => (x.Activo == true && x.Reactivo.ReactivoID == reactivoBancoAux.Reactivo.ReactivoID));

                            int? orden = reactivoBanco.Orden;
                            int? ordenAux = reactivoBancoAux.Orden;
                            reactivoBanco.Orden = ordenAux;
                            reactivoBancoAux.Orden = orden;

                            ListaReactivosBanco.RemoveAt(index);
                            ListaReactivosBanco.RemoveAt(indexAux);
                            ListaReactivosBanco.Insert(indexAux, reactivoBanco);
                            ListaReactivosBanco.Insert(index, reactivoBancoAux);
                            grdBancoReactivos.DataSource = ListaReactivosBanco.Where(x => x.Activo == true).ToList();
                            grdBancoReactivos.DataBind();
                        }
                        grdBancoReactivos.SelectRow(rowindex - 1);
                    }
                }

            }
        }
        private void DownReactivoBanco()
        {
            if (hdnBancoSelectedRow.Value != string.Empty)
            {
                GridViewRow rowSelected = (from GridViewRow row in grdBancoReactivos.Rows let hdnRowAux = (HiddenField)row.FindControl("hdnReactivoID") where hdnBancoSelectedRow.Value == hdnRowAux.Value select row).FirstOrDefault();
                int indexmax = grdBancoReactivos.Rows.Count;

                if (rowSelected != null)
                {
                    int rowindex = rowSelected.RowIndex;
                    if (rowindex < indexmax)
                    {
                        GridViewRow rowAux = grdBancoReactivos.Rows[rowindex + 1];
                        HiddenField hdnRowAuxReactivoID = (HiddenField)rowAux.FindControl("hdnReactivoID");

                        ReactivoBanco reactivoBanco = new ReactivoBanco { Reactivo = new Reactivo { ReactivoID = Guid.Parse(hdnBancoSelectedRow.Value) } };
                        ReactivoBanco reactivoBancoAux = new ReactivoBanco { Reactivo = new Reactivo { ReactivoID = Guid.Parse(hdnRowAuxReactivoID.Value) } };
                        reactivoBanco = ListaReactivosBanco.FirstOrDefault(x => (x.Activo == true && x.Reactivo.ReactivoID == reactivoBanco.Reactivo.ReactivoID));
                        reactivoBancoAux = ListaReactivosBanco.FirstOrDefault(x => x.Activo == true && x.Reactivo.ReactivoID == reactivoBancoAux.Reactivo.ReactivoID);
                        if (reactivoBanco != null && reactivoBancoAux != null)
                        {
                            int index = ListaReactivosBanco.FindIndex(x => (x.Activo == true && x.Reactivo.ReactivoID == reactivoBanco.Reactivo.ReactivoID));
                            int indexAux = ListaReactivosBanco.FindIndex(x => (x.Activo == true && x.Reactivo.ReactivoID == reactivoBancoAux.Reactivo.ReactivoID));

                            int? orden = reactivoBanco.Orden;
                            int? ordenAux = reactivoBancoAux.Orden;
                            reactivoBanco.Orden = ordenAux;
                            reactivoBancoAux.Orden = orden;

                            ListaReactivosBanco.RemoveAt(indexAux);
                            ListaReactivosBanco.Insert(indexAux, reactivoBanco);
                            ListaReactivosBanco.RemoveAt(index);
                            ListaReactivosBanco.Insert(index, reactivoBancoAux);


                            grdBancoReactivos.DataSource = ListaReactivosBanco.Where(x => x.Activo == true).ToList();
                            grdBancoReactivos.DataBind();
                        }
                        // grdBancoReactivos.SelectRow(rowindex +1);
                    }
                }

            }
        }
        private void ClearSession()
        {
            LastObject = null;
            LastBancoReactivos = null;
            ListaReactivosBanco = null;
            ListaReactivosModelo = null;
            DsReactivosModelo = null;
            CountReactivos = 0;
            CountSelectedReactivos = 0;
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
            List<Permiso> permisos = userSession.CurrentPrivilegios.GetPermisos();

            bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOCATPRUEBAS) != null;
            bool edit = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POEDITARCATPRUEBA) != null;

            if (!acceso)
                redirector.GoToHomePage(true);
            if (!edit)
                redirector.GoToHomePage(true);
        }

        #endregion
    }
}