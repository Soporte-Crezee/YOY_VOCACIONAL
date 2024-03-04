using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.ContenidosDigital.BO;
using POV.Profesionalizacion.BO;

namespace POV.Profesionalizacion.DAO 
{ 
   /// <summary>
   /// Guarda un registro de ContenidoDigitalAgrupador en la BD
   /// </summary>
   internal class ContenidoDigitalAgrupadorInsHlp 
   { 
      /// <summary>
      /// Crea un registro de ContenidoDigitalAgrupador en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="contenidoDigitalAgrupador">ContenidoDigitalAgrupador que desea crear</param>
      public void Action(IDataContext dctx, ContenidoDigitalAgrupador contenidoDigitalAgrupador)
      {
         object myFirm = new object();
         string sError = String.Empty;
         if (contenidoDigitalAgrupador == null)
            sError += ", contenidoDigitalAgrupador";
         if (sError.Length > 0)
            throw new Exception("ContenidoDigitalAgrupadorInsHlp: Los siguientes objetos no pueden ser vacios " + sError.Substring(2));
         if (contenidoDigitalAgrupador.EjeTematico == null)
            sError += ", EjeTematico";
         if (contenidoDigitalAgrupador.EjeTematico.EjeTematicoID == null)
            sError += ", EjeTematico.EjeTematicoID";
         if (contenidoDigitalAgrupador.SituacionAprendizaje == null)
            sError += ", SituacionAprendizaje";
         if (contenidoDigitalAgrupador.SituacionAprendizaje.SituacionAprendizajeID == null)
            sError += ", SituacionAprendizaje.SituacionAprendizajeID";
         if (contenidoDigitalAgrupador.AgrupadorContenidoDigital == null)
            sError += ", AgrupadorContenidoDigital";
         if (contenidoDigitalAgrupador.AgrupadorContenidoDigital.AgrupadorContenidoDigitalID == null)
            sError += ", AgrupadorContenidoDigital.AgrupadorContenidoDigitalID";
         if (contenidoDigitalAgrupador.ContenidoDigital == null)
            sError += ", ContenidoDigital";
         if (contenidoDigitalAgrupador.ContenidoDigital.ContenidoDigitalID == null)
            sError += ", ContenidoDigital.ContenidoDigitalID";
         if (contenidoDigitalAgrupador.Activo == null)
            sError += ", Activo";
         if (contenidoDigitalAgrupador.FechaRegistro == null)
            sError += ", FechaRegistro";
         if (sError.Length > 0)
            throw new Exception("TramaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Profesionalizacion.DAO", 
         "ContenidoDigitalAgrupadorInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try
         {
             dctx.OpenConnection(myFirm);
             sqlCmd = dctx.CreateCommand();
         }
         catch (Exception ex)
         {
             throw new StandardException(MessageType.Error, "", "ContenidoDigitalAgrupadorInsHlp: No se pudo conectar a la base de datos", "POV.Profesionalizacion.DAO",
                "ContenidoDigitalAgrupadorInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO ContenidoDigitalAgrupador (EjeTematicoID, SituacionAprendizajeID, AgrupadorContenidoDigitalID, ContenidoDigitalID, Activo, FechaRegistro) ");
         sCmd.Append(" VALUES ");
         sCmd.Append(" ( ");
         // contenidoDigitalAgrupador.EjeTematico.EjeTematicoID
         sCmd.Append(" @dbp4ram2 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram2";
         if (contenidoDigitalAgrupador.EjeTematico.EjeTematicoID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = contenidoDigitalAgrupador.EjeTematico.EjeTematicoID;
         sqlParam.DbType = DbType.Int64;
         sqlCmd.Parameters.Add(sqlParam);
         // contenidoDigitalAgrupador.SituacionAprendizaje.SituacionAprendizajeID
         sCmd.Append(" ,@dbp4ram3 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram3";
         if (contenidoDigitalAgrupador.SituacionAprendizaje.SituacionAprendizajeID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = contenidoDigitalAgrupador.SituacionAprendizaje.SituacionAprendizajeID;
         sqlParam.DbType = DbType.Int64;
         sqlCmd.Parameters.Add(sqlParam);
         // contenidoDigitalAgrupador.AgrupadorContenidoDigital.AgrupadorContenidoDigitalID
         sCmd.Append(" ,@dbp4ram4 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram4";
         if (contenidoDigitalAgrupador.AgrupadorContenidoDigital.AgrupadorContenidoDigitalID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = contenidoDigitalAgrupador.AgrupadorContenidoDigital.AgrupadorContenidoDigitalID;
         sqlParam.DbType = DbType.Int64;
         sqlCmd.Parameters.Add(sqlParam);
         // contenidoDigitalAgrupador.ContenidoDigital.ContenidoDigitalID
         sCmd.Append(" ,@dbp4ram5 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram5";
         if (contenidoDigitalAgrupador.ContenidoDigital.ContenidoDigitalID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = contenidoDigitalAgrupador.ContenidoDigital.ContenidoDigitalID;
          sqlParam.DbType = DbType.Int64;
         sqlCmd.Parameters.Add(sqlParam);
         // contenidoDigitalAgrupador.Activo
         sCmd.Append(" ,@dbp4ram6 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram6";
         if (contenidoDigitalAgrupador.Activo == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = contenidoDigitalAgrupador.Activo;
         sqlParam.DbType = DbType.Boolean;
         sqlCmd.Parameters.Add(sqlParam);
         // contenidoDigitalAgrupador.FechaRegistro
         sCmd.Append(" ,@dbp4ram7 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram7";
         if (contenidoDigitalAgrupador.FechaRegistro == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = contenidoDigitalAgrupador.FechaRegistro;
         sqlParam.DbType = DbType.DateTime;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ) ");
         int iRes = 0;
         try
         {
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } 
         catch(Exception ex)
         {
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } 
            catch(Exception){ }
            throw new Exception("ContenidoDigitalAgrupadorInsHlp: Ocurrio un error al ingresar el registro. " + exmsg);
         } 
         finally
         {
            try{ dctx.CloseConnection(myFirm); } 
            catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("ContenidoDigitalAgrupadorInsHlp: Ocurrio un error al ingresar el registro.");
      }
   } 
}
