using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Framework.Base.DataAccess;
using POV.Seguridad.BO;
using POV.Seguridad.Service;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.Localizacion.Service;
using POV.Comun.BO;
using POV.Comun.Service;
using POV.Logger.Service;
using POV.Localizacion.BO;
using POV.Web.PortalUniversidad.Helper;
using POV.CentroEducativo.Services;
using POV.Core.Universidades.Interfaces;
using POV.Core.Universidades.Implements;

namespace POV.Web.PortalUniversidad.Pages
{
    public partial class EditarPerfil : System.Web.UI.Page
    {
        private IUserSession userSession;
        private IRedirector redirector;
        private UsuarioCtrl usuarioCtrl;
        private UniversidadCtrl universidadCtrl;
        private EscuelaCtrl escuelaCtrl;
        private UbicacionCtrl ubicacionCtrl;
        private PaisCtrl paisCtrl;
        private EstadoCtrl estadoCtrl;
        private CiudadCtrl ciudadCtrl;
        private readonly IDataContext dctx = ConnectionHlp.Default.Connection;

        public EditarPerfil()
        {
            userSession = new UserSession();
            redirector = new Redirector();
            usuarioCtrl = new UsuarioCtrl();
            universidadCtrl = new UniversidadCtrl(null);
            escuelaCtrl = new EscuelaCtrl();
            ubicacionCtrl = new UbicacionCtrl();
            paisCtrl = new PaisCtrl();
            estadoCtrl = new EstadoCtrl();
            ciudadCtrl = new CiudadCtrl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (userSession.IsLogin())                    
                        LlenarCamposPerfil();                    
                    else
                        redirector.GoToLoginPage(true);
                }
            }
            catch (Exception ex)
            {
                LoggerHlp.Default.Error(this, ex);
            }
        }

        private void LlenarCamposPerfil()
        {
            Usuario usuarioSession = userSession.CurrentUser;
            Universidad universidadSession = userSession.CurrentUniversidad;

            #region ***Llenar información del universidad***
            //llenar información de universidad 
            Universidad universidad = universidadCtrl.Retrieve(new Universidad { UniversidadID = universidadSession.UniversidadID }, false).FirstOrDefault();

            this.txtNombre.Text = universidad.NombreUniversidad;
            this.txtDirecion.Text = universidad.Direccion;
            this.txtPaginaWeb.Text = universidad.PaginaWEB;
            this.TxtDescripcion.Text = universidad.Descripcion;
            this.txtSiglas.Text = universidad.Siglas;

            LoadUbicacion(universidad);

            #endregion

            #region ***LLenar información de usuario***
            //LLenar la informacion del perfil del usuario
            this.LblNombreUsuario.Text = universidad.NombreUniversidad;

            //llenar email y celular
            Usuario usuario = usuarioCtrl.LastDataRowToUsuario(usuarioCtrl.Retrieve(dctx, new Usuario { UsuarioID = usuarioSession.UsuarioID }));
            this.TxtUsuario.Text = usuario.NombreUsuario;
            this.TxtEmail.Text = usuario.Email;
            this.txtCorreoEdit.Text = usuario.Email;
            this.TxtTelefono.Text = usuario.TelefonoReferencia;
            this.txtTelefonoEdit.Text = usuario.TelefonoReferencia;
            #endregion
        }

        private string ValidateFieldsForInsert()
        {
            String error = "";

            if (TxtEmail.Text.Trim().Length > 50)
                error += "El correo excede el límite permitido: 50, ";
            if (TxtTelefono.Text.Trim().Length > 20)
                error += "El teléfono excede el límite permitido: 20, ";

            return error;
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            redirector.GoToHomePage(false);
        }


        protected void btnGuardarCambios_Click(object sender, EventArgs e)
        {
            //validamos los campos de entrada...
            String error = ValidateFieldsForInsert();
            error += UniversidadValidateData();
            error += UniversidadUbicacionValidateData();
            //si la validacion es incorrecta...
            if (error.Length > 0)
                lblError.Text = error;
            else
            {
                GuardarPerfil();
            }
        }

        private string UniversidadValidateData()
        {
            //Campos Requeridos

            string sError = string.Empty;

            //Valores Requeridos.
            if (txtNombre.Text.Trim().Length <= 0)
                sError += " ,Nombre";
            if (txtDirecion.Text.Trim().Length <= 0)
                sError += " ,Dirección";

            if (CbPais.SelectedIndex == 0)
                sError += " ,País";
            if (CbEstado.SelectedIndex == 0)
                sError += " ,Estado";
            if (CbMunicipio.SelectedIndex == 0)
                sError += " ,Municipio";

            if (sError.Trim().Length > 0)
            {
                sError = sError.Substring(2);
                return sError = (string.Format("Los siguientes parámetros son requeridos: {0}", sError));
            }

            //Valores Incorrectos
            if (txtNombre.Text.Trim().Length > 250)
                sError += " ,Nombre";
            if (txtDirecion.Text.Trim().Length > 500)
                sError += " ,Dirección";
            if (TxtTelefono.Text.Trim().Length > 50)
                sError += " ,Teléfono";
            if (TxtDescripcion.Text.Trim().Length > 254)
                sError += " ,Descripción";
            if (txtSiglas.Text.Trim().Length > 15)
                sError += " ,Nombre corto";

            if (sError.Trim().Length > 0)
            {
                sError = sError.Substring(2);
                return sError = (string.Format("Los siguientes parámetros son inválidos: {0}", sError));
            }

            return sError;
        }

        private string UniversidadUbicacionValidateData()
        {
            string sError = string.Empty;
            if ((CbPais.SelectedIndex > 0) && CbEstado.SelectedIndex == 0)
            {
                sError += "Estado";
                return sError = (string.Format("El siguiente parámetro es requerido: {0}", sError));
            }

            return sError;
        }

        private void GuardarPerfil()
        {
            object myFirm = new object();
            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);
            try
            {
                //informacion de usuario seguridad
                if (!string.IsNullOrEmpty(TxtEmail.Text.Trim()))
                {
                    DataSet dsUsuarioEmail = usuarioCtrl.Retrieve(dctx, new Usuario { Email = TxtEmail.Text.Trim(), EsActivo = true });

                    if (dsUsuarioEmail.Tables[0].Rows.Count > 0)
                    {
                        Usuario usuarioTemp = usuarioCtrl.LastDataRowToUsuario(dsUsuarioEmail);

                        if (usuarioTemp.UsuarioID != userSession.CurrentUser.UsuarioID)
                            throw new Exception("Ya existe un usuario con el mismo correo electrónico, por favor introduce otro.");
                    }
                }

                if (!string.IsNullOrEmpty(TxtTelefono.Text.Trim()))
                {
                    DataSet dsUsuarioTelefono = usuarioCtrl.Retrieve(dctx, new Usuario { TelefonoReferencia = TxtTelefono.Text.Trim() });

                    if (dsUsuarioTelefono.Tables[0].Rows.Count > 0)
                    {
                        Usuario usuarioTemp = usuarioCtrl.LastDataRowToUsuario(dsUsuarioTelefono);

                        if (usuarioTemp.UsuarioID != userSession.CurrentUser.UsuarioID)
                            throw new Exception("Ya existe un usuario con el mismo teléfono, por favor introduce otro.");
                    }
                }

                List<bool> datosCompletos = new List<bool>();
                Universidad universidad = universidadCtrl.Retrieve(new Universidad { UniversidadID = userSession.CurrentUniversidad.UniversidadID }, true).FirstOrDefault();
                universidad.NombreUniversidad = this.txtNombre.Text;
                universidad.Direccion = this.txtDirecion.Text;
                universidad.Descripcion = this.TxtDescripcion.Text;
                universidad.PaginaWEB = this.txtPaginaWeb.Text;
                universidad.Siglas = this.txtSiglas.Text;

                #region insertar ubicacion
                //Ubicación
                Ubicacion ubicacion = new Ubicacion();
                ubicacion.Pais = new Pais { PaisID = CbPais.SelectedIndex > 0 ? int.Parse(CbPais.SelectedItem.Value) : (int?)null };
                ubicacion.Estado = new Estado { EstadoID = CbEstado.SelectedIndex > 0 ? int.Parse(CbEstado.SelectedItem.Value) : (int?)null };
                ubicacion.Ciudad = new Ciudad { CiudadID = CbMunicipio.SelectedIndex > 0 ? int.Parse(CbMunicipio.SelectedItem.Value) : (int?)null };

                DataSet dsUbicacion = ubicacionCtrl.RetrieveExacto(dctx, ubicacion);
                int index = dsUbicacion.Tables["Ubicacion"].Rows.Count;
                if (index == 1)
                    ubicacion = ubicacionCtrl.LastDataRowToUbicacion(dsUbicacion);
                //si no existe se inserta la ubicacion
                if (ubicacion.UbicacionID == null)
                {
                    if ((ubicacion.Pais.PaisID != null) && (universidad.Ubicacion.Estado.EstadoID != null))
                    {
                        ubicacion.FechaRegistro = DateTime.Now;
                        ubicacionCtrl.Insert(dctx, ubicacion);
                        universidad.UbicacionID = (long)ubicacionCtrl.LastDataRowToUbicacion(ubicacionCtrl.RetrieveExacto(dctx, ubicacion)).UbicacionID;
                    }
                }
                else
                    universidad.UbicacionID = (long)ubicacion.UbicacionID;
                #endregion

                Usuario usuario = usuarioCtrl.LastDataRowToUsuario(usuarioCtrl.Retrieve(dctx, new Usuario { UsuarioID = userSession.CurrentUser.UsuarioID }));
                Usuario usuarioClone = (Usuario)usuario.Clone();
                usuarioClone.Email = this.TxtEmail.Text.Trim();
                usuarioClone.TelefonoReferencia = this.TxtTelefono.Text.Trim();

                //Actualizar usuario
                usuarioCtrl.Update(dctx, usuarioClone, usuario);

                //Actualizar universidad
                universidadCtrl.Update(universidad);

                dctx.CommitTransaction(myFirm);

                userSession.CurrentUniversidad = universidadCtrl.RetrieveWithRelationship(new Universidad { UniversidadID = userSession.CurrentUniversidad.UniversidadID }, false).FirstOrDefault();
                userSession.CurrentUser = usuarioCtrl.LastDataRowToUsuario(usuarioCtrl.Retrieve(dctx, new Usuario { UsuarioID = userSession.CurrentUser.UsuarioID }));

                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(myFirm);

                System.Web.HttpContext.Current.Response.AddHeader("Pragma", "no-cache");
                System.Web.HttpContext.Current.Response.AddHeader("Pragma", "no-store");
                System.Web.HttpContext.Current.Response.Cache.SetNoStore();
                System.Web.HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                System.Web.HttpContext.Current.Response.Redirect("~/Default.aspx");
            }
            catch (Exception ex)
            {
                LoggerHlp.Default.Error(this, ex);
                dctx.RollbackTransaction(myFirm);
                dctx.CloseConnection(myFirm);
                lblError.Text = ex.Message;
            }
            finally
            {
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(myFirm);
            }
        }

        #region ***Información de ubicación***

        #region ***Cargar Ubicación***

        private void LoadUbicacion(Universidad universidad)
        {
            if (universidad.UbicacionID != null)
            {
                Ubicacion ubicacion = new Ubicacion();
                ubicacion.UbicacionID = universidad.UbicacionID;
                ubicacion = ubicacionCtrl.LastDataRowToUbicacion(ubicacionCtrl.Retrieve(dctx, ubicacion));

                universidad.Ubicacion = ubicacion;


                LoadPaises(new Ubicacion { Pais = new Pais { PaisID = universidad.Ubicacion.Pais.PaisID } });
                CbPais.SelectedValue = universidad.Ubicacion.Pais != null ? universidad.Ubicacion.Pais.PaisID.ToString() : null;

                LoadEstados(new Ubicacion { Estado = new Estado { Pais = universidad.Ubicacion.Pais } });
                CbEstado.SelectedValue = universidad.Ubicacion.Estado != null ? universidad.Ubicacion.Estado.EstadoID.ToString() : null;

                LoadCiudades(new Ubicacion { Ciudad = new Ciudad { Estado = universidad.Ubicacion.Estado } });
                CbMunicipio.SelectedValue = universidad.Ubicacion.Ciudad != null ? universidad.Ubicacion.Ciudad.CiudadID.ToString() : null;
            }

            else
            {
                LoadPaises(new Ubicacion { Pais = new Pais() });
            }
        }

        #endregion

        #region ***PAÍS***
        protected void CbPais_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (CbPais.SelectedIndex > 0)
                {
                    LoadEstados(new Ubicacion { Estado = new Estado { Pais = new Pais { PaisID = int.Parse(CbPais.SelectedItem.Value) } } });
                }
                else
                {
                    CbEstado.ClearSelection();
                    CbMunicipio.ClearSelection();
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "reloadUbicacion(1);", true);
            }
            catch (Exception ex)
            {

                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
            }
        }
        #endregion

        #region ***ESTADO***
        protected void CbEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (CbEstado.SelectedIndex > 0)
                {
                    LoadCiudades(new Ubicacion { Ciudad = new Ciudad { Estado = new Estado { EstadoID = int.Parse(CbEstado.SelectedItem.Value) } } });
                }
                else
                {
                    CbMunicipio.ClearSelection();
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "reloadUbicacion(2);", true);
            }
            catch (Exception ex)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
            }
        }

        private void LoadEstados(Ubicacion filter)
        {
            if (filter == null || filter.Estado == null)
                return;
            DataSet ds = estadoCtrl.Retrieve(ConnectionHlp.Default.Connection, filter.Estado);
            CbEstado.DataSource = ds;
            CbEstado.DataValueField = "EstadoID";
            CbEstado.DataTextField = "Nombre";
            CbEstado.DataBind();
            CbEstado.Items.Insert(0, new ListItem("", ""));
        }
        #endregion


        protected void CbMunicipio_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "reloadUbicacion(3);", true);
            }
            catch (Exception ex)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
            }
        }

        private void LoadPaises(Ubicacion filter)
        {
            if (filter == null || filter.Pais == null)
                return;
            DataSet ds = paisCtrl.Retrieve(ConnectionHlp.Default.Connection, filter.Pais);
            CbPais.DataSource = ds;
            CbPais.DataValueField = "PaisID";
            CbPais.DataTextField = "Nombre";
            CbPais.DataBind();
            CbPais.Items.Insert(0, new ListItem("", ""));

        }

        private void LoadCiudades(Ubicacion filter)
        {
            if (filter == null || filter.Ciudad == null)
                return;
            DataSet ds = ciudadCtrl.Retrieve(ConnectionHlp.Default.Connection, filter.Ciudad);
            CbMunicipio.DataSource = ds;
            CbMunicipio.DataValueField = "CiudadID";
            CbMunicipio.DataTextField = "Nombre";
            CbMunicipio.DataBind();
            CbMunicipio.Items.Insert(0, new ListItem("", ""));
        }
        #endregion
    }
}