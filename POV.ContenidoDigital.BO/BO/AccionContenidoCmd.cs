using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.ContenidosDigital.BO
{
    /// <summary>
    /// Clase comando para el calculo de las acciones del contenido digital
    /// </summary>
    public class AccionContenidoCmd
    {

        private List<AVisor> visores;

        private AVisor visorPredeterminado;

        /// <summary>
        /// Obtiene el visor predeterminado
        /// </summary>
        public AVisor VisorPredeterminado
        {
            get { return this.visorPredeterminado; }
        }

        public AccionContenidoCmd(List<AVisor> visores) {
            this.visores = visores;
        }
        /// <summary>
        /// Calcula la accion que se debe llevar acabo para el contenido digital
        /// </summary>
        /// <param name="contenidoDigital">Contenido digital</param>
        /// <returns>Accion</returns>
        public EAccionContenido? CalcularAccion(ContenidoDigital contenidoDigital)
        {
            
                foreach (AVisor visor in ListaVisoresInternos)
                {
                    TipoDocumento tipoDocument = visor.ListaTiposDocumento.FirstOrDefault(item => item.Extension == contenidoDigital.TipoDocumento.Extension);
                    if (tipoDocument != null)
                    {
                        this.visorPredeterminado = visor;
                        return EAccionContenido.REPRODUCIR;
                    }
                }
            
                foreach (AVisor visor in ListaVisoresExternos)
                {
                    TipoDocumento tipoDocument = visor.ListaTiposDocumento.FirstOrDefault(item => item.Fuente == contenidoDigital.TipoDocumento.Fuente);
                    if (tipoDocument != null)
                    {
                        this.visorPredeterminado = visor;
                        return EAccionContenido.REPRODUCIR;
                    }

                }
            

            return null;

        }

        private List<AVisor> ListaVisoresInternos
        {
            get { return this.visores.Where(item => item.EsInterno.Value).ToList(); }
        }

        private List<AVisor> ListaVisoresExternos
        {
            get { return this.visores.Where(item => !item.EsInterno.Value).ToList(); }
        }
    }
}
