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
   /// Guarda un registro de RespuestaPlantillaAbierta en la BD
   /// </summary>
   internal class RespuestaPlantillaAbiertaInsHlp { 
      /// <summary>
      /// Crea un registro de RespuestaPlantillaOpcionMultipe en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="respuestaPlantillaOpcionMultipe">RespuestaPlantillaOpcionMultipe que desea crear</param>
      public void Action(IDataContext dctx, RespuestaPlantillaTexto respuestaPlantilla){
         object myFirm = new object();
         string sError = String.Empty;
         if (respuestaPlantilla == null)
             sError += ", respuestaPlantillaAbierta";
         if (sError.Length > 0)
            throw new Exception("RespuestaPlantillaAbiertaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (respuestaPlantilla.RespuestaPlantillaID == null)
            sError += ", RespuestaPlantillaID";
         if (sError.Length > 0)
            throw new Exception("RespuestaPlantillaAbiertaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (sError.Length > 0)
            throw new Exception("RespuestaPlantillaAbiertaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Reactivos.DAO", 
         "RespuestaPlantillaAbiertaInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "RespuestaPlantillaAbiertaInsHlp: No se pudo conectar a la base de datos", "POV.Reactivos.DAO", 
         "RespuestaPlantillaAbiertaInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO RespuestaPlantillaAbiertaDinamico (RespuestaPlantillaID,Ponderacion,ValorRespuesta,MaximoCaracteres,MinimoCaracteres,");
         sCmd.Append(" EsSensibleMayusculaMinuscula,EsRespuestaCorta, ModeloId, ClasificadorId) ");
         sCmd.Append(" VALUES ");
         sCmd.Append(" ( ");
         // respuestaPlantilla.RespuestaPlantillaID
         sCmd.Append(" @dbp4ram1 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram1";
         if (respuestaPlantilla.RespuestaPlantillaID == null)
             sqlParam.Value = DBNull.Value;
         else
             sqlParam.Value = respuestaPlantilla.RespuestaPlantillaID;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         // respuestaPlantilla.Ponderacion
         sCmd.Append(" ,@dbp4ram2 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram2";
         if (respuestaPlantilla.Ponderacion == null)
             sqlParam.Value = DBNull.Value;
         else
             sqlParam.Value = respuestaPlantilla.Ponderacion;
         sqlParam.DbType = DbType.Decimal;
         sqlCmd.Parameters.Add(sqlParam);
         // respuestaPlantilla.ValorRespuesta
         sCmd.Append(" ,@dbp4ram3 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram3";
         if (respuestaPlantilla.ValorRespuesta == null)
             sqlParam.Value = DBNull.Value;
         else
             sqlParam.Value = respuestaPlantilla.ValorRespuesta;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         // respuestaPlantilla.MaximoCaracteres
         sCmd.Append(" ,@dbp4ram4 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram4";
         if (respuestaPlantilla.MaximoCaracteres == null)
             sqlParam.Value = DBNull.Value;
         else
             sqlParam.Value = respuestaPlantilla.MaximoCaracteres;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         // respuestaPlantilla.MinimoCaracteres
         sCmd.Append(" ,@dbp4ram5 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram5";
         if (respuestaPlantilla.MinimoCaracteres == null)
             sqlParam.Value = DBNull.Value;
         else
             sqlParam.Value = respuestaPlantilla.MinimoCaracteres;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         // respuestaPlantilla.EsSensibleMayusculaMinuscula
         sCmd.Append(" ,@dbp4ram6 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram6";
         if (respuestaPlantilla.EsSensibleMayusculaMinuscula == null)
             sqlParam.Value = DBNull.Value;
         else
             sqlParam.Value = respuestaPlantilla.EsSensibleMayusculaMinuscula;
         sqlParam.DbType = DbType.Boolean;
         sqlCmd.Parameters.Add(sqlParam);
         // respuestaPlantilla.EsRespuestaCorta
         sCmd.Append(" ,@dbp4ram7 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram7";
         if (respuestaPlantilla.EsRespuestaCorta == null)
             sqlParam.Value = DBNull.Value;
         else
             sqlParam.Value = respuestaPlantilla.EsRespuestaCorta;
         sqlParam.DbType = DbType.Boolean;
         sqlCmd.Parameters.Add(sqlParam);
         // respuestaPlantilla.Modelo
         sCmd.Append(" ,@dbp4ram8 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram8";
         if (respuestaPlantilla.Modelo == null)
             sqlParam.Value = DBNull.Value;
         else
             sqlParam.Value = respuestaPlantilla.Modelo.ModeloID;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         // respuestaPlantilla.Clasificador
         sCmd.Append(" ,@dbp4ram9 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram9";
         if (respuestaPlantilla.Clasificador == null)
             sqlParam.Value = DBNull.Value;
         else
             sqlParam.Value = respuestaPlantilla.Clasificador.ClasificadorID;
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
            throw new Exception("RespuestaPlantillaAbiertaInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("RespuestaPlantillaAbiertaInsHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
