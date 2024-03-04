using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Profesionalizacion.BO;

//TODO: Finalizar este objeto

namespace POV.Profesionalizacion.DAO
{
    /// <summary>
    /// Guarda un registro de AAgrupadorContenidoDigital en la BD
    /// </summary>
    public class AgrupadorContenidoDigitalInsHlp
    {
        /// <summary>
        /// Crea un registro de AAgrupadorContenidoDigital en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="aAgrupadorContenidoDigital">AAgrupadorContenidoDigital que desea crear</param>
        public void Action(IDataContext dctx, AAgrupadorContenidoDigital aAgrupadorContenidoDigital, Int64? agrupadorPadreID)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (aAgrupadorContenidoDigital == null)
                sError += ", AAgrupadorContenidoDigital";
            if (sError.Length > 0)
                throw new Exception("AgrupadorContenidoDigitalInsHlp: Los siguientes objetos no pueden ser vacios " + sError.Substring(2));
            if (aAgrupadorContenidoDigital.Nombre == null || aAgrupadorContenidoDigital.Nombre.Trim().Length == 0)
                sError += ", Nombre";
            if (aAgrupadorContenidoDigital.EsPredeterminado == null)
                sError += ", Activo";
            if (aAgrupadorContenidoDigital.FechaRegistro == null)
                sError += ", FechaRegistro";
            if (aAgrupadorContenidoDigital.Estatus == null)
                sError += ", Estatus";
            if (sError.Length > 0)
                throw new Exception("TramaInsHlp: Los siguientes campos no pueden ser vacíos " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Profesionalizacion.DAO",
                   "AgrupadorContenidoDigitalInsHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "AgrupadorContenidoDigitalInsHlp: No se pudo conectar a la base de datos", "POV.Profesionalizacion.DAO",
                   "AgrupadorContenidoDigitalInsHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" INSERT INTO AgrupadorContenidoDigital (Nombre,EsPredeterminado,FechaRegistro,EstatusProfesionalizacion,TipoAgrupador,AgrupadorPadreID, Competencias, Aprendizajes) ");
            sCmd.Append(" VALUES ");
            sCmd.Append(" ( ");

            // aAgrupadorContenidoDigital.Nombre
            sCmd.Append(" @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            if (aAgrupadorContenidoDigital.Nombre == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = aAgrupadorContenidoDigital.Nombre;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            // aAgrupadorContenidoDigital.EsPredeterminado
            sCmd.Append(" ,@dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            if (aAgrupadorContenidoDigital.EsPredeterminado == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = aAgrupadorContenidoDigital.EsPredeterminado;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
            // aAgrupadorContenidoDigital.FechaRegistro
            sCmd.Append(" ,@dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            if (aAgrupadorContenidoDigital.FechaRegistro == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = aAgrupadorContenidoDigital.FechaRegistro;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
            // aAgrupadorContenidoDigital.Estatus
            sCmd.Append(" ,@dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            if (aAgrupadorContenidoDigital.Estatus == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = aAgrupadorContenidoDigital.Estatus;
            sqlParam.DbType = DbType.Byte;
            sqlCmd.Parameters.Add(sqlParam);
            // aAgrupadorContenidoDigital.TipoAgrupador
            sCmd.Append(" ,@dbp4ram6 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram6";
            sqlParam.Value = aAgrupadorContenidoDigital.TipoAgrupador;
            sqlParam.DbType = DbType.Byte;
            sqlCmd.Parameters.Add(sqlParam);
            // agrupadorPadreID
            sCmd.Append(" ,@dbp4ram7 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram7";
            if (agrupadorPadreID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = agrupadorPadreID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
            // aAgrupadorContenidoDigital.Competencias
            sCmd.Append(" ,@dbp4ram8 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram8";
            if (aAgrupadorContenidoDigital.Competencias == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = aAgrupadorContenidoDigital.Competencias;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            // aAgrupadorContenidoDigital.Aprendizajes
            sCmd.Append(" ,@dbp4ram9 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram9";
            if (aAgrupadorContenidoDigital.Aprendizajes == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = aAgrupadorContenidoDigital.Aprendizajes;
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
                throw new Exception("AgrupadorContenidoDigitalInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("AgrupadorContenidoDigitalInsHlp: Ocurrió un error al ingresar el registro.");
        }
    }
}
