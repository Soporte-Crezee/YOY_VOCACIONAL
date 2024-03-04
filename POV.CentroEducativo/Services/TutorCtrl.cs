using POV.CentroEducativo.BO;
using POV.CentroEducativo.Queries;
using POV.Modelo.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POV.CentroEducativo.Services
{
    public class TutorCtrl : IDisposable
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
        public TutorCtrl(Contexto contexto)
        {
            _sing = new object();
            _model = contexto ?? new Contexto(_sing);
        }

        /// <summary>
        /// Consulta el tutor que existen en el sistema
        /// </summary>
        /// <param name="criteria">Tutor que provee el criterio de búsqueda</param>
        /// <param name="tracking">Permite saber si se rastrearán los objetos que se obtengan de la consulta</param>
        /// <returns>Tutores que satisfacen el criterio de búsqueda</returns>
        public List<Tutor> Retrieve(Tutor criteria, bool tracking)
        {
            DbQuery<Tutor> qryTutores = (tracking) ? _model.Tutor : _model.Tutor.AsNoTracking();

            return qryTutores.Where(new TutorQry(criteria).Action()).ToList();
        }

        /// <summary>
        /// Agrega un Tutor al sistema
        /// </summary>
        /// <param name="tutor"> Tutor que se registrará</param>
        /// <returns>Resultado del registro</returns>
        public Boolean Insert(Tutor tutor)
        {
            var resultado = false;

            #region ** validaciones **
            if (tutor == null) throw new ArgumentNullException("Tutor", "Tutor no puede ser nulo");           
            if (string.IsNullOrEmpty(tutor.Nombre)) throw new ArgumentException("tutor.Nombre", "Nombre de Tutor no puede ser nulo o vacio");
            if (string.IsNullOrEmpty(tutor.PrimerApellido)) throw new ArgumentException("tutor.PrimerApellido", "Primer Apellido de tutor no puede ser nulo o vacio");
            if (tutor.Sexo == null) throw new ArgumentException("tutor.Sexo", "Sexo de tutor no puede ser nulo o vacio");
            if (string.IsNullOrEmpty(tutor.Codigo)) throw new ArgumentException("tutor.Codigo", "Codigo de Tutor no puede ser nulo o vacio");

            #endregion

            try
            {
                _model.Tutor.Add(tutor);
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
        /// Actualiza un Tutor
        /// </summary>
        /// <param name="tutor">Tutor a actualizar</param>
        /// <returns></returns>
        public Boolean Update(Tutor tutor)
        {
            var resultado = false;

            #region ** validaciones **
            if (tutor == null) throw new ArgumentNullException("Tutor", "Tutor no puede ser nulo");
            if (tutor.TutorID == null) throw new ArgumentNullException("tutor.TutorID", "Identificador de tutor no puede ser nulo");
            #endregion

            var afectados = _model.Commit(_sing);
            if (afectados != 0) resultado = true;

            return resultado;
        }

        /// <summary>
        /// Elimina un Tutor
        /// </summary>
        /// <param name="tutor">Tutor a eliminar</param>
        /// <returns></returns>
        public int Delete(Tutor tutor)
        {
            _model.Tutor.Remove(tutor);

            var afectados = _model.Commit(_sing);

            return afectados;
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
