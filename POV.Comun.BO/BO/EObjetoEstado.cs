using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Comun.BO
{
    public enum EObjetoEstado : byte
    {
        DESCONOCIDO = 0,
        SINCAMBIO = 1,
        NUEVO = 2,
        EDITADO = 3,
        ELIMINADO = 4
    }
}
