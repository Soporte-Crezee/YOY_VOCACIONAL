using System;
using System.Collections.Generic;
using System.Text;

namespace POV.Reactivos.BO
{
    /// <summary>
    /// RespuestaPlantillaOpcionMultiple
    /// </summary>
    public class RespuestaPlantillaOpcionMultiple : RespuestaPlantilla
    {
        private int? numeroSeleccionablesMaximo;
        /// <summary>
        /// Numero de Seleccionables Maximo
        /// </summary>
        public int? NumeroSeleccionablesMaximo
        {
            get { return this.numeroSeleccionablesMaximo; }
            set { this.numeroSeleccionablesMaximo = value; }
        }
        private int? numeroSeleccionablesMinimo;
        /// <summary>
        /// Numero de Seleccionables Minimo
        /// </summary>
        public int? NumeroSeleccionablesMinimo
        {
            get { return this.numeroSeleccionablesMinimo; }
            set { this.numeroSeleccionablesMinimo = value; }
        }
        private EModoSeleccion? modoSeleccion;
        /// <summary>
        /// Modo de seleccion de la RespuestaPlantilla
        /// </summary>
        public EModoSeleccion? ModoSeleccion
        {
            get { return this.modoSeleccion; }
            set { this.modoSeleccion = value; }
        }
        private EPresentacionOpcion? presentacionOpcion;
        /// <summary>
        /// Modo de presentación de la opción
        /// </summary>
        public EPresentacionOpcion? PresentacionOpcion
        {
            get { return this.presentacionOpcion; }
            set { this.presentacionOpcion = value; }
        }

        public short? ToShortModoSeleccion
        {
            get { return (short)this.modoSeleccion; }
            set { this.modoSeleccion = (EModoSeleccion)value; }
        }
        private List<OpcionRespuestaPlantilla> listaOpcionRespuestaPlantilla;
        /// <summary>
        /// opciones de respuesta de la pregunta
        /// </summary>
        public List<OpcionRespuestaPlantilla> ListaOpcionRespuestaPlantilla
        {
            get { return this.listaOpcionRespuestaPlantilla; }
            set { this.listaOpcionRespuestaPlantilla = value; }
        }

        public override object Clone()
        {
            RespuestaPlantillaOpcionMultiple nuevo = new RespuestaPlantillaOpcionMultiple();
            nuevo = (RespuestaPlantillaOpcionMultiple)base.Clone();
            nuevo.NumeroSeleccionablesMaximo = this.NumeroSeleccionablesMaximo;
            nuevo.NumeroSeleccionablesMinimo = this.NumeroSeleccionablesMinimo;
            nuevo.ModoSeleccion = this.ModoSeleccion;
            nuevo.ToShortModoSeleccion = this.ToShortModoSeleccion;

            if (this.ListaOpcionRespuestaPlantilla != null)
            {
                nuevo.ListaOpcionRespuestaPlantilla = new List<OpcionRespuestaPlantilla>();
                foreach (OpcionRespuestaPlantilla aOpcionRespuestaPlantilla in this.ListaOpcionRespuestaPlantilla)
                {
                    nuevo.ListaOpcionRespuestaPlantilla.Add((OpcionRespuestaPlantilla)aOpcionRespuestaPlantilla.Clone());
                }
            }
            return nuevo;
        }

        public OpcionRespuestaPlantilla ObtenerRespuestaCorrecta()
        {
            OpcionRespuestaPlantilla opcion = new OpcionRespuestaPlantilla();

            if (this.listaOpcionRespuestaPlantilla != null)
            {
                foreach (OpcionRespuestaPlantilla opcionRespuesta in this.listaOpcionRespuestaPlantilla)
                {
                    if ((bool)opcionRespuesta.EsOpcionCorrecta)
                    {
                        opcion = opcionRespuesta;
                        break;
                    }
                }
            }
            return opcion;
        }
        /// <summary>
        /// Valida que por lo menos una de las opciones sea correcta.
        /// </summary>
        /// <returns>True si por lo menos una de las opciones es correcta, false de lo contrario</returns>
        public bool ValidarSelecciones()
        {
           
            if (listaOpcionRespuestaPlantilla == null)
                return false;
            if (listaOpcionRespuestaPlantilla.Count <= 0)
                return false;

            if (listaOpcionRespuestaPlantilla.Find(x =>  x.EsOpcionCorrecta.Value==true) == null)
                return false;
            
             return true;
        }
        /// <summary>
        /// Obtiene las opciones correctas
        /// </summary>
        /// <returns>Lista de opciones correctas</returns>
        public List<OpcionRespuestaPlantilla> ObtenerOpcionesCorrectas()
        {
            List<OpcionRespuestaPlantilla> listaReturn = new List<OpcionRespuestaPlantilla>();
            if (this.listaOpcionRespuestaPlantilla != null)
            {
                foreach (OpcionRespuestaPlantilla opcionRespuesta in this.listaOpcionRespuestaPlantilla)
                {
                    if(opcionRespuesta.EsOpcionCorrecta!=null)
                    if ((bool)opcionRespuesta.EsOpcionCorrecta)
                    {
                        listaReturn.Add(opcionRespuesta);
                    }
                }
            }
            return listaReturn;
        }
        /// <summary>
        /// Obtiene las opciones disponibles de la plantilla
        /// </summary>
        /// <returns></returns>
        public List<OpcionRespuestaPlantilla> ObtenerOpcionesDisponibles()
        {
            List<OpcionRespuestaPlantilla> listaReturn = new List<OpcionRespuestaPlantilla>();

            if (this.listaOpcionRespuestaPlantilla != null)
            {
                foreach (OpcionRespuestaPlantilla opcionRespuesta in this.listaOpcionRespuestaPlantilla)
                {
                    if(opcionRespuesta.Activo!=null)
                    if ((bool)opcionRespuesta.Activo)
                    {
                        listaReturn.Add(opcionRespuesta);
                    }
                }
            }
            return listaReturn;
        }
    }
}
