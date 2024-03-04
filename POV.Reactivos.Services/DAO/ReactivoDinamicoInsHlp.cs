using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Reactivos.BO;
using POV.Modelo.BO;

namespace POV.Reactivos.DAO
{
    /// <summary>
    /// Guarda un registro de Reactivo en la BD
    /// </summary>
    internal class ReactivoDinamicoInsHlp
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
                throw new Exception("ReactivoDinamicoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (reactivo.NombreReactivo == null || reactivo.NombreReactivo.Trim().Length == 0)
                sError += ", NombreReactivo";
            if (reactivo.Activo == null)
                sError += ", Activo";
            if (reactivo.Asignado == null)
                sError += ", Asignado";
            if (reactivo.Clave == null || reactivo.Clave.Trim().Length == 0)
                sError += ", Clave";
            if (reactivo.TipoReactivo == null)
                sError += ", TipoReactivo";
            if (reactivo.PresentacionPlantilla == null)
                sError += ", PresentacionPlantilla";
            if (reactivo.FechaRegistro == null)
                sError += ", FechaRegistro";
            if (sError.Length > 0)
                throw new Exception("ReactivoDinamicoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (reactivo.Caracteristicas == null)
                sError += ", Caracteristicas";
            if (sError.Length > 0)
                throw new Exception("ReactivoDinamicoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (!(reactivo.Caracteristicas is CaracteristicasModeloGenerico))
                sError += ", CaracteristicasModeloGenerico";
            if (sError.Length > 0)
                throw new Exception("ReactivoDinamicoInsHlp: Los siguientes campos son requeridos " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Reactivos.DAO",
                   "ReactivoDinamicoInsHlp", "Action", null, null);

            if ((reactivo.Caracteristicas as CaracteristicasModeloGenerico).Modelo == null)
                (reactivo.Caracteristicas as CaracteristicasModeloGenerico).Modelo = new ModeloDinamico();

            if ((reactivo.Caracteristicas as CaracteristicasModeloGenerico).Clasificador == null)
                (reactivo.Caracteristicas as CaracteristicasModeloGenerico).Clasificador = new Clasificador();

            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "ReactivoDinamicoInsHlp: No se pudo conectar a la base de datos", "POV.Reactivos.DAO",
                   "ReactivoDinamicoInsHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" INSERT INTO ReactivoDinamico (ReactivoID, NombreReactivo, PlantillaReactivo, Descripcion, Activo, Asignado, Clave, TipoReactivo, PresentacionPlantilla, ModeloID, ClasificadorID, FechaRegistro, Grupo) ");
            sCmd.Append(" VALUES ");
            sCmd.Append(" ( ");
            // reactivo.ReactivoID
            sCmd.Append(" @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            if (reactivo.ReactivoID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = reactivo.ReactivoID;
            sqlParam.DbType = DbType.Guid;
            sqlCmd.Parameters.Add(sqlParam);
            // reactivo.NombreReactivo
            sCmd.Append(" ,@dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            if (reactivo.NombreReactivo == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = reactivo.NombreReactivo;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            // reactivo.PlantillaReactivo
            sCmd.Append(" ,@dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            if (reactivo.PlantillaReactivo == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = reactivo.PlantillaReactivo;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            // reactivo.Descripcion
            sCmd.Append(" ,@dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            if (reactivo.Descripcion == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = reactivo.Descripcion;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            // reactivo.Activo
            sCmd.Append(" ,@dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            if (reactivo.Activo == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = reactivo.Activo;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
            // reactivo.Asignado
            sCmd.Append(" ,@dbp4ram6 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram6";
            if (reactivo.Asignado == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = reactivo.Asignado;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
            // reactivo.Clave
            sCmd.Append(" ,@dbp4ram7 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram7";
            if (reactivo.Clave == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = reactivo.Clave;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            // reactivo.TipoReactivo
            sCmd.Append(" ,@dbp4ram8 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram8";
            if (reactivo.TipoReactivo == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = reactivo.TipoReactivo;
            sqlParam.DbType = DbType.Byte;
            sqlCmd.Parameters.Add(sqlParam);
            // reactivo.PresentacionPlantilla
            sCmd.Append(" ,@dbp4ram9 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram9";
            if (reactivo.PresentacionPlantilla == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = reactivo.PresentacionPlantilla;
            sqlParam.DbType = DbType.Byte;
            sqlCmd.Parameters.Add(sqlParam);
            // (reactivo.Caracteristicas as CarasteristicasModeloGenerico).Modelo.ModeloID
            sCmd.Append(" ,@dbp4ram10 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram10";
            if ((reactivo.Caracteristicas as CaracteristicasModeloGenerico).Modelo.ModeloID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = (reactivo.Caracteristicas as CaracteristicasModeloGenerico).Modelo.ModeloID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            // (reactivo.Caracteristicas as CarasteristicasModeloGenerico).Clasificador.ClasificadorID
            sCmd.Append(" ,@dbp4ram11 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram11";
            if ((reactivo.Caracteristicas as CaracteristicasModeloGenerico).Clasificador.ClasificadorID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = (reactivo.Caracteristicas as CaracteristicasModeloGenerico).Clasificador.ClasificadorID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            // reactivo.FechaRegistro
            sCmd.Append(" ,@dbp4ram12 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram12";
            if (reactivo.FechaRegistro == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = reactivo.FechaRegistro;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
            // reactivo.Grupo
            sCmd.Append(" ,@dbp4ram13 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram13";
            if (reactivo.Grupo == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = reactivo.Grupo;
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
                throw new Exception("ReactivoDinamicoInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("ReactivoDinamicoInsHlp: Ocurrió un error al ingresar el registro.");
        }
    }
}
