using System;
using POV.CentroEducativo.BO;
using GP.SocialEngine.BO;
using POV.Seguridad.BO;

namespace POV.Licencias.BO
{
    /// <summary>
    /// Licencia social.
    /// </summary>
    public abstract class ALicenciaSocial : ALicencia
    {
        private UsuarioSocial usuarioSocial;
        /// <summary>
        /// Usuario social de la licencia.
        /// </summary>
        public UsuarioSocial UsuarioSocial
        {
            get { return this.usuarioSocial; }
            set { this.usuarioSocial = value; }
        }
    }
}
