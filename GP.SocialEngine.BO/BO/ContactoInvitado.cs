namespace GP.SocialEngine.BO { 
   /// <summary>
   /// Representa a una persona invitada a vitada que aun no se registra
   /// </summary>
   public class ContactoInvitado: IInvitable { 
      private long? contactoInvitadoID;
      /// <summary>
      /// Identificador unico de Contacto Invitado
      /// </summary>
      public long? ContactoInvitadoID{
         get{ return this.contactoInvitadoID; }
         set{ this.contactoInvitadoID = value; }
      }
      private string nombreCompleto;
      /// <summary>
      /// Nombre completo del contaco invitado
      /// </summary>
      public string NombreCompleto{
         get{ return this.nombreCompleto; }
         set{ this.nombreCompleto = value; }
      }
      private string correoElectronico;
      /// <summary>
      /// Correo Electronico del Invitado
      /// </summary>
      public string CorreoElectronico{
         get{ return this.correoElectronico; }
         set{ this.correoElectronico = value; }
      }
      private string mensaje;
      /// <summary>
      /// Mensaje que lleva relacionado al contacto
      /// </summary>
      public string Mensaje{
         get{ return this.mensaje; }
         set{ this.mensaje = value; }
      }
   } 
}
