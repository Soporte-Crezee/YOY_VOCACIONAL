// Clase motor de red social
using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.DAO;
using POV.Localizacion.Service;
using POV.Localizacion.BO;

namespace POV.CentroEducativo.DAO
{
    /// <summary>
    /// Actualiza un registro de Docente en la BD
    /// </summary>
    public class DocenteUpdHlp
    {
        /// <summary>
        /// Actualiza de manera optimista un registro de DocenteUpdHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="docente">DocenteUpdHlp que tiene los datos nuevos</param>
        /// <param name="anterior">DocenteUpdHlp que tiene los datos anteriores</param>
        public void Action(IDataContext dctx, Docente docente, Docente anterior)
        {
            object myFirm = new object();
            String sError = string.Empty;
            if (docente == null)
                sError += ", Docente";
            if (anterior == null)
                sError += ", Anterior";
            if (sError.Length > 0)
                throw new Exception("DocenteUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (docente.DocenteID == null)
                sError += ", DocenteID";
            if (docente.Curp == null || docente.Curp.Trim().Length == 0)
                sError += ", Curp";
            if (docente.Nombre == null || docente.Nombre.Trim().Length == 0)
                sError += ", Nombre";
            if (docente.PrimerApellido == null || docente.PrimerApellido.Trim().Length == 0)
                sError += ", PrimerApellido";
            if (docente.Sexo == null)
                sError += ", Sexo";
            if (docente.FechaRegistro == null)
                sError += ", FechaRegistro";
            if (docente.Estatus == null)
                sError += ", Estatus";
            if (docente.Clave == null || docente.Clave.Trim().Length == 0)
                sError += ", Clave";
            if (docente.EstatusIdentificacion == null)
                sError += ", EstatusIdentificacion";
            if (sError.Length > 0)
                throw new Exception("DocenteUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (anterior.DocenteID == null)
                sError += ", DirectorID anterior";
            if (anterior.Curp == null || anterior.Curp.Trim().Length == 0)
                sError += ", Curp anterior";
            if (anterior.Nombre == null || anterior.Nombre.Trim().Length == 0)
                sError += ", Nombre anterior";
            if (anterior.PrimerApellido == null || anterior.PrimerApellido.Trim().Length == 0)
                sError += ", PrimerApellido anterior";
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
                throw new Exception("DocenteUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO", "DocenteUpdHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "DocenteUpdHlpUpdHl: Hubo un error al conectarse a la base de datos", "POV.CentroEducativo.DAO", "DocenteUpdHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" UPDATE Docente ");
            if (docente.Curp == null)
            {
                sCmd.Append(" SET Curp = NULL ");
            }
            else
            {
                sCmd.Append(" SET Curp = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = docente.Curp;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (docente.Nombre == null)
                sCmd.Append(" ,Nombre = NULL ");
            else
            {
                sCmd.Append(" ,Nombre = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = docente.Nombre;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (docente.PrimerApellido == null)
                sCmd.Append(" ,PrimerApellido = NULL ");
            else
            {
                sCmd.Append(" ,PrimerApellido = @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = docente.PrimerApellido;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (docente.SegundoApellido == null)
                sCmd.Append(" ,SegundoApellido = NULL ");
            else
            {
                sCmd.Append(" ,SegundoApellido = @dbp4ram4 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = docente.SegundoApellido;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (docente.FechaNacimiento == null)
            {
                sCmd.Append(" ,FechaNacimiento = NULL ");
            }
            else
            {
                sCmd.Append(" ,FechaNacimiento = @dbp4ram5 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram5";
                sqlParam.Value = docente.FechaNacimiento;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (docente.Sexo == null)
            {
                sCmd.Append(" ,Sexo = NULL ");
            }
            else
            {
                sCmd.Append(" ,Sexo = @dbp4ram6 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram6";
                sqlParam.Value = docente.Sexo;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (docente.Correo == null)
            {
                sCmd.Append(" ,Correo = NULL ");
            }
            else
            {
                // director.Correo
                sCmd.Append(" ,Correo = @dbp4ram7 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram7";
                sqlParam.Value = docente.Correo;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (docente.Estatus == null)
                sCmd.Append(" ,Estatus = NULL ");
            else
            {
                sCmd.Append(" ,Estatus = @dbp4ram9 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram9";
                sqlParam.Value = docente.Estatus;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (docente.EstatusIdentificacion == null)
            {
                sCmd.Append(" ,EstatusIdentificacion = NULL ");
            }
            else
            {
                sCmd.Append(" ,EstatusIdentificacion = @dbp4ram10 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram10";
                sqlParam.Value = docente.EstatusIdentificacion;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (docente.Cedula == null)
            {
                sCmd.Append(" ,Cedula = NULL ");
            }
            else
            {
                sCmd.Append(" ,Cedula = @dbp4ram31 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram31";
                sqlParam.Value = docente.Cedula;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (docente.NivelEstudio == null)
            {
                sCmd.Append(" ,NivelEstudio = NULL ");
            }
            else
            {
                sCmd.Append(" ,NivelEstudio = @dbp4ram32 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram32";
                sqlParam.Value = docente.NivelEstudio;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (docente.Titulo == null)
            {
                sCmd.Append(" ,Titulo = NULL ");
            }
            else
            {
                sCmd.Append(" ,Titulo = @dbp4ram33 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram33";
                sqlParam.Value = docente.Titulo;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (docente.Especialidades == null)
            {
                sCmd.Append(" ,Especialidades = NULL ");
            }
            else
            {
                sCmd.Append(" ,Especialidades = @dbp4ram34 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram34";
                sqlParam.Value = docente.Especialidades;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (docente.Experiencia == null)
            {
                sCmd.Append(" ,Experiencia = NULL ");
            }
            else
            {
                sCmd.Append(" ,Experiencia = @dbp4ram35 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram35";
                sqlParam.Value = docente.Experiencia;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (docente.Cursos == null)
            {
                sCmd.Append(" ,Cursos = NULL ");
            }
            else
            {
                sCmd.Append(" ,Cursos = @dbp4ram36 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram36";
                sqlParam.Value = docente.Cursos;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (docente.UsuarioSkype == null)
            {
                sCmd.Append(" ,UsuarioSkype = NULL ");
            }
            else
            {
                sCmd.Append(" ,UsuarioSkype = @dbp4ram37 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram37";
                sqlParam.Value = docente.UsuarioSkype;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (docente.EsPremium == null)
            {
                sCmd.Append(" ,EsPremium = 0 ");
            }
            else
            {
                sCmd.Append(" ,EsPremium = @dbp4ram38 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram38";
                sqlParam.Value = docente.EsPremium;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            
            
            if (anterior.DocenteID == null)
                sCmd.Append(" WHERE DocenteID IS NULL ");
            else
            {
                sCmd.Append(" WHERE DocenteID = @dbp4ram22 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram22";
                sqlParam.Value = anterior.DocenteID;
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
                throw new Exception("DocenteUpdHlp: Hubo  un Error al Actualizar el Registro . " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("DocenteUpdHlp: Hubo  un Error al Actualizar el Registro .");
        }
    }
}
