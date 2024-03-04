using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Profesionalizacion.BO;
using POV.ContenidosDigital.BO;
using POV.ContenidosDigital.Service;
using POV.Profesionalizacion.DA;
using POV.Profesionalizacion.DAO;

namespace POV.Profesionalizacion.Service
{
    /// <summary>
    /// Controlador del objeto ContenidoDigitalAgrupador
    /// </summary>
    public class ContenidoDigitalAgrupadorCtrl
    {
        /// <summary>
        /// Consulta registros de ContenidoDigitalAgrupadorRetHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="contenidoDigitalAgrupadorRetHlp">ContenidoDigitalAgrupadorRetHlp que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de ContenidoDigitalAgrupadorRetHlp generada por la consulta</returns>
        public DataSet Retrieve(IDataContext dctx, ContenidoDigitalAgrupador contenidoDigitalAgrupador)
        {
            ContenidoDigitalAgrupadorRetHlp da = new ContenidoDigitalAgrupadorRetHlp();
            DataSet ds = da.Action(dctx, contenidoDigitalAgrupador);
            return ds;
        }

        public DataSet Retrieve(IDataContext dctx, ContenidoDigital contenido)
        {
            ContenidoDigitalAgrupadorDARetHlp da = new ContenidoDigitalAgrupadorDARetHlp();
            DataSet ds = da.Action(dctx, contenido);
            return ds;
        }

        /// <summary>
        /// Devuelve un registro completo de ContenidoDigitalAgrupador
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de dato</param>
        /// <param name="contenidoDigitalAgrupador">ContenidoDigitalAgrupador</param>
        /// <returns>ContenidoDigitalAgrupador, null en caso de no encontrarse</returns>
        public ContenidoDigitalAgrupador RetrieveCompete(IDataContext dctx, ContenidoDigitalAgrupador contenidoDigitalAgrupador)
        {
            if (contenidoDigitalAgrupador == null) throw new ArgumentNullException("ContenidoDigitalAgrupadorCtrl: contenidoDigitalAgrupador no puede ser nulo");
            if (contenidoDigitalAgrupador.ContenidoDigitalAgrupadorID == null) throw new ArgumentNullException("ContenidoDigitalAgrupadorCtrl: el identificador contenidoDigitalAgrupador no puede ser nulo");
            ContenidoDigitalAgrupador contenidoComplete = null;

            DataSet dsContenido = Retrieve(dctx, contenidoDigitalAgrupador);

            if (dsContenido.Tables[0].Rows.Count > 0)
            {
                EjeTematicoCtrl ejeTematicoCtrl = new EjeTematicoCtrl();
                SituacionAprendizajeCtrl situacionAprendizajeCtrl = new SituacionAprendizajeCtrl();
                ContenidoDigitalCtrl contenidoDigitalCtrl = new ContenidoDigitalCtrl();

                contenidoComplete = LastDataRowToContenidoDigitalAgrupador(dsContenido);
                contenidoComplete.EjeTematico = ejeTematicoCtrl.LastDataRowToEjeTematico(ejeTematicoCtrl.Retrieve(dctx, contenidoComplete.EjeTematico));
                contenidoComplete.SituacionAprendizaje = situacionAprendizajeCtrl.LastDataRowToSituacionAprendizaje(situacionAprendizajeCtrl.Retrieve(dctx,contenidoComplete.EjeTematico,contenidoComplete.SituacionAprendizaje));
                contenidoComplete.ContenidoDigital = contenidoDigitalCtrl.LastDataRowToContenidoDigital(contenidoDigitalCtrl.Retrieve(dctx, contenidoComplete.ContenidoDigital));
            }
            return contenidoComplete;
        }
        /// <summary>
        /// Crea un registro de ContenidoDigitalAgrupador en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="contenidoDigitalAgrupador">ContenidoDigitalAgrupador que desea crear</param>
        public void Insert(IDataContext dctx, ContenidoDigitalAgrupador contenidoDigitalAgrupador)
        {
            ContenidoDigitalAgrupadorInsHlp da = new ContenidoDigitalAgrupadorInsHlp();
            da.Action(dctx, contenidoDigitalAgrupador);
        }

        /// <summary>
        /// Elimina un registro de ContenidoDigitalAgrupador en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="contenidoDigitalAgrupador">contenidoDigitalAgrupador que desea eliminar</param>
        public void Delete(IDataContext dctx, ContenidoDigitalAgrupador contenidoDigitalAgrupador)
        {
            ContenidoDigitalAgrupadorDelHlp da = new ContenidoDigitalAgrupadorDelHlp();
            da.Action(dctx, contenidoDigitalAgrupador);
        }

        /// <summary>
        /// Crea un objeto de ContenidoDigitalAgrupador a partir de los datos del último DataRow del DataSet.
        /// </summary>
        /// <param name="ds">El DataSet que contiene la información de ContenidoDigitalAgrupador</param>
        /// <returns>Un objeto de ContenidoDigitalAgrupador creado a partir de los datos</returns>
        public ContenidoDigitalAgrupador LastDataRowToContenidoDigitalAgrupador(DataSet ds)
        {
            if (!ds.Tables.Contains("ContenidoDigitalAgrupador"))
                throw new Exception("LastDataRowToContenidoDigitalAgrupador: DataSet no tiene la tabla ContenidoDigitalAgrupador");
            int index = ds.Tables["ContenidoDigitalAgrupador"].Rows.Count;
            if (index < 1)
                throw new Exception("LastDataRowToContenidoDigitalAgrupador: El DataSet no tiene filas");
            return this.DataRowToContenidoDigitalAgrupador(ds.Tables["ContenidoDigitalAgrupador"].Rows[index - 1]);
        }
        /// <summary>
        /// Crea un objeto de ContenidoDigitalAgrupador a partir de los datos de un DataRow.
        /// </summary>
        /// <param name="row">El DataRow que contiene la información de ContenidoDigitalAgrupador</param>
        /// <returns>Un objeto de ContenidoDigitalAgrupador creado a partir de los datos</returns>
        public ContenidoDigitalAgrupador DataRowToContenidoDigitalAgrupador(DataRow row)
        {
            ContenidoDigitalAgrupador contenidoDigitalAgrupador = new ContenidoDigitalAgrupador();
            contenidoDigitalAgrupador.EjeTematico = new EjeTematico();
            contenidoDigitalAgrupador.SituacionAprendizaje = new SituacionAprendizaje();
            contenidoDigitalAgrupador.ContenidoDigital = new ContenidoDigital();

            if (row.IsNull("ContenidoDigitalAgrupadorID"))
                contenidoDigitalAgrupador.ContenidoDigitalAgrupadorID = null;
            else
                contenidoDigitalAgrupador.ContenidoDigitalAgrupadorID = (long)Convert.ChangeType(row["ContenidoDigitalAgrupadorID"], typeof(long));
            if (row.IsNull("EjeTematicoID"))
                contenidoDigitalAgrupador.EjeTematico.EjeTematicoID = null;
            else
                contenidoDigitalAgrupador.EjeTematico.EjeTematicoID = (long)Convert.ChangeType(row["EjeTematicoID"], typeof(long));
            if (row.IsNull("SituacionAprendizajeID"))
                contenidoDigitalAgrupador.SituacionAprendizaje.SituacionAprendizajeID = null;
            else
                contenidoDigitalAgrupador.SituacionAprendizaje.SituacionAprendizajeID = (long)Convert.ChangeType(row["SituacionAprendizajeID"], typeof(long));
            if (row.IsNull("ContenidoDigitalID"))
                contenidoDigitalAgrupador.ContenidoDigital.ContenidoDigitalID = null;
            else
                contenidoDigitalAgrupador.ContenidoDigital.ContenidoDigitalID = (long)Convert.ChangeType(row["ContenidoDigitalID"], typeof(long));
            if (row.IsNull("Activo"))
                contenidoDigitalAgrupador.Activo = null;
            else
                contenidoDigitalAgrupador.Activo = (bool)Convert.ChangeType(row["Activo"], typeof(bool));
            if (row.IsNull("FechaRegistro"))
                contenidoDigitalAgrupador.FechaRegistro = null;
            else
                contenidoDigitalAgrupador.FechaRegistro = (DateTime)Convert.ChangeType(row["FechaRegistro"], typeof(DateTime));
            if (row.IsNull("TipoAgrupador"))
            {
                contenidoDigitalAgrupador.AgrupadorContenidoDigital = null;
            }
            else
            {
                byte tipoAgrupador = (byte)Convert.ChangeType(row["TipoAgrupador"], typeof(byte));
                if (tipoAgrupador == (byte)ETipoAgrupador.COMPUESTO)
                {
                    contenidoDigitalAgrupador.AgrupadorContenidoDigital = new AgrupadorCompuesto();

                    if (row.IsNull("AgrupadorContenidoDigitalID"))
                        contenidoDigitalAgrupador.AgrupadorContenidoDigital.AgrupadorContenidoDigitalID = null;
                    else
                        contenidoDigitalAgrupador.AgrupadorContenidoDigital.AgrupadorContenidoDigitalID = (long)Convert.ChangeType(row["AgrupadorContenidoDigitalID"], typeof(long));
                }
                else if (tipoAgrupador == (byte)ETipoAgrupador.SIMPLE)
                {
                    contenidoDigitalAgrupador.AgrupadorContenidoDigital = new AgrupadorSimple();

                    if (row.IsNull("AgrupadorContenidoDigitalID"))
                        contenidoDigitalAgrupador.AgrupadorContenidoDigital.AgrupadorContenidoDigitalID = null;
                    else
                        contenidoDigitalAgrupador.AgrupadorContenidoDigital.AgrupadorContenidoDigitalID = (long)Convert.ChangeType(row["AgrupadorContenidoDigitalID"], typeof(long));

                } 
                else
                    contenidoDigitalAgrupador.AgrupadorContenidoDigital = null;
            }
            return contenidoDigitalAgrupador;
        }

        /// <summary>
        /// Elimina un registro de ContenidoDigitalAgrupador en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="contenidoDigitalAgrupador">contenidoDigitalAgrupador que desea eliminar</param>
        public void DeleteComplete(IDataContext dctx, ContenidoDigitalAgrupador contenidoDigitalAgrupador)
        {
            object firm = new object();
            try
            {
                dctx.OpenConnection(firm);
                dctx.BeginTransaction(firm);

                //Eliminar AgrupadorContenidoDigitalDetalle
                AgrupadorContenidoDigitalCtrl agrupadorContenidoDCtrl = new AgrupadorContenidoDigitalCtrl();
                agrupadorContenidoDCtrl.DeleteAgrupadorContenidoDigitalDetalle(dctx, contenidoDigitalAgrupador.AgrupadorContenidoDigital, contenidoDigitalAgrupador.ContenidoDigital);
                
                //Eliminar ContenidoDigitalAgrupador
                this.Delete(dctx, contenidoDigitalAgrupador);
            }
            catch (Exception ex)
            {
                dctx.RollbackTransaction(firm);
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(firm);                

                throw new Exception("Ocurrió un error mientras se eliminaba el contenidoDigitalAgrupador");
            }
            finally
            {
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(firm);
            }            
        }
    }
}
