using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
using POV.Expediente.BO;
using POV.Expediente.Services;
using System.Net.Mail;
using System.Web;
using System.Configuration;
using POV.Logger.Service;
using System.Data.Entity.Validation;

namespace POV.Administracion.Service
{
    public class CargarAlumnoCtrl
    {
        private DataSet dsAlumnos = new DataSet();

        public DataSet DsAlumnos
        {
            get { return dsAlumnos.Tables[0] != null ? dsAlumnos : null; }
        }

        private DataSet ValidarArchivoAlumno(string urlArchivo)
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
            catch (Exception ex)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
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
            if (!dsArchivoCarga.Tables[0].Columns.Contains("Matricula"))
                error.Append(" ,Matricula");
            if (!dsArchivoCarga.Tables[0].Columns.Contains("CorreoElectronico"))
                error.Append(" ,CorreoElectronico");
            if (!dsArchivoCarga.Tables[0].Columns.Contains("Direccion"))
                error.Append(" ,Direccion");
            if (!dsArchivoCarga.Tables[0].Columns.Contains("ClaveEscolar"))
                error.Append(" ,ClaveEscolar");
            if (!dsArchivoCarga.Tables[0].Columns.Contains("CURPOrientador"))
                error.Append(" ,CURPOrientador");
            if (!dsArchivoCarga.Tables[0].Columns.Contains("NombreTutor"))
                error.Append(" ,NombreTutor");
            if (!dsArchivoCarga.Tables[0].Columns.Contains("PrimerApellidoTutor"))
                error.Append(" ,PrimerApellidoTutor");
            if (!dsArchivoCarga.Tables[0].Columns.Contains("SegundoApellidoTutor"))
                error.Append(" ,SegundoApellidoTutor");
            if (!dsArchivoCarga.Tables[0].Columns.Contains("ParentescoTutor"))
                error.Append(" ,ParentescoTutor");


            if (error.Length > 0)
                throw new Exception("Las siguiente columnas no estan presentes: " + error.ToString().Substring(2));

            if (dsArchivoCarga.Tables[0].Rows.Count == 0)
                throw new Exception("El archivo cargado no contiene estudiantes.");

            return dsArchivoCarga;
        }

        public DataTable CargarArchivoAlumno(IDataContext dctx, string urlArchivo, Escuela escuela, CicloEscolar cicloEscolar)
        {
            if (escuela == null || escuela.EscuelaID == null || escuela.EscuelaID.Value <= 0)
                throw new ArgumentException("Escuela es requerido.", "escuela");

            if (cicloEscolar == null || cicloEscolar.CicloEscolarID == null || cicloEscolar.CicloEscolarID.Value <= 0)
                throw new ArgumentException("CicloEscolar es requerido.", "cicloEscolar");

            DataSet dsArchivoCarga = this.ValidarArchivoAlumno(urlArchivo);

            var objeto = new object();
            var contexto = new Contexto(objeto);

            dsArchivoCarga.Tables[0].Columns.Add(new DataColumn("Cargado", typeof(bool)));
            dsArchivoCarga.Tables[0].Columns.Add(new DataColumn("RowIndex", typeof(int)));
            dsAlumnos = dsArchivoCarga;

            #region Crear estructura de respuesta
            DataTable dtResultado = new DataTable();
            dtResultado.Columns.Add(new DataColumn("RowIndex", typeof(int)));

            dtResultado.Columns.Add(new DataColumn("AlumnoCURP", typeof(string)));
            dtResultado.Columns.Add(new DataColumn("ExisteAlumno", typeof(bool)));
            dtResultado.Columns.Add(new DataColumn("ActivoAlumno", typeof(bool)));
            dtResultado.Columns.Add(new DataColumn("Inconsistencia", typeof(string)));

            dtResultado.Columns.Add(new DataColumn("EscuelaClave", typeof(string)));
            dtResultado.Columns.Add(new DataColumn("ExisteEscuela", typeof(bool)));
            dtResultado.Columns.Add(new DataColumn("ActivoEscuela", typeof(bool)));

            dtResultado.Columns.Add(new DataColumn("OrientadorCURP", typeof(string)));
            dtResultado.Columns.Add(new DataColumn("ExisteOrientador", typeof(bool)));
            dtResultado.Columns.Add(new DataColumn("ActivoOrientador", typeof(bool)));

            dtResultado.Columns.Add(new DataColumn("TutorNombre", typeof(string)));
            dtResultado.Columns.Add(new DataColumn("ExisteTutor", typeof(bool)));
            dtResultado.Columns.Add(new DataColumn("ActivoTutor", typeof(bool)));
            #endregion

            object con = new object();

            string path = System.Configuration.ConfigurationManager.AppSettings["POVUrlResultadoExportacionAlumno"];

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


                licenciaEscuela = licenciaEscuelaCtrl.LastDataRowToLicenciaEscuela(ds);

                foreach (DataRow row in dsArchivoCarga.Tables[0].Rows)
                {
                    object tran = new object();

                    #region Inicializacion de Resultado de inconsistencias
                    DataRow rwResultado = dtResultado.NewRow();
                    rwResultado.SetField<int>("RowIndex", dsArchivoCarga.Tables[0].Rows.IndexOf(row));
                    row.SetField<int>("RowIndex", dsArchivoCarga.Tables[0].Rows.IndexOf(row));

                    rwResultado.SetField<string>("AlumnoCURP", "");
                    rwResultado.SetField<bool>("ExisteAlumno", false);
                    rwResultado.SetField<bool>("ActivoAlumno", false);
                    rwResultado.SetField<string>("Inconsistencia", "");

                    rwResultado.SetField<string>("EscuelaClave", "");
                    rwResultado.SetField<bool>("ExisteEscuela", false);
                    rwResultado.SetField<bool>("ActivoEscuela", false);

                    rwResultado.SetField<string>("OrientadorCURP", "");
                    rwResultado.SetField<bool>("ExisteOrientador", false);
                    rwResultado.SetField<bool>("ActivoOrientador", false);

                    rwResultado.SetField<bool>("TutorNombre", false);
                    rwResultado.SetField<bool>("ExisteTutor", false);
                    rwResultado.SetField<bool>("ActivoTutor", false);
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
                            string curp = row.IsNull("Curp") ? "" : row["Curp"].ToString();
                            string sexo = row.IsNull("Sexo") ? "" : row["Sexo"].ToString();
                            string matricula = row.IsNull("Matricula") ? "" : row["Matricula"].ToString();
                            string correoElectronico = row.IsNull("CorreoElectronico") ? "" : row["CorreoElectronico"].ToString();
                            string direccion = row.IsNull("Direccion") ? "" : row["Direccion"].ToString();
                            string claveEscolar = row.IsNull("ClaveEscolar") ? "" : row["ClaveEscolar"].ToString();
                            string orientadorCURP = row.IsNull("CURPOrientador") ? "" : row["CURPOrientador"].ToString();
                            string nivelEscolar = row.IsNull("NivelEscolar") ? "" : row["NivelEscolar"].ToString();
                            string estatusPago = row.IsNull("EstatusPago") ? "" : row["EstatusPago"].ToString();
                            string nombreTutor = row.IsNull("NombreTutor") ? "" : row["NombreTutor"].ToString();
                            string primerApellidoTutor = row.IsNull("PrimerApellidoTutor") ? "" : row["PrimerApellidoTutor"].ToString();
                            string segundoApellidoTutor = row.IsNull("SegundoApellidoTutor") ? "" : row["SegundoApellidoTutor"].ToString();
                            string parentescoTutor = row.IsNull("ParentescoTutor") ? "" : row["ParentescoTutor"].ToString();
                            #endregion

                            #region Validaciones de datos requeridos del sistema
                            try
                            {
                                string sError = string.Empty;
                                #region Requeridos
                                // valores requerido
                                if (string.IsNullOrEmpty(nombre))
                                    sError += " ,Nombre";
                                if (string.IsNullOrEmpty(primerApellido))
                                    sError += " ,Primer Apellido";
                                if (string.IsNullOrEmpty(fechaNacimiento))
                                    sError += " ,Fecha Nacimiento";
                                if (string.IsNullOrEmpty(curp))
                                    sError += " , CURP";
                                if (string.IsNullOrEmpty(matricula))
                                    sError += " , Matrícula";
                                if (string.IsNullOrEmpty(sexo))
                                    sError += " ,Sexo";
                                if (string.IsNullOrEmpty(correoElectronico))
                                    sError += " , Correo Electrónico";
                                if (string.IsNullOrEmpty(claveEscolar))
                                    sError += " , Clave Escolar";
                                if (string.IsNullOrEmpty(orientadorCURP))
                                    sError += " , CURP Orientador";
                                if (string.IsNullOrEmpty(nivelEscolar))
                                    sError += " , Nivel Escolar";
                                if (string.IsNullOrEmpty(estatusPago))
                                    sError += " , Estatus Pago";
                                switch (nivelEscolar)
                                {
                                    case "Básico":
                                        if (string.IsNullOrEmpty(nombreTutor))
                                            sError += " , Nombre Tutor";
                                        if (string.IsNullOrEmpty(primerApellidoTutor))
                                            sError += " ,Primer Apellido Tutor";
                                        if (string.IsNullOrEmpty(parentescoTutor))
                                            sError += " ,Parentesco Tutor";
                                        break;
                                    case "Medio":
                                        if (string.IsNullOrEmpty(nombreTutor))
                                            sError += " , Nombre Tutor";
                                        if (string.IsNullOrEmpty(primerApellidoTutor))
                                            sError += " ,Primer Apellido Tutor";
                                        if (string.IsNullOrEmpty(parentescoTutor))
                                            sError += " ,Parentesco Tutor";
                                        break;
                                }


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
                                if (correoElectronico.Trim().Length > 100)
                                    sError += " ,Correo Electrónico";
                                if (orientadorCURP.Trim().Length > 18)
                                    sError += " ,CURP Orientador";
                                if (nombreTutor.Trim().Length > 80)
                                    sError += " ,Nombre Tutor";
                                if (primerApellidoTutor.Trim().Length > 50)
                                    sError += " ,Primer Apellido Tutor";
                                if (segundoApellidoTutor.Trim().Length > 50)
                                    sError += " ,Segundo Apellido Tutor";

                                if (sError.Trim().Length > 0)
                                {
                                    sError = sError.Substring(2);
                                    throw new Exception(string.Format("Los siguientes parámetros son inválidos :{0}", sError));
                                }
                                #endregion

                                if (!ValidateEmailRegex(correoElectronico.Trim()))
                                    sError += " ,Correo Electrónico";

                                if (!ValidateCurpRegex(curp.Trim()))
                                    sError += " ,CURP Estudiante";

                                if (!ValidateCurpRegex(orientadorCURP.Trim()))
                                    sError += " ,CURP Orientador";


                                DateTime fn;
                                DateTime tmpFecha = new DateTime();
                                tmpFecha = Convert.ToDateTime(fechaNacimiento);
                                fechaNacimiento = tmpFecha.ToShortDateString();
                                if (!DateTime.TryParseExact(fechaNacimiento.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out fn))
                                    sError += " ,Fecha Nacimiento";
                                if (sError.Trim().Length > 0)
                                {
                                    sError = sError.Substring(2);
                                    throw new Exception(string.Format("Los siguientes parámetros tienen un formato no válido:{0}", sError));
                                }
                            }
                            catch (Exception ex)
                            {
                                rwResultado.SetField<string>("Inconsistencia", rwResultado.Field<string>("Inconsistencia") + "\n" + ex.Message);
                            }
                            #endregion

                            Alumno alumno = new Alumno();
                            AlumnoCtrl alumnoCtrl = new AlumnoCtrl();

                            Universidad existeEscuela = new Universidad();
                            Universidad universidad = new Universidad();
                            UniversidadCtrl universidadCtrl = new UniversidadCtrl(contexto);

                            Docente existeDocente = new Docente();
                            Docente orientador = new Docente();
                            EFDocenteCtrl efDocenteCtrl = new EFDocenteCtrl(contexto);

                            Tutor existeTutor = new Tutor();
                            Tutor tutor = new Tutor();
                            TutorCtrl efTutorCtrl = new TutorCtrl(contexto);

                            EFAlumnoCtrl efAlumnoCtrl = new EFAlumnoCtrl(contexto);

                            UsuarioExpedienteCtrl usuarioExpedienteCtrl = new UsuarioExpedienteCtrl();

                            TutorAlumnoCtrl tutorAlumnoCtrl = new TutorAlumnoCtrl(contexto);

                            #region Existe Escuela, Alumno, Orientador, Tutor
                            try
                            {
                                #region Alumno
                                alumno.Curp = curp;

                                ds = alumnoCtrl.Retrieve(dctx, alumno);
                                rwResultado.SetField<bool>("ExisteAlumno", ds.Tables[0].Rows.Count != 0);

                                if (rwResultado.Field<bool>("ExisteAlumno"))
                                {
                                    alumno = alumnoCtrl.LastDataRowToAlumno(ds);
                                    rwResultado.SetField<bool>("ActivoAlumno", alumno.Estatus.Value);
                                }
                                #endregion

                                #region Escuela
                                existeEscuela.ClaveEscolar = claveEscolar;
                                universidad = universidadCtrl.Retrieve(new Universidad { ClaveEscolar = existeEscuela.ClaveEscolar }, false).FirstOrDefault();
                                rwResultado.SetField<bool>("ExisteEscuela", universidad != null);

                                if (rwResultado.Field<bool>("ExisteEscuela"))
                                {
                                    rwResultado.SetField<bool>("ActivoEscuela", universidad.Activo.Value);
                                }
                                #endregion

                                #region Orientador
                                existeDocente.Curp = orientadorCURP;

                                orientador = efDocenteCtrl.Retrieve(new Docente { Curp = existeDocente.Curp }, false).FirstOrDefault();
                                rwResultado.SetField<bool>("ExisteOrientador", orientador != null);

                                if (rwResultado.Field<bool>("ExisteOrientador"))
                                {
                                    rwResultado.SetField<bool>("ActivoOrientador", orientador.Estatus.Value);
                                }
                                #endregion

                                #region Tutor
                                if (!string.IsNullOrEmpty(nombreTutor) && !string.IsNullOrEmpty(primerApellidoTutor))
                                {
                                    existeTutor.Nombre = nombreTutor;
                                    existeTutor.PrimerApellido = primerApellidoTutor;
                                    existeTutor.SegundoApellido = segundoApellidoTutor;

                                    tutor = efTutorCtrl.Retrieve(new Tutor { Nombre = existeTutor.Nombre, PrimerApellido = existeTutor.PrimerApellido, SegundoApellido = existeTutor.SegundoApellido }, true).FirstOrDefault();
                                    rwResultado.SetField<bool>("ExisteTutor", tutor != null);

                                    if (rwResultado.Field<bool>("ExisteTutor"))
                                    {
                                        rwResultado.SetField<bool>("ActivoTutor", tutor.Estatus.Value);
                                    }
                                }
                                #endregion
                            }
                            catch (Exception ex)
                            {
                                rwResultado.SetField<string>("Inconsistencia", rwResultado.Field<string>("Inconsistencia") + "\n" + ex.Message);
                            }
                            #endregion

                            if (rwResultado.Field<bool>("ExisteEscuela") == false && rwResultado.Field<bool>("ActivoEscuela") == false)
                                throw new Exception("La Escuela del alumno no está registrada o disponible.");

                            if (rwResultado.Field<bool>("ExisteOrientador") == false && rwResultado.Field<bool>("ActivoOrientador") == false)
                                throw new Exception("El Orientador del alumno no está registrado o disponible.");

                            if (!string.IsNullOrEmpty(nombreTutor) && !string.IsNullOrEmpty(primerApellidoTutor))
                            {
                                if (rwResultado.Field<bool>("ExisteTutor") == false && rwResultado.Field<bool>("ActivoTutor") == false)
                                    throw new Exception("El Tutor de alumno no está registrado o disponible.");
                            }

                            if (rwResultado.Field<string>("Inconsistencia") == "")
                                registrar = true;

                            row.SetField<bool>("Cargado", registrar);

                            if (!registrar)
                            {
                                dctx.RollbackTransaction(tran);
                                dtResultado.Rows.Add(rwResultado);
                                continue;
                            }

                            Usuario usuario = new Usuario();
                            UsuarioSocial usuarioSocial = new UsuarioSocial();


                            #region Registrar Alumno

                            Docente docenteSelect = efDocenteCtrl.Retrieve(orientador, true).FirstOrDefault();
                            Usuario userOrientador = licenciaEscuelaCtrl.RetrieveUsuarios(dctx, docenteSelect).Where(x => x.UniversidadId == universidad.UniversidadID).FirstOrDefault();
                            if (userOrientador == null)
                                throw new Exception("El orientador no pertenece a la escuela");


                            if (rwResultado.Field<bool>("ExisteAlumno")) // si el Alumno existe, actualizamos Alumno
                            {
                                Alumno aAlumno = (Alumno)alumno.Clone();
                                aAlumno.Nombre = nombre;
                                aAlumno.PrimerApellido = primerApellido;
                                aAlumno.SegundoApellido = segundoApellido;
                                aAlumno.Matricula = matricula;
                                aAlumno.Estatus = true;
                                aAlumno.FechaNacimiento = DateTime.ParseExact(fechaNacimiento, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                aAlumno.Sexo = sexo.Equals("Hombre");
                                aAlumno.Direccion = direccion;
                                alumno.Curp = curp;
                                switch (nivelEscolar)
                                {
                                    case "Básico":
                                        alumno.NivelEscolar = ENivelEscolar.Basico;
                                        break;
                                    case "Medio":
                                        alumno.NivelEscolar = ENivelEscolar.Medio;
                                        break;
                                    case "Superior":
                                        alumno.NivelEscolar = ENivelEscolar.Superior;
                                        break;
                                }

                                switch (estatusPago)
                                {
                                    case "Sin pago":
                                        alumno.EstatusPago = EEstadoPago.NO_PAGADO;
                                        break;
                                    case "Pendiente":
                                        alumno.EstatusPago = EEstadoPago.PENDIENTE;
                                        break;
                                    case "Pagado":
                                        alumno.EstatusPago = EEstadoPago.PAGADO;
                                        break;
                                }

                                alumnoCtrl.Update(dctx, alumno, aAlumno); // actualizamos Alumno
                                alumno = aAlumno;

                                usuario = licenciaEscuelaCtrl.RetrieveUsuario(dctx, alumno);
                            }
                            else // si el Alumno no existe, registramos Alumno
                            {
                                alumno.Nombre = nombre;
                                alumno.PrimerApellido = primerApellido;
                                alumno.SegundoApellido = segundoApellido;
                                alumno.Matricula = matricula;
                                alumno.FechaNacimiento = DateTime.ParseExact(fechaNacimiento, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                alumno.Direccion = direccion;
                                alumno.Sexo = sexo.Equals("Hombre");
                                alumno.FechaRegistro = DateTime.Now;
                                alumno.Estatus = true;
                                alumno.CorreoConfirmado = false;
                                alumno.EstatusIdentificacion = false;
                                alumno.CarreraSeleccionada = false;
                                alumno.Curp = curp;
                                alumno.Premium = false;
                                switch (nivelEscolar)
                                {
                                    case "Básico":
                                        alumno.NivelEscolar = ENivelEscolar.Superior;
                                        break;
                                    case "Medio":
                                        alumno.NivelEscolar = ENivelEscolar.Medio;
                                        break;
                                    case "Superior":
                                        alumno.NivelEscolar = ENivelEscolar.Superior;
                                        break;
                                }

                                switch (estatusPago)
                                {
                                    case "Sin pago":
                                        alumno.EstatusPago = EEstadoPago.NO_PAGADO;
                                        break;
                                    case "Pendiente":
                                        alumno.EstatusPago = EEstadoPago.PENDIENTE;
                                        break;
                                    case "Pagado":
                                        alumno.EstatusPago = EEstadoPago.PAGADO;
                                        break;
                                }

                                alumnoCtrl.Insert(dctx, alumno); // registramos Alumno
                                alumno = alumnoCtrl.LastDataRowToAlumno(alumnoCtrl.Retrieve(dctx, alumno)); // obtenemos Alumno

                                usuario = licenciaEscuelaCtrl.RetrieveUsuario(dctx, alumno);
                            }

                            #endregion

                            UsuarioCtrl usuarioCtrl = new UsuarioCtrl();
                            new Seguridad.Utils.PasswordProvider().GetNewPassword();
                            string passwordTemp = "";
                            #region Registrar Usuario
                            if (usuario.UsuarioID != null)
                            {
                                usuario = usuarioCtrl.LastDataRowToUsuario(usuarioCtrl.Retrieve(dctx, usuario));
                                if (usuario.EsActivo == false || (usuario.Email != correoElectronico))
                                {
                                    Usuario original = (Usuario)usuario.Clone();
                                    usuario.EsActivo = true;
                                    usuario.Email = correoElectronico;

                                    if (!string.IsNullOrEmpty(usuario.Email))
                                    {

                                        if (!EmailDisponible(dctx, usuario))
                                        {
                                            Exception ex = new Exception("El correo electrónico proporcionado no se encuentra disponible") { Source = "Email" };
                                            throw ex;
                                        }
                                        else
                                        {
                                            passwordTemp = new PasswordProvider(8).GetNewPassword();
                                            byte[] pws = EncryptHash.SHA1encrypt(passwordTemp);
                                            usuario.Password = pws;
                                            usuario.EmailVerificado = false;
                                        }
                                    }

                                    usuarioCtrl.Update(dctx, usuario, original);
                                    int usuarioID = usuario.UsuarioID.Value;
                                    usuario = new Usuario();
                                    usuario.UsuarioID = usuarioID;
                                    usuario = usuarioCtrl.LastDataRowToUsuario(usuarioCtrl.Retrieve(dctx, usuario));
                                }
                            }
                            else
                            {
                                usuario.NombreUsuario = usuarioCtrl.GenerarNombreUsuarioUnico(dctx, alumno.Nombre, alumno.PrimerApellido, alumno.FechaNacimiento.Value);
                                usuario.Email = correoElectronico;
                                passwordTemp = new PasswordProvider(8).GetNewPassword();
                                byte[] pws = EncryptHash.SHA1encrypt(passwordTemp);
                                usuario.Password = pws;
                                usuario.EmailVerificado = false;
                                usuario.EsActivo = true;
                                usuario.FechaCreacion = DateTime.Now;

                                usuario.Termino = new GP.SocialEngine.BO.Termino() { TerminoID = 1, Cuerpo = "Terminos legales", Estatus = true };

                                if (alumno.NivelEscolar != ENivelEscolar.Superior)
                                    usuario.AceptoTerminos = true;
                                else
                                    usuario.AceptoTerminos = false;

                                usuario.PasswordTemp = true;

                                if (!string.IsNullOrEmpty(usuario.NombreUsuario))
                                    if (UsuarioExiste(dctx, usuario))
                                    {
                                        Exception ex = new Exception("El nombre de usuario ya se encuentra registrado") { Source = "Usuario" };
                                        throw ex;
                                    }

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
                            Perfil perfil = new Perfil { PerfilID = (int)EPerfil.ALUMNO };

                            List<IPrivilegio> privilegios = new List<IPrivilegio>();
                            privilegios.Add(perfil);

                            usuarioPrivilegiosCtrl.InsertUsuarioEscolarPrivilegios(dctx, usuario, licenciaEscuela.Escuela, licenciaEscuela.CicloEscolar, privilegios);

                            #endregion

                            UsuarioSocialCtrl usrSocialCtrl = new UsuarioSocialCtrl();

                            // Consultar licencia del alumno
                            LicenciaAlumno licenciaAlumno = licenciaEscuelaCtrl.RetrieveLicenciaAlumno(dctx, licenciaEscuela, alumno, usuario);
                            UsuarioSocial antusuariosocial = null;
                            #region Registrar UsuarioSocial
                            if (licenciaAlumno != null && licenciaAlumno.LicenciaID != null)
                            {
                                // Actualizar usuario social
                                usuarioSocial = licenciaAlumno.UsuarioSocial;
                                usuarioSocial = usrSocialCtrl.LastDataRowToUsuarioSocial(usrSocialCtrl.Retrieve(dctx, usuarioSocial));

                                UsuarioSocial nuevoUsrSocial = (UsuarioSocial)usuarioSocial.Clone();
                                nuevoUsrSocial.Estatus = true;
                                nuevoUsrSocial.ScreenName = alumno.Nombre + " " + alumno.PrimerApellido + ((alumno.SegundoApellido == null || alumno.SegundoApellido.Trim().Length == 0) ? "" : " " + alumno.SegundoApellido);
                                nuevoUsrSocial.FechaNacimiento = alumno.FechaNacimiento;
                                nuevoUsrSocial.Email = null;
                                nuevoUsrSocial.LoginName = usuarioSocial.LoginName;
                                usrSocialCtrl.Update(dctx, nuevoUsrSocial, usuarioSocial);
                                long usuarioSocialID = usuarioSocial.UsuarioSocialID.Value;
                                antusuariosocial = usrSocialCtrl.LastDataRowToUsuarioSocial(usrSocialCtrl.Retrieve(dctx, new UsuarioSocial { UsuarioSocialID = nuevoUsrSocial.UsuarioSocialID }));
                                usuarioSocial = antusuariosocial;
                            }
                            else
                            {
                                // Registrar Usuario Social
                                usuarioSocial.LoginName = usuario.NombreUsuario;
                                usuarioSocial.ScreenName = alumno.Nombre + " " + alumno.PrimerApellido + ((alumno.SegundoApellido == null || alumno.SegundoApellido.Trim().Length == 0) ? "" : " " + alumno.SegundoApellido);
                                usuarioSocial.FechaNacimiento = alumno.FechaNacimiento;
                                usuarioSocial.Estatus = true;

                                usrSocialCtrl.Insert(dctx, usuarioSocial);
                                usuarioSocial = usrSocialCtrl.LastDataRowToUsuarioSocial(usrSocialCtrl.Retrieve(dctx, usuarioSocial));
                            }
                            #endregion

                            // Registro social hub Grupo social
                            #region Registrar SocialHub
                            SocialHubCtrl socialHubCtrl = new SocialHubCtrl();
                            //Crear el socialHub de no existir
                            socialHubCtrl.InsertSocialHubUsuarioSocial(dctx, usuarioSocial);
                            SocialHub socialHub = new SocialHub();
                            socialHub.SocialProfile = usuarioSocial;
                            socialHub.SocialProfileType = ESocialProfileType.USUARIOSOCIAL;
                            DataSet dsSocialHub = socialHubCtrl.RetrieveSocialHubUsuario(dctx, socialHub);
                            socialHub = socialHubCtrl.LastDataRowToSocialHub(dsSocialHub);
                            GrupoSocial grupoSocial = new GrupoSocial { GrupoSocialID = 1 };
                            #endregion

                            socialHubCtrl.InsertGrupoSocialSocialHub(dctx, grupoSocial, socialHub);

                            UsuarioGrupoCtrl usrGrupoCtrl = new UsuarioGrupoCtrl();
                            UsuarioGrupo usuarioGrupo = new UsuarioGrupo();

                            // Asignar Grupo ciclo escolar
                            GrupoCicloEscolarCtrl gpoCicloEscolarCtrl = new GrupoCicloEscolarCtrl();
                            GrupoCicloEscolar grupoCicloEscolar = new GrupoCicloEscolar();
                            grupoCicloEscolar.GrupoCicloEscolarID = new Guid("39f5a1b8-20f0-437c-aa13-78f5974d881a");
                            grupoCicloEscolar.CicloEscolar = new CicloEscolar() { CicloEscolarID = 3 };
                            gpoCicloEscolarCtrl.InsertAsignacionAlumno(dctx, alumno, grupoCicloEscolar);

                            licenciaEscuelaCtrl.InsertLicenciaAlumno(dctx, licenciaEscuela, alumno, usuario, usuarioSocial);

                            //Registrar UsuarioGrupo -  para publicar
                            usuarioGrupo.FechaAsignacion = DateTime.Now;
                            usuarioGrupo.Estatus = true;
                            usuarioGrupo.EsModerador = false;
                            usuarioGrupo.UsuarioSocial = usuarioSocial;

                            usrGrupoCtrl.Insert(dctx, usuarioGrupo, grupoSocial);

                            #region Escuela
                            AsignacionUniversidadCtrl universidadesCtrl = new AsignacionUniversidadCtrl();
                            //TODO: revisar
                            Universidad escuelaSeleccionada = universidadesCtrl.LastDataRowToUniversidad(universidadesCtrl.RetrieveUniversidad(dctx, universidad));
                            UniversidadAlumno universidadAlumno = new UniversidadAlumno();
                            universidadAlumno.UniversidadID = escuelaSeleccionada.UniversidadID;
                            universidadAlumno.AlumnoID = alumno.AlumnoID;
                            UniversidadAlumno universidadAlumnoRegistrado = universidadesCtrl.LastDataRowToUniversidadAlumno(universidadesCtrl.RetrieveUniversidadAlumno(dctx, universidadAlumno));
                            if (universidadAlumnoRegistrado == null)
                                universidadesCtrl.InsertUniversidadAlumno(dctx, universidadAlumno);

                            #endregion

                            #region Orientador
                            UsuarioExpediente usuarioExpediente = new UsuarioExpediente();

                            usuarioExpediente.AlumnoID = alumno.AlumnoID;
                            usuarioExpediente.UsuarioID = userOrientador.UsuarioID;
                            UsuarioExpediente usuarioExpedienteRegistrado = usuarioExpedienteCtrl.LastDataRowToUsuarioExpediente(usuarioExpedienteCtrl.Retrieve(dctx, usuarioExpediente));
                            if (usuarioExpedienteRegistrado == null)
                                usuarioExpedienteCtrl.Insert(dctx, usuarioExpediente);
                            #endregion

                            dctx.CommitTransaction(tran);

                            #region Vinculacion de Alumno Escuela, Orientador, Tutor
                            Alumno alumnoSelect = efAlumnoCtrl.Retrieve(new Alumno { AlumnoID = alumno.AlumnoID }, true).FirstOrDefault();
                            
                            #region Tutor
                            int parentesco = 0;
                            switch (parentescoTutor)
                            {
                                case "Padre":
                                    parentesco = 1;
                                    break;
                                case "Madre":
                                    parentesco = 2;
                                    break;
                                case "Familiar":
                                    parentesco = 3;
                                    break;
                            }

                            if (!string.IsNullOrEmpty(nombreTutor) && !string.IsNullOrEmpty(primerApellidoTutor))
                            {
                                TutorAlumno tutorAlumnoFind = new TutorAlumno();
                                tutorAlumnoFind.Tutor = efTutorCtrl.Retrieve(new Tutor { TutorID = tutor.TutorID }, true).FirstOrDefault();
                                tutorAlumnoFind.TutorID = tutor.TutorID;
                                tutorAlumnoFind.Parentesco = (Int16)parentesco;
                                if (alumnoSelect.Tutores.FirstOrDefault(x => x.TutorID == tutorAlumnoFind.TutorID) == null)
                                {
                                    alumnoSelect.Tutores.Add(tutorAlumnoFind);
                                }
                            }
                            #endregion
                            #endregion

                            efAlumnoCtrl.Update(alumnoSelect);
                            contexto.Commit(objeto);

                            string lines = @"Nombre: " + alumno.Nombre + " Primer Apellido: " + alumno.PrimerApellido + " Segundo Apellido: " + alumno.SegundoApellido
                                + " Matrícula: " + alumno.Matricula + " Curp: " + alumno.Curp + " Sexo: " + sexo + " Fecha Nacimiento: " + alumno.FechaNacimiento
                                + " Fecha Registro: " + DateTime.Now;

                            var serverPath = System.Web.Hosting.HostingEnvironment.MapPath(path);
                            StreamWriter sw = new StreamWriter(serverPath, true);
                            sw.WriteLine(lines.Trim());
                            sw.Close();
                        }
                        catch (Exception ex)
                        {
                            dctx.RollbackTransaction(tran);
                            POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
                            row.SetField<bool>("Cargado", false);
                            rwResultado.SetField<string>("Inconsistencia", rwResultado.Field<string>("Inconsistencia") + "\n" + ex.Message);
                            dtResultado.Rows.Add(rwResultado);
                            //continue;
                        }
                    }
                    catch (Exception ex)
                    {
                        dctx.RollbackTransaction(tran);
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

        public bool UsuarioExiste(IDataContext dctx, Usuario usuario)
        {
            if (usuario == null || string.IsNullOrEmpty(usuario.Email))
                throw new ArgumentException("Usuario requerido", "usuario");
            UsuarioCtrl usuarioCtrl = new UsuarioCtrl();
            DataSet ds = usuarioCtrl.Retrieve(dctx, new Usuario { NombreUsuario = usuario.NombreUsuario, EsActivo = true });
            int index = ds.Tables[0].Rows.Count;
            if (index == 1 && usuario.NombreUsuario != null)
            {
                Usuario usr = usuarioCtrl.LastDataRowToUsuario(ds);
                if (usr.NombreUsuario == usuario.NombreUsuario && usr.UsuarioID != null)
                    return true;
            }

            else if (index <= 0)
                return false;
            return false;
        }

        public bool EmailDisponible(IDataContext dctx, Usuario usuario)
        {
            if (usuario == null || string.IsNullOrEmpty(usuario.Email))
                throw new ArgumentException("Email requerido", "usuario");
            UsuarioCtrl usuarioCtrl = new UsuarioCtrl();
            DataSet ds = usuarioCtrl.Retrieve(dctx, new Usuario { Email = usuario.Email, EsActivo = true });
            int index = ds.Tables[0].Rows.Count;
            if (index == 1 && usuario.UsuarioID != null)
            {
                Usuario usr = usuarioCtrl.LastDataRowToUsuario(ds);
                return usr.UsuarioID == usuario.UsuarioID;
            }

            if (index <= 0)
                return true;

            return false;
        }

        //Enviar Correo de registro exitoso
        private void enviarCorreo(Usuario usuario, Alumno alumno, string password)
        {
            CorreoCtrl correoCtrl = new CorreoCtrl();
            #region Variables
            string urllogo = ConfigurationManager.AppSettings["POVUrlImgLogo"];
            string urlimgbackground = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgBackground"];
            string urlimgface = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgFace"];
            string urlimgtwit = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgTwit"];
            string urlimglinked = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgLinked"];
            string urlimginstagram = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgInstagram"];
            const string imgalt = "YOY - ESTUDIANTES";
            const string titulo = "Registro exitoso";
            string dominio = String.Format("{0}://{1}:{2}/", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.Port);
            string linkportal = System.Configuration.ConfigurationManager.AppSettings["POVUrlPortalAspirante"];
            string linkAutologin = linkportal + GenerarTokenYUrl(alumno);
            #endregion

            string cuerpo = string.Empty;
            var serverPath = System.Web.Hosting.HostingEnvironment.MapPath(ConfigurationManager.AppSettings["POVUrlEmailTemplateNewUser"]);
            using (StreamReader reader = new StreamReader(serverPath))
            {
                cuerpo = reader.ReadToEnd();
            }
            cuerpo = cuerpo.Replace("{urllogo}", urllogo);
            cuerpo = cuerpo.Replace("{urlimgbackground}", urlimgbackground);
            cuerpo = cuerpo.Replace("{title}", titulo);
            cuerpo = cuerpo.Replace("{user}", usuario.NombreUsuario);
            cuerpo = cuerpo.Replace("{password}", password);
            cuerpo = cuerpo.Replace("{linkportal}", linkAutologin);
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
            }
            catch (Exception ex)
            {
                LoggerHlp.Default.Error(this, ex);
            }
        }

        private string GenerarTokenYUrl(Alumno alumno)
        {
            string strUrlAutoLogin;
            string formatoFecha = "yyyyMMddHHmmss.fff";
            DateTime fecha = System.DateTime.Now;
            string nombre = alumno.Nombre;
            nombre = nombre.Trim();
            string apellido = alumno.PrimerApellido;
            apellido = apellido.Trim();
            string curp = alumno.Curp;
            DateTime fechaNacimiento = (DateTime)alumno.FechaNacimiento;
            curp = curp.Trim();
            string token = fechaNacimiento.ToString(formatoFecha) + nombre + apellido + curp + fecha.ToString(formatoFecha);
            byte[] clave = POV.Seguridad.Utils.EncryptHash.SHA1encrypt(token);
            token = POV.Seguridad.Utils.EncryptHash.byteArrayToStringBase64(clave);
            token = System.Web.HttpUtility.UrlEncode(token);
            strUrlAutoLogin = "?alumno=" + alumno.Curp + "&fechahora=" + fecha.ToString(formatoFecha) + "&token=" + token;
            return strUrlAutoLogin;
        }
    }
}
