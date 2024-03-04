using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Framework.Base.DataAccess;
using POV.Comun.BO;
using POV.Comun.DAO;

namespace POV.Comun.Service { 
   /// <summary>
   /// Servicios para Acceder a los Paises
   /// </summary>
   public class PaisCtrl { 
      /// <summary>
      /// Crea un registro de País en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="país">País que desea crear</param>
      public void Insert(IDataContext dctx, Pais pais){
          try
          {
              if (!ValidateDataForInsert(dctx, new Pais { Nombre = pais.Nombre },pais))
              {
                  throw new DuplicateNameException("El nombre del País ya se encuentra configurado en el sistema, por favor verifique.");
              }
              else
              {
                  PaisInsHlp da = new PaisInsHlp();
                  da.Action(dctx, pais);
              }
          }
          catch (Exception ex)
          {
              throw new DuplicateNameException(ex.Message);
          }
      }
      /// <summary>
      /// Consulta registros de País en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="país">País que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de País generada por la consulta</returns>
      public DataSet Retrieve(IDataContext dctx, Pais pais){
         PaisRetHlp da = new PaisRetHlp();
         DataSet ds = da.Action(dctx, pais);
         return ds;
      }
      /// <summary>
      /// Actualiza de manera optimista un registro de País en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="país">País que tiene los datos nuevos</param>
      /// <param name="anterior">País que tiene los datos anteriores</param>
      public void Update(IDataContext dctx, Pais pais, Pais previous){
          try
          {
              if (!ValidateDataForInsert(dctx, new Pais { Nombre = pais.Nombre },previous))
              {
                  throw new DuplicateNameException("El nombre del País ya se encuentra configurado en el sistema, por favor verifique.");
              }
              else
              {
                  PaisUpdHlp da = new PaisUpdHlp();
                  da.Action(dctx, pais, previous);
              }
          }
          catch (Exception ex)
          {
              throw new DuplicateNameException(ex.Message);
          }
      }
      /// <summary>
      /// Elimina un registro de País en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="pais">País que desea eliminar</param>
      public void Delete(IDataContext dctx, Pais pais){
         PaisDelHlp da = new PaisDelHlp();
         da.Action(dctx, pais);
      }
      /// <summary>
      /// Crea un objeto de País a partir de los datos del último DataRow del DataSet.
      /// </summary>
      /// <param name="ds">El DataSet que contiene la información de País</param>
      /// <returns>Un objeto de País creado a partir de los datos</returns>
      public Pais LastDataRowToPais(DataSet ds) {
         if (!ds.Tables.Contains("Pais"))
            throw new Exception("LastDataRowToPais: DataSet no tiene la tabla Pais");
         int index = ds.Tables["Pais"].Rows.Count;
         if (index < 1)
            throw new Exception("LastDataRowToPais: El DataSet no tiene filas");
         return this.DataRowToPais(ds.Tables["Pais"].Rows[index - 1]);
      }
      /// <summary>
      /// Crea un objeto de País a partir de los datos de un DataRow.
      /// </summary>
      /// <param name="row">El DataRow que contiene la información de País</param>
      /// <returns>Un objeto de País creado a partir de los datos</returns>
      public Pais DataRowToPais(DataRow row){
         Pais pais = new Pais();
         if (row.IsNull("PaisID"))
            pais.PaisID = null;
         else
            pais.PaisID = (int)Convert.ChangeType(row["PaisID"], typeof(int));
         if (row.IsNull("Nombre"))
            pais.Nombre = null;
         else
            pais.Nombre = (string)Convert.ChangeType(row["Nombre"], typeof(string));
         if (row.IsNull("FechaRegistro"))
            pais.FechaRegistro = null;
         else
            pais.FechaRegistro = (DateTime)Convert.ChangeType(row["FechaRegistro"], typeof(DateTime));
         if (row.IsNull("Codigo"))
             pais.Codigo = null;
         else
             pais.Codigo = (string)Convert.ChangeType(row["Codigo"], typeof(string));
         return pais;
      }
      /// <summary>
      /// Valida si el Pais a insertar ya esta configurado en el sistema
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="pais">Pais a insertar</param>
      /// <returns>True si no existe el Pais</returns>
      private bool ValidateDataForInsert(IDataContext dctx, Pais pais, Pais previous)
      {
          DataSet ds = this.Retrieve(dctx, pais);
          if (ds.Tables["Pais"].Rows.Count > 0)
          {
              if (previous.PaisID == null)
                return false;
              Pais paisDB = LastDataRowToPais(ds);
              if (paisDB.PaisID != previous.PaisID)
                  return false;
          }
          return true;
      }
   } 
}
