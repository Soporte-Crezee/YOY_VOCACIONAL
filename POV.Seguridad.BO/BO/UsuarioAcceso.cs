using System;
using System.Collections.Generic;
using System.Text;

namespace POV.Seguridad.BO
{ 
   /// <summary>
   /// Detalle de Permisos asignados al Usuario
   /// </summary>
   public class UsuarioAcceso : ICloneable { 
      private int? usuarioAccesoID;
      /// <summary>
      /// Identificador autonumérico del UsuarioAcceso
      /// </summary>
      public int? UsuarioAccesoID{
         get{ return this.usuarioAccesoID; }
         set{ this.usuarioAccesoID = value; }
      }
      private DateTime? fechaAsignacion;
      /// <summary>
      /// Fecha en la que se asignó el privilegio al Usuario
      /// </summary>
      public DateTime? FechaAsignacion{
         get{ return this.fechaAsignacion; }
         set{ this.fechaAsignacion = value; }
      }
      private bool? estado;
       /// <summary>
       /// 
       /// </summary>
      public bool? Estado
      {
          get { return estado; }
          set { estado = value; }
      }
      private Usuario usuarioAsigno;
      /// <summary>
      /// Usuario que asignó el permiso al Usuario
      /// </summary>
      public Usuario UsuarioAsigno{
         get{ return this.usuarioAsigno; }
         set{ this.usuarioAsigno = value; }
      }
      private IPrivilegio privilegio;
      /// <summary>
      /// Privilegio asignado al Usuario
      /// </summary>
      public IPrivilegio Privilegio{
         get{ return this.privilegio; }
         set{ this.privilegio = value; }
      }

      public object Clone()
      {
          return this.MemberwiseClone();
      }
   } 
}
