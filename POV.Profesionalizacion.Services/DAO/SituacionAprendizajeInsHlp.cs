using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Profesionalizacion.BO;

namespace POV.Profesionalizacion.DAO { 
   /// <summary>
   /// Guarda un registro de SituacionAprendizaje en la BD
   /// </summary>
   internal class SituacionAprendizajeInsHlp { 
      /// <summary>
      /// Crea un registro de SituacionAprendizaje en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="situacionAprendizaje">SituacionAprendizaje que desea crear</param>
      public void Action(IDataContext dctx, EjeTematico ejeTematico, SituacionAprendizaje situacionAprendizaje){
         object myFirm = new object();
         string sError = String.Empty;
         if (ejeTematico == null)
            sError += ", EjeTematico";
         if (sError.Length > 0)
            throw new Exception("SituacionAprendizajeInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (situacionAprendizaje == null)
            sError += ", SituacionAprendizaje";
         if (sError.Length > 0)
            throw new Exception("SituacionAprendizajeInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (situacionAprendizaje.Nombre == null || situacionAprendizaje.Nombre.Trim().Length == 0)
            sError += ", Nombre";
         if (situacionAprendizaje.AgrupadorContenidoDigital.AgrupadorContenidoDigitalID == null || situacionAprendizaje.AgrupadorContenidoDigital.AgrupadorContenidoDigitalID == 0)
            sError += ", AgrupadorContenidoDigital";
         if (sError.Length > 0)
            throw new Exception("SituacionAprendizajeInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Profesionalizacion.DAO", 
         "SituacionAprendizajeInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "TipoDocumentoInsHlp: No se pudo conectar a la base de datos", "POV.Profesionalizacion.DAO", 
         "SituacionAprendizajeInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO SituacionAprendizaje (EjeTematicoID,AgrupadorContenidoDigitalID,Nombre,Descripcion,FechaRegistro,EstatusProfesionalizacion) ");
         sCmd.Append(" VALUES ");
         sCmd.Append(" ( ");
         // ejeTematico.EjeTematicoID
         sCmd.Append(" @dbp4ram1 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram1";
         if (ejeTematico.EjeTematicoID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = ejeTematico.EjeTematicoID;
         sqlParam.DbType = DbType.Int64;
         sqlCmd.Parameters.Add(sqlParam);
         // situacionAprendizaje.AgrupadorContenidoDigital.AgrupadorContenidoDigitalID
         sCmd.Append(" ,@dbp4ram2 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram2";
         if (situacionAprendizaje.AgrupadorContenidoDigital.AgrupadorContenidoDigitalID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = situacionAprendizaje.AgrupadorContenidoDigital.AgrupadorContenidoDigitalID;
         sqlParam.DbType = DbType.Int64;
         sqlCmd.Parameters.Add(sqlParam);
         // situacionAprendizaje.Nombre
         sCmd.Append(" ,@dbp4ram3 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram3";
         if (situacionAprendizaje.Nombre == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = situacionAprendizaje.Nombre;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         // situacionAprendizaje.Descripcion
         sCmd.Append(" ,@dbp4ram4 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram4";
         if (situacionAprendizaje.Descripcion == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = situacionAprendizaje.Descripcion;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         // situacionAprendizaje.FechaRegistro
         sCmd.Append(" ,@dbp4ram5 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram5";
         if (situacionAprendizaje.FechaRegistro == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = situacionAprendizaje.FechaRegistro;
         sqlParam.DbType = DbType.DateTime;
         sqlCmd.Parameters.Add(sqlParam);
         // situacionAprendizaje.EstatusProfesionalizacion
         sCmd.Append(" ,@dbp4ram6 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram6";
         if (situacionAprendizaje.EstatusProfesionalizacion == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = situacionAprendizaje.EstatusProfesionalizacion;
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
            throw new Exception("SituacionAprendizajeInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("SituacionAprendizajeInsHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
