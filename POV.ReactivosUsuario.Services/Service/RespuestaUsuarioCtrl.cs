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
using POV.ReactivosUsuario.DAO;

namespace POV.ReactivosUsuario.Service
{
    /// <summary>
    /// Controlador del objeto RespuestaUsuario
    /// </summary>
    public class RespuestaUsuarioCtrl
    {
        /// <summary>
        /// Consulta registros de RespuestaUsuarioRetHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="respuestaUsuarioRetHlp">RespuestaUsuarioRetHlp que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de RespuestaUsuarioRetHlp generada por la consulta</returns>
        public DataSet Retrieve(IDataContext dctx, RespuestaPreguntaUsuario respuestaPreguntaUsuario, RespuestaUsuario respuestaUsuario)
        {
            RespuestaUsuarioRetHlp da = new RespuestaUsuarioRetHlp();
            DataSet ds = da.Action(dctx, respuestaPreguntaUsuario, respuestaUsuario);
            return ds;
        }

        /// <summary>
        /// Devuelve una respuesta de usuario a partir de una respuesta pregunta usuario
        /// </summary>
        /// <param name="dctx"></param>
        /// <param name="respuestaPreguntaUsuario"></param>
        /// <returns></returns>
        public RespuestaUsuario RetrieveRespuestaUsuario(IDataContext dctx, RespuestaPreguntaUsuario respuestaPreguntaUsuario)
        {
            RespuestaUsuario respuesta = null;

            DataSet ds = Retrieve(dctx, respuestaPreguntaUsuario, null);

            if (ds.Tables[0].Rows.Count > 0)
            {
                int index = ds.Tables[0].Rows.Count;
                DataRow dr = ds.Tables["RespuestaUsuario"].Rows[index - 1];
                short tipo = (short)Convert.ChangeType(dr["TipoRespuestaUsuario"], typeof(short));
                if (tipo == (short)ETipoRespuestaUsuario.OPCION_MULTIPLE)
                {
                    respuesta = DataRowToRespuestaUsuarioOpcionMultiple(dr);// RetrieveCompleteRespuestaUsuarioOpcionMultiple(dctx, respuestaPreguntaUsuario, DataRowToRespuestaUsuarioOpcionMultiple(dr));

                    OpcionRespuestaPlantillaCtrl opcionCtrl = new OpcionRespuestaPlantillaCtrl();
                    (respuesta as RespuestaUsuarioOpcionMultiple).OpcionRespuestaPlantilla = opcionCtrl.LastDataRowToOpcionRespuestaPlantilla(opcionCtrl.Retrieve(dctx, (respuesta as RespuestaUsuarioOpcionMultiple).OpcionRespuestaPlantilla, null, ETipoReactivo.Estandarizado), ETipoReactivo.Estandarizado);


                }
                else if (tipo == (short)ETipoRespuestaUsuario.ABIERTA)
                    respuesta = DataRowToRespuestaUsuarioAbierta(dr);
            }

            return respuesta;
        }

        public RespuestaUsuarioOpcionMultiple RetrieveCompleteRespuestaUsuarioOpcionMultiple(IDataContext dctx, RespuestaPreguntaUsuario respuestaPreguntaUsuario, RespuestaUsuarioOpcionMultiple respuestaUsuario)
        {
            if (respuestaUsuario.TipoRespuestaUsuario == null)
                throw new Exception("El tipo de respuesta es requerido");

            RespuestaUsuarioOpcionMultiple respuesta = null;

            DataSet ds = Retrieve(dctx, respuestaPreguntaUsuario, respuestaUsuario);

            if (ds.Tables[0].Rows.Count > 0)
            {
                respuesta = this.LastDataRowToRespuestaUsuarioOpcionMultiple(ds);

                OpcionRespuestaPlantillaCtrl opcionCtrl = new OpcionRespuestaPlantillaCtrl();
                respuesta.OpcionRespuestaPlantilla = opcionCtrl.LastDataRowToOpcionRespuestaPlantilla(opcionCtrl.Retrieve(dctx, respuestaUsuario.OpcionRespuestaPlantilla, null, ETipoReactivo.Estandarizado), ETipoReactivo.Estandarizado);

            }
            return respuesta;
        }


        public RespuestaUsuarioAbierta RetrieveCompleteRespuestaUsuarioAbierta(IDataContext dctx, RespuestaPreguntaUsuario respuestaPreguntaUsuario, RespuestaUsuarioAbierta respuestaUsuario)
        {
            if (respuestaUsuario.TipoRespuestaUsuario == null)
                throw new Exception("El tipo de respuesta es requerido");

            RespuestaUsuarioAbierta respuesta = null;

            DataSet ds = Retrieve(dctx, respuestaPreguntaUsuario, respuestaUsuario);

            if (ds.Tables[0].Rows.Count > 0)
            {
                respuesta = this.LastDataRowToRespuestaUsuarioAbierta(ds);

            }
            return respuesta;
        }

        /// <summary>
        /// Crea un registro de RespuestaUsuarioInsHlp en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="respuestaUsuarioInsHlp">RespuestaUsuarioInsHlp que desea crear</param>
        public void Insert(IDataContext dctx, RespuestaPreguntaUsuario respuestaPreguntaUsuario, RespuestaUsuario respuestaUsuario)
        {
            if (respuestaPreguntaUsuario == null)
                throw new Exception("RespuestaPreguntaUsuario es requerido");
            if (respuestaPreguntaUsuario.RespuestaPreguntaUsuarioID == null)
                throw new Exception("RespuestaPreguntaUsuario.RespuestaPreguntaUsuarioID es requerido");
            if (respuestaUsuario.TipoRespuestaUsuario == null)
                throw new Exception("TipoRespuestaUsuario es requerido");
            object myFirm = new object();
            try
            {
                dctx.OpenConnection(myFirm);
                dctx.BeginTransaction(myFirm);

                switch (respuestaUsuario.TipoRespuestaUsuario)
                {
                    case ETipoRespuestaUsuario.OPCION_MULTIPLE:
                        RespuestaUsuarioOpcionMultiple respuestaUsuarioOpcionMultiple = (RespuestaUsuarioOpcionMultiple)respuestaUsuario;
                        
                        RespuestaUsuarioOpcionMultipleInsHlp rpomDa = new RespuestaUsuarioOpcionMultipleInsHlp();
                        rpomDa.Action(dctx, respuestaPreguntaUsuario, respuestaUsuarioOpcionMultiple);

                        break;
                    case ETipoRespuestaUsuario.ABIERTA:
                        RespuestaUsuarioAbierta respuestaUsuarioAbierta = (RespuestaUsuarioAbierta)respuestaUsuario;
                        
                        RespuestaUsuarioAbiertaInsHlp rpaDa = new RespuestaUsuarioAbiertaInsHlp();
                        rpaDa.Action(dctx, respuestaPreguntaUsuario, respuestaUsuarioAbierta);
                        break;
                    default:
                        throw new Exception("InsertRespuestaUsuario: El tipo de respuestaUsuario no esta definido.");
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
        /// Actualiza de manera optimista un registro de RespuestaUsuarioOpcionMultipleUpdHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="respuestaUsuarioOpcionMultipleUpdHlp">RespuestaUsuarioOpcionMultipleUpdHlp que tiene los datos nuevos</param>
        /// <param name="anterior">RespuestaUsuarioOpcionMultipleUpdHlp que tiene los datos anteriores</param>
        public void UpdateOpcionMultiple(IDataContext dctx, RespuestaPreguntaUsuario respuestaPreguntaUsuario, RespuestaUsuarioOpcionMultiple respuestaUsuario, RespuestaUsuarioOpcionMultiple previous)
        {
            RespuestaUsuarioOpcionMultipleUpdHlp da = new RespuestaUsuarioOpcionMultipleUpdHlp();
            da.Action(dctx, respuestaPreguntaUsuario, respuestaUsuario, previous);
        }

        /// <summary>
        /// Crea un objeto de RespuestaUsuarioOpcionMultiple a partir de los datos del Ãºltimo DataRow del DataSet.
        /// </summary>
        /// <param name="ds">El DataSet que contiene la informaciÃ³n de RespuestaUsuarioOpcionMultiple</param>
        /// <returns>Un objeto de RespuestaUsuarioOpcionMultiple creado a partir de los datos</returns>
        public RespuestaUsuarioOpcionMultiple LastDataRowToRespuestaUsuarioOpcionMultiple(DataSet ds)
        {
            if (!ds.Tables.Contains("RespuestaUsuario"))
                throw new Exception("LastDataRowToRespuestaUsuarioOpcionMultiple: DataSet no tiene la tabla RespuestaUsuarioOpcionMultiple");
            int index = ds.Tables["RespuestaUsuario"].Rows.Count;
            if (index < 1)
                throw new Exception("LastDataRowToRespuestaUsuarioOpcionMultiple: El DataSet no tiene filas");
            return this.DataRowToRespuestaUsuarioOpcionMultiple(ds.Tables["RespuestaUsuario"].Rows[index - 1]);
        }
        /// <summary>
        /// Crea un objeto de RespuestaUsuarioOpcionMultiple a partir de los datos de un DataRow.
        /// </summary>
        /// <param name="row">El DataRow que contiene la informaciÃ³n de RespuestaUsuarioOpcionMultiple</param>
        /// <returns>Un objeto de RespuestaUsuarioOpcionMultiple creado a partir de los datos</returns>
        public RespuestaUsuarioOpcionMultiple DataRowToRespuestaUsuarioOpcionMultiple(DataRow row)
        {
            RespuestaUsuarioOpcionMultiple respuestaUsuarioOpcionMultiple = new RespuestaUsuarioOpcionMultiple();
            respuestaUsuarioOpcionMultiple.OpcionRespuestaPlantilla = new OpcionRespuestaPlantilla();
            if (row.IsNull("FechaRegistro"))
                respuestaUsuarioOpcionMultiple.FechaRegistro = null;
            else
                respuestaUsuarioOpcionMultiple.FechaRegistro = (DateTime)Convert.ChangeType(row["FechaRegistro"], typeof(DateTime));
            if (row.IsNull("TipoRespuestaUsuario"))
                respuestaUsuarioOpcionMultiple.ToShortTipoRespuestaUsuario = null;
            else
                respuestaUsuarioOpcionMultiple.ToShortTipoRespuestaUsuario = (short)Convert.ChangeType(row["TipoRespuestaUsuario"], typeof(short));
            if (row.IsNull("OpcionRespuestaPlantillaID"))
                respuestaUsuarioOpcionMultiple.OpcionRespuestaPlantilla.OpcionRespuestaPlantillaID = null;
            else
                respuestaUsuarioOpcionMultiple.OpcionRespuestaPlantilla.OpcionRespuestaPlantillaID = (int)Convert.ChangeType(row["OpcionRespuestaPlantillaID"], typeof(int));
            return respuestaUsuarioOpcionMultiple;
        }
        /// <summary>
        /// Crea un objeto de RespuestaUsuarioAbierta a partir de los datos del Ãºltimo DataRow del DataSet.
        /// </summary>
        /// <param name="ds">El DataSet que contiene la informaciÃ³n de RespuestaUsuarioAbierta</param>
        /// <returns>Un objeto de RespuestaUsuarioAbierta creado a partir de los datos</returns>
        public RespuestaUsuarioAbierta LastDataRowToRespuestaUsuarioAbierta(DataSet ds)
        {
            if (!ds.Tables.Contains("RespuestaUsuario"))
                throw new Exception("LastDataRowToRespuestaUsuarioAbierta: DataSet no tiene la tabla RespuestaUsuarioAbierta");
            int index = ds.Tables["RespuestaUsuario"].Rows.Count;
            if (index < 1)
                throw new Exception("LastDataRowToRespuestaUsuarioAbierta: El DataSet no tiene filas");
            return this.DataRowToRespuestaUsuarioAbierta(ds.Tables["RespuestaUsuario"].Rows[index - 1]);
        }
        /// <summary>
        /// Crea un objeto de RespuestaUsuarioAbierta a partir de los datos de un DataRow.
        /// </summary>
        /// <param name="row">El DataRow que contiene la informaciÃ³n de RespuestaUsuarioAbierta</param>
        /// <returns>Un objeto de RespuestaUsuarioAbierta creado a partir de los datos</returns>
        public RespuestaUsuarioAbierta DataRowToRespuestaUsuarioAbierta(DataRow row)
        {
            RespuestaUsuarioAbierta respuestaUsuarioAbierta = new RespuestaUsuarioAbierta();
            if (row.IsNull("FechaRegistro"))
                respuestaUsuarioAbierta.FechaRegistro = null;
            else
                respuestaUsuarioAbierta.FechaRegistro = (DateTime)Convert.ChangeType(row["FechaRegistro"], typeof(DateTime));
            if (row.IsNull("TipoRespuestaUsuario"))
                respuestaUsuarioAbierta.ToShortTipoRespuestaUsuario = null;
            else
                respuestaUsuarioAbierta.ToShortTipoRespuestaUsuario = (short)Convert.ChangeType(row["TipoRespuestaUsuario"], typeof(short));
            if (row.IsNull("TextoRespuesta"))
                respuestaUsuarioAbierta.TextoRespuesta = null;
            else
                respuestaUsuarioAbierta.TextoRespuesta = (string)Convert.ChangeType(row["TextoRespuesta"], typeof(string));
            return respuestaUsuarioAbierta;
        }
    }
}
