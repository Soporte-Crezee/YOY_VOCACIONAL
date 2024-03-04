using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using Framework.Base.DataAccess;
using POV.ConfiguracionActividades.BO;
using POV.ConfiguracionActividades.Services;
using POV.Modelo.Context;
using POV.Modelo.Service;
using POV.Modelo.BO;

namespace POV.ServiciosActividades.Controllers
{
    /// <summary>
    /// Controlador de la interfaz de usuario Crear Actividad
    /// </summary>
    public class CrearActividadController
    {
        #region Atributos

        private readonly Contexto ctx;
        private object firma = new object();
        #endregion

        #region Constructores
		/// <summary>
		/// Constructor por defecto
		/// </summary>
        public CrearActividadController()
		{
			ctx = new Contexto(firma);
			
		}
        #endregion

        /// <summary>
        /// Metodo que consulta los tipos de aptitud activos en el sistema
        /// </summary>
        /// <returns>Lista de resultados de la consulta</returns>

        public List<BloqueActividad> ConsultarBloques(BloqueActividad filtro)
        {
            BloqueActividadCtrl bloqueCtrl = new BloqueActividadCtrl(ctx);
            return bloqueCtrl.Retrieve(filtro, false);
        }

        /// <summary>
        /// Metodo que inserta una actividad en el sistema
        /// </summary>
        /// <param name="actividad">Actividad que se desea registrar</param>
        /// <returns>Actividad insertada</returns>
        public Actividad InsertActividad(Actividad actividad)
        {
            var ctrl = new ActividadCtrl(ctx);
            ctrl.Insert(actividad);

            ctx.Commit(firma);

            return actividad;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<AModelo> ConsultarModelos()
        {
            #region Conexion Version Anterior
            IDataContext dctx = new DataContext((new DataProviderFactory()).GetDataProvider("POV"));
            #endregion
            List<AModelo> modelos = new List<AModelo>();

            ModeloCtrl modeloCtrl = new ModeloCtrl();
            Dictionary<string,string> parametros = new Dictionary<string,string>();
            parametros.Add("Activo", "true");
            parametros.Add("ModeloDiagnostico", "true");
            DataSet ds = modeloCtrl.Retrieve(dctx, null, parametros);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                modelos.Add(modeloCtrl.DataRowToAModelo(dr));
            }

            return modelos;
        }
       
        public List<Clasificador> ConsultarClasificadoresModelo(AModelo modelo)
        {
            if (!(modelo is ModeloDinamico)) throw new ArgumentException("modelo", "El modelo proporcionado no es del tipo esperado");

            #region Conexion Version Anterior
            IDataContext dctx = new DataContext((new DataProviderFactory()).GetDataProvider("POV"));
            #endregion

            List<Clasificador> clasificadores = new List<Clasificador>();

            ModeloCtrl modeloCtrl = new ModeloCtrl();
            clasificadores = modeloCtrl.RetrieveClasificadoresModeloDinamico(dctx, new ModeloDinamico { ModeloID = modelo.ModeloID });

            return clasificadores;
        }
    }
}
