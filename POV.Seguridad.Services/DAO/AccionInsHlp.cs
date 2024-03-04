using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Seguridad.BO;

namespace POV.Seguridad.DAO
{ 
   /// <summary>
   /// Crea una Acción en la base de datos
   /// </summary>
   public class AccionInsHlp { 
      /// <summary>
      /// Crea un registro de Acción en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="acción">Acción que desea crear</param>
      public void Action(IDataContext dctx, Accion accion){
         object myFirm = new object();
         string sError = "";
         if (accion == null)
            sError += ", Acción";
         if (sError.Length > 0)
            throw new Exception(" AccionInsHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
         if (accion.Nombre == null || accion.Nombre.Trim().Length == 0)
            sError += ", Descripción";
         if (accion.Activo == null)
             sError += ", Activo";
         if (sError.Length > 0)
             throw new Exception(" AccionInsHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
         if (dctx == null)
             throw new Exception(" AccionInsHlp: DataContext no puede ser nulo");
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
             throw new Exception("AccionInsHlp: Hubo un error al conectarse a la base de datos" + Environment.NewLine + ex.Message);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO Accion(Nombre, Activo) ");

         sCmd.Append(" VALUES (@dbp4ram1 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram1";
         if (accion.Nombre == null)
            sqlParam.Value = DBNull.Value;
         else
             sqlParam.Value = accion.Nombre;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);

         sCmd.Append(" ,@dbp4ram2 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram2";
         if (accion.Activo == null)
             sqlParam.Value = DBNull.Value;
         else
             sqlParam.Value = accion.Activo;
         sqlParam.DbType = DbType.Boolean;
         sqlCmd.Parameters.Add(sqlParam);

         sCmd.Append(" ) ");
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.ToString();
            iRes = sqlCmd.ExecuteNonQuery();
            dctx.CommitTransaction(myFirm);
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.RollbackTransaction(myFirm); } catch(Exception){ }
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("AccionInsHlp: Hubo un error al crear el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
             throw new Exception("AccionInsHlp: Hubo un error al crear el registro.");
      }
   } 
}
