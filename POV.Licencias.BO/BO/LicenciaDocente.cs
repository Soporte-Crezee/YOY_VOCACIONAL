using System;
using POV.CentroEducativo.BO;
using GP.SocialEngine.BO;
using POV.Seguridad.BO;

namespace POV.Licencias.BO
{
    /// <summary>
    /// Licencia de docente.
    /// </summary>
    public class LicenciaDocente : ALicenciaSocial
    {
        /// <summary>
        /// Descontar de las licencias. Valor true para descontar, caso contrario false.
        /// </summary>
        public override bool Descontar
        {
            get { return false; }
        }
        private Docente docente;
        /// <summary>
        /// Alumno de la licencia.
        /// </summary>
        public Docente Docente
        {
            get { return this.docente; }
            set { this.docente = value; }
        }
        /// <summary>
        /// Tipo de licencia.
        /// </summary>
        public override ETipoLicencia Tipo
        {
            get { return ETipoLicencia.DOCENTE; }
        }

        public override object Clone()
        {
            LicenciaDocente licencia = (LicenciaDocente)this.MemberwiseClone();
            licencia.Usuario = (Usuario)this.Usuario.Clone();
            licencia.UsuarioSocial = (UsuarioSocial)this.UsuarioSocial.Clone();
            licencia.Docente = (Docente)this.Docente.Clone();

            return licencia;
        }
    }
}
