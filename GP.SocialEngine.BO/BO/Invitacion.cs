using System;
using System.Text;
using System.Data;

namespace GP.SocialEngine.BO
{
    /// <summary>
    /// Invitacion Social
    /// </summary>
    public class Invitacion: INotificable
    {
        private Guid? invitacionID;
        /// <summary>
        /// Identificador unico de la invitacion
        /// </summary>
        public Guid? InvitacionID
        {
            get { return this.invitacionID; }
            set { this.invitacionID = value; }
        }
        
        private DateTime? fechaRegistro;
        /// <summary>
        /// Fecha en la cual se registra la Invitacion
        /// </summary>
        public DateTime? FechaRegistro
        {
            get { return this.fechaRegistro; }
            set { this.fechaRegistro = value; }
        }
        private EEstatusInvitacion? estatus;
        /// <summary>
        /// Estatus de la invitacion
        /// </summary>
        public EEstatusInvitacion? Estatus
        {
            get { return this.estatus; }
            set { this.estatus = value; }
        }


        public short? ConvertEstatus
        {
            get { return (short) this.estatus; }
            set { this.estatus = (EEstatusInvitacion)value; }
        }

        
        private bool? esSolicitud;
        /// <summary>
        /// Determina si la invitacion se le envia a Alguien que se encuntra registrado o no.
        /// </summary>
        public bool? EsSolicitud
        {
            get { return this.esSolicitud; }
            set { this.esSolicitud = value; }
        }
        private string saludo;
        /// <summary>
        /// Saludo enviado en la Invitacion
        /// </summary>
        public string Saludo
        {
            get { return this.saludo; }
            set { this.saludo = value; }
        }
        private UsuarioSocial remitente;
        /// <summary>
        /// Persona que Envia la Invitacion
        /// </summary>
        public UsuarioSocial Remitente
        {
            get { return this.remitente; }
            set { this.remitente = value; }
        }
        private IInvitable invitado;
        /// <summary>
        /// Persona a quien se le invita
        /// </summary>
        public IInvitable Invitado
        {
            get { return this.invitado; }
            set { this.invitado = value; }
        }
        public long? InvitadoID
        {
            get
            {
                if (Invitado != null)
                {
                    if ((bool)EsSolicitud)
                        return ((UsuarioSocial)Invitado).UsuarioSocialID;
                    return ((ContactoInvitado)Invitado).ContactoInvitadoID;
                }

                return null;
            }
        }

        public Guid? GUID
        {
            get
            {
                return this.invitacionID;
            }
            set
            {
                this.invitacionID = value;
            }
        }

        public string TextoNotificacion
        {
            get
            {
                if (this.Estatus != null)
                    if (this.Estatus == EEstatusInvitacion.ACEPTADA)
                        return " aceptó tu solicitud de contacto ";
                    else if (this.Estatus == EEstatusInvitacion.PENDIENTE)
                        return " envió una solicitud de contacto ";
                    else 
                        return "";
                else
                    return "";
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
                return "";
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
