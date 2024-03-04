using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Prueba.Diagnostico.Dinamica.BO;
using POV.Prueba.Diagnostico.Dinamica.DAO;
using POV.Prueba.Diagnostico.Dinamica.DA;
using POV.Reactivos.BO;
using POV.Reactivos.Service;
using POV.Prueba.BO;

namespace POV.Prueba.Diagnostico.Dinamica.Service
{
    /// <summary>
    /// Controlador del objeto RespuestaReactivoDinamica
    /// </summary>
    public class RespuestaReactivoDinamicaCtrl
    {
        /// <summary>
        /// Consulta registros de RespuestaReactivoDinamica en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="respuestaReactivoDinamicaRetHlp">RespuestaReactivoDinamica que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de RespuestaReactivoDinamica generada por la consulta</returns>
        public DataSet Retrieve(IDataContext dctx, RegistroPruebaDinamica registroPrueba, RespuestaReactivoDinamica respuestaReactivo)
        {
            RespuestaReactivoDinamicaRetHlp da = new RespuestaReactivoDinamicaRetHlp();
            DataSet ds = da.Action(dctx, registroPrueba, respuestaReactivo);
            return ds;
        }

        /// <summary>
        /// Consulta un registro completo de la respuesta reactivo dinamica
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="registroPrueba">RegistroPruebaDinamica</param>
        /// <param name="respuestaReactivo">RespuestaReactivoDinamica</param>
        /// <returns>Registro completo de una respuesta reactivo dinamica, nulo en caso contrario</returns>
        public RespuestaReactivoDinamica RetrieveComplete(IDataContext dctx, RegistroPruebaDinamica registroPrueba, RespuestaReactivoDinamica respuestaReactivo)
        {

            if (registroPrueba == null) throw new ArgumentNullException("RespuestaReactivoDinamicaCtrl: RegistroPruebaDinamica no puede ser nulo");
            if (respuestaReactivo == null) throw new ArgumentNullException("RespuestaReactivoDinamicaCtrl: respuestaReactivo no puede ser nulo");

            RespuestaReactivoDinamica respuestaComplete = new RespuestaReactivoDinamica();

            DataSet dsRespuestaReactivo = Retrieve(dctx, registroPrueba, respuestaReactivo);
            if (dsRespuestaReactivo.Tables[0].Rows.Count > 0)
            {
                ReactivoCtrl reactivoCtrl = new ReactivoCtrl();
                RespuestaPreguntaDinamicaCtrl respuestaPreguntaCtrl = new RespuestaPreguntaDinamicaCtrl();

                respuestaComplete = LastDataRowToRespuestaReactivoDinamica(dsRespuestaReactivo);
                respuestaComplete.Reactivo = reactivoCtrl.LastDataRowToReactivo(reactivoCtrl.Retrieve(dctx, new Reactivo { ReactivoID = respuestaComplete.Reactivo.ReactivoID, TipoReactivo = ETipoReactivo.ModeloGenerico }), ETipoReactivo.ModeloGenerico);
                respuestaComplete.ListaRespuestaPreguntas = respuestaPreguntaCtrl.RetrieveListaRespuestaPregunta(dctx, respuestaComplete);
                
            }

            return respuestaComplete;
        }

        /// <summary>
        /// Consulta una lista de respuesta reactivo relacionados a un registro de prueba en el sistema
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="registroPrueba">RegistroPruebaDinamica</param>
        /// <returns>Lista de respuesta reactivo, lista vacia en caso de no encontrar coincidencias</returns>
        public List<ARespuestaReactivo> RetrieveListRespuestaReactivo(IDataContext dctx, RegistroPruebaDinamica registroPrueba)
        {
            if (registroPrueba == null) throw new ArgumentNullException("RespuestaReactivoDinamicaCtrl: RegistroPruebaDinamica no puede ser nulo");
            if (registroPrueba.RegistroPruebaID == null) throw new ArgumentNullException("RespuestaReactivoDinamicaCtrl: RegistroPruebaID no puede ser nulo");

            List<ARespuestaReactivo> listaRespuestaReactivo = new List<ARespuestaReactivo>();


            DataSet dsRespuestaReactivos = Retrieve(dctx, new RegistroPruebaDinamica { RegistroPruebaID = registroPrueba.RegistroPruebaID }, new RespuestaReactivoDinamica());
            if (dsRespuestaReactivos.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsRespuestaReactivos.Tables[0].Rows)
                {
                    int respuestaReactivoID = Convert.ToInt32(dr["RespuestaReactivoID"].ToString());
                    listaRespuestaReactivo.Add(RetrieveComplete(dctx, new RegistroPruebaDinamica { RegistroPruebaID = registroPrueba.RegistroPruebaID }, new RespuestaReactivoDinamica { RespuestaReactivoID = respuestaReactivoID })); 
                    
                }
            }
            return listaRespuestaReactivo.OrderBy(item => item.RespuestaReactivoID).ToList();
        }
        /// <summary>
        /// Crea un registro de RespuestaReactivoDinamica en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="respuestaReactivoDinamicaInsHlp">RegistroPruebaDinamica que desea crear</param>
        /// <param name="respuestaReactivoDinamicaInsHlp">RespuestaReactivoDinamica que desea crear</param>
        public void Insert(IDataContext dctx, RegistroPruebaDinamica registroPrueba, RespuestaReactivoDinamica respuestaReactivo)
        {
            RespuestaReactivoDinamicaInsHlp da = new RespuestaReactivoDinamicaInsHlp();
            da.Action(dctx, registroPrueba, respuestaReactivo);
        }
        /// <summary>
        /// Inserta un registro completo de respuesta reactivo en la base de datos del sistema
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="registroPrueba"></param>
        /// <param name="respuestaReactivo"></param>
        public void InsertComplete(IDataContext dctx, RegistroPruebaDinamica registroPrueba, RespuestaReactivoDinamica respuestaReactivo)
        {
            if (registroPrueba == null) throw new ArgumentNullException("RespuestaReactivoDinamicaCtrl: RegistroPruebaDinamica no puede ser nulo ");
            if (registroPrueba.RegistroPruebaID == null) throw new ArgumentNullException("RespuestaReactivoDinamicaCtrl: RegistroPruebaID no puede ser nulo ");
            if (respuestaReactivo == null) throw new ArgumentNullException("RespuestaReactivoDinamicaCtrl: RespuestaReactivoDinamica no puede ser nulo ");
            if (respuestaReactivo.ListaRespuestaPreguntas == null) throw new ArgumentNullException("RespuestaReactivoDinamicaCtrl: ListaRespuestaPreguntas no puede ser nulo ");
            if (respuestaReactivo.ListaRespuestaPreguntas.Count == 0) throw new ArgumentNullException("RespuestaReactivoDinamicaCtrl: ListaRespuestaPreguntas no puede ser vacia ");
            
            #region *** begin transaction ***
            object myFirm = new object();
            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);
            try
            {
            #endregion

                Insert(dctx, registroPrueba, respuestaReactivo);

                respuestaReactivo.RespuestaReactivoID = LastDataRowToRespuestaReactivoDinamica(Retrieve(dctx, registroPrueba, respuestaReactivo)).RespuestaReactivoID;

                RespuestaPreguntaDinamicaCtrl respuestaPreguntaCtrl = new RespuestaPreguntaDinamicaCtrl();

                foreach (ARespuestaPregunta respuestaPregunta in respuestaReactivo.ListaRespuestaPreguntas)
                {
                    respuestaPreguntaCtrl.InsertComplete(dctx, respuestaReactivo, respuestaPregunta as RespuestaPreguntaDinamica);
                }

                #region *** commit transaction ***
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
                #endregion
        }

        /// <summary>
        /// Actualiza de manera optimista un registro de RespuestaReactivoDinamicaUpdHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="respuestaReactivoDinamicaUpdHlp">RespuestaReactivoDinamicaUpdHlp que tiene los datos nuevos</param>
        /// <param name="anterior">RespuestaReactivoDinamicaUpdHlp que tiene los datos anteriores</param>
        public void Update(IDataContext dctx, RespuestaReactivoDinamica respuestaReactivo, RespuestaReactivoDinamica previous)
        {
            RespuestaReactivoDinamicaUpdHlp da = new RespuestaReactivoDinamicaUpdHlp();
            da.Action(dctx, respuestaReactivo, previous);
        }
        /// <summary>
        /// Actualiza un registro completo de respuesta reactivo en la base de datos del sistema
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="respuestaReactivo">RespuestaReactivoDinamica</param>
        /// <param name="previous">previous</param>
        public void UpdateComplete(IDataContext dctx, RespuestaReactivoDinamica respuestaReactivo, RespuestaReactivoDinamica previous)
        {
            object myFirm = new object();
            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);
            try
            {
                Update(dctx, respuestaReactivo, previous);

                RespuestaPreguntaDinamicaCtrl respuestaPreguntaCtrl = new RespuestaPreguntaDinamicaCtrl();

                foreach (RespuestaPreguntaDinamica respuestaPregunta in respuestaReactivo.ListaRespuestaPreguntas)
                {

                    RespuestaPreguntaDinamica respuestaPreguntaPrevia = previous.ListaRespuestaPreguntas.FirstOrDefault(item => item.RespuestaPreguntaID == respuestaPregunta.RespuestaPreguntaID) as RespuestaPreguntaDinamica;
                    if (respuestaPreguntaPrevia != null)
                        respuestaPreguntaCtrl.UpdateComplete(dctx,respuestaPregunta, respuestaPreguntaPrevia);
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
        /// Crea un objeto de RespuestaReactivoDinamica a partir de los datos del último DataRow del DataSet.
        /// </summary>
        /// <param name="ds">El DataSet que contiene la información de RespuestaReactivoDinamica</param>
        /// <returns>Un objeto de RespuestaReactivoDinamica creado a partir de los datos</returns>
        public RespuestaReactivoDinamica LastDataRowToRespuestaReactivoDinamica(DataSet ds)
        {
            if (!ds.Tables.Contains("RespuestaReactivo"))
                throw new Exception("LastDataRowToRespuestaReactivoDinamica: DataSet no tiene la tabla RespuestaReactivoDinamica");
            int index = ds.Tables["RespuestaReactivo"].Rows.Count;
            if (index < 1)
                throw new Exception("LastDataRowToRespuestaReactivoDinamica: El DataSet no tiene filas");
            return this.DataRowToRespuestaReactivoDinamica(ds.Tables["RespuestaReactivo"].Rows[index - 1]);
        }
        /// <summary>
        /// Crea un objeto de RespuestaReactivoDinamica a partir de los datos de un DataRow.
        /// </summary>
        /// <param name="row">El DataRow que contiene la información de RespuestaReactivoDinamica</param>
        /// <returns>Un objeto de RespuestaReactivoDinamica creado a partir de los datos</returns>
        public RespuestaReactivoDinamica DataRowToRespuestaReactivoDinamica(DataRow row)
        {
            RespuestaReactivoDinamica respuestaReactivoDinamica = new RespuestaReactivoDinamica();
            respuestaReactivoDinamica.Reactivo = new Reactivo();
            if (row.IsNull("RespuestaReactivoID"))
                respuestaReactivoDinamica.RespuestaReactivoID = null;
            else
                respuestaReactivoDinamica.RespuestaReactivoID = (int)Convert.ChangeType(row["RespuestaReactivoID"], typeof(int));
            if (row.IsNull("ReactivoID"))
                respuestaReactivoDinamica.Reactivo.ReactivoID = null;
            else
                respuestaReactivoDinamica.Reactivo.ReactivoID = (Guid)Convert.ChangeType(row["ReactivoID"], typeof(Guid));
            if (row.IsNull("EstadoReactivo"))
                respuestaReactivoDinamica.EstadoReactivo = null;
            else
                respuestaReactivoDinamica.EstadoReactivo = (EEstadoReactivo)(byte)Convert.ChangeType(row["EstadoReactivo"], typeof(byte));
            if (row.IsNull("Tiempo"))
                respuestaReactivoDinamica.Tiempo = null;
            else
                respuestaReactivoDinamica.Tiempo = (int)Convert.ChangeType(row["Tiempo"], typeof(int));
            if (row.IsNull("FechaRegistro"))
                respuestaReactivoDinamica.FechaRegistro = null;
            else
                respuestaReactivoDinamica.FechaRegistro = (DateTime)Convert.ChangeType(row["FechaRegistro"], typeof(DateTime));
            return respuestaReactivoDinamica;
        }
    }
}
