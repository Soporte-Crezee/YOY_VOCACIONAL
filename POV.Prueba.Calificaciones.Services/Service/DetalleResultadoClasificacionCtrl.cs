using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using POV.Prueba.Calificaciones.BO;
using POV.Prueba.Calificaciones.DAO;
using Framework.Base.DataAccess;
using POV.Prueba.Diagnostico.Dinamica.BO;

namespace POV.Prueba.Calificaciones.Services
{
    public class DetalleResultadoClasificacionCtrl
    {
        /// <summary>
        /// Crea un registro de DetalleResultadoClasificacion en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="resultadoMetodoClasificacion">El ResultadoMetodoClasificacion al que pertenece el registro que se inserta</param>
        /// <param name="detalleResultadoClasificacion">El DetalleResultadoClasificacion que se inserta</param>
        public void Insert(IDataContext dctx, ResultadoMetodoClasificacion resultadoMetodoClasificacion, DetalleResultadoClasificacion detalleResultadoClasificacion)
        {
            DetalleResultadoClasificacionInsHlp da = new DetalleResultadoClasificacionInsHlp();
            da.Action(dctx, resultadoMetodoClasificacion, detalleResultadoClasificacion);
        }
        /// <summary>
        /// Consulta registros de DetalleResultadoClasificacion en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="resultadoMetodoClasificacion">ResultadoMetodoClasificacion que provee el criterio de selección para la consulta</param>
        /// <param name="detalleResultadoClasificacion">DetalleResultadoClasificacion que provee el criterio de selección para la consulta</param>
        /// <returns>DataSet que contiene la información de DetalleResultadoClasificacion recuperada</returns>
        public DataSet Retrieve(IDataContext dctx, ResultadoMetodoClasificacion resultadoMetodoClasificacion, DetalleResultadoClasificacion detalleResultadoClasificacion)
        {
            DetalleResultadoClasificacionRetHlp da = new DetalleResultadoClasificacionRetHlp();
            return da.Action(dctx, resultadoMetodoClasificacion, detalleResultadoClasificacion);
        }
        /// <summary>
        /// Crea un objeto de DetalleResultadoClasificacion a partir de los datos del último DataRow del DataSet.
        /// </summary>
        /// <param name="ds">El DataSet que contiene la información de DetalleResultadoClasificacion</param>
        /// <returns>Un objeto de DetalleResultadoClasificacion creado a partir de los datos</returns>
        public DetalleResultadoClasificacion LastDataRowToDetalleResultadoClasificacion(DataSet ds)
        {
            if (!ds.Tables.Contains("DetalleResultadoClasificacion"))
                throw new Exception("LastDataRowToDetalleResultadoClasificacion: DataSet no tiene la tabla DetalleResultadoClasificacion");
            int index = ds.Tables["DetalleResultadoClasificacion"].Rows.Count;
            if (index < 1)
                throw new Exception("LastDataRowToDetalleResultadoClasificacion: El DataSet no tiene filas");
            return this.DataRowToDetalleResultadoClasificacion(ds.Tables["DetalleResultadoClasificacion"].Rows[index - 1]);
        }
        /// <summary>
        /// Crea un objeto de DetalleResultadoClasificacion a partir de los datos de un DataRow.
        /// </summary>
        /// <param name="row">El DataRow que contiene la información de DetalleResultadoClasificacion</param>
        /// <returns>Un objeto de DetalleResultadoClasificacion creado a partir de los datos</returns>
        public DetalleResultadoClasificacion DataRowToDetalleResultadoClasificacion(DataRow row)
        {
            DetalleResultadoClasificacion detalleResultadoClasificacion = new DetalleResultadoClasificacion();
            detalleResultadoClasificacion.EscalaClasificacionDinamica = new EscalaClasificacionDinamica();
            if (!row.Table.Columns.Contains("DetalleResultadoID") || row.IsNull("DetalleResultadoID"))
                detalleResultadoClasificacion.DetalleResultadoID = null;
            else
                detalleResultadoClasificacion.DetalleResultadoID = (int)Convert.ChangeType(row["DetalleResultadoID"], typeof(int));

            if (!row.Table.Columns.Contains("Valor") || row.IsNull("Valor"))
                detalleResultadoClasificacion.Valor = null;
            else
                detalleResultadoClasificacion.Valor = (Decimal)Convert.ChangeType(row["Valor"], typeof(Decimal));

            if (!row.Table.Columns.Contains("EsAproximado") || row.IsNull("EsAproximado"))
                detalleResultadoClasificacion.EsAproximado = null;
            else
                detalleResultadoClasificacion.EsAproximado = (bool)Convert.ChangeType(row["EsAproximado"], typeof(bool));

            if (!row.Table.Columns.Contains("PuntajeID") || row.IsNull("PuntajeID"))
                detalleResultadoClasificacion.EscalaClasificacionDinamica.PuntajeID = null;
            else
                detalleResultadoClasificacion.EscalaClasificacionDinamica.PuntajeID = (int)Convert.ChangeType(row["PuntajeID"], typeof(int));

            return detalleResultadoClasificacion;
        }

    }
}
