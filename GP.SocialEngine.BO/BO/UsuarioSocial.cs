// Clase motor de red social
using System;
using GP.SocialEngine.Interfaces;
namespace GP.SocialEngine.BO
{
    /// <summary>
    /// Catalogo de sistemas disponibles
    /// </summary>
    public class UsuarioSocial : IInvitable, ISocialProfile, ICloneable,IPrivacidad
    {
        private long? usuarioSocialID;
        /// <summary>
        /// Identificador autonumerico de sistema para el usuario social
        /// </summary>
        public long? UsuarioSocialID
        {
            get { return this.usuarioSocialID; }
            set { this.usuarioSocialID = value; }
        }
        private string email;
        /// <summary>
        /// Email del usuario social
        /// </summary>
        public string Email
        {
            get { return this.email; }
            set { this.email = value; }
        }
        private string loginName;
        /// <summary>
        /// Nick del usuario social
        /// </summary>
        public string LoginName
        {
            get { return this.loginName; }
            set { this.loginName = value; }
        }
        private string screenName;
        /// <summary>
        /// Nombre que mostrará el usuario
        /// </summary>
        public string ScreenName
        {
            get { return this.screenName; }
            set { this.screenName = value; }
        }
        private bool? estatus;
        /// <summary>
        /// Estatus del UsuarioSocial
        /// </summary>
        public bool? Estatus
        {
            get { return this.estatus; }
            set { this.estatus = value; }
        }

        private DateTime? fechaNacimiento;
        /// <summary>
        /// Fecha de Nacimiento del usuario.
        /// </summary>
        public DateTime? FechaNacimiento
        {
            get { return this.fechaNacimiento; }
            set { this.fechaNacimiento = value; }
        }

        public string ShortScreenName
        {
            get
            {
                string sScreenName = string.Empty;
                sScreenName += ScreenName;
                if (sScreenName.Length > 19)
                {
                    sScreenName = sScreenName.Substring(0, 19);
                    sScreenName += "...";
                }

                return sScreenName;
            }
            set { }
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
