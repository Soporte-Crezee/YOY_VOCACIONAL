using POV.CentroEducativo.BO;
using POV.CentroEducativo.Queries;
using POV.Modelo.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace POV.CentroEducativo.Services
{
    public class CarreraCtrl : IDisposable
    {
        /// <summary>
        /// Contexto interno de conexión a la base de datos
        /// </summary>
        private readonly Contexto _model;

        /// <summary>
        /// Firma de la conexión a la base de datos
        /// </summary>
        private readonly object _sing;

        /// <summary>
        /// Constructor encargado de crear la conexion interna a la base de datos
        /// </summary>
        /// <param name="contexto">Contexto de conexion a la base de datos</param>
        public CarreraCtrl(Contexto contexto)
        {
            _sing = new object();
            _model = contexto ?? new Contexto(_sing);
        }

        /// <summary>
        /// Consulta la información completa de los Carreraes del sistema
        /// </summary>
        /// <param name="criteria">Carrera que provee el criterio de búsqueda</param>
        /// <param name="tracking">Permite saber si se rastrearán los objetos que se obtengan de la consulta</param>
        /// <returns>Carreraes que satisfacen el criterio de búsqueda</returns>
        public List<Carrera> RetrieveWithRelationship(Carrera criteria, bool tracking)
        {
            DbQuery<Carrera> qryCarrera = (tracking) ? _model.Carrera : _model.Carrera.AsNoTracking();

            return qryCarrera.Where(new CarreraQry(criteria).Action())
                                .Include(x => x.Universidades)
                                .ToList();
        }

        /// <summary>
        /// Consulta la Carreraes que existen en el sistema
        /// </summary>
        /// <param name="criteria">Carrera que provee el criterio de búsqueda</param>
        /// <param name="tracking">Permite saber si se rastrearán los objetos que se obtengan de la consulta</param>
        /// <returns>Carreraes que satisfacen el criterio de búsqueda</returns>
        public List<Carrera> Retrieve(Carrera criteria, bool tracking)
        {
            DbQuery<Carrera> qryCarreraes = (tracking) ? _model.Carrera : _model.Carrera.AsNoTracking();

            return qryCarreraes.Where(new CarreraQry(criteria).Action()).ToList();
        }

        /// <summary>
        /// Agrega una Carrera al sistema
        /// </summary>
        /// <param name="Carrera"> Carrera que se registrará</param>
        /// <returns>Resultado del registro</returns>
        public Boolean Insert(Carrera Carrera)
        {
            var resultado = false;

            #region ** validaciones **
            if (Carrera == null) throw new ArgumentNullException("Carrera", "Carrera no puede ser nulo");
            //if (Carrera.CarreraID == null) throw new ArgumentNullException("Carrera.CarreraID", "Identificador de carrera no puede ser nulo");
            if (string.IsNullOrEmpty(Carrera.NombreCarrera)) throw new ArgumentException("Carrera.NombreCarrera", "Nombre de Carrera no puede ser nulo o vacio");
            if (string.IsNullOrEmpty(Carrera.Descripcion)) throw new ArgumentException("Carrera.Descripcion", "Descripción de Carrera no puede ser nulo o vacio");
            if (Carrera.Activo == null) throw new ArgumentNullException("Carrera.Activo", "Activo no puede ser nulo");
            if (Carrera.ClasificadorID == null) throw new ArgumentNullException("Carrera.ClasificadorID", "ClasificadorID no puede ser nulo");
            #endregion

            try
            {
                _model.Carrera.Add(Carrera);
                var afectados = _model.Commit(_sing);

                if (afectados != 0) resultado = true;
            }
            catch (Exception exception)
            {
                throw exception;
            }

            return resultado;
        }


        /// <summary>
        /// Actualiza una Carrera
        /// </summary>
        /// <param name="Carrera">Carrera a actualizar</param>
        /// <param name="tareasEliminadas">Lista de tareas a eliminar</param>
        /// <returns></returns>
        public Boolean Update(Carrera Carrera, List<long> tareasEliminadas = null, List<int> enfoquesEliminados = null)
        {
            var resultado = false;

            #region ** validaciones **
            if (Carrera == null) throw new ArgumentNullException("Carrera", "Carrera no puede ser nulo");
            if (Carrera.CarreraID == null) throw new ArgumentNullException("Carrera.CarreraID", "Identificador de carrera no puede ser nulo");
            if (string.IsNullOrEmpty(Carrera.NombreCarrera)) throw new ArgumentException("Carrera.Nombre", "Nombre de Carrera no puede ser nulo o vacio");
            if (string.IsNullOrEmpty(Carrera.Descripcion)) throw new ArgumentException("Carrera.Descripcion", "Descripción de Carrera no puede ser nulo o vacio");
            if (Carrera.Activo == null) throw new ArgumentNullException("Carrera.Activo", "Activo no puede ser nulo");

            #endregion

            var afectados = _model.Commit(_sing);
            if (afectados != 0) resultado = true;

            return resultado;
        }

        /// <summary>
        /// Elimina una Carrera de la base de datos
        /// </summary>
        /// <param name="tarea"></param>
        /// <returns></returns>
        public Boolean Delete(Carrera carrera)
        {
            var resultado = false;

            try
            {
                _model.Carrera.Remove(carrera);
                var afectados = _model.Commit(_sing);

                if (afectados != 0) resultado = true;
            }
            catch (Exception exception)
            {
                throw exception;
            }

            return resultado;
        }

        /// <summary>
        /// Método para cerrar la conexión del controlador con la base de datos
        /// </summary>
        public void Dispose()
        {
            _model.Disposing(_sing);
        }
    }
}
