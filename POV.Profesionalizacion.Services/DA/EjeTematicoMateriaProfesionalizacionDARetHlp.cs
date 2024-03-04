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
   /// Consulta un registro de Materia en la BD
   /// </summary>
   internal class EjeTematicoMateriaProfesionalizacionDARetHlp { 
      /// <summary>
      /// Consulta registros de Materias de un Eje Temático en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="materiasdeunEjeTemático">Materias de un Eje Temático que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de Materias de un Eje Temático generada por la consulta</returns>
      public DataSet Action(IDataContext dctx, EjeTematico ejeTematico,MateriaProfesionalizacion materiaProfesionalizacion){
         object myFirm = new object();
         string sError = String.Empty;
         if (materiaProfesionalizacion == null)
            sError += ", Materia";
         if (ejeTematico == null)
            sError += ", EjeTematico";
         if (sError.Length > 0)
            throw new Exception("EjeTematicoMateriaProfesionalizacionRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (ejeTematico.EjeTematicoID == null)
            sError += ", EjeTematicoID";
         if (sError.Length > 0)
            throw new Exception("EjeTematicoMateriaProfesionalizacionRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Profesionalizacion.EjeTematicoMateriaProfesionalizacion", 
         "EjeTematicoMateriaProfesionalizacionRetHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "EjeTematicoMateriaProfesionalizacionRetHlp: No se pudo conectar a la base de datos", "POV.Profesionalizacion.EjeTematicoMateriaProfesionalizacion", 
         "EjeTematicoMateriaProfesionalizacionRetHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" SELECT em.MateriaID, em.EjeTematicoID, Nombre ");
         sCmd.Append(" FROM EjeTematicoMateriaProfesionalizacion em ");
         sCmd.Append(" INNER JOIN MateriaProfesionalizacion m ON em.MateriaID = m.MateriaID ");

         StringBuilder s_Var = new StringBuilder();
         // ejeTematico.EjeTematicoID
         s_Var.Append(" em.EjeTematicoID =@dbp4ram1 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram1";
         if (ejeTematico.EjeTematicoID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = ejeTematico.EjeTematicoID;
         sqlParam.DbType = DbType.Int64;
         sqlCmd.Parameters.Add(sqlParam);
         if (materiaProfesionalizacion.MateriaID != null){
            s_Var.Append(" AND  em.MateriaID =@dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = materiaProfesionalizacion.MateriaID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         s_Var.Append("  ");
         string s_Varres = s_Var.ToString().Trim();
         if (s_Varres.Length > 0){
            if (s_Varres.StartsWith("AND "))
               s_Varres = s_Varres.Substring(4);
            else if (s_Varres.StartsWith("OR "))
               s_Varres = s_Varres.Substring(3);
            else if (s_Varres.StartsWith(","))
               s_Varres = s_Varres.Substring(1);
            sCmd.Append(" WHERE  " + s_Varres);
         }
         DataSet ds = new DataSet();
         DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
         sqlAdapter.SelectCommand = sqlCmd;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            sqlAdapter.Fill(ds, "EjeTematicoMateriaProfesionalizacion");
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("EjeTematicoMateriaProfesionalizacionRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }
   } 
}
