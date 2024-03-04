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
using POV.Logger.Service;

namespace POV.Operaciones.Service
{
    public class CargarEscuelaCtrl
    {
        private DataSet dsEscuelas = new DataSet();

        public DataSet DsEscuelas
        {
            get { return dsEscuelas.Tables[0] != null ? dsEscuelas : null; }
        }

        public string UrlImgNatware { get; set; }

        public string UrlPortalAdministrativo { get; set; }
        

        private DataSet ValidarArchivoEscuela(string urlArchivo)
        {
            DataSet dsArchivoCarga = new DataSet();
            ExcelCtrl excelCtrl = new ExcelCtrl();

            if (urlArchivo == null || (urlArchivo != null && urlArchivo == ""))
                throw new Exception("Es requerido la ruta del archivo");

            if (!File.Exists(urlArchivo))
                throw new Exception("Se encontraron inconsistencias al momento de cargar el archivo.");

            FileInfo file = new FileInfo(urlArchivo);
            if (file.Extension.ToLower() != ".xls")
                throw new Exception("El archivo seleccionado no tiene el formato o extensión correcta ( *.xls Excel 97-2003)");

            try
            {
                dsArchivoCarga = excelCtrl.Consultar(file.FullName);
            }
            catch
            {
                throw new Exception("Se encontraron inconsistencias al momento de cargar el archivo.");
            }

            StringBuilder error = new StringBuilder();

            if (!dsArchivoCarga.Tables[0].Columns.Contains("Clave"))
                error.Append(" ,Clave");
            if (!dsArchivoCarga.Tables[0].Columns.Contains("NombreEscuela"))
                error.Append(" ,NombreEscuela");
            if (!dsArchivoCarga.Tables[0].Columns.Contains("Turno"))
                error.Append(" ,Turno");
            if (!dsArchivoCarga.Tables[0].Columns.Contains("Ambito"))
                error.Append(" ,Ambito");
            if (!dsArchivoCarga.Tables[0].Columns.Contains("Control"))
                error.Append(" ,Control");
            if (!dsArchivoCarga.Tables[0].Columns.Contains("ClaveMunicipio"))
                error.Append(" ,ClaveMunicipio");
            if (!dsArchivoCarga.Tables[0].Columns.Contains("ClaveLocalidad"))
                error.Append(" ,ClaveLocalidad");
            if (!dsArchivoCarga.Tables[0].Columns.Contains("ClaveTipoServicio"))
                error.Append(" ,ClaveTipoServicio");
            if (!dsArchivoCarga.Tables[0].Columns.Contains("ClaveZona"))
                error.Append(" ,ClaveZona");
            if (!dsArchivoCarga.Tables[0].Columns.Contains("DirectorCURP"))
                error.Append(" ,DirectorCURP");
            if (!dsArchivoCarga.Tables[0].Columns.Contains("DirectorNombre"))
                error.Append(" ,DirectorNombre");
            if (!dsArchivoCarga.Tables[0].Columns.Contains("DirectorPrimerApellido"))
                error.Append(" ,DirectorPrimerApellido");
            if (!dsArchivoCarga.Tables[0].Columns.Contains("DirectorSegundoApellido"))
                error.Append(" ,DirectorSegundoApellido");
            if (!dsArchivoCarga.Tables[0].Columns.Contains("DirectorFechaNacimiento"))
                error.Append(" ,DirectorFechaNacimiento");
            if (!dsArchivoCarga.Tables[0].Columns.Contains("DirectorSexo"))
                error.Append(" ,DirectorSexo");
            if (!dsArchivoCarga.Tables[0].Columns.Contains("DirectorNivelEscolar"))
                error.Append(" ,DirectorNivelEscolar");
            if (!dsArchivoCarga.Tables[0].Columns.Contains("DirectorCorreo"))
                error.Append(" ,DirectorCorreo");
            if (!dsArchivoCarga.Tables[0].Columns.Contains("DirectorTelefono"))
                error.Append(" ,DirectorTelefono");

            if (error.Length > 0)
                throw new Exception("Las siguiente columnas no estan presentes: " + error.ToString().Substring(2));

            if (dsArchivoCarga.Tables[0].Rows.Count == 0)
                throw new Exception("El archivo cargado no contiene escuelas.");

            return dsArchivoCarga;
        }

        public bool EnviarCorreo(Usuario usuario, string pws, string clave)
        {
            bool envio = false;
            CorreoCtrl correoCtrl = new CorreoCtrl();

            #region Variables
            string urlimg = UrlImgNatware;
            const string imgalt = "Natware - Portal Administración";
            const string titulo = "NATWARE - PORTAL ADMINISTRACIÓN";
            string linkportal = UrlPortalAdministrativo;
            #endregion
            
            string cuerpo = string.Format(@"<table width='600'><tr><td>
                                            <img src='{0}' alt='{1}' /></td></tr>
                                            <tr><td><h2 style='color:#A5439A'>{2}</h2>
                                            </p><p>Estos son los datos para que accedas a tu portal.</p><p>Usuario: {3}</p>
                                            <p>Contraseña: {4}</p><p>Una vez que entres al portal, te recomendamos cambiar tu contraseña.</p>
                                            <p>Clave de activación:{5}</p></td>
                                            </tr>
                                            <tr><td>
                                            <a href='{6}'>Natware - Portal administraci&oacute;n</a>
                                            </td></tr>
                                          </table>"
                                          , urlimg, imgalt, titulo, usuario.NombreUsuario,pws,clave, linkportal);
            List<string> tos = new List<string>();
            tos.Add(usuario.Email);
            AlternateView texto = null;
            try
            {
                correoCtrl.sendMessage(tos, "Natware - Confirmación cuenta", cuerpo, texto, new List<string>(), new List<string>());
                envio = true;
            }
            catch (Exception ex)
            {
                LoggerHlp.Default.Error(this, ex);
                envio = false;
            }
            return envio;
        }

        public DataTable CargarArchivoPedido(IDataContext dctx, string urlArchivo, Pais pais, Estado estado, CicloEscolar cicloEscolar, Contrato contrato, List<ModuloFuncional> modulosFuncionales)
        {
            DataSet dsArchivoCarga = this.ValidarArchivoEscuela(urlArchivo);
            dsArchivoCarga.Tables[0].Columns.Add(new DataColumn("Cargado", typeof(bool)));
            dsArchivoCarga.Tables[0].Columns.Add(new DataColumn("RowIndex", typeof(int)));

            dsEscuelas = dsArchivoCarga;

             //Agregar columnas para beneficio del reporte
            dsEscuelas.Tables[0].Columns.Add(new DataColumn("NombreZona", typeof(string)));
            dsEscuelas.Tables[0].Columns.Add(new DataColumn("NombreTurno", typeof(string)));
            dsEscuelas.Tables[0].Columns.Add(new DataColumn("NombreControl", typeof(string)));
            dsEscuelas.Tables[0].Columns.Add(new DataColumn("NombreTipoServicio", typeof(string)));
            dsEscuelas.Tables[0].Columns.Add(new DataColumn("NombreMunicipio", typeof(string)));
            dsEscuelas.Tables[0].Columns.Add(new DataColumn("NombreLocalidad", typeof(string)));
            dsEscuelas.Tables[0].Columns.Add(new DataColumn("NombreAmbito", typeof(string)));


            #region Crear estructura de respuesta
            DataTable dtResultado = new DataTable();
            dtResultado.Columns.Add(new DataColumn("RowIndex", typeof(int)));
            dtResultado.Columns.Add(new DataColumn("ClaveEscuela", typeof(string)));
            dtResultado.Columns.Add(new DataColumn("Turno", typeof(string)));
            dtResultado.Columns.Add(new DataColumn("ExisteEscuela", typeof(bool)));
            dtResultado.Columns.Add(new DataColumn("ActivoEscuela", typeof(bool)));
            dtResultado.Columns.Add(new DataColumn("DirectorCURP", typeof(string)));
            dtResultado.Columns.Add(new DataColumn("ExisteDirector", typeof(bool)));
            dtResultado.Columns.Add(new DataColumn("ActivoDirector", typeof(bool)));
            dtResultado.Columns.Add(new DataColumn("ExisteMunicipio", typeof(bool)));
            dtResultado.Columns.Add(new DataColumn("ExisteZona", typeof(bool)));
            dtResultado.Columns.Add(new DataColumn("ExisteLocalidad", typeof(bool)));
            dtResultado.Columns.Add(new DataColumn("ExisteTipoServicio", typeof(bool)));
            dtResultado.Columns.Add(new DataColumn("ValidoTurno", typeof(bool)));
            dtResultado.Columns.Add(new DataColumn("ValidoAmbito", typeof(bool)));
            dtResultado.Columns.Add(new DataColumn("ValidoControl", typeof(bool)));
            dtResultado.Columns.Add(new DataColumn("Inconsistencia", typeof(string)));

             

            #endregion

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

            try
            {
                #region contrato
                ContratoCtrl contratoCtrl = new ContratoCtrl();
                Contrato cContrato = contratoCtrl.LastDataRowToContrato(contratoCtrl.Retrieve(dctx, contrato));
                #endregion

                foreach (DataRow row in dsArchivoCarga.Tables[0].Rows)
                {
                    object tran = new object();

                    #region Inicializacion de Resultado de inconsistencias
                    DataRow rwResultado = dtResultado.NewRow();
                    rwResultado.SetField<int>("RowIndex", dsArchivoCarga.Tables[0].Rows.IndexOf(row));
                    row.SetField<int>("RowIndex", dsArchivoCarga.Tables[0].Rows.IndexOf(row));
                    rwResultado.SetField<string>("ClaveEscuela", "");
                    rwResultado.SetField<string>("Turno", "");
                    rwResultado.SetField<bool>("ExisteEscuela", false);
                    rwResultado.SetField<bool>("ActivoEscuela", false);
                    rwResultado.SetField<string>("DirectorCURP", "");
                    rwResultado.SetField<bool>("ExisteDirector", false);
                    rwResultado.SetField<bool>("ActivoDirector", false);
                    rwResultado.SetField<bool>("ExisteMunicipio", true);
                    rwResultado.SetField<bool>("ExisteZona", true);
                    rwResultado.SetField<bool>("ExisteLocalidad", true);
                    rwResultado.SetField<bool>("ExisteTipoServicio", true);
                    rwResultado.SetField<bool>("ValidoTurno", true);
                    rwResultado.SetField<bool>("ValidoAmbito", true);
                    rwResultado.SetField<bool>("ValidoControl", true);
                    rwResultado.SetField<string>("Inconsistencia", "");

              //Agregar columnas para beneficio del reporte
                    row.SetField<string>("NombreZona", "");
                    row.SetField<string>("NombreTurno", "");
                    row.SetField<string>("NombreControl", "");
                    row.SetField<string>("NombreTipoServicio", "");
                    row.SetField<string>("NombreMunicipio", "");
                    row.SetField<string>("NombreLocalidad", "");
                    row.SetField<string>("NombreAmbito", "");
                


                    #endregion

                    try
                    {
                        dctx.BeginTransaction(tran);

                        try
                        {
                            bool registrar = false; // suponemos que la informacion es erronea

                            #region Obtenemos informacion como texto
                            string clave = row.IsNull("Clave") ? "" : row["Clave"].ToString();
                            string turno = row.IsNull("Turno") ? "" : row["Turno"].ToString();
                            string claveMunicipio = row.IsNull("ClaveMunicipio") ? "" : row["ClaveMunicipio"].ToString();
                            string claveLocalidad = row.IsNull("ClaveLocalidad") ? "" : row["ClaveLocalidad"].ToString();
                            string claveTipoServicio = row.IsNull("ClaveTipoServicio") ? "" : row["ClaveTipoServicio"].ToString();
                            string claveZona = row.IsNull("ClaveZona") ? "" : row["ClaveZona"].ToString();
                            string directorCURP = row.IsNull("DirectorCURP") ? "" : row["DirectorCURP"].ToString();
                            string ambito = row.IsNull("Ambito") ? "" : row["Ambito"].ToString();
                            string control = row.IsNull("Control") ? "" : row["Control"].ToString();
                            #endregion

                            DataSet ds = null; // dataset para almancenar resultado de consultas

                            try
                            {
                                #region Existe Director
                                Director director = new Director();
                                director.Curp = directorCURP;

                                DirectorCtrl directorCtrl = new DirectorCtrl();
                                ds = directorCtrl.Retrieve(dctx, director);
                                rwResultado.SetField<bool>("ExisteDirector", ds.Tables[0].Rows.Count != 0);

                                if (rwResultado.Field<bool>("ExisteDirector"))
                                {
                                    director = directorCtrl.LastDataRowToDirector(ds);
                                    rwResultado.SetField<bool>("ActivoDirector", director.Estatus.Value);
                                }
                                #endregion

                                #region Existe Ciudad
                                Ciudad ciudad = new Ciudad();
                                ciudad.Codigo = claveMunicipio;

                                CiudadCtrl ciudadCtrl = new CiudadCtrl();
                                ds = ciudadCtrl.Retrieve(dctx, ciudad);
                                rwResultado.SetField<bool>("ExisteMunicipio", ds.Tables[0].Rows.Count != 0);
                                #endregion

                                #region Existe Zona
                                Zona zona = new Zona();
                                zona.Clave = claveZona;

                                ZonaCtrl zonaCtrl = new ZonaCtrl();
                                ds = zonaCtrl.Retrieve(dctx, zona);
                                rwResultado.SetField<bool>("ExisteZona", ds.Tables[0].Rows.Count != 0);
                                #endregion

                                #region Existe Localidad
                                Localidad localidad = new Localidad();
                                localidad.Codigo = claveLocalidad;

                                LocalidadCtrl localidadCtrl = new LocalidadCtrl();
                                ds = localidadCtrl.Retrieve(dctx, localidad);
                                rwResultado.SetField<bool>("ExisteLocalidad", ds.Tables[0].Rows.Count != 0);
                                #endregion

                                #region Existe TipoServicio
                                TipoServicio tipoServicio = new TipoServicio();
                                tipoServicio.Clave = claveTipoServicio;

                                TipoServicioCtrl tipoServicioCtrl = new TipoServicioCtrl();
                                ds = tipoServicioCtrl.Retrieve(dctx, tipoServicio);
                                rwResultado.SetField<bool>("ExisteTipoServicio", ds.Tables[0].Rows.Count != 0);
                                #endregion

                                #region Existe Escuela
                                Escuela escuela = new Escuela();
                                escuela.Clave = clave;

                                try
                                {
                                    escuela.Turno = (ETurno)Convert.ToByte(turno); // Validar turno 
                                }
                                catch (Exception)
                                {
                                    rwResultado.SetField<bool>("ValidoTurno", false);
                                }

                                try
                                {
                                    escuela.Ambito = (EAmbito)Convert.ToByte(ambito); // Validar ambito
                                }
                                catch (Exception)
                                {
                                    rwResultado.SetField<bool>("ValidoAmbito", false);          
                                }

                                try
                                {
                                    escuela.Control = (EControl)Convert.ToByte(control); // Validar control
                                }
                                catch (Exception)
                                {
                                    rwResultado.SetField<bool>("ValidoControl", false);
                                }

                                if (rwResultado.Field<bool>("ValidoTurno"))
                                {
                                    EscuelaCtrl escuelaCtrl = new EscuelaCtrl();
                                    ds = escuelaCtrl.Retrieve(dctx, escuela);
                                    rwResultado.SetField<bool>("ExisteEscuela", ds.Tables[0].Rows.Count != 0);

                                    if (rwResultado.Field<bool>("ExisteEscuela"))
                                    {
                                        escuela = escuelaCtrl.LastDataRowToEscuela(ds);
                                        rwResultado.SetField<bool>("ActivoEscuela", escuela.Estatus.Value);
                                    }
                                }
                                #endregion
                            }
                            catch (Exception ex)
                            {
                                LoggerHlp.Default.Error(this, ex);
                                rwResultado.SetField<string>("Inconsistencia", rwResultado.Field<string>("Inconsistencia") + "\n" + ex.Message);
                            }

                            if (rwResultado.Field<bool>("ExisteMunicipio") && rwResultado.Field<bool>("ExisteZona") && rwResultado.Field<bool>("ExisteLocalidad") && rwResultado.Field<bool>("ExisteTipoServicio") && rwResultado.Field<bool>("ValidoTurno") && rwResultado.Field<bool>("ValidoAmbito") && rwResultado.Field<bool>("ValidoControl") && rwResultado.Field<string>("Inconsistencia") == "")
                                registrar = true;

                            row.SetField<bool>("Cargado", registrar);

                            if (!registrar)
                            {
                                dctx.RollbackTransaction(tran);
                                dtResultado.Rows.Add(rwResultado);
                                continue;
                            }

                            // generamos datos de Escuela, Director
                            Escuela nEscuela = this.DataRowToEscuela(row, new Pais(), new Estado());

                            #region Obtenemos Ubicacion de Escuela
                            CiudadCtrl cCiudadCtrl = new CiudadCtrl();
                            nEscuela.Ubicacion.Ciudad = cCiudadCtrl.LastDataRowToCiudad(cCiudadCtrl.Retrieve(dctx, nEscuela.Ubicacion.Ciudad));        
                            LocalidadCtrl cLocalidadCtrl = new LocalidadCtrl();
                            nEscuela.Ubicacion.Localidad = cLocalidadCtrl.LastDataRowToLocalidad(cLocalidadCtrl.Retrieve(dctx, nEscuela.Ubicacion.Localidad));                
                            nEscuela.Ubicacion.Colonia = null;

                            EstadoCtrl cestadoCtrl = new EstadoCtrl();
                            nEscuela.Ubicacion.Estado = cestadoCtrl.LastDataRowToEstado(cestadoCtrl.Retrieve(dctx, nEscuela.Ubicacion.Ciudad.Estado));
                            PaisCtrl cpaisCtrl = new PaisCtrl();
                            nEscuela.Ubicacion.Pais = cpaisCtrl.LastDataRowToPais(cpaisCtrl.Retrieve(dctx, nEscuela.Ubicacion.Estado.Pais));
                            UbicacionCtrl ubicacionCtrl = new UbicacionCtrl();

                            //Consultar la ubicación de la escuela
                            DataSet uds = ubicacionCtrl.Retrieve(dctx, nEscuela.Ubicacion);
                            
                            //verificar ubicación de la escuela
                            if (uds.Tables[0].Rows.Count != 1)
                            {
                                Ubicacion ubc = (Ubicacion)nEscuela.Ubicacion.Clone();
                                ubc.FechaRegistro = DateTime.Now;
                                      ubicacionCtrl.Insert(dctx,ubc);
                                uds = ubicacionCtrl.Retrieve(dctx, ubc);
                            }
                   
                            nEscuela.Ubicacion = ubicacionCtrl.LastDataRowToUbicacion(uds);
                        
                            #endregion

                            Usuario usuario = null;
                            LicenciaEscuelaCtrl licenciaEscuelaCtrl = new LicenciaEscuelaCtrl();

                            #region Registro de Director
                            Director nDirector = nEscuela.DirectorID; // suponemos que es nuevo Director
                            DirectorCtrl cDirectorCtrl = new DirectorCtrl();

                            if (rwResultado.Field<bool>("ExisteDirector")) // si el Director existe, actualizamos Director
                            {
                                Director cDirector = new Director();
                                cDirector.Curp = nDirector.Curp;

                                cDirector = cDirectorCtrl.LastDataRowToDirector(cDirectorCtrl.Retrieve(dctx, cDirector)); // obtenemos Director

                                nDirector.DirectorID = cDirector.DirectorID;
                                nDirector.Estatus = true;
                                nDirector.FechaRegistro = cDirector.FechaRegistro;
                                nDirector.Clave = cDirector.Clave;
                                nDirector.EstatusIdentificacion = cDirector.EstatusIdentificacion;
                                
                                cDirectorCtrl.Update(dctx, nDirector, cDirector); // actualizamos Director

                                usuario = licenciaEscuelaCtrl.RetrieveUsuario(dctx, nDirector);
                            }
                            else // si el Director no existe, registramos Director
                            {
                                nDirector.FechaRegistro = DateTime.Now;
                                nDirector.Estatus = true;
                                nDirector.Clave = new PasswordProvider(5).GetNewPassword(); // generamos Clave para confirmar información
                                nDirector.EstatusIdentificacion = false;

                                cDirectorCtrl.Insert(dctx, nDirector); // registramos Director
                                nEscuela.DirectorID = cDirectorCtrl.LastDataRowToDirector(cDirectorCtrl.Retrieve(dctx, nDirector)); // obtenemos Director
                                nDirector = nEscuela.DirectorID;

                                usuario = licenciaEscuelaCtrl.RetrieveUsuario(dctx, nEscuela.DirectorID);
                            }
                            #endregion

                            #region Registro de Escuela
                            Zona cZona = new Zona();
                            cZona.Clave = claveZona;

                            ZonaCtrl cZonaCtrl = new ZonaCtrl();
                            cZona = cZonaCtrl.LastDataRowToZona(cZonaCtrl.Retrieve(dctx, cZona));
                            nEscuela.ZonaID = cZona;

                            TipoServicio cTipoServicio = new TipoServicio();
                            cTipoServicio.Clave = claveTipoServicio;

                            TipoServicioCtrl cTipoServicioCtrl = new TipoServicioCtrl();
                            cTipoServicio = cTipoServicioCtrl.LastDataRowToTipoServicio(cTipoServicioCtrl.Retrieve(dctx, cTipoServicio));
                            nEscuela.TipoServicio = cTipoServicio;

                            CicloEscolarCtrl cCicloEscolarCtrl = new CicloEscolarCtrl();
                            CicloEscolar cCicloEscolar = cCicloEscolarCtrl.LastDataRowToCicloEscolar(cCicloEscolarCtrl.Retrieve(dctx, cicloEscolar));

                            EscuelaCtrl cEscuelaCtrl = new EscuelaCtrl();
                            if (rwResultado.Field<bool>("ExisteEscuela")) // si la Escuela existe, actualizamos Escuela
                            {
                                Escuela cEscuela = new Escuela();
                                cEscuela.Clave = nEscuela.Clave;
                                cEscuela.Turno = nEscuela.Turno;

                                cEscuela = cEscuelaCtrl.LastDataRowToEscuela(cEscuelaCtrl.Retrieve(dctx, cEscuela)); // obtenemos Escuela
                                nEscuela.EscuelaID = cEscuela.EscuelaID;
                                nEscuela.FechaRegistro = cEscuela.FechaRegistro;
                                nEscuela.Estatus = true;

                                cEscuelaCtrl.Update(dctx, nEscuela, cEscuela); // actualizamos Escuela
                            }
                            else // si la Escuela no existe, registramos Escuela
                            {
                                nEscuela.Estatus = true;
                                nEscuela.FechaRegistro = DateTime.Now;

                                cEscuelaCtrl.Insert(dctx, nEscuela); // registramos Escuela
                                nEscuela = cEscuelaCtrl.LastDataRowToEscuela(cEscuelaCtrl.Retrieve(dctx, nEscuela)); // obtenemos Escuela
                                nEscuela.DirectorID = nDirector;
                            }

                            nEscuela.Ubicacion = ubicacionCtrl.LastDataRowToUbicacion(ubicacionCtrl.Retrieve(dctx, nEscuela.Ubicacion));

                            //Consultar detalles de la ubicación
                            if (nEscuela.Ubicacion.Ciudad != null)
                                nEscuela.Ubicacion.Ciudad = cCiudadCtrl.LastDataRowToCiudad(cCiudadCtrl.Retrieve(dctx, nEscuela.Ubicacion.Ciudad));

                            if (nEscuela.Ubicacion.Localidad != null)
                                nEscuela.Ubicacion.Localidad = cLocalidadCtrl.LastDataRowToLocalidad(cLocalidadCtrl.Retrieve(dctx, nEscuela.Ubicacion.Localidad));

                            //Agregar resultados para beneficio del reporte
                            row.SetField<string>("NombreZona",nEscuela.ZonaID.Nombre);
                            row.SetField<string>("NombreTurno",nEscuela.Turno.ToString());
                            row.SetField<string>("NombreControl",nEscuela.Control.ToString());
                            row.SetField<string>("NombreTipoServicio",nEscuela.TipoServicio.Nombre);
                            row.SetField<string>("NombreMunicipio", nEscuela.Ubicacion.Ciudad.Nombre);
                            row.SetField<string>("NombreLocalidad", nEscuela.Ubicacion.Localidad.Nombre);
                            row.SetField<string>("NombreAmbito",nEscuela.Ambito.ToString());

                            
                            #endregion

                            LicenciaEscuela licenciaEscuela = new LicenciaEscuela();
                            licenciaEscuela.CicloEscolar = cCicloEscolar;
                            licenciaEscuela.Escuela = nEscuela;
                            licenciaEscuela.Activo = true;
                            licenciaEscuela.ModulosFuncionales = modulosFuncionales;
                            

                            ds = licenciaEscuelaCtrl.Retrieve(dctx, licenciaEscuela);
                            bool NoLicenciaEscuela = ds.Tables[0].Rows.Count == 0;

                            if (!NoLicenciaEscuela)
                            {
                                dctx.RollbackTransaction(tran);

                                row.SetField<bool>("Cargado", false);
                                rwResultado.SetField<string>("Inconsistencia", "La escuela ya esta registrada en el ciclo " + licenciaEscuela.CicloEscolar.InicioCiclo.Value.Year.ToString(CultureInfo.InvariantCulture) + "-" + licenciaEscuela.CicloEscolar.FinCiclo.Value.Year.ToString(CultureInfo.InvariantCulture));
                                dtResultado.Rows.Add(rwResultado);
                                continue;
                            }

                            licenciaEscuela.NumeroLicencias = 0;
                            licenciaEscuela.Contrato = cContrato;

                            bool envioCorreo = false;
                            string pwsTemporal = string.Empty;
                            UsuarioCtrl usuarioCtrl = new UsuarioCtrl();
                            if(usuario.UsuarioID != null)
                            {
                                usuario = usuarioCtrl.LastDataRowToUsuario(usuarioCtrl.Retrieve(dctx, usuario));

                                if (usuario.EsActivo == false)
                                {
                                    Usuario original = (Usuario)usuario.Clone();
                                    usuario.EsActivo = true;

                                    usuarioCtrl.Update(dctx, usuario, original);
                                }
                            }
                            else
                            {
                                usuario.NombreUsuario = usuarioCtrl.GenerarNombreUsuarioUnico(dctx, nEscuela.DirectorID.Nombre, nEscuela.DirectorID.PrimerApellido, nEscuela.DirectorID.FechaNacimiento.Value);
                                usuario.Email = nEscuela.DirectorID.Correo;
                                pwsTemporal = new PasswordProvider(8).GetNewPassword();
                                usuario.Password = EncryptHash.SHA1encrypt(pwsTemporal);
                                usuario.EsActivo = true;
                                usuario.FechaCreacion = DateTime.Now;
                                usuario.PasswordTemp = true;

                                //Consultar Termino Activo
                                TerminoCtrl terminoCtrl = new TerminoCtrl();
                                DataSet dsTermino = (terminoCtrl.Retrieve(dctx, new Termino { Estatus = true }));

                                usuario.Termino = dsTermino.Tables[0].Rows.Count >= 1
                                                      ? terminoCtrl.LastDataRowToTermino(dsTermino)
                                                      : new Termino();
                                usuario.AceptoTerminos = false;

                                usuarioCtrl.Insert(dctx, usuario);
                                usuario = usuarioCtrl.LastDataRowToUsuario(usuarioCtrl.Retrieve(dctx, usuario));

                                envioCorreo = true;
                               
                               


                            }

                            UsuarioPrivilegiosCtrl usuarioPrivilegiosCtrl = new UsuarioPrivilegiosCtrl();

                            #region registrar usuario privilegios

                            //asignamos el perfil alumno a la lista de privilegios
                            Perfil perfil = new Perfil { PerfilID = (int)EPerfil.DIRECTOR };

                            List<IPrivilegio> privilegios = new List<IPrivilegio>();
                            privilegios.Add(perfil);

                            usuarioPrivilegiosCtrl.InsertUsuarioEscolarPrivilegios(dctx, usuario, licenciaEscuela.Escuela, licenciaEscuela.CicloEscolar, privilegios);

                            #endregion

                            licenciaEscuelaCtrl.Insert(dctx, licenciaEscuela);
                            ds = licenciaEscuelaCtrl.Retrieve(dctx, licenciaEscuela);
                            licenciaEscuela = licenciaEscuelaCtrl.LastDataRowToLicenciaEscuela(ds);

                            licenciaEscuelaCtrl.InsertLicenciaDirector(dctx, licenciaEscuela, nDirector, usuario);
                            dctx.CommitTransaction(tran);
                            /*Envió Correo electrónico*/
                            if(envioCorreo)
                                EnviarCorreo(usuario, pwsTemporal, nEscuela.DirectorID.Clave);
                        }
                        catch (Exception ex)
                        {
                            dctx.RollbackTransaction(tran);
                            LoggerHlp.Default.Error(this, ex);
                            row.SetField<bool>("Cargado", false);
                            rwResultado.SetField<string>("Inconsistencia", rwResultado.Field<string>("Inconsistencia") + "\n" + ex.Message);
                            dtResultado.Rows.Add(rwResultado);
                        }
                    }
                    catch (Exception ex)
                    {
                        dctx.RollbackTransaction(tran);
                        LoggerHlp.Default.Error(this, ex);
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
            }

            return dtResultado;
        }

        private Escuela DataRowToEscuela(DataRow dataRow, Pais pais, Estado estado)
        {
            Escuela escuela = new Escuela();
            escuela.Ubicacion = new Ubicacion();
            escuela.Ubicacion.Pais = pais;
            escuela.Ubicacion.Estado = estado;
            escuela.Ubicacion.Ciudad = new Ciudad();
            escuela.Ubicacion.Localidad = new Localidad();
            escuela.TipoServicio = new TipoServicio();
            escuela.ZonaID = new Zona();
            escuela.DirectorID = new Director();

            // Escuela
            if (dataRow.IsNull("Clave"))
                escuela.Clave = null;
            else
                escuela.Clave = (string)Convert.ChangeType(dataRow["Clave"], typeof(string));
            if (dataRow.IsNull("NombreEscuela"))
                escuela.NombreEscuela = null;
            else
                escuela.NombreEscuela = (string)Convert.ChangeType(dataRow["NombreEscuela"], typeof(string));
            if (dataRow.IsNull("Turno"))
                escuela.Turno = null;
            else
                escuela.Turno = (ETurno)Convert.ChangeType(dataRow["Turno"], typeof(byte));
            if (dataRow.IsNull("Ambito"))
                escuela.Ambito = null;
            else
                escuela.Ambito = (EAmbito)Convert.ChangeType(dataRow["Ambito"], typeof(byte));
            if (dataRow.IsNull("Control"))
                escuela.Control = null;
            else
                escuela.Control = (EControl)Convert.ChangeType(dataRow["Control"], typeof(byte));
            if (dataRow.IsNull("ClaveMunicipio"))
                escuela.Ubicacion.Ciudad.Codigo = null;
            else
                escuela.Ubicacion.Ciudad.Codigo = (string)Convert.ChangeType(dataRow["ClaveMunicipio"], typeof(string));
            if (dataRow.IsNull("ClaveLocalidad"))
                escuela.Ubicacion.Localidad.Codigo = null;
            else
                escuela.Ubicacion.Localidad.Codigo = (string)Convert.ChangeType(dataRow["ClaveLocalidad"], typeof(string));
            if (dataRow.IsNull("ClaveTipoServicio"))
                escuela.TipoServicio.Clave = null;
            else
                escuela.TipoServicio.Clave = (string)Convert.ChangeType(dataRow["ClaveTipoServicio"], typeof(string));
            if (dataRow.IsNull("ClaveZona"))
                escuela.ZonaID.Clave = null;
            else
                escuela.ZonaID.Clave = (string)Convert.ChangeType(dataRow["ClaveZona"], typeof(string));

            //Director
            if (dataRow.IsNull("DirectorCurp"))
                escuela.DirectorID.Curp = null;
            else
                escuela.DirectorID.Curp = (string)Convert.ChangeType(dataRow["DirectorCurp"], typeof(string));
            if (dataRow.IsNull("DirectorNombre"))
                escuela.DirectorID.Nombre = null;
            else
                escuela.DirectorID.Nombre = (string)Convert.ChangeType(dataRow["DirectorNombre"], typeof(string));
            if (dataRow.IsNull("DirectorPrimerApellido"))
                escuela.DirectorID.PrimerApellido = null;
            else
                escuela.DirectorID.PrimerApellido = (string)Convert.ChangeType(dataRow["DirectorPrimerApellido"], typeof(string));
            if (dataRow.IsNull("DirectorSegundoApellido"))
                escuela.DirectorID.SegundoApellido = null;
            else
                escuela.DirectorID.SegundoApellido = (string)Convert.ChangeType(dataRow["DirectorSegundoApellido"], typeof(string));
            if (dataRow.IsNull("DirectorFechaNacimiento"))
                escuela.DirectorID.FechaNacimiento = null;
            else
                escuela.DirectorID.FechaNacimiento = DateTime.ParseExact(Convert.ChangeType(dataRow["DirectorFechaNacimiento"], typeof(string)).ToString(), "dd-MM-yyyy", CultureInfo.InvariantCulture);
            if (dataRow.IsNull("DirectorSexo"))
                escuela.DirectorID.Sexo = null;
            else
                escuela.DirectorID.Sexo = Convert.ChangeType(dataRow["DirectorSexo"], typeof(string)).ToString().Trim() == "H";
            if (dataRow.IsNull("DirectorNivelEscolar"))
                escuela.DirectorID.NivelEscolar = null;
            else
                escuela.DirectorID.NivelEscolar = (string)Convert.ChangeType(dataRow["DirectorNivelEscolar"], typeof(string));
            if (dataRow.IsNull("DirectorCorreo"))
                escuela.DirectorID.Correo = null;
            else
                escuela.DirectorID.Correo = (string)Convert.ChangeType(dataRow["DirectorCorreo"], typeof(string));
            if (dataRow.IsNull("DirectorTelefono"))
                escuela.DirectorID.Telefono = null;
            else
                escuela.DirectorID.Telefono = (string)Convert.ChangeType(dataRow["DirectorTelefono"], typeof(string));

            return escuela;
        }
    }
}
