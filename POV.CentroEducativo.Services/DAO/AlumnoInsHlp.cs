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
    /// Guarda un registro de Alumno en la BD
    /// </summary>
    public class AlumnoInsHlp
    {
        /// <summary>
        /// Crea un registro de Alumno en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="alumno">Alumno que desea crear</param>
        public void Action(IDataContext dctx, Alumno alumno)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (alumno == null)
                sError += ", Alumno";
            if (sError.Length > 0)
                throw new Exception("AlumnotInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO","AlumnoInsHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "AlumnoInsHlp: No se pudo conectar a la base de datos", "POV.CentroEducativo.DAO", "AlumnoInsHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" INSERT INTO Alumno (Curp,Matricula,Nombre,PrimerApellido,SegundoApellido,FechaNacimiento,Direccion,NombreCompletoTutor,NombreCompletoTutorDos,Estatus,FechaRegistro,Sexo,EstatusIdentificacion,CorreoConfirmado,UbicacionID,Escuela,Grado,RecibirInformacion,DatosCompletos,CarreraSeleccionada,Premium, NivelEscolar, EstatusPago, IDReferenciaOXXO, ReferenciaOXXO,NotificacionPago) ");
            sCmd.Append(" VALUES ");
            sCmd.Append(" ( ");
            // alumno.Curp
            sCmd.Append(" @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            if (alumno.Curp == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = alumno.Curp;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            // alumno.Matricula
            sCmd.Append(" ,@dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            if (alumno.Matricula == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = alumno.Matricula;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            // alumno.Nombre
            sCmd.Append(" ,@dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            if (alumno.Nombre == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = alumno.Nombre;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            // alumno.PrimerApellido
            sCmd.Append(" ,@dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            if (alumno.PrimerApellido == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = alumno.PrimerApellido;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            // alumno.SegundoApellido
            sCmd.Append(" ,@dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            if (alumno.SegundoApellido == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = alumno.SegundoApellido;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            // alumno.FechaNacimiento
            sCmd.Append(" ,@dbp4ram6 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram6";
            if (alumno.FechaNacimiento == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = alumno.FechaNacimiento;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
            // alumno.Direccion
            sCmd.Append(" ,@dbp4ram7 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram7";
            if (alumno.Direccion == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = alumno.Direccion;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            // alumno.NombreCompletoTutor
            sCmd.Append(" ,@dbp4ram8 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram8";
            if (alumno.NombreCompletoTutor == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = alumno.NombreCompletoTutor;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            // alumno.NombreCompletoTutorDos
            sCmd.Append(" ,@dbp4ram9 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram9";
            if (alumno.NombreCompletoTutorDos == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = alumno.NombreCompletoTutorDos;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            // alumno.Estatus
            sCmd.Append(" ,@dbp4ram10 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram10";
            if (alumno.Estatus == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = alumno.Estatus;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
            // alumno.FechaRegistro
            sCmd.Append(" ,@dbp4ram11 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram11";
            if (alumno.FechaRegistro == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = alumno.FechaRegistro;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
            // alumno.Sexo
            sCmd.Append(" ,@dbp4ram12 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram12";
            if (alumno.Sexo == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = alumno.Sexo;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
            // alumno.EstatusIdentificacion
            sCmd.Append(" ,@dbp4ram13 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram13";
            if (alumno.EstatusIdentificacion == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = alumno.EstatusIdentificacion;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
            //alumno.CorreoConfirmado
            sCmd.Append(" ,@dbp4ram14 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram14";
            if (alumno.CorreoConfirmado == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = alumno.CorreoConfirmado;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
            // alumno.Ubicacion
            sCmd.Append(" ,@dbp4ram15 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram15";
            if (alumno.Ubicacion == null || alumno.Ubicacion.UbicacionID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = alumno.Ubicacion.UbicacionID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
            // alumno.Escuela
            sCmd.Append(" ,@dbp4ram16 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram16";
            if (alumno.Escuela == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = alumno.Escuela;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            // alumno.Grado
            sCmd.Append(" ,@dbp4ram17 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram17";
            if (alumno.Grado == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = alumno.Grado;
            sqlParam.DbType = DbType.Byte;
            sqlCmd.Parameters.Add(sqlParam);
            //alumno.RecibirInformacion
            sCmd.Append(" ,@dbp4ram18 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram18";
            if (alumno.RecibirInformacion == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = alumno.RecibirInformacion;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
            //alumno.DatosCompletos
            sCmd.Append(" ,@dbp4ram19 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram19";
            if (alumno.DatosCompletos == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = alumno.DatosCompletos;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
            //alumno.CarreraSeleccionada
            sCmd.Append(" ,@dbp4ram20 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram20";
            if (alumno.CarreraSeleccionada == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = alumno.CarreraSeleccionada;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
            //alumno.Premium
            sCmd.Append(" ,@dbp4ram21 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram21";
            if (alumno.Premium == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = alumno.Premium;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
            //alumno.NivelEscolar
            sCmd.Append(" ,@dbp4ram22 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram22";
            if (alumno.NivelEscolar == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = alumno.NivelEscolar;
            sqlParam.DbType = DbType.Byte;
            sqlCmd.Parameters.Add(sqlParam);
            //alumno.EstatusPago
            sCmd.Append(" ,@dbp4ram23 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram23";
            if (alumno.EstatusPago == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = alumno.EstatusPago;
            sqlParam.DbType = DbType.Byte;
            sqlCmd.Parameters.Add(sqlParam);
            //alumno.ReferenciaOXXO
            sCmd.Append(" ,@dbp4ram24 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram24";
            if (alumno.ReferenciaOXXO == null)
                sqlParam.Value = "N/A";
            else
                sqlParam.Value = alumno.ReferenciaOXXO;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            //alumno.IDReferenciaOXXO
            sCmd.Append(" ,@dbp4ram25 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram25";
            if (alumno.IDReferenciaOXXO == null)
                sqlParam.Value = "N/A";
            else
                sqlParam.Value = alumno.IDReferenciaOXXO;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            //alumno.NotificacionPago
            sCmd.Append(" ,@dbp4ram26 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram26";
            if (alumno.NotificacionPago == null)
                sqlParam.Value = false;
            else
                sqlParam.Value = alumno.NotificacionPago;
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
                throw new Exception("AlumnotInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("AlumnotInsHlp: Ocurrió un error al ingresar el registro.");
        }
    }
}
