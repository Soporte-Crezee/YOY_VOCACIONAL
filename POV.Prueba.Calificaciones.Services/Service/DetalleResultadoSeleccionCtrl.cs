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
    public class DetalleResultadoSeleccionCtrl
    {
        /// <summary>
        /// Crea un registro de DetalleResultadoSeleccion en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="resultadoMetodoSeleccion">ResultadoMetodoSeleccion al que pertenece el registro que se inserta</param>
        /// <param name="detalleResultadoSeleccion">DetalleResultadoSeleccion que se inserta</param>
        public void Insert(IDataContext dctx, ResultadoMetodoSeleccion resultadoMetodoSeleccion, DetalleResultadoSeleccion detalleResultadoSeleccion)
        {
            DetalleResultadoSeleccionInsHlp da = new DetalleResultadoSeleccionInsHlp();
            da.Action(dctx, resultadoMetodoSeleccion, detalleResultadoSeleccion);
        }
        /// <summary>
        /// Consulta registros de DetalleResultadoSeleccion en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="resultadoMetodoSeleccion">ResultadoMetodoSeleccion que provee los parámetros para la consulta</param>
        /// <param name="detalleResultadoSeleccion">DetalleResultadoSeleccion que provee los parámetros para la consulta</param>
        /// <returns>DataSet que contiene la información de DetalleResultadoSeleccion recuperada</returns>
        public DataSet Retrieve(IDataContext dctx, ResultadoMetodoSeleccion resultadoMetodoSeleccion, DetalleResultadoSeleccion detalleResultadoSeleccion)
        {
            DetalleResultadoSeleccionRetHlp da = new DetalleResultadoSeleccionRetHlp();
            return da.Action(dctx, resultadoMetodoSeleccion, detalleResultadoSeleccion);
        }
        /// <summary>
        /// Crea un objeto de DetalleResultadoSeleccion a partir de los datos del último DataRow del DataSet.
        /// </summary>
        /// <param name="ds">El DataContext que proveerá acceso a la base de datos</param>
        /// <returns>Un objeto DetalleResultadoSeleccion a partir de los datos</returns>
        public DetalleResultadoSeleccion LastDataRowToDetalleResultadoSeleccion(DataSet ds)
        {
            if (!ds.Tables.Contains("DetalleResultadoSeleccion"))
                throw new Exception("LastDataRowToDetalleResultadoSeleccion: DataSet no tiene la tabla DetalleResultadoSeleccion");
            int index = ds.Tables["DetalleResultadoSeleccion"].Rows.Count;
            if (index < 1)
                throw new Exception("LastDataRowToDetalleResultadoSeleccion: El DataSet no tiene filas");
            return this.DataRowToDetalleResultadoSeleccion(ds.Tables["DetalleResultadoSeleccion"].Rows[index - 1]);
        }
        /// <summary>
        /// Crea un objeto de DetalleResultadoSeleccion a partir de los datos de un DataRow.
        /// </summary>
        /// <param name="row">El DataRow que contiene la información de DetalleResultadoSeleccion</param>
        /// <returns>Un objeto DetalleResultadoSeleccion a partir de los datos</returns>
        public DetalleResultadoSeleccion DataRowToDetalleResultadoSeleccion(DataRow row)
        {
            DetalleResultadoSeleccion detalleResultadoSeleccion = new DetalleResultadoSeleccion();
            detalleResultadoSeleccion.EscalaSeleccionDinamica = new EscalaSeleccionDinamica();
            if (!row.Table.Columns.Contains("DetalleResultadoID") || row.IsNull("DetalleResultadoID"))
                detalleResultadoSeleccion.DetalleResultadoID = null;
            else
                detalleResultadoSeleccion.DetalleResultadoID = (int)Convert.ChangeType(row["DetalleResultadoID"], typeof(int));

            if (!row.Table.Columns.Contains("Valor") || row.IsNull("Valor"))
                detalleResultadoSeleccion.Valor = null;
            else
                detalleResultadoSeleccion.Valor = (Decimal)Convert.ChangeType(row["Valor"], typeof(Decimal));

            if (!row.Table.Columns.Contains("EsAproximado") || row.IsNull("EsAproximado"))
                detalleResultadoSeleccion.EsAproximado = null;
            else
                detalleResultadoSeleccion.EsAproximado = (bool)Convert.ChangeType(row["EsAproximado"], typeof(bool));

            if (!row.Table.Columns.Contains("PuntajeID") || row.IsNull("PuntajeID"))
                detalleResultadoSeleccion.EscalaSeleccionDinamica.PuntajeID = null;
            else
                detalleResultadoSeleccion.EscalaSeleccionDinamica.PuntajeID = (int)Convert.ChangeType(row["PuntajeID"], typeof(int));

            return detalleResultadoSeleccion;
        }

    }
}
