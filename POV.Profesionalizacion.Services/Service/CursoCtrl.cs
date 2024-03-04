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
using POV.ContenidosDigital.BO;
using POV.ContenidosDigital.Service;

namespace POV.Profesionalizacion.Service
{
    /// <summary>
    /// Controlador del objeto Curso
    /// </summary>
    public class CursoCtrl
    {
        #region Métodos básicos del generador

        /// <summary>
        /// Crea un objeto de Curso (AAgrupadorContenidoDigital) a partir de los datos del último DataRow del DataSet.
        /// </summary>
        /// <param name="ds">El DataSet que contiene la información de Curso</param>
        /// <returns>Un objeto de Curso creado a partir de los datos</returns>
        public AAgrupadorContenidoDigital LastDataRowToAAgrupadorContenidoDigital(DataSet ds)
        {
            if (!ds.Tables.Contains("Curso"))
                throw new Exception("LastDataRowToCurso: DataSet no tiene la tabla Curso");
            int index = ds.Tables["Curso"].Rows.Count;
            if (index < 1)
                throw new Exception("LastDataRowToCurso: El DataSet no tiene filas");
            return this.DataRowToAAgrupadorContenidoDigital(ds.Tables["Curso"].Rows[index - 1]);
        }
        /// <summary>
        /// Crea un objeto de Curso (AAgrupadorContenidoDigital) a partir de los datos de un DataRow.
        /// </summary>
        /// <param name="row">El DataRow que contiene la información de Curso</param>
        /// <returns>Un objeto de Curso creado a partir de los datos</returns>
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
                case ETipoAgrupador.COMPUESTO_CURSO:
                    agrupadorContenidoDigital = new Curso() { TemaCurso = new TemaCurso() };
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

            if (tipoAgrupador == ETipoAgrupador.COMPUESTO_CURSO)
            {
                if (row.IsNull("TemaCursoID"))
                    ((Curso)agrupadorContenidoDigital).TemaCurso.TemaCursoID = null;
                else
                    ((Curso)agrupadorContenidoDigital).TemaCurso.TemaCursoID = (Int32)Convert.ChangeType(row["TemaCursoID"], typeof(Int32));
                if (row.IsNull("Presencial"))
                    ((Curso)agrupadorContenidoDigital).Presencial = null;
                else
                    ((Curso)agrupadorContenidoDigital).Presencial = (EPresencial)Convert.ChangeType(row["Presencial"], typeof(Byte));
                if (row.IsNull("Informacion"))
                    ((Curso)agrupadorContenidoDigital).Informacion = null;
                else
                    ((Curso)agrupadorContenidoDigital).Informacion = (String)Convert.ChangeType(row["Informacion"], typeof(String));
            }
            return agrupadorContenidoDigital;
        }
        #endregion

        #region Métodos para la red social


        /// <summary>
        /// Consulta los registros cursos con paginacion
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos.</param>
        /// <param name="pageSize">Número del tamaño de la página (cantidad de registros por página)</param>
        /// <param name="currentPage">Número de página se desea visualizar.</param>
        /// <param name="sortColumn">Columna encargada del ordenamiento de la consulta.</param>
        /// <param name="sortorder">Tipo de ordenamiento (ASC o DESC)</param>
        /// <param name="parametros">Diccionario de parámetros encargados del filtro de la consulta.</param>
        /// <returns></returns>
        public DataSet Retrieve(IDataContext dctx, int pageSize, int currenPage, string sortColumn, string sortOrder, Dictionary<string, string> parametros)
        {
            CursoDARetHlp da = new CursoDARetHlp();
            DataSet ds = da.Action(dctx, pageSize, currenPage, sortColumn, sortOrder, parametros);
            return ds;
        }
        /// <summary>
        /// Consulta los detalles, informacion y contenidos digitales de un curso
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos.</param>
        /// <param name="pageSize">Número del tamaño de la página (cantidad de registros por página)</param>
        /// <param name="currentPage">Número de página se desea visualizar.</param>
        /// <param name="sortColumn">Columna encargada del ordenamiento de la consulta.</param>
        /// <param name="sortorder">Tipo de ordenamiento (ASC o DESC)</param>
        /// <param name="parametros">Diccionario de parámetros encargados del filtro de la consulta.</param>
        /// <returns></returns>
        public DataSet RetrieveDetails(IDataContext dctx, int pageSize, int currenPage, string sortColumn, string sortOrder, Dictionary<string, string> parametros)
        {
            CursoDetalleDARetHlp da = new CursoDetalleDARetHlp();
            DataSet ds = da.Action(dctx, pageSize, currenPage, sortColumn, sortOrder, parametros);
            return ds;
        }
        #endregion

        #region Métodos "Retrieve" para el tratamiento de los Agrupadores de Contenido (Curso)
        /// <summary>
        /// Consulta registros de AAgrupadorContenidoDigital (Curso) en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="agrupadorContenido">AAgrupadorContenidoDigital que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de Curso generada por la consulta</returns>
        public DataSet Retrieve(IDataContext dctx, AAgrupadorContenidoDigital agrupadorContenido)
        {
            CursoRetHlp da = new CursoRetHlp();
            DataSet ds = da.Action(dctx, agrupadorContenido);
            return ds;
        }
        /// <summary>
        /// Consulta los registros hijos de Curso en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="agrupadorPadreID">Identificador del AgrupadorCompuesto del que se requieren los hijos</param>
        /// <returns>El DataSet que contiene la información de AAgrupadorContenidoDigital generada por la consulta</returns>
        public DataSet Retrieve(IDataContext dctx, Int64 agrupadorPadreID)
        {
            CursoRetHlp da = new CursoRetHlp();
            DataSet ds = da.Action(dctx, agrupadorPadreID);
            return ds;
        }
        /// <summary>
        /// Devuelve el árbol completo de un objeto concreto de Curso (AAgrupadorContenidoDigital) sin objetos relacionados (simple)
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

            DataSet ds = this.Retrieve(dctx, (Curso)agrupadorContenido);
            if (ds != null && ds.Tables.Count > 0)
                if (ds.Tables[0].Rows.Count > 1)
                    throw new Exception("RetrieveSimple: La consulta devolvió más de un registro para las condiciones proporcionadas en agrupadorContenido");

            agrupadorContenidoReturn = this.LastDataRowToAAgrupadorContenidoDigital(ds);

            this.RetrieveArbol(dctx, agrupadorContenidoReturn, lCompleto);

            return agrupadorContenidoReturn;
        }
        private void RetrieveArbol(IDataContext dctx, AAgrupadorContenidoDigital agrupadorContenido, bool lCompleto)
        {
            if (agrupadorContenido is Curso)
            {
                //Traer hijos
                DataSet dsHijos = this.Retrieve(dctx, agrupadorContenido.AgrupadorContenidoDigitalID.Value);
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
                //Recuparar relaciones del CursoCtrl
                if (lCompleto)
                {
                    TemaCursoCtrl temaCtrl = new TemaCursoCtrl();
                    ((Curso)agrupadorContenido).TemaCurso = temaCtrl.LastDataRowToTemaCurso(temaCtrl.Retrieve(dctx, ((Curso)agrupadorContenido).TemaCurso));
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

        /// <summary>
        /// Consulta registros de CursoDetalleDARetHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="aAgrupadorContenidoDigital">Provee el criterio para seleccion para la realizacion de la consulta</param>
        /// <returns>El DataSet que contiene la información de CursoDetalleDARetHlp generada por la consulta</returns>
        public DataSet RetrieveContenidoDigital(IDataContext dctx, AAgrupadorContenidoDigital aAgrupadorContenidoDigital, long? contenidoDigitalID)
        {
            CursoDetalleDARetHlp da = new CursoDetalleDARetHlp();
            DataSet ds = da.Action(dctx, aAgrupadorContenidoDigital, contenidoDigitalID);
            return ds;
        }
        /// <summary>
        /// Obtiene los contenidos digitales asociados al curso solicitado.
        /// </summary>
        /// <param name="dctx">DataContext que proveerá el acceso a la base de datos.</param>
        /// <param name="contenidoDigital">Contenido digital del que se desea saber el curso asociado.</param>
        /// <returns>Regresa un dataset con los resultados de la consulta.</returns>
        public DataSet RetrieveCurso(IDataContext dctx, ContenidoDigital contenidoDigital)
        {
            CursoDARetHlp da = new CursoDARetHlp();
            DataSet ds = da.Action(dctx, contenidoDigital);
            return ds;
        }

        /// <summary>
        /// Recupera la lista de Contenido Digital de un agrupador (AgrupadorSimple de un Curso)
        /// </summary>
        /// <param name="dctx"></param>
        /// <param name="agrupadorContenido"></param>
        /// <returns></returns>
        public List<ContenidoDigital> RetrieveListContenidoDigital(IDataContext dctx, AAgrupadorContenidoDigital agrupadorContenido)
        {
            List<ContenidoDigital> listaContenidoReturn = new List<ContenidoDigital>();

            CursoDetalleDARetHlp helper = new CursoDetalleDARetHlp();
            ContenidoDigitalCtrl ctrl = new ContenidoDigitalCtrl();

            DataSet ds = helper.Action(dctx, agrupadorContenido);
            if (ds != null && ds.Tables.Count > 0)
            {
                Int64 contenidoDigitalID = 0;
                ContenidoDigital contenidoDigitalNew;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    if (row.IsNull("ContenidoDigitalID"))
                        throw new Exception("RetrieveListContenidoDigital: Se han encontrado datos nulos en la tabla CursoDetalle");
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

        #region Métodos para Insertar registros de Curso y AgrupadoresSimples de contenido para Cursos
        /// <summary>
        /// Crea un registro de AAgrupadorContenidoDigital (Curso ó AgrupadorSimple) en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="agrupadorContenido">AAgrupadorContenido que desea crear (Curso ó AgrupadorSimple)</param>
        /// <param name="agrupadorPadreID">Identificador del padre del agrupador que se agrega</param>
        public void Insert(IDataContext dctx, AAgrupadorContenidoDigital agrupadorContenido, Int64? agrupadorPadreID)
        {
            if (agrupadorContenido == null)
                throw new ArgumentException("Insert: El objeto AAgrupadorContenidoDigital no puede ser null");
            if (this.HayNombreRepetido(dctx, agrupadorContenido))
                throw new Exception("Insert: Datos duplicados; existe al menos un registro con el mismo nombre del que desea ingresar!");

            CursoInsHlp da = new CursoInsHlp();

            // Reglas de negocio
            agrupadorContenido.FechaRegistro = DateTime.Now;
            agrupadorContenido.Estatus = EEstatusProfesionalizacion.MANTENIMIENTO;

            if (agrupadorContenido is Curso)
                da.Action(dctx, agrupadorContenido as Curso, agrupadorPadreID);
            else
                if (agrupadorContenido is AgrupadorSimple)
                    da.Action(dctx, agrupadorContenido as AgrupadorSimple, agrupadorPadreID.Value);
                else
                    throw new Exception("Insert: Tipo de agrupador incorrecto!!!");
        }
        /// <summary>
        /// Crea un registro de AAgrupadorContenidoDigital con todas sus ramas en la base de datos
        /// (Objetos Curso y AgrupadorSimple)
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="agrupadorContenido">AAgrupadorContenido que desea crear (Curso ó AgrupadorSimple)</param>
        /// <param name="agrupadorPadreID">Identificador del padre del agrupador que se agrega</param>
        public void InsertComplete(IDataContext dctx, AAgrupadorContenidoDigital agrupadorContenido, Int64? agrupadorPadreID)
        {
            object myFirm = new object();
            try
            {
                dctx.OpenConnection(myFirm);
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "Error de conexión", "Hubo un error al conectarse a la base de datos \n "
                    + ex.Message, "POV.Profesionalizacion.Service", "CursoCtrl", "InsertComplete", null, ex.InnerException);
            }

            try
            {
                #region Proceso Principal
                try
                {
                    dctx.BeginTransaction(myFirm);

                    this.InsertArbol(dctx, agrupadorContenido, agrupadorPadreID);

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

        private void InsertArbol(IDataContext dctx, AAgrupadorContenidoDigital agrupadorContenido, Int64? agrupadorPadreID)
        {
            // Insertar el registro principal
            this.Insert(dctx, agrupadorContenido, agrupadorPadreID);
            if (agrupadorContenido is Curso)
            {
                if (((Curso)agrupadorContenido).AgrupadoresContenido != null && ((Curso)agrupadorContenido).AgrupadoresContenido.Count > 0)
                {
                    Curso cursoNuevo = this.LastDataRowToAAgrupadorContenidoDigital(this.Retrieve(dctx, agrupadorContenido)) as Curso;
                    // Recorrer el árbol
                    foreach (AAgrupadorContenidoDigital agrupadorActual in ((Curso)agrupadorContenido).AgrupadoresContenido)
                    {
                        this.InsertArbol(dctx, agrupadorActual, cursoNuevo.AgrupadorContenidoDigitalID);
                    }
                }
            }
        }

        #endregion

        #region Métodos para Modificar un Curso y su árbol respectivo
        /// <summary>
        /// Actualiza un registro de AAgrupadorContenidoDigital (Curso ó AgrupadorSimple) en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="agrupadorContenido">AAgrupadorContenidoDigital que tiene los datos nuevos</param>
        /// <param name="anterior">AAgrupadorContenidoDigital que tiene los datos anteriores</param>
        public void Update(IDataContext dctx, AAgrupadorContenidoDigital agrupadorContenido, AAgrupadorContenidoDigital previous)
        {
            if (agrupadorContenido == null)
                throw new ArgumentException("Update: El objeto AAgrupadorContenidoDigital no puede ser null");
            if (agrupadorContenido.AgrupadorContenidoDigitalID != previous.AgrupadorContenidoDigitalID)
                throw new Exception("Update: Datos incongruentes, los Identificadores no coinciden!"); 
            if (this.HayNombreRepetido(dctx, agrupadorContenido))
                throw new Exception("Update: Datos duplicados; existe otro registro con el mismo nombre del que desea ingresar!");

            CursoUpdHlp da = new CursoUpdHlp();

            if (agrupadorContenido is Curso && previous is Curso)
                da.Action(dctx, agrupadorContenido as Curso, previous as Curso);
            else
                if (agrupadorContenido is AgrupadorSimple && previous is AgrupadorSimple)
                    da.Action(dctx, agrupadorContenido as AgrupadorSimple, previous as AgrupadorSimple);
                else
                    throw new Exception("Update: Los Tipos de objetos que desea actualizar son incorrectos!!!");
        }
        /// <summary>
        /// Actualiza el árbol completo de AAGrupadorContenidoDigital (sólo objetos Curso y AgrupadorSimple) en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="agrupadorContenido">AAgrupadorContenidoDigital que tiene los datos nuevos</param>
        /// <param name="previous">AAgrupadorContenidoDigital que tiene los datos anteriores</param>
        public void UpdateComplete(IDataContext dctx, AAgrupadorContenidoDigital agrupadorContenido, AAgrupadorContenidoDigital previous)
        {
            object myFirm = new object();
            try
            {
                dctx.OpenConnection(myFirm);
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "Error de conexión", "Hubo un error al conectarse a la base de datos \n "
                    + ex.Message, "POV.Profesionalizacion.Service", "CursoCtrl", "UpdateComplete", null, ex.InnerException);
            }

            try
            {
                #region Proceso Principal
                try
                {
                    dctx.BeginTransaction(myFirm);

                    this.UpdateArbol(dctx, agrupadorContenido, previous);

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

        private void UpdateArbol(IDataContext dctx, AAgrupadorContenidoDigital agrupadorContenido, AAgrupadorContenidoDigital previous)
        {
            //Actualizar el registro que llega
            this.Update(dctx, agrupadorContenido, previous);
            if (agrupadorContenido is Curso)
            {
                if (previous is Curso)
                {
                    if (((Curso)agrupadorContenido).AgrupadoresContenido != null && ((Curso)agrupadorContenido).AgrupadoresContenido.Count > 0)
                    {
                        // Recorrer el árbol del objeto que se actualiza
                        foreach (AAgrupadorContenidoDigital agrupadorActual in ((Curso)agrupadorContenido).AgrupadoresContenido)
                        {
                            AAgrupadorContenidoDigital agrupadorAnterior = null;
                            if (((Curso)previous).AgrupadoresContenido != null)
                                agrupadorAnterior = ((Curso)previous).AgrupadoresContenido.FirstOrDefault(a => a.AgrupadorContenidoDigitalID == agrupadorActual.AgrupadorContenidoDigitalID);

                            if (agrupadorAnterior == null)  //ES NUEVO
                                this.InsertArbol(dctx, agrupadorActual, agrupadorContenido.AgrupadorContenidoDigitalID);
                            else   //ACTUALIZAR
                                this.UpdateArbol(dctx, agrupadorActual, agrupadorAnterior);
                        }

                    }
                    // Recorre el árbol del objeto anterior para eliminar los que no se encuentren
                    if (((Curso)previous).AgrupadoresContenido != null && ((Curso)previous).AgrupadoresContenido.Count > 0)
                    {
                        foreach (AAgrupadorContenidoDigital agrupadorAnterior in ((Curso)previous).AgrupadoresContenido)
                        {
                            AAgrupadorContenidoDigital agrupadorActual = null;
                            if (((Curso)agrupadorContenido).AgrupadoresContenido != null)
                                agrupadorActual = ((Curso)agrupadorContenido).AgrupadoresContenido.FirstOrDefault(a => a.AgrupadorContenidoDigitalID == agrupadorAnterior.AgrupadorContenidoDigitalID);
                            if (agrupadorActual == null)    //BORRAR
                                this.DeleteArbol(dctx, agrupadorAnterior);
                        }
                    }
                }
                else
                    throw new Exception("UpdateArbol: Los Tipos de objetos que desea actualizar no coinciden!!!");

            }
        }
        #endregion

        #region Métodos para Eliminar un CursoCtrl
        /// <summary>
        /// Elimina un registro de Curso en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="agrupadorContenido">Curso que desea eliminar</param>
        private void Delete(IDataContext dctx, AAgrupadorContenidoDigital agrupadorContenido)
        {
            CursoDelHlp da = new CursoDelHlp();
            da.Action(dctx, agrupadorContenido);
        }
        /// <summary>
        /// Elimina un AAgrupadorContenidoDigital (Curso ó AgrupadorSimple) con todas sus ramas y hojas hacia abajo.
        /// La eliminación de los Cursos y AgrupadoresSimples de Cursos es Lógica.
        /// La eliminación de la asignación de contenidos digitales al Curso es física.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="agrupadorContenidoDigitalID">Identificador del AAgrupadorContenidoDigital que desea eliminar</param>
        public void DeleteComplete(IDataContext dctx, Int64 agrupadorContenidoDigitalID)
        {
            AAgrupadorContenidoDigital agrupadorContenido = this.RetrieveComplete(dctx, new Curso() { AgrupadorContenidoDigitalID = agrupadorContenidoDigitalID });
            object myFirm = new object();
            try
            {
                dctx.OpenConnection(myFirm);
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "Error de conexión", "Hubo un error al conectarse a la base de datos \n "
                    + ex.Message, "POV.Profesionalizacion.Service", "CursoCtrl", "DeleteComplete", null, ex.InnerException);
            }

            try
            {
                #region Proceso Principal
                try
                {
                    dctx.BeginTransaction(myFirm);

                    this.DeleteArbol(dctx, agrupadorContenido);

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

        private void DeleteArbol(IDataContext dctx, AAgrupadorContenidoDigital agrupadorContenido)
        {
            this.Delete(dctx, agrupadorContenido);
            if (agrupadorContenido.ContenidosDigitales != null && agrupadorContenido.ContenidosDigitales.Count > 0)
            {
                foreach (var contenidoDigital in agrupadorContenido.ContenidosDigitales)
                {
                    this.DeleteContenidoCurso(dctx, agrupadorContenido, contenidoDigital);
                }
            }

            if (agrupadorContenido is Curso && ((Curso)agrupadorContenido).AgrupadoresContenido != null)
            {
                foreach (AAgrupadorContenidoDigital agrupadorActual in ((Curso)agrupadorContenido).AgrupadoresContenido)
                {
                    this.DeleteArbol(dctx, agrupadorActual);
                }
            }
        }
        #endregion

        #region Validaciones
        /// <summary>
        /// Valida la Existencia de un CURSO repetido (mismo nombre) en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá el acceso a la base de datos</param>
        /// <param name="agrupadorContenido">AAgrupadorContenidoDigital que se evalúa</param>
        /// <returns>True=Sí se encontraron nombres repetidos, False=No existe un registro con el mismo nombre en la base de datos</returns>
        public bool HayNombreRepetido(IDataContext dctx, AAgrupadorContenidoDigital agrupadorContenido)
        {
            bool lHayRepetidos = false;
            // Solamente se validan los PADRES (CURSOS)
            if (agrupadorContenido is Curso)
            {
                AAgrupadorContenidoDigital agrupadorTest = new Curso() { Nombre = agrupadorContenido.Nombre };
                DataSet dsTest = this.Retrieve(dctx, agrupadorTest);
                if (dsTest != null && dsTest.Tables.Count > 0 && dsTest.Tables["Curso"].Rows.Count > 0)
                {
                    if (agrupadorContenido.AgrupadorContenidoDigitalID == null) //Insert
                        lHayRepetidos = true;
                    else  //Update
                        lHayRepetidos = dsTest.Tables["Curso"].Select("AgrupadorContenidoDigitalID <> " + agrupadorContenido.AgrupadorContenidoDigitalID.ToString()).Length > 0;
                }
            }

            return lHayRepetidos;
        }
        #endregion

        #region Asignar contenidos digitales a cursos
        /// <summary>
        /// Asigna contenidos digital a un curso
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="aAgrupadorContenidoDigital">Agrupador que se desea insertal</param>
        /// <param name="contenidoDigital">Contenido digital que se desea asignar</param>
        public void InsertContenidoCurso(IDataContext dctx, AAgrupadorContenidoDigital aAgrupadorContenidoDigital, ContenidoDigital contenidoDigital)
        {
            DataSet ds = this.RetrieveContenidoDigital(dctx, aAgrupadorContenidoDigital, contenidoDigital.ContenidoDigitalID);
            if (ds.Tables["CursoDetalle"].Rows.Count <= 0)
            {
                ContenidoDigitalCursoInsHlp da = new ContenidoDigitalCursoInsHlp();
                da.Action(dctx, aAgrupadorContenidoDigital, contenidoDigital);
            }
            else
            {
                throw new DuplicateNameException("El contenido digital ya se encuentra asignado al curso, por favor verifique.");
            }
        }

        /// <summary>
        /// Desasigna contenidos digital a un curso
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="aAgrupadorContenidoDigital">Provee los criterios de eliminación</param>
        /// <param name="contenidoDigital">Provee los criterios de eliminación</param>
        public void DeleteContenidoCurso(IDataContext dctx, AAgrupadorContenidoDigital aAgrupadorContenidoDigital, ContenidoDigital contenidoDigital)
        {
            ContenidoDigitalCursoDelHlp da = new ContenidoDigitalCursoDelHlp();
            da.Action(dctx, aAgrupadorContenidoDigital, contenidoDigital);
        }

        #endregion

    }
}
