// DAO de sistema, para implementacion
using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;

namespace POV.CentroEducativo.DAO { 
   /// <summary>
   /// Inserta objeto ciclo escolar en la base de datos
   /// </summary>
   public class CicloEscolarInsHlp { 
      /// <summary>
      /// Crea un registro de CicloEscolar en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="cicloEscolar">CicloEscolar que desea crear</param>
      public void Action(IDataContext dctx, CicloEscolar cicloEscolar){
         object myFirm = new object();
         string sError = String.Empty;
         if (cicloEscolar == null)
            sError += ", CicloEscolar";
         if (sError.Length > 0)
            throw new Exception("CicloEscolarInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (cicloEscolar.Titulo == null || cicloEscolar.Titulo.Trim().Length == 0)
            sError += ", Titulo";
         if (cicloEscolar.Descripcion == null || cicloEscolar.Descripcion.Trim().Length == 0)
            sError += ", Descripcion";
         if (cicloEscolar.InicioCiclo == null)
            sError += ", InicioCiclo";
         if (cicloEscolar.FinCiclo == null)
            sError += ", FinCiclo";
         if (cicloEscolar.UbicacionID == null)
            sError += ", Ubicacion";
         if (cicloEscolar.Activo == null)
            sError += ", Activo";
         if (sError.Length > 0)
            throw new Exception("CicloEscolarInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (cicloEscolar.UbicacionID.UbicacionID == null)
            sError += ", UbicacionID";
         if (sError.Length > 0)
            throw new Exception("CicloEscolarInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (sError.Length > 0)
            throw new Exception("CicloEscolarInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO", 
         "CicloEscolarInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "CicloEscolarInsHlp: No se pudo conectar a la base de datos", "POV.CentroEducativo.DAO", 
         "CicloEscolarInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO CICLOESCOLAR (TITULO,DESCRIPCION,INICIOCICLO,FINCICLO,UBICACIONID,ACTIVO) ");
         sCmd.Append(" VALUES ");
         sCmd.Append(" ( ");
         // cicloEscolar.Titulo
         sCmd.Append(" @dbp4ram1 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram1";
         if (cicloEscolar.Titulo == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = cicloEscolar.Titulo;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         // cicloEscolar.Descripcion
         sCmd.Append(" ,@dbp4ram2 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram2";
         if (cicloEscolar.Descripcion == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = cicloEscolar.Descripcion;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         // cicloEscolar.InicioCiclo
         sCmd.Append(" ,@dbp4ram3 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram3";
         if (cicloEscolar.InicioCiclo == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = cicloEscolar.InicioCiclo;
         sqlParam.DbType = DbType.DateTime;
         sqlCmd.Parameters.Add(sqlParam);
         // cicloEscolar.FinCiclo
         sCmd.Append(" ,@dbp4ram4 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram4";
         if (cicloEscolar.FinCiclo == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = cicloEscolar.FinCiclo;
         sqlParam.DbType = DbType.DateTime;
         sqlCmd.Parameters.Add(sqlParam);
         // cicloEscolar.UbicacionID.UbicacionID
         sCmd.Append(" ,@dbp4ram5 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram5";
         if (cicloEscolar.UbicacionID.UbicacionID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = cicloEscolar.UbicacionID.UbicacionID;
         sqlParam.DbType = DbType.Int64;
         sqlCmd.Parameters.Add(sqlParam);
         // cicloEscolar.Activo
         sCmd.Append(" ,@dbp4ram6 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram6";
         if (cicloEscolar.Activo == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = cicloEscolar.Activo;
         sqlParam.DbType = DbType.Boolean;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ) ");
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("CicloEscolarInsHlp: Ocurrio un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("CicloEscolarInsHlp: Ocurrio un error al ingresar el registro.");
      }
   } 
}
