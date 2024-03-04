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
    /// Guarda un registro de Docente en la BD
    /// </summary>
    public class DocenteInsHlp
    {
        /// <summary>
        /// Crea un registro de Docente en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="docente">Docente que desea crear</param>
        public void Action(IDataContext dctx, Docente docente)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (docente == null)
                sError += ", Docente";
            if (sError.Length > 0)
                throw new Exception("DocenteInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));

            if (sError.Length > 0)
                throw new Exception("DocenteInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO", "DocenteInsHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "DocenteInsHlp: No se pudo conectar a la base de datos", "POV.CentroEducativo.DAO", "DocenteInsHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" INSERT INTO Docente ( Nombre, CURP, PrimerApellido, SegundoApellido, Estatus, FechaRegistro, FechaNacimiento, Sexo, Correo, Clave, EstatusIdentificacion, ");
            sCmd.Append(" Cedula,NivelEstudio,Titulo,Especialidades,Experiencia,Cursos, UsuarioSkype, EsPremium) ");             
            sCmd.Append(" VALUES ");
            sCmd.Append(" ( ");
            // docente.Nombre
            sCmd.Append(" @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            if (docente.Nombre == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = docente.Nombre;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            // docente.Curp
            sCmd.Append(" ,@dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            if (docente.Curp == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = docente.Curp;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            // docente.PrimerApellido
            sCmd.Append(" ,@dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            if (docente.PrimerApellido == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = docente.PrimerApellido;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            // docente.SegundoApellido
            sCmd.Append(" ,@dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            if (docente.SegundoApellido == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = docente.SegundoApellido;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            // docente.Estatus
            sCmd.Append(" ,@dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            if (docente.Estatus == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = docente.Estatus;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
            // docente.FechaRegistro
            sCmd.Append(" ,@dbp4ram6 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram6";
            if (docente.FechaRegistro == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = docente.FechaRegistro;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
            // docente.FechaNacimiento
            sCmd.Append(" ,@dbp4ram7 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram7";
            if (docente.FechaNacimiento == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = docente.FechaNacimiento;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
            // docente.Sexo
            sCmd.Append(" ,@dbp4ram8 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram8";
            if (docente.Sexo == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = docente.Sexo;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
            // docente.Correo
            sCmd.Append(" ,@dbp4ram9 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram9";
            if (docente.Correo == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = docente.Correo;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            // docente.Clave
            sCmd.Append(" ,@dbp4ram10 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram10";
            if (docente.Clave == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = docente.Clave;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            // docente.EstatusIdentificacion
            sCmd.Append(" ,@dbp4ram11 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram11";
            if (docente.EstatusIdentificacion == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = docente.EstatusIdentificacion;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
            // docente.Cedula
            sCmd.Append(" ,@dbp4ram12 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram12";
            if (docente.Cedula== null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = docente.Cedula;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            // docente.NivelEstudio
            sCmd.Append(" ,@dbp4ram13 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram13";
            if (docente.NivelEstudio == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = docente.NivelEstudio;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            // docente.Titulo
            sCmd.Append(" ,@dbp4ram14 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram14";
            if (docente.Titulo == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = docente.Titulo;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            // docente.Especialidades
            sCmd.Append(" ,@dbp4ram15 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram15";
            if (docente.Especialidades == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = docente.Especialidades;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            // docente.Experiencia
            sCmd.Append(" ,@dbp4ram16 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram16";
            if (docente.Experiencia == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = docente.Experiencia;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            // docente.Cursos
            sCmd.Append(" ,@dbp4ram17 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram17";
            if (docente.Cursos == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = docente.Cursos;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            // docente.UsuarioSkype
            sCmd.Append(" ,@dbp4ram18 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram18";
            if (docente.UsuarioSkype == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = docente.UsuarioSkype;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            // docente.EsPremium
            sCmd.Append(" ,@dbp4ram19 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram19";
            if (docente.EsPremium == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = docente.EsPremium;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
            sCmd.Append(" ) ");
           
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
                throw new Exception("DocenteInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("DocenteInsHlp: Ocurrió un error al ingresar el registro.");
        }
    }
}
