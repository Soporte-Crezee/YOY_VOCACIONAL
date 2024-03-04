using System;
using System.Data;
using Framework.Base.DataAccess;
using POV.ContenidosDigital.Busqueda.BO;
using POV.ContenidosDigital.Busqueda.DAO;

namespace POV.ContenidosDigital.Busqueda.Service 
{ 
   /// <summary>
   /// Controlador del objeto PalabraClave
   /// </summary>
   public class PalabraClaveCtrl 
   { 
      /// <summary>
      /// Consulta registros de PalabraClave en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="palabraClave">PalabraClave que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de PalabraClave generada por la consulta</returns>
      public DataSet Retrieve(IDataContext dctx, PalabraClave palabraClave)
      {
         PalabraClaveRetHlp da = new PalabraClaveRetHlp();
         DataSet ds = da.Action(dctx, palabraClave);
         return ds;
      }

      /// <summary>
      /// Crea un registro de PalabraClave en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="palabraClave">PalabraClave que desea crear</param>
      public void Insert(IDataContext dctx, PalabraClave palabraClave)
      {
         PalabraClaveInsHlp da = new PalabraClaveInsHlp();
         da.Action(dctx, palabraClave);
      }

      /// <summary>
      /// Actualiza de manera optimista un registro de PalabraClave en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="palabraClave">PalabraClave que tiene los datos nuevos</param>
      /// <param name="anterior">PalabraClave que tiene los datos anteriores</param>
      public void Update(IDataContext dctx, PalabraClave palabraClave, PalabraClave previous)
      {
         //PalabraClaveUpdHlp da = new PalabraClaveUpdHlp();
         //da.Action(dctx, palabraClave, previous);
      }
      
      /// <summary>
      /// Elimina un registro de PalabraClave en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="palabraClave">PalabraClave que desea eliminar</param>
      public void Delete(IDataContext dctx, PalabraClave palabraClave)
      {
          PalabraClaveDelHlp da = new PalabraClaveDelHlp();
          da.Action(dctx, palabraClave);
      }
      
       /// <summary>
      /// Crea un objeto de PalabraClave a partir de los datos del último DataRow del DataSet.
      /// </summary>
      /// <param name="ds">El DataSet que contiene la información de PalabraClave</param>
      /// <returns>Un objeto de PalabraClave creado a partir de los datos</returns>
      public PalabraClave LastDataRowToPalabraClave(DataSet ds) {
         if (!ds.Tables.Contains("PalabraClave"))
            throw new Exception("LastDataRowToPalabraClave: DataSet no tiene la tabla PalabraClave");
         int index = ds.Tables["PalabraClave"].Rows.Count;
         if (index < 1)
            throw new Exception("LastDataRowToPalabraClave: El DataSet no tiene filas");
         return this.DataRowToPalabraClave(ds.Tables["PalabraClave"].Rows[index - 1]);
      }
      /// <summary>
      /// Crea un objeto de PalabraClave a partir de los datos de un DataRow.
      /// </summary>
      /// <param name="row">El DataRow que contiene la información de PalabraClave</param>
      /// <returns>Un objeto de PalabraClave creado a partir de los datos</returns>
      public PalabraClave DataRowToPalabraClave(DataRow row){
         PalabraClave palabraClave = new PalabraClave();
         if (row.IsNull("PalabraClaveID"))
            palabraClave.PalabraClaveID = null;
         else
            palabraClave.PalabraClaveID = (Int64)Convert.ChangeType(row["PalabraClaveID"], typeof(Int64));
         if (row.IsNull("Tag"))
            palabraClave.Tag = null;
         else
            palabraClave.Tag = (String)Convert.ChangeType(row["Tag"], typeof(String));
         if (row.IsNull("TipoPalabraClave"))
            palabraClave.TipoPalabraClave = null;
         else
            palabraClave.TipoPalabraClave = (ETipoPalabraClave)Convert.ChangeType(row["TipoPalabraClave"], typeof(Byte));
         return palabraClave;
      }
   } 
}
