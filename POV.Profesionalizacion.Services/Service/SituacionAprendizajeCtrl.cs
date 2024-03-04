using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Profesionalizacion.BO;
using POV.Profesionalizacion.DA;
using POV.Profesionalizacion.DAO;

namespace POV.Profesionalizacion.Service
{
    /// <summary>
    /// Controlador del objeto SituacionAprendizaje
    /// </summary>
    public class SituacionAprendizajeCtrl
    {
        /// <summary>
        /// Consulta registros de SituacionAprendizaje en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="ejeTematico">Eje Temático que provee el criterio de selección</param>
        /// <param name="situacionAprendizaje">SituacionAprendizaje que provee el criterio de selección para realizar la consulta 
        /// Si EstatusProfesionalizacion es nulo se consultaran las situacionesAprendizaje con estado Activo o  Mantenimiento</param>
        /// <returns>El DataSet que contiene la información de SituacionAprendizaje generada por la consulta</returns>
        public DataSet Retrieve(IDataContext dctx, EjeTematico ejeTematico, SituacionAprendizaje situacionAprendizaje)
        {
            SituacionAprendizajeRetHlp da = new SituacionAprendizajeRetHlp();
            DataSet ds = da.Action(dctx, ejeTematico, situacionAprendizaje);
            return ds;
        }
        /// <summary>
        /// Consulta los registros completos de SituacionAprendizaje con sus respectivos Ejes temáticos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos.</param>
        /// <param name="pageSize">Número del tamaño de la página (cantidad de registros por página)</param>
        /// <param name="currentPage">Número de página se desea visualizar.</param>
        /// <param name="sortColumn">Columna encargada del ordenamiento de la consulta.</param>
        /// <param name="sortorder">Tipo de ordenamiento (ASC o DESC)</param>
        /// <param name="parametros">Diccionario de parámetros encargados del filtro de la consulta.</param>
        /// <returns></returns>
        public DataSet RetrieveComplete(IDataContext dctx, int pageSize, int currentPage, string sortColumn, string sortorder,
            Dictionary<string, string> parametros)
        {
            SituacionAprendizajeDARetHlp da = new SituacionAprendizajeDARetHlp();
            DataSet ds = da.Action(dctx, pageSize, currentPage, sortColumn, sortorder, parametros);
            return ds;
        }

        /// <summary>
        /// Consulta una Situación Aprendizaje en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="ejeTematico">Eje Temático que provee el criterio de selección</param>
        /// <param name="situacion">SituacionAprendizaje que provee el criterio de selección para realizar la consulta
        /// Si EstatusProfesionalizacion es nulo se consultaran las situacionesAprendizaje con estado Activo o  Mantenimiento</param>
        /// <returns>El DataSet que contiene la información de SituacionAprendizajeRetHlp generada por la consulta</returns>
        public SituacionAprendizaje RetrieveComplete(IDataContext dctx, EjeTematico ejeTematico, SituacionAprendizaje situacion)
        {
            if (ejeTematico == null) throw new ArgumentNullException("ejeTematico", "SituacionAprendizajeCtrl:El Eje Temático no puede ser nulo");
            if (situacion == null) throw new ArgumentNullException("situacion", "SituacionAprendizajeCtrl:SituacionAprendizaje no puede ser nulo");

            SituacionAprendizaje _situacion = null;
            DataSet ds = Retrieve(dctx, ejeTematico, situacion);
            AgrupadorContenidoDigitalCtrl agrupadorContenidoCtrl = new AgrupadorContenidoDigitalCtrl();
            if (ds.Tables[0].Rows.Count > 0)
            {
                _situacion = this.LastDataRowToSituacionAprendizaje(ds);
                if (_situacion.AgrupadorContenidoDigital != null)
                {
                    //Consultar Agrupadores con su contenido digital
                    _situacion.AgrupadorContenidoDigital = agrupadorContenidoCtrl.RetrieveComplete(dctx, _situacion.AgrupadorContenidoDigital);

                }
            }
            return _situacion;
        }

        /// <summary>
        /// Consulta una Situación Aprendizaje en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="ejeTematico">Eje Temático que provee el criterio de selección</param>
        /// <param name="situacion">SituacionAprendizaje que provee el criterio de selección para realizar la consulta
        /// Si EstatusProfesionalizacion es nulo se consultaran las situacionesAprendizaje con estado Activo o  Mantenimiento</param>
        /// <returns>El DataSet que contiene la información de SituacionAprendizaje y sus Clasificadores generada por la consulta</returns>
        public SituacionAprendizaje RetrieveSimple(IDataContext dctx, EjeTematico ejeTematico, SituacionAprendizaje situacion)
        {
            if (ejeTematico == null) throw new ArgumentNullException("ejeTematico", "SituacionAprendizajeCtrl:El Eje Temático no puede ser nulo");
            if (situacion == null) throw new ArgumentNullException("situacion", "SituacionAprendizajeCtrl:SituacionAprendizaje no puede ser nulo");

            SituacionAprendizaje _situacion = null;
            DataSet ds = Retrieve(dctx, ejeTematico, situacion);
            AgrupadorContenidoDigitalCtrl agrupadorContenidoCtrl = new AgrupadorContenidoDigitalCtrl();
            if (ds.Tables[0].Rows.Count > 0)
            {
                _situacion = this.LastDataRowToSituacionAprendizaje(ds);
                if (_situacion.AgrupadorContenidoDigital != null)
                {
                    _situacion.AgrupadorContenidoDigital = agrupadorContenidoCtrl.RetrieveSimple(dctx, _situacion.AgrupadorContenidoDigital);
                }
            }
            return _situacion;
        }

        /// <summary>
        /// Elimina registro de Situación de Aprendizaje.Baja Lógica
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="ejeTematico">Eje Temático de la Situación Aprendizaje que se desea eliminar</param>
        /// <param name="situacion">Situación Aprendizaje que se desea eliminar</param>
        public void Delete(IDataContext dctx, EjeTematico ejeTematico, SituacionAprendizaje situacion)
        {
            SituacionAprendizajeDelHlp da = new SituacionAprendizajeDelHlp();
            da.Action(dctx, ejeTematico, situacion);
        }

        /// <summary>
        /// Crea un objeto de SituacionAprendizaje a partir de los datos del último DataRow del DataSet.
        /// </summary>
        /// <param name="ds">El DataSet que contiene la información de SituacionAprendizaje</param>
        /// <returns>Un objeto de SituacionAprendizaje creado a partir de los datos</returns>
        public SituacionAprendizaje LastDataRowToSituacionAprendizaje(DataSet ds)
        {
            if (!ds.Tables.Contains("SituacionAprendizaje"))
                throw new Exception("LastDataRowToSituacionAprendizaje: DataSet no tiene la tabla SituacionAprendizaje");
            int index = ds.Tables["SituacionAprendizaje"].Rows.Count;
            if (index < 1)
                throw new Exception("LastDataRowToSituacionAprendizaje: El DataSet no tiene filas");
            return this.DataRowToSituacionAprendizaje(ds.Tables["SituacionAprendizaje"].Rows[index - 1]);
        }

        /// <summary>
        /// Crea un objeto de SituacionAprendizaje a partir de los datos de un DataRow.
        /// </summary>
        /// <param name="row">El DataRow que contiene la información de SituacionAprendizaje</param>
        /// <returns>Un objeto de SituacionAprendizaje creado a partir de los datos</returns>
        public SituacionAprendizaje DataRowToSituacionAprendizaje(DataRow row)
        {
            SituacionAprendizaje situacionAprendizaje = new SituacionAprendizaje();
            if (row.IsNull("SituacionAprendizajeID"))
                situacionAprendizaje.SituacionAprendizajeID = null;
            else
                situacionAprendizaje.SituacionAprendizajeID = (long)Convert.ChangeType(row["SituacionAprendizajeID"], typeof(long));
            if (row.IsNull("Nombre"))
                situacionAprendizaje.Nombre = null;
            else
                situacionAprendizaje.Nombre = (string)Convert.ChangeType(row["Nombre"], typeof(string));
            if (row.IsNull("Descripcion"))
                situacionAprendizaje.Descripcion = null;
            else
                situacionAprendizaje.Descripcion = (string)Convert.ChangeType(row["Descripcion"], typeof(string));
            if (row.IsNull("EstatusProfesionalizacion"))
                situacionAprendizaje.EstatusProfesionalizacion = null;
            else
            {
                byte status = (byte)Convert.ChangeType(row["EstatusProfesionalizacion"], typeof(byte));
                situacionAprendizaje.EstatusProfesionalizacion = (EEstatusProfesionalizacion?)status;
            }

            if (row.IsNull("FechaRegistro"))
                situacionAprendizaje.FechaRegistro = null;
            else
                situacionAprendizaje.FechaRegistro = (DateTime)Convert.ChangeType(row["FechaRegistro"], typeof(DateTime));
            if (row.IsNull("AgrupadorContenidoDigitalID"))
                situacionAprendizaje.AgrupadorContenidoDigital = null;
            else
            {
                long idagrupador = (long)Convert.ChangeType(row["AgrupadorContenidoDigitalID"], typeof(long));
                situacionAprendizaje.AgrupadorContenidoDigital = new AgrupadorCompuesto();
                situacionAprendizaje.AgrupadorContenidoDigital.AgrupadorContenidoDigitalID = idagrupador;
            }
            return situacionAprendizaje;
        }

        /// <summary>
        /// Consulta las situaciones de aprendizaje para un eje temático junto con sus clasificadores y contenidos digitales
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="ejeTematico">Eje Temático que provee el criterio de selección</param>
        /// <param name="situacion">Situación Aprendizaje que provee el criterio de selección
        /// Si EstatusProfesionalizacion es nulo se consultaran las situacionesAprendizaje con estado Activo o  Mantenimiento</param>
        /// <returns>Lista de situaciones de aprendizaje de un eje temático</returns>
        public List<SituacionAprendizaje> RetrieveList(IDataContext dctx, EjeTematico ejeTematico, SituacionAprendizaje situacion)
        {
            if (ejeTematico == null || ejeTematico.EjeTematicoID == null) throw new ArgumentNullException("ejeTematico", "SituacionAprendizajeCtrl:El Eje Temático no puede ser nulo");
            if (situacion == null) situacion = new SituacionAprendizaje();

            List<SituacionAprendizaje> ls = null;
            DataSet ds = Retrieve(dctx, ejeTematico, situacion);
            int index = ds.Tables[0].Rows.Count;
            if (index > 0)
            {
                ls = new List<SituacionAprendizaje>();
                foreach (DataRow sit in ds.Tables[0].Rows)
                {
                    SituacionAprendizaje situacionAprendizaje = DataRowToSituacionAprendizaje(sit);
                    situacionAprendizaje = RetrieveComplete(dctx, ejeTematico, situacionAprendizaje);
                    ls.Add(situacionAprendizaje);
                }
            }
            else
            {
                ls = new List<SituacionAprendizaje>();
            }
            return ls;
        }

        /// <summary>
        ///Inserta un registro de situacion aprendizaje en la base de datos 
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="ejeTematico">Eje Tematico que se le insertara la situacion de aprendizaje</param>
        /// <param name="situacionAprendizaje">Situacion de aprendizaje que se desea insetar</param>
        public void Insert(IDataContext dctx, EjeTematico ejeTematico, SituacionAprendizaje situacionAprendizaje)
        {
            SituacionAprendizajeInsHlp da = new SituacionAprendizajeInsHlp();
            da.Action(dctx, ejeTematico, situacionAprendizaje);
        }

        /// <summary>
        /// Inserta un registro completo de situacion aprendizaje en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="ejeTematico">Eje Tematico que se le insertara la situacion de aprendizaje</param>
        /// <param name="situacionAprendizaje">Situacion de aprendizaje completo que se desea insertar</param>
        public void InsertComplete(IDataContext dctx, EjeTematico ejeTematico, SituacionAprendizaje situacionAprendizaje)
        {
            if (ejeTematico == null)
                throw new ArgumentNullException("SituacionAprendizajeCtrl: El Eje Tematico no puede ser nulo");
            if (ejeTematico.EjeTematicoID == null)
                throw new ArgumentNullException("SituacionAprendizajeCtrl: El EjeTematicoID no puede ser nulo");
            if (situacionAprendizaje == null)
                throw new ArgumentNullException("SituacionAprendizajeCtrl: La Situacion de Aprendizaje no puede ser nulo");
            if (situacionAprendizaje.AgrupadorContenidoDigital == null)
                throw new ArgumentNullException("SituacionAprendizajeCtrl: El AgrupadorContenidoDigital no puede ser nulo");
            object myFirm = new object();
            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);

            try
            {

                //insertarmos el agrupador compuesto padre
                AgrupadorContenidoDigitalCtrl agrupadorContenidoDigitalCtrl = new AgrupadorContenidoDigitalCtrl();
                agrupadorContenidoDigitalCtrl.Insert(dctx, situacionAprendizaje.AgrupadorContenidoDigital);

                //recuperaramos el identificador de agrupador compuesto padre
                DataSet dsAgrupador = agrupadorContenidoDigitalCtrl.Retrieve(dctx, situacionAprendizaje.AgrupadorContenidoDigital);
                situacionAprendizaje.AgrupadorContenidoDigital.AgrupadorContenidoDigitalID = agrupadorContenidoDigitalCtrl.LastDataRowToAAgrupadorContenidoDigital(dsAgrupador).AgrupadorContenidoDigitalID;

                //insertarmos la situacion de aprendizaje
                if (ValidateForInsert(dctx, ejeTematico, situacionAprendizaje))
                    Insert(dctx, ejeTematico, situacionAprendizaje);
                else
                    throw new DuplicateNameException("El nombre de la Situación de Aprendizaje ya se encuentra registrada en el sistema, por favor verifique.");

                //insertar simples
                if (situacionAprendizaje.AgrupadorContenidoDigital is AgrupadorCompuesto)
                {

                    List<AAgrupadorContenidoDigital> agrupadores = (situacionAprendizaje.AgrupadorContenidoDigital as AgrupadorCompuesto).AgrupadoresContenido;

                    foreach (AAgrupadorContenidoDigital agrupadorHijo in agrupadores)
                    {
                        agrupadorContenidoDigitalCtrl.Insert(dctx, agrupadorHijo, situacionAprendizaje.AgrupadorContenidoDigital.AgrupadorContenidoDigitalID.Value);
                    }
                }
                dctx.CommitTransaction(myFirm);
            }
            catch (Exception ex)
            {
                dctx.RollbackTransaction(myFirm);
                dctx.CloseConnection(myFirm);
                throw ex;

            }
            finally
            {
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(myFirm);
            }

        }

        /// <summary>
        /// Valida si existe una situacion de aprendizaje para un eje tematico con el mismo nombre
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="ejeTematico">eje tematico que provee el criterio de seleccion</param>
        /// <param name="situacionAprendizaje">situacion de aprendizaje que provee el criterio de selección</param>
        /// <returns>si existe o no un registro de situacion de aprendizaje</returns>
        private bool ValidateForInsert(IDataContext dctx, EjeTematico ejeTematico, SituacionAprendizaje situacionAprendizaje)
        {
            DataSet ds = Retrieve(dctx, ejeTematico, new SituacionAprendizaje { Nombre = situacionAprendizaje.Nombre });
            return ds.Tables[0].Rows.Count <= 0;
        }

        /// <summary>
        /// Actualiza de manera optimista un registro de SituacionAprendizaje en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="ejeTematico">Eje Temático que contiene a la Situación de Aprendizaje</param>
        /// <param name="situacionAprendizaje">SituacionAprendizaje que tiene los datos nuevos</param>
        /// <param name="anterior">SituacionAprendizaje que tiene los datos anteriores</param>
        public void Update(IDataContext dctx, EjeTematico ejeTematico, SituacionAprendizaje situacionAprendizaje, SituacionAprendizaje previous)
        {
            if (ejeTematico == null || ejeTematico.EjeTematicoID == null)
                throw new StandardException(MessageType.Error, "Error!", "El identificador del Eje Temático es requerido",
                    "POV.Profesionalizacion.Service", "SituacionAprendizajeCtrl", "UpdateComplete", null);

            if (situacionAprendizaje.SituacionAprendizajeID != previous.SituacionAprendizajeID)
                throw new StandardException(MessageType.Error, "Error de inconsistencia", "Los identificadores de Situación de Aprendizaje no coinciden",
                    "POV.Profesionalizacion.Service", "SituacionAprendizajeCtrl", "UpdateComplete", null);

            if (this.HayNombreRepetido(dctx, ejeTematico, situacionAprendizaje))
                throw new StandardException(MessageType.Error, "Error de datos", "El nombre de la situación ya se encuentra registrada. Verifique.",
                    "POV.Profesionalizacion.Service", "SituacionAprendizajeCtrl", "Update", null);

            SituacionAprendizajeUpdHlp da = new SituacionAprendizajeUpdHlp();
            da.Action(dctx, ejeTematico, situacionAprendizaje, previous);
        }

        private bool HayNombreRepetido(IDataContext dctx, EjeTematico ejeTematico, SituacionAprendizaje situacionAprendizaje)
        {
            bool lHayRepetidos = false;

            DataSet dsTest = this.Retrieve(dctx, ejeTematico, new SituacionAprendizaje() { Nombre = situacionAprendizaje.Nombre });
            if (dsTest != null && dsTest.Tables.Count > 0 && dsTest.Tables["SituacionAprendizaje"].Rows.Count > 0)
            {
                if (situacionAprendizaje.SituacionAprendizajeID == null) //Insert
                    lHayRepetidos = true;
                else  //Update
                {
                    String strCondicion = String.Format("SituacionAprendizajeID <> {0}", situacionAprendizaje.SituacionAprendizajeID.ToString());
                    lHayRepetidos = dsTest.Tables["SituacionAprendizaje"].Select(strCondicion).Length > 0;
                }
            }

            return lHayRepetidos;
        }
        /// <summary>
        /// Actualizar un registro de SituacionAprendizaje y sus objetos relacionados
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="ejeTematico">Eje Temático que contiene a la Situación de Aprendizaje</param>
        /// <param name="situacionAprendizaje">SituacionAprendizaje que tiene los datos nuevos</param>
        /// <param name="previous">SituacionAprendizaje que tiene los datos anteriores</param>
        public void UpdateComplete(IDataContext dctx, EjeTematico ejeTematico, SituacionAprendizaje situacionAprendizaje, SituacionAprendizaje previous)
        {
            if (situacionAprendizaje == null || previous == null)
                throw new ArgumentException("La Situación de Aprendizaje, así como el objeto original son requeridos!");

            if (ejeTematico == null || ejeTematico.EjeTematicoID == null)
                throw new ArgumentException("El Eje Temático es requerido!");

            if (situacionAprendizaje.AgrupadorContenidoDigital == null || situacionAprendizaje.AgrupadorContenidoDigital.AgrupadorContenidoDigitalID == null)
                throw new ArgumentException("El Agrupador de Contenido Digital para la Situación de Aprendizaje que se actualiza es requerido!", "situacionAprendizaje");

            if (previous.AgrupadorContenidoDigital == null || previous.AgrupadorContenidoDigital.AgrupadorContenidoDigitalID == null)
                throw new ArgumentException("El Agrupador de Contenido Digital para la Situación de Aprendizaje original es requerido!", "previous");

            object myFirm = new object();
            try
            {
                dctx.OpenConnection(myFirm);
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "Error de conexión", "Hubo un error al conectarse a la base de datos \n "
                    + ex.Message, "POV.Profesionalizacion.Service", "SituacionAprendizajeCtrl", "UpdateComplete", null, ex.InnerException);
            }

            try
            {
                #region Proceso Principal
                try
                {
                    dctx.BeginTransaction(myFirm);

                    this.Update(dctx, ejeTematico, situacionAprendizaje, previous);

                    AgrupadorContenidoDigitalCtrl ctrl = new AgrupadorContenidoDigitalCtrl();

                    ctrl.UpdateComplete(dctx, situacionAprendizaje.AgrupadorContenidoDigital, previous.AgrupadorContenidoDigital);


                    dctx.CommitTransaction(myFirm);
                }
                catch (Exception e2)
                {
                    string msgErr = e2.Message;
                    dctx.RollbackTransaction(myFirm);
                    throw new Exception(msgErr);
                }
                #endregion // Proceso Principal
            }
            catch (Exception e1)
            {
                throw e1;
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
        }
    }
}
