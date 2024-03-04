using System;
using System.Collections.Generic;
using System.Text;

namespace GP.SocialEngine.BO { 
   /// <summary>
   /// GrupoSocial
   /// </summary>
   public class GrupoSocial: IPrivacidad { 
      private long? grupoSocialID;
      /// <summary>
      /// Identificador autonumérico del GrupoSocial
      /// </summary>
      public long? GrupoSocialID{
         get{ return this.grupoSocialID; }
         set{ this.grupoSocialID = value; }
      }
      private string nombre;
      /// <summary>
      /// Nombre
      /// </summary>
      public string Nombre{
         get{ return this.nombre; }
         set{ this.nombre = value; }
      }
      private string descripcion;
      /// <summary>
      /// Descrición
      /// </summary>
      public string Descripcion{
         get{ return this.descripcion; }
         set{ this.descripcion = value; }
      }
      private Guid? grupoSocialGuid;
      /// <summary>
      /// Descrición
      /// </summary>
      public Guid? GrupoSocialGuid
      {
          get { return this.grupoSocialGuid; }
          set { this.grupoSocialGuid = value; }
      }
      private DateTime? fechaCreacion;
      /// <summary>
      /// Fecha de Cracionón
      /// </summary>
      public DateTime? FechaCreacion{
         get{ return this.fechaCreacion; }
         set{ this.fechaCreacion = value; }
      }
      private int? numeroMiembros;
      /// <summary>
      /// Número de Miembros
      /// </summary>
      public  int? NumeroMiembros{
         get{ return this.numeroMiembros; }
         set{ this.numeroMiembros = value; }
      }
      private List<UsuarioGrupo> listaUsuarioGrupo;
      /// <summary>
      /// Lista de UsuarioGrupo
      /// </summary>
      public List<UsuarioGrupo> ListaUsuarioGrupo{
         get{ return this.listaUsuarioGrupo; }
         set{ this.listaUsuarioGrupo = value; }
      }

      private ETipoGrupoSocial? tipoGrupoSocial;

      public ETipoGrupoSocial? TipoGrupoSocial
      {
          get { return this.tipoGrupoSocial; }
          set { this.tipoGrupoSocial = value;}
      }
      public short? ToShortTipoGrupoSocial
      {
          get { return (short)this.tipoGrupoSocial; }
          set { this.tipoGrupoSocial = (ETipoGrupoSocial)value; }
      }
   } 
}
