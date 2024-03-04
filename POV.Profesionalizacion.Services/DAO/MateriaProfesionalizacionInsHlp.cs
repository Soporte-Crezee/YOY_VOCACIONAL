using System;
using System.Data;
using System.Data.Common;
using System.Text;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Profesionalizacion.BO;

namespace POV.Profesionalizacion.DAO { 
   /// <summary>
   /// Guarda un registro de MateriaProfesionalizacion en la BD
   /// </summary>
   internal class MateriaProfesionalizacionInsHlp { 
      /// <summary>
      /// Crea un registro de MateriaProfesionalizacion en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="materiaProfesionalizacion">MateriaProfesionalizacion que desea crear</param>
      public void Action(IDataContext dctx, MateriaProfesionalizacion materiaProfesionalizacion, AreaProfesionalizacion areaProfesionalizacion){
         object myFirm = new object();
         string sError = String.Empty;
         if (materiaProfesionalizacion == null)
            sError += ", MateriaProfesionalizacion";
         if (materiaProfesionalizacion.Nombre == null)
            sError += ", Nombre";
         if (areaProfesionalizacion == null)
            sError += ", MateriaProfesionalizacion";
         if (areaProfesionalizacion.AreaProfesionalizacionID == null)
            sError += ", AreaProfesionalizacionID";
         if (sError.Length > 0)
            throw new Exception("MateriaProfesionalizacionInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Profesionalizacion.DAO", 
         "MateriaProfesionalizacionInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "MateriaProfesionalizacionInsHlp: No se pudo conectar a la base de datos", "POV.Profesionalizacion.DAO", 
         "MateriaProfesionalizacionInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO MateriaProfesionalizacion (Nombre, AreaProfesionalizacionID, FechaRegistro, Activo) ");
         sCmd.Append(" VALUES ");
         sCmd.Append(" ( ");
         // materiaProfesionalizacion.Nombre
         sCmd.Append(" @dbp4ram1 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram1";
         if (materiaProfesionalizacion.Nombre == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = materiaProfesionalizacion.Nombre;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         // areaProfesionalizacion.AreaProfesionalizacionID
         sCmd.Append(" ,@dbp4ram2 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram2";
         if (areaProfesionalizacion.AreaProfesionalizacionID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = areaProfesionalizacion.AreaProfesionalizacionID;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         // materiaProfesionalizacion.FechaRegistro
         sCmd.Append(" ,@dbp4ram3 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram3";
         if (materiaProfesionalizacion.FechaRegistro == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = materiaProfesionalizacion.FechaRegistro;
         sqlParam.DbType = DbType.DateTime;
         sqlCmd.Parameters.Add(sqlParam);
         // materiaProfesionalizacion.Activo
         sCmd.Append(" ,@dbp4ram4 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram4";
         if (materiaProfesionalizacion.Activo == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = materiaProfesionalizacion.Activo;
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
            throw new Exception("MateriaProfesionalizacionInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("MateriaProfesionalizacionInsHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
