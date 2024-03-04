using POV.Administracion.BO;
using POV.Administracion.Queries;
using POV.Modelo.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace POV.Administracion.Services
{
    public class ProductoCosteoCtrl : IDisposable
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
        public ProductoCosteoCtrl(Contexto contexto) 
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
        public List<ProductoCosteo> RetrieveWithRelationship(ProductoCosteo criteria, bool tracking)
        {
            DbQuery<ProductoCosteo> queryProductoCosteo = (tracking) ? _model.Producto : _model.Producto.AsNoTracking();

            return queryProductoCosteo.Where(new ProductoCosteoQry(criteria).Action())
                                .Include(x => x.CostoProducto)
                                .ToList();
        }

        /// <summary>
        /// Consulta el ProductoCosteo que existe en el sistema
        /// </summary>
        /// <param name="criteria">ProductoCosteo que provee el criterio de busqueda</param>
        /// <param name="tracking">Permite saber si se rastrearan los objetos que se obtengan de la consulta</param>
        /// <returns>ProductoCosteo que atisface el criterio de busqueda</returns>
        public List<ProductoCosteo> Retrieve(ProductoCosteo criteria, bool tracking) 
        {
            DbQuery<ProductoCosteo> queryProductoCosteo = (tracking) ? _model.Producto : _model.Producto.AsNoTracking();
            return queryProductoCosteo.Where(new ProductoCosteoQry(criteria).Action()).ToList();
        }

        /// <summary>
        /// Agrega un ProductoCosteo al sistema
        /// </summary>
        /// <param name="productoCosteo">ProductoCosteo que se registrará</param>
        /// <returns>Resultado de registro</returns>
        public Boolean Insert(ProductoCosteo productoCosteo) 
        {
            var resultado = false;
            
            #region Validaciones
            if (productoCosteo == null) throw new ArgumentNullException("ProductoCosteo", "ProductoCosteo no puede se nulo");
            if (string.IsNullOrEmpty(productoCosteo.Nombre)) throw new ArgumentException("productoCosteo.Nombre", "Nombre no puedo ser nulo");
            if (string.IsNullOrEmpty(productoCosteo.Descripcion)) throw new ArgumentException("productoCosteo.Descripcion", "Descripcion no puede ser nulo");
            #endregion

            try
            {
                _model.Producto.Add(productoCosteo);
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
        /// <param name="productoCosteo"></param>
        /// <returns></returns>
        public Boolean Update(ProductoCosteo productoCosteo) 
        {
            var resultado = false;

            #region Validaciones
            if (productoCosteo == null) throw new ArgumentNullException("ProductoCosteo", "ProductoCosteo no puede se nulo");
            if (string.IsNullOrEmpty(productoCosteo.Descripcion)) throw new ArgumentException("productoCosteo.Descripcion", "Descripcion no puede ser nulo");
            #endregion

            var afectados = _model.Commit(_sing);
            if (afectados != 0) resultado = true;

            return resultado;
        }

        /// <summary>
        /// Elimina un ProductoCosteo de la base de datos
        /// </summary>
        /// <param name="productoCosteo"></param>
        /// <returns></returns>
        public Boolean Delete(ProductoCosteo productoCosteo) 
        {
            var resultado = false;

            try
            {
                _model.Producto.Remove(productoCosteo);
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
