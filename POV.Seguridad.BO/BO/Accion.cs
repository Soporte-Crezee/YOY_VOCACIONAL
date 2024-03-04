namespace POV.Seguridad.BO { 
   /// <summary>
   /// Catalogo de Acciones disponibles
   /// </summary>
   public class Accion {
       private int? accionID;
       /// <summary>
       /// Identificador autonumerico de Accion
       /// </summary>
       public int? AccionID
       {
           get { return this.accionID; }
           set { this.accionID = value; }
       }
       private string nombre;
       /// <summary>
       /// Nombre descriptivo de la Accion
       /// </summary>
       public string Nombre
       {
           get { return this.nombre; }
           set { this.nombre = value; }
       }
       private bool? activo;
       /// <summary>
       /// Estatus de la Accion
       /// </summary>
       public bool? Activo
       {
           get { return this.activo; }
           set { this.activo = value; }
       }
   } 
}
