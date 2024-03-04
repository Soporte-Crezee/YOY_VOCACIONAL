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
    /// Actualiza un registro de Alumno en la BD
    /// </summary>
    public class AlumnoUpdHlp
    {
        /// <summary>
        /// Actualiza de manera optimista un registro de AlumnoUpdHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="alumnoUpdHlp">AlumnoUpdHlp que tiene los datos nuevos</param>
        /// <param name="anterior">AlumnoUpdHlp que tiene los datos anteriores</param>
        public void Action(IDataContext dctx, Alumno alumno, Alumno anterior)
        {
            object myFirm = new object();
            String sError = string.Empty;
            if (alumno == null)
                sError += ", Alumno";
            if (anterior == null)
                sError += ", Anterior";
            if (sError.Length > 0)
                throw new Exception("AlumnoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (anterior.AlumnoID == null)
                sError += ", Anterior AlumnoID";
            if (sError.Length > 0)
                throw new Exception("AlumnoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO", "AlumnoUpdHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "AlumnoUpdHlpUpdHl: Hubo un error al conectarse a la base de datos", "POV.CentroEducativo.DAO", "AlumnoUpdHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" UPDATE Alumno ");
            if (alumno.Curp == null)
                sCmd.Append(" SET Curp = NULL ");
            else
            {
                // alumno.Curp
                sCmd.Append(" SET Curp = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = alumno.Curp;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (alumno.Matricula == null)
                sCmd.Append(" ,Matricula = NULL ");
            else
            {
                // alumno.Matricula
                sCmd.Append(" ,Matricula = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = alumno.Matricula;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (alumno.Nombre == null)
                sCmd.Append(" ,Nombre = NULL ");
            else
            {
                // alumno.Nombre
                sCmd.Append(" ,Nombre = @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = alumno.Nombre;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (alumno.PrimerApellido == null)
                sCmd.Append(" ,PrimerApellido = NULL ");
            else
            {
                // alumno.PrimerApellido
                sCmd.Append(" ,PrimerApellido = @dbp4ram4 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = alumno.PrimerApellido;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (alumno.SegundoApellido == null)
                sCmd.Append(" ,SegundoApellido = NULL ");
            else
            {
                // alumno.SegundoApellido
                sCmd.Append(" ,SegundoApellido = @dbp4ram5 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram5";
                sqlParam.Value = alumno.SegundoApellido;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (alumno.FechaNacimiento == null)
                sCmd.Append(" ,FechaNacimiento = NULL ");
            else
            {
                // alumno.FechaNacimiento
                sCmd.Append(" ,FechaNacimiento = @dbp4ram6 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram6";
                sqlParam.Value = alumno.FechaNacimiento;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (alumno.Direccion == null)
                sCmd.Append(" ,Direccion = NULL ");
            else
            {
                // alumno.Direccion
                sCmd.Append(" ,Direccion = @dbp4ram7 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram7";
                sqlParam.Value = alumno.Direccion;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (alumno.NombreCompletoTutor == null)
                sCmd.Append(" ,NombreCompletoTutor = NULL ");
            else
            {
                // alumno.NombreCompletoTutor
                sCmd.Append(" ,NombreCompletoTutor = @dbp4ram8 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram8";
                sqlParam.Value = alumno.NombreCompletoTutor;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (alumno.NombreCompletoTutorDos == null)
                sCmd.Append(" ,NombreCompletoTutorDos = NULL ");
            else
            {
                // alumno.NombreCompletoTutorDos
                sCmd.Append(" ,NombreCompletoTutorDos = @dbp4ram9 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram9";
                sqlParam.Value = alumno.NombreCompletoTutorDos;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (alumno.Estatus == null)
                sCmd.Append(" ,Estatus = NULL ");
            else
            {
                // alumno.Estatus
                sCmd.Append(" ,Estatus = @dbp4ram10 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram10";
                sqlParam.Value = alumno.Estatus;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (alumno.FechaRegistro == null)
                sCmd.Append(" ,FechaRegistro = NULL ");
            else
            {
                // alumno.FechaRegistro
                sCmd.Append(" ,FechaRegistro = @dbp4ram11 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram11";
                sqlParam.Value = alumno.FechaRegistro;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (alumno.Sexo == null)
                sCmd.Append(" ,Sexo = NULL ");
            else
            {
                // alumno.Sexo
                sCmd.Append(" ,Sexo = @dbp4ram12 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram12";
                sqlParam.Value = alumno.Sexo;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (alumno.EstatusIdentificacion == null)
                sCmd.Append(" ,EstatusIdentificacion = NULL ");
            else
            {
                // alumno.EstatusIdentificacion
                sCmd.Append(" ,EstatusIdentificacion = @dbp4ram13 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram13";
                sqlParam.Value = alumno.EstatusIdentificacion;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (alumno.CorreoConfirmado == null)
                sCmd.Append(" ,CorreoConfirmado = NULL ");
            else
            {
                // alumno.CorreoConfirmado
                sCmd.Append(" ,CorreoConfirmado = @dbp4ram14 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram14";
                sqlParam.Value = alumno.CorreoConfirmado;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }

            if (alumno.Ubicacion.UbicacionID == null)
                sCmd.Append(" ,UbicacionID = NULL ");
            else
            {
                // alumno.Ubicacion.UbicacionID
                sCmd.Append(" ,UbicacionID = @dbp4ram29 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram29";
                sqlParam.Value = alumno.Ubicacion.UbicacionID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }

            if (alumno.Escuela == null)
                sCmd.Append(" ,Escuela = NULL ");
            else
            {
                // alumno.Escuela
                sCmd.Append(" ,Escuela = @dbp4ram30 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram30";
                sqlParam.Value = alumno.Escuela;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }

            if (alumno.Grado == null)
                sCmd.Append(" ,Grado = NULL ");
            else
            {
                // alumno.Grado
                sCmd.Append(" ,Grado = @dbp4ram31 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram31";
                sqlParam.Value = alumno.Grado;
                sqlParam.DbType = DbType.Byte;
                sqlCmd.Parameters.Add(sqlParam);
            }

            if (alumno.RecibirInformacion == null)
                sCmd.Append(" ,RecibirInformacion = NULL ");
            else
            {
                // alumno.RecibirInformacion
                sCmd.Append(" ,RecibirInformacion = @dbp4ram32 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram32";
                sqlParam.Value = alumno.RecibirInformacion;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }

            if (alumno.DatosCompletos == null)
                sCmd.Append(" ,DatosCompletos = NULL ");
            else
            {
                // alumno.DatosCompletos
                sCmd.Append(" ,DatosCompletos = @dbp4ram33 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram33";
                sqlParam.Value = alumno.DatosCompletos;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }

            if (alumno.CarreraSeleccionada == null)
                sCmd.Append(" ,CarreraSeleccionada = NULL ");
            else
            {
                // alumno.CarreraSeleccionada
                sCmd.Append(" ,CarreraSeleccionada = @dbp4ram34 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram34";
                sqlParam.Value = alumno.CarreraSeleccionada;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }

            if (alumno.Premium == null)
                sCmd.Append(" ,Premium = NULL ");
            else
            {
                 //alumno.Premium
                sCmd.Append(" ,Premium = @dbp4ram35 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram35";
                sqlParam.Value = alumno.Premium;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }

            if (alumno.NivelEscolar == null)
                sCmd.Append(" ,NiVelEscolar = NULL ");
            else
            {
                 //alumno.NivelEscolar
                sCmd.Append(" ,NiVelEscolar = @dbp4ram36 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram36";
                sqlParam.Value = alumno.NivelEscolar;
                sqlParam.DbType = DbType.Byte;
                sqlCmd.Parameters.Add(sqlParam);
            }

            if (alumno.EstatusPago == null)
                sCmd.Append(" ,EstatusPago = NULL ");
            else
            {
                 //alumno.EstatusPago
                sCmd.Append(" ,EstatusPago = @dbp4ram37 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram37";
                sqlParam.Value = alumno.EstatusPago;
                sqlParam.DbType = DbType.Byte;
                sqlCmd.Parameters.Add(sqlParam);
            }

            if (alumno.IDReferenciaOXXO == null)
                sCmd.Append(" ,IDRefrenciaOXXO = NULL ");
            else
            {
                // alumno.DatosCompletos
                sCmd.Append(" ,IDReferenciaOXXO = @dbp4ram38 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram38";
                sqlParam.Value = alumno.IDReferenciaOXXO;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }

            if (alumno.NotificacionPago == null)
                sCmd.Append(" ,NotificacionPago = false ");
            else
            {
                // alumno.NotificacionPago
                sCmd.Append(" ,NotificacionPago = @dbp4ram40 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram40";
                sqlParam.Value = alumno.NotificacionPago;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }

            if (alumno.ReferenciaOXXO == null)
                sCmd.Append(" ,ReferenciaOXXO = NULL ");
            else
            {
                // alumno.ReferenciaOXXO
                sCmd.Append(" ,ReferenciaOXXO = @dbp4ram39 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram39";
                sqlParam.Value = alumno.ReferenciaOXXO;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }


            if (anterior.AlumnoID == null)
                sCmd.Append(" WHERE alumnoID = NULL ");
            else
            {
                // anterior.AlumnoID
                sCmd.Append(" WHERE alumnoID = @dbp4ram15 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram15";
                sqlParam.Value = anterior.AlumnoID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.Curp == null)
                sCmd.Append(" AND Curp IS NULL ");
            else
            {
                // anterior.Curp
                sCmd.Append(" AND Curp = @dbp4ram16 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram16";
                sqlParam.Value = anterior.Curp;
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
                throw new Exception("AlumnoUpdHlp: Hubo  un Error al Actualizar el Registro . " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("AlumnoUpdHlp: Hubo  un Error al Actualizar el Registro .");
        }
    }
}
