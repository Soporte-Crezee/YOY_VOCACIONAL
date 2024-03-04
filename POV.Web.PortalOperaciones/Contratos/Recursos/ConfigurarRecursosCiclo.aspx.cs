using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Web.PortalOperaciones.Helper;
using POV.Licencias.BO;
using POV.Licencias.Service;
using POV.Prueba.BO;
using POV.Prueba.Diagnostico.Service;
using POV.Operaciones.Service;
using POV.Logger.Service;
using POV.CentroEducativo.Service;
using POV.CentroEducativo.BO;
namespace POV.Web.PortalOperaciones.Contratos.Recursos
{
    public partial class ConfigurarRecursosCiclo : System.Web.UI.Page
    {
        private CicloContratoCtrl cicloContratoCtrl;
        private ContratoCtrl contratoCtrl;
        private CatalogoPruebaCtrl catalogoPruebaCtrl;
        private IDataContext dctx = ConnectionHlp.Default.Connection;
        private RecursoContratoCtrl recursoContratoCtrl;
        private CicloContrato LastObject
        {
            get { return Session["lastCicloContrato"] != null ? (CicloContrato)Session["lastCicloContrato"] : null; }
            set { Session["lastCicloContrato"] = value; }
        }

        private Contrato SS_LastObject
        {
            set { Session["lastContrato"] = value; }
            get { return Session["lastContrato"] != null ? (Contrato)Session["lastContrato"] : null; }
        }

        private DataTable SS_RecursosPruebas
        {
            get { return (DataTable)this.Session["dsRecursoPruebas"]; }
            set { this.Session["dsRecursoPruebas"] = value; }
        }

        private DataTable SS_RecursosPaqueteJuegos
        {
            get { return (DataTable)this.Session["dsRecursoPaqueteJuegos"]; }
            set { this.Session["dsRecursoPaqueteJuegos"] = value; }
        }

        public ConfigurarRecursosCiclo()
        {
            cicloContratoCtrl = new CicloContratoCtrl();
            catalogoPruebaCtrl = new CatalogoPruebaCtrl();
            contratoCtrl = new ContratoCtrl();
            recursoContratoCtrl = new RecursoContratoCtrl();
        }

        #region *** eventos de pagina ***
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (SS_LastObject == null || LastObject == null)
                {
                    txtRedirect.Value = "../BuscarContrato.aspx";
                    ShowMessage("No se ha seleccionado un elemento", MessageType.Error);
                    return;
                }

                if (SS_LastObject != null && SS_LastObject.ContratoID == null)
                {
                    txtRedirect.Value = "../BuscarContrato.aspx";
                    ShowMessage("No se ha seleccionado un elemento", MessageType.Error);
                    return;
                }

                if (LastObject != null && LastObject.CicloContratoID == null)
                {
                    txtRedirect.Value = "../BuscarContrato.aspx";
                    ShowMessage("No se ha seleccionado un elemento", MessageType.Error);
                    return;
                }

                DataToUserInterface();
            }
        }

        protected void BtnActualizarConfig_OnClick(object sender, EventArgs e)
        {
            try
            {

                string sError = string.Empty;

                CicloContrato cicloContrato = cicloContratoCtrl.RetrieveComplete(dctx, SS_LastObject, new CicloContrato
                {
                    CicloContratoID = LastObject.CicloContratoID
                });

                RecursoContrato recursoContrato = cicloContrato.RecursoContrato.CloneAll() as RecursoContrato;

                recursoContrato.EsPaquetePorPruebaPivote = false;

                recursoContrato.EsAsignacionManual = false;

                recursoContratoCtrl.Update(dctx, recursoContrato, cicloContrato.RecursoContrato);

                DataToUserInterface();
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message, MessageType.Error);
            }
        }

        protected void BtnCancelarConfig_OnClick(object sender, EventArgs e)
        {

        }

        protected void BtnEditarConfig_OnClick(object sender, EventArgs e)
        {
            CicloContrato cicloContrato = cicloContratoCtrl.RetrieveComplete(dctx, SS_LastObject, new CicloContrato { CicloContratoID = LastObject.CicloContratoID });
        }

        protected void BtnCancelar_OnClick(object sender, EventArgs e)
        {
            LimpiarObjetosSesion();
            Response.Redirect("~/CiclosEscolares/BuscarCicloEscolar.aspx", true);
        }

        protected void GrdViewPruebaPivote_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName.Trim())
            {
                case "eliminar":
                    try
                    {
                        PruebaContratoCtrl pruebaContratoCtrl = new PruebaContratoCtrl();
                        //validamos si ay juegos asigandos al contrato, no se puede eliminar la prueba pivote
                        pruebaContratoCtrl.DeletePruebaContrato(dctx, new PruebaContrato { PruebaContratoID = Convert.ToInt64(e.CommandArgument.ToString()) });
                        DataToUserInterface();
                    }
                    catch (Exception ex)
                    {
                        ShowMessage(ex.Message, MessageType.Error);
                    }
                    break;
                default: ShowMessage("No se encontró el comando.", MessageType.Error); break;
            }
        }

        protected void GrdViewPruebas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName.Trim())
            {
                case "eliminar":
                    try
                    {
                        CalendarizacionPruebaGrupoCtrl calendarizacionPruebaGrupoCtrl = new CalendarizacionPruebaGrupoCtrl();

                        GrupoCicloEscolarCtrl grupoCicloEscolarCtrl = new GrupoCicloEscolarCtrl();
                        GrupoCicloEscolar grupoCicloEscolarActual = new GrupoCicloEscolar() { CicloEscolar = LastObject.CicloEscolar };
                        GrupoCicloEscolar gce = grupoCicloEscolarCtrl.LastDataRowToGrupoCicloEscolar(grupoCicloEscolarCtrl.Retrieve(dctx, grupoCicloEscolarActual));
                        GrupoCicloEscolar grupoCicloEscolar = grupoCicloEscolarCtrl.RetriveComplete(ConnectionHlp.Default.Connection, new GrupoCicloEscolar { GrupoCicloEscolarID = gce.GrupoCicloEscolarID, Escuela = gce.Escuela });

                        CalendarizacionPruebaGrupo calendarizacion = new CalendarizacionPruebaGrupo()
                        {
                            ConVigencia = false,
                            Activo = true,
                            GrupoCicloEscolar = grupoCicloEscolar,
                            PruebaContrato = new PruebaContrato(){PruebaContratoID = Convert.ToInt64(e.CommandArgument.ToString())}
                        };

                        var aux = calendarizacionPruebaGrupoCtrl.LastDataRowToCalendarizacionPruebaGrupo(calendarizacionPruebaGrupoCtrl.Retrieve(dctx, calendarizacion));

                        calendarizacionPruebaGrupoCtrl.Delete(dctx, calendarizacionPruebaGrupoCtrl.LastDataRowToCalendarizacionPruebaGrupo(calendarizacionPruebaGrupoCtrl.Retrieve(dctx, calendarizacion)));

                        PruebaContratoCtrl pruebaContratoCtrl = new PruebaContratoCtrl();
                        pruebaContratoCtrl.DeletePruebaContrato(dctx, new PruebaContrato { PruebaContratoID = Convert.ToInt64(e.CommandArgument.ToString()) });
                        DataToUserInterface();
                    }
                    catch (Exception ex)
                    {
                        ShowMessage(ex.Message, MessageType.Error);
                    }
                    break;
                default: ShowMessage("No se encontró el comando.", MessageType.Error); break;
            }
        }
        #endregion

        #region *** validaciones ***
        #endregion

        #region *** Data to UserInterface ***

        private void DataToUserInterface()
        {
            Contrato contrato = contratoCtrl.LastDataRowToContrato(contratoCtrl.Retrieve(dctx, new Contrato { ContratoID = SS_LastObject.ContratoID }));
            LastObject = cicloContratoCtrl.RetrieveComplete(dctx, contrato, new CicloContrato
            {
                CicloContratoID = LastObject.CicloContratoID
            });
            RecursoContrato recurso = LastObject.RecursoContrato;

            TxtNombreCiclo.Text = LastObject.CicloEscolar.Titulo;
            TxtInicioCiclo.Text = String.Format("{0:dd/MM/yyyy}", LastObject.CicloEscolar.InicioCiclo);
            TxtFinCiclo.Text = String.Format("{0:dd/MM/yyyy}", LastObject.CicloEscolar.FinCiclo);

            PruebaContrato pruebaPivote = recurso.PruebasContrato.FirstOrDefault(item => item.TipoPruebaContrato == ETipoPruebaContrato.Pivote);
            //tiene prueba pivote
            if (pruebaPivote != null)
            {
                HpLinkAsignarPruebaPivote.Visible = false;
                LoadPruebaPivote(pruebaPivote);
            }
            else //no tiene prueba pivote
            {
                HpLinkAsignarPruebaPivote.Visible = true;
                LoadPruebaPivote(null);
            }

            LoadGridPruebas(recurso.PruebasContrato.ToList());
        }

        private void LoadPruebaPivote(PruebaContrato pruebaContrato)
        {
            DataTable dtPruebaPivote = new DataTable();
            dtPruebaPivote.Columns.Add("PruebaContratoID", typeof(long));
            dtPruebaPivote.Columns.Add("PruebaID", typeof(long));
            dtPruebaPivote.Columns.Add("Clave", typeof(string));
            dtPruebaPivote.Columns.Add("Nombre", typeof(string));
            dtPruebaPivote.Columns.Add("Modelo", typeof(string));

            if (pruebaContrato != null && pruebaContrato.Prueba != null && pruebaContrato.Prueba.PruebaID != null)
            {
                APrueba prueba = pruebaContrato.Prueba;
                prueba = catalogoPruebaCtrl.RetrieveComplete(dctx, prueba, false);
                if (prueba != null)
                {
                    DataRow dr = dtPruebaPivote.NewRow();
                    dr[0] = pruebaContrato.PruebaContratoID;
                    dr[1] = prueba.PruebaID;
                    dr[2] = prueba.Clave;
                    dr[3] = prueba.Nombre;
                    dr[4] = prueba.Modelo.Nombre;
                    dtPruebaPivote.Rows.Add(dr);
                }
            }

            GrdViewPruebaPivote.DataSource = dtPruebaPivote;
            GrdViewPruebaPivote.DataBind();
        }

        private void LoadGridPruebas(List<PruebaContrato> pruebas)
        {
            DataTable dtPruebasDiagnosticas = new DataTable();
            dtPruebasDiagnosticas.Columns.Add("PruebaContratoID", typeof(long));
            dtPruebasDiagnosticas.Columns.Add("PruebaID", typeof(long));
            dtPruebasDiagnosticas.Columns.Add("Clave", typeof(string));
            dtPruebasDiagnosticas.Columns.Add("Nombre", typeof(string));
            dtPruebasDiagnosticas.Columns.Add("Modelo", typeof(string));
            foreach (PruebaContrato pruebaItem in pruebas)
            {
                if (pruebaItem.TipoPruebaContrato != ETipoPruebaContrato.Pivote)
                {
                  
                    APrueba prueba = catalogoPruebaCtrl.RetrieveComplete(dctx, pruebaItem.Prueba, false);
                    if (prueba != null)
                    {
                        DataRow dr = dtPruebasDiagnosticas.NewRow();
                        dr[0] = pruebaItem.PruebaContratoID;
                        dr[1] = prueba.PruebaID;
                        dr[2] = prueba.Clave;
                        dr[3] = prueba.Nombre;
                        dr[4] = prueba.Modelo.Nombre;
                        dtPruebasDiagnosticas.Rows.Add(dr);
                    }
                }
            }
            SS_RecursosPruebas = dtPruebasDiagnosticas;
            GrdViewPruebas.DataSource = dtPruebasDiagnosticas;
            GrdViewPruebas.DataBind();
        }

        #endregion

        #region *** UserInterface to Data ***
        #endregion

        #region *** metodos auxiliares ***
        private void LimpiarObjetosSesion()
        {

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