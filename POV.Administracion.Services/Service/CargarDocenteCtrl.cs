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
    public class CargarDocenteCtrl
    {
        private DataSet dsDocentes = new DataSet();

        public DataSet DsDocentes
        {
            get { return dsDocentes.Tables[0] != null ? dsDocentes : null; }
        }

        public string UrlImgNatware { get; set; }

        public string UrlPortalSocial { get; set; }

        private DataSet ValidarArchivoDocente(string urlArchivo)
        {
            DataSet dsArchivoCarga = new DataSet();
            ExcelCtrl excelCtrl = new ExcelCtrl();

            if (urlArchivo == null || (urlArchivo != null && urlArchivo == ""))
                throw new Exception("Es requerido la ruta del archivo");

            if (!File.Exists(urlArchivo))
                throw new Exception("Se encontraron inconsistencias al momento de cargar el archivo.");

            FileInfo file = new FileInfo(urlArchivo);
            if (file.Extension.ToLower() != ".xls")
                if (file.Extension.ToLower() != ".xlsx")
                    throw new Exception("El archivo seleccionado no tiene el formato o extensión correcta ( *.xls Excel 97-2003, *.xlsx Excel 2003)");

            try
            {
                dsArchivoCarga = excelCtrl.Consultar(file.FullName);
            }
            catch
            {
                throw new Exception("Se encontraron inconsistencias al momento de cargar el archivo.");
            }

            StringBuilder error = new StringBuilder();

            if (!dsArchivoCarga.Tables[0].Columns.Contains("Nombre"))
                error.Append(" ,Nombre");
            if (!dsArchivoCarga.Tables[0].Columns.Contains("PrimerApellido"))
                error.Append(" ,PrimerApellido");
            if (!dsArchivoCarga.Tables[0].Columns.Contains("SegundoApellido"))
                error.Append(" ,SegundoApellido");
            if (!dsArchivoCarga.Tables[0].Columns.Contains("FechaNacimiento"))
                error.Append(" ,FechaNacimiento");
            if (!dsArchivoCarga.Tables[0].Columns.Contains("Curp"))
                error.Append(" ,Curp");
            if (!dsArchivoCarga.Tables[0].Columns.Contains("Sexo"))
                error.Append(" ,Sexo");
            if (!dsArchivoCarga.Tables[0].Columns.Contains("Cedula"))
                error.Append(" ,Cedula");
            if (!dsArchivoCarga.Tables[0].Columns.Contains("NivelEstudio"))
                error.Append(" ,NivelEstudio");
            if (!dsArchivoCarga.Tables[0].Columns.Contains("Titulo"))
                error.Append(" ,Titulo");
            if (!dsArchivoCarga.Tables[0].Columns.Contains("UsuarioSkype"))
                error.Append(" ,UsuarioSkype");
            if (!dsArchivoCarga.Tables[0].Columns.Contains("Correo"))
                error.Append(" ,Correo");
            if (!dsArchivoCarga.Tables[0].Columns.Contains("ClaveEscolar"))
                error.Append(" ,ClaveEscolar");

            if (error.Length > 0)
                throw new Exception("Las siguiente columnas no estan presentes: " + error.ToString().Substring(2));

            if (dsArchivoCarga.Tables[0].Rows.Count == 0)
                throw new Exception("El archivo cargado no contiene docentes.");

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
            const string imgalt = "YOY - ORIENTADOR";
            const string titulo = "Registro exitoso";
            string linkportal = System.Configuration.ConfigurationManager.AppSettings["POVUrlOrientador"]; ;
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

        public DataTable CargarArchivoDocente(IDataContext dctx, string urlArchivo, Escuela escuela, CicloEscolar cicloEscolar)
        {
            if (escuela == null || escuela.EscuelaID == null || escuela.EscuelaID.Value <= 0)
                throw new ArgumentException("Escuela es requerido.", "escuela");

            if (cicloEscolar == null || cicloEscolar.CicloEscolarID == null || cicloEscolar.CicloEscolarID.Value <= 0)
                throw new ArgumentException("CicloEscolar es requerido.", "cicloEscolar");

            var objeto = new object();
            var contexto = new Contexto(objeto);

            DataSet dsArchivoCarga = this.ValidarArchivoDocente(urlArchivo);
            dsArchivoCarga.Tables[0].Columns.Add(new DataColumn("RowIndex", typeof(int)));
            dsArchivoCarga.Tables[0].Columns.Add(new DataColumn("Cargado", typeof(bool)));
            dsDocentes = dsArchivoCarga;

            #region Crear estructura de respuesta
            DataTable dtResultado = new DataTable();
            dtResultado.Columns.Add(new DataColumn("RowIndex", typeof(int)));
            dtResultado.Columns.Add(new DataColumn("DocenteCURP", typeof(string)));
            dtResultado.Columns.Add(new DataColumn("ExisteDocente", typeof(bool)));
            dtResultado.Columns.Add(new DataColumn("ActivoDocente", typeof(bool)));
            dtResultado.Columns.Add(new DataColumn("Inconsistencia", typeof(string)));

            dtResultado.Columns.Add(new DataColumn("EscuelaClave", typeof(string)));
            dtResultado.Columns.Add(new DataColumn("ExisteEscuela", typeof(bool)));
            dtResultado.Columns.Add(new DataColumn("ActivoEscuela", typeof(bool)));
            #endregion

            object con = new object();

            string path = System.Configuration.ConfigurationManager.AppSettings["POVUrlResultadoExportacionOrientador"];

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

            try
            {
                CicloEscolarCtrl cicloEscolarCtrl = new CicloEscolarCtrl();
                cicloEscolar = cicloEscolarCtrl.LastDataRowToCicloEscolar(cicloEscolarCtrl.Retrieve(dctx, cicloEscolar));

                LicenciaEscuela licenciaEscuela = new LicenciaEscuela();
                licenciaEscuela.CicloEscolar = cicloEscolar;
                licenciaEscuela.Escuela = escuela;
                licenciaEscuela.Activo = true;

                LicenciaEscuelaCtrl licenciaEscuelaCtrl = new LicenciaEscuelaCtrl();
                DataSet ds = licenciaEscuelaCtrl.Retrieve(dctx, licenciaEscuela);

                if (ds.Tables[0].Rows.Count == 0)
                    throw new Exception("La escuela no cuenta con licencia para el ciclo escolar especificado.");

                EscuelaCtrl escuelaCtrl = new EscuelaCtrl();
                licenciaEscuela = licenciaEscuelaCtrl.LastDataRowToLicenciaEscuela(ds);
                Escuela escuelaActual = escuelaCtrl.LastDataRowToEscuela(escuelaCtrl.Retrieve(dctx, licenciaEscuela.Escuela));

                foreach (DataRow row in dsArchivoCarga.Tables[0].Rows)
                {
                    object tran = new object();

                    #region Inicializacion de Resultado de inconsistencias
                    DataRow rwResultado = dtResultado.NewRow();
                    rwResultado.SetField<int>("RowIndex", dsArchivoCarga.Tables[0].Rows.IndexOf(row));
                    row.SetField<int>("RowIndex", dsArchivoCarga.Tables[0].Rows.IndexOf(row));
                    rwResultado.SetField<string>("DocenteCURP", "");
                    rwResultado.SetField<bool>("ExisteDocente", false);
                    rwResultado.SetField<bool>("ActivoDocente", false);
                    rwResultado.SetField<string>("Inconsistencia", "");

                    rwResultado.SetField<string>("EscuelaClave", "");
                    rwResultado.SetField<bool>("ExisteEscuela", false);
                    rwResultado.SetField<bool>("ActivoEscuela", false);
                    #endregion

                    try
                    {
                        dctx.BeginTransaction(tran);

                        try
                        {
                            bool registrar = false; // suponemos que la informacion es erronea

                            #region Obtenemos informacion como texto
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
                            string curp = row.IsNull("Curp") ? "" : row["Curp"].ToString();
                            string sexo = row.IsNull("Sexo") ? "" : row["Sexo"].ToString();
                            string cedula = row.IsNull("Cedula") ? "" : row["Cedula"].ToString();
                            string nivelEstudio = row.IsNull("NivelEstudio") ? "" : row["NivelEstudio"].ToString();
                            string titulo = row.IsNull("Titulo") ? "" : row["Titulo"].ToString();
                            string usuarioSkype = row.IsNull("UsuarioSkype") ? "" : row["UsuarioSkype"].ToString();
                            string correo = row.IsNull("Correo") ? "" : row["Correo"].ToString();
                            string claveEscolar = row.IsNull("ClaveEscolar") ? "" : row["ClaveEscolar"].ToString();
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
                            if (string.IsNullOrEmpty(curp))
                                sError += " , CURP";
                            if (string.IsNullOrEmpty(sexo))
                                sError += " ,Sexo";
                            if (string.IsNullOrEmpty(cedula))
                                sError += " , Cedula";
                            if (string.IsNullOrEmpty(nivelEstudio))
                                sError += " , Nivel Estudio";
                            if (string.IsNullOrEmpty(titulo))
                                sError += " , Titulo";
                            if (string.IsNullOrEmpty(usuarioSkype))
                                sError += " , Usuario Skype";
                            if (string.IsNullOrEmpty(correo))
                                sError += " , Correo Electrónico";
                            if (string.IsNullOrEmpty(claveEscolar))
                                sError += " ,Clave escolar";

                            if (sError.Trim().Length > 0)
                            {
                                sError = sError.Substring(2);
                                throw new Exception(string.Format("Los siguientes parámetros son requeridos:{0}", sError));
                            }
                            #endregion

                            //Valores con Incorrectos.
                            #region Incorrecto
                            if (nombre.Trim().Length > 80)
                                sError += " ,Nombre";
                            if (primerApellido.Trim().Length > 50)
                                sError += " ,Primer Apellido";
                            if (segundoApellido.Trim().Length > 50)
                                sError += " ,Segundo Apellido";
                            if (curp.Trim().Length > 18)
                                sError += " ,CURP";
                            if (cedula.Trim().Length > 10)
                                sError += " ,Cédula";
                            if (nivelEstudio.Trim().Length > 10)
                                sError += " ,Nivel estudio";
                            if (correo.Trim().Length > 100)
                                sError += " ,Correo Electrónico";
                            if (sError.Trim().Length > 0)
                            {
                                sError = sError.Substring(2);
                                throw new Exception(string.Format("Los siguientes parámetros son inválidos :{0}", sError));
                            }
                            #endregion

                            if (!ValidateEmailRegex(correo.Trim()))
                                sError += " ,Correo Electrónico";

                            if (!ValidateCurpRegex(curp.Trim()))
                                sError += " ,CURP";

                            DateTime fn;
                            if (!DateTime.TryParseExact(fechaNacimiento.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out fn))
                                sError += " ,Fecha Nacimiento";
                            if (sError.Trim().Length > 0)
                            {
                                sError = sError.Substring(2);
                                throw new Exception(string.Format("Los siguientes parámetros tienen un formato no válido:{0}", sError));
                            }
                            #endregion

                            Docente docente = new Docente();
                            DocenteCtrl docenteCtrl = new DocenteCtrl();

                            Universidad existeEscuela = new Universidad();
                            Universidad universidad = new Universidad();
                            UniversidadCtrl universidadCtrl = new UniversidadCtrl(contexto);

                            #region Existe Escuela, Docente
                            try
                            {
                                #region Docente
                                docente.Curp = curp;

                                ds = docenteCtrl.Retrieve(dctx, docente);
                                rwResultado.SetField<bool>("ExisteDocente", ds.Tables[0].Rows.Count != 0);

                                if (rwResultado.Field<bool>("ExisteDocente"))
                                {
                                    docente = docenteCtrl.LastDataRowToDocente(ds);
                                    rwResultado.SetField<bool>("ActivoDocente", docente.Estatus.Value);
                                }
                                #endregion

                                #region Escuela
                                existeEscuela.ClaveEscolar = claveEscolar;
                                universidad = universidadCtrl.Retrieve(new Universidad { ClaveEscolar = existeEscuela.ClaveEscolar }, false).FirstOrDefault();
                                rwResultado.SetField<bool>("ExisteEscuela", universidad != null);

                                if (rwResultado.Field<bool>("ExisteEscuela"))
                                {
                                    rwResultado.SetField<bool>("ExisteEscuela", universidad.Activo.Value);
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

                            if (rwResultado.Field<bool>("ExisteEscuela") == false && rwResultado.Field<bool>("ActivoEscuela") == false)
                                throw new Exception("La Escuela del orientador no está registrada o disponible.");

                            if (!registrar)
                            {
                                dctx.RollbackTransaction(tran);
                                dtResultado.Rows.Add(rwResultado);
                                continue;
                            }

                            #region Registro de Docente
                            Usuario usuario = new Usuario();
                            UsuarioSocial usuarioSocial = new UsuarioSocial();
                            UsuarioCtrl usuarioCtrl = new UsuarioCtrl();
                            string strNombreUsuario = string.Empty;
                            if (rwResultado.Field<bool>("ExisteDocente")) // si el Docente existe, actualizamos Docente
                            {
                                Docente aDocente = (Docente)docente.Clone();
                                aDocente.Nombre = nombre;
                                aDocente.PrimerApellido = primerApellido;
                                aDocente.SegundoApellido = segundoApellido;
                                aDocente.Estatus = true;
                                if (fechaNacimiento != string.Empty)
                                    aDocente.FechaNacimiento = DateTime.ParseExact(fechaNacimiento, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                aDocente.Sexo = sexo.Equals("Hombre");
                                aDocente.Correo = correo;
                                aDocente.EsPremium = false;
                                aDocente.Curp = curp;
                                aDocente.NivelEstudio = nivelEstudio;
                                aDocente.Titulo = titulo;
                                aDocente.UsuarioSkype = usuarioSkype;
                                aDocente.Cedula = cedula;
                                aDocente.Cursos = "N/A";
                                aDocente.Especialidades = "N/A";
                                aDocente.Experiencia = "N/A";

                                docenteCtrl.Update(dctx, aDocente, docente); // actualizamos Docente
                                docente = aDocente;
                                usuario = licenciaEscuelaCtrl.RetrieveUsuario(dctx, docente);
                            }
                            else // si el Docente no existe, registramos Docente
                            {
                                docente.Nombre = nombre;
                                docente.PrimerApellido = primerApellido;
                                docente.SegundoApellido = segundoApellido;
                                docente.Estatus = true;
                                if (fechaNacimiento != string.Empty)
                                    docente.FechaNacimiento = DateTime.ParseExact(fechaNacimiento, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                docente.Sexo = sexo.Equals("Hombre");
                                docente.Correo = correo;
                                docente.FechaRegistro = DateTime.Now;
                                docente.Clave = new PasswordProvider(5).GetNewPassword(); // generamos Clave para confirmar información
                                docente.EstatusIdentificacion = false;
                                docente.Curp = curp;
                                docente.NivelEstudio = nivelEstudio;
                                docente.Titulo = titulo;
                                docente.EsPremium = false;
                                docente.UsuarioSkype = usuarioSkype;
                                docente.Cedula = cedula;
                                docente.Cursos = "N/A";
                                docente.Especialidades = "N/A";
                                docente.Experiencia = "N/A";

                                docenteCtrl.Insert(dctx, docente); // registramos Docente
                                docente = docenteCtrl.LastDataRowToDocente(docenteCtrl.Retrieve(dctx, docente)); // obtenemos Docente

                                usuario = licenciaEscuelaCtrl.RetrieveUsuario(dctx, docente);
                            }
                            #endregion

                            //Registrar asignación de Docente a -Escuela  
                            escuelaCtrl.InsertAsignacionDocente(dctx, docente, escuelaActual);

                            #region Registro de Usuario
                            string paswordtemp = string.Empty;
                            if (universidad != null)
                            {
                                usuario = licenciaEscuelaCtrl.RetrieveUsuarios(dctx, docente).Where(x => x.UniversidadId == universidad.UniversidadID).FirstOrDefault();
                                if (usuario == null)
                                    usuario = new Usuario();
                            }
                            else
                                usuario = licenciaEscuelaCtrl.RetrieveUsuario(dctx, docente);


                            if (usuario.UsuarioID != null)
                            {
                                usuario = usuarioCtrl.LastDataRowToUsuario(usuarioCtrl.Retrieve(dctx, usuario));                               

                                if ((usuario.Email != docente.Correo) || usuario.EsActivo == false)
                                {
                                    Usuario original = (Usuario)usuario.Clone();
                                    usuario.EsActivo = true;
                                    usuario.Email = docente.Correo;

                                    if (!string.IsNullOrEmpty(usuario.Email))                                        
                                    {
                                        
                                        if (!EmailDisponible(dctx, usuario))
                                        {
                                            Exception ex = new Exception("El correo electrónico proporcionado no se encuentra disponible") { Source = "Email" };
                                            throw ex;
                                        }
                                        else
                                        {
                                            paswordtemp = new PasswordProvider(8).GetNewPassword();
                                            byte[] pws = EncryptHash.SHA1encrypt(paswordtemp);
                                            usuario.Password = pws;
                                            usuario.EmailVerificado = false;
                                        }
                                    }

                                    usuarioCtrl.Update(dctx, usuario, original);
                                }

                            }
                            else
                            {
                                usuario.NombreUsuario = usuarioCtrl.GenerarNombreUsuarioUnico(dctx, docente.Nombre, docente.PrimerApellido, docente.FechaNacimiento.Value);
                                usuario.Email = docente.Correo;
                                paswordtemp = new PasswordProvider(8).GetNewPassword();
                                byte[] pws = EncryptHash.SHA1encrypt(paswordtemp);
                                usuario.Password = pws;
                                usuario.EmailVerificado = false;
                                usuario.EsActivo = true;
                                usuario.FechaCreacion = DateTime.Now;
                                usuario.PasswordTemp = true;
                                usuario.UniversidadId = universidad.UniversidadID;

                                //Consultar Termino Activo
                                TerminoCtrl terminoCtrl = new TerminoCtrl();
                                DataSet dsTermino = (terminoCtrl.Retrieve(dctx, new Termino { Estatus = true }));

                                usuario.Termino = dsTermino.Tables[0].Rows.Count >= 1 ? terminoCtrl.LastDataRowToTermino(dsTermino) : new Termino();
                                usuario.AceptoTerminos = false;

                                if (!string.IsNullOrEmpty(usuario.Email))
                                    if (!EmailDisponible(dctx, usuario))
                                    {
                                        Exception ex = new Exception("El correo electrónico proporcionado no se encuentra disponible") { Source = "Email" };
                                        throw ex;
                                    }

                                usuarioCtrl.Insert(dctx, usuario);
                                usuario = usuarioCtrl.LastDataRowToUsuario(usuarioCtrl.Retrieve(dctx, usuario));
                            }
                            #endregion

                            UsuarioPrivilegiosCtrl usuarioPrivilegiosCtrl = new UsuarioPrivilegiosCtrl();

                            #region registrar usuario privilegios

                            //asignamos el perfil alumno a la lista de privilegios
                            Perfil perfil = new Perfil { PerfilID = (int)EPerfil.DOCENTE };

                            List<IPrivilegio> privilegios = new List<IPrivilegio>();
                            privilegios.Add(perfil);

                            usuarioPrivilegiosCtrl.InsertUsuarioEscolarPrivilegios(dctx, usuario, licenciaEscuela.Escuela, licenciaEscuela.CicloEscolar, privilegios);

                            #endregion

                            LicenciaDocente licenciaDocente = licenciaEscuelaCtrl.RetrieveLicenciaDocente(dctx, licenciaEscuela, docente, usuario);
                            if (licenciaDocente.LicenciaID != null)
                                usuarioSocial = licenciaDocente.UsuarioSocial;

                            #region Registro de UsuarioSocial
                            UsuarioSocialCtrl usrSocialCtrl = new UsuarioSocialCtrl();
                            if (usuarioSocial.UsuarioSocialID != null)
                            {
                                usuarioSocial = usrSocialCtrl.LastDataRowToUsuarioSocial(usrSocialCtrl.Retrieve(dctx, usuarioSocial));

                                UsuarioSocial originalUsrSocial = (UsuarioSocial)usuarioSocial.Clone();
                                originalUsrSocial.Estatus = true;
                                originalUsrSocial.ScreenName = docente.Nombre + " " + docente.PrimerApellido + ((docente.SegundoApellido == null || docente.SegundoApellido.Trim().Length == 0) ? "" : " " + docente.SegundoApellido);
                                originalUsrSocial.FechaNacimiento = docente.FechaNacimiento;
                                usrSocialCtrl.Update(dctx, usuarioSocial, originalUsrSocial);

                                usuarioSocial = usrSocialCtrl.LastDataRowToUsuarioSocial(usrSocialCtrl.Retrieve(dctx, new UsuarioSocial { UsuarioSocialID = originalUsrSocial.UsuarioSocialID }));

                            }
                            else
                            {
                                usuarioSocial.Email = docente.Correo;
                                usuarioSocial.LoginName = usuario.NombreUsuario;
                                usuarioSocial.ScreenName = docente.Nombre + " " + docente.PrimerApellido + ((docente.SegundoApellido == null || docente.SegundoApellido.Trim().Length == 0) ? "" : " " + docente.SegundoApellido);
                                usuarioSocial.FechaNacimiento = docente.FechaNacimiento;
                                usuario.FechaCreacion = DateTime.Now;
                                usuarioSocial.Estatus = true;

                                usrSocialCtrl.Insert(dctx, usuarioSocial);
                                usuarioSocial = usrSocialCtrl.LastDataRowToUsuarioSocial(usrSocialCtrl.Retrieve(dctx, usuarioSocial));
                            }
                            #endregion

                            licenciaEscuelaCtrl.InsertLicenciaDocente(dctx, licenciaEscuela, docente, usuario, usuarioSocial);

                            AsignacionMateriaGrupo asignacionMateriaGrupo = new AsignacionMateriaGrupo() { Docente = docente, Materia = new Materia() { MateriaID = 18 } };
                            GrupoCicloEscolar grupoCicloEscolar = new GrupoCicloEscolar();

                            grupoCicloEscolar.CicloEscolar = new CicloEscolar() { CicloEscolarID = 3 };
                            grupoCicloEscolar.GrupoSocialID = 1;
                            grupoCicloEscolar.GrupoCicloEscolarID = new Guid("39f5a1b8-20f0-437c-aa13-78f5974d881a");
                            grupoCicloEscolar.Grupo = new Grupo() { GrupoID = new Guid("{4537c8f9-089e-42d4-bfd5-a54ee0d99d75}"), Grado = 1 };
                            grupoCicloEscolar.Escuela = new Escuela { EscuelaID = escuelaActual.EscuelaID };

                            (new AsignarDocenteGrupoCicloEscolarCtrl()).InsertAsignacionDocenteGrupoCicloEscolar(dctx, asignacionMateriaGrupo.Docente, asignacionMateriaGrupo.Materia, grupoCicloEscolar, new List<AreaConocimiento>(), (long)universidad.UniversidadID);
                            dctx.CommitTransaction(tran);

                            #region *** Vincular Universidad-Docente
                            var universidadSave = universidadCtrl.RetrieveWithRelationship(universidad, true).FirstOrDefault();

                            EFDocenteCtrl efDocenteCtrl = new EFDocenteCtrl(contexto);

                            Docente docenteSeleccionado = efDocenteCtrl.Retrieve(docente, true).FirstOrDefault();
                            var result = universidadSave.Docentes.FirstOrDefault(x => x.DocenteID == docente.DocenteID);
                            if (universidadSave.Docentes.FirstOrDefault(x => x.DocenteID == docente.DocenteID) == null)
                            {
                                universidadSave.Docentes.Add(docenteSeleccionado);
                            }
                            universidadCtrl.Update(universidadSave);
                            #endregion

                            contexto.Commit(objeto);

                            string lines = @"Nombre: " + docente.Nombre + " Primer Apellido: " + docente.PrimerApellido + " Segundo Apellido: " + docente.SegundoApellido + " Curp: "
                                + docente.Curp + " Sexo: " + sexo + " Fecha Nacimiento: " + docente.FechaNacimiento + " Fecha Registro: " + DateTime.Now;

                            var serverPath = System.Web.Hosting.HostingEnvironment.MapPath(path);
                            StreamWriter sw = new StreamWriter(serverPath, true);
                            sw.WriteLine(lines.Trim());
                            sw.Close();

                        }
                        catch (Exception ex)
                        {
                            dctx.RollbackTransaction(tran);
                            contexto.Database.BeginTransaction().Rollback();
                            POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
                            row.SetField<bool>("Cargado", false);
                            rwResultado.SetField<string>("Inconsistencia", rwResultado.Field<string>("Inconsistencia") + "\n" + ex.Message);
                            dtResultado.Rows.Add(rwResultado);
                        }
                    }
                    catch (Exception ex)
                    {
                        dctx.RollbackTransaction(tran);
                        contexto.Database.BeginTransaction().Rollback();
                        POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
                        row.SetField<bool>("Cargado", false);
                        rwResultado.SetField<string>("Inconsistencia", rwResultado.Field<string>("Inconsistencia") + "\n" + ex.Message);
                        dtResultado.Rows.Add(rwResultado);
                    }
                }
            }
            finally
            {
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(con);
                contexto.Dispose();
            }

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
    }
}
