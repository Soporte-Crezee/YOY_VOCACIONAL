using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Linq;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Profesionalizacion.BO;
using POV.Profesionalizacion.DAO;
using POV.Profesionalizacion.DA;
using POV.ContenidosDigital.Service;
using POV.ContenidosDigital.BO;

namespace POV.Profesionalizacion.Service
{
    /// <summary>
    /// Controlador del objeto AAgrupadorContenidoDigital
    /// </summary>
    public class AgrupadorContenidoDigitalCtrl
    {
        #region Métodos básicos del generador
        /// <summary>
        /// Consulta registros de AAgrupadorContenidoDigital en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="aAgrupadorContenidoDigital">AAgrupadorContenidoDigital que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de AAgrupadorContenidoDigital generada por la consulta</returns>
        public DataSet Retrieve(IDataContext dctx, AAgrupadorContenidoDigital aAgrupadorContenidoDigital)
        {
            AgrupadorContenidoDigitalRetHlp da = new AgrupadorContenidoDigitalRetHlp();
            DataSet ds = da.Action(dctx, aAgrupadorContenidoDigital);
            return ds;
        }
        /// <summary>
        /// Consulta los registros hijos de AgrupadorCompuesto en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="agrupadorPadreID">Identificador del AgrupadorCompuesto del que se requieren los hijos</param>
        /// <returns>El DataSet que contiene la información de AAgrupadorContenidoDigital generada por la consulta</returns>
        public DataSet Retrieve(IDataContext dctx, Int64? agrupadorPadreID)
        {
            AgrupadorContenidoDigitalRetHlp da = new AgrupadorContenidoDigitalRetHlp();
            DataSet ds = da.Action(dctx, agrupadorPadreID);
            return ds;
        }
        /// <summary>
        /// Crea un registro de AAgrupadorContenidoDigital en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="aAgrupadorContenidoDigital">AAgrupadorContenidoDigital que desea crear</param>
        public void Insert(IDataContext dctx, AAgrupadorContenidoDigital aAgrupadorContenidoDigital)
        {
            AgrupadorContenidoDigitalInsHlp da = new AgrupadorContenidoDigitalInsHlp();
            da.Action(dctx, aAgrupadorContenidoDigital, null);
        }

        /// <summary>
        /// Crea un registro de AAgrupadorContenidoDigital en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="agrupadorContenido">AAgrupadorContenido que desea crear</param>
        /// <param name="agrupadorPadreID">Identificador del padre del agrupador que se agrega</param>
        public void Insert(IDataContext dctx, AAgrupadorContenidoDigital agrupadorContenido, Int64? agrupadorPadreID)
        {
            if (agrupadorContenido == null)
                throw new ArgumentException("Insert: El objeto AAgrupadorContenidoDigital no puede ser null");
            if (this.HayNombreRepetido(dctx, agrupadorContenido, agrupadorPadreID))
                throw new Exception("Insert: Datos duplicados; existe al menos un registro con el mismo nombre del que desea ingresar!");

            AgrupadorContenidoDigitalInsHlp da = new AgrupadorContenidoDigitalInsHlp();

            // Reglas de negocio
            agrupadorContenido.FechaRegistro = DateTime.Now;
            agrupadorContenido.Estatus = EEstatusProfesionalizacion.ACTIVO;

            da.Action(dctx, agrupadorContenido, agrupadorPadreID);
        }

        /// <summary>
        /// Elimina un registro de AAgrupadorContenidoDigital en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="aAgrupadorContenidoDigital">AAgrupadorContenidoDigital que desea eliminar</param>
        public void Delete(IDataContext dctx, AAgrupadorContenidoDigital aAgrupadorContenidoDigital)
        {
            AgrupadorContenidoDigitalDelHlp da = new AgrupadorContenidoDigitalDelHlp();
            da.Action(dctx, aAgrupadorContenidoDigital);
        }
        /// <summary>
        /// Crea un objeto de AAgrupadorContenidoDigital a partir de los datos del último DataRow del DataSet.
        /// </summary>
        /// <param name="ds">El DataSet que contiene la información de AAgrupadorContenidoDigital</param>
        /// <returns>Un objeto de AAgrupadorContenidoDigital creado a partir de los datos</returns>
        public AAgrupadorContenidoDigital LastDataRowToAAgrupadorContenidoDigital(DataSet ds)
        {
            if (!ds.Tables.Contains("AgrupadorContenidoDigital"))
                throw new Exception("LastDataRowToAAgrupadorContenidoDigital: DataSet no tiene la tabla AAgrupadorContenidoDigital");
            int index = ds.Tables["AgrupadorContenidoDigital"].Rows.Count;
            if (index < 1)
                throw new Exception("LastDataRowToAAgrupadorContenidoDigital: El DataSet no tiene filas");
            return this.DataRowToAAgrupadorContenidoDigital(ds.Tables["AgrupadorContenidoDigital"].Rows[index - 1]);
        }
        /// <summary>
        /// Crea un objeto de AAgrupadorContenidoDigital a partir de los datos de un DataRow.
        /// </summary>
        /// <param name="row">El DataRow que contiene la información de AAgrupadorContenidoDigital</param>
        /// <returns>Un objeto de AAgrupadorContenidoDigital creado a partir de los datos</returns>
        public AAgrupadorContenidoDigital DataRowToAAgrupadorContenidoDigital(DataRow row)
        {
            AAgrupadorContenidoDigital agrupadorContenidoDigital = null;
            ETipoAgrupador? tipoAgrupador = null;
            if (row.IsNull("TipoAgrupador"))
                throw new Exception("DataRowToAAgrupadorContenidoDigital: El Tipo de Agrupador no puede ser NULL");
            else
                tipoAgrupador = (ETipoAgrupador)Convert.ChangeType(row["TipoAgrupador"], typeof(Byte));

            switch (tipoAgrupador)
            {
                case ETipoAgrupador.SIMPLE:
                    agrupadorContenidoDigital = new AgrupadorSimple();
                    break;
                case ETipoAgrupador.COMPUESTO:
                    agrupadorContenidoDigital = new AgrupadorCompuesto();
                    break;
                default:
                    throw new Exception(string.Format("DataRowToAAgrupadorContenidoDigital: Tipo de Agrupador incorrecto: {0} ", tipoAgrupador.ToString()));
            }

            if (row.IsNull("AgrupadorContenidoDigitalID"))
                agrupadorContenidoDigital.AgrupadorContenidoDigitalID = null;
            else
                agrupadorContenidoDigital.AgrupadorContenidoDigitalID = (Int64)Convert.ChangeType(row["AgrupadorContenidoDigitalID"], typeof(Int64));
            if (row.IsNull("Nombre"))
                agrupadorContenidoDigital.Nombre = null;
            else
                agrupadorContenidoDigital.Nombre = (String)Convert.ChangeType(row["Nombre"], typeof(String));
            if (row.IsNull("EsPredeterminado"))
                agrupadorContenidoDigital.EsPredeterminado = null;
            else
                agrupadorContenidoDigital.EsPredeterminado = (bool)Convert.ChangeType(row["EsPredeterminado"], typeof(bool));
            if (row.IsNull("FechaRegistro"))
                agrupadorContenidoDigital.FechaRegistro = null;
            else
                agrupadorContenidoDigital.FechaRegistro = (DateTime)Convert.ChangeType(row["FechaRegistro"], typeof(DateTime));
            if (row.IsNull("EstatusProfesionalizacion"))
                agrupadorContenidoDigital.Estatus = null;
            else
                agrupadorContenidoDigital.Estatus = (EEstatusProfesionalizacion)Convert.ChangeType(row["EstatusProfesionalizacion"], typeof(Byte));
            if (row.IsNull("Competencias"))
                agrupadorContenidoDigital.Competencias = null;
            else
                agrupadorContenidoDigital.Competencias = (String)Convert.ChangeType(row["Competencias"], typeof(String));
            if (row.IsNull("Aprendizajes"))
                agrupadorContenidoDigital.Aprendizajes = null;
            else
                agrupadorContenidoDigital.Aprendizajes = (String)Convert.ChangeType(row["Aprendizajes"], typeof(String));
            return agrupadorContenidoDigital;
        }
        #endregion

        #region Métodos especializados para el tratamiento de los Agrupadores de Contenido
        /// <summary>
        /// Devuelve el árbol completo de un objeto concreto de AAgrupadorContenidoDigital sin objetos relacionados (simple)
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="agrupadorContenido">AAgrupadorContenidoDigital que provee el criterio de selección para realizar la consulta</param>
        /// <returns>AAgrupadorContenidoDigital con todas sus ramas y hojas por cada rama</returns>
        public AAgrupadorContenidoDigital RetrieveSimple(IDataContext dctx, AAgrupadorContenidoDigital agrupadorContenido)
        {
            return this.RetrieveSimple(dctx, agrupadorContenido, false);
        }

        private AAgrupadorContenidoDigital RetrieveSimple(IDataContext dctx, AAgrupadorContenidoDigital agrupadorContenido, bool lCompleto)
        {
            AAgrupadorContenidoDigital agrupadorContenidoReturn = null;

            DataSet ds = this.Retrieve(dctx, agrupadorContenido);

            if (ds == null || ds.Tables[0].Rows.Count == 0)
                return null;

            if (ds != null && ds.Tables.Count > 0)
                if (ds.Tables[0].Rows.Count > 1)
                    throw new Exception("RetrieveSimple: La consulta devolvió más de un registro para las condiciones proporcionadas en agrupadorContenido");
                else if (ds.Tables[0].Rows.Count <= 0)
                    return null;

            agrupadorContenidoReturn = this.LastDataRowToAAgrupadorContenidoDigital(ds);

            this.RetrieveArbol(dctx, agrupadorContenidoReturn, lCompleto);

            return agrupadorContenidoReturn;
        }

        private void RetrieveArbol(IDataContext dctx, AAgrupadorContenidoDigital agrupadorContenido, bool lCompleto)
        {
            if (agrupadorContenido is AgrupadorCompuesto)
            {
                //Traer hijos
                DataSet dsHijos = this.Retrieve(dctx, (Int64)agrupadorContenido.AgrupadorContenidoDigitalID);
                if (dsHijos != null && dsHijos.Tables.Count > 0)
                {
                    AAgrupadorContenidoDigital aAgrupadorNew;
                    foreach (DataRow rowHijo in dsHijos.Tables[0].Rows)
                    {
                        aAgrupadorNew = this.DataRowToAAgrupadorContenidoDigital(rowHijo);
                        this.RetrieveArbol(dctx, aAgrupadorNew, lCompleto);
                        agrupadorContenido.Agregar(aAgrupadorNew);
                    }
                }
            }

            if (lCompleto)
                agrupadorContenido.ContenidosDigitales = this.RetrieveListContenidoDigital(dctx, agrupadorContenido);
        }
        /// <summary>
        /// Recupera el árbol completo de AAgrupadorContenidoDigital y los datos completos relacionados
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="agrupadorContenido">>AAgrupadorContenidoDigital que provee el criterio de selección para realizar la consulta</param>
        /// <returns>AAgrupadorContenidoDigital con todas sus ramas y hojas completas por cada rama</returns>
        public AAgrupadorContenidoDigital RetrieveComplete(IDataContext dctx, AAgrupadorContenidoDigital agrupadorContenido)
        {
            AAgrupadorContenidoDigital agrupadorContenidoReturn = null;

            try
            {
                agrupadorContenidoReturn = this.RetrieveSimple(dctx, agrupadorContenido, true);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("RetrieveComplete: ({0})", ex.Message));
            }

            return agrupadorContenidoReturn;
        }

        public List<ContenidoDigital> RetrieveListContenidoDigital(IDataContext dctx, AAgrupadorContenidoDigital agrupadorContenido)
        {
            List<ContenidoDigital> listaContenidoReturn = new List<ContenidoDigital>();

            AgrupadorContenidoDetalleRetHlp helper = new AgrupadorContenidoDetalleRetHlp();
            ContenidoDigitalCtrl ctrl = new ContenidoDigitalCtrl();

            DataSet ds = helper.Action(dctx, agrupadorContenido);
            if (ds != null && ds.Tables.Count > 0)
            {
                Int64 contenidoDigitalID = 0;
                ContenidoDigital contenidoDigitalNew;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    if (row.IsNull("ContenidoDigitalID"))
                        throw new Exception("RetrieveListContenidoDigital: Se han encontrado datos nulos en la tabla AgrupadorContenidoDigitalDetalle");
                    else
                        contenidoDigitalID = (long)Convert.ChangeType(row["ContenidoDigitalID"], typeof(long));
                    try
                    {
                        DataSet dsContenido = ctrl.Retrieve(dctx, new ContenidoDigital() { ContenidoDigitalID = contenidoDigitalID });
                        if (dsContenido != null && dsContenido.Tables.Count > 0 && dsContenido.Tables[0].Rows.Count > 0)
                            contenidoDigitalNew = ctrl.LastDataRowToContenidoDigital(dsContenido);
                        else
                            contenidoDigitalNew = null;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format("RetrieveListContenidoDigital: Error al recuperar los Contenidos Digitales de {0}. \r\n {1}",
                            agrupadorContenido.Nombre.Trim(), ex.Message));
                    }
                    if (contenidoDigitalNew != null)
                        listaContenidoReturn.Add(contenidoDigitalNew);
                }
            }

            return listaContenidoReturn;
        }
        #endregion

        #region Métodos para acceder al DA "AgrupadorContenidoDA" para el paginado
        public DataSet Retrive(IDataContext dctx, AAgrupadorContenidoDigital agrupadorContenido, int pageSize, int currentPage, string sortColumn, string sortOrder, long contratoID)
        {
            AgrupadorContenidoDARetHlp da = new AgrupadorContenidoDARetHlp();
            DataSet ds = da.Action(dctx, agrupadorContenido, pageSize, currentPage, sortColumn, sortOrder, contratoID);
            return ds;
        }

        #endregion

        #region Métodos específicos para AgrupadorContenidoDigitalDetalle
        /// <summary>
        /// Insertar AgrupadorContenidoDigitalDetalle
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="aAgrupadorContenido">AAgrupadorContenidoDigital</param>
        /// <param name="contenido">ContenidoDigital</param>
        public void InsertAgrupadorContenidoDigitalDetalle(IDataContext dctx, AAgrupadorContenidoDigital aAgrupadorContenido, ContenidoDigital contenido)
        {
            //Validar que la relacion AAgrupadorContenidoDigital-ContenidoDigital no exista en la BD
            var existe = this.ValidarRelacionAAgrupadorContenidoDigital(dctx, aAgrupadorContenido, contenido);

            if (existe)
                throw new Exception("InsertAgrupadorContenidoDigitalDetalle: La relacion AAgrupadorContenidoDigital-ContenidoDigital ya existe");

            AgrupadorContenidoDigitalDetalleInsHlp da = new AgrupadorContenidoDigitalDetalleInsHlp();
            da.Action(dctx, aAgrupadorContenido, contenido);
        }

        /// <summary>
        /// Eliminar AgrupadorContenidoDigitalDetalle
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="aAgrupadorContenido">AAgrupadorContenidoDigital</param>
        /// <param name="contenido">ContenidoDigital</param>
        public void DeleteAgrupadorContenidoDigitalDetalle(IDataContext dctx, AAgrupadorContenidoDigital aAgrupadorContenido, ContenidoDigital contenido)
        {
            AgrupadorContenidoDigitalDetalleDelHlp da = new AgrupadorContenidoDigitalDetalleDelHlp();
            da.Action(dctx, aAgrupadorContenido, contenido);
        }

        /// <summary>
        /// Consultar AgrupadorContenidoDigitalDetalle
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="aAgrupadorContenido">AAgrupadorContenidoDigital</param>
        /// <param name="contenido">ContenidoDigital</param>
        /// <returns></returns>
        public DataSet RetrieveAgrupadorContenidoDigitalDetalle(IDataContext dctx, AAgrupadorContenidoDigital aAgrupadorContenido, ContenidoDigital contenido)
        {
            AgrupadorContenidoDigitalDetalleRetHlp da = new AgrupadorContenidoDigitalDetalleRetHlp();
            DataSet ds = da.Action(dctx, aAgrupadorContenido, contenido);
            return ds;
        }

        /// <summary>
        /// Validar si existe la clave en AgrupadorContenidoDigitalDetalle
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="aAgrupadorContenido">AAgrupadorContenidoDigital</param>
        /// <param name="contenido">ContenidoDigital</param>
        /// <returns></returns>
        private bool ValidarRelacionAAgrupadorContenidoDigital(IDataContext dctx, AAgrupadorContenidoDigital aAgrupadorContenido, ContenidoDigital contenido)
        {
            var result = false;

            DataSet ds = this.RetrieveAgrupadorContenidoDigitalDetalle(dctx, aAgrupadorContenido, contenido);

            if (ds.Tables[0].Rows.Count > 0)
                result = true;

            return result;
        }
        #endregion

        #region Métodos para Actualizar el árbol de Agrupadores de Contenido
        /// <summary>
        /// Actualiza un registro de AAgrupadorContenidoDigital en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="agrupadorContenido">AAgrupadorContenidoDigital que tiene los datos nuevos</param>
        /// <param name="anterior">AAgrupadorContenidoDigital que tiene los datos anteriores</param>
        public void Update(IDataContext dctx, AAgrupadorContenidoDigital agrupadorContenido, AAgrupadorContenidoDigital previous, Int64? agrupadorPadreID)
        {
            if (agrupadorContenido == null)
                throw new ArgumentException("Update: El objeto AAgrupadorContenidoDigital no puede ser null");
            if (agrupadorContenido.AgrupadorContenidoDigitalID != previous.AgrupadorContenidoDigitalID)
                throw new Exception("Update: Datos incongruentes, los identificadores no coinciden!");
            if (this.HayNombreRepetido(dctx, agrupadorContenido, agrupadorPadreID))
                throw new Exception("Update: Datos duplicados; existe otro registro con el mismo nombre del que desea ingresar!");

            AgrupadorContenidoDigitalUpdHlp da = new AgrupadorContenidoDigitalUpdHlp();

            if (agrupadorContenido is AgrupadorCompuesto && previous is AgrupadorCompuesto)
                da.Action(dctx, agrupadorContenido, previous);
            else
                if (agrupadorContenido is AgrupadorSimple && previous is AgrupadorSimple)
                    da.Action(dctx, agrupadorContenido, previous);
                else
                    throw new Exception("Update: Los Tipos de objetos que desea actualizar son incorrectos!!!");
        }
        /// <summary>
        /// Actualiza el árbol completo de AAGrupadorContenidoDigital en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="agrupadorContenido">AAgrupadorContenidoDigital que tiene los datos nuevos</param>
        /// <param name="previous">AAgrupadorContenidoDigital que tiene los datos anteriores</param>
        /// <param name="agrupadorPadreID">Opcional. El Identificador Padre del Agrupador si se requiere actualizar una rama</param>
        public void UpdateComplete(IDataContext dctx, AAgrupadorContenidoDigital agrupadorContenido, AAgrupadorContenidoDigital previous, Int64? agrupadorPadreID = null)
        {
            object myFirm = new object();
            try
            {
                dctx.OpenConnection(myFirm);
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "Error de conexión", "Hubo un error al conectarse a la base de datos \n "
                    + ex.Message, "POV.Profesionalizacion.Service", "AgrupadorContenidoDigitalCtrl", "UpdateComplete", null, ex.InnerException);
            }

            try
            {
                #region Proceso Principal
                try
                {
                    dctx.BeginTransaction(myFirm);

                    this.UpdateArbol(dctx, agrupadorContenido, previous, agrupadorPadreID);

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

        private void UpdateArbol(IDataContext dctx, AAgrupadorContenidoDigital agrupadorContenido, AAgrupadorContenidoDigital previous, Int64? agrupadorPadreID)
        {
            //Actualizar el registro que llega
            this.Update(dctx, agrupadorContenido, previous, agrupadorPadreID);
            if (agrupadorContenido is AgrupadorCompuesto)
            {
                if (previous is AgrupadorCompuesto)
                {
                    // Recorre el árbol del objeto anterior para eliminar los que no se encuentren
                    if (((AgrupadorCompuesto)previous).AgrupadoresContenido != null && ((AgrupadorCompuesto)previous).AgrupadoresContenido.Count > 0)
                    {
                        foreach (AAgrupadorContenidoDigital agrupadorAnterior in ((AgrupadorCompuesto)previous).AgrupadoresContenido)
                        {
                            AAgrupadorContenidoDigital agrupadorActual = null;
                            if (((AgrupadorCompuesto)agrupadorContenido).AgrupadoresContenido != null)
                                agrupadorActual = ((AgrupadorCompuesto)agrupadorContenido).AgrupadoresContenido.FirstOrDefault(a => a.AgrupadorContenidoDigitalID == agrupadorAnterior.AgrupadorContenidoDigitalID);
                            if (agrupadorActual == null)    //BORRAR
                                this.DeleteArbol(dctx, agrupadorAnterior);
                        }
                    }
                    if (((AgrupadorCompuesto)agrupadorContenido).AgrupadoresContenido != null && ((AgrupadorCompuesto)agrupadorContenido).AgrupadoresContenido.Count > 0)
                    {
                        // Recorrer el árbol del objeto que se actualiza
                        foreach (AAgrupadorContenidoDigital agrupadorActual in ((AgrupadorCompuesto)agrupadorContenido).AgrupadoresContenido)
                        {
                            AAgrupadorContenidoDigital agrupadorAnterior = null;
                            if (((AgrupadorCompuesto)previous).AgrupadoresContenido != null)
                                agrupadorAnterior = ((AgrupadorCompuesto)previous).AgrupadoresContenido.FirstOrDefault(a => a.AgrupadorContenidoDigitalID == agrupadorActual.AgrupadorContenidoDigitalID);

                            if (agrupadorAnterior == null)  //ES NUEVO
                                this.InsertArbol(dctx, agrupadorActual, agrupadorContenido.AgrupadorContenidoDigitalID);
                            else   //ACTUALIZAR
                                this.UpdateArbol(dctx, agrupadorActual, agrupadorAnterior, agrupadorContenido.AgrupadorContenidoDigitalID);
                        }

                    }
                }
                else
                    throw new Exception("UpdateArbol: Los Tipos de objetos que desea actualizar no coinciden!!!");

            }
        }

        private void InsertArbol(IDataContext dctx, AAgrupadorContenidoDigital agrupadorContenido, Int64? agrupadorPadreID)
        {
            // Insertar el registro principal
            this.Insert(dctx, agrupadorContenido, agrupadorPadreID);
            if (agrupadorContenido is AgrupadorCompuesto)
            {
                if (((AgrupadorCompuesto)agrupadorContenido).AgrupadoresContenido != null && ((AgrupadorCompuesto)agrupadorContenido).AgrupadoresContenido.Count > 0)
                {
                    AgrupadorCompuesto agrupadorNuevo = this.LastDataRowToAAgrupadorContenidoDigital(this.Retrieve(dctx, agrupadorContenido)) as AgrupadorCompuesto;
                    // Recorrer el árbol
                    foreach (AAgrupadorContenidoDigital agrupadorActual in ((AgrupadorCompuesto)agrupadorContenido).AgrupadoresContenido)
                    {
                        this.InsertArbol(dctx, agrupadorActual, agrupadorNuevo.AgrupadorContenidoDigitalID);
                    }
                }
            }
        }

        private void DeleteArbol(IDataContext dctx, AAgrupadorContenidoDigital agrupadorContenido)
        {
            this.Delete(dctx, agrupadorContenido);

            if (agrupadorContenido is AgrupadorCompuesto && ((AgrupadorCompuesto)agrupadorContenido).AgrupadoresContenido != null)
            {
                foreach (AAgrupadorContenidoDigital agrupadorActual in ((AgrupadorCompuesto)agrupadorContenido).AgrupadoresContenido)
                {
                    this.DeleteArbol(dctx, agrupadorActual);
                }
            }
        }
        #endregion

        #region Validaciones
        /// <summary>
        /// Valida la Existencia de un AgrupadorSimple repetido (mismo nombre) en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá el acceso a la base de datos</param>
        /// <param name="agrupadorContenido">AAgrupadorContenidoDigital que se evalúa</param>
        /// <param name="agrupadorPadreID">Identificador del Agrupador padre para validar sus hijos</param>
        /// <returns>True=Sí se encontraron nombres repetidos, False=No existe un registro con el mismo nombre en la base de datos</returns>
        public bool HayNombreRepetido(IDataContext dctx, AAgrupadorContenidoDigital agrupadorContenido, Int64? agrupadorPadreID)
        {
            bool lHayRepetidos = false;
            // Solamente se validan los HIJOS (AgrupadorSimple)
            if (agrupadorContenido is AgrupadorSimple && agrupadorPadreID != null)
            {
                DataSet dsTest = this.Retrieve(dctx, agrupadorPadreID);
                if (dsTest != null && dsTest.Tables.Count > 0 && dsTest.Tables["AgrupadorContenidoDigital"].Rows.Count > 0)
                {
                    String strCondicion;
                    if (agrupadorContenido.AgrupadorContenidoDigitalID == null) //Insert
                        strCondicion = String.Format("Nombre = '{0}'", agrupadorContenido.Nombre);
                    else  //Update
                        strCondicion = String.Format("Nombre = '{0}' AND AgrupadorContenidoDigitalID <> {1}", agrupadorContenido.Nombre, agrupadorContenido.AgrupadorContenidoDigitalID.ToString());
                    lHayRepetidos = dsTest.Tables["AgrupadorContenidoDigital"].Select(strCondicion).Length > 0;
                }
            }

            return lHayRepetidos;
        }
        #endregion
        
        #region Métodos para el DeleteComplete
        /// <summary>
        /// Obtiene los agrupadores de contenido digital de acuerdo al contenido digital proporcionado.
        /// </summary>
        /// <param name="dctx">DataContext que proveerá el acceso a la base de datos.</param>
        /// <param name="contenidoDigital">Contenido digital del que se desea saber sus agrupadores.</param>
        /// <returns>Regresa un dataset con los resultados de la búsqueda.</returns>
        public DataSet RetrieveAgrupadorContenidoDigital(IDataContext dctx, ContenidoDigital contenidoDigital)
        {
            AgrupadorContenidoDARetHlp da = new AgrupadorContenidoDARetHlp();
            DataSet ds = da.Action(dctx, contenidoDigital);
            return ds;
        }
        
        public DataSet RetrieveAgrupadorContenidoDigital(IDataContext dctx,
                                                         AAgrupadorContenidoDigital aAgrupadorContenidoDigital,
                                                         ContenidoDigital contenido)
        {
            AgrupadorContenidoDigitalDetalleRetHlp da = new AgrupadorContenidoDigitalDetalleRetHlp();
            DataSet ds = da.Action(dctx,aAgrupadorContenidoDigital, contenido);
            return ds;
        }
        #endregion    }
    }
}
