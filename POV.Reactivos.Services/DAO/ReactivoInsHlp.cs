using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Reactivos.BO;
using POV.Modelo.Estandarizado.BO;

namespace POV.Reactivos.DAO
{
    /// <summary>
    /// Guarda un registro de Reactivo en la BD
    /// </summary>
    internal class ReactivoInsHlp
    {
        /// <summary>
        /// Crea un registro de Reactivo en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="reactivo">Reactivo que desea crear</param>
        public void Action(IDataContext dctx, Reactivo reactivo)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (reactivo == null)
                sError += ", Reactivo";
            if (sError.Length > 0)
                throw new Exception("ReactivoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (reactivo.ReactivoID == null)
                sError += ", ReactivoID";
            if (reactivo.TipoReactivo == null)
                sError += ", Tipo Reactivo";
            if (reactivo.Caracteristicas == null)
                sError += ", Caracteristicas";
            if (sError.Length > 0)
                throw new Exception("ReactivoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (reactivo.NombreReactivo == null || reactivo.NombreReactivo.Trim().Length == 0)
                sError += ", NombreReactivo";
            if (reactivo.PlantillaReactivo == null)
                sError += ", PlantillaReactivo";
            if (sError.Length > 0)
                throw new Exception("ReactivoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (reactivo is ReactivoDocente)
            {
                if ((reactivo as ReactivoDocente).Docente == null || (reactivo as ReactivoDocente).Docente.DocenteID == null)
                {
                    sError += ", Docente";
                }

                if (sError.Length > 0)
                    throw new Exception("ReactivoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            }
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Reactivos.DAO",
                   "ReactivoInsHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "ReactivoInsHlp: No se pudo conectar a la base de datos", "POV.Reactivos.DAO",
                   "ReactivoInsHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" INSERT INTO Reactivo (ReactivoID, NombreReactivo, Valor, Vigencia, TipoComplejidadID, AreaAplicacionID, NumeroIntentos, PlantillaReactivo, Descripcion, Retroalimentacion, Activo,PresentacionPlantilla, DocenteID) ");
            sCmd.Append(" VALUES ");
            sCmd.Append(" ( ");
            sCmd.Append(" @reactivo_ReactivoID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "reactivo_ReactivoID";
            if (reactivo.ReactivoID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = reactivo.ReactivoID;
            sqlParam.DbType = DbType.Guid;
            sqlCmd.Parameters.Add(sqlParam);
            sCmd.Append(" ,@reactivo_NombreReactivo ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "reactivo_NombreReactivo";
            if (reactivo.NombreReactivo == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = reactivo.NombreReactivo;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);

            sCmd.Append(" ,@reactivo_Valor ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "reactivo_Valor";
            if (reactivo.Valor == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = reactivo.Valor;
            sqlParam.DbType = DbType.Decimal;
            sqlCmd.Parameters.Add(sqlParam);

            sCmd.Append(" ,@reactivo_Vigencia ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "reactivo_Vigencia";
            if (reactivo.FechaRegistro == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = reactivo.FechaRegistro;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);

            sCmd.Append(" ,@reactivo_NumeroIntentos ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "reactivo_NumeroIntentos";
            if (reactivo.NumeroIntentos == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = reactivo.NumeroIntentos;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            sCmd.Append(" ,@reactivo_PlantillaReactivo ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "reactivo_PlantillaReactivo";
            if (reactivo.PlantillaReactivo == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = reactivo.PlantillaReactivo;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);

            sCmd.Append(" ,@reactivo_Descripcion ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "reactivo_Descripcion";
            if (reactivo.Descripcion == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = reactivo.Descripcion;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);

            sCmd.Append(" ,@reactivo_Activo ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "reactivo_Activo";
            if (reactivo.Activo == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = reactivo.Activo;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);

            sCmd.Append(" ,@reactivo_Presentacion ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "reactivo_Presentacion";
            if (reactivo.PresentacionPlantilla == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = reactivo.PresentacionPlantilla;
            sqlParam.DbType = DbType.Byte;
            sqlCmd.Parameters.Add(sqlParam);

            sCmd.Append(" ,@reactivo_DocenteID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "reactivo_DocenteID";
            if (reactivo is ReactivoDocente)
                sqlParam.Value = (reactivo as ReactivoDocente).Docente.DocenteID;
            else
                sqlParam.Value = DBNull.Value;
            sqlParam.DbType = DbType.Int32;
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
                throw new Exception("ReactivoInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("ReactivoInsHlp: Ocurrió un error al ingresar el registro.");
        }
    }
}
