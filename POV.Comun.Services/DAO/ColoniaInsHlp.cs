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
   /// Guarda un registro de Colonia en la BD
   /// </summary>
   public class ColoniaInsHlp { 
      /// <summary>
      /// Crea un registro de Colonia en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="colonia">Colonia que desea crear</param>
      public void Action(IDataContext dctx, Colonia colonia){
         object myFirm = new object();
         string sError = String.Empty;
         if (colonia == null)
            sError += ", Colonia";
         if (sError.Length > 0)
            throw new Exception("ColoniaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (colonia.Localidad == null)
            sError += ", Localidad";
         if (sError.Length > 0)
            throw new Exception("ColoniaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (colonia.Localidad.LocalidadID == null)
            sError += ", LocalidadID";
         if (sError.Length > 0)
            throw new Exception("ColoniaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Comun.DAO", 
         "ColoniaInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "ColoniaInsHlp: No se pudo conectar a la base de datos", "POV.Comun.DAO", 
         "ColoniaInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO Colonia ( Nombre, FechaRegistro, LocalidadID ) ");
         sCmd.Append(" VALUES ");
         sCmd.Append(" ( ");
         sCmd.Append(" @colonia_Nombre ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "colonia_Nombre";
         if (colonia.Nombre == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = colonia.Nombre;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ,@colonia_FechaRegistro ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "colonia_FechaRegistro";
         if (colonia.FechaRegistro == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = colonia.FechaRegistro;
         sqlParam.DbType = DbType.DateTime;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ,@colonia_Localidad_LocalidadID ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "colonia_Localidad_LocalidadID";
         if (colonia.Localidad.LocalidadID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = colonia.Localidad.LocalidadID;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ) ");
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("ColoniaInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("ColoniaInsHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
