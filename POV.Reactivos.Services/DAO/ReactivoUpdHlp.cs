using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Reactivos.BO;
using POV.Modelo.Estandarizado.BO;

namespace POV.Reactivos.DAO { 
   /// <summary>
   /// Actualiza un registro de Reactivo en la BD
   /// </summary>
   internal class ReactivoUpdHlp { 
      /// <summary>
      /// Actualiza de manera optimista un registro de ReactivoUpdHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="reactivoUpdHlp">ReactivoUpdHlp que tiene los datos nuevos</param>
      /// <param name="anterior">ReactivoUpdHlp que tiene los datos anteriores</param>
      public void Action(IDataContext dctx, Reactivo reactivo, Reactivo anterior){
         object myFirm = new object();
         String sError = string.Empty;
         if (reactivo == null)
            sError += ", Reactivo";
         if (anterior == null)
            sError += ", Anterior";
         if (sError.Length > 0)
            throw new Exception("ReactivoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (anterior.ReactivoID == null)
            sError += ", Anterior ReactivoID";
         if (reactivo.TipoReactivo == null)
             sError += ", Tipo Reactivo";
         if (reactivo.Caracteristicas == null)
             sError += ", Caracteristicas";
         if (sError.Length > 0)
            throw new Exception("ReactivoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (reactivo.NombreReactivo == null || reactivo.NombreReactivo.Trim().Length == 0)
             sError += ", NombreReactivo";
         if (reactivo.PlantillaReactivo == null)
             sError += ", PlantillaReactivo";
         if (sError.Length > 0)
             throw new Exception("ReactivoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));

         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Reactivos.DAO", 
         "ReactivoUpdHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
             throw new StandardException(MessageType.Error, "", "ReactivoUpdHlp: Hubo un error al conectarse a la base de datos", "POV.Reactivos.DAO", 
         "ReactivoUpdHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE Reactivo ");
         if (reactivo.NombreReactivo != null){
            sCmd.Append(" SET NombreReactivo = @reactivo_NombreReactivo ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "reactivo_NombreReactivo";
            sqlParam.Value = reactivo.NombreReactivo;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (reactivo.Valor != null)
         {
             sCmd.Append(" ,Valor = @reactivo_Valor ");
             sqlParam = sqlCmd.CreateParameter();
             sqlParam.ParameterName = "reactivo_Valor";
             sqlParam.Value = reactivo.Valor;
             sqlParam.DbType = DbType.Decimal;
             sqlCmd.Parameters.Add(sqlParam);
         }
         if (reactivo.NumeroIntentos != null)
         {
             sCmd.Append(" , NumeroIntentos = @reactivo_NumeroIntentos ");
             sqlParam = sqlCmd.CreateParameter();
             sqlParam.ParameterName = "reactivo_NumeroIntentos";
             sqlParam.Value = reactivo.NumeroIntentos;
             sqlParam.DbType = DbType.Int32;
             sqlCmd.Parameters.Add(sqlParam);
         }
         if (reactivo.PlantillaReactivo != null){
            sCmd.Append(" ,PlantillaReactivo = @reactivo_PlantillaReactivo ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "reactivo_PlantillaReactivo";
            sqlParam.Value = reactivo.PlantillaReactivo;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (reactivo.Descripcion != null)
         {
             sCmd.Append(" ,Descripcion = @reactivo_Descripcion ");
             sqlParam = sqlCmd.CreateParameter();
             sqlParam.ParameterName = "reactivo_Descripcion";
             sqlParam.Value = reactivo.Descripcion;
             sqlParam.DbType = DbType.String;
             sqlCmd.Parameters.Add(sqlParam);
         }
         if (reactivo.Activo != null)
         {
             sCmd.Append(" ,Activo = @reactivo_Activo ");
             sqlParam = sqlCmd.CreateParameter();
             sqlParam.ParameterName = "reactivo_Activo";
             sqlParam.Value = reactivo.Activo;
             sqlParam.DbType = DbType.Boolean;
             sqlCmd.Parameters.Add(sqlParam);
         }
         if (reactivo.PresentacionPlantilla != null)
         {
             sCmd.Append(" ,PresentacionPlantilla=@dbp4ram7 ");
             sqlParam = sqlCmd.CreateParameter();
             sqlParam.ParameterName = "dbp4ram7";
             sqlParam.Value = reactivo.PresentacionPlantilla;
             sqlParam.DbType = DbType.Byte;
             sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.ReactivoID == null)
            sCmd.Append(" WHERE ReactivoID IS NULL ");
         else{ 
            sCmd.Append(" WHERE ReactivoID = @anterior_ReactivoID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "anterior_ReactivoID";
            sqlParam.Value = anterior.ReactivoID;
            sqlParam.DbType = DbType.Guid;
            sqlCmd.Parameters.Add(sqlParam);
         }
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("ReactivoUpdHlp: Hubo  un Error al Actualizar el Registro . " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("ReactivoUpdHlp: Hubo  un Error al Actualizar el Registro .");
      }
   } 
}
