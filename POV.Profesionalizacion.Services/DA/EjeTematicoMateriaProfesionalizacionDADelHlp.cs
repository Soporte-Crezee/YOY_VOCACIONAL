using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Profesionalizacion.BO;

namespace POV.Profesionalizacion.DA { 
   /// <summary>
   /// Elimina un registro de EjeTematicoMateriaProfesionalizacion en la BD
   /// </summary>
   internal class EjeTematicoMateriaProfesionalizacionDADelHlp { 
      /// <summary>
      /// Elimina un registro de EjeTematicoMateriaProfesionalizacionDADeltHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="ejeTematicoMateriaProfesionalizacionDADeltHlp">EjeTematicoMateriaProfesionalizacionDADeltHlp que desea eliminar</param>
      public void Action(IDataContext dctx, EjeTematico ejeTematico,MateriaProfesionalizacion materia){
         object myFirm = new object();
         string sError = String.Empty;
         if (ejeTematico == null)
            sError += ", EjeTematico";
         if (materia == null)
            sError += ", MateriaProfesionalizacion";
         if (sError.Length > 0)
            throw new Exception("EjeTematicoMateriaProfesionalizacionDADelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (ejeTematico.EjeTematicoID == null)
            sError += ", EjeTematicoID";
         if (materia.MateriaID == null)
            sError += ", MateriaProfesionalizacionID";
         if (sError.Length > 0)
            throw new Exception("EjeTematicoMateriaProfesionalizacionDADeltHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Profesionalizacion.EjeTematicoMateriaProfesionalizacion", 
         "EjeTematicoMateriaProfesionalizacionDADelHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "EjeTematicoMateriaProfesionalizacionDADelHlp: No se pudo conectar a la base de datos", "POV.Profesionalizacion.EjeTematicoMateriaProfesionalizacion", 
         "EjeTematicoMateriaProfesionalizacionDADelHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" DELETE FROM EjeTematicoMateriaProfesionalizacion ");
         StringBuilder s_VarWHERE = new StringBuilder();
         // ejeTematico.EjeTematicoID
         s_VarWHERE.Append(" EjeTematicoID =@dbp4ram1 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram1";
         if (ejeTematico.EjeTematicoID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = ejeTematico.EjeTematicoID;
         sqlParam.DbType = DbType.Int64;
         sqlCmd.Parameters.Add(sqlParam);
         // materia.MateriaID
         s_VarWHERE.Append(" AND MateriaID =@dbp4ram2 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram2";
         if (materia.MateriaID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = materia.MateriaID;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
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
            throw new Exception("EjeTematicoMateriaProfesionalizacionDADelHlp: Ocurrió un error al eliminar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("EjeTematicoMateriaProfesionalizacionDADelHlp: Ocurrió un error al eliminar el registro.");
      }
   } 
}
