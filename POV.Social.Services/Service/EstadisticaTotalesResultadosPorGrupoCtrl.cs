using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Base.DataAccess;
using POV.Social.BO;
using System.Data;
using POV.Reactivos.BO;
using POV.Modelo.Garner.BO;
using POV.Modelo.Garner.Service;
namespace POV.Social.Service
{
    /// <summary>
    /// Controlador que crea el objeto EstadisticaTotalesResultadosPorGrupo para mostrarlos de manera gráfica.
    /// </summary>
    public class EstadisticaTotalesResultadosPorGrupoCtrl
    {
        /// <summary>
        /// Construye un objeto que contiene los niveles de proclividad del sistema junto con la 
        /// cantidad de alumnos del grupo que cayeron en alguna clasificación.
        /// </summary>
        /// <param name="listReporteResultadoDiganosticoPorGrupo"> lista de resultados diagnósticos 
        /// por grupo</param>
        /// <returns>regresa una lista con los resultados estadísticos.</returns>
        public List<EstadisticaTotalesReporteResultadosPorGrupo> ConstructObject(IDataContext dctx, List<ReporteResultadoDiagnosticoPorGrupo> listReporteResultadoDiganosticoPorGrupo, DataSet ds)
        {
            
            List<EstadisticaTotalesReporteResultadosPorGrupo> listEstadisticas = new List<EstadisticaTotalesReporteResultadosPorGrupo>();
            Dictionary<string, int> inteligenciasExistentes = new Dictionary<string, int>();

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                inteligenciasExistentes.Add(row["Nombre"].ToString(), 0);
            }
            inteligenciasExistentes.Add("Prueba Incompleta o Pendiente", 0);

            foreach (ReporteResultadoDiagnosticoPorGrupo reporteResultadoDiagnosticoPorGrupo in listReporteResultadoDiganosticoPorGrupo)
            {
                if (reporteResultadoDiagnosticoPorGrupo.InteligenciaPredominante != null)
                {
                    foreach (TipoInteligencia tipoInteligencia in reporteResultadoDiagnosticoPorGrupo.InteligenciaPredominante)
                    {
                        try
                        {
                            inteligenciasExistentes.Add(tipoInteligencia.Nombre, 1);
                        }
                        catch (Exception ex)
                        {
                            inteligenciasExistentes[tipoInteligencia.Nombre] += 1;
                        }
                    }                    
                }

                if (reporteResultadoDiagnosticoPorGrupo.ConResultado == false)
                    inteligenciasExistentes["Prueba Incompleta o Pendiente"] += 1;
            }


            foreach (KeyValuePair<string, int> reg in inteligenciasExistentes)
            {
                listEstadisticas.Add(new EstadisticaTotalesReporteResultadosPorGrupo { CifraEstadistica=reg.Value, DatoEstadistico = reg.Key });
            }
            return listEstadisticas;
        }
    }
}
