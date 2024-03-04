using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Drawing;
using GP.SocialEngine.BO;
using GP.SocialEngine.Service;
using Framework.Base.DataAccess;
using POV.Seguridad.BO;
using POV.Seguridad.Service;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.Localizacion.Service;
using POV.Comun.BO;
using POV.Comun.Service;
using POV.Logger.Service;

using System.Configuration;
using POV.Localizacion.BO;
using System.Globalization;
using POV.Core.PadreTutor.Interfaces;
using POV.Core.PadreTutor.Implements;
using POV.Web.PortalTutor.Helper;
using POV.CentroEducativo.Services;
using System.Text.RegularExpressions;

namespace POV.Web.PortalTutor.Pages
{
    public partial class EditarPerfil : System.Web.UI.Page
    {

        private IUserSession userSession;
        private InformacionSocialCtrl informacionSocialCtrl;
        private IRedirector redirector;
        private UsuarioCtrl usuarioCtrl;
        private AlumnoCtrl alumnoCtrl;
        private EscuelaCtrl escuelaCtrl;
        private UbicacionCtrl ubicacionCtrl;
        private PaisCtrl paisCtrl;
        private EstadoCtrl estadoCtrl;
        private CiudadCtrl ciudadCtrl;
        private readonly IDataContext dctx = ConnectionHlp.Default.Connection;
        private TutorCtrl tutorCtrl;

        public EditarPerfil()
        {
            userSession = new UserSession();
            informacionSocialCtrl = new InformacionSocialCtrl();
            redirector = new Redirector();
            usuarioCtrl = new UsuarioCtrl();
            alumnoCtrl = new AlumnoCtrl();
            escuelaCtrl = new EscuelaCtrl();
            ubicacionCtrl = new UbicacionCtrl();
            paisCtrl = new PaisCtrl();
            estadoCtrl = new EstadoCtrl();
            ciudadCtrl = new CiudadCtrl();
            tutorCtrl = new TutorCtrl(null);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (userSession.IsLogin())
                    {
                        if ((bool)userSession.CurrentTutor.CorreoConfirmado)
                        {
                            LblNombreUsuario.Text = userSession.CurrentTutor.Nombre + " " + userSession.CurrentTutor.PrimerApellido;
                            LlenarCamposPerfil();
                        }
                        else
                            redirector.GoToHomePage(true);
                    }
                    else
                        redirector.GoToLoginPage(true);
                }
                else
                {
                    if (!userSession.IsLogin()) //es tutor
                    {
                        redirector.GoToLoginPage(true);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHlp.Default.Error(this, ex);
                throw ex;
            }
        }

        private void LlenarCamposPerfil()
        {
            Usuario usuarioSession = userSession.CurrentUser;
            Tutor tutorSession = userSession.CurrentTutor;

            #region ***Llenar información del tutor***
            //llenar información de alumno 
            Tutor tutor = tutorCtrl.Retrieve(new Tutor { TutorID = tutorSession.TutorID }, false).FirstOrDefault();
            this.txtNombre.Text = tutor.Nombre;
            this.txtPrimerApellido.Text = tutor.PrimerApellido;
            this.txtSegundoApellido.Text = tutor.SegundoApellido;
            this.txtFechaNacimiento.Text = string.Format("{0:dd/MM/yyyy}", tutor.FechaNacimiento);
            this.txtDirecion.Text = tutor.Direccion;
            this.TxtEmail.Text = tutor.CorreoElectronico;

            if (tutor.Sexo != null)
                CbSexo.SelectedValue = (bool)tutor.Sexo ? "True" : "False";

            LoadUbicacion(tutor);

            #endregion

            #region ***LLenar información de usuario***

            //llenar email y celular
            Usuario usuario = usuarioCtrl.LastDataRowToUsuario(usuarioCtrl.Retrieve(dctx, new Usuario { UsuarioID = usuarioSession.UsuarioID }));
            this.TxtUsuario.Text = usuario.NombreUsuario;
            this.TxtEmail.Text = usuario.Email;
            this.TxtTelefono.Value = usuario.TelefonoReferencia;
            this.TxtTelefonoCasa.Value = usuario.TelefonoCasa;

            #endregion
        }

        private string ValidateFieldsForInsert()
        {

            String error = "";

            if (TxtEmail.Text.Trim().Length > 100)
                error += "El correo excede el límite permitido: 100, ";
            if (TxtTelefono.Value.Trim().Length > 20)
                error += "El teléfono excede el límite permitido: 20, ";
            if (TxtTelefonoCasa.Value.Trim().Length > 20)
                error += "El teléfono excede el límite permitido: 20, ";

            return error;
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            System.Web.HttpContext.Current.Response.Redirect("~/Default.aspx");
        }


        protected void btnGuardarCambios_Click(object sender, EventArgs e)
        {
            //validamos los campos de entrada...
            String error = ValidateFieldsForInsert();
            error += AlumnoValidateData();
            error += AlumnoUbicacionValidateData();
            //si la validacion es incorrecta...
            if (error.Length > 0)
            {
                lblError.Text = error;
            }
            else
            {
                GuardarPerfil();
            }
        }

        private string AlumnoValidateData()
        {
            string sError = string.Empty;

            //Valores Requeridos.
            if (txtNombre.Text.Trim().Length <= 0)
                sError += " ,Nombre";
            if (txtPrimerApellido.Text.Trim().Length <= 0)
                sError += " ,Primer apellido";
            if (CbSexo.SelectedIndex == -1 || CbSexo.SelectedValue.Length <= 0)
                sError += " ,Sexo";
            if (TxtTelefono.Value.Length <= 0)
                sError += " ,Teléfono celular";

            if (sError.Trim().Length > 0)
            {
                sError = sError.Substring(2);
                return sError = (string.Format("Los siguientes parámetros son requeridos: {0}", sError));
            }

            //Valores Incorrectos
            if (txtNombre.Text.Trim().Length > 80)
                sError += " ,Nombre";
            if (txtPrimerApellido.Text.Trim().Length > 50)
                sError += " ,Primer apellido";
            if (TxtTelefono.Value.Trim().Length < 13)
                sError += " ,El teléfono celular es menor a 10 dígitos";
            if (!string.IsNullOrEmpty(TxtTelefonoCasa.Value.Trim()))
            {
                if (TxtTelefonoCasa.Value.Trim().Length < 13)
                    sError += " ,El teléfono de casa es menor a 10 dígitos";
            }

            if (sError.Trim().Length > 0)
            {
                sError = sError.Substring(2);
                return sError = (string.Format("Los siguientes parámetros son inválidos: {0}", sError));
            }

            //Formatos Inválidos
            DateTime fn;
            if (!DateTime.TryParseExact(txtFechaNacimiento.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out fn))
                sError += " ,Fecha nacimiento";

            if (!ValidateEmailRegex(TxtEmail.Text.Trim()))
                sError += " ,Correo electrónico";

            if (sError.Trim().Length > 0)
            {
                sError = sError.Substring(2);
                return sError = (string.Format("Los siguientes parámetros tienen un formato no válido: {0}", sError));
            }
            return sError;
        }

        private bool ValidateEmailRegex(string email)
        {
            string patternLenient = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
            Regex reLenient = new Regex(patternLenient);
            bool match = reLenient.IsMatch(email);
            return match;
        }

        private string AlumnoUbicacionValidateData()
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

                if (!string.IsNullOrEmpty(TxtTelefono.Value.Trim()))
                {
                    DataSet dsUsuarioTelefono = usuarioCtrl.Retrieve(dctx, new Usuario { TelefonoReferencia = TxtTelefono.Value.Trim() });

                    if (dsUsuarioTelefono.Tables[0].Rows.Count > 0)
                    {
                        Usuario usuarioTemp = usuarioCtrl.LastDataRowToUsuario(dsUsuarioTelefono);

                        if (usuarioTemp.UsuarioID != userSession.CurrentUser.UsuarioID)
                            throw new Exception("Ya existe un usuario con el mismo teléfono, por favor introduce otro.");
                    }
                }

                bool boolval;
                List<bool> datosCompletos = new List<bool>();

                Tutor tutor = tutorCtrl.Retrieve(new Tutor { TutorID = userSession.CurrentTutor.TutorID }, true)[0];
                tutor.Nombre = this.txtNombre.Text.Trim();
                tutor.PrimerApellido = this.txtPrimerApellido.Text.Trim();
                tutor.SegundoApellido = this.txtSegundoApellido.Text.Trim();
                if (CbSexo.SelectedIndex != -1 && !string.IsNullOrEmpty(CbSexo.SelectedItem.Value))
                    if (bool.TryParse(CbSexo.SelectedValue, out boolval)) tutor.Sexo = boolval;
                tutor.FechaNacimiento = DateTime.ParseExact(this.txtFechaNacimiento.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                tutor.Direccion = this.txtDirecion.Text.Trim();
                tutor.CorreoElectronico = this.TxtEmail.Text.Trim();
                tutor.Telefono = this.TxtTelefonoCasa.Value.Trim();


                #region insertar ubicacion
                //Ubicación
                tutor.Ubicacion = new Ubicacion();
                tutor.Ubicacion.Pais = new Pais { PaisID = CbPais.SelectedIndex > 0 ? int.Parse(CbPais.SelectedItem.Value) : (int?)null };
                tutor.Ubicacion.Estado = new Estado { EstadoID = CbEstado.SelectedIndex > 0 ? int.Parse(CbEstado.SelectedItem.Value) : (int?)null };
                tutor.Ubicacion.Ciudad = new Ciudad { CiudadID = CbMunicipio.SelectedIndex > 0 ? int.Parse(CbMunicipio.SelectedItem.Value) : (int?)null };

                Ubicacion ubicacion = new Ubicacion();
                DataSet dsUbicacion = ubicacionCtrl.RetrieveExacto(dctx, tutor.Ubicacion);
                int index = dsUbicacion.Tables["Ubicacion"].Rows.Count;
                if (index == 1)
                    ubicacion = ubicacionCtrl.LastDataRowToUbicacion(dsUbicacion);
                //si no existe se inserta la ubicacion
                if (ubicacion.UbicacionID == null)
                {
                    if ((tutor.Ubicacion.Pais.PaisID != null) && (tutor.Ubicacion.Estado.EstadoID != null))
                    {
                        tutor.Ubicacion.FechaRegistro = DateTime.Now;
                        ubicacionCtrl.Insert(dctx, tutor.Ubicacion);
                        tutor.UbicacionID = ubicacionCtrl.LastDataRowToUbicacion(ubicacionCtrl.RetrieveExacto(dctx, tutor.Ubicacion)).UbicacionID;
                    }
                }
                else
                    tutor.UbicacionID = ubicacion.UbicacionID;
                #endregion



                Usuario usuario = usuarioCtrl.LastDataRowToUsuario(usuarioCtrl.Retrieve(dctx, new Usuario { UsuarioID = userSession.CurrentUser.UsuarioID }));
                Usuario usuarioClone = (Usuario)usuario.Clone();
                usuarioClone.Email = this.TxtEmail.Text.Trim();
                usuarioClone.TelefonoReferencia = this.TxtTelefono.Value.Trim();
                usuarioClone.TelefonoCasa = this.TxtTelefonoCasa.Value.Trim();

                //Datos Completos 
                datosCompletos.Add(!(string.IsNullOrEmpty(usuarioClone.Email.ToString())) ? true : false);
                datosCompletos.Add(!(string.IsNullOrEmpty(usuarioClone.TelefonoReferencia.ToString())) ? true : false);
                datosCompletos.Add(!(string.IsNullOrEmpty(tutor.FechaNacimiento.ToString())) ? true : false);
                datosCompletos.Add(!(string.IsNullOrEmpty(tutor.Ubicacion.Pais.PaisID.ToString())) ? true : false);
                datosCompletos.Add(!(string.IsNullOrEmpty(tutor.Ubicacion.Estado.EstadoID.ToString())) ? true : false);
                datosCompletos.Add(!(string.IsNullOrEmpty(tutor.Ubicacion.Ciudad.CiudadID.ToString())) ? true : false);

                //Actualizar tutor
                tutor.DatosCompletos = (datosCompletos.Contains(false)) ? false : true;

                //Actualizar usuario

                if ((bool)tutor.DatosCompletos)
                {
                    usuarioCtrl.Update(dctx, usuarioClone, usuario);
                    tutorCtrl.Update(tutor);
                    dctx.CommitTransaction(myFirm);
                    // Se recarga la sesion
                    TutorCtrl updSession = new TutorCtrl(null);
                    userSession.CurrentTutor = updSession.Retrieve(tutor, false).FirstOrDefault();
                    userSession.CurrentUser = usuarioCtrl.LastDataRowToUsuario(usuarioCtrl.Retrieve(dctx, new Usuario { UsuarioID = userSession.CurrentUser.UsuarioID }));
                    System.Web.HttpContext.Current.Response.AddHeader("Pragma", "no-cache");
                    System.Web.HttpContext.Current.Response.AddHeader("Pragma", "no-store");
                    System.Web.HttpContext.Current.Response.Cache.SetNoStore();
                    System.Web.HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    System.Web.HttpContext.Current.Response.Redirect("~/Default.aspx", true);
                }

                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "messageError('error');", true);
                }
            }
            catch (Exception ex)
            {
                LoggerHlp.Default.Error(this, ex);
                dctx.RollbackTransaction(myFirm);
                dctx.CloseConnection(myFirm);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "messageError('" + ex.Message + "');", true);
            }
            finally
            {
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(myFirm);
            }
        }

        #region ***Información de ubicación***

        #region ***Cargar Ubicación***

        private void LoadUbicacion(Tutor tutor)
        {
            if (tutor.UbicacionID != null)
            {
                Ubicacion ubicacion = new Ubicacion();
                ubicacion.UbicacionID = tutor.UbicacionID;
                ubicacion = ubicacionCtrl.LastDataRowToUbicacion(ubicacionCtrl.Retrieve(dctx, ubicacion));

                tutor.Ubicacion = ubicacion;


                LoadPaises(new Ubicacion { Pais = new Pais { PaisID = tutor.Ubicacion.Pais.PaisID } });
                CbPais.SelectedValue = tutor.Ubicacion.Pais != null ? tutor.Ubicacion.Pais.PaisID.ToString() : null;

                LoadEstados(new Ubicacion { Estado = new Estado { Pais = tutor.Ubicacion.Pais } });
                CbEstado.SelectedValue = tutor.Ubicacion.Estado != null ? tutor.Ubicacion.Estado.EstadoID.ToString() : null;

                LoadCiudades(new Ubicacion { Ciudad = new Ciudad { Estado = tutor.Ubicacion.Estado } });
                CbMunicipio.SelectedValue = tutor.Ubicacion.Ciudad != null ? tutor.Ubicacion.Ciudad.CiudadID.ToString() : null;
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
                    CbEstado.DataSource = null;
                    CbEstado.DataBind();
                    CbEstado.ClearSelection();
                    CbEstado.Items.Clear();
                    CbMunicipio.DataSource = null;
                    CbMunicipio.DataBind();
                    CbMunicipio.ClearSelection();
                    CbMunicipio.Items.Clear();
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
                    CbMunicipio.DataSource = null;
                    CbMunicipio.DataBind();
                    CbMunicipio.Items.Clear();
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

        private byte[] GetImageStream(HttpPostedFile myFile)
        {
            byte[] myData = null;
            int nFileLen = myFile.ContentLength;
            myData = new byte[nFileLen];
            myFile.InputStream.Read(myData, 0, nFileLen);
            return myData;
        }

        // Writes file to current folder
        private void WriteToFile(string path, ref byte[] Buffer)
        {
            // Create a file
            FileStream newFile = new FileStream(path, FileMode.Create);

            // Write data to the file
            newFile.Write(Buffer, 0, Buffer.Length);

            // Close file
            newFile.Close();
        }

        public static byte[] CropImageToScuare(byte[] imageFile)
        {
            System.Drawing.Image original = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageFile));
            int pixels = 0;
            int positionX = 0;
            int positionY = 0;
            if (original.Height > original.Width)
            {
                pixels = original.Width;
                positionY = (int)((float)(original.Height - original.Width) / (float)2);
            }
            else
            {
                pixels = original.Height;
                positionX = (int)((float)(original.Width - original.Height) / (float)2);
            }
            Bitmap bmpImage = new Bitmap(original);

            Rectangle cropArea = new Rectangle(positionX, positionY, pixels, pixels);

            Bitmap bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);

            System.IO.MemoryStream mm = new System.IO.MemoryStream();
            bmpCrop.Save(mm, System.Drawing.Imaging.ImageFormat.Png);
            bmpImage.Dispose();
            bmpCrop.Dispose();

            return mm.GetBuffer();
        }

        public static byte[] ResizeImageFile(byte[] imageFile, int targetSize)
        {
            System.Drawing.Image original = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageFile));
            int targetH, targetW;
            if (original.Height > original.Width)
            {
                targetH = targetSize;
                targetW = (int)(original.Width * ((float)targetSize / (float)original.Height));
            }
            else
            {
                targetW = targetSize;
                targetH = (int)(original.Height * ((float)targetSize / (float)original.Width));
            }
            System.Drawing.Image imgPhoto = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageFile));
            //Create a new blank canvas.  The resized image will be drawn on this canvas.
            Bitmap bmPhoto = new Bitmap(targetW, targetH, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(90, 90);
            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            grPhoto.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            grPhoto.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            grPhoto.DrawImage(imgPhoto, new Rectangle(0, 0, targetW, targetH), 0, 0, original.Width, original.Height, GraphicsUnit.Pixel);
            //Save out to memory and then to a file.  We dispose of all objects to make sure the files don't stay locked.
            System.IO.MemoryStream mm = new System.IO.MemoryStream();
            bmPhoto.Save(mm, System.Drawing.Imaging.ImageFormat.Png);
            original.Dispose();
            imgPhoto.Dispose();
            bmPhoto.Dispose();
            grPhoto.Dispose();
            return mm.GetBuffer();
        }
    }
}