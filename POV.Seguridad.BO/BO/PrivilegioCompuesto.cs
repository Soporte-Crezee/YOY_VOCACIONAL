using System;
using System.Collections.Generic;
using System.Text;

namespace POV.Seguridad.BO
{ 
   /// <summary>
   /// Implementación genérica de un Privilegio Compuesto
   /// </summary>
   public abstract class PrivilegioCompuesto : IPrivilegio {
      protected List<IPrivilegio> privilegios = new List<IPrivilegio>();
      protected Dictionary<int?, Permiso> permisos = new Dictionary<int?, Permiso>();
      public virtual string TipoPrivilegio {
          get { return "PERFIL"; }
      }
      public abstract string Descripcion {
         get;
         set;
      }
      public virtual List<IPrivilegio> Privilegios {
         get { return this.privilegios; }
      }
      public abstract int? PrivilegioID {
         get;
      }
      public virtual void AgregarPrivilegio(IPrivilegio privilegio) {
         if (privilegio is Permiso)
            this.permisos.Add(privilegio.PrivilegioID, (Permiso)privilegio);
         this.privilegios.Add(privilegio);
      }
      public virtual void QuitarPrivilegio(IPrivilegio privilegio) {
         if (privilegio is Permiso)
            this.permisos.Remove(privilegio.PrivilegioID);
         this.privilegios.Remove(privilegio);
      }
      public virtual bool VerificarPermisoPorPermisoID(Permiso permiso) {
         if (this.permisos.ContainsKey((int)permiso.PermisoID))
            return true;
         foreach (IPrivilegio priv in this.privilegios) {
             if (priv.TipoPrivilegio.CompareTo("PERFIL") != 0)
               continue;
            if (priv is PrivilegioCompuesto)
               if (((PrivilegioCompuesto)priv).VerificarPermisoPorPermisoID(permiso))
                  return true;
         }
         return false;
      }
      public virtual bool VerificarPermisoPorAccionAplicacionID(Permiso permiso) {
         Permiso p1;
         foreach (IPrivilegio priv in this.privilegios) {
            if (priv is Permiso) {
               p1 = priv as Permiso;
               if (p1.Aplicacion.AplicacionID == permiso.Aplicacion.AplicacionID &&
                   p1.Accion.AccionID == permiso.Accion.AccionID)
                  return true;
            }
            else if (priv.TipoPrivilegio.CompareTo("PERFIL") == 0)
            {
               if (priv is PrivilegioCompuesto)
                  if (((PrivilegioCompuesto)priv).VerificarPermisoPorAccionAplicacionID(permiso))
                     return true;
            }
         }
         return false;
      }
      public virtual bool VerificarPermisoPorDescripciones(Permiso permiso) {
         Permiso p1;
         foreach (IPrivilegio priv in this.privilegios) {
            if (priv is Permiso) {
               p1 = priv as Permiso;
               if (p1.Aplicacion.Nombre.CompareTo(permiso.Aplicacion.Nombre) == 0 &&
                   p1.Accion.Nombre.CompareTo(permiso.Accion.Nombre) == 0)
                  return true;
            } else if (priv.TipoPrivilegio.CompareTo("PERFIL") == 0) {
               if (priv is PrivilegioCompuesto)
                  if (((PrivilegioCompuesto)priv).VerificarPermisoPorDescripciones(permiso))
                     return true;
            }
         }
         return false;
      }

      public virtual bool VerificarPermisoPorDescripcionAplicacion(Permiso permiso)
      {
          Permiso p1;
          foreach (IPrivilegio priv in this.privilegios)
          {
              if (priv is Permiso)
              {
                  p1 = priv as Permiso;
                  if (p1.Aplicacion.Nombre.CompareTo(permiso.Aplicacion.Nombre) == 0)
                      return true;
              }
              else if (priv.TipoPrivilegio.CompareTo("PERFIL") == 0)
              {
                  if (priv is PrivilegioCompuesto)
                      if (((PrivilegioCompuesto)priv).VerificarPermisoPorDescripcionAplicacion(permiso))
                          return true;
              }
          }
          return false;
      }

   } 
}
