using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Comun.BO
{
    public class ObjetoLista<T> : IObserverChange where T : class, ICloneable, IObservableChange
    {
        public ObjetoLista(T objeto)
        {
            this.Objeto = objeto;
            this.Objeto.Registrar(this);
            this.Original = (T)objeto.CloneAll();
        }

        public T Objeto { get; private set; }
        public T Original { get; private set; }
        public EObjetoEstado Estado { get; set; }

        public void Cambio()
        {
            if (this.Estado == EObjetoEstado.SINCAMBIO)
                this.Estado = EObjetoEstado.EDITADO;
        }
    }
}
