using System;
using System.Collections.Generic;
using System.Text;
using GP.SocialEngine.Interfaces;
using POV.Reactivos.BO;

namespace GP.SocialEngine.BO
{
    /// <summary>
    /// Social Hub
    /// </summary>
    public class SocialHub
    {
        private long? socialHubID;
        /// <summary>
        /// Identificador autonumÃ©rico del SocialHub
        /// </summary>
        public long? SocialHubID
        {
            get { return this.socialHubID; }
            set { this.socialHubID = value; }
        }
        private ISocialProfile socialProfile;
        /// <summary>
        /// SocialProfile
        /// </summary>
        public ISocialProfile SocialProfile
        {
            get { return this.socialProfile; }
            set { this.socialProfile = value; }
        }
        private DateTime? fechaRegistro;
        /// <summary>
        /// Fecha de Registro
        /// </summary>
        public DateTime? FechaRegistro
        {
            get { return this.fechaRegistro; }
            set { this.fechaRegistro = value; }
        }
        private InformacionSocial informacionSocial;
        /// <summary>
        /// InformaciÃ³n Social
        /// </summary>
        public InformacionSocial InformacionSocial
        {
            get { return this.informacionSocial; }
            set { this.informacionSocial = value; }
        }
        private string alias;
        /// <summary>
        /// Alias
        /// </summary>
        public string Alias
        {
            get { return this.alias; }
            set { this.alias = value; }
        }
        private List<GrupoSocial> listaGrupoSocial;
        /// <summary>
        /// Lista de GrupoSocial
        /// </summary>
        public List<GrupoSocial> ListaGrupoSocial
        {
            get { return this.listaGrupoSocial; }
            set { this.listaGrupoSocial = value; }
        }
        private List<AppSuscripcion> listaAppSuscripcion;
        /// <summary>
        /// Lista de Applicaciones
        /// </summary>
        public List<AppSuscripcion> ListaAppSuscripcion
        {
            get { return this.listaAppSuscripcion; }
            set { this.listaAppSuscripcion = value; }
        }
        private ESocialProfileType? socialProfileType;
        /// <summary>
        /// Tipo de Social Profile que implementa
        /// </summary>
        public ESocialProfileType? SocialProfileType
        {
            get { return this.socialProfileType; }
            set { this.socialProfileType = value; }
        }
        public short? ToShortSocialProfileType
        {
            get { return (short)this.socialProfileType; }
            set { this.socialProfileType = (ESocialProfileType)value; }
        }
        public Object SocialProfileID
        {
            get
            {
                switch (SocialProfileType)
                {
                    case ESocialProfileType.REACTIVO:
                        return ((Reactivo)this.socialProfile).ReactivoID;
                    case ESocialProfileType.USUARIOSOCIAL:
                        return ((UsuarioSocial)this.socialProfile).UsuarioSocialID;
                }
                return null;
            }
            set
            {
            }
        }
    }
}
