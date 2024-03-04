using System;
using POV.CentroEducativo.BO;
using GP.SocialEngine.BO;
using POV.Seguridad.BO;

namespace POV.Licencias.BO
{
    /// <summary>
    /// Licencia de alumno.
    /// </summary>
    public class LicenciaAlumno : ALicenciaSocial
    {
        /// <summary>
        /// Descontar de las licencias. Valor true para descontar, caso contrario false.
        /// </summary>
        public override bool Descontar
        {
            get { return true; }
        }
        private Alumno alumno;
        /// <summary>
        /// Alumno de la licencia.
        /// </summary>
        public Alumno Alumno
        {
            get { return this.alumno; }
            set { this.alumno = value; }
        }
        /// <summary>
        /// Tipo de licencia.
        /// </summary>
        public override ETipoLicencia Tipo
        {
            get { return ETipoLicencia.ALUMNO; }
        }

        public override object Clone()
        {
            LicenciaAlumno licencia = (LicenciaAlumno)this.MemberwiseClone();
            licencia.Usuario = (Usuario)this.Usuario.Clone();
            licencia.UsuarioSocial = (UsuarioSocial)this.UsuarioSocial.Clone();
            licencia.Alumno = (Alumno) this.alumno.Clone();

            return licencia;
        }
    }
}
