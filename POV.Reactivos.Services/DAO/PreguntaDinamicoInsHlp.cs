using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Reactivos.BO;
using POV.Modelo.BO;

namespace POV.Reactivos.DAO { 
   /// <summary>
   /// Guarda un registro de Pregunta en la BD
   /// </summary>
   internal class PreguntaDinamicoInsHlp { 
      /// <summary>
      /// Crea un registro de Pregunta en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="pregunta">Pregunta que desea crear</param>
      public void Action(IDataContext dctx, Reactivo reactivo, Pregunta pregunta){
         object myFirm = new object();
         string sError = String.Empty;
         if (pregunta == null)
            sError += ", Pregunta";
         if (sError.Length > 0)
            throw new Exception("PreguntaDinamicoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (reactivo == null)
            sError += ", Reactivo";
         if (sError.Length > 0)
            throw new Exception("PreguntaDinamicoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (reactivo.ReactivoID == null)
            sError += ", ReactivoID";
         if (sError.Length > 0)
            throw new Exception("PreguntaDinamicoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (pregunta.Orden == null)
            sError += ", Orden";
         if (pregunta.TextoPregunta == null || pregunta.TextoPregunta.Trim().Length == 0)
            sError += ", TextoPregunta";
         if (pregunta.FechaRegistro == null)
            sError += ", FechaRegistro";
         if (pregunta.Activo == null)
            sError += ", Activo";
         if (pregunta.PresentacionPlantilla == null)
            sError += ", PresentacionPlantilla";
         if (sError.Length > 0)
            throw new Exception("PreguntaDinamicoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Reactivos.DAO", 
         "PreguntaDinamicoInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "PreguntaDinamicoInsHlp: No se pudo conectar a la base de datos", "POV.Reactivos.DAO", 
         "PreguntaDinamicoInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO PreguntaDinamico (ReactivoID, Orden, TextoPregunta, PlantillaPregunta, FechaRegistro, Valor, PuedeOmitir, Activo, PresentacionPlantilla) ");
         sCmd.Append(" VALUES ");
         sCmd.Append(" ( ");
         // reactivo.ReactivoID
         sCmd.Append(" @dbp4ram1 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram1";
         if (reactivo.ReactivoID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = reactivo.ReactivoID;
         sqlParam.DbType = DbType.Guid;
         sqlCmd.Parameters.Add(sqlParam);
         // pregunta.Orden
         sCmd.Append(" ,@dbp4ram2 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram2";
         if (pregunta.Orden == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = pregunta.Orden;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         // pregunta.TextoPregunta
         sCmd.Append(" ,@dbp4ram3 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram3";
         if (pregunta.TextoPregunta == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = pregunta.TextoPregunta;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         // pregunta.PlantillaPregunta
         sCmd.Append(" ,@dbp4ram4 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram4";
         if (pregunta.PlantillaPregunta == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = pregunta.PlantillaPregunta;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         // pregunta.FechaRegistro
         sCmd.Append(" ,@dbp4ram5 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram5";
         if (pregunta.FechaRegistro == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = pregunta.FechaRegistro;
         sqlParam.DbType = DbType.DateTime;
         sqlCmd.Parameters.Add(sqlParam);
         // pregunta.Valor
         sCmd.Append(" ,@dbp4ram6 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram6";
         if (pregunta.Valor == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = pregunta.Valor;
         sqlParam.DbType = DbType.Decimal;
         sqlCmd.Parameters.Add(sqlParam);
         // pregunta.PuedeOmitir
         sCmd.Append(" ,@dbp4ram7 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram7";
         if (pregunta.PuedeOmitir == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = pregunta.PuedeOmitir;
         sqlParam.DbType = DbType.Boolean;
         sqlCmd.Parameters.Add(sqlParam);
         // pregunta.Activo
         sCmd.Append(" ,@dbp4ram8 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram8";
         if (pregunta.Activo == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = pregunta.Activo;
         sqlParam.DbType = DbType.Boolean;
         sqlCmd.Parameters.Add(sqlParam);
         // pregunta.PresentacionPlantilla
         sCmd.Append(" ,@dbp4ram9 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram9";
         if (pregunta.PresentacionPlantilla == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = pregunta.PresentacionPlantilla;
         sqlParam.DbType = DbType.Byte;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ) ");
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("PreguntaDinamicoInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("PreguntaDinamicoInsHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
