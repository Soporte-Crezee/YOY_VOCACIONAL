using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using Framework.Base.DataAccess;
using GP.SocialEngine.BO;

namespace GP.SocialEngine.DA
{
    public class ReporteAbusoDARetHlp
    {
        public DataSet Action(IDataContext dctx,int pageSize, int currentPage,string sortColumn, string sortorder,Dictionary<string,string> parametros,List<ETipoContenido> lstipocontenido,List<EEstadoReporteAbuso> lsEstados,List<GrupoSocial> lsgrupos)
        {        
            //validaciones 
            if (pageSize==null || pageSize <= 0)
                pageSize = 10;
            if (currentPage == null || currentPage <= 0)
                currentPage = 1;

            if (string.IsNullOrEmpty(sortColumn))
                sortColumn = "FechaReporte";
            if (string.IsNullOrEmpty(sortorder))
                sortorder = "desc";

            if (parametros == null)
                throw new Exception("Los parámetros no pueden ser nulos");


            int pageIndex = 0;

            //variables
            string swhere = string.Empty;
            string swhereOrGr = string.Empty;
            string swhereOrEs = string.Empty;
            string swhereOrTipos = string.Empty;

            int cont = 1;

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
                throw new Exception(string.Format("ReporteAbusoDARetHlp: No se pudo conectar con la base de datos {0}", ex.Message));
            }
          
            #region ControlData

            queryControl.Append(" SELECT");
            queryControl.Append(" COUNT(ReporteAbuso.ReporteAbusoID) as RecordCount,");
            queryControl.Append(" Ceiling(cast(count(ReporteAbuso.ReporteAbusoID) as float) / cast (@PageSize as float)) as PageCount");
            queryControl.Append(" FROM ReporteAbuso ");

            //parámetros consulta
            dbParameter = dbCommandControl.CreateParameter();
            dbParameter.ParameterName = "@PageSize";
            dbParameter.DbType = DbType.Int32;
            dbParameter.Value = pageSize;
            dbCommandControl.Parameters.Add(dbParameter);

            //parámetros de condición
            if (parametros.ContainsKey("ReporteAbusoID"))
            {
                if (swhere.Length > 0) swhere += " AND ";

                swhere += " ReporteAbuso.ReporteAbusoID =  @ReporteAbusoID";
                dbParameter = dbCommandControl.CreateParameter();
                dbParameter.ParameterName = "@ReporteAbusoID";
                dbParameter.DbType = DbType.Guid;
                dbParameter.Value = Guid.Parse(parametros["ReporteAbusoID"].Trim());
                dbCommandControl.Parameters.Add(dbParameter);
            }
            if (parametros.ContainsKey("ReportadoID"))
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += " ReporteAbuso.ReportadoID = @ReportadoID";
                dbParameter = dbCommandControl.CreateParameter();
                dbParameter.ParameterName = "@ReportadoID";
                dbParameter.DbType = DbType.Int64;
                dbParameter.Value = long.Parse(parametros["ReportadoID"]);
                dbCommandControl.Parameters.Add(dbParameter);
            }
            if (parametros.ContainsKey("ReportanteID"))
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += " ReporteAbuso.ReportanteID = @ReportanteID";
                dbParameter = dbCommandControl.CreateParameter();
                dbParameter.ParameterName = "@ReportanteID";
                dbParameter.DbType = DbType.Int64;
                dbParameter.Value = long.Parse(parametros["ReportanteID"]);
                dbCommandControl.Parameters.Add(dbParameter);
            }
            if (parametros.ContainsKey("FechaReporte"))
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += " ReporteAbuso.FechaReporte = @FechaReporte";
                dbParameter = dbCommandControl.CreateParameter();
                dbParameter.ParameterName = "@FechaReporte";
                dbParameter.DbType = DbType.DateTime;
                dbParameter.Value = DateTime.Parse(parametros["FechaReporte"]);
                dbCommandControl.Parameters.Add(dbParameter);
            }
            if (parametros.ContainsKey("FechaFinReporte"))
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += " ReporteAbuso.FechaFinReporte= @FechaFinReporte";
                dbParameter = dbCommandControl.CreateParameter();
                dbParameter.ParameterName = "@FechaFinReporte";
                dbParameter.DbType = DbType.DateTime;
                dbParameter.Value = DateTime.Parse(parametros["FechaFinReporte"]);
                dbCommandControl.Parameters.Add(dbParameter);

            }
            if (parametros.ContainsKey("ReportableID"))
            {
                if (swhere.Length > 0) swhere += " AND ";

                swhere += " ReporteAbuso.ReportableID =  @ReportableID";
                dbParameter = dbCommandControl.CreateParameter();
                dbParameter.ParameterName = "@ReportableID";
                dbParameter.DbType = DbType.Guid;
                dbParameter.Value = Guid.Parse(parametros["ReportableID"].Trim());
                dbCommandControl.Parameters.Add(dbParameter);

            }

            //lista de grupos
            foreach (GrupoSocial lgrupo in lsgrupos)
            {
                if (swhereOrGr.Length > 0) swhereOrGr += " OR ";

                swhereOrGr += string.Format(" ReporteAbuso.GrupoSocialID = @GrupoSocialID{0}", cont);
                dbParameter = dbCommandControl.CreateParameter();
                dbParameter.ParameterName = string.Format("@GrupoSocialID{0}", cont);
                dbParameter.DbType = DbType.Int64;
                dbParameter.Value = lgrupo.GrupoSocialID;
                dbCommandControl.Parameters.Add(dbParameter);
                cont++;
            }

            //lista de estado
            cont = 1;
            foreach (EEstadoReporteAbuso eestado in lsEstados)
            {
                //agregar parámetros
                if (swhereOrEs.Length > 0) swhereOrEs += " OR ";

                swhereOrEs += string.Format(" ReporteAbuso.EstatusReporte = @EstatusReporte{0}", cont);
                dbParameter = dbCommandControl.CreateParameter();
                dbParameter.ParameterName = string.Format("@EstatusReporte{0}", cont);
                dbParameter.DbType = DbType.Int16;
                dbParameter.Value = (short)eestado;
                dbCommandControl.Parameters.Add(dbParameter);
                cont++;
            }
            cont = 1;
            foreach (ETipoContenido contenido in lstipocontenido)
            {
                //agregar parámetros
                if (swhereOrTipos.Length > 0) swhereOrTipos += " OR ";

                swhereOrTipos += string.Format(" ReporteAbuso.TipoContenido = @TipoContenido{0}", cont);
                dbParameter = dbCommandControl.CreateParameter();
                dbParameter.ParameterName = string.Format("@TipoContenido{0}", cont);
                dbParameter.DbType = DbType.Int16;
                dbParameter.Value = (short) contenido;
                dbCommandControl.Parameters.Add(dbParameter);
                cont++;
            }
            if (swhereOrGr.Length > 0)
            {
                swhereOrGr = string.Format(" ( {0} ) ", swhereOrGr);

                if (swhere.Length > 0) swhere += " AND ";
                swhere += swhereOrGr;
            }

            if (swhereOrEs.Length > 0)
            {
                swhereOrEs = string.Format(" ( {0} ) ", swhereOrEs);
                if (swhere.Length > 0) swhere += " AND ";
                swhere += swhereOrEs;
            }

            if (swhereOrTipos.Length > 0)
            {
                swhereOrTipos = string.Format(" ( {0} ) ", swhereOrTipos);
                if (swhere.Length > 0) swhere += " AND ";
                swhere += swhereOrTipos;
            }

         
            if (swhere.Length > 0) queryControl.AppendFormat(" WHERE {0}", swhere);
            DataSet ds = new DataSet();
            try
            {
                DbDataAdapter adapter = dctx.CreateDataAdapter();
                dbCommandControl.CommandText = queryControl.ToString();
                adapter.SelectCommand = dbCommandControl;
                adapter.Fill(ds, "ControlData");
                pageIndex = currentPage - 1;


            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("ReporteAbusoDARetHlp: Ocurrió un problema al recuperar los datos de control: {0}", ex.Message));
            }

            #endregion

            #region SubQuery

            swhere = string.Empty;
            swhereOrEs = string.Empty;
            swhereOrTipos = string.Empty;
            swhereOrGr = string.Empty;

            query.Append(" SELECT ReporteAbuso.ReporteAbusoID, ReporteAbuso.FechaReporte, ReporteAbuso.FechaFinReporte,");
            query.Append(" ReporteAbuso.ReportableID,ReporteAbuso.EstatusReporte, ReporteAbuso.TipoContenido, ");
            query.Append(" ReporteAbuso.ReportadoID, ReporteAbuso.ReportanteID, ReporteAbuso.GrupoSocialID, ");
            query.Append(" rownumber = ROW_NUMBER() OVER ( ");
            query.Append(string.Format(" ORDER BY {0} {1} ",sortColumn,sortorder));
            query.Append(" ) ");
            query.Append(" FROM ReporteAbuso ");

            //parámetros de condición
            if (parametros.ContainsKey("ReporteAbusoID"))
            {
                if (swhere.Length > 0) swhere += " AND ";

                swhere += " ReporteAbuso.ReporteAbusoID =  @ReporteAbusoID";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@ReporteAbusoID";
                dbParameter.DbType = DbType.Guid;
                dbParameter.Value = Guid.Parse(parametros["ReporteAbusoID"].Trim());
                dbCommand.Parameters.Add(dbParameter);
            }
            if (parametros.ContainsKey("ReportadoID"))
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += " ReporteAbuso.ReportadoID = @ReportadoID";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@ReportadoID";
                dbParameter.DbType = DbType.Int64;
                dbParameter.Value = long.Parse(parametros["ReportadoID"]);
                dbCommand.Parameters.Add(dbParameter);
            }
            if (parametros.ContainsKey("ReportanteID"))
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += " ReporteAbuso.ReportanteID = @ReportanteID";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@ReportanteID";
                dbParameter.DbType = DbType.Int64;
                dbParameter.Value = long.Parse(parametros["ReportanteID"]);
                dbCommand.Parameters.Add(dbParameter);
            }
            if (parametros.ContainsKey("FechaReporte"))
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += " ReporteAbuso.FechaReporte = @FechaReporte";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@FechaReporte";
                dbParameter.DbType = DbType.DateTime;
                dbParameter.Value = DateTime.Parse(parametros["FechaReporte"]);
                dbCommand.Parameters.Add(dbParameter);
            }
            if (parametros.ContainsKey("FechaFinReporte"))
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += " ReporteAbuso.FechaFinReporte= @FechaFinReporte";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@FechaFinReporte";
                dbParameter.DbType = DbType.DateTime;
                dbParameter.Value = DateTime.Parse(parametros["FechaFinReporte"]);
                dbCommand.Parameters.Add(dbParameter);

            }
            if (parametros.ContainsKey("ReportableID"))
            {
                if (swhere.Length > 0) swhere += " AND ";

                swhere += " ReporteAbuso.ReportableID =  @ReportableID";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@ReportableID";
                dbParameter.DbType = DbType.Guid;
                dbParameter.Value = Guid.Parse(parametros["ReportableID"].Trim());
                dbCommand.Parameters.Add(dbParameter);

            }
            if (parametros.ContainsKey("TipoContenido"))
            {
                if (swhere.Length > 0) swhere += " AND ";

                swhere += " ReporteAbuso.TipoContenido =  @TipoContenido";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@TipoContenido";
                dbParameter.DbType = DbType.Int16;
                dbParameter.Value = short.Parse(parametros["TipoContenido"].Trim());
                dbCommand.Parameters.Add(dbParameter);
            }

            //lista de grupos
            cont = 1;
            foreach (GrupoSocial lgrupo in lsgrupos)
            {
                if (swhereOrGr.Length > 0) swhereOrGr += " OR ";

                swhereOrGr += string.Format(" ReporteAbuso.GrupoSocialID = @GrupoSocialID{0}", cont);
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = string.Format("@GrupoSocialID{0}",cont);
                dbParameter.DbType = DbType.Int64;
                dbParameter.Value = lgrupo.GrupoSocialID;
                dbCommand.Parameters.Add(dbParameter);
                cont++;
            }

            //lista de estado
            cont = 1;
            foreach (EEstadoReporteAbuso eestado in lsEstados)
            {
                //agregar parámetros
                if (swhereOrEs.Length > 0) swhereOrEs += " OR ";

                swhereOrEs += string.Format(" ReporteAbuso.EstatusReporte = @EstatusReporte{0}", cont);
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = string.Format("@EstatusReporte{0}", cont);
                dbParameter.DbType = DbType.Int16;
                dbParameter.Value = (short) eestado;
                dbCommand.Parameters.Add(dbParameter);
                cont++;
            }

            cont = 1;
            foreach (ETipoContenido contenido in lstipocontenido)
            {
                //agregar parámetros
                if (swhereOrTipos.Length > 0) swhereOrTipos += " OR ";

                swhereOrTipos += string.Format(" ReporteAbuso.TipoContenido = @TipoContenido{0}", cont);
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = string.Format("@TipoContenido{0}", cont);
                dbParameter.DbType = DbType.Int16;
                dbParameter.Value = (short)contenido;
                dbCommand.Parameters.Add(dbParameter);
                cont++;
            }

            if (swhereOrGr.Length > 0)
            {
                swhereOrGr = string.Format(" ( {0} ) ", swhereOrGr);

                if (swhere.Length > 0) swhere += " AND ";
                swhere += swhereOrGr;
            }


            if (swhereOrEs.Length > 0)
            {
                swhereOrEs = string.Format(" ( {0} ) ", swhereOrEs);
                if (swhere.Length > 0) swhere += " AND ";
                swhere += swhereOrEs;
            }

            if (swhereOrTipos.Length > 0)
            {
                swhereOrTipos = string.Format(" ( {0} )", swhereOrTipos);
                if (swhere.Length > 0) swhere += " AND ";
                swhere += swhereOrTipos;
            }

            if (swhere.Length > 0)
                query.AppendFormat(" WHERE {0} ",swhere);

            #endregion

            #region Query

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


            queryCols.Append("SELECT ReporteAbusoID,FechaReporte,FechaFinReporte,ReportableID,EstatusReporte,TipoContenido,ReportadoID,ReportanteID,GrupoSocialID,");
            queryCols.Append(" rownumber");
            queryCols.Append(" FROM ( ");
            queryCols.Append(query.ToString());
            queryCols.Append(" ) as ReportesAbuso");
            queryCols.Append(" WHERE rownumber BETWEEN cast(@PageSize * @PageIndex + 1 as varchar(10))");
            queryCols.Append(" AND cast(@PageSize * (@PageIndex + 1) as varchar(10))");

            try
            {
                DbDataAdapter da = dctx.CreateDataAdapter();
                dbCommand.CommandText = queryCols.ToString();
                da.SelectCommand = dbCommand;
                da.Fill(ds, "ReporteAbuso");
            }
            catch (Exception ex)
            {
                
               throw new Exception(string.Format("ReporteAbusoDaRetHlp:Ocurrió un error al recuperar los reportes de abuso {0}",ex.Message));
            }
            #endregion
            return ds;
        }
    }
}
