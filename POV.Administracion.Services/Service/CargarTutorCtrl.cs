using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Data;
using System.IO;
using Framework.Base.DataAccess;
using GP.SocialEngine.BO;
using GP.SocialEngine.Service;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.Comun.BO;
using POV.Comun.Service;
using POV.Licencias.BO;
using POV.Licencias.Service;
using POV.Localizacion.BO;
using POV.Localizacion.Service;
using POV.Seguridad.BO;
using POV.Seguridad.Service;
using POV.Seguridad.Utils;
using POV.CentroEducativo.Services;
using POV.Modelo.Context;
using System.Text.RegularExpressions;
using POV.Operaciones.Services;

namespace POV.Administracion.Service
{
    public class CargarTutorCtrl
    {
        private DataSet dsTutores = new DataSet();

        public DataSet DsTutores
        {
            get { return dsTutores.Tables[0] != null ? dsTutores : null; }
        }

        public string UrlImgNatware { get; set; }

        public string UrlPortalSocial { get; set; }

        private DataSet ValidarArchivoTutor(string urlArchivo)
        {
            DataSet dsArchivoCarga = new DataSet();
            ExcelCtrl excelCtrl = new ExcelCtrl();

            if (urlArchivo == null || (urlArchivo != null && urlArchivo == ""))
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, new Exception("Es requerido la ruta del archivo"));
                throw new Exception("Es requerido la ruta del archivo");
            }


            if (!File.Exists(urlArchivo))
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, new Exception("El archivo especificado no existe en la ruta: " + urlArchivo));
                throw new Exception("El archivo especificado no existe.");
            }

            FileInfo file = new FileInfo(urlArchivo);
            if (file.Extension.ToLower() != ".xls")
            {
                if (file.Extension.ToLower() != ".xlsx")
                {
                    POV.Logger.Service.LoggerHlp.Default.Error(this, new Exception("El archivo seleccionado no tiene el formato o extensión correcta ( *.xls Excel 97-2003, *.xlsx Excel 2003)"));
                    throw new Exception("El archivo seleccionado no tiene el formato o extensión correcta ( *.xls Excel 97-2003, *.xlsx Excel 2003)");
                }
            }
            try
            {
                dsArchivoCarga = excelCtrl.Consultar(file.FullName);
            }
            catch
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, new Exception("No se pudo consultar el archivo porque se encontraron inconsistencias al momento de cargar el archivo."));
                throw new Exception("No se pudo consultar el archivo porque se encontraron inconsistencias al momento de cargar el archivo.");
            }

            StringBuilder error = new StringBuilder();
            //Validamos que existan las columnas en el documento
            if (!dsArchivoCarga.Tables[0].Columns.Contains("Nombre"))
                error.Append(" ,Nombre");
            if (!dsArchivoCarga.Tables[0].Columns.Contains("PrimerApellido"))
                error.Append(" ,PrimerApellido");
            if (!dsArchivoCarga.Tables[0].Columns.Contains("SegundoApellido"))
                error.Append(" ,SegundoApellido");
            if (!dsArchivoCarga.Tables[0].Columns.Contains("FechaNacimiento"))
                error.Append(" ,FechaNacimiento");
            if (!dsArchivoCarga.Tables[0].Columns.Contains("Sexo"))
                error.Append(" ,Sexo");
            if (!dsArchivoCarga.Tables[0].Columns.Contains("Telefono"))
                error.Append(" ,Telefono");
            if (!dsArchivoCarga.Tables[0].Columns.Contains("Correo"))
                error.Append(" ,Correo");
            if (!dsArchivoCarga.Tables[0].Columns.Contains("Curp"))
                error.Append(", Curp");

            if (error.Length > 0)
                throw new Exception("Las siguiente columnas no estan presentes: " + error.ToString().Substring(2));

            if (dsArchivoCarga.Tables[0].Rows.Count == 0)
                throw new Exception("El archivo cargado no contiene padres.");

            return dsArchivoCarga;
        }

        private bool EnviarCorreo(Usuario usuario, string pws, string clave)
        {
            bool envio = false;

            #region Variables
            string urllogo = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgLogo"];
            string urlimgbackground = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgBackground"];
            string urlimgface = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgFace"];
            string urlimgtwit = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgTwit"];
            string urlimglinked = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgLinked"];
            string urlimginstagram = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgInstagram"];
            const string imgalt = "YOY - PADRES";
            const string titulo = "Registro exitoso";
            string linkportal = System.Configuration.ConfigurationManager.AppSettings["POVUrlPadres"]; ;
            #endregion
            CorreoCtrl correoCtrl = new CorreoCtrl();

            string cuerpo = string.Empty;

            var serverPath = System.Web.Hosting.HostingEnvironment.MapPath(System.Configuration.ConfigurationManager.AppSettings["POVUrlEmailTemplateNewOrientador"]);
            using (StreamReader reader = new StreamReader(serverPath))
            {
                cuerpo = reader.ReadToEnd();
            }

            cuerpo = cuerpo.Replace("{urllogo}", urllogo);
            cuerpo = cuerpo.Replace("{urlimgbackground}", urlimgbackground);
            cuerpo = cuerpo.Replace("{title}", titulo);
            cuerpo = cuerpo.Replace("{user}", usuario.NombreUsuario);
            cuerpo = cuerpo.Replace("{password}", pws);
            cuerpo = cuerpo.Replace("{linkportal}", linkportal);
            cuerpo = cuerpo.Replace("{urlimgface}", urlimgface);
            cuerpo = cuerpo.Replace("{urlimgtwit}", urlimgtwit);
            cuerpo = cuerpo.Replace("{urlimglinked}", urlimglinked);
            cuerpo = cuerpo.Replace("{urlimginstagram}", urlimginstagram);

            List<string> tos = new List<string>();
            tos.Add(usuario.Email);
            AlternateView texto = null;
            List<string> archivos = new List<string>();
            List<string> copias = new List<string>();

            try
            {
                correoCtrl.sendMessage(tos, "YOY - Registro exitoso", cuerpo, texto, archivos, copias);
                envio = true;
            }
            catch (Exception ex)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
                envio = false;
            }
            return envio;
        }

        public DataTable CargarArchivoTutor(IDataContext dctx, string urlArchivo)
        {
            var objeto = new object();
            var contexto = new Contexto(objeto);

            DataSet dsArchivoCarga = this.ValidarArchivoTutor(urlArchivo);
            dsArchivoCarga.Tables[0].Columns.Add(new DataColumn("RowIndex", typeof(int)));
            dsArchivoCarga.Tables[0].Columns.Add(new DataColumn("Cargado", typeof(bool)));
            dsTutores = dsArchivoCarga;

            #region Crear estructura de respuesta
            DataTable dtResultado = new DataTable();
            dtResultado.Columns.Add(new DataColumn("RowIndex", typeof(int)));
            dtResultado.Columns.Add(new DataColumn("Correo", typeof(string)));
            dtResultado.Columns.Add(new DataColumn("ExisteTutor", typeof(bool)));
            dtResultado.Columns.Add(new DataColumn("ActivoTutor", typeof(bool)));
            dtResultado.Columns.Add(new DataColumn("Inconsistencia", typeof(string)));
            #endregion

            string path = System.Configuration.ConfigurationManager.AppSettings["POVUrlResultadoExportacionTutor"];

            object con = new object();

            #region Inicializar Conexion a BD
            try
            {
                dctx.OpenConnection(con);
            }
            catch (Exception ex)
            {
                throw new Exception("Inconsistencias al conectarse a la base de datos.");
            }
            #endregion

            Tutor tutor = new Tutor();
            Usuario usuario = new Usuario();
            UsuarioCtrl usuarioCtrl = new UsuarioCtrl();
            String passTmp = string.Empty;

            #region Try 1
            object tran = new object();
            dctx.BeginTransaction(tran);

            try
            {
                LicenciaEscuelaCtrl licenciaEscuelaCtrl = new LicenciaEscuelaCtrl();

                foreach (DataRow row in dsArchivoCarga.Tables[0].Rows)
                {
                    #region Inicializacion de Resultado de inconsistencias
                    DataRow rwResultado = dtResultado.NewRow();
                    rwResultado.SetField<int>("RowIndex", dsArchivoCarga.Tables[0].Rows.IndexOf(row));
                    row.SetField<int>("RowIndex", dsArchivoCarga.Tables[0].Rows.IndexOf(row));
                    rwResultado.SetField<bool>("ExisteTutor", false);
                    rwResultado.SetField<bool>("ActivoTutor", false);
                    rwResultado.SetField<string>("Inconsistencia", "");
                    #endregion

                    #region Try 2
                    try
                    {
                        #region Try 3
                        try
                        {
                            bool registrar = false; // suponemos que la informacion es erronea

                            #region Obtenemos información como texto
                            string nombre = row.IsNull("Nombre") ? "" : row["Nombre"].ToString();
                            string primerApellido = row.IsNull("PrimerApellido") ? "" : row["PrimerApellido"].ToString();
                            string segundoApellido = row.IsNull("SegundoApellido") ? "" : row["SegundoApellido"].ToString();
                            string fechaNacimiento = row.IsNull("FechaNacimiento") ? "" : row["FechaNacimiento"].ToString();
                            DateTime tmpFecha = new DateTime();
                            if (!string.IsNullOrEmpty(fechaNacimiento))
                            {
                                tmpFecha = Convert.ToDateTime(fechaNacimiento);
                                fechaNacimiento = tmpFecha.ToShortDateString();
                            }
                            string sexo = row.IsNull("Sexo") ? "" : row["Sexo"].ToString();
                            string telefono = row.IsNull("Telefono") ? "" : row["Telefono"].ToString();
                            string correo = row.IsNull("Correo") ? "" : row["Correo"].ToString();
                            string curp = row.IsNull("Curp") ? "" : row["Curp"].ToString();
                            #endregion

                            #region Validaciones de datos requerido del sistema
                            #region Requeridos
                            string sError = string.Empty;
                            // valores requerido
                            if (string.IsNullOrEmpty(nombre))
                                sError += " ,Nombre";
                            if (string.IsNullOrEmpty(primerApellido))
                                sError += " ,Primer Apellido";
                            if (string.IsNullOrEmpty(fechaNacimiento))
                                sError += " ,Fecha Nacimiento";
                            if (string.IsNullOrEmpty(sexo))
                                sError += " ,Sexo";
                            if (string.IsNullOrEmpty(telefono))
                                sError += " ,Teléfono";
                            if (string.IsNullOrEmpty(correo))
                                sError += " , Correo Electrónico";
                            if (string.IsNullOrEmpty(curp))
                                sError += " , Curp";

                            if (sError.Trim().Length > 0)
                            {
                                sError = sError.Substring(2);
                                throw new Exception(string.Format("Los siguientes parámetros son requeridos:{0}", sError));
                            }
                            #endregion

                            #region Longitud
                            //Valores con Incorrectos - longitud excedida.
                            if (nombre.Trim().Length > 80)
                                sError += " ,Nombre";
                            if (primerApellido.Trim().Length > 50)
                                sError += " ,Primer Apellido";
                            if (segundoApellido.Trim().Length > 50)
                                sError += " ,Segundo Apellido";
                            if (correo.Trim().Length > 100)
                                sError += " ,Correo Electrónico";
                            if (curp.Trim().Length > 18)
                                sError += " ,Curp";

                            if (sError.Trim().Length > 0)
                            {
                                sError = sError.Substring(2);
                                throw new Exception(string.Format("Los siguientes parámetros son inválidos :{0}", sError));
                            }
                            #endregion

                            #region Formato incorrecto
                            if (!ValidateEmailRegex(correo.Trim()))
                                sError += " ,Correo Electrónico";

                            if (!ValidateCurpRegex(curp.Trim()))
                                sError += " ,Curp";

                            DateTime fn;
                            if (!DateTime.TryParseExact(fechaNacimiento.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out fn))
                                sError += " ,Fecha Nacimiento";

                            if (sError.Trim().Length > 0)
                            {
                                sError = sError.Substring(2);
                                throw new Exception(string.Format("Los siguientes parámetros tienen un formato no válido:{0}", sError));
                            }
                            #endregion

                            #endregion

                            tutor = new Tutor();
                            TutorCtrl tutorCtrl = new TutorCtrl(null);


                            #region Try 4 Existe Tutor
                            try
                            {
                                #region Tutor
                                tutor.Curp = curp;//Unicamente por la curp validamos el registro

                                tutor = tutorCtrl.Retrieve(tutor, true).FirstOrDefault();
                                rwResultado.SetField<bool>("ExisteTutor", tutor != null);

                                if (rwResultado.Field<bool>("ExisteTutor"))
                                {
                                    rwResultado.SetField<bool>("ActivoTutor", tutor.Estatus.Value);
                                }
                                #endregion
                            }
                            catch (Exception ex)
                            {
                                rwResultado.SetField<string>("Inconsistencia", rwResultado.Field<string>("Inconsistencia") + "\n" + ex.Message);
                            }
                            #endregion

                            if (rwResultado.Field<string>("Inconsistencia") == "")
                                registrar = true;

                            row.SetField<bool>("Cargado", registrar);

                            if (!registrar)
                            {
                                dctx.RollbackTransaction(tran);
                                dtResultado.Rows.Add(rwResultado);
                                continue;
                            }

                            #region Registro de Tutor
                            usuario = new Usuario();
                            UsuarioSocial usuarioSocial = new UsuarioSocial();
                            passTmp = string.Empty;

                            string strNombreUsuario = string.Empty;
                            if (rwResultado.Field<bool>("ExisteTutor")) // si el Tutor existe, actualizamos Tutor
                            {
                                //Guardamos el correo del tutor anterior
                                string lastEmail = tutor.CorreoElectronico;

                                tutor.Nombre = nombre;
                                tutor.PrimerApellido = primerApellido;
                                tutor.SegundoApellido = segundoApellido;
                                tutor.Estatus = true;
                                if (fechaNacimiento != string.Empty)
                                    tutor.FechaNacimiento = DateTime.ParseExact(fechaNacimiento, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                tutor.Sexo = sexo.Equals("H");
                                tutor.Telefono = telefono;
                                tutor.CorreoElectronico = correo;
                                tutor.Curp = curp;

                                tutorCtrl.Update(tutor); // actualizamos Tutor

                                #region Actualizamos el usuario
                                string paswordtemp = string.Empty;
                                usuario = licenciaEscuelaCtrl.RetrieveUsuarioTutor(dctx, tutor);

                                usuarioCtrl = new UsuarioCtrl();
                                if (usuario.UsuarioID != null)
                                {
                                    usuario = usuarioCtrl.LastDataRowToUsuario(usuarioCtrl.Retrieve(dctx, usuario));

                                    if (usuario.EsActivo == false || (tutor.CorreoElectronico != lastEmail))
                                    {
                                        Usuario original = (Usuario)usuario.Clone();
                                        usuario.EsActivo = true;
                                        usuario.Email = tutor.CorreoElectronico;

                                        if (!string.IsNullOrEmpty(usuario.Email))
                                            if (!EmailDisponible(dctx, usuario))
                                            {
                                                Exception ex = new Exception("El correo electrónico proporcionado no se encuentra disponible") { Source = "Email" };
                                                throw ex;
                                            }

                                        usuarioCtrl.Update(dctx, usuario, original);

                                        if (tutor.CorreoElectronico == lastEmail)
                                            passTmp = "Use la ultima contraseña registrada";
                                        else
                                        {
                                            passTmp = new PasswordProvider(8).GetNewPassword();
                                            usuario.Password = EncryptHash.SHA1encrypt(passTmp);
                                            usuario.EmailVerificado = false;
                                        }
                                        EnviarCorreo(usuario, passTmp, string.Empty);
                                    }
                                }
                                #endregion

                            }
                            else // si el Tutor no existe, registramos Tutor
                            {
                                if (tutor == null)
                                    tutor = new Tutor();

                                #region Se crea el tutor
                                PasswordProvider passwordProvider = new PasswordProvider();
                                passwordProvider.LongitudPassword = 10;
                                passwordProvider.PorcentajeMayusculas = 50;
                                passwordProvider.PorcentajeNumeros = 50;

                                tutor.Nombre = nombre;
                                tutor.PrimerApellido = primerApellido;
                                tutor.SegundoApellido = segundoApellido;
                                tutor.Estatus = true;
                                if (fechaNacimiento != string.Empty)
                                    tutor.FechaNacimiento = DateTime.ParseExact(fechaNacimiento, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                tutor.Sexo = sexo.Equals("Hombre");
                                tutor.Telefono = telefono;
                                tutor.CorreoElectronico = correo;
                                tutor.EstatusIdentificacion = false;
                                tutor.Curp = curp;
                                #endregion

                                #region Se crea el usuario del tutor
                                UsernameProvider provider = new UsernameProvider(tutor.Nombre, tutor.PrimerApellido, tutor.FechaNacimiento.Value);
                                strNombreUsuario = provider.GenerarUsername();
                                usuario.NombreUsuario = strNombreUsuario;
                                usuario.Email = correo;
                                passTmp = new PasswordProvider(8).GetNewPassword();
                                usuario.Password = EncryptHash.SHA1encrypt(passTmp);
                                usuario.EmailVerificado = false;
                                usuario.TelefonoReferencia = telefono;
                                #endregion

                                CatalogoTutoresCtrl catalogoTutorCtrl = new CatalogoTutoresCtrl();

                                if (catalogoTutorCtrl.InsertTutor(dctx, tutor, usuario)) // registramos Tutor con su usuario
                                {
                                    dctx.CommitTransaction(tran);
                                    RegistroLog(tutor, path, "Registrado");
                                }
                            }
                            #endregion

                        }
                        catch (Exception ex)
                        {
                            dctx.RollbackTransaction(tran);
                            POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
                            row.SetField<bool>("Cargado", false);
                            rwResultado.SetField<string>("Inconsistencia", rwResultado.Field<string>("Inconsistencia") + "\n" + ex.Message);
                            dtResultado.Rows.Add(rwResultado);                            
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        dctx.RollbackTransaction(tran);
                        POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
                        row.SetField<bool>("Cargado", false);
                        rwResultado.SetField<string>("Inconsistencia", rwResultado.Field<string>("Inconsistencia") + "\n" + ex.Message);
                        dtResultado.Rows.Add(rwResultado);                        
                    }
                    finally
                    {
                        dctx.CommitTransaction(tran);

                    }
                    #endregion
                }
            }
            finally
            {
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(con);
            }
            #endregion

            return dtResultado;
        }

        public bool ValidateEmailRegex(string email)
        {
            string patternLenient = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
            Regex reLenient = new Regex(patternLenient);
            bool match = reLenient.IsMatch(email);
            return match;
        }

        private bool ValidateCurpRegex(string curp)
        {
            curp = curp.ToUpper();
            string patternLenient = @"[A-Z]{1}[AEIOUX]{1}[A-Z]{2}[0-9]{2}"
                + "(0[1-9]|1[0-2])(0[1-9]|1[0-9]|2[0-9]|3[0-1])" +
                "[HM]{1}" +
                "(AS|BC|BS|CC|CS|CH|CL|CM|DF|DG|GT|GR|HG|JC|MC|MN|MS|NT|NL|OC|PL|QT|QR|SP|SL|SR|TC|TS|TL|VZ|YN|ZS|NE)" +
                "[B-DF-HJ-NP-TV-Z]{3}" +
                "[0-9A-Z]{1}[0-9]{1}$";
            Regex reLenient = new Regex(patternLenient);

            bool match = reLenient.IsMatch(curp);
            return match;
        }

        public bool EmailDisponible(IDataContext dctx, Usuario usuario)
        {
            if (usuario == null || string.IsNullOrEmpty(usuario.Email))
                throw new ArgumentException("Email requerido", "usuario");

            UsuarioCtrl usuarioCtrl = new UsuarioCtrl();

            Usuario usr = new Usuario();
            if (usuario.Email != null)
                usr.Email = usuario.Email;
            usr.EsActivo = true;
            if (usuario.UniversidadId != null)
                usr.UniversidadId = usuario.UniversidadId;

            DataSet ds = usuarioCtrl.Retrieve(dctx, usr);
            int index = ds.Tables[0].Rows.Count;
            if (index == 1 && usuario.UsuarioID != null)
            {
                Usuario user = usuarioCtrl.LastDataRowToUsuario(ds);
                return usr.UsuarioID == usuario.UsuarioID;
            }

            if (index <= 0)
                return true;

            return false;
        }

        public void RegistroLog(Tutor tutor, string path, string Log)
        {
            string sexo;
            if ((bool)tutor.Sexo)
                sexo = "Hombre";
            else
                sexo = "Mujer";
            string lines = @"Nombre: " + tutor.Nombre + " Primer Apellido: " + tutor.PrimerApellido + " Segundo Apellido: " + tutor.SegundoApellido + " Sexo: " +
                sexo + " Fecha Nacimiento: " + tutor.FechaNacimiento + " Fecha Registro: " + DateTime.Now + "\nEstatus: " + Log;

            var serverPath = System.Web.Hosting.HostingEnvironment.MapPath(path);
            StreamWriter sw = new StreamWriter(serverPath, true);
            sw.WriteLine(lines.Trim());
            sw.Close();

        }
    }
}