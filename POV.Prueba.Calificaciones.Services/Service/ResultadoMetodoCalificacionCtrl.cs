using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using POV.Prueba.Calificaciones.BO;
using POV.Prueba.Calificaciones.DAO;
using Framework.Base.DataAccess;
using POV.Prueba.Diagnostico.Dinamica.BO;

using Framework.Base.Exceptions;
using POV.Prueba.BO;

namespace POV.Prueba.Calificaciones.Services
{
    public class ResultadoMetodoCalificacionCtrl
    {
        /// <summary>
        /// Crea un registro de AResultadoMetodoCalificacion en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="resultadoMetodoCalificacion">AResultadoMetodoCalificacion que desea crear</param>
        public void Insert(IDataContext dctx, AResultadoMetodoCalificacion resultadoMetodoCalificacion)
        {
            if (resultadoMetodoCalificacion == null)
                throw new Exception("ResultadoMetodoCalificacionCtrl.Action: El ResultadoMetodoCalificacion es requerido");
            if (resultadoMetodoCalificacion.ResultadoPrueba == null || resultadoMetodoCalificacion.ResultadoPrueba.ResultadoPruebaID == null)
                throw new Exception("ResultadoMetodoCalificacionCtrl.Insert: El campo ResultadoPruebaID es requerido");

            if (this.ExisteResultadoMetodoCalificacion(dctx, resultadoMetodoCalificacion))
                throw new Exception("ResultadoMetodoCalificacionCtrl.Insert: Existe un registro previo de AResultadoMetodoCalificaion, imposible crear otro");

            ResultadoMetodoCalificacionInsHlp da = new ResultadoMetodoCalificacionInsHlp();
            resultadoMetodoCalificacion.FechaRegistro = System.DateTime.Now;
            da.Action(dctx, resultadoMetodoCalificacion);
        }

        private bool ExisteResultadoMetodoCalificacion(IDataContext dctx, AResultadoMetodoCalificacion resultadoMetodoCalificacion)
        {
            bool existeResultado = false;
            AResultadoMetodoCalificacion resultadoMetodoCalificacionTest = null;
            if (resultadoMetodoCalificacion is ResultadoMetodoPuntos)
            {
                resultadoMetodoCalificacionTest = new ResultadoMetodoPuntos();
                resultadoMetodoCalificacionTest.ResultadoPrueba = new ResultadoPruebaDinamica();
            }
            if (resultadoMetodoCalificacion is ResultadoMetodoPorcentaje)
            {
                resultadoMetodoCalificacionTest = new ResultadoMetodoPorcentaje();
                resultadoMetodoCalificacionTest.ResultadoPrueba = new ResultadoPruebaDinamica();
            }
            if (resultadoMetodoCalificacion is ResultadoMetodoClasificacion)
            {
                resultadoMetodoCalificacionTest = new ResultadoMetodoClasificacion();
                resultadoMetodoCalificacionTest.ResultadoPrueba = new ResultadoPruebaDinamica();
            }
            if (resultadoMetodoCalificacion is ResultadoMetodoSeleccion)
            {
                resultadoMetodoCalificacionTest = new ResultadoMetodoSeleccion();
                resultadoMetodoCalificacionTest.ResultadoPrueba = new ResultadoPruebaDinamica();
            }
            
            resultadoMetodoCalificacionTest.ResultadoPrueba.ResultadoPruebaID = resultadoMetodoCalificacion.ResultadoPrueba.ResultadoPruebaID;

            DataSet ds = this.Retrieve(dctx, resultadoMetodoCalificacionTest);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                existeResultado = true;

            return existeResultado;
        }
        /// <summary>
        /// Inserta en la base de datos un registro de AResultadoMetodoCalificación y sus objetos dependientes
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="resultadoMetodoCalificacion">AResultadoMetodoCalificacion completo que desea crear</param>
        public void InsertComplete(IDataContext dctx, AResultadoMetodoCalificacion resultadoMetodoCalificacion)
        {
            if (resultadoMetodoCalificacion == null)
                throw new Exception("InsertComplete: El objeto AResultadoMetodoCalificacion es requerido!");

            object myFirm = new object();
            try
            {
                dctx.OpenConnection(myFirm);
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "Error de conexión", "Hubo un error al conectarse a la base de datos \n "
                    + ex.Message, "POV.Prueba.Calificaciones.Services", "ResultadoMetodoCalificacionCtrl", "InsertComplete", null, ex.InnerException);
            }

            try
            {
                #region Proceso Principal
                try
                {
                    dctx.BeginTransaction(myFirm);

                    this.Insert(dctx, resultadoMetodoCalificacion);

                    AResultadoMetodoCalificacion resultadoMetodoCalificacionNuevo =
                        this.LastDataRowToResultadoMetodoCalificacion(this.Retrieve(dctx, resultadoMetodoCalificacion));
                    resultadoMetodoCalificacion.ResultadoMetodoCalificacionID = resultadoMetodoCalificacionNuevo.ResultadoMetodoCalificacionID;

                    if (resultadoMetodoCalificacion is ResultadoMetodoClasificacion)
                        this.InsertDetalleResultadoClasificacion(dctx, resultadoMetodoCalificacion);

                    if (resultadoMetodoCalificacion is ResultadoMetodoSeleccion)
                        this.InsertDetalleResultadoSeleccion(dctx, resultadoMetodoCalificacion);

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

        private void InsertDetalleResultadoSeleccion(IDataContext dctx, AResultadoMetodoCalificacion resultadoMetodoCalificacion)
        {
            if (!(resultadoMetodoCalificacion is ResultadoMetodoSeleccion))
                throw new StandardException(MessageType.Error, "Error de tipos", "Se esperaba un objeto ResultadoMetodoSeleccion",
                    "POV.Prueba.Calificaciones.Services", "ResultadoMetodoCalificacionCtrl", "InsertDetalleResultadoSeleccion()", null, null);

            if (((ResultadoMetodoSeleccion)resultadoMetodoCalificacion).ListaDetalleResultadoSeleccion.Count > 0)
            {
                DetalleResultadoSeleccionInsHlp da = new DetalleResultadoSeleccionInsHlp();
                foreach (DetalleResultadoSeleccion detalle in ((ResultadoMetodoSeleccion)resultadoMetodoCalificacion).ListaDetalleResultadoSeleccion)
                {
                    da.Action(dctx, resultadoMetodoCalificacion as ResultadoMetodoSeleccion, detalle);
                }
            }
        }

        private void InsertDetalleResultadoClasificacion(IDataContext dctx, AResultadoMetodoCalificacion resultadoMetodoCalificacion)
        {
            if (!(resultadoMetodoCalificacion is ResultadoMetodoClasificacion))
                throw new StandardException(MessageType.Error, "Error de tipos", "Se esperaba un objeto ResultadoMetodoClasificacion",
                    "POV.Prueba.Calificaciones.Services", "ResultadoMetodoCalificacionCtrl", "InsertDetalleResultadoClasificacion()", null, null);

            if (((ResultadoMetodoClasificacion)resultadoMetodoCalificacion).ListaDetalleResultadoClasificacion.Count > 0)
            {
                DetalleResultadoClasificacionInsHlp da = new DetalleResultadoClasificacionInsHlp();
                foreach (DetalleResultadoClasificacion detalle in ((ResultadoMetodoClasificacion)resultadoMetodoCalificacion).ListaDetalleResultadoClasificacion)
                {
                    da.Action(dctx, resultadoMetodoCalificacion as ResultadoMetodoClasificacion, detalle);
                }
            }
        }

        /// <summary>
        /// Consulta registros de AResultadoMetodoCalificacion en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="resultadoMetodoCalificacion">AResultadoMetodoCalificacion que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de AResultadoMetodoCalificacion</returns>
        public DataSet Retrieve(IDataContext dctx, AResultadoMetodoCalificacion resultadoMetodoCalificacion)
        {
            ResultadoMetodoCalificacionRetHlp da = new ResultadoMetodoCalificacionRetHlp();
            return da.Action(dctx, resultadoMetodoCalificacion);
        }
        /// <summary>
        /// Crea un objeto de AResultadoMetodoCalificacion a partir de los datos del último DataRow del DataSet.
        /// </summary>
        /// <param name="ds">El DataSet que contiene la información de AResultadoMetodoCalificacion</param>
        /// <returns>Un objeto de AResultadoMetodocalificacion creado a partir de los datos</returns>
        public AResultadoMetodoCalificacion LastDataRowToResultadoMetodoCalificacion(DataSet ds)
        {
            if (!ds.Tables.Contains("ResultadoMetodoCalificacion"))
                throw new Exception("LastDataRowToResultadoMetodoCalificacion: DataSet no tiene la tabla ResultadoMetodoCalificacion");
            int index = ds.Tables["ResultadoMetodoCalificacion"].Rows.Count;
            if (index < 1)
                throw new Exception("LastDataRowToResultadoMetodoCalificacion: El DataSet no tiene filas");
            return this.DataRowToResultadoMetodoCalificacion(ds.Tables["ResultadoMetodoCalificacion"].Rows[index - 1]);
        }
        /// <summary>
        /// Crea un objeto de AResultadoMetodoCalificacion a partir de los datos de un DataRow.
        /// </summary>
        /// <param name="row">El DataRow que contiene la información de ResultadoMetodoCalificacion</param>
        /// <returns>Un objeto ResultadoMetodoCalificacion creado a partir de los datos</returns>
        public AResultadoMetodoCalificacion DataRowToResultadoMetodoCalificacion(DataRow row)
        {
            AResultadoMetodoCalificacion resultadoMetodoCalificacion;

            ETipoResultadoMetodo tipoResultadoMetodo;
            if (!row.Table.Columns.Contains("TipoResultadoMetodo") || row.IsNull("TipoResultadoMetodo"))
                throw new Exception("DataRowToResultadoMetodoCalificacion:\n No se puede determinar el tipo del resultado, la consulta no tiene la columna TipoResultadoMetodo");
            tipoResultadoMetodo = (ETipoResultadoMetodo)Convert.ChangeType(row["TipoResultadoMetodo"], typeof(byte));

            switch (tipoResultadoMetodo)
            {
                case ETipoResultadoMetodo.PUNTOS:
                    resultadoMetodoCalificacion = new ResultadoMetodoPuntos();
                    resultadoMetodoCalificacion.ResultadoPrueba = new ResultadoPruebaDinamica();
                    break;
                case ETipoResultadoMetodo.PORCENTAJE:
                    resultadoMetodoCalificacion = new ResultadoMetodoPorcentaje();
                    resultadoMetodoCalificacion.ResultadoPrueba = new ResultadoPruebaDinamica();
                    break;
                case ETipoResultadoMetodo.CLASIFICACION:
                    resultadoMetodoCalificacion = new ResultadoMetodoClasificacion();
                    resultadoMetodoCalificacion.ResultadoPrueba = new ResultadoPruebaDinamica();
                    break;
                case ETipoResultadoMetodo.SELECCION:
                    resultadoMetodoCalificacion = new ResultadoMetodoSeleccion();
                    resultadoMetodoCalificacion.ResultadoPrueba = new ResultadoPruebaDinamica();
                    break;
                default:
                    throw new Exception("DataRowToResultadoMetodoCalificacion:\n El tipo del resultado es incorrecto");
            }

            if (!row.Table.Columns.Contains("ResultadoMetodoCalificacionID") || row.IsNull("ResultadoMetodoCalificacionID"))
                resultadoMetodoCalificacion.ResultadoMetodoCalificacionID = null;
            else
                resultadoMetodoCalificacion.ResultadoMetodoCalificacionID = (int)Convert.ChangeType(row["ResultadoMetodoCalificacionID"], typeof(int));

            if (!row.Table.Columns.Contains("ResultadoPruebaID") || row.IsNull("ResultadoPruebaID"))
                resultadoMetodoCalificacion.ResultadoPrueba.ResultadoPruebaID = null;
            else
                resultadoMetodoCalificacion.ResultadoPrueba.ResultadoPruebaID = (int)Convert.ChangeType(row["ResultadoPruebaID"], typeof(int));

            if (!row.Table.Columns.Contains("FechaRegistro") || row.IsNull("FechaRegistro"))
                resultadoMetodoCalificacion.FechaRegistro = null;
            else
                resultadoMetodoCalificacion.FechaRegistro = (DateTime)Convert.ChangeType(row["FechaRegistro"], typeof(DateTime));

            if (resultadoMetodoCalificacion is ResultadoMetodoPuntos)
            {
                ((ResultadoMetodoPuntos)resultadoMetodoCalificacion).EscalaPuntajeDinamica = new EscalaPuntajeDinamica();
                if (!row.Table.Columns.Contains("Puntos") || row.IsNull("Puntos"))
                    ((ResultadoMetodoPuntos)resultadoMetodoCalificacion).Puntos = null;
                else
                    ((ResultadoMetodoPuntos)resultadoMetodoCalificacion).Puntos = (Decimal)Convert.ChangeType(row["Puntos"], typeof(Decimal));

                if (!row.Table.Columns.Contains("MaximoPuntos") || row.IsNull("MaximoPuntos"))
                    ((ResultadoMetodoPuntos)resultadoMetodoCalificacion).MaximoPuntos = null;
                else
                    ((ResultadoMetodoPuntos)resultadoMetodoCalificacion).MaximoPuntos = (Decimal)Convert.ChangeType(row["MaximoPuntos"], typeof(Decimal));

                if (!row.Table.Columns.Contains("EsAproximado") || row.IsNull("EsAproximado"))
                    ((ResultadoMetodoPuntos)resultadoMetodoCalificacion).EsAproximado = null;
                else
                    ((ResultadoMetodoPuntos)resultadoMetodoCalificacion).EsAproximado = (bool)Convert.ChangeType(row["EsAproximado"], typeof(bool));

                if (!row.Table.Columns.Contains("PuntajeID") || row.IsNull("PuntajeID"))
                    ((ResultadoMetodoPuntos)resultadoMetodoCalificacion).EscalaPuntajeDinamica.PuntajeID = null;
                else
                    ((ResultadoMetodoPuntos)resultadoMetodoCalificacion).EscalaPuntajeDinamica.PuntajeID = (int)Convert.ChangeType(row["PuntajeID"], typeof(int));
            }

            if (resultadoMetodoCalificacion is ResultadoMetodoPorcentaje)
            {
                ((ResultadoMetodoPorcentaje)resultadoMetodoCalificacion).EscalaPorcentajeDinamica = new EscalaPorcentajeDinamica();
                if (!row.Table.Columns.Contains("NumeroAciertos") || row.IsNull("NumeroAciertos"))
                    ((ResultadoMetodoPorcentaje)resultadoMetodoCalificacion).NumeroAciertos = null;
                else
                    ((ResultadoMetodoPorcentaje)resultadoMetodoCalificacion).NumeroAciertos = (int)Convert.ChangeType(row["NumeroAciertos"], typeof(int));

                if (!row.Table.Columns.Contains("TotalAciertos") || row.IsNull("TotalAciertos"))
                    ((ResultadoMetodoPorcentaje)resultadoMetodoCalificacion).TotalAciertos = null;
                else
                    ((ResultadoMetodoPorcentaje)resultadoMetodoCalificacion).TotalAciertos = (int)Convert.ChangeType(row["TotalAciertos"], typeof(int));

                if (!row.Table.Columns.Contains("EsAproximado") || row.IsNull("EsAproximado"))
                    ((ResultadoMetodoPorcentaje)resultadoMetodoCalificacion).EsAproximado = null;
                else
                    ((ResultadoMetodoPorcentaje)resultadoMetodoCalificacion).EsAproximado = (bool)Convert.ChangeType(row["EsAproximado"], typeof(bool));

                if (!row.Table.Columns.Contains("PuntajeID") || row.IsNull("PuntajeID"))
                    ((ResultadoMetodoPorcentaje)resultadoMetodoCalificacion).EscalaPorcentajeDinamica.PuntajeID = null;
                else
                    ((ResultadoMetodoPorcentaje)resultadoMetodoCalificacion).EscalaPorcentajeDinamica.PuntajeID = (int)Convert.ChangeType(row["PuntajeID"], typeof(int));
            }

            return resultadoMetodoCalificacion;
        }

    }
}
