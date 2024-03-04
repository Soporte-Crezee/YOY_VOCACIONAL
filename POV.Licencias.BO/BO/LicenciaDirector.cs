using System;
using POV.CentroEducativo.BO;
using GP.SocialEngine.BO;
using POV.Seguridad.BO;

namespace POV.Licencias.BO
{
    /// <summary>
    /// Licencia de director.
    /// </summary>
    public class LicenciaDirector : ALicencia
    {
        /// <summary>
        /// Descontar de las licencias. Valor true para descontar, caso contrario false.
        /// </summary>
        public override bool Descontar
        {
            get { return false; }
        }
        private Director director;
        /// <summary>
        /// Director de la licencia.
        /// </summary>
        public Director Director
        {
            get { return this.director; }
            set { this.director = value; }
        }
        /// <summary>
        /// Tipo de licencia.
        /// </summary>
        public override ETipoLicencia Tipo
        {
            get { return ETipoLicencia.DIRECTOR; }
        }

        public override object Clone()
        {

            LicenciaDirector licencia = (LicenciaDirector)this.MemberwiseClone();
            licencia.Usuario = (Usuario)this.Usuario.Clone();

            return licencia;
        }
    }
}
