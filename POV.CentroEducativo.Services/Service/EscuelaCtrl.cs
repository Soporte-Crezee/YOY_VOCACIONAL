// Clase motor de red social
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.DA;
using POV.CentroEducativo.DAO;
using POV.CentroEducativo.Service;
using POV.Localizacion.Service;
using POV.Localizacion.BO;

namespace POV.CentroEducativo.Service
{
    /// <summary>
    /// Controlador del objeto Escuela
    /// </summary>
    public class EscuelaCtrl
    {
        /// <summary>
        /// Consulta registros de EscuelaRetHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="escuela">EscuelaRetHlp que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de EscuelaRetHlp generada por la consulta</returns>
        public DataSet Retrieve(IDataContext dctx, Escuela escuela)
        {
            EscuelaRetHlp da = new EscuelaRetHlp();
            DataSet ds = da.Action(dctx, escuela);
            return ds;
        }

        public Escuela RetrieveComplete(IDataContext dctx, Escuela escuela)
        {
            DataSet dsEscuela = new EscuelaRetHlp().Action(dctx, escuela);
            Escuela resultado = this.LastDataRowToEscuela(dsEscuela);

            DataSet ds = null;

            GrupoRetHlp gpoRet =  new GrupoRetHlp();
            ds = gpoRet.Action(dctx, new Grupo(), resultado);
            List<Grupo> grupos = new List<Grupo>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++) grupos.Add(this.DataRowToGrupo(ds.Tables[0].Rows[i]));

            AsignacionDocenteEscuelaRetHlp asigRet = new AsignacionDocenteEscuelaRetHlp();
            ds = asigRet.Action(dctx, new AsignacionDocenteEscuela { Activo = true }, resultado);
            List<AsignacionDocenteEscuela> asignaciones = new List<AsignacionDocenteEscuela>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++) asignaciones.Add(this.DataRowToAsignacionDocenteEscuela(ds.Tables[0].Rows[i]));

            AsignacionEspecialistaEscuelaRetHlp asigEspRet = new AsignacionEspecialistaEscuelaRetHlp();
            ds = asigEspRet.Action(dctx, new AsignacionEspecialistaEscuela { Activo = true }, resultado);
            List<AsignacionEspecialistaEscuela> asignacionesEsp = new List<AsignacionEspecialistaEscuela>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++) asignacionesEsp.Add(this.DataRowToAsignacionEspecialistaEscuela(ds.Tables[0].Rows[i]));


            UbicacionCtrl ubicacionCtrl = new UbicacionCtrl();
            TipoServicioCtrl tipoServicioCtrl = new TipoServicioCtrl();
            DirectorCtrl directorCtrl = new DirectorCtrl();
            ZonaCtrl zonaCtrl = new ZonaCtrl();

            resultado = this.DataRowToEscuela(dsEscuela.Tables[0].Rows[ dsEscuela.Tables[0].Rows.Count - 1], grupos, asignaciones, asignacionesEsp);
            resultado.Ubicacion = ubicacionCtrl.LastDataRowToUbicacion(ubicacionCtrl.Retrieve(dctx, resultado.Ubicacion));
            resultado.TipoServicio = tipoServicioCtrl.LastDataRowToTipoServicio(tipoServicioCtrl.Retrieve(dctx, resultado.TipoServicio));
            resultado.ZonaID = zonaCtrl.LastDataRowToZona(zonaCtrl.Retrieve(dctx, resultado.ZonaID));
            resultado.DirectorID = directorCtrl.LastDataRowToDirector(directorCtrl.Retrieve(dctx, resultado.DirectorID));

            return resultado;
        }
        /// <summary>
        /// Crea un registro de EscuelaInsHlp en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="escuela">EscuelaInsHlp que desea crear</param>
        public void Insert(IDataContext dctx, Escuela escuela)
        {
            if (validarEscuelaRepetida(escuela, dctx))
            {
                throw new Exception("No pueden existir dos escuelas con la misma clave y turno");
            }
            else
            {
                EscuelaInsHlp da = new EscuelaInsHlp();
                da.Action(dctx, escuela);
            }
        }

        public void InsertAsignacionEspecialista(IDataContext dctx, EspecialistaPruebas especialista, Escuela escuela)
        {
            AsignacionEspecialistaEscuela asignacion = new AsignacionEspecialistaEscuela();
            asignacion.Especialista = especialista;

            AsignacionEspecialistaEscuelaRetHlp asigRet = new AsignacionEspecialistaEscuelaRetHlp();
            DataSet ds = asigRet.Action(dctx, asignacion, escuela);
            int total = ds.Tables[0].Rows.Count;

            if (total > 0)
                asignacion = this.DataRowToAsignacionEspecialistaEscuela(ds.Tables[0].Rows[total - 1]);

            if (asignacion.AsignacionEspecialistaEscuelaID != null)
            {
                if (asignacion.Activo.Value)
                    return;

                AsignacionEspecialistaEscuela anterior = (AsignacionEspecialistaEscuela)asignacion.Clone();
                asignacion.Activo = true;
                asignacion.FechaBaja = null;
                new AsignacionEspecialistaEscuelaUpdHlp().Action(dctx, asignacion, anterior, escuela);

                return;
            }
            else
            {
                asignacion.Activo = true;
                asignacion.FechaRegistro = DateTime.Now;
                new AsignacionEspecialistaEscuelaInsHlp().Action(dctx, asignacion, escuela);
            }
        }
        
        public void InsertAsignacionDocente(IDataContext dctx, Docente docente , Escuela escuela)
        {
            AsignacionDocenteEscuela asignacion = new AsignacionDocenteEscuela();
            asignacion.Docente = docente;

            AsignacionDocenteEscuelaRetHlp asigRet = new AsignacionDocenteEscuelaRetHlp();
            DataSet ds = asigRet.Action(dctx, asignacion, escuela);
            int total = ds.Tables[0].Rows.Count;

            if (total > 0)
                asignacion = this.DataRowToAsignacionDocenteEscuela(ds.Tables[0].Rows[total-1]);

            if (asignacion.AsignacionDocenteEscuelaID != null)
            {
                if(asignacion.Activo.Value)
                    return;

                AsignacionDocenteEscuela anterior = (AsignacionDocenteEscuela)asignacion.Clone();
                asignacion.Activo = true;
                asignacion.FechaBaja = null;
                new AsignacionDocenteEscuelaUpdHlp().Action(dctx, asignacion, anterior, escuela);

                return;
            }
            else
            {
                asignacion.Activo = true;
                asignacion.FechaRegistro = DateTime.Now;
                new AsignacionDocenteEscuelaInsHlp().Action(dctx, asignacion, escuela);
            }
        }

        public void DeleteAignacionesDocente(IDataContext dctx, AsignacionDocenteEscuela asignacion, Escuela escuela)
        {
            
            AsignacionDocenteEscuelaRetHlp asigRet = new AsignacionDocenteEscuelaRetHlp();
            DataSet ds = asigRet.Action(dctx, asignacion, escuela);
            int total = ds.Tables[0].Rows.Count;
            
            if (total > 0)
            {
                object firma = new object();
                dctx.OpenConnection(firma);
                try
                {
                    dctx.BeginTransaction(firma);
                    foreach (DataRow drasignacion in ds.Tables[0].Rows)
                    {
                        asignacion = this.DataRowToAsignacionDocenteEscuela(drasignacion);

                        if (asignacion.AsignacionDocenteEscuelaID != null)
                        {
                            AsignacionDocenteEscuela anterior = (AsignacionDocenteEscuela)asignacion.CloneAll();
                            if (asignacion.Activo.Value)
                            {
                                asignacion.Activo = false;
                                asignacion.FechaBaja = DateTime.Now;
                                new AsignacionDocenteEscuelaUpdHlp().Action(dctx, asignacion, anterior, escuela);
                            }
                        }
                    }
                    dctx.CommitTransaction(firma);
                }
                catch (Exception ex)
                {
                    dctx.RollbackTransaction(firma);
                    if(dctx.ConnectionState==ConnectionState.Open)
                        dctx.CloseConnection(firma);
                    throw;
                }
                finally
                {
                    if(dctx.ConnectionState == ConnectionState.Open)
                        dctx.CloseConnection(firma);
                }
            }                
        }

        public void DeleteAignacionesEspecialista(IDataContext dctx, AsignacionEspecialistaEscuela asignacion, Escuela escuela)
        {

            AsignacionEspecialistaEscuelaRetHlp asigRet = new AsignacionEspecialistaEscuelaRetHlp();
            DataSet ds = asigRet.Action(dctx, asignacion, escuela);
            int total = ds.Tables[0].Rows.Count;

            if (total > 0)
            {
                object firma = new object();
                dctx.OpenConnection(firma);
                try
                {
                    dctx.BeginTransaction(firma);
                    foreach (DataRow drasignacion in ds.Tables[0].Rows)
                    {
                        asignacion = this.DataRowToAsignacionEspecialistaEscuela(drasignacion);

                        if (asignacion.Especialista.EspecialistaPruebaID != null)
                        {
                            AsignacionEspecialistaEscuela anterior = (AsignacionEspecialistaEscuela)asignacion.CloneAll();
                            if (asignacion.Activo.Value)
                            {
                                asignacion.Activo = false;
                                asignacion.FechaBaja = DateTime.Now;
                                new AsignacionEspecialistaEscuelaUpdHlp().Action(dctx, asignacion, anterior, escuela);
                            }
                        }
                    }
                    dctx.CommitTransaction(firma);
                }
                catch (Exception ex)
                {
                    dctx.RollbackTransaction(firma);
                    if (dctx.ConnectionState == ConnectionState.Open)
                        dctx.CloseConnection(firma);
                    throw;
                }
                finally
                {
                    if (dctx.ConnectionState == ConnectionState.Open)
                        dctx.CloseConnection(firma);
                }
            }
        }

        public void InsertGrupo(IDataContext dctx, Grupo grupo, Escuela escuela)
        {
            new GrupoInsHlp().Action(dctx, grupo, escuela);
        }

        private bool validarEscuelaRepetida(Escuela escuela, IDataContext dctx)
        {
            Escuela escuelaValidar = new Escuela();
            EscuelaRetHlp daValidar = new EscuelaRetHlp();
            escuelaValidar.Clave = escuela.Clave;
            escuelaValidar.ToShortTurno = escuela.ToShortTurno;
            DataSet dsValidar = daValidar.Action(dctx, escuelaValidar);
            bool validar = false;
            if (dsValidar.Tables["Escuela"].Rows.Count <= 0)
            {
                validar = false;
            }
            else
            {
                validar = true;
            }
            return validar;
        }

        public List<Grupo> RetrieveGrupos(IDataContext dctx, Escuela escuela, Grupo grupo)
        {
            List<Grupo> grupos = new List<Grupo>();

            GrupoRetHlp da = new GrupoRetHlp();
            DataSet ds = da.Action(dctx,grupo, escuela);

            foreach (DataRow row in ds.Tables[0].Rows) grupos.Add(this.DataRowToGrupo(row));

            return grupos;
        }

        public List<Docente> RetrieveDocentes(IDataContext dctx, Escuela escuela)
        {
            List<Docente> docentes = new List<Docente>();

            AsignacionDocenteEscuela asignacion = new AsignacionDocenteEscuela();
            asignacion.Activo = true;

            AsignacionDocenteEscuelaRetHlp da = new AsignacionDocenteEscuelaRetHlp();
            DataSet ds = da.Action(dctx, asignacion, escuela);
            DocenteCtrl docCtrl = new DocenteCtrl();

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                asignacion = this.DataRowToAsignacionDocenteEscuela(row);
                Docente docente = new Docente();
                docente.DocenteID = asignacion.Docente.DocenteID;

                docentes.Add(docCtrl.LastDataRowToDocente(docCtrl.Retrieve(dctx, docente)));
            }

            return docentes;
        }

        public List<EspecialistaPruebas> RetrieveEspecialistas(IDataContext dctx, Escuela escuela)
        {
            List<EspecialistaPruebas> especialistas = new List<EspecialistaPruebas>();

            AsignacionEspecialistaEscuela asignacion = new AsignacionEspecialistaEscuela();
            asignacion.Activo = true;

            AsignacionEspecialistaEscuelaRetHlp da = new AsignacionEspecialistaEscuelaRetHlp();
            DataSet ds = da.Action(dctx, asignacion, escuela);
            EspecialistaCtrl espCtrl = new EspecialistaCtrl();

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                asignacion = this.DataRowToAsignacionEspecialistaEscuela(row);
                EspecialistaPruebas especialista = new EspecialistaPruebas();
                especialista.EspecialistaPruebaID = asignacion.Especialista.EspecialistaPruebaID;

                especialistas.Add(espCtrl.LastDataRowToEspecialista(espCtrl.Retrieve(dctx, especialista)));
            }

            return especialistas;
        }

        public AsignacionDocenteEscuela RetrieveAsignacionDocenteEscuela(IDataContext dctx, Docente docente, Escuela escuela)
        {
            AsignacionDocenteEscuelaRetHlp asigRet = new AsignacionDocenteEscuelaRetHlp();
            return LastDataRowToAsignacionDocenteEscuela(asigRet.Action(dctx, new AsignacionDocenteEscuela { Activo = true, Docente = docente }, escuela));
        }

       
        /// <summary>
        /// Actualiza de manera optimista un registro de EscuelaUpdHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="escuela">EscuelaUpdHlp que tiene los datos nuevos</param>
        /// <param name="previous">EscuelaUpdHlp que tiene los datos anteriores</param>
        public void Update(IDataContext dctx, Escuela escuela, Escuela previous)
        {
            bool nuevo = false;

            if (escuela.Clave != previous.Clave || escuela.Turno != previous.Turno)
                nuevo = validarEscuelaRepetida(escuela, dctx);

            if (nuevo)
                throw new Exception("No pueden existir dos escuelas con la misma clave y turno");

            EscuelaUpdHlp da = new EscuelaUpdHlp();
            da.Action(dctx, escuela, previous);
        }

        /// <summary>
        /// Consulta las escuelas que correspondan con los criterios de búsqueda
        /// Criterios de Búsqueda:Escuela,ZonaID,UbicacionID,PaisID,EstadoID,CiudadID,LocalidadID,TipoServicioID,NivelEducativoID,TipoNivelEducativoID
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="escuela">Escuela que provee el criterio de selección para la consulta</param>
        /// <returns>DataSet que contiene la información generada por la consulta</returns>
        public DataSet RetrieveFilter(IDataContext dctx,Escuela escuela)
        {
            EscuelaDARetHlp da = new EscuelaDARetHlp();
            DataSet ds = da.Action(dctx, escuela);
            return ds;
        }

        private bool ValidarLicenciaEscuela(Escuela escuela)
        {
            bool licenciaActiva = false;
            return licenciaActiva;
        }

        #region Centro Computo de la escuela
        public void InsertCentroComputo(IDataContext dctx, CentroComputo centroComputo, Escuela escuela)
        {
            new CentroComputoInsHlp().Action(dctx, escuela, centroComputo);
        }

        public void UpdateCentroComputo(IDataContext dctx, CentroComputo centroComputo, CentroComputo anterior, Escuela escuela)
        {
            new CentroComputoUpdHlp().Action(dctx, escuela, centroComputo, anterior);
        }

        public CentroComputo RetrieveCentroComputo(IDataContext dctx, Escuela escuela)
        {
            CentroComputo centro = new CentroComputo();

            CentroComputoRetHlp da = new CentroComputoRetHlp();
            DataSet ds = da.Action(dctx, escuela, new CentroComputo());

            if (ds.Tables[0].Rows.Count > 0)
            {
                centro = LastDataRowToCentroComputo(ds);
            }

            return centro;
        }

        /// <summary>
        /// Crea un objeto de CentroComputo a partir de los datos del Ãºltimo DataRow del DataSet.
        /// </summary>
        /// <param name="ds">El DataSet que contiene la informaciÃ³n de CentroComputo</param>
        /// <returns>Un objeto de CentroComputo creado a partir de los datos</returns>
        public CentroComputo LastDataRowToCentroComputo(DataSet ds)
        {
            if (!ds.Tables.Contains("CentroComputo"))
                throw new Exception("LastDataRowToCentroComputo: DataSet no tiene la tabla CentroComputo");
            int index = ds.Tables["CentroComputo"].Rows.Count;
            if (index < 1)
                throw new Exception("LastDataRowToCentroComputo: El DataSet no tiene filas");
            return this.DataRowToCentroComputo(ds.Tables["CentroComputo"].Rows[index - 1]);
        }
        /// <summary>
        /// Crea un objeto de CentroComputo a partir de los datos de un DataRow.
        /// </summary>
        /// <param name="row">El DataRow que contiene la informaciÃ³n de CentroComputo</param>
        /// <returns>Un objeto de CentroComputo creado a partir de los datos</returns>
        public CentroComputo DataRowToCentroComputo(DataRow row)
        {
            CentroComputo centroComputo = new CentroComputo();
            if (row.IsNull("CentroComputoID"))
                centroComputo.CentroComputoID = null;
            else
                centroComputo.CentroComputoID = (int)Convert.ChangeType(row["CentroComputoID"], typeof(int));
            if (row.IsNull("TieneCentroComputo"))
                centroComputo.TieneCentroComputo = null;
            else
                centroComputo.TieneCentroComputo = (bool)Convert.ChangeType(row["TieneCentroComputo"], typeof(bool));
            if (row.IsNull("TieneInternet"))
                centroComputo.TieneInternet = null;
            else
                centroComputo.TieneInternet = (bool)Convert.ChangeType(row["TieneInternet"], typeof(bool));
            if (row.IsNull("AnchoBanda"))
                centroComputo.AnchoBanda = null;
            else
                centroComputo.AnchoBanda = (decimal)Convert.ChangeType(row["AnchoBanda"], typeof(decimal));
            if (row.IsNull("NombreProveedor"))
                centroComputo.NombreProveedor = null;
            else
                centroComputo.NombreProveedor = (string)Convert.ChangeType(row["NombreProveedor"], typeof(string));
            if (row.IsNull("TipoContrato"))
                centroComputo.TipoContrato = null;
            else
                centroComputo.TipoContrato = (string)Convert.ChangeType(row["TipoContrato"], typeof(string));
            if (row.IsNull("Responsable"))
                centroComputo.Responsable = null;
            else
                centroComputo.Responsable = (string)Convert.ChangeType(row["Responsable"], typeof(string));
            if (row.IsNull("NumeroComputadoras"))
                centroComputo.NumeroComputadoras = null;
            else
                centroComputo.NumeroComputadoras = (int)Convert.ChangeType(row["NumeroComputadoras"], typeof(int));
            if (row.IsNull("TelefonoResponsable"))
                centroComputo.TelefonoResponsable = null;
            else
                centroComputo.TelefonoResponsable = (long)Convert.ChangeType(row["TelefonoResponsable"], typeof(long));
            if (row.IsNull("FechaRegistro"))
                centroComputo.FechaRegistro = null;
            else
                centroComputo.FechaRegistro = (DateTime)Convert.ChangeType(row["FechaRegistro"], typeof(DateTime));
            if (row.IsNull("Activo"))
                centroComputo.Activo = null;
            else
                centroComputo.Activo = (bool)Convert.ChangeType(row["Activo"], typeof(bool));
            return centroComputo;
        }
        #endregion

        /// <summary>
        /// Crea un objeto de Escuela a partir de los datos del último DataRow del DataSet.
        /// </summary>
        /// <param name="ds">El DataSet que contiene la información de Escuela</param>
        /// <returns>Un objeto de Escuela creado a partir de los datos</returns>
        public Escuela LastDataRowToEscuela(DataSet ds)
        {
            if (!ds.Tables.Contains("Escuela"))
                throw new Exception("LastDataRowToEscuela: DataSet no tiene la tabla Escuela");
            int index = ds.Tables["Escuela"].Rows.Count;
            if (index < 1)
                throw new Exception("LastDataRowToEscuela: El DataSet no tiene filas");
            return this.DataRowToEscuela(ds.Tables["Escuela"].Rows[index - 1]);
        }

        public AsignacionEspecialistaEscuela LastDataRowToAsignacionEspecialistaEscuela(DataSet ds)
        {
            if (!ds.Tables.Contains("AsignacionEspecialistaEscuela"))
                throw new Exception("LastDataRowToAsignacionEspecialistaEscuela: DataSet no tiene la tabla AsignacionEspecialistaEscuela");
            int index = ds.Tables["AsignacionEspecialistaEscuela"].Rows.Count;
            if (index < 1)
                throw new Exception("LastDataRowToEscuela: El DataSet no tiene filas");
            return this.DataRowToAsignacionEspecialistaEscuela(ds.Tables["AsignacionEspecialistaEscuela"].Rows[index - 1]);
        }
        /// <summary>
        /// Crea un objeto de Escuela a partir de los datos de un DataRow.
        /// </summary>
        /// <param name="row">El DataRow que contiene la información de Escuela</param>
        /// <returns>Un objeto de Escuela creado a partir de los datos</returns>
        public Escuela DataRowToEscuela(DataRow row)
        {
            Escuela escuela = new Escuela();
            if (escuela.Ubicacion == null)
                escuela.Ubicacion = new Ubicacion();
            if (escuela.TipoServicio == null)
                escuela.TipoServicio = new TipoServicio();
            if (escuela.ZonaID == null)
                escuela.ZonaID = new Zona();
            if (escuela.DirectorID == null)
                escuela.DirectorID = new Director();
            if (row.IsNull("EscuelaID"))
                escuela.EscuelaID = null;
            else
                escuela.EscuelaID = (int)Convert.ChangeType(row["EscuelaID"], typeof(int));
            if (row.IsNull("NombreEscuela"))
                escuela.NombreEscuela = null;
            else
                escuela.NombreEscuela = (string)Convert.ChangeType(row["NombreEscuela"], typeof(string));
            if (row.IsNull("Clave"))
                escuela.Clave = null;
            else
                escuela.Clave = (string)Convert.ChangeType(row["Clave"], typeof(string));
            if (row.IsNull("Estatus"))
                escuela.Estatus = null;
            else
                escuela.Estatus = (bool)Convert.ChangeType(row["Estatus"], typeof(bool));
            if (row.IsNull("FechaRegistro"))
                escuela.FechaRegistro = null;
            else
                escuela.FechaRegistro = (DateTime)Convert.ChangeType(row["FechaRegistro"], typeof(DateTime));
            if (row.IsNull("UbicacionID"))
                escuela.Ubicacion.UbicacionID = null;
            else
                escuela.Ubicacion.UbicacionID = (long)Convert.ChangeType(row["UbicacionID"], typeof(long));
            if (row.IsNull("Turno"))
                escuela.Turno = null;
            else
            {
                short num = (short)Convert.ChangeType(row["Turno"], typeof(short));
                escuela.Turno = (ETurno)num;
            }
            if (row.IsNull("TipoServicioID"))
                escuela.TipoServicio.TipoServicioID = null;
            else
                escuela.TipoServicio.TipoServicioID = (int)Convert.ChangeType(row["TipoServicioID"], typeof(int));
            if (row.IsNull("ZonaID"))
                escuela.ZonaID.ZonaID = null;
            else
                escuela.ZonaID.ZonaID = (long)Convert.ChangeType(row["ZonaID"], typeof(long));
            if (row.IsNull("DirectorID"))
                escuela.DirectorID.DirectorID = null;
            else
                escuela.DirectorID.DirectorID = (int)Convert.ChangeType(row["DirectorID"], typeof(int));
            if (row.IsNull("Ambito"))
                escuela.Ambito = null;
            else
            {
                short num = (short)Convert.ChangeType(row["Ambito"], typeof(short));
                escuela.Ambito = (EAmbito)num;
            }
            if (row.IsNull("Control"))
                escuela.Control = null;
            else
            {
                short num = (short)Convert.ChangeType(row["Control"], typeof(short));
                escuela.Control = (EControl)num;
            }

            return escuela;
        }

        private Escuela DataRowToEscuela(DataRow row, IEnumerable<Grupo> grupos, IEnumerable<AsignacionDocenteEscuela> asignacionDocentes, IEnumerable<AsignacionEspecialistaEscuela> asignacionEspecialistas)
        {
            Escuela escuela = new Escuela(grupos, asignacionDocentes);
            escuela.CentroComputo = new CentroComputo();
            if (escuela.Ubicacion == null)
                escuela.Ubicacion = new Ubicacion();
            if (escuela.TipoServicio == null)
                escuela.TipoServicio = new TipoServicio();
            if (escuela.ZonaID == null)
                escuela.ZonaID = new Zona();
            if (escuela.DirectorID == null)
                escuela.DirectorID = new Director();
            if (row.IsNull("EscuelaID"))
                escuela.EscuelaID = null;
            else
                escuela.EscuelaID = (int)Convert.ChangeType(row["EscuelaID"], typeof(int));
            if (row.IsNull("NombreEscuela"))
                escuela.NombreEscuela = null;
            else
                escuela.NombreEscuela = (string)Convert.ChangeType(row["NombreEscuela"], typeof(string));
            if (row.IsNull("Clave"))
                escuela.Clave = null;
            else
                escuela.Clave = (string)Convert.ChangeType(row["Clave"], typeof(string));
            if (row.IsNull("Estatus"))
                escuela.Estatus = null;
            else
                escuela.Estatus = (bool)Convert.ChangeType(row["Estatus"], typeof(bool));
            if (row.IsNull("FechaRegistro"))
                escuela.FechaRegistro = null;
            else
                escuela.FechaRegistro = (DateTime)Convert.ChangeType(row["FechaRegistro"], typeof(DateTime));
            if (row.IsNull("UbicacionID"))
                escuela.Ubicacion.UbicacionID = null;
            else
                escuela.Ubicacion.UbicacionID = (long)Convert.ChangeType(row["UbicacionID"], typeof(long));
            if (row.IsNull("Turno"))
                escuela.Turno = null;
            else
            {
                short num = (short)Convert.ChangeType(row["Turno"], typeof(short));
                escuela.Turno = (ETurno)num;
            }
            if (row.IsNull("TipoServicioID"))
                escuela.TipoServicio.TipoServicioID = null;
            else
                escuela.TipoServicio.TipoServicioID = (int)Convert.ChangeType(row["TipoServicioID"], typeof(int));
            if (row.IsNull("ZonaID"))
                escuela.ZonaID.ZonaID = null;
            else
                escuela.ZonaID.ZonaID = (long)Convert.ChangeType(row["ZonaID"], typeof(long));
            if (row.IsNull("DirectorID"))
                escuela.DirectorID.DirectorID = null;
            else
                escuela.DirectorID.DirectorID = (int)Convert.ChangeType(row["DirectorID"], typeof(int));
            if (row.IsNull("Ambito"))
                escuela.Ambito = null;
            else
            {
                short num = (short)Convert.ChangeType(row["Ambito"], typeof(short));
                escuela.Ambito = (EAmbito)num;
            }
            if (row.IsNull("Control"))
                escuela.Control = null;
            else
            {
                short num = (short)Convert.ChangeType(row["Control"], typeof(short));
                escuela.Control = (EControl)num;
            }

            return escuela;
        }

        private Escuela DataRowToEscuela(DataRow row, IEnumerable<Grupo> grupos, IEnumerable<AsignacionDocenteEscuela> asignacionDocentes )
        {
            Escuela escuela = new Escuela(grupos, asignacionDocentes);
            escuela.CentroComputo = new CentroComputo();
            if (escuela.Ubicacion == null)
                escuela.Ubicacion = new Ubicacion();
            if (escuela.TipoServicio == null)
                escuela.TipoServicio = new TipoServicio();
            if (escuela.ZonaID == null)
                escuela.ZonaID = new Zona();
            if (escuela.DirectorID == null)
                escuela.DirectorID = new Director();
            if (row.IsNull("EscuelaID"))
                escuela.EscuelaID = null;
            else
                escuela.EscuelaID = (int)Convert.ChangeType(row["EscuelaID"], typeof(int));
            if (row.IsNull("NombreEscuela"))
                escuela.NombreEscuela = null;
            else
                escuela.NombreEscuela = (string)Convert.ChangeType(row["NombreEscuela"], typeof(string));
            if (row.IsNull("Clave"))
                escuela.Clave = null;
            else
                escuela.Clave = (string)Convert.ChangeType(row["Clave"], typeof(string));
            if (row.IsNull("Estatus"))
                escuela.Estatus = null;
            else
                escuela.Estatus = (bool)Convert.ChangeType(row["Estatus"], typeof(bool));
            if (row.IsNull("FechaRegistro"))
                escuela.FechaRegistro = null;
            else
                escuela.FechaRegistro = (DateTime)Convert.ChangeType(row["FechaRegistro"], typeof(DateTime));
            if (row.IsNull("UbicacionID"))
                escuela.Ubicacion.UbicacionID = null;
            else
                escuela.Ubicacion.UbicacionID = (long)Convert.ChangeType(row["UbicacionID"], typeof(long));
            if (row.IsNull("Turno"))
                escuela.Turno = null;
            else
            {
                short num = (short)Convert.ChangeType(row["Turno"], typeof(short));
                escuela.Turno = (ETurno)num;
            }
            if (row.IsNull("TipoServicioID"))
                escuela.TipoServicio.TipoServicioID = null;
            else
                escuela.TipoServicio.TipoServicioID = (int)Convert.ChangeType(row["TipoServicioID"], typeof(int));
            if (row.IsNull("ZonaID"))
                escuela.ZonaID.ZonaID = null;
            else
                escuela.ZonaID.ZonaID = (long)Convert.ChangeType(row["ZonaID"], typeof(long));
            if (row.IsNull("DirectorID"))
                escuela.DirectorID.DirectorID = null;
            else
                escuela.DirectorID.DirectorID = (int)Convert.ChangeType(row["DirectorID"], typeof(int));
            if (row.IsNull("Ambito"))
                escuela.Ambito = null;
            else
            {
                short num = (short)Convert.ChangeType(row["Ambito"], typeof(short));
                escuela.Ambito = (EAmbito)num;
            }
            if (row.IsNull("Control"))
                escuela.Control = null;
            else
            {
                short num = (short)Convert.ChangeType(row["Control"], typeof(short));
                escuela.Control = (EControl)num;
            }

            return escuela;
        }

        private Grupo DataRowToGrupo(DataRow row)
        {
            Grupo grupo = new Grupo();
            if (row.IsNull("GRUPOID"))
                grupo.GrupoID = null;
            else
                grupo.GrupoID = (Guid)Convert.ChangeType(row["GRUPOID"], typeof(Guid));
            if (row.IsNull("NOMBRE"))
                grupo.Nombre = null;
            else
                grupo.Nombre = (string)Convert.ChangeType(row["NOMBRE"], typeof(string));
            if (row.IsNull("GRADO"))
                grupo.Grado = null;
            else
                grupo.Grado = (byte)Convert.ChangeType(row["GRADO"], typeof(byte));
            return grupo;
        }

        public AsignacionDocenteEscuela LastDataRowToAsignacionDocenteEscuela(DataSet ds)
        {
            if (!ds.Tables.Contains("AsignacionDocenteEscuela"))
                throw new Exception("LastDataRowToAsignacionDocenteEscuela: DataSet no tiene la tabla Escuela");
            int index = ds.Tables["AsignacionDocenteEscuela"].Rows.Count;
            if (index < 1)
                throw new Exception("LastDataRowToDocenteEscuela: El DataSet no tiene filas");
            return this.DataRowToAsignacionDocenteEscuela(ds.Tables["AsignacionDocenteEscuela"].Rows[index - 1]);
        }

        private AsignacionDocenteEscuela DataRowToAsignacionDocenteEscuela(DataRow row)
        {
            AsignacionDocenteEscuela asignacion = new AsignacionDocenteEscuela();
            asignacion.Docente = new Docente();

            if (row.IsNull("DOCENTEESCUELAID"))
                asignacion.AsignacionDocenteEscuelaID = null;
            else
                asignacion.AsignacionDocenteEscuelaID = (Int64)Convert.ChangeType(row["DOCENTEESCUELAID"], typeof(Int64));
            if (row.IsNull("DOCENTEID"))
                asignacion.Docente.DocenteID = null;
            else
                asignacion.Docente.DocenteID = (int)Convert.ChangeType(row["DOCENTEID"], typeof(int));
            if (row.IsNull("ESTATUS"))
                asignacion.Activo = null;
            else
                asignacion.Activo = (bool)Convert.ChangeType(row["ESTATUS"], typeof(bool));
            if (row.IsNull("FECHAREGISTRO"))
                asignacion.FechaRegistro = null;
            else
                asignacion.FechaRegistro = (DateTime)Convert.ChangeType(row["FECHAREGISTRO"], typeof(DateTime));
            if (row.IsNull("FECHABAJA"))
                asignacion.FechaBaja = null;
            else
                asignacion.FechaBaja = (DateTime)Convert.ChangeType(row["FECHABAJA"], typeof(DateTime));

            return asignacion;
        }
        private AsignacionEspecialistaEscuela DataRowToAsignacionEspecialistaEscuela(DataRow row)
        {
            AsignacionEspecialistaEscuela asignacion = new AsignacionEspecialistaEscuela();

            asignacion.Especialista = new EspecialistaPruebas();

            if (row.IsNull("ESPECIALISTAESCUELAID"))
                asignacion.AsignacionEspecialistaEscuelaID = null;
            else
                asignacion.AsignacionEspecialistaEscuelaID = (Int64)Convert.ChangeType(row["ESPECIALISTAESCUELAID"], typeof(Int64));
            if (row.IsNull("ESPECIALISTAID"))
                asignacion.Especialista.EspecialistaPruebaID = null;
            else
                asignacion.Especialista.EspecialistaPruebaID = (int)Convert.ChangeType(row["ESPECIALISTAID"], typeof(int));
            if (row.IsNull("ESTATUS"))
                asignacion.Activo = null;
            else
                asignacion.Activo = (bool)Convert.ChangeType(row["ESTATUS"], typeof(bool));
            if (row.IsNull("FECHAREGISTRO"))
                asignacion.FechaRegistro = null;
            else
                asignacion.FechaRegistro = (DateTime)Convert.ChangeType(row["FECHAREGISTRO"], typeof(DateTime));
            if (row.IsNull("FECHABAJA"))
                asignacion.FechaBaja = null;
            else
                asignacion.FechaBaja = (DateTime)Convert.ChangeType(row["FECHABAJA"], typeof(DateTime));

            return asignacion;
        }

       
        // Retrieve de UniversidadCarrrarRetHlp
        /// <summary>
        /// Consulta registros de EscuelaRetHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="escuela">EscuelaRetHlp que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de EscuelaRetHlp generada por la consulta</returns>
        public DataSet RetrieveUniversidadCarrera(IDataContext dctx, UniversidadCarrera universidadCarrera)
        {
            UniversidadCarreraRetHlp ctrl = new UniversidadCarreraRetHlp();
            DataSet ds = ctrl.Action(dctx, universidadCarrera);
            return ds;
        }

        public UniversidadCarrera DataRowToUniversidadCarrera(DataRow row)
        {
            UniversidadCarrera universidadCarrera = new UniversidadCarrera();

            if (row.IsNull("UniversidadCarreraID"))
                universidadCarrera.UniversidadCarreraID = null;
            else
                universidadCarrera.UniversidadCarreraID = (int)Convert.ChangeType(row["UniversidadCarreraID"], typeof(int));
            if (row.IsNull("UniversidadID"))
                universidadCarrera.UniversidadID = null;
            else
                universidadCarrera.UniversidadID = (long)Convert.ChangeType(row["UniversidadID"], typeof(long));
            if (row.IsNull("CarreraID"))
                universidadCarrera.CarreraID = null;
            else
                universidadCarrera.CarreraID = (long)Convert.ChangeType(row["CarreraID"], typeof(long));

            return universidadCarrera;
        }    
    }
}
