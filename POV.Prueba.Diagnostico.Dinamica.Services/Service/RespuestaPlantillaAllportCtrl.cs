using Framework.Base.DataAccess;
using POV.Modelo.BO;
using POV.Prueba.Diagnostico.Dinamica.DA;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace POV.Prueba.Diagnostico.Dinamica.Service
{
    /// <summary>
    /// Controlador del DataSet RespuestaPlantillaAllport
    /// </summary>
    public class RespuestaPlantillaAllportCtrl
    {
        /// <summary>
        /// Consulta los registros de RespuestaPlantillaAllport en la base de datos
        /// </summary>
        /// <param name="dctx"> El DataContext que proveerá acceso a la base de datos </param>
        /// <param name="clasificador"> clasificador que provee el criterio de selección para realizar la consulta </param>
        /// <returns> DataSet que contiene la informacion de RespuestaPlantillaAllport generada por la consulta </returns>
        public DataSet Retrieve(IDataContext dctx, Clasificador clasificador) 
        {
            RespuestaPlantillaAllportDARetHlp da = new RespuestaPlantillaAllportDARetHlp();
            DataSet ds = da.Action(dctx, clasificador);
            return ds;
        }
    }
}
