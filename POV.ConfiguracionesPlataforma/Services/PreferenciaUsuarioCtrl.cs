using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using POV.Modelo.Context;
using POV.ConfiguracionesPlataforma.BO;
using POV.ConfiguracionesPlataforma.Queries;
using POV.Seguridad.BO;

namespace POV.ConfiguracionesPlataforma.Services
{
    public class PreferenciaUsuarioCtrl: IDisposable
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
        public PreferenciaUsuarioCtrl(Contexto contexto)
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
        /// Consulta las Preferencia de Usuario existentes en el sistema
        /// </summary>
        /// <param name="criteria">Preferencia de Usuario que provee el criterio de búsqueda</param>
        /// <param name="isTracking">Permite saber si se rastrearan los objetos que se obtengan de la búsqueda</param>
        /// <returns>Preferencia de Usuario que satisfacen el criterio de búsqueda</returns>
        public List<PreferenciaUsuario> Retrieve(PreferenciaUsuario criteria, bool isTracking)
        {
            DbQuery<PreferenciaUsuario> qryPreferenciaUsuario = (isTracking) ? _model.PreferenciaUsuarios : _model.PreferenciaUsuarios.AsNoTracking();
            return qryPreferenciaUsuario.Where(new PreferenciaUsuarioQry(criteria).Action()).ToList();
        }

        /// <summary>
        /// Consulta la información completa de las Preferencia de Usuario del sistema
        /// </summary>
        /// <param name="criteria">PreferenciaUsuario que provee el criterio de búsqueda</param>
        /// <param name="isTracking">Permite saber si se rastrearan los objetos que se obtengan de la consulta</param>
        /// <returns>Preferencias del usuario que satisfacen el criterio de búsqueda</returns>
        public List<PreferenciaUsuario> RetrieveWithRelationship(PreferenciaUsuario criteria, bool isTracking)
        {
            DbQuery<PreferenciaUsuario> qryPreferenciaUsuario = (isTracking) ? _model.PreferenciaUsuarios : _model.PreferenciaUsuarios.AsNoTracking();
            return qryPreferenciaUsuario.Where(new PreferenciaUsuarioQry(criteria).Action())
                .Include(x => x.PlantillasLudicas)
                .Include(y => y.Usuarios)
                .ToList();
        }

        /// <summary>
        /// Agrega la preferencia de usuario al sistema
        /// </summary>
        /// <param name="PreferenciaUsuario">Preferencia de Usuario a registrar</param>
        /// <returns>Resultado de registro de la asignación</returns>
        public Boolean Insert(PreferenciaUsuario preferenciaUsuario)
        {
            var resultado = false;

            try
            {

                PreferenciaUsuarioCtrl ctrl = new PreferenciaUsuarioCtrl(_model);

                var preferenciaUsuarioRtv = new PreferenciaUsuario() { Usuarios = new Usuario { UsuarioID = preferenciaUsuario.UsuarioId } };

                List<PreferenciaUsuario> lstPreferenciaUsuario = ctrl.Retrieve(preferenciaUsuarioRtv, false);

                if (lstPreferenciaUsuario.Count > 0)
                {
                    throw new ArgumentException("PreferenciaUsuarioCtrl.Insert", "Ya existe una configuración para el usuario");
                }
                else
                {

                    _model.PreferenciaUsuarios.Add(preferenciaUsuario);
                    var afectados = _model.Commit(_sign);

                    if (afectados != 0) resultado = true;
                }
                
            }
            catch (Exception exception)
            {
                throw exception;
            }

            return resultado;
        }

        /// <summary>
        /// Método para Actualizar la Preferencia de Usuario
        /// </summary>
        public Boolean Update(PreferenciaUsuario preferenciasUsuario)
        {
            var resultado = false;

            if (preferenciasUsuario.UsuarioId == null) throw new ArgumentException("PreferenciaUsuario.UsuarioId", "El identificador del usuario no puede ser nulo o vacio");
            if (preferenciasUsuario.FechaRegistro == null) throw new ArgumentNullException("PreferenciaUsuario.FechaCreacion", "Fecha de creación no puede ser nulo");

            var afectados = _model.Commit(_sign);
            if (afectados != 0) resultado = true;

            return resultado;
        }
    }
}
