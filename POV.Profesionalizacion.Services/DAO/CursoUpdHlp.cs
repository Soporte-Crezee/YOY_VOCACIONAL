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
    /// Actualiza un registro de Curso en la BD
    /// </summary>
    internal class CursoUpdHlp
    {
        /// <summary>
        /// Actualiza de manera optimista un registro de Curso en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="curso">Curso que tiene los datos nuevos</param>
        /// <param name="anterior">Curso que tiene los datos anteriores</param>
        public void Action(IDataContext dctx, Curso curso, Curso anterior)
        {
            object myFirm = new object();
            String sError = string.Empty;
            if (curso == null)
                sError += ", Curso";
            if (anterior == null)
                sError += ", Anterior";
            if (curso.TemaCurso == null)
                sError += ", Curso";
            if (sError.Length > 0)
                throw new Exception("CursoUpdHlp: Los siguientes objetos no pueden ser vacios " + sError.Substring(2));
            if (curso.AgrupadorContenidoDigitalID == null)
                sError += ", AgrupadorContenidoDigitalID";
            if (anterior.AgrupadorContenidoDigitalID == null)
                sError += ", anterior.AgrupadorContenidoDigitalID";
            if (curso.Nombre == null || curso.Nombre.Trim().Length == 0)
                sError += ", Nombre";
            if (curso.EsPredeterminado == null)
                sError += ", EsPredeterminado";
            if (curso.FechaRegistro == null)
                sError += ", FechaRegistro";
            if (curso.Estatus == null)
                sError += ", Estatus";
            if (curso.TipoAgrupador == null)
                sError += ", TipoAgrupador";
            if (curso.TemaCurso.TemaCursoID == null)
                sError += ", TemaCursoID";
            if (curso.Presencial == null)
                sError += ", Presencial";
            if (sError.Length > 0)
                throw new Exception("CursoUpdHlp: Los siguientes campos no pueden ser vacíos " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Profesionalizacion.DAO",
                   "CursoUpdHlp", "Action", null, null);
            if (curso.AgrupadorContenidoDigitalID != anterior.AgrupadorContenidoDigitalID)
                throw new StandardException(MessageType.Error, "Campos incongruentes", "Los identificadores para actualizar no coinciden", "POV.Profesionalizacion.DAO", "CursoUpdHlp", "Action", null, null);

            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "CursoUpdHlp: Hubo un error al conectarse a la base de datos", "POV.Profesionalizacion.DAO",
                   "CursoUpdHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" UPDATE Curso ");
            if (curso.Nombre == null)
                sCmd.Append(" SET Nombre = NULL ");
            else
            {
                // curso.Nombre
                sCmd.Append(" SET Nombre = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = curso.Nombre;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (curso.EsPredeterminado == null)
                sCmd.Append(" ,EsPredeterminado = NULL ");
            else
            {
                // curso.EsPredeterminado
                sCmd.Append(" ,EsPredeterminado = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = curso.EsPredeterminado;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (curso.FechaRegistro == null)
                sCmd.Append(" ,FechaRegistro = NULL ");
            else
            {
                // curso.FechaRegistro
                sCmd.Append(" ,FechaRegistro = @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = curso.FechaRegistro;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (curso.Estatus == null)
                sCmd.Append(" ,EstatusProfesionalizacion = NULL ");
            else
            {
                // curso.Estatus
                sCmd.Append(" ,EstatusProfesionalizacion = @dbp4ram4 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = curso.Estatus;
                sqlParam.DbType = DbType.Byte;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (curso.TipoAgrupador == null)
                sCmd.Append(" ,TipoAgrupador = NULL ");
            else
            {
                // curso.TipoAgrupador
                sCmd.Append(" ,TipoAgrupador = @dbp4ram5 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram5";
                sqlParam.Value = curso.TipoAgrupador;
                sqlParam.DbType = DbType.Byte;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (curso.TemaCurso.TemaCursoID == null)
                sCmd.Append(" ,TemaCursoID = NULL ");
            else
            {
                // curso.TemaCurso.TemaCursoID
                sCmd.Append(" ,TemaCursoID = @dbp4ram6 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram6";
                sqlParam.Value = curso.TemaCurso.TemaCursoID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (curso.Presencial == null)
                sCmd.Append(" ,Presencial = NULL ");
            else
            {
                // curso.Presencial
                sCmd.Append(" ,Presencial = @dbp4ram7 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram7";
                sqlParam.Value = curso.Presencial;
                sqlParam.DbType = DbType.Byte;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (curso.Informacion == null)
                sCmd.Append(" ,Informacion = NULL ");
            else
            {
                // curso.Informacion
                sCmd.Append(" ,Informacion = @dbp4ram8 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram8";
                sqlParam.Value = curso.Informacion;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.AgrupadorContenidoDigitalID == null)
                sCmd.Append(" WHERE AgrupadorContenidoDigitalID IS NULL ");
            else
            {
                // anterior.AgrupadorContenidoDigitalID
                sCmd.Append(" WHERE AgrupadorContenidoDigitalID = @dbp4ram9 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram9";
                sqlParam.Value = anterior.AgrupadorContenidoDigitalID;
                sqlParam.DbType = DbType.Int64;
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
                throw new Exception("CursoUpdHlp: Hubo  un Error al Actualizar el Registro . " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("CursoUpdHlp: Hubo  un Error al Actualizar el Registro .");
        }
        /// <summary>
        /// Actualiza de manera optimista un registro de AgrupadorSimple en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="agrupadorSimple">AgrupadorSimple que tiene los datos nuevos</param>
        /// <param name="anterior">AgrupadorSimple que tiene los datos anteriores</param>
        public void Action(IDataContext dctx, AgrupadorSimple agrupadorSimple, AgrupadorSimple anterior)
        {
            object myFirm = new object();
            String sError = string.Empty;
            if (agrupadorSimple == null)
                sError += ", Curso";
            if (anterior == null)
                sError += ", Anterior";
            if (sError.Length > 0)
                throw new Exception("CursoUpdHlp: Los siguientes objetos no pueden ser vacios " + sError.Substring(2));
            if (agrupadorSimple.AgrupadorContenidoDigitalID == null)
                sError += ", AgrupadorContenidoDigitalID";
            if (anterior.AgrupadorContenidoDigitalID == null)
                sError += ", anterior.AgrupadorContenidoDigitalID";
            if (agrupadorSimple.Nombre == null || agrupadorSimple.Nombre.Trim().Length == 0)
                sError += ", Nombre";
            if (agrupadorSimple.EsPredeterminado == null)
                sError += ", EsPredeterminado";
            if (agrupadorSimple.FechaRegistro == null)
                sError += ", FechaRegistro";
            if (agrupadorSimple.Estatus == null)
                sError += ", Estatus";
            if (agrupadorSimple.TipoAgrupador == null)
                sError += ", TipoAgrupador";
            if (sError.Length > 0)
                throw new Exception("CursoUpdHlp: Los siguientes campos no pueden ser vacíos " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Profesionalizacion.DAO",
                   "CursoUpdHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "CursoUpdHlp: Hubo un error al conectarse a la base de datos", "POV.Profesionalizacion.DAO",
                   "CursoUpdHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" UPDATE Curso ");
            if (agrupadorSimple.Nombre == null)
                sCmd.Append(" SET Nombre = NULL ");
            else
            {
                // curso.Nombre
                sCmd.Append(" SET Nombre = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = agrupadorSimple.Nombre;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (agrupadorSimple.EsPredeterminado == null)
                sCmd.Append(" ,EsPredeterminado = NULL ");
            else
            {
                // curso.EsPredeterminado
                sCmd.Append(" ,EsPredeterminado = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = agrupadorSimple.EsPredeterminado;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (agrupadorSimple.FechaRegistro == null)
                sCmd.Append(" ,FechaRegistro = NULL ");
            else
            {
                // curso.FechaRegistro
                sCmd.Append(" ,FechaRegistro = @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = agrupadorSimple.FechaRegistro;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (agrupadorSimple.Estatus == null)
                sCmd.Append(" ,EstatusProfesionalizacion = NULL ");
            else
            {
                // curso.Estatus
                sCmd.Append(" ,EstatusProfesionalizacion = @dbp4ram4 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = agrupadorSimple.Estatus;
                sqlParam.DbType = DbType.Byte;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (agrupadorSimple.TipoAgrupador == null)
                sCmd.Append(" ,TipoAgrupador = NULL ");
            else
            {
                // curso.TipoAgrupador
                sCmd.Append(" ,TipoAgrupador = @dbp4ram5 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram5";
                sqlParam.Value = agrupadorSimple.TipoAgrupador;
                sqlParam.DbType = DbType.Byte;
                sqlCmd.Parameters.Add(sqlParam);
            }
            sCmd.Append(" ,TemaCursoID = NULL ");
            sCmd.Append(" ,Presencial = NULL ");
            sCmd.Append(" ,Informacion = NULL ");
            if (anterior.AgrupadorContenidoDigitalID == null)
                sCmd.Append(" WHERE AgrupadorContenidoDigitalID IS NULL ");
            else
            {
                // anterior.AgrupadorContenidoDigitalID
                sCmd.Append(" WHERE AgrupadorContenidoDigitalID = @dbp4ram8 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram8";
                sqlParam.Value = anterior.AgrupadorContenidoDigitalID;
                sqlParam.DbType = DbType.Int64;
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
                throw new Exception("CursoUpdHlp: Hubo  un Error al Actualizar el Registro . " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("CursoUpdHlp: Hubo  un Error al Actualizar el Registro .");
        }
    }
}
