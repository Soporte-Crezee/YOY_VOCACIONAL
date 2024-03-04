using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Comun.BO;
using POV.Comun.DAO;

namespace POV.Comun.Service { 
   /// <summary>
   /// Controlador del objeto Colonia
   /// </summary>
   public class ColoniaCtrl { 
      /// <summary>
      /// Consulta registros de ColoniaRetHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="coloniaRetHlp">ColoniaRetHlp que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de ColoniaRetHlp generada por la consulta</returns>
      public DataSet Retrieve(IDataContext dctx, Colonia colonia){
         ColoniaRetHlp da = new ColoniaRetHlp();
         DataSet ds = da.Action(dctx, colonia);
         return ds;
      }
      /// <summary>
      /// Crea un registro de ColoniaInsHlp en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="coloniaInsHlp">ColoniaInsHlp que desea crear</param>
      public void Insert(IDataContext dctx, Colonia  colonia){
         ColoniaInsHlp da = new ColoniaInsHlp();
         da.Action(dctx,  colonia);
      }
      /// <summary>
      /// Actualiza de manera optimista un registro de ColoniaUpdHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="coloniaUpdHlp">ColoniaUpdHlp que tiene los datos nuevos</param>
      /// <param name="anterior">ColoniaUpdHlp que tiene los datos anteriores</param>
      public void Update(IDataContext dctx, Colonia  colonia, Colonia previous){
         ColoniaUpdHlp da = new ColoniaUpdHlp();
         da.Action(dctx,  colonia, previous);
      }
      /// <summary>
      /// Regresa un resgistro completo de Colonia
      /// </summary>
      public Colonia RetrieveComplete(IDataContext dctx, Colonia  colonia){
          try{
          colonia=LastDataRowToColonia(Retrieve(dctx, new Colonia {ColoniaID = colonia.ColoniaID}));
          return colonia;
          }
          catch(Exception ex){
          throw new Exception(ex.Message);
          }
      }
      /// <summary>
      /// Crea un objeto de Colonia a partir de los datos del último DataRow del DataSet.
      /// </summary>
      /// <param name="ds">El DataSet que contiene la información de Colonia</param>
      /// <returns>Un objeto de Colonia creado a partir de los datos</returns>
      public Colonia LastDataRowToColonia(DataSet ds) {
         if (!ds.Tables.Contains("Colonia"))
            throw new Exception("LastDataRowToColonia: DataSet no tiene la tabla Colonia");
         int index = ds.Tables["Colonia"].Rows.Count;
         if (index < 1)
            throw new Exception("LastDataRowToColonia: El DataSet no tiene filas");
         return this.DataRowToColonia(ds.Tables["Colonia"].Rows[index - 1]);
      }
      /// <summary>
      /// Crea un objeto de Colonia a partir de los datos de un DataRow.
      /// </summary>
      /// <param name="row">El DataRow que contiene la información de Colonia</param>
      /// <returns>Un objeto de Colonia creado a partir de los datos</returns>
      public Colonia DataRowToColonia(DataRow row){
         Colonia colonia = new Colonia();
         if (row.IsNull("ColoniaID"))
            colonia.ColoniaID = null;
         else
            colonia.ColoniaID = (int)Convert.ChangeType(row["ColoniaID"], typeof(int));
         if (row.IsNull("Nombre"))
            colonia.Nombre = null;
         else
            colonia.Nombre = (string)Convert.ChangeType(row["Nombre"], typeof(string));
         if (row.IsNull("FechaRegistro"))
            colonia.FechaRegistro = null;
         else
            colonia.FechaRegistro = (DateTime)Convert.ChangeType(row["FechaRegistro"], typeof(DateTime));
         if (row.IsNull("LocalidadID"))
            colonia.Localidad.LocalidadID = null;
         else
            colonia.Localidad.LocalidadID = (int)Convert.ChangeType(row["LocalidadID"], typeof(int));
         return colonia;
      }
   } 
}
