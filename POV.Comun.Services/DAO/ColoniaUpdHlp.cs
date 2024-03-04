// Clase motor de red social
using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Comun.BO;

namespace POV.Comun.DAO { 
   /// <summary>
   /// Actualiza un registro de Colonia en la BD
   /// </summary>
   public class ColoniaUpdHlp { 
      /// <summary>
      /// Actualiza de manera optimista un registro de ColoniaUpdHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="coloniaUpdHlp">ColoniaUpdHlp que tiene los datos nuevos</param>
      /// <param name="anterior">ColoniaUpdHlp que tiene los datos anteriores</param>
      public void Action(IDataContext dctx, Colonia colonia, Colonia anterior){
         object myFirm = new object();
         String sError = string.Empty;
         if (colonia == null)
            sError += ", Colonia";
         if (anterior == null)
            sError += ", Anterior";
         if (sError.Length > 0)
            throw new Exception("ColoniaUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (anterior.ColoniaID == null)
            sError += ", Anterior ColoniaID";
         if (sError.Length > 0)
            throw new Exception("ColoniaUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Comun.DAO", 
         "ColoniaUpdHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "ColoniaUpdHlpUpdHl: Hubo un error al conectarse a la base de datos", "POV.Comun.DAO", 
         "ColoniaUpdHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE Colonia ");
         if (colonia.Nombre == null)
            sCmd.Append(" SET Nombre = NULL ");
         else{ 
            sCmd.Append(" SET Nombre = @colonia_Nombre ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "colonia_Nombre";
            sqlParam.Value = colonia.Nombre;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (colonia.Localidad.LocalidadID == null)
            sCmd.Append(" ,LocalidadID = NULL ");
         else{ 
            sCmd.Append(" ,LocalidadID = @colonia_Localidad_LocalidadID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "colonia_Localidad_LocalidadID";
            sqlParam.Value = colonia.Localidad.LocalidadID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.ColoniaID == null)
            sCmd.Append(" WHERE coloniaID IS NULL ");
         else{ 
            sCmd.Append(" WHERE coloniaID = @anterior_ColoniaID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "anterior_ColoniaID";
            sqlParam.Value = anterior.ColoniaID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("ColoniaUpdHlp: Hubo  un Error al Actualizar el Registro . " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("ColoniaUpdHlp: Hubo  un Error al Actualizar el Registro .");
      }
   } 
}
