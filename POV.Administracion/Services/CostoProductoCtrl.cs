using POV.Administracion.BO;
using POV.Administracion.Queries;
using POV.Modelo.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POV.Administracion.Services
{
    public class CostoProductoCtrl : IDisposable
    {
        /// <summary>
        /// Contexto interno de conexion a la base de datos
        /// </summary>
        private readonly Contexto _model;

        /// <summary>
        /// Firma de la conexion a la base de datos
        /// </summary>
        private readonly object _sing;

        /// <summary>
        /// Constructor encargado de crear la conexion interna a la base de datos
        /// </summary>
        /// <param name="contexto">Contexto de conexion a la bae de datos</param>
        public CostoProductoCtrl(Contexto contexto) 
        {
            _sing = new object();
            _model = contexto ?? new Contexto(_sing);
        }

        /// <summary>
        /// Consulta el CostoProducto que existe en el sistema
        /// </summary>
        /// <param name="criteria">CostoProducto que provee el criterio de busqueda</param>
        /// <param name="tracking">Permite saber si se rastrearan los objetos que se obtengan de la consulta</param>
        /// <returns>CostoProducto que atisface el criterio de busqueda</returns>
        public List<CostoProducto> Retrieve(CostoProducto criteria, bool tracking) 
        {
            DbQuery<CostoProducto> queryCostoProducto = (tracking) ? _model.CostoProducto : _model.CostoProducto.AsNoTracking();
            return queryCostoProducto.Where(new CostoProductoQry(criteria).Action()).ToList();
        }

        /// <summary>
        /// Consulta la información completa de los CostoProducto del sistema
        /// </summary>
        /// <param name="criteria">CostoProducto que provee el criterio de búsqueda</param>
        /// <param name="tracking">Permite saber si se rastrearán los objetos que se obtengan de la consulta</param>
        /// <returns>CostoProducto que satisfacen el criterio de búsqueda</returns>
        public List<CostoProducto> RetrieveWithRelationship(CostoProducto criteria, bool tracking)
        {
            DbQuery<CostoProducto> qryCostoProducto = (tracking) ? _model.CostoProducto : _model.CostoProducto.AsNoTracking();

            return qryCostoProducto.Where(new CostoProductoQry(criteria).Action())
                                .Include(x => x.Producto)
                                .ToList();
        }

        /// <summary>
        /// Agrega un CostoProducto al sistema
        /// </summary>
        /// <param name="costoProducto">CostoProducto que se registrará</param>
        /// <returns>Resultado de registro</returns>
        public Boolean Insert(CostoProducto costoProducto) 
        {
            var resultado = false;
            
            #region Validaciones
            if (costoProducto == null) throw new ArgumentNullException("CostoProducto", "CostorProducto no puede se nulo");
            //if (string.IsNullOrEmpty(costoProducto.Nombre)) throw new ArgumentException("costoProducto.Nombre", "Nombre no puedo ser nulo");
            //if (string.IsNullOrEmpty(costoProducto.Descripcion)) throw new ArgumentException("costoProducto.Descripcion", "Descripcion no puede ser nulo");
            if (costoProducto.Precio == null) throw new ArgumentException("costoProducto.Precio", "Precio no puede ser nulo");
            if (costoProducto.FechaInicio == null) throw new ArgumentException("costoProducto.FechaInicio", "FechaInicio no puede ser nulo");
            #endregion

            try
            {
                _model.CostoProducto.Add(costoProducto);
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
        /// 
        /// </summary>
        /// <param name="costoProducto"></param>
        /// <returns></returns>
        public Boolean Update(CostoProducto costoProducto) 
        {
            var resultado = false;

            #region Validaciones
            if (costoProducto == null) throw new ArgumentNullException("CostoProducto", "CostorProducto no puede se nulo");
            //if (string.IsNullOrEmpty(costoProducto.Descripcion)) throw new ArgumentException("costoProducto.Descripcion", "Descripcion no puede ser nulo");
            #endregion

            var afectados = _model.Commit(_sing);
            if (afectados != 0) resultado = true;

            return resultado;
        }

        /// <summary>
        /// Elimina un CostoProducto de la base de datos
        /// </summary>
        /// <param name="costoProducto"></param>
        /// <returns></returns>
        public Boolean Delete(CostoProducto costoProducto) 
        {
            var resultado = false;

            try
            {
                _model.CostoProducto.Remove(costoProducto);
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
