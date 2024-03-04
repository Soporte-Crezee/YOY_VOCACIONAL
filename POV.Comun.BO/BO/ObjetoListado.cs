using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Linq.Expressions;

namespace POV.Comun.BO
{
    public class ObjetoListado<T> where T : class, ICloneable, IObservableChange
    {
        private List<ObjetoLista<T>> objetos;
        public IEnumerable<T> Objetos
        {
            get { return this.objetos.Select(x => x.Objeto); }
        }

        public ObjetoListado()
        {
            objetos = new List<ObjetoLista<T>>();
        }
        public ObjetoListado(IEnumerable<T> objetos) : this()
        {
            foreach (T objeto in objetos)
            {
                ObjetoLista<T> sincambio = new ObjetoLista<T>(objeto);
                sincambio.Estado = EObjetoEstado.SINCAMBIO;

                this.objetos.Add(sincambio);
            }
        }

        /// <summary>
        /// Agrega objeto
        /// </summary>
        public T Agregar(Func<T, bool> identificador, T nuevo)
        {
            ObjetoLista<T> agregar = new ObjetoLista<T>(nuevo);
            agregar.Estado = EObjetoEstado.NUEVO;

            T objeto = this.Objetos.FirstOrDefault(identificador);

            if (objeto == null)
            {
                this.objetos.Add(agregar);
                objeto = nuevo;
            }
            else
            {
                agregar = this.objetos.Find(x => x.Objeto == objeto);
                if (agregar.Estado == EObjetoEstado.ELIMINADO)
                    agregar.Estado = EObjetoEstado.SINCAMBIO;
            }

            return objeto;
        }
        /// <summary>
        /// Obtener objeto basado en el indice
        /// </summary>
        public T Obtener(int indice)
        {
            return this.objetos[indice].Objeto;
        }
        /// <summary>
        /// Elimina objeto
        /// </summary>
        public T Eliminar(Func<T, bool> identificador, T eliminar)
        {
            T objeto = this.Objetos.FirstOrDefault(identificador);
            if (objeto == null)
                return objeto;

            ObjetoLista<T> elemento = this.objetos.First(x => x.Objeto == objeto);
            bool nuevo = elemento.Estado == EObjetoEstado.NUEVO;
            elemento.Estado = EObjetoEstado.ELIMINADO;

            if (nuevo)
                this.objetos.Remove(elemento);

            return elemento.Objeto;
        }
        /// <summary>
        /// Elimina UnidadOperativa basado en el indice
        /// </summary>
        public T Eliminar(int indice)
        {
            ObjetoLista<T> objeto = this.objetos[indice];
            bool nuevo = objeto.Estado == EObjetoEstado.NUEVO;
            objeto.Estado = EObjetoEstado.ELIMINADO;

            if (nuevo)
                this.objetos.Remove(objeto);

            return objeto.Objeto;
        }
        /// <summary>
        /// Obtiene el numero de elementos de la lista
        /// </summary>
        public int Elementos()
        {
            return this.objetos.Count(x => x.Estado != EObjetoEstado.ELIMINADO);
        }
        /// <summary>
        /// Obtiene el numero completo de elementos de la lista
        /// </summary>
        public int ElementosTodos()
        {
            return this.objetos.Count();
        }
        /// <summary>
        /// Elimina todos los elementos del lista
        /// </summary>
        public void Limpiar()
        {
            ObjetoLista<T>[] eliminacion = new ObjetoLista<T>[this.objetos.Count];
            this.objetos.CopyTo(eliminacion);

            bool nuevo = false;
            foreach (ObjetoLista<T> objeto in eliminacion)
            {
                nuevo = objeto.Estado == EObjetoEstado.NUEVO;
                objeto.Estado = EObjetoEstado.ELIMINADO;

                if (nuevo)
                    this.objetos.Remove(objeto);
            }
        }
        /// <summary>
        ///  Obtener el estado actual del objeto
        /// </summary>
        public EObjetoEstado Estado(T objeto)
        {
            ObjetoLista<T> estado = this.objetos.FirstOrDefault(x => x.Objeto  == objeto);

            EObjetoEstado resultado = EObjetoEstado.DESCONOCIDO;
            if (estado != null)
                resultado = estado.Estado;

            return resultado;
        }
        /// <summary>
        /// Obtener el estado original del objeto
        /// </summary>
        public T Original(T objeto)
        {
            ObjetoLista<T> anterior = this.objetos.FirstOrDefault(x => x.Objeto == objeto);

            T original = null;
            if (anterior != null)
                original = anterior.Original;

            return original;
        }
    }
}
