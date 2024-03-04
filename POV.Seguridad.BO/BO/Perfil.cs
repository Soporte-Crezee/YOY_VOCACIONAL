using System;
using System.Text;

namespace POV.Seguridad.BO { 
   /// <summary>
   /// Catalogo de perfiles disponibles
   /// </summary>
    public class Perfil : PrivilegioCompuesto, ICloneable
    {

       private int? perfilID;
       /// <summary>
       /// Identificador autonumerico de Perfil
       /// </summary>
       public int? PerfilID
       {
           get { return this.perfilID; }
           set { this.perfilID = value; }
       }

       private string nombre;
       /// <summary>
       /// Es el nombre descriptivo para el usuario
       /// </summary>
       public string Nombre
       {
           get { return this.nombre; }
           set { this.nombre = value; }
       }
       
       private bool? operaciones;
       /// <summary>
       /// Indica si es un perfil de operaciones
       /// </summary>
       public bool? Operaciones
       {
           get { return this.operaciones; }
           set { this.operaciones = value; }
       }
       private string descripcion;
       /// <summary>
       /// Es el nombre descriptivo para el usuario
       /// </summary>
       public override string Descripcion
       {
           get { return this.descripcion; }
           set { this.descripcion = value; }
       }

       private bool? estatus;
       /// <summary>
       /// Indica si es un perfil de operaciones
       /// </summary>
       public bool? Estatus
       {
           get { return this.estatus; }
           set { this.estatus = value; }
       }

       /// <summary>
       /// Identificador del Privilegio
       /// </summary>
       public override int? PrivilegioID
       {
           get
           {
               if (this.perfilID == null)
                   throw new Exception("Perfil: No tiene ID");
               return (int)this.perfilID;
           }
       }
       /// <summary>
       /// Descripción del Tipo de Privilegio (PERFIL)
       /// </summary>
       public override string TipoPrivilegio
       {
           get { return "PERFIL"; }
       }

       public object Clone()
       {
           return this.MemberwiseClone();
       }
   } 
}
