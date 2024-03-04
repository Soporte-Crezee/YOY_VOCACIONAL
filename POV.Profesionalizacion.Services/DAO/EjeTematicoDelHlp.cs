using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Profesionalizacion.BO;

namespace POV.Profesionalizacion.DAO { 
   /// <summary>
   /// Actualiza en la base de datos
   /// </summary>
   internal class EjeTematicoDelHlp {
       /// <summary>
       /// Actualiza de manera optimista un registro de eje temático en la base de datos.
       /// </summary>
       /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
       /// <param name="ejeTematico">EjeTematico al que pertenece a eliminar</param>
        public void Action(IDataContext dctx, EjeTematico ejeTematico){
         object myFirm = new object();
         string sError = String.Empty;
         if (ejeTematico == null)
            sError += ", Eje Temático";
           if (sError.Length > 0)
            throw new Exception("EjeTematicoDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (ejeTematico.EjeTematicoID == null)
            sError += ", Identificador del eje temático";
          if (sError.Length > 0)
            throw new Exception("EjeTematicoDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Profesionalizacion.DAO", 
         "EjeTematicoDelHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
             throw new StandardException(MessageType.Error, "", "EjeTematicoDelHlp: Ocurrió un error al conectarse a la base de datos", "POV.Profesionalizacion.DAO",
         "EjeTematicoDelHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE EjeTematico");
         // situacionAprendizaje.EstatusProfesionalizacion
         sCmd.Append(" SET estatusprofesionalizacion =@dbp4ram1 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram1";
         sqlParam.Value = EEstatusProfesionalizacion.INACTIVO;
         sqlParam.DbType = DbType.Byte;
         sqlCmd.Parameters.Add(sqlParam);
         if (ejeTematico.EjeTematicoID != null)
         {
             sCmd.Append(" WHERE EjeTematicoID= @dbp4ram2 ");
             sqlParam = sqlCmd.CreateParameter();
             sqlParam.ParameterName = "dbp4ram2";
             sqlParam.Value = ejeTematico.EjeTematicoID;
             sqlParam.DbType = DbType.Int64;
             sqlCmd.Parameters.Add(sqlParam);
         }
         else {
                sCmd.Append(" WHERE EjeTematicoID IS NULL ");
         }
    
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("EjeTematicoDelHlp: Ocurrio un error al actualizar el eje temático o fue modificado mientras era editado. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
             throw new Exception("EjeTematicoDelHlp: Ocurrio un error al actualizar el eje temático o fue modificado mientras era editado.");
      }
   } 
}
