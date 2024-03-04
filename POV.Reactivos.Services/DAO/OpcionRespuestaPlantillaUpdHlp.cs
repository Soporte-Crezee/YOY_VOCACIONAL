using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Reactivos.BO;
using POV.Reactivos.DAO;

namespace POV.Reactivos.DAO { 
   /// <summary>
   /// Actualiza un registro de OpcionRespuestaPlantilla en la BD
   /// </summary>
   public class OpcionRespuestaPlantillaUpdHlp { 
      /// <summary>
      /// Actualiza de manera optimista un registro de OpcionRespuestaPlantillaUpdHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="opcionRespuestaPlantillaUpdHlp">OpcionRespuestaPlantillaUpdHlp que tiene los datos nuevos</param>
      /// <param name="anterior">OpcionRespuestaPlantillaUpdHlp que tiene los datos anteriores</param>
      public void Action(IDataContext dctx, OpcionRespuestaPlantilla opcionRespuestaPlantilla, OpcionRespuestaPlantilla anterior){
         object myFirm = new object();
         String sError = string.Empty;
         if (opcionRespuestaPlantilla == null)
            sError += ", OpcionRespuestaPlantilla";
         if (anterior == null)
            sError += ", Anterior";
         if (sError.Length > 0)
            throw new Exception("OpcionRespuestaPlantillaUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (anterior.OpcionRespuestaPlantillaID == null)
            sError += ", Anterior OpcionRespuestaPlantillaID";
         if (sError.Length > 0)
            throw new Exception("OpcionRespuestaPlantillaUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Reactivos.DAO", 
         "OpcionRespuestaPlantillaUpdHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "OpcionRespuestaPlantillaUpdHlpUpdHl: Hubo un error al conectarse a la base de datos", "POV.Reactivos.DAO", 
         "OpcionRespuestaPlantillaUpdHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE OpcionRespuestaPlantilla ");
         if (opcionRespuestaPlantilla.Texto != null){
            sCmd.Append(" SET Texto = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = opcionRespuestaPlantilla.Texto;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (opcionRespuestaPlantilla.ImagenUrl != null){
            sCmd.Append(" ,ImagenUrl = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = opcionRespuestaPlantilla.ImagenUrl;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (opcionRespuestaPlantilla.EsPredeterminado != null){
            sCmd.Append(" ,EsPredeterminado = @dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = opcionRespuestaPlantilla.EsPredeterminado;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (opcionRespuestaPlantilla.EsOpcionCorrecta != null){
            sCmd.Append(" ,EsOpcionCorrecta = @dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            sqlParam.Value = opcionRespuestaPlantilla.EsOpcionCorrecta;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (opcionRespuestaPlantilla.Activo != null){
            sCmd.Append(" ,Activo = @dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            sqlParam.Value = opcionRespuestaPlantilla.Activo;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.OpcionRespuestaPlantillaID == null)
            sCmd.Append(" WHERE OpcionRespuestaPlantillaID IS NULL ");
         else{ 
            // anterior.OpcionRespuestaPlantillaID
            sCmd.Append(" WHERE OpcionRespuestaPlantillaID = @dbp4ram6 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram6";
            sqlParam.Value = anterior.OpcionRespuestaPlantillaID;
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
            throw new Exception("OpcionRespuestaPlantillaUpdHlp: Hubo  un Error al Actualizar el Registro . " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("OpcionRespuestaPlantillaUpdHlp: Hubo  un Error al Actualizar el Registro .");
      }
   } 
}
