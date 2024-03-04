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
   /// Guarda un registro de CentroComputo en la BD
   /// </summary>
   public class CentroComputoInsHlp { 
      /// <summary>
      /// Crea un registro de CentroComputo en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="centroComputo">CentroComputo que desea crear</param>
      public void Action(IDataContext dctx, Escuela escuela, CentroComputo centroComputo){
         object myFirm = new object();
         string sError = String.Empty;
         if (centroComputo == null)
            sError += ", CentroComputo";
         if (sError.Length > 0)
            throw new Exception("CentroComputoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (escuela == null)
            sError += ", Escuela";
         if (sError.Length > 0)
            throw new Exception("CentroComputoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (escuela.EscuelaID == null)
            sError += ", EscuelaID";
         if (sError.Length > 0)
            throw new Exception("CentroComputoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (centroComputo.NumeroComputadoras == null)
            sError += ", NumeroComputadoras";
         if (centroComputo.TieneCentroComputo == null)
            sError += ", TieneCentroComputo";
         if (centroComputo.TieneInternet == null)
            sError += ", TieneInternet";
         if (centroComputo.AnchoBanda == null)
            sError += ", AnchoBanda";
         if (centroComputo.Responsable == null || centroComputo.Responsable.Trim().Length == 0)
            sError += ", Responsable";
         if (centroComputo.TelefonoResponsable == null)
            sError += ", TelefonoResponsable";
         if (centroComputo.FechaRegistro == null)
            sError += ", FechaRegistro";
         if (centroComputo.Activo == null)
            sError += ", Activo";
         if (sError.Length > 0)
            throw new Exception("CentroComputoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO", 
         "CentroComputoInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "CentroComputoInsHlp: No se pudo conectar a la base de datos", "POV.CentroEducativo.DAO", 
         "CentroComputoInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO CentroComputo (TieneCentroComputo, NumeroComputadoras, TieneInternet, AnchoBanda, NombreProveedor, TipoContrato, Responsable, TelefonoResponsable, FechaRegistro, Activo, EscuelaID) ");
         sCmd.Append(" VALUES ");
         sCmd.Append(" ( ");
         // centroComputo.TieneCentroComputo
         sCmd.Append(" @dbp4ram1 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram1";
         if (centroComputo.TieneCentroComputo == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = centroComputo.TieneCentroComputo;
         sqlParam.DbType = DbType.Boolean;
         sqlCmd.Parameters.Add(sqlParam);
         // centroComputo.NumeroComputadoras
         sCmd.Append(" ,@dbp4ram2 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram2";
         if (centroComputo.NumeroComputadoras == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = centroComputo.NumeroComputadoras;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         // centroComputo.TieneInternet
         sCmd.Append(" ,@dbp4ram3 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram3";
         if (centroComputo.TieneInternet == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = centroComputo.TieneInternet;
         sqlParam.DbType = DbType.Boolean;
         sqlCmd.Parameters.Add(sqlParam);
         // centroComputo.AnchoBanda
         sCmd.Append(" ,@dbp4ram4 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram4";
         if (centroComputo.AnchoBanda == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = centroComputo.AnchoBanda;
         sqlParam.DbType = DbType.Decimal;
         sqlCmd.Parameters.Add(sqlParam);
         // centroComputo.NombreProveedor
         sCmd.Append(" ,@dbp4ram5 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram5";
         if (centroComputo.NombreProveedor == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = centroComputo.NombreProveedor;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         // centroComputo.TipoContrato
         sCmd.Append(" ,@dbp4ram6 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram6";
         if (centroComputo.TipoContrato == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = centroComputo.TipoContrato;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         // centroComputo.Responsable
         sCmd.Append(" ,@dbp4ram7 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram7";
         if (centroComputo.Responsable == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = centroComputo.Responsable;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         // centroComputo.TelefonoResponsable
         sCmd.Append(" ,@dbp4ram8 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram8";
         if (centroComputo.TelefonoResponsable == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = centroComputo.TelefonoResponsable;
         sqlParam.DbType = DbType.Int64;
         sqlCmd.Parameters.Add(sqlParam);
         // centroComputo.FechaRegistro
         sCmd.Append(" ,@dbp4ram9 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram9";
         if (centroComputo.FechaRegistro == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = centroComputo.FechaRegistro;
         sqlParam.DbType = DbType.DateTime;
         sqlCmd.Parameters.Add(sqlParam);
         // centroComputo.Activo
         sCmd.Append(" ,@dbp4ram10 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram10";
         if (centroComputo.Activo == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = centroComputo.Activo;
         sqlParam.DbType = DbType.Boolean;
         sqlCmd.Parameters.Add(sqlParam);
         // escuela.EscuelaID
         sCmd.Append(" ,@dbp4ram11 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram11";
         if (escuela.EscuelaID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = escuela.EscuelaID;
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
            throw new Exception("CentroComputoInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("CentroComputoInsHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
