using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.DAO;

namespace POV.CentroEducativo.DAO { 
   /// <summary>
   /// Guarda un registro de un Nivel Educativo en la BD
   /// </summary>
   public class NivelEducativoInsHlp { 
      /// <summary>
      /// Crea un registro de NivelEducativo en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="nivelEducativo">NivelEducativo que desea crear</param>
      public void Action(IDataContext dctx, NivelEducativo nivelEducativo){
         object myFirm = new object();
         string sError = String.Empty;
         if (nivelEducativo == null)
            sError += ", NivelEducativo";
         if (sError.Length > 0)
            throw new Exception("NivelEducativoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (nivelEducativo.Titulo == null)
            sError += ", Titulo";
         if (nivelEducativo.Descripcion == null)
            sError += ", Descripcion";
         if (nivelEducativo.NumeroGrados == null)
            sError += ", NumeroGrados";
         if (nivelEducativo.TipoNivelEducativoID == null)
            sError += ", TipoNivelEducativo";
         if (sError.Length > 0)
            throw new Exception("NivelEducativoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (nivelEducativo.TipoNivelEducativoID.TipoNivelEducativoID == null)
            sError += ", TipoNivelEducativoID";
         if (sError.Length > 0)
            throw new Exception("NivelEducativoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO", 
         "NivelEducativoInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "NivelEducativoInsHlp: No se pudo conectar a la base de datos", "POV.CentroEducativo.DAO", 
         "NivelEducativoInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO NIVELEDUCATIVO(Titulo,Descripcion,NumeroGrados,TipoNivelEducativoID) ");
         sCmd.Append(" VALUES ");
         sCmd.Append(" ( ");
         // nivelEducativo.Titulo
         sCmd.Append(" @dbp4ram1 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram1";
         if (nivelEducativo.Titulo == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = nivelEducativo.Titulo;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         // nivelEducativo.Descripcion
         sCmd.Append(" ,@dbp4ram2 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram2";
         if (nivelEducativo.Descripcion == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = nivelEducativo.Descripcion;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         // nivelEducativo.NumeroGrados
         sCmd.Append(" ,@dbp4ram3 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram3";
         if (nivelEducativo.NumeroGrados == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = nivelEducativo.NumeroGrados;
         sqlParam.DbType = DbType.Int16;
         sqlCmd.Parameters.Add(sqlParam);
         // nivelEducativo.TipoNivelEducativoID.TipoNivelEducativoID
         sCmd.Append(" ,@dbp4ram4 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram4";
         if (nivelEducativo.TipoNivelEducativoID.TipoNivelEducativoID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = nivelEducativo.TipoNivelEducativoID.TipoNivelEducativoID;
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
            throw new Exception("NivelEducativoInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("NivelEducativoInsHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
