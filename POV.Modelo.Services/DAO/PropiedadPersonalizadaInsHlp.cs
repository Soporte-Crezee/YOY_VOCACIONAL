using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Modelo.BO;
using POV.Modelo.DAO;

namespace POV.Modelo.DAO { 
   /// <summary>
   /// Guarda un registro de PropiedadPersonalizada en la BD
   /// </summary>
   internal class PropiedadPersonalizadaInsHlp { 
      /// <summary>
      /// Crea un registro de PropiedadPersonalizada en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="propiedadPersonalizada">PropiedadPersonalizada que desea crear</param>
      public void Action(IDataContext dctx, ModeloDinamico modelo, PropiedadPersonalizada propiedad){
         object myFirm = new object();
         string sError = String.Empty;
         if (modelo == null)
            sError += ", ModeloDinamico";
         if (propiedad == null)
            sError += ", PropiedadPersonalizada";
         if (sError.Length > 0)
            throw new Exception("PropiedadPersonalizadaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Modelo.DAO", 
         "PropiedadPersonalizadaInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "PropiedadPersonalizadaInsHlp: No se pudo conectar a la base de datos", "POV.Modelo.DAO", 
         "PropiedadPersonalizadaInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO PropiedadPersonalizada (ModeloID, Nombre, Descripcion, EsVisible, Activo, FechaRegistro) ");
         sCmd.Append(" VALUES ");
         sCmd.Append(" ( ");
         // modelo.ModeloID
         sCmd.Append(" @dbp4ram1 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram1";
         if (modelo.ModeloID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = modelo.ModeloID;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         // propiedad.Nombre
         sCmd.Append(" ,@dbp4ram2 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram2";
         if (propiedad.Nombre == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = propiedad.Nombre;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         // propiedad.Descripcion
         sCmd.Append(" ,@dbp4ram3 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram3";
         if (propiedad.Descripcion == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = propiedad.Descripcion;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         // propiedad.EsVisible
         sCmd.Append(" ,@dbp4ram4 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram4";
         if (propiedad.EsVisible == null)
             sqlParam.Value = DBNull.Value;
         else
             sqlParam.Value = propiedad.EsVisible;
         sqlParam.DbType = DbType.Boolean;
         sqlCmd.Parameters.Add(sqlParam);
         // propiedad.Activo
         sCmd.Append(" ,@dbp4ram5 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram5";
         if (propiedad.Activo == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = propiedad.Activo;
         sqlParam.DbType = DbType.Boolean;
         sqlCmd.Parameters.Add(sqlParam);
         // propiedad.FechaRegistro
         sCmd.Append(" ,@dbp4ram6 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram6";
         if (propiedad.FechaRegistro == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = propiedad.FechaRegistro;
         sqlParam.DbType = DbType.DateTime;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ) ");
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("PropiedadPersonalizadaInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("PropiedadPersonalizadaInsHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
