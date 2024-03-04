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
    /// Guarda un registro de Escuela en la BD
    /// </summary>
    public class EscuelaInsHlp
    {
        /// <summary>
        /// Crea un registro de Escuela en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="escuela">Escuela que desea crear</param>
        public void Action(IDataContext dctx, Escuela escuela)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (escuela == null)
                sError += ", Escuela";
            if (sError.Length > 0)
                throw new Exception("EscuelaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (escuela.Ubicacion == null)
                sError += ", Ubicacion";
            if (sError.Length > 0)
                throw new Exception("EscuelaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (escuela.Ubicacion.UbicacionID == null)
                sError += ", UbicacionID";
            if (sError.Length > 0)
                throw new Exception("EscuelaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
           
            if (escuela.Clave == null)
                sError += ", Clave";
            if (sError.Length > 0)
                throw new Exception("EscuelaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (escuela.NombreEscuela == null)
                sError += ", NombreEscuela";
            if (sError.Length > 0)
                throw new Exception("EscuelaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (escuela.FechaRegistro == null)
                sError += ", FechaRegistro";
            if (sError.Length > 0)
                throw new Exception("EscuelaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (escuela.Estatus == null)
                sError += ", Estatus";
            if (sError.Length > 0)
                throw new Exception("EscuelaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (escuela.ToShortTurno == null)
                sError += ", Turno";
            if (sError.Length > 0)
                throw new Exception("EscuelaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (escuela.TipoServicio.TipoServicioID == null)
                sError += ", TipoServicioID";
            if (sError.Length > 0)
                throw new Exception("EscuelaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (escuela.ZonaID.ZonaID == null)
                sError += ", ZonaID";
            if (sError.Length > 0)
                throw new Exception("EscuelaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (escuela.DirectorID.DirectorID == null)
                sError += ", DirectorID";
            if (sError.Length > 0)
                throw new Exception("EscuelaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (escuela.ToShortAmbito == null)
                sError += ", Ambito";
            if (sError.Length > 0)
                throw new Exception("EscuelaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (escuela.ToShortControl == null)
                sError += ", Control";
            if (sError.Length > 0)
                throw new Exception("EscuelaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));

            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO",
                   "EscuelaInsHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "EscuelaInsHlp: No se pudo conectar a la base de datos", "POV.CentroEducativo.DAO",
                   "EscuelaInsHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" INSERT INTO Escuela (Clave,NombreEscuela,FechaRegistro,UbicacionID,Estatus,Turno,TipoServicioID,ZonaID,Ambito,DirectorID,Control) ");
            sCmd.Append(" VALUES ");
            sCmd.Append(" ( ");
            // escuela.Clave
            sCmd.Append(" @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            if (escuela.Clave == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = escuela.Clave;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            // escuela.NombreEscuela
            sCmd.Append(" ,@dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            if (escuela.NombreEscuela == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = escuela.NombreEscuela;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            // escuela.FechaRegistro
            sCmd.Append(" ,@dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            if (escuela.FechaRegistro == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = escuela.FechaRegistro;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
            // escuela.Ubicacion
            sCmd.Append(" ,@dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            if (escuela.Ubicacion.UbicacionID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = escuela.Ubicacion.UbicacionID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
            // escuela.Estatus
            sCmd.Append(" ,@dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            if (escuela.Estatus == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = escuela.Estatus;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
            // escuela.Turno
            sCmd.Append(" ,@dbp4ram6 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram6";
            if (escuela.ToShortTurno == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value =escuela.ToShortTurno;
            sqlParam.DbType = DbType.Byte;
            sqlCmd.Parameters.Add(sqlParam);
            // escuela.TipoServicioID
            sCmd.Append(" ,@dbp4ram7 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram7";
            if (escuela.TipoServicio.TipoServicioID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = escuela.TipoServicio.TipoServicioID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            // escuela.ZonaID
            sCmd.Append(" ,@dbp4ram8 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram8";
            if (escuela.ZonaID.ZonaID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = escuela.ZonaID.ZonaID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
            // escuela.Ambito
            sCmd.Append(" ,@dbp4ram9 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram9";
            if (escuela.ToShortAmbito == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = escuela.ToShortAmbito;
            sqlParam.DbType = DbType.Byte;
            sqlCmd.Parameters.Add(sqlParam);
            // escuela.DirectorID
            sCmd.Append(" ,@dbp4ram10 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram10";
            if (escuela.DirectorID.DirectorID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = escuela.DirectorID.DirectorID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            // escuela.Control
            sCmd.Append(" ,@dbp4ram11 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram11";
            if (escuela.ToShortControl == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = escuela.ToShortControl;
            sqlParam.DbType = DbType.Byte;
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
                throw new Exception("EscuelaInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("EscuelaInsHlp: Ocurrió un error al ingresar el registro.");
        }
    }
}
