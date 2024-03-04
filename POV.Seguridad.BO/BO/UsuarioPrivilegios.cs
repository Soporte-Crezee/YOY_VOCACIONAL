using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Linq.Expressions;


namespace POV.Seguridad.BO
{ 
   /// <summary>
   /// Privilegios del usuario en el Sistema
   /// </summary>
   public class UsuarioPrivilegios: ICloneable {
       public UsuarioPrivilegios()
       {
           this.Usuario = new Usuario();
       }

      private int? usuarioPrivilegiosID;
      /// <summary>
      /// Identificador autonumérico del UsuarioPrivilegios
      /// </summary>
      public int? UsuarioPrivilegiosID{
         get{ return this.usuarioPrivilegiosID; }
         set{ this.usuarioPrivilegiosID = value; }
      }
      private DateTime? fechaCreacion;
      /// <summary>
      /// Fecha de creación del UsuarioPrivilegios
      /// </summary>
      public DateTime? FechaCreacion{
         get{ return this.fechaCreacion; }
         set{ this.fechaCreacion = value; }
      }
      private bool? estado;
      /// <summary>
      /// Estado del UsuarioPrivilegios
      /// </summary>
      public bool? Estado{
         get{ return this.estado; }
         set{ this.estado = value; }
      }
      private Usuario usuario;
      /// <summary>
      /// Usuario del cual los UsuarioPrivilegios son
      /// </summary>
      public Usuario Usuario{
         get{ return this.usuario; }
         set{ this.usuario = value; }
      }
      private List<UsuarioAcceso> usuarioAccesos = new List<UsuarioAcceso>();
      /// <summary>
      /// UsuarioAcceso asignados al UsuarioPrivilegios
      /// </summary>
      public List<UsuarioAcceso> UsuarioAccesos{
         get{ return this.usuarioAccesos; }
         set{

             IEnumerable<Permiso> permisos = value.Where(i => i.Privilegio.TipoPrivilegio == "PERMISO").Select(i => i.Privilegio).Cast<Permiso>();
             bool duplicado = false;
             foreach(Permiso usracc in permisos)
             {
                 if (duplicado = permisos.Count(i => i.PermisoID == usracc.PermisoID || (i.Aplicacion.Nombre == usracc.Aplicacion.Nombre && i.Accion.Nombre == usracc.Accion.Nombre)) > 1)
                     break;
             }
             if (duplicado) throw new InvalidOperationException("El usuario tiene duplicado la asignacion de un permiso.");

             this.usuarioAccesos = value;
         }
      }
      /// <summary>
      /// Agrega un UsuarioAcceso a los Privilegios del Usuario
      /// </summary>
      /// <param name="usuarioAcceso">El UsuarioAcceso a agregar</param>
      public void AgregarUsuarioAcceso(UsuarioAcceso usuarioAcceso) {

          this.usuarioAccesos.Add(usuarioAcceso);
      }
      /// <summary>
      /// Quitar un UsuarioAcceso a los Privilegios del Usuario
      /// </summary>
      /// <param name="usuarioAcceso"></param>
      public void QuitarUsuarioAcceso(UsuarioAcceso usuarioAcceso) {
         this.usuarioAccesos.Remove(usuarioAcceso);
      }
      /// <summary>
      /// Elimina el Privilegio del Usuario a través del ID de Permiso
      /// </summary>
      /// <param name="privilegio">Privilegio.PrivilegioID a Eliminar</param>
      /// <param name="tipoPrivilegio">Tipo de Privilegio a Eliminar</param>
      /// <returns>Verdadero si es exitoso, falso en caso contrario</returns>
      public virtual bool EliminarPrivilegioPorPrivilegioID(IPrivilegio privilegio, string tipoPrivilegio) {
         UsuarioAcceso ua1 = null;
         foreach (UsuarioAcceso ua in this.usuarioAccesos) {
            if (ua.Privilegio.TipoPrivilegio.CompareTo(tipoPrivilegio) == 0 &&
                ua.Privilegio.PrivilegioID == privilegio.PrivilegioID)
               ua1 = ua;
         }
         if (ua1 != null) {
            this.usuarioAccesos.Remove(ua1);
            return true;
         }
         return false;
      }
      /// <summary>
      /// Obtiene la lista de perfiles que tiene asignado el usuario
      /// </summary>
      public List<Perfil> Perfiles {
         get {
            List<Perfil> perfiles = new List<Perfil>();
            foreach (UsuarioAcceso ua in this.usuarioAccesos) {
               if (ua.Privilegio.TipoPrivilegio.CompareTo("PERFIL") == 0)
                  perfiles.Add((Perfil)ua.Privilegio);
            }
            return perfiles;
         }
      }
      /// <summary>
      /// Verifica si el Usuario tiene asignado el Perfil que coincida por Descripción
      /// </summary>
      /// <param name="perfil">Perfil.Descripcion a verificar</param>
      /// <returns>Verdadero si es exitoso, falso en caso contrario</returns>
      public virtual bool VerificarPerfilPorDescripcion(Perfil perfil) {
         foreach (UsuarioAcceso ua in this.usuarioAccesos) {
            if (ua.Privilegio.TipoPrivilegio.CompareTo("PERFIL") == 0 &&
                ua.Privilegio.Descripcion.CompareTo(perfil.Descripcion) == 0)
               return true;
         }
         return false;
      }
      /// <summary>
      /// Verifica si el Usuario tiene asignado el Permiso que coincida por Permiso.PermisoID
      /// </summary>
      /// <param name="perfil">Permiso.PermisoID a verificar</param>
      /// <returns>Verdadero si es exitoso, falso en caso contrario</returns>
      public virtual bool VerificarPermisoPorPermisoID(Permiso permiso) {
         foreach (UsuarioAcceso ua in this.usuarioAccesos) {
            if (ua.Privilegio.VerificarPermisoPorPermisoID(permiso))
               return true;
         }
         return false;
      }
      /// <summary>
      /// Verifica si el Usuario tiene asignado el Permiso que coincida 
      /// por Permiso.Accion.AccionID y Permiso.Aplicacion.AplicacionID
      /// </summary>
      /// <param name="perfil">Permiso.Accion.AccionID y Permiso.Aplicacion.AplicacionID a verificar</param>
      /// <returns>Verdadero si es exitoso, falso en caso contrario</returns>
      public virtual bool VerificarPermisoPorAccionAplicacionID(Permiso permiso) {
         foreach (UsuarioAcceso ua in this.usuarioAccesos) {
            if (ua.Privilegio.VerificarPermisoPorAccionAplicacionID(permiso))
               return true;
         }
         return false;
      }
      /// <summary>
      /// Verifica si el Usuario tiene asignado el Permiso que coincida 
      /// por Permiso.Accion.Descripcion y Permiso.Aplicacion.Descripcion
      /// </summary>
      /// <param name="perfil">Permiso.Accion.Descripcion y Permiso.Aplicacion.Descripcion a verificar</param>
      /// <returns>Verdadero si es exitoso, falso en caso contrario</returns>
      public virtual bool VerificarPermisoPorDescripciones(Permiso permiso) {
         foreach (UsuarioAcceso ua in this.usuarioAccesos) {
            if (ua.Privilegio.VerificarPermisoPorDescripciones(permiso))
               return true;
         }
         return false;
      }

      public virtual bool VerificarPermisoPorDescripcionAplicacion(Permiso permiso)
      {
          foreach (UsuarioAcceso ua in this.usuarioAccesos)
          {
              if (ua.Privilegio.VerificarPermisoPorDescripcionAplicacion(permiso))
                  return true;
          }
          return false;
      }

      public virtual ETipoUsuarioPrivilegios Tipo
      {
          get { return ETipoUsuarioPrivilegios.USUARIO_PRIVILEGIOS; }
      }

      public object Clone()
      {
          return this.MemberwiseClone();
      }

      public List<Permiso> GetPermisos()
      {
          List<Permiso> permisos = new List<Permiso>();
          foreach (UsuarioAcceso usAcc in usuarioAccesos)
          {
              int i = 0;
              if (usAcc.Privilegio.TipoPrivilegio == "PERFIL")
              {
                  foreach (Permiso per in usAcc.Privilegio.Privilegios)
                  {
                      permisos.Add(per);
                  }
              }
              else
              {
                  permisos.Add((Permiso)usAcc.Privilegio);
              }
              
          }

          if (permisos.Count > 0)
              return permisos;
          else
              return null;
      }
   } 
}
