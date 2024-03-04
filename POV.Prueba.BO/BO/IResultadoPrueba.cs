using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POV.CentroEducativo.BO;

namespace POV.Prueba.BO
{
    public interface IResultadoPrueba
    {
        Alumno Alumno { get; set; }

        ARegistroPrueba RegistroPrueba { get; set; }
    }
}
