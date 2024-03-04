using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.DAO;

namespace POV.CentroEducativo.DAO
{
    /// <summary>
    /// Actualiza un registro de Director en la BD
    /// </summary>
    public class DirectorUpdHlp
    {
        /// <summary>
        /// Actualiza de manera optimista un registro de Director en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="director">Director que tiene los datos nuevos</param>
        /// <param name="anterior">Director que tiene los datos anteriores</param>
        public void Action(IDataContext dctx, Director director, Director anterior)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (director == null)
                sError += ", Director";
            if (anterior == null)
                sError += ", Director anterior";
            if (sError.Length > 0)
                throw new Exception("DirectorUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (director.DirectorID == null)
                sError += ", DirectorID";
            if (director.Curp == null || director.Curp.Trim().Length == 0)
                sError += ", Curp";
            if (director.Nombre == null || director.Nombre.Trim().Length == 0)
                sError += ", Nombre";
            if (director.PrimerApellido == null || director.PrimerApellido.Trim().Length == 0)
                sError += ", PrimerApellido";
            if (director.FechaNacimiento == null)
                sError += ", FechaNacimiento";
            if (director.Sexo == null)
                sError += ", Sexo";
            if (director.FechaRegistro == null)
                sError += ", FechaRegistro";
            if (director.Estatus == null)
                sError += ", Estatus";
            if (director.Clave == null || director.Clave.Trim().Length == 0)
                sError += ", Clave";
            if (director.EstatusIdentificacion == null)
                sError += ", EstatusIdentificacion";
            if (sError.Length > 0)
                throw new Exception("DirectorUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (anterior.DirectorID == null)
                sError += ", DirectorID anterior";
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
                throw new Exception("DirectorUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO","DirectorUpdHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "DirectorInsHlp: No se pudo conectar a la base de datos", "POV.CentroEducativo.DAO","DirectorUpdHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" UPDATE Director ");
            if (director.Curp == null)
            {
                sCmd.Append(" SET Curp = NULL ");
            }
            else
            {
                sCmd.Append(" SET Curp = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = director.Curp;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (director.Nombre == null)
            {
                sCmd.Append(" ,Nombre = NULL ");
            }
            else
            {
                sCmd.Append(" ,Nombre = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = director.Nombre;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (director.PrimerApellido == null)
            {
                sCmd.Append(" ,PrimerApellido = NULL ");
            }
            else
            {
                sCmd.Append(" ,PrimerApellido = @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = director.PrimerApellido;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (director.SegundoApellido == null)
            {
                sCmd.Append(" ,SegundoApellido = NULL ");
            }
            else
            {
                // director.SegundoApellido
                sCmd.Append(" ,SegundoApellido = @dbp4ram4 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = director.SegundoApellido;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (director.FechaNacimiento == null)
            {
                sCmd.Append(" ,FechaNacimiento = NULL ");
            }
            else
            {
                sCmd.Append(" ,FechaNacimiento = @dbp4ram5 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram5";
                sqlParam.Value = director.FechaNacimiento;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (director.Sexo == null)
            {
                sCmd.Append(" ,Sexo = NULL ");
            }
            else
            {
                sCmd.Append(" ,Sexo = @dbp4ram6 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram6";
                sqlParam.Value = director.Sexo;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (director.NivelEscolar == null)
            {
                sCmd.Append(" ,NivelEscolar = NULL ");
            }
            else
            {
                sCmd.Append(" ,NivelEscolar = @dbp4ram7 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram7";
                sqlParam.Value = director.NivelEscolar;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (director.Correo == null)
            {
                sCmd.Append(" ,Correo = NULL ");
            }
            else
            {
                // director.Correo
                sCmd.Append(" ,Correo = @dbp4ram8 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram8";
                sqlParam.Value = director.Correo;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (director.Telefono == null)
            {
                sCmd.Append(" ,Telefono = NULL ");
            }
            else
            {
                // director.Telefono
                sCmd.Append(" ,Telefono = @dbp4ram9 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram9";
                sqlParam.Value = director.Telefono;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (director.Estatus == null)
            {
                sCmd.Append(" ,Estatus = NULL ");
            }
            else
            {
                sCmd.Append(" ,Estatus = @dbp4ram10 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram10";
                sqlParam.Value = director.Estatus;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (director.EstatusIdentificacion == null)
            {
                sCmd.Append(" ,EstatusIdentificacion = NULL ");
            }
            else
            {
                sCmd.Append(" ,EstatusIdentificacion = @dbp4ram11 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram11";
                sqlParam.Value = director.EstatusIdentificacion;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.DirectorID == null)
                sCmd.Append(" WHERE DirectorID IS NULL ");
            else
            {
                // anterior.DirectorID
                sCmd.Append(" WHERE DirectorID = @dbp4ram12 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram12";
                sqlParam.Value = anterior.DirectorID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.Curp == null)
                sCmd.Append(" AND Curp IS NULL ");
            else
            {
                // anterior.Curp
                sCmd.Append(" AND Curp = @dbp4ram13 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram13";
                sqlParam.Value = anterior.Curp;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.Nombre == null)
                sCmd.Append(" AND Nombre IS NULL ");
            else
            {
                // anterior.Nombre
                sCmd.Append(" AND Nombre = @dbp4ram14 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram14";
                sqlParam.Value = anterior.Nombre;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.PrimerApellido == null)
                sCmd.Append(" AND PrimerApellido IS NULL ");
            else
            {
                // anterior.PrimerApellido
                sCmd.Append(" AND PrimerApellido = @dbp4ram15 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram15";
                sqlParam.Value = anterior.PrimerApellido;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.SegundoApellido == null)
                sCmd.Append(" AND SegundoApellido IS NULL ");
            else
            {
                // anterior.SegundoApellido
                sCmd.Append(" AND SegundoApellido = @dbp4ram16 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram16";
                sqlParam.Value = anterior.SegundoApellido;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.FechaNacimiento == null)
                sCmd.Append(" AND FechaNacimiento IS NULL ");
            else
            {
                // anterior.FechaNacimiento
                sCmd.Append(" AND FechaNacimiento = @dbp4ram17 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram17";
                sqlParam.Value = anterior.FechaNacimiento;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.Sexo == null)
                sCmd.Append(" AND Sexo IS NULL ");
            else
            {
                // anterior.Sexo
                sCmd.Append(" AND Sexo = @dbp4ram18 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram18";
                sqlParam.Value = anterior.Sexo;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.NivelEscolar == null)
                sCmd.Append(" AND NivelEscolar IS NULL ");
            else
            {
                // anterior.NivelEscolar
                sCmd.Append(" AND NivelEscolar = @dbp4ram19 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram19";
                sqlParam.Value = anterior.NivelEscolar;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.Correo == null)
                sCmd.Append(" AND Correo IS NULL ");
            else
            {
                // anterior.Correo
                sCmd.Append(" AND Correo = @dbp4ram20 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram20";
                sqlParam.Value = anterior.Correo;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.Telefono == null)
                sCmd.Append(" AND Telefono IS NULL ");
            else
            {
                // anterior.Telefono
                sCmd.Append(" AND Telefono = @dbp4ram21 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram21";
                sqlParam.Value = anterior.Telefono;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.FechaRegistro == null)
                sCmd.Append(" AND FechaRegistro IS NULL ");
            else
            {
                // anterior.FechaRegistro
                sCmd.Append(" AND FechaRegistro = @dbp4ram22 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram22";
                sqlParam.Value = anterior.FechaRegistro;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.Estatus == null)
                sCmd.Append(" AND Estatus IS NULL ");
            else
            {
                // anterior.Estatus
                sCmd.Append(" AND Estatus = @dbp4ram23 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram23";
                sqlParam.Value = anterior.Estatus;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.Clave == null)
                sCmd.Append(" AND Clave IS NULL ");
            else
            {
                // anterior.Clave
                sCmd.Append(" AND Clave = @dbp4ram24 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram24";
                sqlParam.Value = anterior.Clave;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.EstatusIdentificacion == null)
                sCmd.Append(" AND EstatusIdentificacion IS NULL ");
            else
            {
                // anterior.EstatusIdentificacion
                sCmd.Append(" AND EstatusIdentificacion = @dbp4ram25 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram25";
                sqlParam.Value = anterior.EstatusIdentificacion;
                sqlParam.DbType = DbType.Boolean;
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
                throw new Exception("DirectorUpdHlp: Ocurrió un error al actualizar el registro. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("DirectorUpdHlp: Ocurrió un error al actualizar el registro.");
        }
    }
}
