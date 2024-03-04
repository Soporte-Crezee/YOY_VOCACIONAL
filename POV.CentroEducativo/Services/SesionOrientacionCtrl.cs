using POV.CentroEducativo.BO;
using POV.CentroEducativo.Queries;
using POV.Modelo.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POV.CentroEducativo.Services
{
    public class SesionOrientacionCtrl : IDisposable
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
        /// Constructor encargado de crear la conexión interna a la base de datos
        /// </summary>
        /// <param name="contexto"> Contexto de conexión a la base de datos </param>
        public SesionOrientacionCtrl(Contexto contexto) 
        {
            _sing = new object();
            _model = contexto ?? new Contexto(_sing);
        }

        /// <summary>
        /// Consulta la SesionOrientacion que existen en el sistema
        /// </summary>
        /// <param name="criteria"> SesionOrientacion que provee el criterio de búsqueda </param>
        /// <param name="tracking"> Permite saber si se rastrearán los objetos que se obtengan de la consulta </param>
        /// <returns> SesionOrientacion que satisfacen el criterio de búsqueda </returns>
        public List<SesionOrientacion> Retrieve(SesionOrientacion criteria, bool tracking) 
        {
            DbQuery<SesionOrientacion> qrySesionOrientacion = (tracking) ? _model.SesionOrientacion : _model.SesionOrientacion.AsNoTracking();
            return qrySesionOrientacion.Where(new SesionOrientacionQry(criteria).Action()).ToList();
        }

        /// <summary>
        /// Agrega una SesionOrientacion al sistema
        /// </summary>
        /// <param name="sesionOrientacion"> SesionOrientacion que se registrará </param>
        /// <returns> Resultado del registro </returns>
        public Boolean Insert(SesionOrientacion sesionOrientacion) 
        {
            var resultado = false;

            #region ** validaciones **
            if (sesionOrientacion == null) throw new ArgumentNullException("SesionOrientacion", "SesionOrientacion no puede ser nulo");
            if (string.IsNullOrEmpty(sesionOrientacion.Inicio)) throw new ArgumentException("SesionOrientacion.Inicio", "Inicio no piede ser nulo");
            if (string.IsNullOrEmpty(sesionOrientacion.Fin)) throw new ArgumentException("SesionOrientacion.Fin", "Fin no puede ser nulo");
            if (sesionOrientacion.Fecha == null) throw new ArgumentNullException("SesionOrientacion.Fecha", "Fecha no puede ser nulo");
            if (sesionOrientacion.EstatusSesion == null) throw new ArgumentNullException("SesionOrientacion.EstusSesion", "EstatusSesion no puede ser nulo");
            if (sesionOrientacion.EncuestaContestada == null) throw new ArgumentNullException("SesionOrientacion.EncuestaContestada", "EncuestaContestada no puede ser nulo");
            if (sesionOrientacion.AlumnoID == null) throw new ArgumentNullException("SesionOrientacion.AlumnoID", "AlumnoID no puede ser nulo");
            if (sesionOrientacion.DocenteID == null) throw new ArgumentNullException("SesionOrientacion.DocenteID", "DocenteID no puede ser nulo");            
            #endregion

            try
            {
                _model.SesionOrientacion.Add(sesionOrientacion);
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
        /// Actualiza una SesionOrientacion
        /// </summary>
        /// <param name="sesionOrientacion"> SesionOrientacion a actualizar </param>
        /// <returns></returns>
        public Boolean Update(SesionOrientacion sesionOrientacion) 
        {
            var resultado = false;

            #region ** validaciones **
            if (sesionOrientacion == null) throw new ArgumentNullException("SesionOrientacion", "SesionOrientacion no puede ser nulo");
            //if (string.IsNullOrEmpty(sesionOrientacion.Inicio)) throw new ArgumentException("SesionOrientacion.Inicio", "Inicio no piede ser nulo");
            //if (sesionOrientacion.HoraFinalizado == null) throw new ArgumentNullException("SesionOrientacion.HoraFinalizado", "HoraFinalizado no piede ser nulo");
            //if (string.IsNullOrEmpty(sesionOrientacion.Fin)) throw new ArgumentException("SesionOrientacion.Fin", "Fin no puede ser nulo");
            //if (sesionOrientacion.AsistenciaAspirante == null) throw new ArgumentNullException("SesionOrientacion.AsistenciaAspirante", "AsistenciaAspirante no puede ser nulo");
            //if (sesionOrientacion.Fecha == null) throw new ArgumentNullException("SesionOrientacion.Fecha", "Fecha no puede ser nulo");
            //if (sesionOrientacion.AsistenciaOrientador == null) throw new ArgumentNullException("SesionOrientacion.AsistenciaOrientador", "AsistenciaOrientador no puede ser nulo");
            //if (sesionOrientacion.EstatusSesion == null) throw new ArgumentNullException("SesionOrientacion.EstusSesion", "EstatusSesion no puede ser nulo");
            //if (sesionOrientacion.EncuestaContestada == null) throw new ArgumentNullException("SesionOrientacion.EncuestaContestada", "EncuestaContestada no puede ser nulo");
            //if (sesionOrientacion.AlumnoID == null) throw new ArgumentNullException("SesionOrientacion.AlumnoID", "AlumnoID no puede ser nulo");
            //if (sesionOrientacion.DocenteID == null) throw new ArgumentNullException("SesionOrientacion.DocenteID", "DocenteID no puede ser nulo");

            #endregion

            var afectados = _model.Commit(_sing);
            if (afectados != 0) resultado = true;

            return resultado;
        }

        /// <summary>
        /// Elimina una SesionOrientacion de la base de datos
        /// </summary>
        /// <param name="sesionOrientacion"></param>
        /// <returns></returns>
        public Boolean Delete(SesionOrientacion sesionOrientacion) 
        {
            var resultado = false;

            try
            {
                _model.SesionOrientacion.Remove(sesionOrientacion);
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
        /// Método para crear la conexión del controlador con la base de datos
        /// </summary>
        public void Dispose() 
        {
            _model.Disposing(_sing);
        }
    }
}
