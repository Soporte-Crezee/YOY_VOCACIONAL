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
    /// Actualiza un registro de Escuela en la BD
    /// </summary>
    public class EscuelaUpdHlp
    {
        /// <summary>
        /// Actualiza de manera optimista un registro de Escuela en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="escuela">Escuela que tiene los datos nuevos</param>
        /// <param name="anterior">Escuela que tiene los datos anteriores</param>
        public void Action(IDataContext dctx, Escuela escuela, Escuela anterior)
        {
            object myFirm = new object();
            String sError = string.Empty;
            if (escuela == null)
                sError += ", Escuela";
            if (anterior == null)
                sError += ", Anterior";
            if (sError.Length > 0)
                throw new Exception("EscuelaUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (escuela.EscuelaID == null)
                sError += ",  EscuelaID";
            if (escuela.Clave == null || escuela.Clave.Trim().Length == 0)
                sError += ",  Clave";
            if (escuela.NombreEscuela == null || escuela.NombreEscuela.Trim().Length == 0)
                sError += ",  NombreEscuela";
            if (escuela.FechaRegistro == null)
                sError += ",  FechaRegistro";
            if (escuela.Ubicacion == null)
                sError += ", Ubicacion";
            if (escuela.Estatus == null)
                sError += ",  Estatus";
            if (escuela.Turno == null)
                sError += ",  Turno";
            if (escuela.TipoServicio == null)
                sError += ",  TipoServicio";
            if (escuela.ZonaID == null)
                sError += ",  Zona";
            if (escuela.DirectorID == null)
                sError += ",  Director";
            if (escuela.Ambito == null)
                sError += ",  Ambito";
            if (escuela.Control == null)
                sError += ",  Control";
            if (sError.Length > 0)
                throw new Exception("EscuelaUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (escuela.Ubicacion.UbicacionID == null)
                sError += ",  Ubicacion.UbicacionID";
            if (escuela.TipoServicio.TipoServicioID == null)
                sError += ",  TipoServicio.TipoServicioID";
            if (escuela.ZonaID.ZonaID == null)
                sError += ",  ZonaID.ZonaID";
            if (escuela.DirectorID.DirectorID == null)
                sError += ",  Director.DirectorID";
            if (sError.Length > 0)
                throw new Exception("EscuelaUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (anterior.EscuelaID == null)
                sError += ",  EscuelaID anterior";
            if (anterior.Clave == null || anterior.Clave.Trim().Length == 0)
                sError += ",  Clave anterior";
            if (anterior.NombreEscuela == null || anterior.NombreEscuela.Trim().Length == 0)
                sError += ",  NombreEscuela anterior";
            if (anterior.FechaRegistro == null)
                sError += ",  FechaRegistro anterior";
            if (anterior.Ubicacion == null)
                sError += ", Ubicacion anterior";
            if (anterior.Estatus == null)
                sError += ",  Estatus anterior";
            if (anterior.Turno == null)
                sError += ",  Turno anterior";
            if (anterior.TipoServicio == null)
                sError += ",  TipoServicio anterior";
            if (anterior.ZonaID == null)
                sError += ",  Zona anterior";
            if (anterior.DirectorID == null)
                sError += ",  Director anterior";
            if (anterior.Ambito == null)
                sError += ",  Ambito anterior";
            if (anterior.Control == null)
                sError += ",  Control anterior";
            if (sError.Length > 0)
                throw new Exception("EscuelaUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (anterior.Ubicacion.UbicacionID == null)
                sError += ",  Ubicacion.UbicacionID";
            if (anterior.TipoServicio.TipoServicioID == null)
                sError += ",  TipoServicio.TipoServicioID";
            if (anterior.ZonaID.ZonaID == null)
                sError += ",  ZonaID.ZonaID";
            if (anterior.DirectorID.DirectorID == null)
                sError += ",  Director.DirectorID";
            if (sError.Length > 0)
                throw new Exception("EscuelaUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO", "EscuelaUpdHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "EscuelaUpdHlpUpdHl: Hubo un error al conectarse a la base de datos", "POV.CentroEducativo.DAO", "EscuelaUpdHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" UPDATE Escuela ");
            if (escuela.Clave == null)
                sCmd.Append(" SET Clave = NULL ");
            else
            {
                // escuela.Clave
                sCmd.Append(" SET Clave = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = escuela.Clave;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (escuela.Turno == null)
                sCmd.Append(" ,Turno = NULL ");
            else
            {
                // escuela.Turno
                sCmd.Append(" ,Turno = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = escuela.Turno;
                sqlParam.DbType = DbType.Byte;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (escuela.NombreEscuela == null)
                sCmd.Append(" ,NombreEscuela = NULL ");
            else
            {
                // escuela.NombreEscuela
                sCmd.Append(" ,NombreEscuela = @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = escuela.NombreEscuela;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (escuela.Ambito == null)
                sCmd.Append(" ,Ambito = NULL ");
            else
            {
                // escuela.Ambito
                sCmd.Append(" ,Ambito = @dbp4ram4 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = (byte)escuela.Ambito;
                sqlParam.DbType = DbType.Byte;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (escuela.Ambito == null)
                sCmd.Append(" ,Control = NULL ");
            else
            {
                // escuela.Control
                sCmd.Append(" ,Control =@dbp4ram5 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram5";
                sqlParam.Value = (byte)escuela.Control;
                sqlParam.DbType = DbType.Byte;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (escuela.TipoServicio.TipoServicioID == null)
                sCmd.Append(" ,TipoServicioID = NULL ");
            else
            {
                // escuela.TipoServicio.TipoServicioID
                sCmd.Append(" ,TipoServicioID =@dbp4ram6 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram6";
                sqlParam.Value = escuela.TipoServicio.TipoServicioID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (escuela.Estatus == null)
                sCmd.Append(" ,Estatus = NULL ");
            else
            {
                // escuela.Estatus
                sCmd.Append(" ,Estatus = @dbp4ram7 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram7";
                sqlParam.Value = escuela.Estatus;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (escuela.ZonaID.ZonaID == null)
                sCmd.Append(" ,ZonaID = NULL ");
            else
            {
                // escuela.ZonaID.ZonaID
                sCmd.Append(" ,ZonaID = @dbp4ram8 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram8";
                sqlParam.Value = escuela.ZonaID.ZonaID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (escuela.Ubicacion.UbicacionID == null)
                sCmd.Append(" ,UbicacionID = NULL ");
            else
            {
                // escuela.Ubicacion.UbicacionID
                sCmd.Append(" ,UbicacionID = @dbp4ram9 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram9";
                sqlParam.Value = escuela.Ubicacion.UbicacionID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (escuela.DirectorID.DirectorID == null)
                sCmd.Append(" ,DirectorID = NULL ");
            else
            {
                // escuela.Director.DirectorID
                sCmd.Append(" ,DirectorID = @dbp4ram10 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram10";
                sqlParam.Value = escuela.DirectorID.DirectorID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.EscuelaID == null)
                sCmd.Append(" WHERE EscuelaID IS NULL ");
            else
            {
                // anterior.EscuelaID
                sCmd.Append(" WHERE EscuelaID = @dbp4ram11 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram11";
                sqlParam.Value = anterior.EscuelaID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.Clave == null)
                sCmd.Append(" AND Clave IS NULL ");
            else
            {
                // anterior.Clave
                sCmd.Append(" AND Clave = @dbp4ram12 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram12";
                sqlParam.Value = anterior.Clave;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.Turno == null)
                sCmd.Append(" AND Turno IS NULL ");
            else
            {
                // anterior.Turno
                sCmd.Append(" AND Turno = @dbp4ram13 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram13";
                sqlParam.Value = anterior.Turno;
                sqlParam.DbType = DbType.Byte;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.FechaRegistro != null)
            {
                sCmd.Append(" AND FechaRegistro= @dbp4ram14 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram14";
                sqlParam.Value = anterior.FechaRegistro;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.NombreEscuela == null)
                sCmd.Append(" AND NombreEscuela IS NULL ");
            else
            {
                // anterior.NombreEscuela
                sCmd.Append(" AND NombreEscuela = @dbp4ram15 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram15";
                sqlParam.Value = anterior.NombreEscuela;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.Ambito == null)
                sCmd.Append(" AND Ambito IS NULL ");
            else
            {
                // anterior.Ambito
                sCmd.Append(" AND Ambito = @dbp4ram16 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram16";
                sqlParam.Value = (byte)anterior.Ambito;
                sqlParam.DbType = DbType.Byte;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.Ambito == null)
                sCmd.Append(" AND Control IS NULL ");
            else
            {
                // anterior.Control
                sCmd.Append(" AND Control =@dbp4ram17 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram17";
                sqlParam.Value = (byte)anterior.Control;
                sqlParam.DbType = DbType.Byte;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.TipoServicio.TipoServicioID == null)
                sCmd.Append(" AND TipoServicioID IS NULL ");
            else
            {
                // anterior.TipoServicio.TipoServicioID
                sCmd.Append(" AND TipoServicioID =@dbp4ram18 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram18";
                sqlParam.Value = anterior.TipoServicio.TipoServicioID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.Estatus == null)
                sCmd.Append(" AND Estatus IS NULL ");
            else
            {
                // anterior.Clave
                sCmd.Append(" AND Estatus = @dbp4ram19 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram19";
                sqlParam.Value = anterior.Estatus;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.ZonaID.ZonaID == null)
                sCmd.Append(" AND ZonaID IS NULL ");
            else
            {
                // anterior.ZonaID.ZonaID
                sCmd.Append(" AND ZonaID = @dbp4ram20 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram20";
                sqlParam.Value = anterior.ZonaID.ZonaID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.Ubicacion.UbicacionID == null)
                sCmd.Append(" AND UbicacionID IS NULL ");
            else
            {
                // anterior.Ubicacion.UbicacionID
                sCmd.Append(" AND UbicacionID = @dbp4ram21 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram21";
                sqlParam.Value = anterior.Ubicacion.UbicacionID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.DirectorID.DirectorID == null)
                sCmd.Append(" AND DirectorID IS NULL ");
            else
            {
                // anterior.Director.DirectorID
                sCmd.Append(" AND DirectorID = @dbp4ram22 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram22";
                sqlParam.Value = anterior.DirectorID.DirectorID;
                sqlParam.DbType = DbType.Int32;
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
                throw new Exception("EscuelaUpdHlp: Hubo  un Error al Actualizar el Registro . " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("EscuelaUpdHlp: Hubo  un Error al Actualizar el Registro .");
        }
    }
}
