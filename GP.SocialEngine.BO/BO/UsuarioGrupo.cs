using System;
using System.Collections.Generic;
using System.Text;

namespace GP.SocialEngine.BO { 
   /// <summary>
   /// UsuarioGrupo
   /// </summary>
   public class UsuarioGrupo : ICloneable { 
      private int? usuarioGrupoID;
      /// <summary>
      /// Identificador autonumérico del UsuarioGrupo
      /// </summary>
      public int? UsuarioGrupoID{
         get{ return this.usuarioGrupoID; }
         set{ this.usuarioGrupoID = value; }
      }
      private DateTime? fechaAsignacion;
      /// <summary>
      /// Fecha de la Asignacion
      /// </summary>
      public DateTime? FechaAsignacion{
         get{ return this.fechaAsignacion; }
         set{ this.fechaAsignacion = value; }
      }
      private bool? estatus;
      /// <summary>
      /// Estatus
      /// </summary>
      public bool? Estatus{
         get{ return this.estatus; }
         set{ this.estatus = value; }
      }
      private bool? esModerador;
      /// <summary>
      /// Estatus
      /// </summary>
      public bool? EsModerador
      {
          get { return this.esModerador; }
          set { this.esModerador = value; }
      }
      private UsuarioSocial usuarioSocial;
      /// <summary>
      /// UsuarioSocial
      /// </summary>
      public UsuarioSocial UsuarioSocial{
         get{ return this.usuarioSocial; }
         set{ this.usuarioSocial = value; }
      }

      public long? DocenteID { get; set; }

       public object Clone()
       {
           UsuarioGrupo copia = (UsuarioGrupo) this.MemberwiseClone();
           copia.UsuarioSocial = (UsuarioSocial) copia.usuarioSocial.Clone();

           return copia;
       }
   } 
}
    