using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using POV.Reactivos.BO;
using POV.Reactivos.Service;
using GP.SocialEngine.BO;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.ReactivosUsuario.BO;
using POV.ReactivosUsuario.DAO;

namespace POV.ReactivosUsuario.Service
{
    /// <summary>
    /// Controlador del objeto RespuestaPreguntaUsuario
    /// </summary>
    public class RespuestaPreguntaUsuarioCtrl
    {
        /// <summary>
        /// Consulta registros de RespuestaPreguntaUsuarioRetHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="respuestaPreguntaUsuarioRetHlp">RespuestaPreguntaUsuarioRetHlp que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de RespuestaPreguntaUsuarioRetHlp generada por la consulta</returns>
        public DataSet Retrieve(IDataContext dctx, RespuestaReactivoUsuario respuestaReactivo, RespuestaPreguntaUsuario respuestaPreguntaUsuario)
        {

            RespuestaPreguntaUsuarioRetHlp da = new RespuestaPreguntaUsuarioRetHlp();
            DataSet ds = da.Action(dctx, respuestaReactivo, respuestaPreguntaUsuario);
            return ds;
        }
        /// <summary>
        /// Devuelve una lista de respuestas pregunta completa
        /// </summary>
        /// <param name="dctx"></param>
        /// <param name="respuestaReactivoUsuario"></param>
        /// <returns></returns>
        public List<RespuestaPreguntaUsuario> RetrieveCompleteListaRespuestaPregunta(IDataContext dctx, RespuestaReactivoUsuario respuestaReactivoUsuario)
        {
            if (respuestaReactivoUsuario == null)
                throw new Exception("La respuesta reactivo no puede ser nulo");
            if (respuestaReactivoUsuario.RespuestaReactivoUsuarioID == null)
                throw new Exception("El identificador de la respuesta reactivo no puede ser nulo");

            List<RespuestaPreguntaUsuario> lista = new List<RespuestaPreguntaUsuario>();

            DataSet ds = Retrieve(dctx, respuestaReactivoUsuario, null);

            if (ds.Tables[0].Rows.Count > 0)
            {
                RespuestaUsuarioCtrl respuestaUsuarioCtrl = new RespuestaUsuarioCtrl();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    RespuestaPreguntaUsuario respuesta = DataRowToRespuestaPreguntaUsuario(dr);

                    respuesta.RespuestaUsuario = respuestaUsuarioCtrl.RetrieveRespuestaUsuario(dctx, respuesta);

                    lista.Add(respuesta);
                }
            }
            return lista;
        }
        /// <summary>
        /// Devuelve un registro completo de RespuestaPreguntaUsuario
        /// </summary>
        /// <param name="dctx"></param>
        /// <param name="respuestaPreguntaUsuario"></param>
        /// <returns></returns>
        public RespuestaPreguntaUsuario RetrieveComplete(IDataContext dctx, RespuestaReactivoUsuario respuestaReactivo, RespuestaPreguntaUsuario respuestaPreguntaUsuario)
        {
            if (respuestaReactivo == null)
                throw new Exception("Se requiere la respuesta reactivo");
            if (respuestaReactivo.RespuestaReactivoUsuarioID == null)
                throw new Exception("Se requiere el identificador de la respuesta reactivo usuario");
            if (respuestaPreguntaUsuario == null)
                throw new Exception("Se requiere la respuesta pregunta");
            if (respuestaPreguntaUsuario.RespuestaPreguntaUsuarioID == null)
                throw new Exception("Se requiere el identificador de la respuesta pregunta usuario");
            object myFirm = new object();
            try
            {
                dctx.OpenConnection(myFirm);
                RespuestaPreguntaUsuario respuesta = null;

                DataSet ds = Retrieve(dctx, respuestaReactivo, respuestaPreguntaUsuario);

                if (ds.Tables[0].Rows.Count > 0) 
                {
                    respuesta = LastDataRowToRespuestaPreguntaUsuario(ds);

                    RespuestaUsuarioCtrl respuestaUsuarioCtrl = new RespuestaUsuarioCtrl();
                    respuesta.RespuestaUsuario = respuestaUsuarioCtrl.RetrieveRespuestaUsuario(dctx, respuesta);

                }
                return respuesta;
            }
            catch (Exception ex)
            {
                throw new Exception("Hubo un error al consultar. " + ex.Message);
            }
            finally
            {
                dctx.CommitTransaction(myFirm);
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(myFirm);
            }
        }
        /// <summary>
        /// Crea un registro de RespuestaPreguntaUsuarioInsHlp en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="respuestaPreguntaUsuarioInsHlp">RespuestaPreguntaUsuarioInsHlp que desea crear</param>
        public void Insert(IDataContext dctx, RespuestaReactivoUsuario respuestaReactivo, RespuestaPreguntaUsuario respuestaPreguntaUsuario)
        {
            if (respuestaReactivo == null)
                throw new Exception("La respuesta reactivo no puede ser nulo");
            if (respuestaReactivo.RespuestaReactivoUsuarioID == null)
                throw new Exception("El identificador de la respuesta reactivo no puede ser nulo");
            if (respuestaPreguntaUsuario == null)
                throw new Exception("La respuesta pregunta no puede ser nulo");
            if (respuestaPreguntaUsuario.RespuestaPreguntaUsuarioID == null)
                throw new Exception("El identificador de la respuesta pregunta no puede ser nulo");
            if (respuestaPreguntaUsuario.Pregunta == null)
                throw new Exception("La pregunta no puede ser nulo");
            if (respuestaPreguntaUsuario.Pregunta.PreguntaID == null)
                throw new Exception("El identificador de la pregunta no puede ser nulo");

            RespuestaPreguntaUsuarioInsHlp da = new RespuestaPreguntaUsuarioInsHlp();
            da.Action(dctx, respuestaReactivo, respuestaPreguntaUsuario);
        }

       
        /// <summary>
        /// Crea un objeto de RespuestaPreguntaUsuario a partir de los datos del último DataRow del DataSet.
        /// </summary>
        /// <param name="ds">El DataSet que contiene la información de RespuestaPreguntaUsuario</param>
        /// <returns>Un objeto de RespuestaPreguntaUsuario creado a partir de los datos</returns>
        public RespuestaPreguntaUsuario LastDataRowToRespuestaPreguntaUsuario(DataSet ds)
        {
            if (!ds.Tables.Contains("RespuestaPreguntaUsuario"))
                throw new Exception("LastDataRowToRespuestaPreguntaUsuario: DataSet no tiene la tabla RespuestaPreguntaUsuario");
            int index = ds.Tables["RespuestaPreguntaUsuario"].Rows.Count;
            if (index < 1)
                throw new Exception("LastDataRowToRespuestaPreguntaUsuario: El DataSet no tiene filas");
            return this.DataRowToRespuestaPreguntaUsuario(ds.Tables["RespuestaPreguntaUsuario"].Rows[index - 1]);
        }
        /// <summary>
        /// Crea un objeto de RespuestaPreguntaUsuario a partir de los datos de un DataRow.
        /// </summary>
        /// <param name="row">El DataRow que contiene la información de RespuestaPreguntaUsuario</param>
        /// <returns>Un objeto de RespuestaPreguntaUsuario creado a partir de los datos</returns>
        public RespuestaPreguntaUsuario DataRowToRespuestaPreguntaUsuario(DataRow row)
        {
            RespuestaPreguntaUsuario respuestaPreguntaUsuario = new RespuestaPreguntaUsuario();
            respuestaPreguntaUsuario.Pregunta = new Pregunta();
            if (row.IsNull("RespuestaPreguntaUsuarioID"))
                respuestaPreguntaUsuario.RespuestaPreguntaUsuarioID = null;
            else
                respuestaPreguntaUsuario.RespuestaPreguntaUsuarioID = (Guid)Convert.ChangeType(row["RespuestaPreguntaUsuarioID"], typeof(Guid));
            if (row.IsNull("PreguntaID"))
                respuestaPreguntaUsuario.Pregunta.PreguntaID = null;
            else
                respuestaPreguntaUsuario.Pregunta.PreguntaID = (int)Convert.ChangeType(row["PreguntaID"], typeof(int));
            return respuestaPreguntaUsuario;
        }
    }
}
