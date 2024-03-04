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
   /// Actualiza un cicloEscolar en la base de datos
   /// </summary>
   public class CicloEscolarUpdHlp { 
      /// <summary>
      /// Actualiza de manera optimista un registro de CicloEscolar en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="cicloEscolar">CicloEscolar que tiene los datos nuevos</param>
      /// <param name="anterior">CicloEscolar que tiene los datos anteriores</param>
      public void Action(IDataContext dctx, CicloEscolar cicloEscolar,CicloEscolar anterior){
         object myFirm = new object();
         string sError = String.Empty;
         if (cicloEscolar == null)
            sError += ", CicloEscolar";
         if (anterior == null)
            sError += ", anterior";
         if (sError.Length > 0)
            throw new Exception("CicloEscolarUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (cicloEscolar.CicloEscolarID == null)
            sError += ", CicloEscolarID";
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
         if (sError.Length > 0)
            throw new Exception("CicloEscolarInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (cicloEscolar.UbicacionID.UbicacionID == null)
            sError += ", UbicacionID";
         if (sError.Length > 0)
            throw new Exception("CicloEscolarInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (anterior.CicloEscolarID == null)
            sError += ", Anterior CicloEscolarID";
         if (anterior.Titulo == null || anterior.Titulo.Trim().Length == 0)
            sError += ", Anterior Titulo";
         if (anterior.Descripcion == null || anterior.Descripcion.Trim().Length == 0)
            sError += ", Anterior Descripcion";
         if (anterior.InicioCiclo == null)
            sError += ", Anterior InicioCiclo";
         if (anterior.FinCiclo == null)
            sError += ", Anterior FinCiclo";
         if (anterior.UbicacionID == null)
            sError += ", Anterior Ubicacion";
         if (sError.Length > 0)
            throw new Exception("CicloEscolarInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (anterior.UbicacionID.UbicacionID == null)
            sError += ", Anterior UbicacionID";
         if (sError.Length > 0)
            throw new Exception("CicloEscolarInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
      if (cicloEscolar.CicloEscolarID != anterior.CicloEscolarID) {
         sError = "Los parametros no coinciden";
      }
         if (sError.Length > 0)
            throw new Exception("CicloEscolarUpdHlp: " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO", 
         "CicloEscolarUpdHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "CicloEscolarUpdHlp: Ocurrió un error al conectarse a la base de datos", "POV.CentroEducativo.DAO", 
         "CicloEscolarUpdHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE CICLOESCOLAR ");
         // cicloEscolar.Titulo
         sCmd.Append(" SET TITULO =@dbp4ram1 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram1";
         if (cicloEscolar.Titulo == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = cicloEscolar.Titulo;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         // cicloEscolar.Descripcion
         sCmd.Append(" ,DESCRIPCION =@dbp4ram2 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram2";
         if (cicloEscolar.Descripcion == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = cicloEscolar.Descripcion;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         // cicloEscolar.InicioCiclo
         sCmd.Append(" ,INICIOCICLO =@dbp4ram3 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram3";
         if (cicloEscolar.InicioCiclo == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = cicloEscolar.InicioCiclo;
         sqlParam.DbType = DbType.DateTime;
         sqlCmd.Parameters.Add(sqlParam);
         // cicloEscolar.FinCiclo
         sCmd.Append(" ,FINCICLO =@dbp4ram4 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram4";
         if (cicloEscolar.FinCiclo == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = cicloEscolar.FinCiclo;
         sqlParam.DbType = DbType.DateTime;
         sqlCmd.Parameters.Add(sqlParam);
         // cicloEscolar.UbicacionID.UbicacionID
         sCmd.Append(" ,UBICACIONID =@dbp4ram5 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram5";
         if (cicloEscolar.UbicacionID.UbicacionID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = cicloEscolar.UbicacionID.UbicacionID;
         sqlParam.DbType = DbType.Int64;
         sqlCmd.Parameters.Add(sqlParam);
         // cicloEscolar.Activo
         sCmd.Append(" ,ACTIVO =@dbp4ram6 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram6";
         if (cicloEscolar.Activo == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = cicloEscolar.Activo;
         sqlParam.DbType = DbType.Boolean;
         sqlCmd.Parameters.Add(sqlParam);
         // anterior.CicloEscolarID
         sCmd.Append(" WHERE CICLOESCOLARID =@dbp4ram7 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram7";
         if (anterior.CicloEscolarID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = anterior.CicloEscolarID;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         // anterior.Titulo
         sCmd.Append(" AND TITULO =@dbp4ram8 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram8";
         if (anterior.Titulo == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = anterior.Titulo;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
        
         // anterior.InicioCiclo
         sCmd.Append(" AND INICIOCICLO =@dbp4ram10 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram10";
         if (anterior.InicioCiclo == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = anterior.InicioCiclo;
         sqlParam.DbType = DbType.DateTime;
         sqlCmd.Parameters.Add(sqlParam);
         // anterior.FinCiclo
         sCmd.Append(" AND FINCICLO =@dbp4ram11 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram11";
         if (anterior.FinCiclo == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = anterior.FinCiclo;
         sqlParam.DbType = DbType.DateTime;
         sqlCmd.Parameters.Add(sqlParam);
         if (anterior.Activo == null)
            sCmd.Append(" AND ACTIVO IS NULL ");
         else{ 
            // anterior.Activo
            sCmd.Append(" AND ACTIVO = @dbp4ram12 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram12";
            sqlParam.Value = anterior.Activo;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         // anterior.UbicacionID.UbicacionID
         sCmd.Append(" AND UBICACIONID =@dbp4ram13 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram13";
         if (anterior.UbicacionID.UbicacionID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = anterior.UbicacionID.UbicacionID;
         sqlParam.DbType = DbType.Int64;
         sqlCmd.Parameters.Add(sqlParam);
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("CicloEscolarUpdHlp: Ocurrio un error al actualizar el objeto o fue modificado mientras era editado. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("CicloEscolarUpdHlp: Ocurrio un error al actualizar el objeto o fue modificado mientras era editado.");
      }
   } 
}
