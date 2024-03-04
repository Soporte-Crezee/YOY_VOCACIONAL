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
    /// Actualiza un registro de Reactivo en la BD
    /// </summary>
    internal class ReactivoDinamicoUpdHlp
    {
        /// <summary>
        /// Actualiza de manera optimista un registro de ReactivoDinamicoUpdHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="reactivoDinamicoUpdHlp">ReactivoDinamicoUpdHlp que tiene los datos nuevos</param>
        /// <param name="anterior">ReactivoDinamicoUpdHlp que tiene los datos anteriores</param>
        public void Action(IDataContext dctx, Reactivo reactivo, Reactivo anterior)
        {
            object myFirm = new object();
            String sError = string.Empty;
            if (reactivo == null)
                sError += ", Reactivo";
            if (anterior == null)
                sError += ", Anterior";
            if (sError.Length > 0)
                throw new Exception("ReactivoDinamicoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (anterior.ReactivoID == null)
                sError += ", Anterior ReactivoID";
            if (sError.Length > 0)
                throw new Exception("ReactivoDinamicoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Reactivos.DAO",
                   "ReactivoDinamicoUpdHlp", "Action", null, null);

            if (reactivo.Caracteristicas == null)
            {
                reactivo.Caracteristicas = new CaracteristicasModeloGenerico();
                (reactivo.Caracteristicas as CaracteristicasModeloGenerico).Modelo = new ModeloDinamico();
                (reactivo.Caracteristicas as CaracteristicasModeloGenerico).Clasificador = new Clasificador();
            }

            if (reactivo.Caracteristicas is CaracteristicasModeloGenerico)
            {
                if ((reactivo.Caracteristicas as CaracteristicasModeloGenerico).Modelo == null)
                    (reactivo.Caracteristicas as CaracteristicasModeloGenerico).Modelo = new ModeloDinamico();
                if ((reactivo.Caracteristicas as CaracteristicasModeloGenerico).Clasificador == null)
                    (reactivo.Caracteristicas as CaracteristicasModeloGenerico).Clasificador = new Clasificador();
            }
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "ReactivoDinamicoUpdHlpUpdHl: Hubo un error al conectarse a la base de datos", "POV.Reactivos.DAO",
                   "ReactivoDinamicoUpdHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" UPDATE ReactivoDinamico ");
            StringBuilder s_VarSET = new StringBuilder();
            if (reactivo.NombreReactivo != null)
            {
                s_VarSET.Append(" NombreReactivo = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = reactivo.NombreReactivo;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (reactivo.Asignado != null)
            {
                s_VarSET.Append(" ,Asignado = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = reactivo.Asignado;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (reactivo.Clave != null)
            {
                s_VarSET.Append(" ,Clave = @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = reactivo.Clave;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (reactivo.PlantillaReactivo != null)
            {
                s_VarSET.Append(" ,PlantillaReactivo = @dbp4ram4 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = reactivo.PlantillaReactivo;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (reactivo.Descripcion != null)
            {
                s_VarSET.Append(" ,Descripcion = @dbp4ram5 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram5";
                sqlParam.Value = reactivo.Descripcion;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (reactivo.PresentacionPlantilla != null)
            {
                s_VarSET.Append(" ,PresentacionPlantilla = @dbp4ram6 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram6";
                sqlParam.Value = reactivo.PresentacionPlantilla;
                sqlParam.DbType = DbType.Byte;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if ((reactivo.Caracteristicas as CaracteristicasModeloGenerico).Modelo.ModeloID != null)
            {
                s_VarSET.Append(" ,ModeloID = @dbp4ram7 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram7";
                sqlParam.Value = (reactivo.Caracteristicas as CaracteristicasModeloGenerico).Modelo.ModeloID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if ((reactivo.Caracteristicas as CaracteristicasModeloGenerico).Clasificador.ClasificadorID != null)
            {
                s_VarSET.Append(" ,ClasificadorID = @dbp4ram8 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram8";
                sqlParam.Value = (reactivo.Caracteristicas as CaracteristicasModeloGenerico).Clasificador.ClasificadorID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (reactivo.Valor != null)
            {
                s_VarSET.Append(" ,Valor = @dbp4ram9 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram9";
                sqlParam.Value = reactivo.Valor;
                sqlParam.DbType = DbType.Decimal;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (reactivo.Grupo != null)
            {
                s_VarSET.Append(" ,Grupo = @dbp4ram18 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram18";
                sqlParam.Value = reactivo.Grupo;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            string s_VarSETres = s_VarSET.ToString().Trim();
            if (s_VarSETres.Length > 0)
            {
                if (s_VarSETres.StartsWith("AND "))
                    s_VarSETres = s_VarSETres.Substring(4);
                else if (s_VarSETres.StartsWith("OR "))
                    s_VarSETres = s_VarSETres.Substring(3);
                else if (s_VarSETres.StartsWith(","))
                    s_VarSETres = s_VarSETres.Substring(1);
                sCmd.Append(" SET " + s_VarSETres);
            }
            StringBuilder s_VarWHERE = new StringBuilder();
            if (anterior.ReactivoID != null)
            {
                s_VarWHERE.Append(" ReactivoID = @dbp4ram10 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram10";
                sqlParam.Value = anterior.ReactivoID;
                sqlParam.DbType = DbType.Guid;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.NombreReactivo != null)
            {
                s_VarWHERE.Append(" AND NombreReactivo = @dbp4ram11 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram11";
                sqlParam.Value = anterior.NombreReactivo;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.Activo != null)
            {
                s_VarWHERE.Append(" AND Activo = @dbp4ram12 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram12";
                sqlParam.Value = anterior.Activo;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.Asignado != null)
            {
                s_VarWHERE.Append(" AND Asignado = @dbp4ram13 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram13";
                sqlParam.Value = anterior.Asignado;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.Clave != null)
            {
                s_VarWHERE.Append(" AND Clave = @dbp4ram14 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram14";
                sqlParam.Value = anterior.Clave;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.PresentacionPlantilla != null)
            {
                s_VarWHERE.Append(" AND PresentacionPlantilla = @dbp4ram15 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram15";
                sqlParam.Value = anterior.PresentacionPlantilla;
                sqlParam.DbType = DbType.Byte;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if ((anterior.Caracteristicas as CaracteristicasModeloGenerico).Modelo != null && (anterior.Caracteristicas as CaracteristicasModeloGenerico).Modelo.ModeloID != null)
            {
                s_VarWHERE.Append(" AND ModeloID = @dbp4ram16 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram16";
                sqlParam.Value = (anterior.Caracteristicas as CaracteristicasModeloGenerico).Modelo.ModeloID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if ((anterior.Caracteristicas as CaracteristicasModeloGenerico).Clasificador != null && (anterior.Caracteristicas as CaracteristicasModeloGenerico).Clasificador.ClasificadorID != null)
            {
                s_VarWHERE.Append(" AND ClasificadorID = @dbp4ram17 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram17";
                sqlParam.Value = (anterior.Caracteristicas as CaracteristicasModeloGenerico).Clasificador.ClasificadorID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            string s_VarWHEREres = s_VarWHERE.ToString().Trim();
            if (s_VarWHEREres.Length > 0)
            {
                if (s_VarWHEREres.StartsWith("AND "))
                    s_VarWHEREres = s_VarWHEREres.Substring(4);
                else if (s_VarWHEREres.StartsWith("OR "))
                    s_VarWHEREres = s_VarWHEREres.Substring(3);
                else if (s_VarWHEREres.StartsWith(","))
                    s_VarWHEREres = s_VarWHEREres.Substring(1);
                sCmd.Append(" WHERE " + s_VarWHEREres);
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
                throw new Exception("ReactivoDinamicoUpdHlp: Hubo  un Error al Actualizar el Registro . " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("ReactivoDinamicoUpdHlp: Hubo  un Error al Actualizar el Registro .");
        }
    }
}
