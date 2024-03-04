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
   /// Actualiza un registro de CentroComputo en la BD
   /// </summary>
   public class CentroComputoUpdHlp { 
      /// <summary>
      /// Actualiza de manera optimista un registro de CentroComputoUpdHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="centroComputoUpdHlp">CentroComputoUpdHlp que tiene los datos nuevos</param>
      /// <param name="anterior">CentroComputoUpdHlp que tiene los datos anteriores</param>
      public void Action(IDataContext dctx, Escuela escuela, CentroComputo centroComputo, CentroComputo anterior){
         object myFirm = new object();
         String sError = string.Empty;
         if (escuela == null)
            sError += ", Escuela";
         if (sError.Length > 0)
            throw new Exception("CentroComputoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (escuela.EscuelaID == null)
            sError += ", EscuelaID";
         if (sError.Length > 0)
            throw new Exception("CentroComputoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (centroComputo == null)
            sError += ", CentroComputo";
         if (anterior == null)
            sError += ", Anterior";
         if (sError.Length > 0)
            throw new Exception("CentroComputoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (anterior.CentroComputoID == null)
            sError += ", Anterior CentroComputoID";
         if (sError.Length > 0)
            throw new Exception("CentroComputoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO", 
         "CentroComputoUpdHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "CentroComputoUpdHlpUpdHl: Hubo un error al conectarse a la base de datos", "POV.CentroEducativo.DAO", 
         "CentroComputoUpdHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE CentroComputo ");
         if (centroComputo.TieneCentroComputo != null){
            sCmd.Append(" SET TieneCentroComputo = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = centroComputo.TieneCentroComputo;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (centroComputo.NumeroComputadoras != null){
            sCmd.Append(" ,NumeroComputadoras = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = centroComputo.NumeroComputadoras;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (centroComputo.TieneInternet != null){
            sCmd.Append(" ,TieneInternet = @dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = centroComputo.TieneInternet;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (centroComputo.AnchoBanda != null){
            sCmd.Append(" ,AnchoBanda = @dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            sqlParam.Value = centroComputo.AnchoBanda;
            sqlParam.DbType = DbType.Decimal;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (centroComputo.NombreProveedor != null){
            sCmd.Append(" ,NombreProveedor = @dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            sqlParam.Value = centroComputo.NombreProveedor;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (centroComputo.TipoContrato != null){
            sCmd.Append(" ,TipoContrato = @dbp4ram6 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram6";
            sqlParam.Value = centroComputo.TipoContrato;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (centroComputo.Responsable != null){
            sCmd.Append(" ,Responsable = @dbp4ram7 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram7";
            sqlParam.Value = centroComputo.Responsable;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (centroComputo.TelefonoResponsable != null){
            sCmd.Append(" ,TelefonoResponsable = @dbp4ram8 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram8";
            sqlParam.Value = centroComputo.TelefonoResponsable;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (centroComputo.FechaRegistro != null){
            sCmd.Append(" ,FechaRegistro = @dbp4ram9 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram9";
            sqlParam.Value = centroComputo.FechaRegistro;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (centroComputo.Activo != null){
            sCmd.Append(" ,Activo = @dbp4ram10 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram10";
            sqlParam.Value = centroComputo.Activo;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.CentroComputoID == null)
            sCmd.Append(" WHERE centroComputoID IS NULL ");
         else{ 
            // anterior.CentroComputoID
            sCmd.Append(" WHERE centroComputoID = @dbp4ram11 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram11";
            sqlParam.Value = anterior.CentroComputoID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (escuela.EscuelaID == null)
            sCmd.Append(" AND EscuelaID IS NULL ");
         else{ 
            // escuela.EscuelaID
            sCmd.Append(" AND EscuelaID = @dbp4ram12 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram12";
            sqlParam.Value = escuela.EscuelaID;
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
            throw new Exception("CentroComputoUpdHlp: Hubo  un Error al Actualizar el Registro . " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("CentroComputoUpdHlp: Hubo  un Error al Actualizar el Registro .");
      }
   } 
}
