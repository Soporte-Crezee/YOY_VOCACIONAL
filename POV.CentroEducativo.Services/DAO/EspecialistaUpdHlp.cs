using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;
using System;
using System.Data;
using System.Data.Common;
using System.Text;

namespace POV.CentroEducativo.DAO
{
    public class EspecialistaUpdHlp
    {
        public void Action(IDataContext dctx, EspecialistaPruebas especialista, EspecialistaPruebas anterior)
        {
            object myFirm = new object();
            String sError = string.Empty;
            if (especialista == null)
                sError += ", Especialista";
            if (anterior == null)
                sError += ", Anterior";
            if (sError.Length > 0)
                throw new Exception("EspecialistaUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (especialista.EspecialistaPruebaID == null)
                sError += ", EspecialistaID";
            if (especialista.Curp == null || especialista.Curp.Trim().Length == 0)
                sError += ", Curp";
            if (especialista.Nombre == null || especialista.Nombre.Trim().Length == 0)
                sError += ", Nombre";
            if (especialista.PrimerApellido == null || especialista.PrimerApellido.Trim().Length == 0)
                sError += ", PrimerApellido";
            if (especialista.FechaNacimiento == null)
                sError += ", FechaNacimiento";
            if (especialista.Sexo == null)
                sError += ", Sexo";
            if (especialista.FechaRegistro == null)
                sError += ", FechaRegistro";
            if (especialista.Estatus == null)
                sError += ", Estatus";
            if (especialista.Clave == null || especialista.Clave.Trim().Length == 0)
                sError += ", Clave";
            if (especialista.EstatusIdentificacion == null)
                sError += ", EstatusIdentificacion";
            if (sError.Length > 0)
                throw new Exception("EspecialistaUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (anterior.EspecialistaPruebaID == null)
                sError += ", EspecialistaID anterior";
            if (anterior.Curp == null || anterior.Curp.Trim().Length == 0)
                sError += ", Curp anterior";
            if (anterior.Nombre == null || anterior.Nombre.Trim().Length == 0)
                sError += ", Nombre anterior";
            if (anterior.PrimerApellido == null || anterior.PrimerApellido.Trim().Length == 0)
                sError += ", PrimerApellido anterior";
            if (anterior.FechaNacimiento == null)
                sError += ", FechaNacimiento anterior";
            if (anterior.Sexo == null)
                sError += ", Sexo anterior";
            if (anterior.FechaRegistro == null)
                sError += ", FechaRegistro anterior";
            if (anterior.Estatus == null)
                sError += ", Estatus anterior";
            if (anterior.Clave == null || anterior.Clave.Trim().Length == 0)
                sError += ", Clave anterior";
            if (anterior.EstatusIdentificacion == null)
                sError += ", EstatusIdentificacion anterior";
            if (sError.Length > 0)
                throw new Exception("EspecialistaUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO", "EspecialistaUpdHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "EspecialistaUpdHlpUpdHl: Hubo un error al conectarse a la base de datos", "POV.CentroEducativo.DAO", "EspecialistaUpdHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" UPDATE EspecialistaPruebas ");
            if (especialista.Curp == null)
            {
                sCmd.Append(" SET Curp = NULL ");
            }
            else
            {
                sCmd.Append(" SET Curp = @especialista_Curp ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "especialista_Curp";
                sqlParam.Value = especialista.Curp;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (especialista.Nombre == null)
                sCmd.Append(" ,Nombre = NULL ");
            else
            {
                sCmd.Append(" ,Nombre = @especialista_Nombre ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "especialista_Nombre";
                sqlParam.Value = especialista.Nombre;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (especialista.PrimerApellido == null)
                sCmd.Append(" ,PrimerApellido = NULL ");
            else
            {
                sCmd.Append(" ,PrimerApellido = @especialista_PrimerApellido ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "especialista_PrimerApellido";
                sqlParam.Value = especialista.PrimerApellido;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (especialista.SegundoApellido == null)
                sCmd.Append(" ,SegundoApellido = NULL ");
            else
            {
                sCmd.Append(" ,SegundoApellido = @especialista_SegundoApellido ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "especialista_SegundoApellido";
                sqlParam.Value = especialista.SegundoApellido;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (especialista.FechaNacimiento == null)
            {
                sCmd.Append(" ,FechaNacimiento = NULL ");
            }
            else
            {
                sCmd.Append(" ,FechaNacimiento = @especialista_FechaNacimiento ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "especialista_FechaNacimiento";
                sqlParam.Value = especialista.FechaNacimiento;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (especialista.Sexo == null)
            {
                sCmd.Append(" ,Sexo = NULL ");
            }
            else
            {
                sCmd.Append(" ,Sexo = @especialista_Sexo ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "especialista_Sexo";
                sqlParam.Value = especialista.Sexo;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (especialista.Correo == null)
            {
                sCmd.Append(" ,Correo = NULL ");
            }
            else
            {
                // director.Correo
                sCmd.Append(" ,Correo = @especialista_Correo ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "especialista_Correo";
                sqlParam.Value = especialista.Correo;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (especialista.Estatus == null)
                sCmd.Append(" ,Estatus = NULL ");
            else
            {
                sCmd.Append(" ,Estatus = @especialista_Estatus ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "especialista_Estatus";
                sqlParam.Value = especialista.Estatus;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (especialista.EstatusIdentificacion == null)
            {
                sCmd.Append(" ,EstatusIdentificacion = NULL ");
            }
            else
            {
                sCmd.Append(" ,EstatusIdentificacion = @especialista_EstatusIdentificacion ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "especialista_EstatusIdentificacion";
                sqlParam.Value = especialista.EstatusIdentificacion;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.EspecialistaPruebaID == null)
                sCmd.Append(" WHERE EspecialistaID IS NULL ");
            else
            {
                sCmd.Append(" WHERE EspecialistaID = @dbp4ram22 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram22";
                sqlParam.Value = anterior.EspecialistaPruebaID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.Curp == null)
            {
                sCmd.Append(" AND Curp = NULL ");
            }
            else
            {
                sCmd.Append(" AND Curp = @dbp4ram11 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram11";
                sqlParam.Value = anterior.Curp;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.Nombre == null)
                sCmd.Append(" AND Nombre = NULL ");
            else
            {
                sCmd.Append(" AND Nombre = @dbp4ram12 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram12";
                sqlParam.Value = anterior.Nombre;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.PrimerApellido == null)
                sCmd.Append(" AND PrimerApellido = NULL ");
            else
            {
                sCmd.Append(" AND PrimerApellido = @dbp4ram13 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram13";
                sqlParam.Value = anterior.PrimerApellido;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.SegundoApellido == null)
                sCmd.Append(" AND SegundoApellido = NULL ");
            else
            {
                sCmd.Append(" AND SegundoApellido = @dbp4ram14 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram14";
                sqlParam.Value = anterior.SegundoApellido;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.FechaNacimiento == null)
            {
                sCmd.Append(" AND FechaNacimiento = NULL ");
            }
            else
            {
                sCmd.Append(" AND FechaNacimiento = @dbp4ram15 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram15";
                sqlParam.Value = anterior.FechaNacimiento;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.Sexo == null)
            {
                sCmd.Append(" AND Sexo = NULL ");
            }
            else
            {
                sCmd.Append(" AND Sexo = @dbp4ram16 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram16";
                sqlParam.Value = anterior.Sexo;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.Correo == null)
            {
                sCmd.Append(" AND Correo = NULL ");
            }
            else
            {
                // director.Correo
                sCmd.Append(" AND Correo = @dbp4ram17 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram17";
                sqlParam.Value = anterior.Correo;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.FechaRegistro == null)
            {
                sCmd.Append(" AND FechaRegistro = NULL ");
            }
            else
            {
                sCmd.Append(" AND FechaRegistro = @dbp4ram18 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram18";
                sqlParam.Value = anterior.FechaRegistro;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.Estatus == null)
                sCmd.Append(" AND Estatus = NULL ");
            else
            {
                sCmd.Append(" AND Estatus = @dbp4ram19 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram19";
                sqlParam.Value = anterior.Estatus;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.EstatusIdentificacion == null)
            {
                sCmd.Append(" AND EstatusIdentificacion = NULL ");
            }
            else
            {
                sCmd.Append(" AND EstatusIdentificacion = @dbp4ram20 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram20";
                sqlParam.Value = anterior.EstatusIdentificacion;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.Clave == null)
            {
                sCmd.Append(" AND Clave = NULL ");
            }
            else
            {
                sCmd.Append(" AND Clave = @dbp4ram21 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram21";
                sqlParam.Value = anterior.Clave;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            int iRes = 0;
            try
            {
                sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
                iRes = sqlCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("EspecialistaUpdHlp: Hubo  un Error al Actualizar el Registro . " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("EspecialistaUpdHlp: Hubo  un Error al Actualizar el Registro .");
        }
    }
}
