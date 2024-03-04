using System;
using System.Collections.Generic;
using System.Data;
using Framework.Base.DataAccess;
using POV.ContenidosDigital.BO;
using POV.ContenidosDigital.Busqueda.BO;
using POV.ContenidosDigital.Busqueda.DA;
using POV.ContenidosDigital.Busqueda.DAO;
using POV.ContenidosDigital.Service;
using POV.Profesionalizacion.BO;
using POV.Profesionalizacion.DA;
using POV.Profesionalizacion.Service;

namespace POV.ContenidosDigital.Busqueda.Service
{    
    /// <summary>
    /// Controlador del objeto APalabraClaveContenido
    /// </summary>
    public class PalabraClaveContenidoCtrl
    {
        /// <summary>
        /// Consulta registros de APalabraClaveContenido en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="aPalabraClaveContenido">APalabraClaveContenido que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de APalabraClaveContenido generada por la consulta</returns>
        public DataSet Retrieve(IDataContext dctx, APalabraClaveContenido palabraClaveContenido)
        {
            PalabraClaveContenidoRetHlp da = new PalabraClaveContenidoRetHlp();
            DataSet ds = da.Action(dctx, palabraClaveContenido);
            return ds;
        }

        /// <summary>
        /// Recupera un registro completo de palabraclavecontenido
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="palabraClaveContenido">PalabraClaveContenido que usaremos como filtro de busqueda</param>
        /// <returns>registro de APalabraClaveContenido completo, null en caso de no encontrarse</returns>
        public APalabraClaveContenido RetrieveComplete(IDataContext dctx, APalabraClaveContenido palabraClaveContenido)
        {

            if (palabraClaveContenido == null) throw new ArgumentNullException("PalabraClaveContenidoCtrl: palabraClaveContenido no puede ser nulo");


            APalabraClaveContenido palabraClaveContenidoComplete = null;

            DataSet dsPalabraClaveContenido = Retrieve(dctx, palabraClaveContenido);
            if (dsPalabraClaveContenido.Tables[0].Rows.Count > 0)
            {
                palabraClaveContenidoComplete = LastDataRowToAPalabraClaveContenido(dsPalabraClaveContenido);

                if (palabraClaveContenidoComplete is PalabraClaveContenidoDigital)
                {
                    (palabraClaveContenidoComplete as PalabraClaveContenidoDigital).ContenidoDigitalAgrupador = RetrieveListContenidoDigitalAgrupador(dctx, (palabraClaveContenidoComplete as PalabraClaveContenidoDigital));
                }
            }


            return palabraClaveContenidoComplete;


        }

        /// <summary>
        /// Recupera la lista de contenidos digitales agrupador de una palabra clave contenido digital de la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="palabraClaveContenidoDigital">PalabraClaveContenido que queremos filtrar</param>
        /// <returns>Listado de contenidos digitales agrupador de la palabra clave contenido digital</returns>
        public List<ContenidoDigitalAgrupador> RetrieveListContenidoDigitalAgrupador(IDataContext dctx, PalabraClaveContenidoDigital palabraClaveContenidoDigital)
        {
            if (palabraClaveContenidoDigital == null) throw new ArgumentNullException("PalabraClaveContenidoCtrl: palabraClaveContenidoDigital no puede ser nulo");
            if (palabraClaveContenidoDigital.PalabraClaveContenidoID == null) throw new ArgumentNullException("PalabraClaveContenidoCtrl: el identificador de palabraClaveContenidoDigital no puede ser nulo");

            List<ContenidoDigitalAgrupador> contenidos = new List<ContenidoDigitalAgrupador>();
            ContenidoDigitalAgrupadorCtrl contenidoDigitalAgrupadorCtrl = new ContenidoDigitalAgrupadorCtrl();

            DataSet dsContenidos = RetrievePalabraClaveContenidoDigital(dctx, palabraClaveContenidoDigital, new ContenidoDigitalAgrupador());
            foreach (DataRow row in dsContenidos.Tables[0].Rows)
            {
                ContenidoDigitalAgrupador contenidoDigital = new ContenidoDigitalAgrupador();
                contenidoDigital.ContenidoDigitalAgrupadorID = (long)Convert.ChangeType(row["ContenidoDigitalAgrupadorID"], typeof(long));

                contenidoDigital = contenidoDigitalAgrupadorCtrl.LastDataRowToContenidoDigitalAgrupador(contenidoDigitalAgrupadorCtrl.Retrieve(dctx, contenidoDigital));

                contenidos.Add(contenidoDigital);
            }

            return contenidos;
        }



        public List<ContenidoDigitalAgrupador> RetrieveListContenidoDigitalAgrupadorActivos(IDataContext dctx, PalabraClaveContenidoDigital palabraClaveContenidoDigital)
        {
            List<ContenidoDigitalAgrupador> contenidos = new List<ContenidoDigitalAgrupador>();

            PalabraClaveContenidoDigitalAgrupadorDARetHlp da = new PalabraClaveContenidoDigitalAgrupadorDARetHlp();
            DataSet dsContenidos = da.Action(dctx, palabraClaveContenidoDigital);
            ContenidoDigitalAgrupadorCtrl contenidoAgrupadorCtrl = new ContenidoDigitalAgrupadorCtrl();

            foreach (DataRow row in dsContenidos.Tables[0].Rows)
            {
                ContenidoDigitalAgrupador contenido = contenidoAgrupadorCtrl.DataRowToContenidoDigitalAgrupador(row);
                contenidos.Add(contenido);
            }

            return contenidos;
        }
        /// <summary>
        /// Consulta registros de Contenido Digital por Palabra Clave en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="pageSize">Número de elementos por página</param>
        /// <param name="currentPage">Página actual</param>
        /// <param name="sortColumn">Columna para ordenar el resultado</param>
        /// <param name="sortorder">Modo de ordenamiento (asc=ascendente, desc=descendente)</param>
        /// <param name="parametros">Arreglo tipo diccionario de parámetros adicionales</param>
        /// <returns>El DataSet que contiene la información generada por la consulta</returns>
        public DataSet RetrieveContenidoPorPalabraClave(IDataContext dctx, int pageSize, int currentPage, string sortColumn, string sortorder, Dictionary<string, string> parametros)
        {
            ContenidoDigitalDARetHlp da = new ContenidoDigitalDARetHlp();
            DataSet ds = da.Action(dctx, pageSize, currentPage, sortColumn, sortorder, parametros);
            return ds;
        }

        /// <summary>
        /// Crea un objeto de APalabraClaveContenido a partir de los datos del último DataRow del DataSet.
        /// </summary>
        /// <param name="ds">El DataSet que contiene la información de APalabraClaveContenido</param>
        /// <returns>Un objeto de APalabraClaveContenido creado a partir de los datos</returns>
        public APalabraClaveContenido LastDataRowToAPalabraClaveContenido(DataSet ds)
        {
            if (!ds.Tables.Contains("PalabraClaveContenido"))
                throw new Exception("LastDataRowToAPalabraClaveContenido: DataSet no tiene la tabla APalabraClaveContenido");
            int index = ds.Tables["PalabraClaveContenido"].Rows.Count;
            if (index < 1)
                throw new Exception("LastDataRowToAPalabraClaveContenido: El DataSet no tiene filas");
            return this.DataRowToAPalabraClaveContenido(ds.Tables["PalabraClaveContenido"].Rows[index - 1]);
        }
        /// <summary>
        /// Crea un objeto de APalabraClaveContenido a partir de los datos de un DataRow.
        /// </summary>
        /// <param name="row">El DataRow que contiene la información de APalabraClaveContenido</param>
        /// <returns>Un objeto de APalabraClaveContenido creado a partir de los datos</returns>
        public APalabraClaveContenido DataRowToAPalabraClaveContenido(DataRow row)
        {
            APalabraClaveContenido aPalabraClaveContenido = new PalabraClaveContenidoDigital() { PalabraClave = new PalabraClave() };
            if (row.IsNull("PalabraClaveContenidoID"))
                aPalabraClaveContenido.PalabraClaveContenidoID = null;
            else
                aPalabraClaveContenido.PalabraClaveContenidoID = (long)Convert.ChangeType(row["PalabraClaveContenidoID"], typeof(long));
            if (row.IsNull("FechaRegistro"))
                aPalabraClaveContenido.FechaRegistro = null;
            else
                aPalabraClaveContenido.FechaRegistro = (DateTime)Convert.ChangeType(row["FechaRegistro"], typeof(DateTime));
            if (row.IsNull("PalabraClaveID"))
                aPalabraClaveContenido.PalabraClave.PalabraClaveID = null;
            else
                aPalabraClaveContenido.PalabraClave.PalabraClaveID = (long)Convert.ChangeType(row["PalabraClaveID"], typeof(long));
            return aPalabraClaveContenido;
        }

        /// <summary>
        /// Crea un registro de APalabraClaveContenido en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="palabraClaveContenido">APalabraClaveContenido que desea crear</param>
        public void Insert(IDataContext dctx, APalabraClaveContenido palabraClaveContenido)
        {
            PalabraClaveContenidoInsHlp da = new PalabraClaveContenidoInsHlp();
            da.Action(dctx, palabraClaveContenido);
        }

        /// <summary>
        /// Crear un registro de PalabraClaveContenidoDigital en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="palabra">PalabraClaveContenidoDigital</param>
        /// <param name="contenido">ContenidoDigitalAgrupador</param>
        public void InsertPalabraClaveContenidoDigital(IDataContext dctx, PalabraClaveContenidoDigital palabra, ContenidoDigitalAgrupador contenido)
        {
            PalabraClaveContenidoDigitalInsHlp da = new PalabraClaveContenidoDigitalInsHlp();
            da.Action(dctx, palabra, contenido);
        }

        /// <summary>
        /// Elimina un registro de PalabraClaveContenidoDigital en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="palabraClaveContenido">PalabraClaveContenido que desea eliminar</param>
        public void DeletePalabraClaveContenidoDigital(IDataContext dctx, PalabraClaveContenidoDigital palabraClaveContenido, ContenidoDigitalAgrupador contenidoDigitalAgrupador)
        {
            PalabraClaveContenidoDigitalDelHlp da = new PalabraClaveContenidoDigitalDelHlp();
            da.Action(dctx, palabraClaveContenido, contenidoDigitalAgrupador);
        }

        /// <summary>
        /// Consulta registros de APalabraClaveContenido en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="aPalabraClaveContenido">APalabraClaveContenido que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de APalabraClaveContenido generada por la consulta</returns>
        public DataSet RetrievePalabraClaveContenidoDigital(IDataContext dctx, PalabraClaveContenidoDigital palabraClaveContenido, ContenidoDigitalAgrupador contenidoDigitalAgrupador)
        {
            PalabraClaveContenidoDigitalRetHlp da = new PalabraClaveContenidoDigitalRetHlp();
            DataSet ds = da.Action(dctx, palabraClaveContenido, contenidoDigitalAgrupador);
            return ds;
        }
        /// <summary>
        /// Obtiene las palabras clave de acuerdo al contenido digital agrupador proporcionado.
        /// </summary>
        /// <param name="dctx">DataContext que proveerá el acceso a la base de datos.</param>
        /// <param name="contenidoDigitalAgrupador">Contenido digital agrupador proporcionado.</param>
        /// <returns>Regresa un dataset con palabras clave específicas.</returns>
        public DataSet RetrievePalabraClaveContenidoDigital(IDataContext dctx,
                                                            ContenidoDigitalAgrupador contenidoDigitalAgrupador)
        {
            ContenidoDigitalDARetHlp da = new ContenidoDigitalDARetHlp();
            DataSet ds = da.Action(dctx, contenidoDigitalAgrupador);
            return ds;
        }

        /// <summary>
        /// Insertar Contenido Digital Agrupador
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="contenidoDigitalAgrupador">ContenidoDigitalAgrupador</param>
        public void InsertContenidoDigitalAgrupador(IDataContext dctx, ContenidoDigitalAgrupador contenidoDigitalAgrupador)
        {
            #region validaciones
            if (contenidoDigitalAgrupador == null)
                throw new ArgumentNullException("contenidoDigitalAgrupador", "PalabraClaveContenidoCtrl: el ContenidoDigitalAgrupador no puede ser nulo");
            if (contenidoDigitalAgrupador.ContenidoDigital == null)
                throw new ArgumentNullException("contenidoDigitalAgrupador", "PalabraClaveContenidoCtrl: el ContenidoDigital no puede ser nulo");
            #endregion

            var contenidoDigitalCtrl = new ContenidoDigitalCtrl();
            var contenidoDigital = new ContenidoDigital();
            var palabraClaveCtrl = new PalabraClaveCtrl();

            contenidoDigital = contenidoDigitalCtrl.LastDataRowToContenidoDigital(
                contenidoDigitalCtrl.Retrieve(dctx, contenidoDigitalAgrupador.ContenidoDigital));

            //recuperar el tag y separarlos
            string tag = contenidoDigital.Tags;
            string[] tags = tag.Split(',');

            foreach (var palabra in tags)
            {
                APalabraClaveContenido palabraClaveContenido = new PalabraClaveContenidoDigital();

                var palabraClave = new PalabraClave();
                palabraClave.Tag = palabra.Trim().ToUpper();

                //Buscar si existe el tag en PalabraClave
                DataSet dsPalabraClave = palabraClaveCtrl.Retrieve(dctx, palabraClave);

                if (dsPalabraClave.Tables[0].Rows.Count > 1)
                    throw new Exception("PalabraClaveContenidoCtrl: InsertContenidoDigitalAgrupador, La consulta devolvió más de un registro");

                if (dsPalabraClave.Tables[0].Rows.Count <= 0)
                {
                    #region Registrar PalabraClave y Recuperarla
                    palabraClave.TipoPalabraClave = ETipoPalabraClave.CONTENIDODIGITAL;
                    palabraClaveCtrl.Insert(dctx, palabraClave);

                    palabraClave.PalabraClaveID = palabraClaveCtrl.LastDataRowToPalabraClave(
                        palabraClaveCtrl.Retrieve(dctx, palabraClave)).PalabraClaveID;
                    #endregion

                    #region Registrar APalabraClaveContenido y Recuperarla
                    palabraClaveContenido.FechaRegistro = DateTime.Now;
                    palabraClaveContenido.PalabraClave = palabraClave;
                    this.Insert(dctx, palabraClaveContenido);

                    palabraClaveContenido.PalabraClaveContenidoID = this.LastDataRowToAPalabraClaveContenido(
                        this.Retrieve(dctx, palabraClaveContenido)).PalabraClaveContenidoID;
                    #endregion
                }
                else
                {
                    #region Recuperar PalabraClave
                    palabraClave = palabraClaveCtrl.LastDataRowToPalabraClave(dsPalabraClave);
                    #endregion

                    #region Recuperar APalabraClaveContenido
                    palabraClaveContenido.PalabraClave = palabraClave;
                    DataSet dsPalabraClaveContenido = this.Retrieve(dctx, palabraClaveContenido);

                    if (dsPalabraClaveContenido.Tables[0].Rows.Count <= 0)
                        throw new Exception("PalabraClaveContenidoCtrl: InsertContenidoDigitalAgrupador, No se encontró ningún registro");
                    if (dsPalabraClaveContenido.Tables[0].Rows.Count > 1)
                        throw new Exception("PalabraClaveContenidoCtrl: InsertContenidoDigitalAgrupador, La consulta devolvió más de un registro");

                    palabraClaveContenido.PalabraClaveContenidoID = 
                        this.LastDataRowToAPalabraClaveContenido(dsPalabraClaveContenido).PalabraClaveContenidoID;
                    #endregion
                }

                #region Registrar PalabraClaveContenidoDigital
                PalabraClaveContenidoDigital palabraClaveContenidoDigital = new PalabraClaveContenidoDigital();
                palabraClaveContenidoDigital.PalabraClaveContenidoID = palabraClaveContenido.PalabraClaveContenidoID;

                //VALIDAR QUE NO EXISTA LA CLAVE X-Y en la BD
                DataSet dsValidacion = 
                    this.RetrievePalabraClaveContenidoDigital(dctx, palabraClaveContenidoDigital, contenidoDigitalAgrupador);
                if (dsValidacion.Tables[0].Rows.Count == 0)
                {
                    //INSERTAR
                    this.InsertPalabraClaveContenidoDigital(dctx, palabraClaveContenidoDigital,
                                                            contenidoDigitalAgrupador);
                }
                #endregion
            }
        }

        /// <summary>
        /// Elimina un registro de PalabraClaveContenido en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="palabraClaveContenido">PalabraClaveContenido que desea eliminar</param>
        public void Delete(IDataContext dctx, APalabraClaveContenido palabraClaveContenido)
        {
            PalabraClaveContenidoDelHlp da = new PalabraClaveContenidoDelHlp();
            da.Action(dctx, palabraClaveContenido);
        }
        

    }
}
