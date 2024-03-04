using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Profesionalizacion.BO;

namespace POV.Profesionalizacion.DAO { 
   /// <summary>
   /// Guarda un registro de TemaCurso en la BD
   /// </summary>
   internal class TemaCursoInsHlp { 
      /// <summary>
      /// Crea un registro de TemaCurso en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="temaCurso">TemaCurso que desea crear</param>
      public void Action(IDataContext dctx, TemaCurso temacurso){
         object myFirm = new object();
         string sError = string.Empty;
         if (temacurso == null)
            sError += ", TemaCurso";
         if (temacurso.Nombre == null)
            sError += ", Nombre";         
         if (temacurso.FechaRegistro == null)
            sError += ", FechaRegistro";
         if (temacurso.Activo == null)
            sError += ", Activo";
         if (sError.Length > 0)
            throw new Exception("TemaCursoInsHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Profesionalizacion.DAO", 
         "TemaCursoInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "TemaCursoInsHlp: No se pudo conectar a la base de datos", "POV.Profesionalizacion.DAO", 
         "TemaCursoInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO TemaCurso (Nombre, Descripcion, FechaRegistro, Activo) ");
         sCmd.Append(" VALUES ( ");
         // temacurso.Nombre
         sCmd.Append(" @dbp4ram1 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram1";
         if (temacurso.Nombre == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = temacurso.Nombre;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         // temacurso.Descripcion
         sCmd.Append(" ,@dbp4ram2 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram2";
         if (temacurso.Descripcion == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = temacurso.Descripcion;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         // temacurso.FechaRegistro
         sCmd.Append(" ,@dbp4ram3 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram3";
         if (temacurso.FechaRegistro == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = temacurso.FechaRegistro;
         sqlParam.DbType = DbType.DateTime;
         sqlCmd.Parameters.Add(sqlParam);
         // temacurso.Activo
         sCmd.Append(" ,@dbp4ram4 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram4";
         if (temacurso.Activo == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = temacurso.Activo;
         sqlParam.DbType = DbType.Boolean;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ) ");
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("TemaCursoInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("TemaCursoInsHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
