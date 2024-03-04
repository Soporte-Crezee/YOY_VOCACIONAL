using Framework.Base.DataAccess;
using POV.Prueba.Diagnostico.Dinamica.DA;
using POV.Reactivos.BO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace POV.Prueba.Diagnostico.Dinamica.Service
{
    public class CalculoResultadoRavenCtrl
    {
        /// <summary>
        /// Obtiene data set con los resultados de la tabla de baremo segun la edad
        /// </summary>
        /// <param name="dctx"> Proveedor de conexion a base de datos </param>
        /// <param name="edad"> Edad que porvee el criterio de seleccion para la busqueda </param>
        /// <returns> Dataset que contiene la informacion de tabla de baremo </returns>
        public DataSet RetrieveTablaBaremo(IDataContext dctx, string edad, long puntajeNormalizado) 
        {
            BaremoRavenDARetHlp da = new BaremoRavenDARetHlp();
            DataSet ds = da.Action(dctx, edad, puntajeNormalizado);
            return ds;
        }

        /// <summary>
        /// Obtiene data set con los resultados de la tabla de discrepacia segun el puntaje
        /// </summary>
        /// <param name="dctx"> Proveedor de conexion a base de datos </param>
        /// <param name="puntajeTotal"> Puntaje que porvee el criterio de seleccion para la busqueda </param>
        /// <returns> Dataset que contiene la informacion de tabla de discrepancia </returns>
        public DataSet RetrieveDiscrepanciaRaven(IDataContext dctx, int puntajeTotal) 
        {
            DiscrepanciaRavenDARetHlp da = new DiscrepanciaRavenDARetHlp();
            DataSet ds = da.Action(dctx, puntajeTotal);
            return ds;
        }

        /// <summary>
        /// Crea un objeto de ResultadoBaremoRaven a partir de los datos del último DataRow del DataSet.
        /// </summary>
        /// <param name="ds">El DataSet que contiene la información de ResultadoBaremoRaven</param>
        /// <returns>Un objeto de ResultadoBaremoRaven creado a partir de los datos</returns>
        public ResultadoBaremoRaven LastDatRowResultadoBaremo(DataSet ds) 
        {
            if (!ds.Tables.Contains("TablaBaremo"))
                throw new Exception("LastDatRowResultadoBaremo: DataSet no tiene la tabla TablaBaremo");
            int index = ds.Tables["TablaBaremo"].Rows.Count;
            if (index < 1)
                throw new Exception("LastDataRowToDirector: El DataSet no tiene filas");
            return this.DataRowToResultadoBaremoRaven(ds.Tables["TablaBaremo"].Rows[index - 1]);
        }

        /// <summary>
        /// Crea un objeto de ResultadoBaremoRaven a partir de los datos de un DataRow.
        /// </summary>
        /// <param name="row">El DataRow que contiene la información de ResultadoBaremoRaven</param>
        /// <returns>Un objeto de ResultadoBaremoRaven creado a partir de los datos</returns>
        public ResultadoBaremoRaven DataRowToResultadoBaremoRaven(DataRow row)
        {
            ResultadoBaremoRaven resultadoBaremoRaven = new ResultadoBaremoRaven();

            if (row.IsNull("BaremoID"))
                resultadoBaremoRaven.BaremoID = 0;
            else
                resultadoBaremoRaven.BaremoID = (long)Convert.ChangeType(row["BaremoID"], typeof(long));

            if (row.IsNull("Edad"))
                resultadoBaremoRaven.Edad = null;
            else
                resultadoBaremoRaven.Edad = (string)Convert.ChangeType(row["Edad"], typeof(string));

            if (row.IsNull("Puntaje"))
                resultadoBaremoRaven.Puntaje = 0;
            else
                resultadoBaremoRaven.Puntaje = (long)Convert.ChangeType(row["Puntaje"], typeof(long));

            if (row.IsNull("Percentil"))
                resultadoBaremoRaven.Percentil = 0;
            else
                resultadoBaremoRaven.Percentil = (long)Convert.ChangeType(row["Percentil"], typeof(long));

            return resultadoBaremoRaven;
        }
    }
}
