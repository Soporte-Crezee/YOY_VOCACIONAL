using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Comun.BO
{
    public interface IObservableChange
    {
        void Registrar(IObserverChange observer);
        object CloneAll();
    }
}
