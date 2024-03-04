using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POV.Profesionalizacion.BO;

namespace POV.Licencias.BO
{
   public class ProfesionalizacionContrato
    {
       private List<EjeTematico> listaEjesTematicos;
       public List<EjeTematico> ListaEjesTematicos 
       {
           get { return this.listaEjesTematicos; }
           set { this.listaEjesTematicos = value; }
       }
    }
}
