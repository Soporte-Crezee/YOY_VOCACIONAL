using POV.Modelo.Context;
using POV.Operaciones.BO;
using POV.Operaciones.Queries;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POV.Operaciones.Services
{
    public class ResultadoExportacionOrientadorCtrl : IDisposable
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
        public ResultadoExportacionOrientadorCtrl(Contexto contexto) 
        {
            _sing = new object();
            _model = contexto ?? new Contexto(_sing);
        }

        /// <summary>
        /// Consulta los ResultadoExportacionOrientador que existen en el sistema
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="tracking"></param>
        /// <returns></returns>
        public List<ResultadoExportacionOrientador> Retrieve(ResultadoExportacionOrientador criteria, bool tracking) 
        {
            DbQuery<ResultadoExportacionOrientador> qryResultadoExportacionOrientador = (tracking) ? _model.ResultadoExportacionOrientador : _model.ResultadoExportacionOrientador.AsNoTracking();

            return qryResultadoExportacionOrientador.Where(new ResultadoExportacionOrientadorQry(criteria).Action()).ToList();
        }

        /// <summary>
        /// Agrega un ResultadoExportacionOrientador al sistema
        /// </summary>
        /// <param name="resultadoExportacionOrientador"> ResultadoExportacionOrientador que se registrará </param>
        /// <returns> Resutado del registro </returns>
        public Boolean Insert(ResultadoExportacionOrientador resultadoExportacionOrientador) 
        {
            var resultado = false;

            #region Validaciones
            if (resultadoExportacionOrientador == null) throw new ArgumentNullException("ResultadoExportacionOrientador", "ResultadoExportacionOrientador no puede ser nulo");
            if (string.IsNullOrEmpty(resultadoExportacionOrientador.Nombre)) throw new ArgumentException("ResultadoExportacionOrientador.Nombre", "Nombre no puede ser nulo");
            if (string.IsNullOrEmpty(resultadoExportacionOrientador.PrimerApellido)) throw new ArgumentException("ResultadoExportacionOrientador.PrimerApellido", "PrimerApellido no puede ser nulo");
            if (string.IsNullOrEmpty(resultadoExportacionOrientador.Curp)) throw new ArgumentException("ResultadoExportacionOrientador.Curp", "Curp no puede ser nulo");
            if (resultadoExportacionOrientador.Sexo == null) throw new ArgumentException("ResultadoExportacionOrientador.Sexo", "Sexo no puede ser nulo");
            if (resultadoExportacionOrientador.FechaNacimiento == null) throw new ArgumentException("ResultadoExportacionOrientador.FechaNacimiento", "FechaNacimiento no puede ser nulo");
            if (string.IsNullOrEmpty(resultadoExportacionOrientador.Correo)) throw new ArgumentException("ResultadoExportacionOrientador.Correo", "Correo no puede ser nulo");
            if (resultadoExportacionOrientador.FechaRegistro == null) throw new ArgumentException("ResultadoExportacionOrientador.FechaRegistro", "FechaRegistro no puede ser nulo");
            #endregion

            try
            {
                _model.ResultadoExportacionOrientador.Add(resultadoExportacionOrientador);
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
        /// Elimina un ResultadoExportacionOrientador de la base de datos
        /// </summary>
        /// <param name="resultadoExportacionOrientador"></param>
        /// <returns></returns>
        public Boolean Delete(ResultadoExportacionOrientador resultadoExportacionOrientador) 
        {
            var resultado = false;
            try
            {
                _model.ResultadoExportacionOrientador.Remove(resultadoExportacionOrientador);
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
        /// Método para cerrar la conexion del controlador con la base de datos
        /// </summary>
        public void Dispose()
        {
            _model.Disposing(_sing);
        }
    }
}
