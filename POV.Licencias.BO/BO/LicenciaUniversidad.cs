using System;
using POV.CentroEducativo.BO;
using GP.SocialEngine.BO;
using POV.Seguridad.BO;

namespace POV.Licencias.BO
{
    /// <summary>
    /// Licencia de tutor.
    /// </summary>
    public class LicenciaUniversidad : ALicencia
    {
        /// <summary>
        /// Descontar de las licencias. Valor true para descontar, caso contrario false.
        /// </summary>
        public override bool Descontar
        {
            get { return false; }
        }
        private Universidad universidad;
        /// <summary>
        /// Universidad de la licencia.
        /// </summary>
        public Universidad Universidad
        {
            get { return this.universidad; }
            set { this.universidad = value; }
        }
        /// <summary>
        /// Tipo de licencia.
        /// </summary>
        public override ETipoLicencia Tipo
        {
            get { return ETipoLicencia.UNIVERSIDAD; }
        }

        public override object Clone()
        {

            LicenciaUniversidad licencia = (LicenciaUniversidad)this.MemberwiseClone();
            licencia.Usuario = (Usuario)this.Usuario.Clone();

            return licencia;
        }
    }
}
