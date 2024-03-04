using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Framework.Base.DataAccess;
using System.Data.Common;
using GP.SocialEngine.BO;

namespace GP.SocialEngine.DA
{
    public class NotificacionDARetHlp
    {
        public DataSet Action(IDataContext dctx, 
            int pageSize, int currentPage, 
            string sortColumn, string sortorder, 
            Dictionary<string, string> parametros, 
            List<ETipoNotificacion> tipos, 
            List<EEstatusNotificacion> estados)
        {

            if (pageSize <= 0)
                throw new Exception("El tamaño de página debe ser mayor a cero");
            if (currentPage < 0)
                throw new Exception("La página actual debe ser un número mayor que cero");
            if (parametros == null)
                throw new Exception("Los parámetros no pueden ser nulos");
            if (parametros.Count == 0)
                throw new Exception("El numéro de parámetros debe ser mayor a cero");

            int recordCount = 0;
            int pageCount = 0;
            int pageIndex = 0;

            //variables
            string swhere = string.Empty;
            string sError = string.Empty;
            StringBuilder query = new StringBuilder();
            StringBuilder queryControl = new StringBuilder();
            StringBuilder queryCols = new StringBuilder();
            DbParameter dbParameter = null;
            DbCommand dbCommand = null;
            DbCommand dbCommandControl = null;
            object myFirm = new object();

            try
            {
                dctx.OpenConnection(myFirm);
                dbCommand = dctx.CreateCommand();
                dbCommandControl = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("NotificacionDARetHlp: No se pudo conectar con la base de datos {0}", ex.Message));
            }

            DataSet ds = new DataSet();
            pageIndex = currentPage - 1;

            swhere = string.Empty;
            query.Append(" SELECT");
            query.Append(" noti.NotificacionID, noti.FechaRegistro, noti.EmisorID, noti.ReceptorID, noti.NotificableID, noti.TipoNotificacion, noti.EstatusNotificacion, rownumber= ROW_NUMBER()");
            query.Append(" OVER( ORDER BY ");
            query.Append(string.Format(" {0}", sortColumn));
            query.Append(string.Format(" {0}", sortorder));
            query.Append(" ) ");
            query.Append(" FROM ");
            query.Append(" Notificacion noti ");

            //Condiciones
            if (parametros.ContainsKey("EmisorID"))
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += " EmisorID =  @EmisorID";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@EmisorID";
                dbParameter.DbType = DbType.Int64;
                dbParameter.Value = parametros["EmisorID"].Trim();
                dbCommand.Parameters.Add(dbParameter);
            }
            if (parametros.ContainsKey("ReceptorID"))
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += " ReceptorID =  @ReceptorID";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@ReceptorID";
                dbParameter.DbType = DbType.Int64;
                dbParameter.Value = parametros["ReceptorID"].Trim();
                dbCommand.Parameters.Add(dbParameter);
            }

            if (tipos != null && tipos.Count > 0)
            {
                if (swhere.Length > 0) swhere += " AND ( ";
                else swhere += " ( ";
                int i = 0;
                bool isFirst = true;
                foreach (ETipoNotificacion item in tipos)
                {
                    if (isFirst)
                    {
                        isFirst = false;

                        swhere += " TipoNotificacion = @TipoNotificacion ";
                        dbParameter = dbCommand.CreateParameter();
                        dbParameter.ParameterName = "@TipoNotificacion";
                        dbParameter.DbType = DbType.Int16;
                        dbParameter.Value = item;
                        dbCommand.Parameters.Add(dbParameter);
                    }
                    else
                    {
                        i++;

                        if (swhere.Length > 0) swhere += " OR ";
                        swhere += " TipoNotificacion = @TipoNotificacion" + i + " ";
                        dbParameter = dbCommand.CreateParameter();
                        dbParameter.ParameterName = "@TipoNotificacion" + i;
                        dbParameter.DbType = DbType.Int16;
                        dbParameter.Value = item;
                        dbCommand.Parameters.Add(dbParameter);
                    }
                }
                if (swhere.Length > 0) swhere += " ) ";
            }

            if (estados != null && estados.Count > 0)
            {
                if (swhere.Length > 0) swhere += " AND ( ";
                else swhere += " ( ";
                int i = 0;
                bool isFirst = true;
                foreach (EEstatusNotificacion item in estados)
                {
                    if (isFirst)
                    {
                        isFirst = false;

                        swhere += " EstatusNotificacion = @EstatusNotificacion ";
                        dbParameter = dbCommand.CreateParameter();
                        dbParameter.ParameterName = "@EstatusNotificacion";
                        dbParameter.DbType = DbType.Int16;
                        dbParameter.Value = item;
                        dbCommand.Parameters.Add(dbParameter);
                    }
                    else
                    {
                        i++;

                        if (swhere.Length > 0) swhere += " OR ";
                        swhere += " EstatusNotificacion = @EstatusNotificacion" + i + " ";
                        dbParameter = dbCommand.CreateParameter();
                        dbParameter.ParameterName = "@EstatusNotificacion" + i;
                        dbParameter.DbType = DbType.Int16;
                        dbParameter.Value = item;
                        dbCommand.Parameters.Add(dbParameter);
                    }
                }
                if (swhere.Length > 0) swhere += " ) ";
            }            

            if (pageSize > 0)
            {
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@PageSize";
                dbParameter.DbType = DbType.Int32;
                dbParameter.Value = pageSize;
                dbCommand.Parameters.Add(dbParameter);
            }

            if (pageIndex > -2)
            {
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@PageIndex";
                dbParameter.DbType = DbType.Int32;
                dbParameter.Value = pageIndex;
                dbCommand.Parameters.Add(dbParameter);
            }


            if (swhere.Length > 0) query.AppendFormat(" WHERE {0}", swhere);

            queryCols.Append("SELECT NotificacionID, FechaRegistro, EmisorID, ReceptorID, NotificableID, TipoNotificacion, EstatusNotificacion");
            queryCols.Append(" FROM ( ");
            queryCols.Append(query.ToString());
            queryCols.Append(" ) as Notificaciones ");
            queryCols.Append(" WHERE rownumber BETWEEN cast(@PageSize * @PageIndex + 1 as varchar(10))");
            queryCols.Append(" AND cast(@PageSize * (@PageIndex + 1) as varchar(10))");

            try
            {
                DbDataAdapter adapter = dctx.CreateDataAdapter();
                dbCommand.CommandText = queryCols.ToString();
                adapter.SelectCommand = dbCommand;
                adapter.Fill(ds, "Notificacion");
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("NotificacionDARetHlp: Ocurrió un problema al recuperar las notificaciones: {0}", ex.Message));
            }
            return ds;
        }
    }
}
