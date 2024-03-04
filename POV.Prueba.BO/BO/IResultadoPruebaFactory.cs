using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POV.Reactivos.BO;
using POV.CentroEducativo.BO;

namespace POV.Prueba.BO
{
    public interface IResultadoPruebaFactory
    {
        IResultadoPrueba CreateResultadoPrueba(Alumno alumno, List<Reactivo> reactivos);
    }
}
