using POV.CentroEducativo.BO;
using System;
using System.Collections.Generic;

namespace POV.Seguridad.BO { 
   /// <summary>
   /// Catalogo de sistemas disponibles
   /// </summary>
   public class Usuario:  ICloneable { 
      private int? usuarioID;
      /// <summary>
      /// Identificador autonumerico de sistema
      /// </summary>
      public int? UsuarioID{
         get{ return this.usuarioID; }
         set{ this.usuarioID = value; }
      }
      
      private string nombreUsuario;
      /// <summary>
      /// Nombre descriptivo del sistema
      /// </summary>
      public string NombreUsuario{
         get{ return this.nombreUsuario; }
         set{ this.nombreUsuario = value; }
      }
      private byte[] password;
      /// <summary>
      /// Password del usuario
      /// </summary>
      public byte[] Password{
         get{ return this.password; }
         set{ this.password = value; }
      }
      private bool? esActivo;
      /// <summary>
      /// Estado del activo
      /// </summary>
      public bool? EsActivo{
         get{ return this.esActivo; }
         set{ this.esActivo = value; }
      }
      private DateTime? fechaCreacion;
      /// <summary>
      /// Fecha de creacion del usuario
      /// </summary>
      public DateTime? FechaCreacion{
         get{ return this.fechaCreacion; }
         set{ this.fechaCreacion = value; }
      }
      private DateTime? fechaUltimoAcceso;
      /// <summary>
      /// Fecha de ultimo acceso al sistema
      /// </summary>
      public DateTime? FechaUltimoAcceso{
         get{ return this.fechaUltimoAcceso; }
         set{ this.fechaUltimoAcceso = value; }
      }
      private DateTime? fechaUltimoCambioPassword;
      /// <summary>
      /// Fecha de ultimo cambio de password
      /// </summary>
      public DateTime? FechaUltimoCambioPassword{
         get{ return this.fechaUltimoCambioPassword; }
         set{ this.fechaUltimoCambioPassword = value; }
      }
      private string comentario;
      /// <summary>
      /// Comentario adicional
      /// </summary>
      public string Comentario{
         get{ return this.comentario; }
         set{ this.comentario = value; }
      }
      private bool? passwordTemp;
      /// <summary>
      /// Indica uso de password temporal
      /// </summary>
      public bool? PasswordTemp{
         get{ return this.passwordTemp; }
         set{ this.passwordTemp = value; }
      }
      private string email;
      /// <summary>
      /// Email delm usuario
      /// </summary>
      public string Email{
         get{ return this.email; }
         set{ this.email = value; }
      }
      private bool? emailVerificado;
      /// <summary>
      /// Email verificado
      /// </summary>
      public bool? EmailVerificado{
         get{ return this.emailVerificado; }
         set{ this.emailVerificado = value; }
      }
      private string emailAlternativo;
      /// <summary>
      /// Email alternativo
      /// </summary>
      public string EmailAlternativo
      {
          get { return this.emailAlternativo; }
          set { this.emailAlternativo = value; }
      }
      private string telefonoReferencia;
      /// <summary>
      /// Telefono referencia
      /// </summary>
      public string TelefonoReferencia
      {
          get { return this.telefonoReferencia; }
          set { this.telefonoReferencia = value; }
      }

      private string telefonoCasa;
      /// <summary>
      /// Telefono casa
      /// </summary>
      public string TelefonoCasa
      {
          get { return this.telefonoCasa; }
          set { this.telefonoCasa = value; }
      }

      private bool? aceptoTerminos;
      /// <summary>
      /// Terminos aceptados
      /// </summary>
      public bool? AceptoTerminos{
         get{ return this.aceptoTerminos; }
         set{ this.aceptoTerminos = value; }
      }
      private GP.SocialEngine.BO.Termino termino;
      /// <summary>
      /// La aceptacion del usuario con la red
      /// </summary>
      public GP.SocialEngine.BO.Termino Termino{
         get{ return this.termino; }
         set{ this.termino = value; }
      }
       
      public virtual Universidad Universidad { get; set; }
      public long? UniversidadId { get; set; }

      public virtual List<ConfigCalendar> ConfigCalendar { get; set; }

      public object Clone ( )
      {
          return this.MemberwiseClone( );
      }

       
   } 
}
