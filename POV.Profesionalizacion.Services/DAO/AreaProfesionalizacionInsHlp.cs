using System;
using System.Data;
using System.Data.Common;
using System.Text;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Profesionalizacion.BO;

namespace POV.Profesionalizacion.DAO { 
   /// <summary>
   /// Guarda un registro de AreaProfesionalizacion en la BD
   /// </summary>
   internal class AreaProfesionalizacionInsHlp { 
      /// <summary>
      /// Crea un registro de AreaProfesionalizacion en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="areaProfesionalizacion">AreaProfesionalizacion que desea crear</param>
      public void Action(IDataContext dctx, AreaProfesionalizacion areaProfesionalizacion){
         object myFirm = new object();
         string sError = String.Empty;
         if (areaProfesionalizacion == null)
            sError += ", AreaProfesionalizacion";
         if (areaProfesionalizacion.Nombre == null)
            sError += ", Nombre";
         if (areaProfesionalizacion.NivelEducativo == null)
             sError += ", NivelEducativo";
         if (areaProfesionalizacion.NivelEducativo.NivelEducativoID == null)
             sError += ", NivelEducativoID";
         if (areaProfesionalizacion.Grado == null)
             sError += ", Grado";
         if (sError.Length > 0)
            throw new Exception("AreaProfesionalizacionInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "Namespace.TipoControlador", 
         "AreaProfesionalizacionInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "AreaProfesionalizacionInsHlp: No se pudo conectar a la base de datos", "Namespace.TipoControlador", 
         "AreaProfesionalizacionInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO AreaProfesionalizacion (Nombre, Descripcion, FechaRegistro, Activo, NivelEducativoID, Grado) ");
         sCmd.Append(" VALUES ");
         sCmd.Append(" ( ");
         // areaProfesionalizacion.Nombre
         sCmd.Append(" @dbp4ram1 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram1";
         if (areaProfesionalizacion.Nombre == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = areaProfesionalizacion.Nombre;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         // areaProfesionalizacion.Descripcion
         sCmd.Append(" ,@dbp4ram2 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram2";
         if (areaProfesionalizacion.Descripcion == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = areaProfesionalizacion.Descripcion;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         // areaProfesionalizacion.FechaRegistro
         sCmd.Append(" ,@dbp4ram3 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram3";
         if (areaProfesionalizacion.FechaRegistro == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = areaProfesionalizacion.FechaRegistro;
         sqlParam.DbType = DbType.DateTime;
         sqlCmd.Parameters.Add(sqlParam);
         // areaProfesionalizacion.Activo
         sCmd.Append(" ,@dbp4ram4 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram4";
         if (areaProfesionalizacion.Activo == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = areaProfesionalizacion.Activo;
         sqlParam.DbType = DbType.Boolean;
         sqlCmd.Parameters.Add(sqlParam);

         // areaProfesionalizacion.NivelEducativo.NivelEducativoID
         sCmd.Append(" ,@dbp4ram5 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram5";
         if (areaProfesionalizacion.NivelEducativo.NivelEducativoID == null)
             sqlParam.Value = DBNull.Value;
         else
             sqlParam.Value = areaProfesionalizacion.NivelEducativo.NivelEducativoID;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         // areaProfesionalizacion.Grado
         sCmd.Append(" ,@dbp4ram6 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram6";
         if (areaProfesionalizacion.Grado == null)
             sqlParam.Value = DBNull.Value;
         else
             sqlParam.Value = areaProfesionalizacion.Grado;
         sqlParam.DbType = DbType.Int16;
         sqlCmd.Parameters.Add(sqlParam);

         sCmd.Append(" ) ");
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("AreaProfesionalizacionInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("AreaProfesionalizacionInsHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
