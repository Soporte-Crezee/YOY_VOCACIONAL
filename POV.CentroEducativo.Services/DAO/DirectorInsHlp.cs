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
   /// Guarda un registro de Director en la BD
   /// </summary>
   public class DirectorInsHlp { 
      /// <summary>
      /// Crea un registro de Director en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="director">Director que desea crear</param>
      public void Action(IDataContext dctx, Director director){
         object myFirm = new object();
         string sError = String.Empty;
         if (director == null)
            sError += ", Director";
         if (sError.Length > 0)
            throw new Exception("DirectorInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO", 
         "DirectorInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "DirectorInsHlp: No se pudo conectar a la base de datos", "POV.CentroEducativo.DAO", 
         "DirectorInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO Director (Curp,Nombre,PrimerApellido,SegundoApellido,FechaNacimiento,Sexo,NivelEscolar,Correo,Telefono,FechaRegistro,Estatus,Clave,EstatusIdentificacion) ");
         sCmd.Append(" VALUES ");
         sCmd.Append(" ( ");
         // director.Curp
         sCmd.Append(" @dbp4ram1 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram1";
         if (director.Curp == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = director.Curp;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         // director.Nombre
         sCmd.Append(" ,@dbp4ram2 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram2";
         if (director.Nombre == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = director.Nombre;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         // director.PrimerApellido
         sCmd.Append(" ,@dbp4ram3 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram3";
         if (director.PrimerApellido == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = director.PrimerApellido;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         // director.SegundoApellido
         sCmd.Append(" ,@dbp4ram4 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram4";
         if (director.SegundoApellido == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = director.SegundoApellido;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         // director.FechaNacimiento
         sCmd.Append(" ,@dbp4ram5 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram5";
         if (director.FechaNacimiento == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = director.FechaNacimiento;
         sqlParam.DbType = DbType.DateTime;
         sqlCmd.Parameters.Add(sqlParam);
         // director.Sexo
         sCmd.Append(" ,@dbp4ram6 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram6";
         if (director.Sexo == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = director.Sexo;
         sqlParam.DbType = DbType.Boolean;
         sqlCmd.Parameters.Add(sqlParam);
         // director.NivelEscolar
         sCmd.Append(" ,@dbp4ram7 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram7";
         if (director.NivelEscolar == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = director.NivelEscolar;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         // director.Correo
         sCmd.Append(" ,@dbp4ram8 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram8";
         if (director.Correo == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = director.Correo;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         // director.Telefono
         sCmd.Append(" ,@dbp4ram9 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram9";
         if (director.Telefono == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = director.Telefono;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         // director.FechaRegistro
         sCmd.Append(" ,@dbp4ram10 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram10";
         if (director.FechaRegistro == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = director.FechaRegistro;
         sqlParam.DbType = DbType.DateTime;
         sqlCmd.Parameters.Add(sqlParam);
         // director.Estatus
         sCmd.Append(" ,@dbp4ram11 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram11";
         if (director.Estatus == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = director.Estatus;
         sqlParam.DbType = DbType.Boolean;
         sqlCmd.Parameters.Add(sqlParam);
         // director.Clave
         sCmd.Append(" ,@dbp4ram12 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram12";
         if (director.Clave == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = director.Clave;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         // director.EstatusIdentificacion
         sCmd.Append(" ,@dbp4ram13 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram13";
         if (director.EstatusIdentificacion == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = director.EstatusIdentificacion;
         sqlParam.DbType = DbType.Boolean;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ) ");
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("DirectorInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("DirectorInsHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
