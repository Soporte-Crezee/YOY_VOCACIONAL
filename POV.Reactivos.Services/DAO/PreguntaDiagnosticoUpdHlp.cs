using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Reactivos.BO;

namespace POV.Reactivos.DAO { 
   /// <summary>
   /// Actualiza un registro de Pregunta (Diagnostico) en la BD
   /// </summary>
   public class PreguntaDiagnosticoUpdHlp { 
      /// <summary>
      /// Actualiza de manera optimista un registro de PreguntaDiagnosticoUpdHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="preguntaDiagnosticoUpdHlp">PreguntaDiagnosticoUpdHlp que tiene los datos nuevos</param>
      /// <param name="anterior">PreguntaDiagnosticoUpdHlp que tiene los datos anteriores</param>
      public void Action(IDataContext dctx, Pregunta pregunta, Pregunta anterior){
         object myFirm = new object();
         String sError = string.Empty;
         if (pregunta == null)
            sError += ", Pregunta";
         if (anterior == null)
            sError += ", Anterior";
         if (sError.Length > 0)
            throw new Exception("PreguntaDiagnosticoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (anterior.PreguntaID == null)
            sError += ", Anterior PreguntaID";
         if (sError.Length > 0)
            throw new Exception("PreguntaDiagnosticoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Reactivos.DAO", 
         "PreguntaDiagnosticoUpdHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "PreguntaDiagnosticoUpdHlpUpdHl: Hubo un error al conectarse a la base de datos", "POV.Reactivos.DAO", 
         "PreguntaDiagnosticoUpdHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE PreguntaDiagnostico ");
         if (pregunta.Orden != null){
            sCmd.Append(" SET Orden = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = pregunta.Orden;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (pregunta.Valor != null){
            sCmd.Append(" ,Valor = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = pregunta.Valor;
            sqlParam.DbType = DbType.Decimal;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (pregunta.PlantillaPregunta != null){
            sCmd.Append(" ,PlantillaPregunta = @dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = pregunta.PlantillaPregunta;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (pregunta.TextoPregunta != null){
            sCmd.Append(" ,TextoPregunta = @dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            sqlParam.Value = pregunta.TextoPregunta;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (pregunta.PuedeOmitir != null){
            sCmd.Append(" ,PuedeOmitir = @dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            sqlParam.Value = pregunta.PuedeOmitir;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (pregunta.SoloImagen != null)
         {
             sCmd.Append(" ,SoloImagen = @dbp4ram51 ");
             sqlParam = sqlCmd.CreateParameter();
             sqlParam.ParameterName = "dbp4ram51";
             sqlParam.Value = pregunta.SoloImagen;
             sqlParam.DbType = DbType.Boolean;
             sqlCmd.Parameters.Add(sqlParam);
         }
         if (pregunta.Activo != null){
            sCmd.Append(" ,Activo = @dbp4ram6 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram6";
            sqlParam.Value = pregunta.Activo;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.PreguntaID == null)
            sCmd.Append(" WHERE PreguntaID IS NULL ");
         else{ 
            // anterior.PreguntaID
            sCmd.Append(" WHERE PreguntaID = @dbp4ram7 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram7";
            sqlParam.Value = anterior.PreguntaID;
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
            throw new Exception("PreguntaDiagnosticoUpdHlp: Hubo un error al actualizar el Registro . " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("PreguntaDiagnosticoUpdHlp: Hubo un error al actualizar el Registro .");
      }
   } 
}
