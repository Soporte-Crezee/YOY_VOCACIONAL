using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using Framework.Base.DataAccess;
using POV.ConfiguracionActividades.BO;
using POV.ConfiguracionActividades.Services;
using POV.Modelo.Context;
using POV.Modelo.Service;
using POV.Modelo.BO;

namespace POV.ServiciosActividades.Controllers
{
    public class MantenerActividadesController : IDisposable
    {
        #region Atributos

        ActividadCtrl ctrl;
        #endregion

        #region Constructores
        /// <summary>
		/// Constructor por defecto
		/// </summary>
        public MantenerActividadesController()
		{
            ctrl = new ActividadCtrl(null);
			
		}
        #endregion

        public List<BloqueActividad> ConsultarBloques(BloqueActividad filtro)
        {
            BloqueActividadCtrl bloqueCtrl = new BloqueActividadCtrl(null);
            return bloqueCtrl.Retrieve(filtro, false);
        }

        /// <summary>
        /// Consulta un actividad con todas sus relaciones
        /// </summary>
        /// <param name="actividad">Actividad filtro</param>
        /// <returns>Actividad encontrada, en caso contrario una actividad vacia</returns>
        public Actividad RetrieveActividadWithRelationship(Actividad actividad)
        {

            List<Actividad> list = ctrl.RetrieveWithRelationship(actividad, false).Where(item => !(item is ActividadDocente)).ToList();
            Actividad result = list.FirstOrDefault();
            if (result == null)
                return new Actividad();

            return result;
        }

        /// <summary>
        /// Consulta actividades de acuerdo a un filtro
        /// </summary>
        /// <param name="actividad">Actividad que servirá como filtro</param>
        /// <returns>Lista de actividades encontradas</returns>
        public List<Actividad> Retrieve(Actividad actividad)
        {
            List<Actividad> list = ctrl.RetrieveWithRelationship(actividad, false).Where(item => !(item is ActividadDocente)).ToList();

            return list;
        }
        /// <summary>
        /// Consulta un actividad
        /// </summary>
        /// <param name="actividad">Actividad filtro</param>
        /// <returns>Actividad encontrada, en caso contrario devuelve una actividad vacia</returns>
        public Actividad RetrieveActividad(Actividad actividad)
        {

            List<Actividad> list = ctrl.Retrieve(actividad, true).Where(item => !(item is ActividadDocente)).ToList();
            Actividad result = list.FirstOrDefault();
            if (result == null)
                return new Actividad();

            return result;
        }

        /// <summary>
        /// Actualiza los datos de una actividad
        /// </summary>
        /// <param name="actividad">Actividad que se quiere actualizar</param>
        /// <param name="tareasEliminadas">Lista de tareas de la actividad que se quiere eliminar de la actividad</param>
        /// <returns></returns>
        public Actividad UpdateActividad(Actividad actividad, List<long> tareasEliminadas, List<int> enfoquesEliminados)
        {
            //actualizamos actividad

            bool resp = ctrl.Update(actividad, tareasEliminadas, enfoquesEliminados);

           
            return actividad;
            
        }

        /// <summary>
        /// Verifica que la actividad no se encuentre asignada en el sistema
        /// </summary>
        /// <param name="actividad">Actividad que se quiere validar</param>
        /// <returns>true si la actividad está asignada, false en caso contrario</returns>
        public bool EsActividadAsignada(Actividad actividad)
        {
            bool esAsignada = false;

            AsignacionActividadCtrl ctrl = new AsignacionActividadCtrl(null);

            List<AsignacionActividad> asignaciones = ctrl.Retrieve(new AsignacionActividad { ActividadId = actividad.ActividadID }, false);

            if (asignaciones.Any())
                esAsignada = true;

            return esAsignada;
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
            Dictionary<string, string> parametros = new Dictionary<string, string>();
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

        public void EliminarActividad(Actividad actividad)
        {
            object firma = new object();
            Contexto contexto = new Contexto(firma);
            var aCtrl = new ActividadCtrl(contexto);
            var aDelete = aCtrl.Retrieve(new Actividad { ActividadID = actividad.ActividadID }, true).FirstOrDefault();
            if (aDelete != null)
            {
                aCtrl.Delete(aDelete);
                contexto.Commit(firma);
                aCtrl.Dispose();
            }
        }
        public void Dispose()
        {
            ctrl.Dispose();
        }
    }
}
