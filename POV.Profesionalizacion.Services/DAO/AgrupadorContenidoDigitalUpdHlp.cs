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
    /// Actualiza un registro de AAgrupadorContenidoDigital en la BD
    /// </summary>
    internal class AgrupadorContenidoDigitalUpdHlp
    {
        /// <summary>
        /// Actualiza de manera optimista un registro de AAgrupadorContenidoDigital en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="agrupadorContenidoDigital">AAgrupadorContenidoDigital que tiene los datos nuevos</param>
        /// <param name="anterior">AAgrupadorContenidoDigital que tiene los datos anteriores</param>
        public void Action(IDataContext dctx, AAgrupadorContenidoDigital agrupadorContenidoDigital, AAgrupadorContenidoDigital anterior)
        {
            object myFirm = new object();
            String sError = string.Empty;
            if (agrupadorContenidoDigital == null)
                sError += ", AAgrupadorContenidoDigital";
            if (anterior == null)
                sError += ", Anterior";
            if (sError.Length > 0)
                throw new Exception("AgrupadorContenidoDigitalUpdHlp: Los siguientes objetos no pueden ser vacios " + sError.Substring(2));
            if (anterior.AgrupadorContenidoDigitalID == null)
                sError += ", Anterior.AgrupadorContenidoDigitalID";
            if (agrupadorContenidoDigital.AgrupadorContenidoDigitalID == null)
                sError += ", agrupadorContenidoDigital.AgrupadorContenidoDigitalID";
            if (agrupadorContenidoDigital.Nombre == null || agrupadorContenidoDigital.Nombre.Trim().Length == 0)
                sError += ", Nombre";
            if (agrupadorContenidoDigital.EsPredeterminado == null)
                sError += ", EsPredeterminado";
            if (agrupadorContenidoDigital.FechaRegistro == null)
                sError += ", FechaRegistro";
            if (agrupadorContenidoDigital.Estatus == null)
                sError += ", Estatus";
            if (agrupadorContenidoDigital.TipoAgrupador == null)
                sError += ", TipoAgrupador";
            if (sError.Length > 0)
                throw new Exception("AgrupadorContenidoDigitalUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Profesionalizacion.DAO",
                   "AgrupadorContenidoDigitalUpdHlp", "Action", null, null);
            if (agrupadorContenidoDigital.AgrupadorContenidoDigitalID != anterior.AgrupadorContenidoDigitalID)
                throw new StandardException(MessageType.Error, "Campos incongruentes", "Los identificadores para actualizar no coinciden", "POV.Profesionalizacion.DAO", "AgrupadorContenidoDigitalUpdHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "AgrupadorContenidoDigitalUpdHlp: Hubo un error al conectarse a la base de datos", "POV.Profesionalizacion.DAO",
                   "AgrupadorContenidoDigitalUpdHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" UPDATE AgrupadorContenidoDigital ");
            if (agrupadorContenidoDigital.Nombre == null)
                sCmd.Append(" SET Nombre = NULL ");
            else
            {
                // agrupadorContenidoDigital.Nombre
                sCmd.Append(" SET Nombre = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = agrupadorContenidoDigital.Nombre;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (agrupadorContenidoDigital.EsPredeterminado == null)
                sCmd.Append(" ,EsPredeterminado = NULL ");
            else
            {
                // agrupadorContenidoDigital.EsPredeterminado
                sCmd.Append(" ,EsPredeterminado = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = agrupadorContenidoDigital.EsPredeterminado;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (agrupadorContenidoDigital.FechaRegistro == null)
                sCmd.Append(" ,FechaRegistro = NULL ");
            else
            {
                // agrupadorContenidoDigital.FechaRegistro
                sCmd.Append(" ,FechaRegistro = @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = agrupadorContenidoDigital.FechaRegistro;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (agrupadorContenidoDigital.Estatus == null)
                sCmd.Append(" ,EstatusProfesionalizacion = NULL ");
            else
            {
                // agrupadorContenidoDigital.Estatus
                sCmd.Append(" ,EstatusProfesionalizacion = @dbp4ram4 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = agrupadorContenidoDigital.Estatus;
                sqlParam.DbType = DbType.Byte;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (agrupadorContenidoDigital.TipoAgrupador == null)
                sCmd.Append(" ,TipoAgrupador = NULL ");
            else
            {
                // agrupadorContenidoDigital.TipoAgrupador
                sCmd.Append(" ,TipoAgrupador = @dbp4ram5 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram5";
                sqlParam.Value = agrupadorContenidoDigital.TipoAgrupador;
                sqlParam.DbType = DbType.Byte;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (agrupadorContenidoDigital.Competencias == null)
                sCmd.Append(" ,Competencias = NULL ");
            else
            {
                // agrupadorContenidoDigital.Competencias
                sCmd.Append(" ,Competencias = @dbp4ram6 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram6";
                sqlParam.Value = agrupadorContenidoDigital.Competencias;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (agrupadorContenidoDigital.Aprendizajes == null)
                sCmd.Append(" ,Aprendizajes = NULL ");
            else
            {
                // agrupadorContenidoDigital.Aprendizajes
                sCmd.Append(" ,Aprendizajes = @dbp4ram7 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram7";
                sqlParam.Value = agrupadorContenidoDigital.Aprendizajes;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.AgrupadorContenidoDigitalID == null)
                sCmd.Append(" WHERE AgrupadorContenidoDigitalID = NULL ");
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
                throw new Exception("AgrupadorContenidoDigitalUpdHlp: Hubo  un Error al Actualizar el Registro . " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("AgrupadorContenidoDigitalUpdHlp: Hubo  un Error al Actualizar el Registro .");
        }
    }
}
