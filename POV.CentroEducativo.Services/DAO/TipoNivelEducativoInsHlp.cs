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
   /// Guarda un registro de un Tipo de Nivel Educativo en la BD
   /// </summary>
   public class TipoNivelEducativoInsHlp { 
      /// <summary>
      /// Crea un registro de TipoNivelEducativo en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="tipoNivelEducativo">TipoNivelEducativo que desea crear</param>
      public void Action(IDataContext dctx, TipoNivelEducativo tipoNivelEducativo){
         object myFirm = new object();
         string sError = String.Empty;
         if (tipoNivelEducativo == null)
            sError += ", TipoNivelEducativo";
         if (sError.Length > 0)
            throw new Exception("TipoNivelEducativoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (tipoNivelEducativo.Clave == null)
            sError += ", Clave";
         if (tipoNivelEducativo.Nombre == null)
            sError += ", Nombre";
         if (sError.Length > 0)
            throw new Exception("TipoNivelEducativoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO", 
         "TipoNivelEducativoInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "TipoNivelEducativoInsHlp: No se pudo conectar a la base de datos", "POV.CentroEducativo.DAO", 
         "TipoNivelEducativoInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO TIPONIVELEDUCATIVO(Clave,Nombre) ");
         sCmd.Append(" VALUES ");
         sCmd.Append(" ( ");
         // tipoNivelEducativo.Clave
         sCmd.Append(" @dbp4ram1 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram1";
         if (tipoNivelEducativo.Clave == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = tipoNivelEducativo.Clave;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         // tipoNivelEducativo.Nombre
         sCmd.Append(" ,@dbp4ram2 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram2";
         if (tipoNivelEducativo.Nombre == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = tipoNivelEducativo.Nombre;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ) ");
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("TipoNivelEducativoInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("TipoNivelEducativoInsHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
