using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POV.Reactivos.BO;

namespace POV.Prueba.BO
{
    public abstract class ARespuestaOpcionMultiple : ARespuestaAlumno
    {
        private List<OpcionRespuestaPlantilla> listaOpcionesRespuesta;
        /// <summary>
        /// Lista de las opciones seleccionadas
        /// </summary>
        public List<OpcionRespuestaPlantilla> ListaOpcionesRespuesta
        {
            get { return this.listaOpcionesRespuesta; }
            set { listaOpcionesRespuesta = value; }
        }

        public abstract List<OpcionRespuestaPlantilla> ObtenerSelecciones();


        public override object Clone()
        {
            ARespuestaOpcionMultiple nueva = (ARespuestaOpcionMultiple) base.Clone();
            if(this.ListaOpcionesRespuesta!=null)
            {
                nueva.ListaOpcionesRespuesta=new List<OpcionRespuestaPlantilla>();
                foreach (OpcionRespuestaPlantilla opcionRespuestaPlantilla in this.ListaOpcionesRespuesta)
                {
                    nueva.ListaOpcionesRespuesta.Add((OpcionRespuestaPlantilla)opcionRespuestaPlantilla.Clone());
                }
            }
            return nueva;
        }
        
    }
}
