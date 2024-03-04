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
   /// Guarda un registro de AEscalaDinamica en la BD
   /// </summary>
   internal class EscalaDinamicaInsHlp { 
      
       /// <summary>
       /// Crea un registro de AEscalaDinamica en la base de datos
       /// </summary>
       /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
       /// <param name="prueba">Prueba relacionada</param>
       /// <param name="aEscalaDinamica">AEscalaDinamica que desea crear</param>
      public void Action(IDataContext dctx, PruebaDinamica prueba, AEscalaDinamica escalaDinamica){
         object myFirm = new object();
         string sError = String.Empty;
         if (escalaDinamica == null)
            sError += ", AEscalaDinamica";
         if (sError.Length > 0)
            throw new Exception("EscalaDinamicaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (prueba == null)
            sError += ", PruebaDinamica";
         if (sError.Length > 0)
            throw new Exception("EscalaDinamicaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (prueba.PruebaID == null)
            sError += ", PruebaID";
         if (sError.Length > 0)
            throw new Exception("EscalaDinamicaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (escalaDinamica.Clasificador == null)
            sError += ", Clasificador";
         if (sError.Length > 0)
            throw new Exception("EscalaDinamicaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (escalaDinamica.PuntajeMinimo == null)
            sError += ", PuntajeMinimo";
         if (escalaDinamica.PuntajeMaximo == null)
            sError += ", PuntajeMaximo";
         if (escalaDinamica.EsPorcentaje == null)
            sError += ", EsPorcentaje";
         if (escalaDinamica.EsPredominante == null)
            sError += ", EsPredominante";
         if (escalaDinamica.Nombre == null || escalaDinamica.Nombre.Trim().Length == 0)
            sError += ", Nombre";
         if (escalaDinamica.Activo == null)
            sError += ", Activo";
         if (escalaDinamica.TipoEscalaDinamica == null)
            sError += ", TipoEscalaDinamica";
         if (sError.Length > 0)
            throw new Exception("EscalaDinamicaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Prueba.Diagnostico.Dinamica.DAO", 
         "EscalaDinamicaInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "EscalaDinamicaInsHlp: No se pudo conectar a la base de datos", "POV.Prueba.Diagnostico.Dinamica.DAO", 
         "EscalaDinamicaInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO EscalaDinamica (PruebaID, PuntajeMinimo, PuntajeMaximo, EsPorcentaje, EsPredominante, ClasificadorID, Nombre, Descripcion, Activo, TipoEscalaDinamica) ");
         sCmd.Append(" VALUES ");
         sCmd.Append(" ( ");
         sCmd.Append(" @dbp4ram2 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram2";
         if (prueba.PruebaID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = prueba.PruebaID;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ,@dbp4ram3 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram3";
         if (escalaDinamica.PuntajeMinimo == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = escalaDinamica.PuntajeMinimo;
         sqlParam.DbType = DbType.Decimal;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ,@dbp4ram4 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram4";
         if (escalaDinamica.PuntajeMaximo == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = escalaDinamica.PuntajeMaximo;
         sqlParam.DbType = DbType.Decimal;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ,@dbp4ram5 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram5";
         if (escalaDinamica.EsPorcentaje == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = escalaDinamica.EsPorcentaje;
         sqlParam.DbType = DbType.Boolean;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ,@dbp4ram6 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram6";
         if (escalaDinamica.EsPredominante == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = escalaDinamica.EsPredominante;
         sqlParam.DbType = DbType.Boolean;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ,@dbp4ram7 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram7";
         if (escalaDinamica.Clasificador.ClasificadorID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = escalaDinamica.Clasificador.ClasificadorID;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ,@dbp4ram8 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram8";
         if (escalaDinamica.Nombre == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = escalaDinamica.Nombre;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ,@dbp4ram9 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram9";
         if (escalaDinamica.Descripcion == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = escalaDinamica.Descripcion;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ,@dbp4ram10 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram10";
         if (escalaDinamica.Activo == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = escalaDinamica.Activo;
         sqlParam.DbType = DbType.Boolean;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ,@dbp4ram11 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram11";
         if (escalaDinamica.TipoEscalaDinamica == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = escalaDinamica.TipoEscalaDinamica;
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
            throw new Exception("EscalaDinamicaInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("EscalaDinamicaInsHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
