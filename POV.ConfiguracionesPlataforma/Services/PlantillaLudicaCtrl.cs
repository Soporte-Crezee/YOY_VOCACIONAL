using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using POV.Modelo.Context;
using POV.ConfiguracionesPlataforma.BO;
using POV.ConfiguracionesPlataforma.Queries;

namespace POV.ConfiguracionesPlataforma.Services
{
    public class PlantillaLudicaCtrl: IDisposable
    {
        /// <summary>
        /// Contexto interno de conexión a la base de datos
        /// </summary>
        private readonly Contexto _model;

        /// <summary>
        /// Firma de la conexion a la base de datos
        /// </summary>
        private readonly object _sign;

        /// <summary>
        /// Constructor encargado de crear la conexion interna a la base de datos
        /// </summary>
        /// <param name="contexto">Contexto de conexion a la base de datos</param>
        public PlantillaLudicaCtrl(Contexto contexto)
        {
            _sign = new object();
            _model = contexto ?? new Contexto(_sign);
        }

        /// <summary>
        /// Método para cerrar la conexión del controlador con la base de datos
        /// </summary>
        public void Dispose()
        {
            _model.Disposing(_sign);
        }

        /// <summary>
        /// Consulta las plantillas lúdicas existentes en el sistema
        /// </summary>
        /// <param name="criteria">Plantilla Lúdica que provee el criterio de búsqueda</param>
        /// <param name="isTracking">Permite saber si se rastrearan los objetos que se obtengan de la búsqueda</param>
        /// <returns>PLantillas Lúdicas que satisfacen el criterio de búsqueda</returns>
        public List<PlantillaLudica> Retrieve(PlantillaLudica criteria, bool isTracking)
        {
            DbQuery<PlantillaLudica> qryPlantillaLudica = (isTracking) ? _model.PlantillasLudicas : _model.PlantillasLudicas.AsNoTracking();
            return qryPlantillaLudica.Where(new PlantillaLudicaQry(criteria).Action()).ToList();
        }

        /// <summary>
        /// Consulta la información completa de las plantillas lúdicas del sistema
        /// </summary>
        /// <param name="criteria">Plantilla ludica que provee el criterio de búsqueda</param>
        /// <param name="isTracking">Permite saber si se rastrearan los objetos que se obtengan de la consulta</param>
        /// <returns>Plantilla Ludica que satisfacen el criterio de búsqueda</returns>
        public List<PlantillaLudica> RetrieveWithRelationship(PlantillaLudica criteria, bool isTracking)
        {
            DbQuery<PlantillaLudica> qryPlantillaLudica = (isTracking) ? _model.PlantillasLudicas : _model.PlantillasLudicas.AsNoTracking();
            return qryPlantillaLudica.Where(new PlantillaLudicaQry(criteria).Action())
                .Include(x => x.PosicionesActividades)
                .ToList();
        }

        /// <summary>
        /// Agrega la plantilla ludica al sistema
        /// </summary>
        /// <param name="plantillaLudica">PLantilla Lúdica a registrar</param>
        /// <returns>Resultado de registro de la asignación</returns>
        public Boolean Insert(PlantillaLudica plantillaLudica)
        {
            var resultado = false;

            try
            {
                _model.PlantillasLudicas.Add(plantillaLudica);
                var afectados = _model.Commit(_sign);

                if (afectados != 0) resultado = true;
            }
            catch (Exception exception)
            {
                throw exception;
            }

            return resultado;
        }

        /// <summary>
        /// Método para Actualizar la plantilla lúdica
        /// </summary>
        public Boolean Update(PlantillaLudica plantillaLudica, List<int> posicionesEliminadas = null)
        {
            var resultado = false;

            if (string.IsNullOrEmpty(plantillaLudica.Nombre)) throw new ArgumentException("plantillaLudica.Nombre", "Nombre de la plantilla no puede ser nulo o vacio");
            if (string.IsNullOrEmpty(plantillaLudica.ImagenFondo)) throw new ArgumentException("plantillaLudica.ImagenFondo", "Imagen de Fondo de la plantilla no puede ser nulo o vacio");
            if (string.IsNullOrEmpty(plantillaLudica.ImagenPendiente)) throw new ArgumentException("plantillaLudica.ImagenPendiente", "Imagen Pendiente de la plantilla no puede ser nulo o vacio");
            if (string.IsNullOrEmpty(plantillaLudica.ImagenIniciado)) throw new ArgumentException("plantillaLudica.ImagenIniciado", "Imagen Iniciado de la plantilla no puede ser nulo o vacio");
            if (string.IsNullOrEmpty(plantillaLudica.ImagenFinalizado)) throw new ArgumentException("plantillaLudica.ImagenFinalizado", "Imagen Finalizado de la plantilla no puede ser nulo o vacio");
            if (string.IsNullOrEmpty(plantillaLudica.ImagenNoDisponible)) throw new ArgumentException("plantillaLudica.ImagenNoDisponible", "Imagen No Disponible de la plantilla no puede ser nulo o vacio");
            if (string.IsNullOrEmpty(plantillaLudica.ImagenFlechaArriba)) throw new ArgumentException("plantillaLudica.ImagenFlechaArriba", "Imagen Flecha Arriba de la plantilla no puede ser nulo o vacio");
            if (string.IsNullOrEmpty(plantillaLudica.ImagenFlechaAbajo)) throw new ArgumentException("plantillaLudica.ImagenFlechaAbajo", "Imagen Flecha Abajo de la plantilla no puede ser nulo o vacio");
            if (plantillaLudica.EsPredeterminado == null) throw new ArgumentNullException("plantillaLudica.EsPredeterminado", "Activo no puede ser nulo");
            if (plantillaLudica.FechaRegistro == null) throw new ArgumentNullException("plantillaLudica.FechaCreacion", "Fecha de creación no puede ser nulo");
            if (plantillaLudica.Activo == null) throw new ArgumentNullException("plantillaLudica.Activo", "Activo no puede ser nulo");
            if (!plantillaLudica.PosicionesActividades.Any()) throw new ArgumentException("plantillaLudica.PosicionesActividad", "Lista de Posiciones Actividad no puede estar vacia");

            //si hay tareas por eliminar, se eliminan del sistema
            if (posicionesEliminadas != null && posicionesEliminadas.Count > 0)
                posicionesEliminadas.ForEach(t => DeletePosicionActividad(plantillaLudica.PosicionesActividades.FirstOrDefault(at => at.PosicionActividadId == t)));

            var afectados = _model.Commit(_sign);
            if (afectados != 0) resultado = true;

            return resultado;
        }

        /// <summary>
        /// Elimina una posicion actividad de la base de datos
        /// </summary>
        /// <param name="posicionActividad"></param>
        /// <returns></returns>
        private Boolean DeletePosicionActividad(PosicionActividad posicionActividad)
        {
            var res = true;
            _model.Entry(posicionActividad).State = EntityState.Deleted;
            return res;

        }
    }
}
