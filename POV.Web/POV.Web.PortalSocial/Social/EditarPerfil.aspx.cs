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
using POV.Core.RedSocial.Interfaces;
using POV.Core.RedSocial.Implement;
using Framework.Base.DataAccess;
using POV.Seguridad.BO;
using POV.Seguridad.Service;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.Localizacion.Service;
using POV.Comun.BO;
using POV.Comun.Service;
using POV.Web.PortalSocial.AppCode;
using POV.Logger.Service;

using System.Configuration;
using POV.Localizacion.BO;
using POV.Web.Administracion.Helper;
using System.Globalization;
using POV.CentroEducativo.Services;
using POV.Modelo.Context;
using POV.Licencias.Service;
using POV.Expediente.BO;
using POV.Expediente.Services;
using POV.Seguridad.Utils;
using System.Data.Entity.Validation;
using System.Text.RegularExpressions;

namespace POV.Web.PortalSocial.Social
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
        private readonly IDataContext dctx = ConnectionHelper.Default.Connection;
        private string pathImage = @"~/Files/ImagenUsuario/Normal/";
        private string pathThumbs = @"~/Files/ImagenUsuario/Thumbs/";

        private UsuarioExpedienteCtrl usuarioExpedienteCtrl;
        private LicenciaEscuelaCtrl licenciaEscuelaCtrl;
        Int64? nTutorID;
        string sCorreoElectronico;

        private TutorAlumno TutorSeleccionado
        {
            get { return (TutorAlumno)this.Session["TUTORSELECT"]; }
            set { this.Session["TUTORSELECT"] = value; }

        }

        private List<TutorAlumno> Session_Tutores
        {
            get { return (List<TutorAlumno>)this.Session["LIST_TUTOR"]; }
            set { this.Session["LIST_TUTOR"] = value; }
        }

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

            usuarioExpedienteCtrl = new UsuarioExpedienteCtrl();
            licenciaEscuelaCtrl = new LicenciaEscuelaCtrl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (userSession.IsLogin())
                {
                    EFAlumnoCtrl alumnoCtrl = new EFAlumnoCtrl(null);
                    Alumno alumno = alumnoCtrl.Retrieve(new Alumno { AlumnoID = userSession.CurrentAlumno.AlumnoID }, false).FirstOrDefault();

                    if ((bool)alumno.CorreoConfirmado)
                        LlenarCamposPerfil();
                    else
                        redirector.GoToHomeAlumno(true);
                }
                else
                    redirector.GoToLoginPage(true);
            }
        }

        private void LlenarCamposPerfil()
        {
            UsuarioSocial usuarioSocial = userSession.CurrentUsuarioSocial;
            SocialHub socialHub = userSession.SocialHub;
            Usuario usuarioSession = userSession.CurrentUser;
            Alumno alumnoSession = userSession.CurrentAlumno;
            InformacionSocialCtrl informacionSocialCtrl = new InformacionSocialCtrl();



            #region ***Llenar información del alumno***
            //llenar información de alumno 
            EFAlumnoCtrl efAlumnoCtrl = new EFAlumnoCtrl(null);

            Alumno alumno = alumnoCtrl.LastDataRowToAlumno(alumnoCtrl.Retrieve(dctx, new Alumno { AlumnoID = alumnoSession.AlumnoID }));
            Alumno alumnoOrientador = efAlumnoCtrl.Retrieve(alumno, false).First();

            this.txtNombre.Text = alumno.Nombre;
            this.txtPrimerApellido.Text = alumno.PrimerApellido;
            this.txtSegundoApellido.Text = alumno.SegundoApellido;
            this.txtFechaNacimiento.Text = string.Format("{0:dd/MM/yyyy}", alumno.FechaNacimiento);
            this.TxtEscuela.Text = alumno.Escuela;
            this.ddlNivelEstudio.SelectedValue = alumno.Grado != null ? ((byte)alumno.Grado).ToString() : null;
            orientadorAsignado.Visible = false;

            #region Docente Asignado
            Docente docente = new Docente();
            OrientadorUniversidad orientador;
            List<OrientadorUniversidad> listaOrientador = new List<OrientadorUniversidad>();
            DataSet ds = usuarioExpedienteCtrl.Retrieve(dctx, new UsuarioExpediente { AlumnoID = alumno.AlumnoID });
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    UsuarioExpediente usExp = usuarioExpedienteCtrl.DataRowToUsuarioExpediente(ds.Tables[0].Rows[i]);
                    orientador = new OrientadorUniversidad();
                    orientador = licenciaEscuelaCtrl.RetrieveUsuarioOrientadorUniversidad(dctx, new Usuario { UsuarioID = usExp.UsuarioID });
                    if (orientador.DocenteID != null)
                    {
                        listaOrientador.Add(orientador);
                    }
                }
            }
            if (listaOrientador.Count > 0)
            {
                txtOrientador.Visible = false;
                orientadorAsignado.Visible = true;
                grdOrientadores.DataSource = listaOrientador;
                grdOrientadores.DataBind();
            }
            else
            {
                txtOrientador.Text = "Sin orientador";
            }
            #endregion

            if (alumno.RecibirInformacion != null)
                ChbInfo.Checked = (bool)alumno.RecibirInformacion ? true : false;

            if (alumno.Sexo != null)
                CbSexo.SelectedValue = (bool)alumno.Sexo ? "True" : "False";

            LoadUbicacion(alumno);

            #endregion

            #region ***LLenar información de usuario***
            //LLenar la informacion del perfil del usuario
            this.LblNombreUsuario.Text = usuarioSocial.ScreenName;

            //llenar email y celular
            Usuario usuario = usuarioCtrl.LastDataRowToUsuario(usuarioCtrl.Retrieve(dctx, new Usuario { UsuarioID = usuarioSession.UsuarioID }));

            this.TxtUsuario.Text = usuario.NombreUsuario;
            this.TxtTelefono.Value = usuario.TelefonoReferencia;
            this.TxtTelefonoCasa.Value = usuario.TelefonoCasa;

            this.TxtEmail.Text = usuario.Email;


            //firma del usuario
            InformacionSocial informacionSocial = new InformacionSocial { InformacionSocialID = socialHub.InformacionSocial.InformacionSocialID };

            informacionSocial = informacionSocialCtrl.LastDataRowToInformacionSocial(informacionSocialCtrl.Retrieve(dctx, informacionSocial));

            this.TxtFirma.Text = string.IsNullOrEmpty(informacionSocial.Firma) ? "" : informacionSocial.Firma;

            this.ImgUser.ImageUrl = UrlHelper.GetImagenPerfilURL("normal", (long)usuarioSocial.UsuarioSocialID);
            #endregion

            #region LLenar Tutores vinculados
            Session_Tutores = (efAlumnoCtrl.Retrieve(userSession.CurrentAlumno, true).FirstOrDefault()).Tutores.ToList();
            grdTutores.DataSource = null;
            grdTutores.DataSource = Session_Tutores;
            grdTutores.DataBind();

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

            HttpPostedFile myFile = filMyFile.PostedFile;
            //limite de 2mb
            int lengthLimit = 2097152;
            if (myFile != null && myFile.ContentLength > lengthLimit)
                error += "El tamaño de la imagen debe ser menor de 2MB.";
            else if (myFile != null && myFile.ContentLength > 0)
            {
                string filename = myFile.FileName;
                string extension = filename.Substring(filename.LastIndexOf('.') + 1).ToLower();


                switch (extension)
                {
                    case "gif":
                    case "jpg":
                    case "jpeg":
                    case "bmp":
                    case "png":
                        break;
                    default:
                        error += "El archivo debe ser una imagen.";
                        break;
                }
            }
            return error;
        }

        protected void btnGuardarCambios_Click(object sender, EventArgs e)
        {
            //validamos los campos de entrada...
            String error = ValidateFieldsForInsert();
            error += AlumnoValidateData();
            error += AlumnoUbicacionValidateData();
            error += ValidateOtraInformacion();
            //si la validacion es incorrecta...
            if (error.Length > 0)
                lblError.Text = error;
            else
            {
                GuardarPerfil();
            }
        }

        private string AlumnoValidateData()
        {
            //Campos Requeridos

            string sError = string.Empty;

            //Valores Requeridos.
            if (txtNombre.Text.Trim().Length <= 0)
                sError += " ,Nombre";
            if (txtPrimerApellido.Text.Trim().Length <= 0)
                sError += " ,Primer Apellido";
            if (CbSexo.SelectedIndex == -1 || CbSexo.SelectedValue.Length <= 0)
                sError += " ,Sexo";
            if (ddlNivelEstudio.SelectedIndex == -1)
                sError += " ,Grado";

            ImagenPerfilCtrl imagenPerfilCtrl = new ImagenPerfilCtrl();
            //creamos la Imagen del perfil....
            ImagenPerfil imagenPerfil = new ImagenPerfil();
            imagenPerfil.AdjuntoImagen = new AdjuntoImagen();
            DataSet ds = imagenPerfilCtrl.Retrieve(dctx, userSession.CurrentUsuarioSocial, imagenPerfil);
            //Si no tiene Imagen de perfil...
            if (ds.Tables["ImagenPerfil"].Rows.Count <= 0)
            {
                HttpPostedFile myFile = filMyFile.PostedFile;
                //limite de 2mb
                //int lengthLimit = 2097152;
                if (string.IsNullOrEmpty(myFile.FileName))
                    sError += " ,Imagen de Perfil";
            }

            if (sError.Trim().Length > 0)
            {
                sError = sError.Substring(2);
                return sError = (string.Format("Los siguientes parámetros son requeridos: {0}", sError));
            }

            //Valores Incorrectos
            if (txtNombre.Text.Trim().Length > 80)
                sError += " ,Nombre";
            if (txtPrimerApellido.Text.Trim().Length > 50)
                sError += " ,Primer Apellido";
            if (txtSegundoApellido.Text.Trim().Length > 50)
                sError += " ,Segundo Apellido";
            if (TxtEscuela.Text.Trim().Length > 250)
                sError += " ,Escuela";
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
                sError += " ,Fecha Nacimiento";

            if (!ValidateEmailRegex(TxtEmail.Text.Trim()))
                sError += " ,Correo Electrónico";

            if (sError.Trim().Length > 0)
            {
                sError = sError.Substring(2);
                return sError = (string.Format("Los siguientes parámetros tienen un formato no valido: {0}", sError));
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
            if (CbPais.SelectedIndex == 0 || CbPais.SelectedValue.Length <= 0)
            {
                sError += " ,País";
                return sError = (string.Format("El siguiente parámetro es requerido: {0}", sError));
            }

            if ((CbPais.SelectedIndex > 0) && CbEstado.SelectedIndex == 0)
            {
                sError += "Estado";
                return sError = (string.Format("El siguiente parámetro es requerido: {0}", sError));
            }
            if ((CbEstado.SelectedIndex > 0) && CbMunicipio.SelectedIndex == 0)
            {
                sError += "Municipio";
                return sError = (string.Format("El siguiente parámetro es requerido: {0}", sError));
            }

            return sError;
        }

        private string ValidateOtraInformacion()
        {
            string sError = string.Empty;

            //Valores Incorrectos
            if (TxtFirma.Text.Trim().Length > 200)
                sError += " ,Estado de perfil";

            if (sError.Trim().Length > 0)
            {
                sError = sError.Substring(2);
                return sError = (string.Format("Los siguientes parámetros son inválidos: {0}", sError));
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

                List<bool> datosCompletos = new List<bool>();
                Alumno alumno = alumnoCtrl.LastDataRowToAlumno(alumnoCtrl.Retrieve(dctx, new Alumno { AlumnoID = userSession.CurrentAlumno.AlumnoID }));
                Alumno alumnoClone = (Alumno)alumno.Clone();
                alumnoClone.SegundoApellido = this.txtSegundoApellido.Text.Trim();
                alumnoClone.FechaNacimiento = DateTime.ParseExact(this.txtFechaNacimiento.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                alumnoClone.Escuela = this.TxtEscuela.Text.Trim();
                alumnoClone.Grado = ddlNivelEstudio.SelectedIndex > 0 ? (EGrado?)byte.Parse(ddlNivelEstudio.SelectedItem.Value) : null;
                alumnoClone.RecibirInformacion = (bool)this.ChbInfo.Checked;

                #region insertar ubicacion
                //Ubicación
                alumnoClone.Ubicacion = new Ubicacion();
                alumnoClone.Ubicacion.Pais = new Pais { PaisID = CbPais.SelectedIndex > 0 ? int.Parse(CbPais.SelectedItem.Value) : (int?)null };
                alumnoClone.Ubicacion.Estado = new Estado { EstadoID = CbEstado.SelectedIndex > 0 ? int.Parse(CbEstado.SelectedItem.Value) : (int?)null };
                alumnoClone.Ubicacion.Ciudad = new Ciudad { CiudadID = CbMunicipio.SelectedIndex > 0 ? int.Parse(CbMunicipio.SelectedItem.Value) : (int?)null };

                Ubicacion ubicacion = new Ubicacion();
                DataSet dsUbicacion = ubicacionCtrl.RetrieveExacto(dctx, alumnoClone.Ubicacion);
                int index = dsUbicacion.Tables["Ubicacion"].Rows.Count;
                if (index == 1)
                    ubicacion = ubicacionCtrl.LastDataRowToUbicacion(dsUbicacion);
                //si no existe se inserta la ubicacion
                if (ubicacion.UbicacionID == null)
                {
                    if ((alumnoClone.Ubicacion.Pais.PaisID != null) && (alumnoClone.Ubicacion.Estado.EstadoID != null))
                    {
                        alumnoClone.Ubicacion.FechaRegistro = DateTime.Now;
                        ubicacionCtrl.Insert(dctx, alumnoClone.Ubicacion);
                        alumnoClone.Ubicacion = ubicacionCtrl.LastDataRowToUbicacion(ubicacionCtrl.RetrieveExacto(dctx, alumnoClone.Ubicacion));
                    }
                }
                else
                    alumnoClone.Ubicacion = ubicacion;
                #endregion



                Usuario usuario = usuarioCtrl.LastDataRowToUsuario(usuarioCtrl.Retrieve(dctx, new Usuario { UsuarioID = userSession.CurrentUser.UsuarioID }));
                Usuario usuarioClone = (Usuario)usuario.Clone();
                usuarioClone.Email = this.TxtEmail.Text.Trim();
                usuarioClone.TelefonoReferencia = this.TxtTelefono.Value.Trim();
                usuarioClone.TelefonoCasa = this.TxtTelefonoCasa.Value.Trim();

                //Actualizar usuario
                usuarioCtrl.Update(dctx, usuarioClone, usuario);

                //Datos Completos 
                datosCompletos.Add(!(string.IsNullOrEmpty(alumnoClone.FechaNacimiento.ToString())) ? true : false);
                datosCompletos.Add(!(string.IsNullOrEmpty(alumnoClone.Escuela.ToString())) ? true : false);
                datosCompletos.Add(!(string.IsNullOrEmpty(alumnoClone.Grado.ToString())) ? true : false);
                datosCompletos.Add(!(string.IsNullOrEmpty(alumnoClone.Ubicacion.Pais.PaisID.ToString())) ? true : false);
                datosCompletos.Add(!(string.IsNullOrEmpty(alumnoClone.Ubicacion.Estado.EstadoID.ToString())) ? true : false);
                datosCompletos.Add(!(string.IsNullOrEmpty(alumnoClone.Ubicacion.Ciudad.CiudadID.ToString())) ? true : false);
                datosCompletos.Add(!(string.IsNullOrEmpty(usuarioClone.Email.ToString())) ? true : false);
                datosCompletos.Add(!(string.IsNullOrEmpty(usuarioClone.TelefonoReferencia.ToString())) ? true : false);

                UsuarioSocial usuarioSocialUpdate = new UsuarioSocial();

                //actualizar informacion social
                InformacionSocial informacionSocialPrevious = new InformacionSocial { InformacionSocialID = userSession.SocialHub.InformacionSocial.InformacionSocialID };
                InformacionSocial informacionSocialUpdate = UserInterfaceToInformacionSocial();

                informacionSocialCtrl.Update(dctx, informacionSocialUpdate, informacionSocialPrevious);

                #region *** guardar/actualizar imagen de perfil
                HttpPostedFile imageFile = filMyFile.PostedFile;
                //si se selecciono una imagen de perfil.... 

                ImagenPerfilCtrl imagenPerfilCtrl = new ImagenPerfilCtrl();
                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    //nota: en este punto ya ha ocurrido una validacion por lo tanto estamos seguros que
                    //el archivo es una imagen de tamaño mayor que cero y menor del limite maximo de bytes.

                    FileManagerCtrl fileManager = new FileManagerCtrl();
                    AdjuntoImagenCtrl adjuntoImagenCtrl = new Comun.Service.AdjuntoImagenCtrl();

                    //creamos la Imagen del perfil....
                    ImagenPerfil imagenPerfil = new ImagenPerfil();
                    imagenPerfil.AdjuntoImagen = new AdjuntoImagen();

                    //consultamos si existe la imagen del perfil de usuario...
                    DataSet ds = imagenPerfilCtrl.Retrieve(dctx, userSession.CurrentUsuarioSocial, imagenPerfil);

                    AdjuntoImagen adjuntoImagen = new AdjuntoImagen();
                    string guidImagen = Guid.NewGuid().ToString("N");
                    string extension = imageFile.FileName.Substring(imageFile.FileName.LastIndexOf('.') + 1).ToLower();

                    //Si no tiene Imagen de perfil...
                    if (ds.Tables["ImagenPerfil"].Rows.Count <= 0)
                    {


                        adjuntoImagen.Extension = extension;
                        adjuntoImagen.NombreImagen = guidImagen + "." + extension;
                        adjuntoImagen.MIME = imageFile.ContentType;
                        adjuntoImagen.CarpetaID = POV.Comun.BO.ECarpetaSistema.IMAGENES_PERFIL;
                        adjuntoImagen.NombreThumb = guidImagen + "_thumb." + extension;

                        adjuntoImagenCtrl.Insert(dctx, adjuntoImagen);

                        adjuntoImagen = adjuntoImagenCtrl.LastDataRowToAdjuntoImagen(adjuntoImagenCtrl.Retrieve(dctx, adjuntoImagen));

                        imagenPerfil.AdjuntoImagen = adjuntoImagen;
                        imagenPerfil.Estatus = true;

                        imagenPerfilCtrl.Insert(dctx, userSession.CurrentUsuarioSocial, imagenPerfil);
                    }
                    else
                    {
                        imagenPerfil = imagenPerfilCtrl.LastDataRowToImagenPerfil(ds);

                        AdjuntoImagen adjuntoImagenAux = adjuntoImagenCtrl.LastDataRowToAdjuntoImagen(adjuntoImagenCtrl.Retrieve(dctx, imagenPerfil.AdjuntoImagen));

                        fileManager.DeleteFile(Server.MapPath(String.Format("{0}{1}", pathImage, adjuntoImagenAux.NombreImagen)));
                        fileManager.DeleteFile(Server.MapPath(String.Format("{0}{1}", pathThumbs, adjuntoImagen.NombreThumb)));

                        adjuntoImagen.AdjuntoImagenID = adjuntoImagenAux.AdjuntoImagenID;
                        adjuntoImagen.Extension = extension;
                        adjuntoImagen.NombreImagen = guidImagen + "." + extension;
                        adjuntoImagen.MIME = imageFile.ContentType;
                        adjuntoImagen.CarpetaID = POV.Comun.BO.ECarpetaSistema.IMAGENES_PERFIL;
                        adjuntoImagen.NombreThumb = guidImagen + "_thumb." + extension;
                        adjuntoImagenCtrl.Update(dctx, adjuntoImagen, adjuntoImagenAux);

                    }

                    byte[] imageBytes = GetImageStream(filMyFile.PostedFile);
                    byte[] imageThumbByte = GetImageStream(filMyFile.PostedFile);

                    //escribimos el archivo...
                    pathImage = @ConfigurationManager.AppSettings["POVCarpetaAvataresNormal"];
                    pathThumbs = @ConfigurationManager.AppSettings["POVCarpetaAvataresThumb"];

                    imageBytes = ResizeImageFile(imageBytes, 200);
                    fileManager.CreateFolder(Server.MapPath(pathImage));
                    fileManager.WriteFile(Server.MapPath(String.Format("{0}{1}", pathImage, adjuntoImagen.NombreImagen)), ref imageBytes);

                    imageBytes = CropImageToScuare(imageBytes);
                    imageBytes = ResizeImageFile(imageBytes, 60);
                    fileManager.CreateFolder(Server.MapPath(pathThumbs));
                    fileManager.WriteFile(Server.MapPath(String.Format("{0}{1}", pathThumbs, adjuntoImagen.NombreThumb)), ref imageBytes);

                }

                //consultamos si existe la imagen del perfil de usuario...
                if (imagenPerfilCtrl.Retrieve(dctx, userSession.CurrentUsuarioSocial, new ImagenPerfil() { }).Tables["ImagenPerfil"].Rows.Count <= 0)
                    datosCompletos.Add(false);

                #endregion

                #region *** Vincular Tutor-Alumno
                var objeto = new object();
                var contexto = new Contexto(objeto);

                EFAlumnoCtrl efAlumnoCtrl = new EFAlumnoCtrl(contexto);

                var alumnoSave = efAlumnoCtrl.Retrieve(userSession.CurrentAlumno, true).FirstOrDefault();

                TutorCtrl tutorCtrl = new TutorCtrl(contexto);
                TutorAlumnoCtrl tutorAlumnoCtrl = new TutorAlumnoCtrl(contexto);

                foreach (TutorAlumno tut in Session_Tutores.ToList())
                {
                    if (tut.TutorID != null)
                    {
                        TutorAlumno tutorAlumnoSelect = tutorAlumnoCtrl.Retrieve(tut, true).FirstOrDefault();
                        if (tutorAlumnoSelect != null)
                            tutorAlumnoSelect.Parentesco = tut.Parentesco;
                        else
                        {
                            tutorAlumnoSelect = new TutorAlumno();
                            tutorAlumnoSelect.AlumnoID = userSession.CurrentAlumno.AlumnoID;
                            tutorAlumnoSelect.TutorID = tut.TutorID;
                            tutorAlumnoSelect.Parentesco = tut.Parentesco;

                        }

                        alumnoSave.Tutores.Add(tutorAlumnoSelect);
                    }
                    else
                    {
                        tut.TutorID = tut.Tutor.TutorID;
                        tut.AlumnoID = userSession.CurrentAlumno.AlumnoID;
                        tut.Parentesco = tut.Tutor.Parentesco;
                        tut.Tutor.EstatusIdentificacion = false;
                        var alumnoTracker = tutorCtrl.Retrieve(tut.Tutor, true).FirstOrDefault();
                        if (alumnoTracker != null) tut.Tutor = alumnoTracker;

                        alumnoSave.Tutores.Add(tut);
                    }
                }
                efAlumnoCtrl.Update(alumnoSave);

                contexto.Commit(objeto);

                contexto.Dispose();

                #endregion

                //Actualizar alumno
                alumnoClone.DatosCompletos = (datosCompletos.Contains(false)) ? false : true;

                alumnoCtrl.Update(dctx, alumnoClone, alumno);

                dctx.CommitTransaction(myFirm);
                System.Web.HttpContext.Current.Response.AddHeader("Pragma", "no-cache");
                System.Web.HttpContext.Current.Response.AddHeader("Pragma", "no-store");
                System.Web.HttpContext.Current.Response.Cache.SetNoStore();
                System.Web.HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                System.Web.HttpContext.Current.Response.Redirect("~/PortalAlumno/Noticias.aspx", false);

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

        private void LoadUbicacion(Alumno alumno)
        {
            if (alumno.Ubicacion.UbicacionID != null)
            {
                alumno.Ubicacion = ubicacionCtrl.LastDataRowToUbicacion(ubicacionCtrl.Retrieve(dctx, alumno.Ubicacion));

                LoadPaises(new Ubicacion { Pais = new Pais { PaisID = alumno.Ubicacion.Pais.PaisID } });
                CbPais.SelectedValue = alumno.Ubicacion.Pais != null ? alumno.Ubicacion.Pais.PaisID.ToString() : null;

                LoadEstados(new Ubicacion { Estado = new Estado { Pais = alumno.Ubicacion.Pais } });
                CbEstado.SelectedValue = alumno.Ubicacion.Estado != null ? alumno.Ubicacion.Estado.EstadoID.ToString() : null;

                LoadCiudades(new Ubicacion { Ciudad = new Ciudad { Estado = alumno.Ubicacion.Estado } });
                CbMunicipio.SelectedValue = alumno.Ubicacion.Ciudad != null ? alumno.Ubicacion.Ciudad.CiudadID.ToString() : null;
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
            CbEstado.Items.Insert(0, new ListItem("", "0"));
        }

        #endregion

        #endregion



        private void LoadPaises(Ubicacion filter)
        {
            if (filter == null || filter.Pais == null)
                return;
            DataSet ds = paisCtrl.Retrieve(ConnectionHlp.Default.Connection, filter.Pais);
            CbPais.DataSource = ds;
            CbPais.DataValueField = "PaisID";
            CbPais.DataTextField = "Nombre";
            CbPais.DataBind();
            CbPais.Items.Insert(0, new ListItem("", "0"));

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
            CbMunicipio.Items.Insert(0, new ListItem("", "0"));
        }

        private InformacionSocial UserInterfaceToInformacionSocial()
        {
            InformacionSocial informacionSocial = new InformacionSocial();

            informacionSocial.Firma = this.TxtFirma.Text;

            return informacionSocial;
        }

        private HttpPostedFile LoadImage()
        {
            HttpPostedFile myFile = filMyFile.PostedFile;
            byte[] myData = null;

            // Check to see if file was uploaded
            if (myFile != null)
            {
                // Get size of uploaded file
                int nFileLen = myFile.ContentLength;
                // make sure the size of the file is > 0
                if (nFileLen > 0)
                {
                    // Allocate a buffer for reading of the file
                    myData = new byte[nFileLen];

                    // Read uploaded file from the Stream
                    myFile.InputStream.Read(myData, 0, nFileLen);

                }
                else
                {
                    myFile = null;
                }
            }

            return myFile;
        }

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

        protected void btnAceptarCodigo_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCodigoTutor.Text))
            {
                TutorCtrl tutorCtrl = new TutorCtrl(null);
                string strCodigo = txtCodigoTutor.Text;

                Tutor tutor = new Tutor() { Codigo = strCodigo };
                Tutor tutorSeleccionado = tutorCtrl.Retrieve(tutor, false).FirstOrDefault();

                if (tutorSeleccionado != null)
                {
                    ddlParentescoTutor.SelectedValue = "";
                    txtTutorID.Text = tutorSeleccionado.TutorID.ToString().Trim();
                    txtNombreTutor.Text = (tutorSeleccionado.Nombre + " " + tutorSeleccionado.PrimerApellido + " " + tutorSeleccionado.SegundoApellido).Trim();
                    LicenciaEscuelaCtrl licCtrl = new LicenciaEscuelaCtrl();
                    Usuario usr = licCtrl.RetrieveUsuarioTutor(dctx, tutorSeleccionado);
                    usr = usuarioCtrl.LastDataRowToUsuario(usuarioCtrl.Retrieve(dctx, usr));
                    txtDireccionTutor.Text = tutorSeleccionado.Direccion;
                    txtCorreoTutor.Text = usr.Email;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "viewModal();", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "mostrarAlerta('error', 'Código incorrecto', '" + txtCodigoTutor.ClientID + "');", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('Inserte código de tutor');", true);
            }
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            if (txtCodigoTutor.Text != string.Empty)
            {
                if (Session_Tutores == null)
                {
                    Session_Tutores = new List<TutorAlumno>();
                }

                TutorCtrl tutorCtrl = new TutorCtrl(null);
                TutorAlumnoCtrl tutorAlumnoCtrl = new TutorAlumnoCtrl(null);
                TutorAlumno tutorAlumnoFind = new TutorAlumno();
                tutorAlumnoFind.AlumnoID = userSession.CurrentAlumno.AlumnoID;
                tutorAlumnoFind.TutorID = Int64.Parse(txtTutorID.Text);  //
                TutorAlumno tutorAceptado = tutorAlumnoCtrl.Retrieve(tutorAlumnoFind, false).FirstOrDefault();

                if (tutorAceptado == null)
                {
                    tutorAceptado = new TutorAlumno() { TutorID = Int64.Parse(txtTutorID.Text), AlumnoID = userSession.CurrentAlumno.AlumnoID };
                    tutorAceptado.Tutor = new Tutor();
                    tutorAceptado.Tutor = tutorCtrl.Retrieve(new Tutor() { TutorID = Int64.Parse(txtTutorID.Text) }, false).FirstOrDefault();
                    Int16 selectVal;
                    if (ddlParentescoTutor.SelectedIndex != -1 && !string.IsNullOrEmpty(ddlParentescoTutor.SelectedValue))
                        if (Int16.TryParse(ddlParentescoTutor.SelectedValue, out selectVal))
                        {
                            tutorAceptado.Parentesco = selectVal;
                            tutorAceptado.Tutor.Parentesco = selectVal;
                        }
                    Session_Tutores.Add(tutorAceptado);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('El tutor ya esta vinculado');", true);
                }

                if (Session_Tutores.Count > 0)
                {
                    grdTutores.DataSource = null;
                    grdTutores.DataSource = Session_Tutores;
                    grdTutores.DataBind();
                    txtCodigoTutor.Text = string.Empty;
                }
            }
        }

        protected void btnAgregarTutor_Click(object sender, EventArgs e)
        {
            string res = ValidateTutorInsert();
            if (res == string.Empty)
            {
                if (TutorSeleccionado != null)

                    Session_Tutores.Remove(TutorSeleccionado);

                Session_Tutores.Add(UserInterfaceToTutor());
                grdTutores.DataSource = null;
                grdTutores.DataSource = Session_Tutores;
                grdTutores.DataBind();
            }
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('" + res + "');", true);
        }

        private string ValidateTutorInsert()
        {
            string sError = string.Empty;

            #region Valores Requeridos.
            if (txtTutorNombre.Text.Trim().Length <= 0)
                sError += " ,Nombre tutor";
            if (txtTutorApellido1.Text.Trim().Length <= 0)
                sError += " ,Primer Apellido tutor";
            if (ddlTutorSexo.SelectedIndex == -1 || ddlTutorSexo.SelectedValue.Length <= 0)
                sError += " ,Sexo tutor";
            if (txtTutorTelefono.Value.Trim().Length <= 0)
                sError += " ,Telefono tutor";
            if (txtTutorEMail.Text.Trim().Length <= 0)
                sError += " ,Correo tutor";
            if (ddlTutorParentesco.SelectedIndex == -1 || ddlTutorParentesco.SelectedValue.Length <= 0)
                sError += " ,Parentesco tutor";

            if (sError.Trim().Length > 0)
            {
                sError = sError.Substring(2);
                return sError = (string.Format("Los siguientes parámetros son requeridos: {0}", sError));
            }
            #endregion

            #region Valores Incorrectos
            if (txtNombre.Text.Trim().Length > 80)
                sError += " ,Nombre";
            if (txtPrimerApellido.Text.Trim().Length > 50)
                sError += " ,Primer Apellido";

            if (sError.Trim().Length > 0)
            {
                sError = sError.Substring(2);
                return sError = (string.Format("Los siguientes parámetros son inválidos: {0}", sError));
            }
            #endregion

            #region Formatos Inválidos
            DateTime fn;
            if (!DateTime.TryParseExact(txtTutorFechaNaciemiento.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out fn))
                sError += " ,Fecha Nacimiento";

            if (sError.Trim().Length > 0)
            {
                sError = sError.Substring(2);
                return sError = (string.Format("Los siguientes parámetros tienen un formato no valido: {0}", sError));
            }
            #endregion

            #region Correo Existente
            if (Session_Tutores.FirstOrDefault(x => x.Tutor.CorreoElectronico == txtCorreoTutor.Text.Trim()) != null)
            {
                sError += " ,El correo del tutor ya esta vinculado";
            }

            if (sError.Trim().Length > 0)
            {
                sError = sError.Substring(2);
                return sError = (string.Format("{0}", sError));
            }


            #endregion

            return sError;
        }

        private TutorAlumno UserInterfaceToTutor()
        {
            PasswordProvider passwordProvider = new PasswordProvider();
            TutorAlumno obj = new TutorAlumno();
            obj.Tutor = new Tutor();
            if (TutorSeleccionado == null)
            {
                TutorSeleccionado = new TutorAlumno();
                TutorSeleccionado.Tutor = new Tutor();
            }

            bool boolval;

            Int16 selectVal;
            #region Agrega valores a TutorAlumno
            if (TutorSeleccionado != null & TutorSeleccionado.Tutor != null & TutorSeleccionado.Tutor.TutorID != null)
                obj.TutorID = TutorSeleccionado.TutorID;
            obj.AlumnoID = userSession.CurrentAlumno.AlumnoID;
            if (ddlTutorParentesco.SelectedIndex != -1 && !string.IsNullOrEmpty(ddlTutorParentesco.SelectedValue))
                if (Int16.TryParse(ddlTutorParentesco.SelectedValue, out selectVal)) obj.Parentesco = selectVal;
            if (TutorSeleccionado != null & TutorSeleccionado.Tutor != null & TutorSeleccionado.Tutor.TutorID != null)
                obj.Tutor.TutorID = TutorSeleccionado.Tutor.TutorID;
            #endregion

            obj.Tutor.Nombre = txtTutorNombre.Text.Trim();
            obj.Tutor.PrimerApellido = txtTutorApellido1.Text.Trim();
            obj.Tutor.SegundoApellido = txtTutorApellido2.Text.Trim();
            obj.Tutor.FechaNacimiento = DateTime.ParseExact(this.txtTutorFechaNaciemiento.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            obj.Tutor.Direccion = txtTutorDireccion.Text.Trim();
            if (ddlTutorSexo.SelectedIndex != -1 && !string.IsNullOrEmpty(ddlTutorSexo.SelectedValue))
                if (bool.TryParse(ddlTutorSexo.SelectedValue, out boolval)) obj.Tutor.Sexo = boolval;
            obj.Tutor.Estatus = true;
            obj.Tutor.FechaRegistro = DateTime.Now;
            obj.Tutor.CorreoConfirmado = false;
            obj.Tutor.Codigo = passwordProvider.GetNewPassword();
            obj.Tutor.Credito = 0;
            obj.Tutor.CreditoUsado = 0;
            if (ddlTutorParentesco.SelectedIndex != -1 && !string.IsNullOrEmpty(ddlTutorParentesco.SelectedValue))
                if (Int16.TryParse(ddlTutorParentesco.SelectedValue, out selectVal)) obj.Tutor.Parentesco = selectVal;
            obj.Tutor.Telefono = txtTutorTelefono.Value.Trim();
            obj.Tutor.CorreoElectronico = txtTutorEMail.Text.Trim();


            return obj;
        }

        private void TutorToUserInterface(TutorAlumno obj)
        {
            txtTutorNombre.Text = obj.Tutor.Nombre;
            txtTutorApellido1.Text = obj.Tutor.PrimerApellido;
            txtTutorApellido2.Text = obj.Tutor.SegundoApellido;
            txtTutorFechaNaciemiento.Text = string.Format("{0:dd/MM/yyyy}", obj.Tutor.FechaNacimiento);
            txtTutorDireccion.Text = obj.Tutor.Direccion;
            ddlTutorSexo.SelectedValue = obj.Tutor.Sexo.ToString();
            ddlTutorParentesco.SelectedValue = obj.Tutor.Parentesco != null ? ((byte)obj.Tutor.Parentesco).ToString() : null;
            txtTutorTelefono.Value = obj.Tutor.Telefono;
            txtTutorEMail.Text = obj.Tutor.CorreoElectronico;
        }

        protected void grdTutores_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "ver":
                    string[] arg = new string[2];
                    arg = e.CommandArgument.ToString().Split(';');
                    if (arg[0] != string.Empty)
                        nTutorID = Int64.Parse(arg[0]);
                    else
                        nTutorID = null;
                    if (arg[1] != string.Empty)
                        sCorreoElectronico = arg[1];
                    else
                        sCorreoElectronico = null;


                    var tutorAlumnoSelect = Session_Tutores.FirstOrDefault(x => x.Tutor.TutorID == nTutorID & x.Tutor.CorreoElectronico == sCorreoElectronico);
                    TutorSeleccionado = tutorAlumnoSelect;

                    if (TutorSeleccionado != null)
                    {
                        TutorToUserInterface(TutorSeleccionado);
                        ddlTutorParentesco.SelectedValue = tutorAlumnoSelect.Parentesco != null ? ((byte)tutorAlumnoSelect.Parentesco).ToString() : null;
                        if (TutorSeleccionado.TutorID != null)
                            EnableFieldTutor(false);
                        else
                            EnableFieldTutor(true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "viewTutorDatos();", true);
                    }
                    break;
            }
        }

        protected void btnAñadirTutor_Click(object sender, EventArgs e)
        {
            TutorSeleccionado = null;
            EnableFieldTutor(true);
            CleanFieldTutor();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "viewTutorDatos();", true);
        }

        protected void btnViewDatosTutor_Click(object sender, ImageClickEventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "viewTutorDatos();", true);
        }

        private void EnableFieldTutor(bool enable)
        {
            txtTutorNombre.Enabled = enable;
            txtTutorApellido1.Enabled = enable;
            txtTutorApellido2.Enabled = enable;
            txtTutorFechaNaciemiento.Enabled = enable;
            txtTutorDireccion.Enabled = enable;
            ddlTutorSexo.Enabled = enable;
            txtTutorEMail.Enabled = enable;
        }

        private void CleanFieldTutor()
        {
            txtTutorNombre.Text = string.Empty;
            txtTutorApellido1.Text = string.Empty;
            txtTutorApellido2.Text = string.Empty;
            txtTutorFechaNaciemiento.Text = string.Empty;
            txtTutorDireccion.Text = string.Empty;
            ddlTutorSexo.SelectedValue = "";
            ddlTutorParentesco.SelectedValue = "";
            txtTutorTelefono.Value = string.Empty;
            txtTutorEMail.Text = string.Empty;
        }
    }
}