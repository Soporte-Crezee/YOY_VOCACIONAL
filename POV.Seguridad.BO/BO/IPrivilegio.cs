using System;
using System.Collections.Generic;
using System.Text;

namespace POV.Seguridad.BO { 
   /// <summary>
   /// Contrato que todo privilegio debe implementar
   /// </summary>
   public interface IPrivilegio {
      /// <summary>
      /// Identificador del Privilegio
      /// </summary>
      int? PrivilegioID {
         get;
      }
      /// <summary>
      /// Indica el tipo de privilegio del IPrivilegio
      /// </summary>
      string TipoPrivilegio {
         get;
      }
      /// <summary>
      /// Descripción del Privilegio
      /// </summary>
      string Descripcion{
         get;
      }
      /// <summary>
      /// List con todos los Privilegios
      /// </summary>
      List<IPrivilegio> Privilegios{
         get;
      }
      /// <summary>
      /// Agrega un Privilegio al Usuario
      /// </summary>
      void AgregarPrivilegio(IPrivilegio privilegio);
      /// <summary>
      /// Quita un Privilegio al Usuario
      /// </summary>
      void QuitarPrivilegio(IPrivilegio privilegio);
      /// <summary>
      /// Verifica si se tiene el Permiso especificado por el PermisoID
      /// </summary>
      bool VerificarPermisoPorPermisoID(Permiso permiso);
      /// <summary>
      /// Verifica si se tiene el Permiso especificado por el AccionID y AplicacionID
      /// </summary>
      bool VerificarPermisoPorAccionAplicacionID(Permiso permiso);
      /// <summary>
      /// Verifica si se tiene el Permiso especificado por la descripción de Accion y Aplicacion
      /// </summary>
      bool VerificarPermisoPorDescripciones(Permiso permiso);

      bool VerificarPermisoPorDescripcionAplicacion(Permiso permiso);
   } 
}
