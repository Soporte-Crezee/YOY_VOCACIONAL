using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using POV.ConfiguracionActividades.BO;
using POV.CentroEducativo.BO;
using POV.ConfiguracionActividades.Queries;
using POV.Modelo.Context;

namespace POV.ConfiguracionActividades.Services
{
    /// <summary>
    /// Controlador de configuracion de bloques
    /// </summary>
    public class BloqueActividadCtrl : IDisposable
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
        public BloqueActividadCtrl(Contexto contexto)
        {
            _sing = new object();
            _model = contexto ?? new Contexto(_sing);
        }

        /// <summary>
        /// Consulta la lista de bloques de acuerdo a los criterios proporcionados como filtro
        /// </summary>
        /// <param name="filtro">Bloque que se usara como filtro</param>
        /// <param name="tracking">booleano que determina si los resultados permaneceran en el cache</param>
        /// <returns>Lista de bloques</returns>
        public List<BloqueActividad> RetrieveWithRelationship(BloqueActividad criteria, bool tracking)
        {
            #region ** validaciones **
            if (criteria == null) throw new ArgumentNullException("criteria", "BloqueActividad no puede ser nulo");
            #endregion

            DbQuery<BloqueActividad> qryBloques = (tracking) ? _model.BloquesActividad : _model.BloquesActividad.AsNoTracking();

            return qryBloques.Where(new BloqueActividadQry(criteria).Action())
                                .Include(x => x.CicloEscolar)
                                .Include(x => x.Escuela)
                                .ToList();
        }

        /// <summary>
        /// Consulta la lista de bloques de acuerdo a los criterios proporcionados como filtro
        /// </summary>
        /// <param name="filtro">Bloque que se usara como filtro</param>
        /// <param name="tracking">booleano que determina si los resultados permaneceran en el cache</param>
        /// <returns>Lista de bloques</returns>
        public List<BloqueActividad> Retrieve(BloqueActividad criteria, bool tracking)
        {
            #region ** validaciones **
            if (criteria == null) throw new ArgumentNullException("criteria", "BloqueActividad no puede ser nulo");
            #endregion

            DbQuery<BloqueActividad> qryBloques = (tracking) ? _model.BloquesActividad : _model.BloquesActividad.AsNoTracking();

            return qryBloques.Where(new BloqueActividadQry(criteria).Action())
                                .ToList();
        }

        /// <summary>
        /// Inserta un bloque
        /// </summary>
        /// <param name="bloque">bloque que se desea insertar</param>
        /// <returns>true indica si todo esta correcto, false en caso contrario</returns>
        public bool Insert(BloqueActividad bloqueActividad)
        {
            var resultado = false;

            #region ** validaciones **
            if (bloqueActividad == null) throw new ArgumentNullException("bloqueActividad", "BloqueActividad no puede ser nulo");
            if (string.IsNullOrEmpty(bloqueActividad.Nombre)) throw new ArgumentException("actividad.Nombre", "Nombre de actividad no puede ser nulo o vacio");
            if (bloqueActividad.CicloEscolarId == null) throw new ArgumentNullException("bloqueActividad.CicloEscolarId", "Identificador de ciclo escolar no puede ser nulo");
            if (bloqueActividad.EscuelaId == null) throw new ArgumentNullException("bloqueActividad.EscuelaId", "Identificador de escuela no puede ser nulo");
            if (bloqueActividad.FechaRegistro == null) throw new ArgumentNullException("bloqueActividad.FechaRegistro", "Fecha de registro no puede ser nulo");
            if (bloqueActividad.Activo == null) throw new ArgumentNullException("bloqueActividad.Activo", "Activo no puede ser nulo");
            if (bloqueActividad.FechaInicio == null) throw new ArgumentNullException("bloqueActividad.FechaInicio", "Fecha de inicio no puede ser nulo");
            if (bloqueActividad.FechaFin == null) throw new ArgumentNullException("bloqueActividad.FechaFin", "Fecha de fin no puede ser nulo");

            if (bloqueActividad.FechaFin.Value.CompareTo(bloqueActividad.FechaInicio.Value) < 0) throw new ArgumentException("bloqueActividad.FechaFin", "La fecha de fin es antes de la fecha de inicio");
            #endregion

            try
            {
                _model.BloquesActividad.Add(bloqueActividad);
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
        /// Actualiza un bloque
        /// </summary>
        /// <param name="bloque">bloque que se desea insertar</param>
        /// <returns>true indica si todo esta correcto, false en caso contrario</returns>
        public bool Update(BloqueActividad bloqueActividad)
        {
            var resultado = false;

            #region ** validaciones **
            if (bloqueActividad == null) throw new ArgumentNullException("bloqueActividad", "BloqueActividad no puede ser nulo");
            if (string.IsNullOrEmpty(bloqueActividad.Nombre)) throw new ArgumentException("actividad.Nombre", "Nombre de actividad no puede ser nulo o vacio");
            if (bloqueActividad.CicloEscolarId == null) throw new ArgumentNullException("bloqueActividad.CicloEscolarId", "Identificador de ciclo escolar no puede ser nulo");
            if (bloqueActividad.EscuelaId == null) throw new ArgumentNullException("bloqueActividad.EscuelaId", "Identificador de escuela no puede ser nulo");
            if (bloqueActividad.FechaRegistro == null) throw new ArgumentNullException("bloqueActividad.FechaRegistro", "Fecha de registro no puede ser nulo");
            if (bloqueActividad.Activo == null) throw new ArgumentNullException("bloqueActividad.Activo", "Activo no puede ser nulo");
            if (bloqueActividad.FechaInicio == null) throw new ArgumentNullException("bloqueActividad.FechaInicio", "Fecha de inicio no puede ser nulo");
            if (bloqueActividad.FechaFin == null) throw new ArgumentNullException("bloqueActividad.FechaFin", "Fecha de fin no puede ser nulo");

            if (bloqueActividad.FechaFin.Value.CompareTo(bloqueActividad.FechaInicio.Value) < 0) throw new ArgumentException("bloqueActividad.FechaFin", "La fecha de fin es antes de la fecha de inicio");
           
            #endregion

 
            var afectados = _model.Commit(_sing);
            if (afectados != 0) resultado = true;

            return resultado;
        }

        /// <summary>
        /// Elimina de manera fisica un registro de bloque actividad
        /// </summary>
        /// <param name="bloqueActividad">Bloque</param>
        /// <returns>True si se eliminó false en caso contrario</returns>
        public bool Delete(BloqueActividad bloqueActividad)
        {
            #region ** validaciones **
            if (bloqueActividad == null) throw new ArgumentNullException("bloqueActividad", "BloqueActividad no puede ser nulo");
            if (bloqueActividad.BloqueActividadId == null) throw new ArgumentNullException("bloqueActividad.BloqueActividadId", "BloqueActividad no puede ser nulo");
            #endregion

            var resultado = false;

            _model.BloquesActividad.Remove(bloqueActividad);

            var afectados = _model.Commit(_sing);
            if (afectados != 0) resultado = true;

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
