using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.ReactivosUsuario.BO;
using POV.Reactivos.BO;
using POV.Reactivos.Service;
using GP.SocialEngine.BO;
using GP.SocialEngine.Service;
using POV.ReactivosUsuario.DAO;

namespace POV.ReactivosUsuario.Service
{
    /// <summary>
    /// Controlador del objeto RespuestaReactivoUsuario
    /// </summary>
    public class RespuestaReactivoUsuarioCtrl
    {
        /// <summary>
        /// Consulta registros de RespuestaReactivoUsuarioRetHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="respuestaReactivoUsuarioRetHlp">RespuestaReactivoUsuarioRetHlp que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de RespuestaReactivoUsuarioRetHlp generada por la consulta</returns>
        public DataSet Retrieve(IDataContext dctx, RespuestaReactivoUsuario respuestaReactivoUsuario)
        {
            RespuestaReactivoUsuarioRetHlp da = new RespuestaReactivoUsuarioRetHlp();
            DataSet ds = da.Action(dctx, respuestaReactivoUsuario);
            return ds;
        }

        /// <summary>
        /// Devuelve un registro completo de RespuestaReactivoUsuario
        /// </summary>
        /// <param name="dctx"></param>
        /// <param name="respuestaReactivoUsuario"></param>
        /// <returns></returns>
        public RespuestaReactivoUsuario RetrieveComplete(IDataContext dctx, RespuestaReactivoUsuario respuestaReactivoUsuario)
        {
            if (respuestaReactivoUsuario == null)
                throw new Exception("La respuesta reactivo no puede ser nulo");
            if (respuestaReactivoUsuario.RespuestaReactivoUsuarioID == null)
                throw new Exception("El identificador de la respuesta reactivo no puede ser nulo");

            DataSet ds = Retrieve(dctx, new RespuestaReactivoUsuario { RespuestaReactivoUsuarioID = respuestaReactivoUsuario.RespuestaReactivoUsuarioID });

            RespuestaReactivoUsuario respuesta = null;
            if (ds.Tables[0].Rows.Count > 0)
            {
                respuesta = LastDataRowToRespuestaReactivoUsuario(ds);

                ReactivoCtrl reactivoCtrl = new ReactivoCtrl();
                respuesta.Reactivo = reactivoCtrl.RetrieveComplete(dctx, new Reactivo { ReactivoID = respuesta.Reactivo.ReactivoID, TipoReactivo = ETipoReactivo.Estandarizado});

                UsuarioSocialCtrl usuarioSocialCtrl = new UsuarioSocialCtrl();
                respuesta.UsuarioSocial = usuarioSocialCtrl.LastDataRowToUsuarioSocial(usuarioSocialCtrl.Retrieve(dctx, new UsuarioSocial { UsuarioSocialID = respuesta.UsuarioSocial.UsuarioSocialID }));

                RespuestaPreguntaUsuarioCtrl respuestaPreguntaUsuarioCtrl = new RespuestaPreguntaUsuarioCtrl();

                respuesta.ListaRespuestaPreguntaUsuario = respuestaPreguntaUsuarioCtrl.RetrieveCompleteListaRespuestaPregunta(dctx, respuesta);

            }

            return respuesta;

        }
        /// <summary>
        /// Crea un registro de RespuestaReactivoUsuarioInsHlp en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="respuestaReactivoUsuarioInsHlp">RespuestaReactivoUsuarioInsHlp que desea crear</param>
        public void Insert(IDataContext dctx, RespuestaReactivoUsuario respuestaReactivoUsuario)
        {
            RespuestaReactivoUsuarioInsHlp da = new RespuestaReactivoUsuarioInsHlp();
            da.Action(dctx, respuestaReactivoUsuario);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dctx"></param>
        /// <param name="respuestaReactivo"></param>
        public void InsertComplete(IDataContext dctx, RespuestaReactivoUsuario respuestaReactivo)
        {
            if (respuestaReactivo == null)
                throw new Exception("La respuesta reactivo no puede ser nulo");
            if (respuestaReactivo.RespuestaReactivoUsuarioID == null)
                throw new Exception("El identificador de la respuesta reactivo no puede ser nulo");
            if (respuestaReactivo.ListaRespuestaPreguntaUsuario == null)
                throw new Exception("La lista de respuesta pregunta del reactivo no puede ser nulo");
            if (respuestaReactivo.ListaRespuestaPreguntaUsuario.Count <= 0)
                throw new Exception("La lista de respuesta pregunta del reactivo no puede ser vacia");

            object myFirm = new object();
            try
            {
                dctx.OpenConnection(myFirm);
                dctx.BeginTransaction(myFirm);

                Insert(dctx, respuestaReactivo);

                RespuestaPreguntaUsuarioCtrl respuestaPreguntaCtrl = new RespuestaPreguntaUsuarioCtrl();
                RespuestaUsuarioCtrl respuestaUsuarioCtrl = new RespuestaUsuarioCtrl();

                foreach (RespuestaPreguntaUsuario respuestaPregunta in respuestaReactivo.ListaRespuestaPreguntaUsuario)
                {
                    respuestaPreguntaCtrl.Insert(dctx,respuestaReactivo, respuestaPregunta);
                    respuestaUsuarioCtrl.Insert(dctx, respuestaPregunta, respuestaPregunta.RespuestaUsuario);
                }
            }
            catch (Exception ex)
            {
                dctx.RollbackTransaction(myFirm);
                dctx.CloseConnection(myFirm);
                throw ex;
            }
            finally
            {
                dctx.CommitTransaction(myFirm);
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(myFirm);
            }
        }


        /// <summary>
        /// Califica un reactivo con preguntas de opcion multiple
        /// </summary>
        /// <param name="dctx"></param>
        /// <param name="respuestaReactivo"></param>
        /// <returns> valor 0 si tuvo al menos una incorrecta, 1 si tuvo todas las respuestas correctas</returns>
        public decimal CalificarReactivo(IDataContext dctx, RespuestaReactivoUsuario respuestaReactivo)
        {
            decimal calificacion = 0;

            RespuestaReactivoUsuario respuesta = RetrieveComplete(dctx, respuestaReactivo);
            PreguntaCtrl preguntaCtrl = new PreguntaCtrl();
            bool esCorrecto = true;
            foreach (RespuestaPreguntaUsuario respuestaPregunta in respuesta.ListaRespuestaPreguntaUsuario)
            {

                Pregunta pregunta = preguntaCtrl.LastDataRowToPregunta(preguntaCtrl.Retrieve(
                    dctx,
                    new Pregunta
                    {
                        PreguntaID = respuestaPregunta.Pregunta.PreguntaID
                    },
                    new Reactivo
                    {
                        ReactivoID = respuesta.Reactivo.ReactivoID,
                        TipoReactivo = ETipoReactivo.Estandarizado
                    }));
                // si la pregunta esta activa la calificamos
                if ((bool)pregunta.Activo)
                    if (respuestaPregunta.RespuestaUsuario.TipoRespuestaUsuario == ETipoRespuestaUsuario.OPCION_MULTIPLE)
                    {
                        RespuestaUsuarioOpcionMultiple respuestaUsuario = (RespuestaUsuarioOpcionMultiple)respuestaPregunta.RespuestaUsuario;

                        if (!(bool)respuestaUsuario.OpcionRespuestaPlantilla.EsOpcionCorrecta)
                        {
                            esCorrecto = false;
                            break;
                        }
                    }
            }

            if (esCorrecto)
                calificacion = 1;

            return calificacion;
        }
        /// <summary>
        /// Actualiza de manera optimista un registro de RespuestaReactivoUsuarioUpdHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="respuestaReactivoUsuarioUpdHlp">RespuestaReactivoUsuarioUpdHlp que tiene los datos nuevos</param>
        /// <param name="anterior">RespuestaReactivoUsuarioUpdHlp que tiene los datos anteriores</param>
        public void Update(IDataContext dctx, RespuestaReactivoUsuario respuestaReactivoUsuario, RespuestaReactivoUsuario previous)
        {
            RespuestaReactivoUsuarioUpdHlp da = new RespuestaReactivoUsuarioUpdHlp();
            da.Action(dctx, respuestaReactivoUsuario, previous);
        }



        /// <summary>
        /// Crea un objeto de RespuestaReactivoUsuario a partir de los datos del Ãºltimo DataRow del DataSet.
        /// </summary>
        /// <param name="ds">El DataSet que contiene la informaciÃ³n de RespuestaReactivoUsuario</param>
        /// <returns>Un objeto de RespuestaReactivoUsuario creado a partir de los datos</returns>
        public RespuestaReactivoUsuario LastDataRowToRespuestaReactivoUsuario(DataSet ds)
        {
            if (!ds.Tables.Contains("RespuestaReactivoUsuario"))
                throw new Exception("LastDataRowToRespuestaReactivoUsuario: DataSet no tiene la tabla RespuestaReactivoUsuario");
            int index = ds.Tables["RespuestaReactivoUsuario"].Rows.Count;
            if (index < 1)
                throw new Exception("LastDataRowToRespuestaReactivoUsuario: El DataSet no tiene filas");
            return this.DataRowToRespuestaReactivoUsuario(ds.Tables["RespuestaReactivoUsuario"].Rows[index - 1]);
        }
        /// <summary>
        /// Crea un objeto de RespuestaReactivoUsuario a partir de los datos de un DataRow.
        /// </summary>
        /// <param name="row">El DataRow que contiene la informaciÃ³n de RespuestaReactivoUsuario</param>
        /// <returns>Un objeto de RespuestaReactivoUsuario creado a partir de los datos</returns>
        public RespuestaReactivoUsuario DataRowToRespuestaReactivoUsuario(DataRow row)
        {
            RespuestaReactivoUsuario respuestaReactivoUsuario = new RespuestaReactivoUsuario();
            respuestaReactivoUsuario.UsuarioSocial = new UsuarioSocial();
            respuestaReactivoUsuario.Reactivo = new Reactivo();
            respuestaReactivoUsuario.ListaRespuestaPreguntaUsuario = new List<RespuestaPreguntaUsuario>();
            if (row.IsNull("RespuestaReactivoUsuarioID"))
                respuestaReactivoUsuario.RespuestaReactivoUsuarioID = null;
            else
                respuestaReactivoUsuario.RespuestaReactivoUsuarioID = (Guid)Convert.ChangeType(row["RespuestaReactivoUsuarioID"], typeof(Guid));
            if (row.IsNull("ReactivoID"))
                respuestaReactivoUsuario.Reactivo.ReactivoID = null;
            else
                respuestaReactivoUsuario.Reactivo.ReactivoID = (Guid)Convert.ChangeType(row["ReactivoID"], typeof(Guid));
            if (row.IsNull("UsuarioSocialID"))
                respuestaReactivoUsuario.UsuarioSocial.UsuarioSocialID = null;
            else
                respuestaReactivoUsuario.UsuarioSocial.UsuarioSocialID = (long)Convert.ChangeType(row["UsuarioSocialID"], typeof(long));
            if (row.IsNull("FechaRegistro"))
                respuestaReactivoUsuario.FechaRegistro = null;
            else
                respuestaReactivoUsuario.FechaRegistro = (DateTime)Convert.ChangeType(row["FechaRegistro"], typeof(DateTime));
            if (row.IsNull("UltimaActualizacion"))
                respuestaReactivoUsuario.UltimaActualizacion = null;
            else
                respuestaReactivoUsuario.UltimaActualizacion = (DateTime)Convert.ChangeType(row["UltimaActualizacion"], typeof(DateTime));
            if (row.IsNull("PrimeraCalificacion"))
                respuestaReactivoUsuario.PrimeraCalificacion = null;
            else
                respuestaReactivoUsuario.PrimeraCalificacion = (decimal)Convert.ChangeType(row["PrimeraCalificacion"], typeof(decimal));
            if (row.IsNull("UltimaCalificacion"))
                respuestaReactivoUsuario.UltimaCalificacion = null;
            else
                respuestaReactivoUsuario.UltimaCalificacion = (decimal)Convert.ChangeType(row["UltimaCalificacion"], typeof(decimal));
            if (row.IsNull("NumeroIntentos"))
                respuestaReactivoUsuario.NumeroIntentos = null;
            else
                respuestaReactivoUsuario.NumeroIntentos = (int)Convert.ChangeType(row["NumeroIntentos"], typeof(int));
            if (row.IsNull("EstadoReactivoUsuario"))
                respuestaReactivoUsuario.EstadoReactivoUsuario = null;
            else
                respuestaReactivoUsuario.EstadoReactivoUsuario = (EEstadoReactivoUsuario)Convert.ToByte(row["EstadoReactivoUsuario"]);
            return respuestaReactivoUsuario;
        }
    }
}
