using System;
using POV.CentroEducativo.BO;
using GP.SocialEngine.BO;
using POV.Seguridad.BO;

namespace POV.Licencias.BO
{
    /// <summary>
    /// Licencia de tutor.
    /// </summary>
    public class LicenciaTutor : ALicencia
    {
        /// <summary>
        /// Descontar de las licencias. Valor true para descontar, caso contrario false.
        /// </summary>
        public override bool Descontar
        {
            get { return false; }
        }
        private Tutor tutor;
        /// <summary>
        /// Tutor de la licencia.
        /// </summary>
        public Tutor Tutor
        {
            get { return this.tutor; }
            set { this.tutor = value; }
        }
        /// <summary>
        /// Tipo de licencia.
        /// </summary>
        public override ETipoLicencia Tipo
        {
            get { return ETipoLicencia.TUTOR; }
        }

        public override object Clone()
        {

            LicenciaTutor licencia = (LicenciaTutor)this.MemberwiseClone();
            licencia.Usuario = (Usuario)this.Usuario.Clone();

            return licencia;
        }
    }
}
