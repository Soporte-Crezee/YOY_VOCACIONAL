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
    /// Consulta un registro de Alumno en la BD
    /// </summary>
    public class AlumnoRetHlp
    {
        /// <summary>
        /// Consulta registros de Alumno en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="alumno">Alumno que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de Alumno generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, Alumno alumno)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (alumno == null)
                sError += ", Alumno";
            if (sError.Length > 0)
                throw new Exception("AlumnoRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            
            if (alumno.Ubicacion == null)
                alumno.Ubicacion = new Ubicacion();

            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO", "AlumnoRetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "AlumnoRetHlp: No se pudo conectar a la base de datos", "POV.CentroEducativo.DAO", "AlumnoRetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT AlumnoID,Curp,Matricula,Nombre,PrimerApellido,SegundoApellido,FechaNacimiento,Direccion,NombreCompletoTutor,NombreCompletoTutorDos,Estatus,FechaRegistro,Sexo,EstatusIdentificacion,CorreoConfirmado,UbicacionID,Escuela,Grado,RecibirInformacion,DatosCompletos,CarreraSeleccionada,Premium,DocenteID,Credito,CreditoUsado,Saldo,NivelEscolar,EstatusPago,IDReferenciaOXXO,ReferenciaOXXO,NotificacionPago ");
            sCmd.Append(" FROM Alumno ");
            StringBuilder s_Var = new StringBuilder();
            if (alumno.AlumnoID != null)
            {
                s_Var.Append(" AlumnoID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = alumno.AlumnoID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (alumno.Curp != null)
            {
                s_Var.Append(" AND Curp = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = alumno.Curp;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (alumno.Matricula != null)
            {
                s_Var.Append(" AND Matricula = @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = alumno.Matricula;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (alumno.Nombre != null)
            {
                s_Var.Append(" AND Nombre = @dbp4ram4 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = alumno.Nombre;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (alumno.PrimerApellido != null)
            {
                s_Var.Append(" AND PrimerApellido = @dbp4ram5 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram5";
                sqlParam.Value = alumno.PrimerApellido;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (alumno.SegundoApellido != null)
            {
                s_Var.Append(" AND SegundoApellido = @dbp4ram6 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram6";
                sqlParam.Value = alumno.SegundoApellido;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (alumno.FechaNacimiento != null)
            {
                s_Var.Append(" AND FechaNacimiento = @dbp4ram7 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram7";
                sqlParam.Value = alumno.FechaNacimiento;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (alumno.Direccion != null)
            {
                s_Var.Append(" AND Direccion = @dbp4ram8 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram8";
                sqlParam.Value = alumno.Direccion;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (alumno.NombreCompletoTutor != null)
            {
                s_Var.Append(" AND NombreCompletoTutor = @dbp4ram9 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram9";
                sqlParam.Value = alumno.NombreCompletoTutor;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (alumno.NombreCompletoTutorDos != null)
            {
                s_Var.Append(" AND NombreCompletoTutorDos = @dbp4ram10 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram10";
                sqlParam.Value = alumno.NombreCompletoTutorDos;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (alumno.Estatus != null)
            {
                s_Var.Append(" AND Estatus = @dbp4ram11 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram11";
                sqlParam.Value = alumno.Estatus;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (alumno.FechaRegistro != null)
            {
                s_Var.Append(" AND FechaRegistro = @dbp4ram12 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram12";
                sqlParam.Value = alumno.FechaRegistro;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (alumno.EstatusIdentificacion != null)
            {
                s_Var.Append(" AND EstatusIdentificacion = @dbp4ram13 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram13";
                sqlParam.Value = alumno.EstatusIdentificacion;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (alumno.CorreoConfirmado != null)
            {
                s_Var.Append(" AND CorreoConfirmado = @dbp4ram14 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram14";
                sqlParam.Value = alumno.CorreoConfirmado;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (alumno.Ubicacion.UbicacionID != null)
            {
                s_Var.Append(" AND UbicacionID= @dbp4ram15 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram15";
                sqlParam.Value = alumno.Ubicacion.UbicacionID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (alumno.Escuela != null)
            {
                s_Var.Append(" AND Escuela = @dbp4ram16 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram16";
                sqlParam.Value = alumno.Escuela;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (alumno.Grado != null)
            {
                s_Var.Append(" AND Grado = @dbp4ram17 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram17";
                sqlParam.Value = alumno.Grado;
                sqlParam.DbType = DbType.Byte;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (alumno.RecibirInformacion != null)
            {
                s_Var.Append(" AND RecibirInformacion = @dbp4ram18 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram18";
                sqlParam.Value = alumno.RecibirInformacion;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (alumno.DatosCompletos != null)
            {
                s_Var.Append(" AND DatosCompletos = @dbp4ram19 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram19";
                sqlParam.Value = alumno.DatosCompletos;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (alumno.CarreraSeleccionada != null)
            {
                s_Var.Append(" AND CarreraSeleccionada = @dbp4ram20 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram20";
                sqlParam.Value = alumno.CarreraSeleccionada;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (alumno.Premium != null)
            {
                s_Var.Append(" AND Premium = @dbp4ram21 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram21";
                sqlParam.Value = alumno.Premium;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (alumno.ReferenciaOXXO != null)
            {
                s_Var.Append(" AND ReferenciaOXXO = @dbp4ram22 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram22";
                sqlParam.Value = alumno.ReferenciaOXXO;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }

            if (alumno.IDReferenciaOXXO != null)
            {
                s_Var.Append(" AND IDReferenciaOXXO = @dbp4ram23 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram23";
                sqlParam.Value = alumno.IDReferenciaOXXO;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }

            if (alumno.NotificacionPago != null)
            {
                s_Var.Append(" AND NotificacionPago = @dbp4ram24 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram24";
                sqlParam.Value = alumno.NotificacionPago;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            

            s_Var.Append("  ");
            string s_Varres = s_Var.ToString().Trim();
            if (s_Varres.Length > 0)
            {
                if (s_Varres.StartsWith("AND "))
                    s_Varres = s_Varres.Substring(4);
                else if (s_Varres.StartsWith("OR "))
                    s_Varres = s_Varres.Substring(3);
                else if (s_Varres.StartsWith(","))
                    s_Varres = s_Varres.Substring(1);
                sCmd.Append(" WHERE " + s_Varres);
            }
            DataSet ds = new DataSet();
            DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
            sqlAdapter.SelectCommand = sqlCmd;
            try
            {
                sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
                sqlAdapter.Fill(ds, "Alumno");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("AlumnoRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            return ds;
        }
    }
}
