using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.DAO;

namespace POV.CentroEducativo.DAO { 
   /// <summary>
   /// Actualiza un registro de Materia en la BD
   /// </summary>
   public class MateriaUpdHlp { 
      /// <summary>
      /// Actualiza de manera optimista un registro de MateriaUpdHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="materiaUpdHlp">MateriaUpdHlp que tiene los datos nuevos</param>
      /// <param name="anterior">MateriaUpdHlp que tiene los datos anteriores</param>
      public void Action(IDataContext dctx, Materia materia, Materia anterior){
         object myFirm = new object();
         String sError = string.Empty;
         if (materia == null)
            sError += ", Materia";
         if (anterior == null)
            sError += ", Anterior";
         if (sError.Length > 0)
            throw new Exception("MateriaUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (anterior.MateriaID == null)
            sError += ", Anterior MateriaID";
         if (sError.Length > 0)
            throw new Exception("MateriaUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO", 
         "MateriaUpdHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "MateriaUpdHlp: Hubo un error al conectarse a la base de datos", "POV.CentroEducativo.DAO", 
         "MateriaUpdHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE Materia ");
         if (materia.Clave == null)
            sCmd.Append(" SET Clave = NULL ");
         else{ 
            // materia.Clave
            sCmd.Append(" SET Clave = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = materia.Clave;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (materia.Titulo == null)
            sCmd.Append(" ,Titulo = NULL ");
         else{ 
            // materia.Titulo
            sCmd.Append(" ,Titulo = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = materia.Titulo;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (materia.Grado == null)
            sCmd.Append(" ,Grado = NULL ");
         else{ 
            // materia.Grado
            sCmd.Append(" ,Grado = @dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = materia.Grado;
            sqlParam.DbType = DbType.Byte;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (materia.AreaAplicacion.AreaAplicacionID == null)
             sCmd.Append(" ,AreaAplicacionID = NULL ");
         else{ 
            // materia.AreaAplicacion.AreaAplicacionID
             sCmd.Append(" ,AreaAplicacionID = @dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            sqlParam.Value = materia.AreaAplicacion.AreaAplicacionID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.MateriaID == null)
             sCmd.Append(" WHERE MateriaID = NULL ");
         else{ 
            // anterior.MateriaID
            sCmd.Append(" WHERE MateriaID = @dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            sqlParam.Value = anterior.MateriaID;
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
            throw new Exception("MateriaUpdHlp: Hubo  un Error al Actualizar el Registro . " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
             throw new Exception("MateriaUpdHlp: Hubo  un Error al Actualizar el Registro .");
      }
   } 
}
