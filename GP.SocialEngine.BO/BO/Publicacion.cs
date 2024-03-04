using System;
using System.Collections.Generic;
using System.Text;
using GP.SocialEngine.Interfaces;

namespace GP.SocialEngine.BO
{
    /// <summary>
    /// Publicacion
    /// </summary>
    public class Publicacion : INotificable, ICloneable,IReportable
    {
        private Guid? publicacionID;
        /// <summary>
        /// Identificador autonumÃ©rico de la PublicaciÃ³n
        /// </summary>
        public Guid? PublicacionID
        {
            get { return this.publicacionID; }
            set { this.publicacionID = value; }
        }
        private string contenido;
        /// <summary>
        /// Contenido
        /// </summary>
        public string Contenido
        {
            get { return this.contenido; }
            set { this.contenido = value; }
        }
        private DateTime? fechaPublicacion;
        /// <summary>
        /// Fecha de la PublicaciÃ³n
        /// </summary>
        public DateTime? FechaPublicacion
        {
            get { return this.fechaPublicacion; }
            set { this.fechaPublicacion = value; }
        }
        private bool? estatus;
        /// <summary>
        /// Estatus
        /// </summary>
        public bool? Estatus
        {
            get { return this.estatus; }
            set { this.estatus = value; }
        }
        private Ranking ranking;
        /// <summary>
        /// Ranking
        /// </summary>
        public Ranking Ranking
        {
            get { return this.ranking; }
            set { this.ranking = value; }
        }
        private SocialHub socialHub;
        /// <summary>
        /// SocialHub
        /// </summary>
        public SocialHub SocialHub
        {
            get { return this.socialHub; }
            set { this.socialHub = value; }
        }
        private List<IPrivacidad> privacidades;
        /// <summary>
        /// IPrivacidad
        /// </summary>
        public List<IPrivacidad> Privacidades
        {
            get { return this.privacidades; }
            set { this.privacidades = value; }
        }
        private List<Comentario> comentariosPublicacion;
        /// <summary>
        /// Lista de Comentarios de la publicaciÃ³n
        /// </summary>
        public List<Comentario> ComentariosPublicacion
        {
            get { return this.comentariosPublicacion; }
            set { this.comentariosPublicacion = value; }
        }
        private UsuarioSocial usuarioSocial;
        /// <summary>
        /// UsuarioSocial
        /// </summary>
        public UsuarioSocial UsuarioSocial
        {
            get { return this.usuarioSocial; }
            set { this.usuarioSocial = value; }
        }
        private IAppSocial appSocial;
        /// <summary>
        /// Aplicacion social que se quiere publicar
        /// </summary>
        public IAppSocial AppSocial
        {
            get { return this.appSocial; }
            set { this.appSocial = value; }
        }
        private ETipoPublicacion? tipoPublicacion;
        /// <summary>
        /// ETipoPublicacion
        /// </summary>
        public ETipoPublicacion? TipoPublicacion
        {
            get { return this.tipoPublicacion; }
            set { this.tipoPublicacion = value; }
        }
        public void AddComentario(Comentario comentario)
        {
            ComentariosPublicacion.Add(comentario);
        }
        public void RemoveComentario(Comentario comentario)
        {
            foreach (Comentario comentari in ComentariosPublicacion)
            {
                if (comentario.ComentarioID == comentari.ComentarioID)
                {
                    ComentariosPublicacion.Remove(comentario);
                    break;
                }
            }
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        public string AppKey
        {
            get
            {
                if (this.appSocial != null)
                {
                    return appSocial.GetAppKey();
                }
                return null;
            }
            set { }
        }

        public Guid? GUID
        {
            get
            {
                return this.publicacionID;
            }
            set
            {
                this.publicacionID = value;
            }
        }
        public string TextoNotificacion
        {
            get
            {
                return GP.SocialEngine.Properties.Resources.PublicacionNotificacion;
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        public string URLReferencia
        {
            get
            {
                return "VerPublicacion.aspx";
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
