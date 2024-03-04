using System;
using System.Collections.Generic;
using System.Text;

namespace POV.Seguridad.BO { 
   /// <summary>
   /// Catalogo de permisos disponibles
   /// </summary>
    public class Permiso : IPrivilegio
    {

       private int? permisoID;
       /// <summary>
       /// Identificador autonumerico de permiso
       /// </summary>
       public int? PermisoID
       {
           get { return this.permisoID; }
           set { this.permisoID = value; }
       }
       
       private bool? activo;
       /// <summary>
       /// Estatus del permiso
       /// </summary>
       public bool? Activo
       {
           get { return this.activo; }
           set { this.activo = value; }
       }
       private Accion accion;
       /// <summary>
       /// Accion Asociada al permiso
       /// </summary>
       public Accion Accion
       {
           get { return this.accion; }
           set { this.accion = value; }
       }
       private Aplicacion aplicacion;
       /// <summary>
       /// Aplicacion asociada al permiso
       /// </summary>
       public Aplicacion Aplicacion
       {
           get { return this.aplicacion; }
           set { this.aplicacion = value; }
       }

       public int? PrivilegioID
       {
           get { return (int)this.permisoID; }
       }

       public string TipoPrivilegio
       {
           get { return "PERMISO"; }
       }

       public string Descripcion
       {
           get { return this.Aplicacion.Nombre + "." + this.Accion.Nombre; }
       }

       public List<IPrivilegio> Privilegios
       {
           get { throw new NotImplementedException(); }
       }

       public virtual void AgregarPrivilegio(IPrivilegio privilegio)
       {
           throw new NotImplementedException();
       }

       public virtual void QuitarPrivilegio(IPrivilegio privilegio)
       {
           throw new NotImplementedException();
       }
       public virtual bool VerificarPermisoPorPermisoID(Permiso permiso)
       {
           if (this.permisoID == permiso.permisoID)
               return true;
           return false;
       }
       public virtual bool VerificarPermisoPorAccionAplicacionID(Permiso permiso)
       {
           if (this.Aplicacion.AplicacionID == permiso.Aplicacion.AplicacionID &&
               this.Accion.AccionID == permiso.Accion.AccionID)
               return true;
           return false;
       }
       public virtual bool VerificarPermisoPorDescripciones(Permiso permiso)
       {
           if (this.Aplicacion.Nombre.CompareTo(permiso.Aplicacion.Nombre) == 0 &&
               this.Accion.Nombre.CompareTo(permiso.Accion.Nombre) == 0)
               return true;
           return false;
       }
       public virtual bool VerificarPermisoPorDescripcionAplicacion(Permiso permiso)
       {
           if (this.Aplicacion.Nombre.CompareTo(permiso.Aplicacion.Nombre) == 0)
               return true;
           return false;
       }
   } 
}
