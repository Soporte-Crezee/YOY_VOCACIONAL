using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Prueba.Diagnostico.Dinamica.BO;
using POV.Modelo.BO;
using POV.Prueba.BO;

namespace POV.Prueba.Diagnostico.Dinamica.DAO { 
   /// <summary>
   /// Actualiza un registro de AEscalaDinamica en la BD
   /// </summary>
   internal class EscalaDinamicaUpdHlp { 
      /// <summary>
      /// Actualiza de manera optimista un registro de EscalaDinamicaUpdHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="escalaDinamicaUpdHlp">EscalaDinamicaUpdHlp que tiene los datos nuevos</param>
      /// <param name="anterior">EscalaDinamicaUpdHlp que tiene los datos anteriores</param>
      public void Action(IDataContext dctx, AEscalaDinamica escalaDinamica, AEscalaDinamica anterior){
         object myFirm = new object();
         String sError = string.Empty;
         if (escalaDinamica == null)
            sError += ", AEscalaDinamica";
         if (anterior == null)
            sError += ", Anterior";
         if (sError.Length > 0)
            throw new Exception("EscalaDinamicaUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (anterior.PuntajeID == null)
            sError += ", Anterior AEscalaDinamicaID";
         if (sError.Length > 0)
            throw new Exception("EscalaDinamicaUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Prueba.Diagnostico.Dinamica.DAO", 
         "EscalaDinamicaUpdHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "EscalaDinamicaUpdHlpUpdHl: Hubo un error al conectarse a la base de datos", "POV.Prueba.Diagnostico.Dinamica.DAO", 
         "EscalaDinamicaUpdHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE EscalaDinamica ");
         if (escalaDinamica.PuntajeMinimo != null){
            sCmd.Append(" SET  PuntajeMinimo = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = escalaDinamica.PuntajeMinimo;
            sqlParam.DbType = DbType.Decimal;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (escalaDinamica.PuntajeMaximo != null){
            sCmd.Append(" ,PuntajeMaximo = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = escalaDinamica.PuntajeMaximo;
            sqlParam.DbType = DbType.Decimal;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (escalaDinamica.EsPorcentaje != null){
            sCmd.Append(" ,EsPorcentaje = @dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = escalaDinamica.EsPorcentaje;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (escalaDinamica.EsPredominante != null){
            sCmd.Append(" ,EsPredominante = @dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            sqlParam.Value = escalaDinamica.EsPredominante;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (escalaDinamica.Clasificador.ClasificadorID != null){
            sCmd.Append(" ,ClasificadorID = @dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            sqlParam.Value = escalaDinamica.Clasificador.ClasificadorID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (escalaDinamica.Nombre != null){
            sCmd.Append(" ,Nombre = @dbp4ram6 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram6";
            sqlParam.Value = escalaDinamica.Nombre;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (escalaDinamica.Descripcion != null){
            sCmd.Append(" ,Descripcion = @dbp4ram7 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram7";
            sqlParam.Value = escalaDinamica.Descripcion;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (escalaDinamica.Activo != null){
            sCmd.Append(" ,Activo = @dbp4ram8 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram8";
            sqlParam.Value = escalaDinamica.Activo;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         StringBuilder s_VarWHERE = new StringBuilder();
         if (anterior.PuntajeID == null)
            s_VarWHERE.Append(" PuntajeID IS NULL ");
         else{ 
            s_VarWHERE.Append(" PuntajeID = @dbp4ram9 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram9";
            sqlParam.Value = anterior.PuntajeID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.PuntajeMinimo != null){
            s_VarWHERE.Append(" AND PuntajeMinimo = @dbp4ram10 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram10";
            sqlParam.Value = anterior.PuntajeMinimo;
            sqlParam.DbType = DbType.Decimal;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.PuntajeMaximo != null){
            s_VarWHERE.Append(" AND PuntajeMaximo = @dbp4ram11 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram11";
            sqlParam.Value = anterior.PuntajeMaximo;
            sqlParam.DbType = DbType.Decimal;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.EsPorcentaje != null){
            s_VarWHERE.Append(" AND EsPorcentaje = @dbp4ram12 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram12";
            sqlParam.Value = anterior.EsPorcentaje;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.EsPredominante != null){
            s_VarWHERE.Append(" AND EsPredominante = @dbp4ram13 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram13";
            sqlParam.Value = anterior.EsPredominante;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.Activo != null){
            s_VarWHERE.Append(" AND Activo = @dbp4ram14 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram14";
            sqlParam.Value = anterior.Activo;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.TipoEscalaDinamica != null){
            s_VarWHERE.Append(" AND TipoEscalaDinamica = @dbp4ram15 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram15";
            sqlParam.Value = anterior.TipoEscalaDinamica;
            sqlParam.DbType = DbType.Byte;
            sqlCmd.Parameters.Add(sqlParam);
         }
         string s_VarWHEREres = s_VarWHERE.ToString().Trim();
         if (s_VarWHEREres.Length > 0){
            if (s_VarWHEREres.StartsWith("AND "))
               s_VarWHEREres = s_VarWHEREres.Substring(4);
            else if (s_VarWHEREres.StartsWith("OR "))
               s_VarWHEREres = s_VarWHEREres.Substring(3);
            else if (s_VarWHEREres.StartsWith(","))
               s_VarWHEREres = s_VarWHEREres.Substring(1);
            sCmd.Append(" WHERE " + s_VarWHEREres);
         }
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("EscalaDinamicaUpdHlp: Hubo  un Error al Actualizar el Registro . " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("EscalaDinamicaUpdHlp: Hubo  un Error al Actualizar el Registro .");
      }
   } 
}
