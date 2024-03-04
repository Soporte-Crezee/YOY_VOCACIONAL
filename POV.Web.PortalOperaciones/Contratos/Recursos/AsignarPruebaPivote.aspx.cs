using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Licencias.BO;
using POV.Licencias.Service;
using POV.Modelo.BO;
using POV.Modelo.Service;
using POV.Prueba.BO;
using POV.Prueba.Diagnostico.Dinamica.BO;
using POV.Prueba.Diagnostico.Dinamica.Service;
using POV.Prueba.Diagnostico.Service;


using POV.Seguridad.BO;
using POV.Web.PortalOperaciones.AppCode.Page;
using POV.Web.PortalOperaciones.Helper;

namespace POV.Web.PortalOperaciones.Contratos.Recursos
{
    public partial class AsignarPruebaPivote : PageBase
    {
        private CatalogoPruebaCtrl catalogoPruebaCtrl;
        private CicloContratoCtrl cicloContratoCtrl;
        private IDataContext dctx = ConnectionHlp.Default.Connection;

		public AsignarPruebaPivote()
		{
			catalogoPruebaCtrl = new CatalogoPruebaCtrl();
			cicloContratoCtrl = new CicloContratoCtrl();
		}

		#region *** Propiedades de Clase ***

		private Contrato SS_Contrato
		{
			set { Session["lastContrato"] = value; }
			get { return Session["lastContrato"] != null ? Session["lastContrato"] as Contrato : null; }
		}
		private CicloContrato SS_CicloContrato
		{
			set { Session["lastCicloContrato"] = value; }
			get { return Session["lastCicloContrato"] != null ? Session["lastCicloContrato"] as CicloContrato : null; }
		}


		#endregion
		#region *** EVentos de página ***

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{								
				if (SS_Contrato != null && SS_Contrato.ContratoID == null)
				{
					txtRedirect.Value = "../BuscarContrato.aspx";
					ShowMessage("No se ha seleccionado un elemento", MessageType.Error);
					return;
				}

				if (SS_CicloContrato != null && SS_CicloContrato.CicloContratoID == null)
				{
					txtRedirect.Value = "../BuscarContrato.aspx";
					ShowMessage("No se ha seleccionado un elemento", MessageType.Error);
					return;
				}

				LoadDatosCicloEscolar();
				LoadModelos();				
			}
		}

		protected void btnBuscar_Click(object sender, EventArgs e)
		{
			APrueba prueba = UserIntefaceToData();
			LoadPruebas(prueba);
		}
		
		protected void grdPruebasPivote_RowCommand(object sender, GridViewCommandEventArgs e)
		{
			switch (e.CommandName.Trim())
			{
				case "AsigPrueba":
					{
						try
						{
							DoInsert(Convert.ToInt32(e.CommandArgument.ToString()));
						}
						catch (Exception ex)
						{
							txtRedirect.Value = "~/Contratos/BuscarContrato.aspx";
							ShowMessage(ex.Message, MessageType.Error);
						}

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
		#region *** UserInterfaceToData ***

		private AModelo GetModeloFromUI()
		{
			AModelo modelo = null;

			int modeloID = 0;
			string valorModelo = ddlModelo.SelectedValue;

			if (int.TryParse(valorModelo, out modeloID))
			{
				ModeloCtrl modeloCtrl = new ModeloCtrl();
				Dictionary<string, string> parametros = new Dictionary<string, string>();
				parametros.Add("Activo", "true");
				DataSet dsModelo = modeloCtrl.Retrieve(dctx, modeloID, parametros);
				if (dsModelo.Tables[0].Rows.Count > 0)
					modelo = modeloCtrl.LastDataRowToModelo(dsModelo);
			}
			return modelo;
		}
		private APrueba UserIntefaceToData()
		{
			APrueba prueba = null;

			AModelo modelo = GetModeloFromUI();

			if (modelo is ModeloDinamico)
				prueba = new PruebaDinamica();

			prueba.Modelo = modelo;

			if (txtNombre.Text.Trim().Length > 0)
				prueba.Nombre = string.Format("%{0}%", txtNombre.Text.Trim());
			if (txtClave.Text.Trim().Length > 0)
				prueba.Clave = txtClave.Text.Trim();
			if (ddlTipoPrueba.SelectedIndex > 0)
			{
				if (ddlTipoPrueba.SelectedItem.Value == "true")
					prueba.EsDiagnostica = true;
				if (ddlTipoPrueba.SelectedItem.Value == "false")
					prueba.EsDiagnostica = false;
			}
			if (ddlEstadoPrueba.SelectedIndex > 0)
			{
				prueba.EstadoLiberacionPrueba = (EEstadoLiberacionPrueba)byte.Parse(ddlEstadoPrueba.SelectedValue);
			}

			return prueba;
		}
		
		#endregion
		#region *** DataToUserInterface ***

		private void LoadDatosCicloEscolar()
		{
			if (SS_CicloContrato != null && SS_CicloContrato.CicloEscolar != null)
			{
				if (SS_CicloContrato.CicloEscolar.Titulo != null)
					txtNombreCiclo.Text = SS_Contrato.Clave;
				if (SS_CicloContrato.CicloEscolar.InicioCiclo != null)
					txtFechaInicioCiclo.Text = string.Format("{0:dd/MM/yyyy}", (DateTime)SS_CicloContrato.CicloEscolar.InicioCiclo);
				if (SS_CicloContrato.CicloEscolar.FinCiclo != null)
					txtFechaFinCiclo.Text = string.Format("{0:dd/MM/yyyy}", (DateTime)SS_CicloContrato.CicloEscolar.FinCiclo);
			}
		}

		private void LoadModelos()
		{
			ModeloCtrl modeloCtrl = new ModeloCtrl();
			Dictionary<string, string> parametros = new Dictionary<string, string>();
			parametros.Add("Activo", "true");
			parametros.Add("ModeloDiagnostico", "true");

			DataSet dsModelos = modeloCtrl.Retrieve(dctx, null, parametros);

			ddlModelo.DataSource = dsModelos;
			ddlModelo.DataTextField = "Nombre";
			ddlModelo.DataValueField = "ModeloID";
			ddlModelo.SelectedIndex = 0;
			ddlModelo.DataBind();
		}

		private void LoadPruebas(APrueba prueba)
		{
			DataSet DsPruebas = catalogoPruebaCtrl.Retrieve(dctx, prueba, false);
			DsPruebas = ConfigureGridResults(DsPruebas);
			grdPruebasPivote.DataSource = DsPruebas;
			grdPruebasPivote.DataBind();
		}
		#endregion
		#region *** Métodos Auxiliares ***
		private DataSet ConfigureGridResults(DataSet ds)
		{
			ds.Tables[0].Columns.Add("NombreModelo");
			ds.Tables[0].Columns.Add("TipoPrueba");
			ds.Tables[0].Columns.Add("Estado");
			ModeloCtrl modeloCtrl = new ModeloCtrl();
			Dictionary<string, string> parametros = new Dictionary<string, string>();

			foreach (DataRow row in ds.Tables[0].Rows)
			{

				int modeloID = (int)Convert.ChangeType(row["ModeloID"], typeof(int));

				AModelo modelo = modeloCtrl.LastDataRowToModelo(modeloCtrl.Retrieve(dctx, modeloID, parametros));
				row["NombreModelo"] = modelo.Nombre;
				
				bool esDiagnostica = (bool)Convert.ChangeType(row["EsDiagnostica"], typeof(bool));
				if (esDiagnostica)
					row["TipoPrueba"] = "DIAGNOSTICA";
				else
					row["TipoPrueba"] = "FINAL";

				EEstadoLiberacionPrueba estadoLiberacion = (EEstadoLiberacionPrueba)Convert.ChangeType(row["EstadoLiberacion"], typeof(byte));
				row["Estado"] = estadoLiberacion.ToString();
			}
			return ds;
		}

		private void DoInsert(int pruebaID)
		{
			CicloContrato cicloContrato = cicloContratoCtrl.RetrieveComplete(dctx, SS_Contrato, new CicloContrato
			{
				CicloContratoID = SS_CicloContrato.CicloContratoID
			});
			RecursoContrato recurso = cicloContrato.RecursoContrato;

			PruebaContrato pruebaPivote = recurso.PruebasContrato.FirstOrDefault(item => item.TipoPruebaContrato == ETipoPruebaContrato.Pivote && item.Activo == true);
			//no tiene prueba pivote
			if (pruebaPivote == null)
			{

				DataSet dsPrueba = catalogoPruebaCtrl.Retrieve(dctx, pruebaID, null, null);
				DataRow drPrueba = dsPrueba.Tables[0].Rows[dsPrueba.Tables[0].Rows.Count - 1];
				
				byte tipoPrueba = (byte)Convert.ChangeType(drPrueba["Tipo"], typeof(byte));
				bool esDiagnostica = (bool)Convert.ChangeType(drPrueba["EsDiagnostica"], typeof(bool));

				if (!esDiagnostica)
					throw new Exception("Sólo se pueden asignar pruebas diagnósticas como prueba pivote al contrato.");

				APrueba prueba = null;
				if (tipoPrueba == (byte)ETipoPrueba.Dinamica)
    				prueba = new PruebaDinamica { PruebaID = pruebaID};

				//Cargar la prueba completa, para verificar puntajes
				prueba = catalogoPruebaCtrl.RetrieveComplete(dctx, prueba, true);
				
				if (prueba.EstadoLiberacionPrueba != EEstadoLiberacionPrueba.LIBERADA)
				{
					throw new Exception("La asignación de pruebas al contrato sólo admite pruebas con estado Liberado. Por favor, verifique");
				}				
				else
				{
					PruebaContrato pruebaContrato = new PruebaContrato { Prueba = prueba, Activo = true };
					PruebaContratoCtrl pruebaContratoCtrl = new PruebaContratoCtrl();

					if (pruebaContratoCtrl.Retrieve(dctx, recurso, pruebaContrato).Tables[0].Rows.Count > 0)
						throw new Exception("La prueba seleccionada ya se encuentra asignada al contrato.");
					else
					{
						pruebaContrato.FechaRegistro = DateTime.Now;
						pruebaContrato.TipoPruebaContrato = ETipoPruebaContrato.Pivote;
						
						pruebaContratoCtrl.InsertPruebaContrato(dctx, recurso, pruebaContrato);
						Response.Redirect("ConfigurarRecursosCiclo.aspx");
					}
				}
			}
			else
			{
				txtRedirect.Value = "~/Contratos/BuscarContrato.aspx";
				ShowMessage("La prueba pivote ya está asignada al contrato, por favor seleccione otro", MessageType.Information);
			}
		}

		private bool tieneBancoReactivos(APrueba prueba)
		{
			bool lflag = false;

			switch (prueba.TipoPrueba)
			{
 				case ETipoPrueba.Dinamica:
					BancoReactivosDinamicoCtrl bancoReactivosDinamicoCtrl = new BancoReactivosDinamicoCtrl();
					lflag = (bancoReactivosDinamicoCtrl.Retrieve(dctx, new BancoReactivosDinamico { Prueba = prueba }).Tables[0].Rows.Count > 0);
					break;
				default:
					lflag = false;
					break;
			}

			return lflag;
		}
		#endregion
		#region *** Autorizacion de la pagina ***
		protected override void AuthorizeUser()
		{
			List<Permiso> permisos = userSession.CurrentPrivilegios.GetPermisos();

			bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOCONTRATO) != null;
			bool edicion = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POEDITARCONTRATOS) != null;

			if (!acceso)
				redirector.GoToHomePage(true);
			if (!edicion)
				redirector.GoToHomePage(true);
		}
        #endregion
        #region *** Mostrar Mensajes ***
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
    }
}