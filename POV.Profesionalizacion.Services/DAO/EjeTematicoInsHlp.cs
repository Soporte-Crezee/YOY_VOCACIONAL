using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Profesionalizacion.BO;
using POV.Profesionalizacion.DAO;

namespace POV.Profesionalizacion.DAO { 
   /// <summary>
   /// Guarda un registro de EjeTematico en la BD
   /// </summary>
   internal class EjeTematicoInsHlp { 
      /// <summary>
      /// Crea un registro de EjeTematico en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="ejeTematico">EjeTematico que desea crear</param>
      public void Action(IDataContext dctx, EjeTematico ejeTematico){
         object myFirm = new object();
         string sError = String.Empty;
         if (ejeTematico == null)
            sError += ", EjeTematico";
         if (sError.Length > 0)
            throw new Exception("EjeTematicoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (ejeTematico.Nombre == null)
            sError += ", Nombre del  Éje Temático";
         if (ejeTematico.AreaProfesionalizacion == null)
            sError += ", Area de Profesionalizacion del Eje Tematico";
         if (ejeTematico.EstatusProfesionalizacion == null)
         {
             ejeTematico.EstatusProfesionalizacion = EEstatusProfesionalizacion.MANTENIMIENTO;
         }
         if (ejeTematico.FechaRegistro == null)
            sError += ", Fecha de Registro del Eje Tematico";
         if (sError.Length > 0)
            throw new Exception("EjeTematicoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (ejeTematico.AreaProfesionalizacion.AreaProfesionalizacionID == null)
            sError += ", Identificador del Área de Profesionalización del Éje Temático";
         if (sError.Length > 0)
            throw new Exception("EjeTematicoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Profesionalizacion.DAO", 
         "EjeTematicoInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "EjeTematicoInsHlp: No se pudo conectar a la base de datos", "POV.Profesionalizacion.DAO", 
         "EjeTematicoInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO EjeTematico (Nombre,Descripcion,EstatusProfesionalizacion,FechaRegistro,AreaProfesionalizacionID) ");
         sCmd.Append(" VALUES ");
         sCmd.Append(" ( ");
         if (ejeTematico.Nombre != null){
            sCmd.Append(" @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = ejeTematico.Nombre;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         // ejeTematico.Descripcion
         sCmd.Append(" ,@dbp4ram2 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram2";
         if (ejeTematico.Descripcion == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = ejeTematico.Descripcion;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         if (ejeTematico.EstatusProfesionalizacion != null){
            sCmd.Append(" ,@dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = ejeTematico.EstatusProfesionalizacion;
            sqlParam.DbType = DbType.Byte;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (ejeTematico.FechaRegistro != null){
            sCmd.Append(" ,@dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            sqlParam.Value = ejeTematico.FechaRegistro;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (ejeTematico.AreaProfesionalizacion.AreaProfesionalizacionID != null){
            sCmd.Append(" ,@dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            sqlParam.Value = ejeTematico.AreaProfesionalizacion.AreaProfesionalizacionID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         sCmd.Append(" ) ");
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("EjeTematicoInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("EjeTematicoInsHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
