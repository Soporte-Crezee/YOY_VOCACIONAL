using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Profesionalizacion.BO;

namespace POV.Profesionalizacion.DAO
{
    /// <summary>
    /// Guarda un registro de Curso en la BD
    /// </summary>
    internal class CursoInsHlp
    {
        /// <summary>
        /// Crea un registro de Curso en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="agrupadorContenido">Curso que desea crear</param>
        /// <param name="agrupadorPadreID">Identificador del Agrupador de Contenido Padre</param>
        public void Action(IDataContext dctx, Curso agrupadorContenido, Int64? agrupadorPadreID)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (agrupadorContenido == null)
                sError += ", Curso";
            if (agrupadorContenido.TemaCurso == null)
                sError += ", TemaCurso";
            if (sError.Length > 0)
                throw new Exception("CursoInsHlp: Los siguientes objetos no pueden ser vacios " + sError.Substring(2));
            if (agrupadorContenido.Nombre == null || agrupadorContenido.Nombre.Trim().Length == 0)
                sError += ", Nombre";
            if (agrupadorContenido.EsPredeterminado == null)
                sError += ", EsPredeterminado";
            if (agrupadorContenido.FechaRegistro == null)
                sError += ", FechaRegistro";
            if (agrupadorContenido.Estatus == null)
                sError += ", Estatus";
            if (agrupadorContenido.TipoAgrupador == null)
                sError += ", TipoAgrupador";
            if (agrupadorContenido.TemaCurso.TemaCursoID == null)
                sError += ", TemaCursoID";
            if (agrupadorContenido.Presencial == null)
                sError += ", Presencial";
            if (sError.Length > 0)
                throw new Exception("CursoInsHlp: Los siguientes campos no pueden ser vacíos " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Profesionalizacion.DAO",
                   "CursoInsHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "CursoInsHlp: No se pudo conectar a la base de datos", "POV.Profesionalizacion.DAO",
                   "CursoInsHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" INSERT INTO Curso (AgrupadorPadreID, Nombre, EsPredeterminado, FechaRegistro, EstatusProfesionalizacion, TipoAgrupador, TemaCursoID, Presencial, Informacion) ");
            sCmd.Append(" VALUES ");
            sCmd.Append(" ( ");
            // curso.AgrupadorPadreID
            sCmd.Append(" @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            if (agrupadorPadreID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = agrupadorPadreID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
            // curso.Nombre
            sCmd.Append(" ,@dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            if (agrupadorContenido.Nombre == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = agrupadorContenido.Nombre;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            // curso.EsPredeterminado
            sCmd.Append(" ,@dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            if (agrupadorContenido.EsPredeterminado == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = agrupadorContenido.EsPredeterminado;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
            // curso.FechaRegistro
            sCmd.Append(" ,@dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            if (agrupadorContenido.FechaRegistro == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = agrupadorContenido.FechaRegistro;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
            // curso.Estatus
            sCmd.Append(" ,@dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            if (agrupadorContenido.Estatus == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = agrupadorContenido.Estatus;
            sqlParam.DbType = DbType.Byte;
            sqlCmd.Parameters.Add(sqlParam);
            // curso.TipoAgrupador
            sCmd.Append(" ,@dbp4ram6 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram6";
            if (agrupadorContenido.TipoAgrupador == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = agrupadorContenido.TipoAgrupador;
            sqlParam.DbType = DbType.Byte;
            sqlCmd.Parameters.Add(sqlParam);
            // curso.TemaCurso.TemaCursoID
            sCmd.Append(" ,@dbp4ram7 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram7";
            if (agrupadorContenido.TemaCurso.TemaCursoID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = agrupadorContenido.TemaCurso.TemaCursoID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            // curso.Presencial
            sCmd.Append(" ,@dbp4ram8 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram8";
            if (agrupadorContenido.Presencial == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = agrupadorContenido.Presencial;
            sqlParam.DbType = DbType.Byte;
            sqlCmd.Parameters.Add(sqlParam);
            // curso.Informacion
            sCmd.Append(" ,@dbp4ram9 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram9";
            if (agrupadorContenido.Informacion == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = agrupadorContenido.Informacion;
            sqlParam.DbType = DbType.String;
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
                throw new Exception("CursoInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("CursoInsHlp: Ocurrió un error al ingresar el registro.");
        }
        /// <summary>
        /// Crea un registro de AgrupadorSimple para un Curso dado
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="agrupadorContenido">Agrupador de contenido que desea crear para el CursoInsHlp</param>
        /// <param name="agrupadorPadreID">Identificador del Agrupador Padre</param>
        public void Action(IDataContext dctx, AgrupadorSimple agrupadorContenido, Int64 agrupadorPadreID)
        {
            if (agrupadorPadreID == null)
            {
                // do it something
            }
            object myFirm = new object();
            string sError = String.Empty;
            if (agrupadorContenido == null)
                sError += ", Curso";
            if (sError.Length > 0)
                throw new Exception("CursoInsHlp: Los siguientes objetos no pueden ser vacios " + sError.Substring(2));
            if (agrupadorContenido.Nombre == null || agrupadorContenido.Nombre.Trim().Length == 0)
                sError += ", Nombre";
            if (agrupadorContenido.EsPredeterminado == null)
                sError += ", EsPredeterminado";
            if (agrupadorContenido.FechaRegistro == null)
                sError += ", FechaRegistro";
            if (agrupadorContenido.Estatus == null)
                sError += ", Estatus";
            if (agrupadorContenido.TipoAgrupador == null)
                sError += ", TipoAgrupador";
            if (sError.Length > 0)
                throw new Exception("CursoInsHlp: Los siguientes campos no pueden ser vacíos " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Profesionalizacion.DAO",
                   "CursoInsHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "CursoInsHlp: No se pudo conectar a la base de datos", "POV.Profesionalizacion.DAO",
                   "CursoInsHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" INSERT INTO Curso (AgrupadorPadreID, Nombre, EsPredeterminado, FechaRegistro, EstatusProfesionalizacion, TipoAgrupador, TemaCursoID, Presencial, Informacion) ");
            sCmd.Append(" VALUES ");
            sCmd.Append(" ( ");
            // curso.AgrupadorPadreID
            sCmd.Append(" @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            if (agrupadorPadreID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = agrupadorPadreID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
            // curso.Nombre
            sCmd.Append(" ,@dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            if (agrupadorContenido.Nombre == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = agrupadorContenido.Nombre;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            // curso.EsPredeterminado
            sCmd.Append(" ,@dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            if (agrupadorContenido.EsPredeterminado == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = agrupadorContenido.EsPredeterminado;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
            // curso.FechaRegistro
            sCmd.Append(" ,@dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            if (agrupadorContenido.FechaRegistro == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = agrupadorContenido.FechaRegistro;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
            // curso.Estatus
            sCmd.Append(" ,@dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            if (agrupadorContenido.Estatus == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = agrupadorContenido.Estatus;
            sqlParam.DbType = DbType.Byte;
            sqlCmd.Parameters.Add(sqlParam);
            // curso.TipoAgrupador
            sCmd.Append(" ,@dbp4ram6 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram6";
            if (agrupadorContenido.TipoAgrupador == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = agrupadorContenido.TipoAgrupador;
            sqlParam.DbType = DbType.Byte;
            sqlCmd.Parameters.Add(sqlParam);
            // curso.TemaCurso.TemaCursoID
            sCmd.Append(" ,@dbp4ram7 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram7";
            sqlParam.Value = DBNull.Value;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            // curso.Presencial
            sCmd.Append(" ,@dbp4ram8 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram8";
            sqlParam.Value = DBNull.Value;
            sqlParam.DbType = DbType.Byte;
            sqlCmd.Parameters.Add(sqlParam);
            // curso.Informacion
            sCmd.Append(" ,@dbp4ram9 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram9";
            sqlParam.Value = DBNull.Value;
            sqlParam.DbType = DbType.String;
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
                throw new Exception("CursoInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("CursoInsHlp: Ocurrió un error al ingresar el registro.");
        }
    }
}
