using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.DAO;
using POV.Modelo.Estandarizado.BO;
namespace POV.CentroEducativo.DAO { 
   /// <summary>
   /// Guarda un registro de una Mataria en la BD
   /// </summary>
   public class MateriaInsHlp { 
      /// <summary>
      /// Crea un registro de Materia en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="mataria">Materia que desea crear</param>
       public void Action(IDataContext dctx,Materia materia)
       {
         object myFirm = new object();
         string sError = String.Empty;
         if (materia == null)
            sError += ", Mataria";
        
         if (sError.Length > 0)
            throw new Exception("MatariaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
      if (materia.AreaAplicacion == null) {
         materia.AreaAplicacion = new AreaAplicacion();
      }
         if (materia.Clave == null)
            sError += ", Clave";
         if (materia.Titulo == null)
            sError += ", Titulo";
         if (materia.Grado == null)
            sError += ", Grado";
         if (materia.AreaAplicacion == null)
            sError += ", AreaAplicacion";
         if (sError.Length > 0)
            throw new Exception("MateriaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (materia.AreaAplicacion.AreaAplicacionID == null)
            sError += ", AreaAplicacionID";
         if (sError.Length > 0)
            throw new Exception("MatariaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO", 
         "MateriaInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "ClaveInsHlp: No se pudo conectar a la base de datos", "POV.CentroEducativo.DAO", 
         "MateriaInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO MATERIA (Clave, Titulo,Grado,AreaAplicacionID) ");
         sCmd.Append(" VALUES ");
         sCmd.Append(" ( ");
         // materia.Clave
         sCmd.Append(" @dbp4ram1 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram1";
         if (materia.Clave == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = materia.Clave;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         // materia.Titulo
         sCmd.Append(" ,@dbp4ram2 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram2";
         if (materia.Titulo == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = materia.Titulo;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         // materia.Grado
         sCmd.Append(" ,@dbp4ram3 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram3";
         if (materia.Grado == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = materia.Grado;
         sqlParam.DbType = DbType.Int16;
         sqlCmd.Parameters.Add(sqlParam);
         
         // materia.AreaAplicacion.AreaAplicacionID
         sCmd.Append(" ,@dbp4ram5 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram5";
         if (materia.AreaAplicacion.AreaAplicacionID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = materia.AreaAplicacion.AreaAplicacionID;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ) ");
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("MateriaInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("MateriaInsHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
