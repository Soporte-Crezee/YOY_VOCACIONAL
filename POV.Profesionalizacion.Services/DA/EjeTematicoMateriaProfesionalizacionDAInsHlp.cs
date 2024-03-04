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
   /// Guarda un registro de Eje Temático y Materia de Profesionalización de Een la BD
   /// </summary>
   internal class EjeTematicoMateriaProfesionalizacionDAInsHlp { 
      /// <summary>
      /// Crea un registro de Guarda Registros de Eje Temático y Materia Profesionalizacion en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="guardaRegistrosdeEjeTemáticoyMateriaProfesionalizacion">Guarda Registros de Eje Temático y Materia Profesionalizacion que desea crear</param>
      public void Action(IDataContext dctx, EjeTematico ejeTematico,MateriaProfesionalizacion materia){
         object myFirm = new object();
         string sError = String.Empty;
         if (ejeTematico == null)
            sError += ", Eje Temático";
         if (materia == null)
            sError += ", Materia de Profesionalizacion";
         if (sError.Length > 0)
            throw new Exception("EjeTematicoMateriaProfesionalizacionInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (ejeTematico.EjeTematicoID == null)
            sError += ", Identificador del Eje Temático";
         if (materia.MateriaID == null)
            sError += ", Identificador de la Materia de Profesionalizacion";
         if (sError.Length > 0)
            throw new Exception("EjeTematicoMateriaProfesionalizacionInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Profesionalizacion.EjeTematicoMateriaProfesionalizacion", 
         "EjeTematicoMateriaProfesionalizacionInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "EjeTematicoMateriaProfesionalizacionInsHlp: No se pudo conectar a la base de datos", "POV.Profesionalizacion.EjeTematicoMateriaProfesionalizacion", 
         "EjeTematicoMateriaProfesionalizacionInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO EjeTematicoMateriaProfesionalizacion (MateriaID,EjeTematicoID) ");
         sCmd.Append(" VALUES ");
         sCmd.Append(" ( ");
         if (materia.MateriaID != null){
            sCmd.Append(" @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = materia.MateriaID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (ejeTematico.EjeTematicoID != null){
            sCmd.Append(" ,@dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = ejeTematico.EjeTematicoID;
            sqlParam.DbType = DbType.Int64;
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
            throw new Exception("EjeTematicoMateriaProfesionalizacionInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("EjeTematicoMateriaProfesionalizacionInsHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
