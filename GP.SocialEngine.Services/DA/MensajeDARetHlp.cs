using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using GP.SocialEngine.BO;
using GP.SocialEngine.DAO;

namespace GP.SocialEngine.DA 
{ 
   /// <summary>
   /// Consulta Mensajes
   /// </summary>
   public class MensajeDARetHlp { 
      /// <summary>
      /// Consulta registros de Mensaje en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="esPadre">Indica si se consultaran los mensajes padres o los hijos o nulo para ambos</param>
      /// <param name="parametros">Filtros</param>
      /// <returns>El DataSet que contiene la información de Mensaje generada por la consulta</returns>
       public DataSet Action(IDataContext dctx, bool? esPadre, long? usuarioSocialId, Dictionary<string, string> parametros)
       {
           if (usuarioSocialId == null || usuarioSocialId <= 0)
               throw new Exception("Debe proporcionar el usuario social que consultara los mensajes");

           if (parametros == null)
               parametros = new Dictionary<string, string>();

           const string sort = "FechaMensaje";
           const string order = "desc";

           //variables
           string swhere = string.Empty;
           StringBuilder query = new StringBuilder();
           StringBuilder queryCols = new StringBuilder();
           DbParameter dbParameter = null;
           DbCommand dbCommand = null;
           DbCommand dbCommandControl = null;
           object myFirm = new object();
           DataSet ds = new DataSet();
          
           try
           {
               dctx.OpenConnection(myFirm);
               dbCommand = dctx.CreateCommand();
               dbCommandControl = dctx.CreateCommand();
           }
           catch (Exception ex)
           {
               throw new Exception(string.Format("MensajeDARetHlp: No se pudo conectar con la base de datos {0}", ex.Message));
           }

           #region SubQuery

           StringBuilder subquery = new StringBuilder();
           swhere = string.Empty;

           if (swhere.Length > 0) swhere += " AND ";
           swhere += " MensajeUsuariosSociales.UsuarioSocialID =  @UsuarioSocialID";
           dbParameter = dbCommand.CreateParameter();
           dbParameter.ParameterName = "@UsuarioSocialID";
           dbParameter.DbType = DbType.Int64;
           dbParameter.Value = Convert.ToInt64(usuarioSocialId);
           dbCommand.Parameters.Add(dbParameter);

           //Solo consultara los usuarios activos
           if (swhere.Length > 0) swhere += " AND ";
           swhere += " MensajeUsuariosSociales.Activo = 1";

           subquery.Append(" (SELECT MensajeID");
           subquery.Append(" FROM MensajeUsuariosSociales ");
           if (swhere.Length > 0) subquery.Append(" WHERE " + swhere);
           subquery.Append(")");
           #endregion

           swhere = string.Empty;
           query.Append(" SELECT");
           query.Append(" Mensaje.MensajeID, Mensaje.Asunto,Mensaje.Contenido, Mensaje.FechaMensaje,Mensaje.Estatus, Mensaje.GuidConversacion,  ");
           query.Append(" Mensaje.RemitenteID,");
           query.Append(" UsuarioSocial.LoginName as RemitenteLoginName, UsuarioSocial.ScreenName as RemitenteScreenName, ");
           query.Append(" rownumber = ROW_NUMBER() ");
           query.Append(" OVER (ORDER BY ");
           query.Append(string.Format(" {0} ", sort));
           query.Append(string.Format(" {0} ", order));
           query.Append(" ) ");
           query.Append(" FROM ");
           query.Append(" Mensaje INNER JOIN UsuarioSocial ON Mensaje.RemitenteID = UsuarioSocial.UsuarioSocialID");
           query.Append(" WHERE Mensaje.GuidConversacion IN ( ");
           query.Append(subquery.ToString());
           query.Append(")");

           //Indica si se consultaran los mensajes padres o los hijos
           if (esPadre != null)
           {
               if (swhere.Length > 0) swhere += " AND ";
               swhere += (bool)esPadre
                             ? " (Mensaje.MensajeID = Mensaje.GuidConversacion) "
                             : " (Mensaje.MensajeID <> Mensaje.GuidConversacion) ";
           }

           if (parametros.ContainsKey("MensajeID"))
           {
               if (swhere.Length > 0) swhere += " AND ";
               swhere += " Mensaje.MensajeID =  @MensajeID";
               dbParameter = dbCommand.CreateParameter();
               dbParameter.ParameterName = "@MensajeID";
               dbParameter.DbType = DbType.String;
               dbParameter.Value = Convert.ToString(parametros["MensajeID"].Trim());
               dbCommand.Parameters.Add(dbParameter);
           }

           if (parametros.ContainsKey("RemitenteID"))
           {
               if (swhere.Length > 0) swhere += " AND ";
               swhere += " Mensaje.RemitenteID =  @RemitenteID";
               dbParameter = dbCommand.CreateParameter();
               dbParameter.ParameterName = "@RemitenteID";
               dbParameter.DbType = DbType.Int64;
               dbParameter.Value = Convert.ToInt64(parametros["RemitenteID"].Trim());
               dbCommand.Parameters.Add(dbParameter);
           }
           if (parametros.ContainsKey("Estatus"))
           {
               if (swhere.Length > 0) swhere += " AND ";
               swhere += " Mensaje.Estatus = @Estatus";
               dbParameter = dbCommand.CreateParameter();
               dbParameter.ParameterName = "@Estatus";
               dbParameter.DbType = DbType.Boolean;
               dbParameter.Value = Convert.ToBoolean(int.Parse(parametros["Estatus"].Trim()));
               dbCommand.Parameters.Add(dbParameter);
           }

           if (swhere.Length > 0) query.Append(" AND " + swhere);

           queryCols.Append("SELECT MensajeID,Asunto,Contenido,FechaMensaje,Estatus,GuidConversacion,RemitenteID,");
           queryCols.Append(" RemitenteLoginName,RemitenteScreenName,");
           queryCols.Append(" rownumber");
           queryCols.Append(" FROM ( ");
           queryCols.Append(query.ToString());
           queryCols.Append(" ) as Mensajes"); 

           try
           {
               DbDataAdapter adapter = dctx.CreateDataAdapter();
               dbCommand.CommandText = queryCols.ToString();
               adapter.SelectCommand = dbCommand;
               adapter.Fill(ds, "Mensaje");
           }
           catch (Exception ex)
           {
               throw new Exception(string.Format("MensajeDARetHlp: Ocurrió un problema al recuperar las publicaciones: {0}", ex.Message));
           }
           return ds;
       }

      /// <summary>
      /// Consulta registros de Mensaje en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="esPadre">Indica si se consultaran los mensajes padres o los hijos o nulo para ambos</param>
      /// <param name="usuarioSocialId">Usuario que participa en los mensajes</param>
      /// <param name="pageSize">Tamaño de la pagina</param>
      /// <param name="currentPage">Pagina Actual</param>
      /// <param name="sortColumn">Tamaño de la pagina</param>
      /// <param name="sortOrder">Ordenamiento</param>
      /// <param name="parametros">Filtros</param>
      /// <returns>El DataSet que contiene la información de Mensaje generada por la consulta</returns>
      public DataSet Action(IDataContext dctx,bool? esPadre,long? usuarioSocialId,int pageSize, int currentPage, string sortColumn, string sortOrder, Dictionary<string, string> parametros)
      {
          if (usuarioSocialId == null || usuarioSocialId<=0)
              throw new Exception("Debe proporcionar el usuario social que consultara los mensajes");
          if (parametros == null)
                parametros = new Dictionary<string, string>();
          if (pageSize <= 0)
              throw new Exception("El tamaño de página debe ser mayor a cero");
          if (currentPage < 0)
              throw new Exception("La página actual debe ser un número mayor que cero");
          if (parametros == null)
              throw new Exception("Los parámetros no pueden ser nulos");

          int pageIndex = 0;

          //variables
          string swhere = string.Empty;
          StringBuilder query = new StringBuilder();
          StringBuilder queryControl = new StringBuilder();
          StringBuilder queryCols = new StringBuilder();
          DbParameter dbParameter = null;
          DbCommand dbCommand = null;
          DbCommand dbCommandControl = null;
          object myFirm = new object();
          DataSet ds = new DataSet();

          try
          {
              dctx.OpenConnection(myFirm);
              dbCommand = dctx.CreateCommand();
              dbCommandControl = dctx.CreateCommand();
          }
          catch (Exception ex)
          {
              throw new Exception(string.Format("MensajeDARetHlp: No se pudo conectar con la base de datos {0}", ex.Message));
          }         

          #region SubQuery
          pageIndex = currentPage - 1;

          StringBuilder subquery = new StringBuilder();
          swhere = string.Empty;

          if (swhere.Length > 0) swhere += " AND ";
          swhere += " MensajeUsuariosSociales.UsuarioSocialID =  @UsuarioSocialID";
          dbParameter = dbCommand.CreateParameter();
          dbParameter.ParameterName = "@UsuarioSocialID";
          dbParameter.DbType = DbType.Int64;
          dbParameter.Value = Convert.ToInt64(usuarioSocialId);
          dbCommand.Parameters.Add(dbParameter);

          //Solo consultara los usuarios activos
          if (swhere.Length > 0) swhere += " AND ";
          swhere += " MensajeUsuariosSociales.Activo = 1";

          subquery.Append(" (SELECT MensajeID");
          subquery.Append(" FROM MensajeUsuariosSociales ");
          if (swhere.Length > 0) subquery.Append(" WHERE " + swhere);
          subquery.Append(")");

          #endregion
          swhere = string.Empty;
          query.Append(" SELECT");
          query.Append(" Mensaje.MensajeID, Mensaje.Asunto,Mensaje.Contenido, Mensaje.FechaMensaje,Mensaje.Estatus, Mensaje.GuidConversacion,  ");
          query.Append(" Mensaje.RemitenteID,");
          query.Append(" UsuarioSocial.LoginName as RemitenteLoginName, UsuarioSocial.ScreenName as RemitenteScreenName, ");
          query.Append(" rownumber = ROW_NUMBER() ");
          query.Append(" OVER (ORDER BY ");
          query.Append(string.Format(" {0} ", sortColumn));
          query.Append(string.Format(" {0} ", sortOrder));
          query.Append(" ) ");
          query.Append(" FROM ");
          query.Append(" Mensaje INNER JOIN UsuarioSocial ON Mensaje.RemitenteID = UsuarioSocial.UsuarioSocialID");
          query.Append(" WHERE Mensaje.GuidConversacion IN ( ");
          query.Append(subquery.ToString());
          query.Append(")");
        
          //Indica si se consultaran los mensajes padres o los hijos
          if (esPadre != null)
          {
              if (swhere.Length > 0) swhere += " AND ";
              swhere += (bool)esPadre
                            ? " (Mensaje.MensajeID = Mensaje.GuidConversacion) "
                            : " (Mensaje.MensajeID <> Mensaje.GuidConversacion) ";
          }

          if (parametros.ContainsKey("MensajeID"))
          {
              if (swhere.Length > 0) swhere += " AND ";
              swhere += " Mensaje.MensajeID =  @MensajeID";
              dbParameter = dbCommand.CreateParameter();
              dbParameter.ParameterName = "@MensajeID";
              dbParameter.DbType = DbType.String;
              dbParameter.Value = Convert.ToString(parametros["MensajeID"].Trim());
              dbCommand.Parameters.Add(dbParameter);
          }

          if (parametros.ContainsKey("RemitenteID"))
          {
              if (swhere.Length > 0) swhere += " AND ";
              swhere += " Mensaje.RemitenteID =  @RemitenteID";
              dbParameter = dbCommand.CreateParameter();
              dbParameter.ParameterName = "@RemitenteID";
              dbParameter.DbType = DbType.Int64;
              dbParameter.Value = Convert.ToInt64(parametros["RemitenteID"].Trim());
              dbCommand.Parameters.Add(dbParameter);
          }

          if (parametros.ContainsKey("Estatus"))
          {
              if (swhere.Length > 0) swhere += " AND ";
              swhere += " Mensaje.Estatus = @Estatus";
              dbParameter = dbCommand.CreateParameter();
              dbParameter.ParameterName = "@Estatus";
              dbParameter.DbType = DbType.Boolean;
              dbParameter.Value = Convert.ToBoolean(int.Parse(parametros["Estatus"].Trim()));
              dbCommand.Parameters.Add(dbParameter);
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

          if (swhere.Length > 0) query.Append(" AND " + swhere);

          queryCols.Append("SELECT MensajeID,Asunto,Contenido,FechaMensaje,Estatus,GuidConversacion,RemitenteID,");
          queryCols.Append(" RemitenteLoginName,RemitenteScreenName,");
          queryCols.Append(" rownumber");
          queryCols.Append(" FROM ( ");
          queryCols.Append(query.ToString());
          queryCols.Append(" ) as Mensajes");
          queryCols.Append(" WHERE rownumber BETWEEN cast(@PageSize * @PageIndex + 1 as varchar(10))");
          queryCols.Append(" AND cast(@PageSize * (@PageIndex + 1) as varchar(10))");

          try
          {
              DbDataAdapter adapter = dctx.CreateDataAdapter();
              dbCommand.CommandText = queryCols.ToString();
              adapter.SelectCommand = dbCommand;
              adapter.Fill(ds, "Mensaje");
          }
          catch (Exception ex)
          {
              throw new Exception(string.Format("MensajeDARetHlp: Ocurrió un problema al recuperar las publicaciones: {0}", ex.Message));
          }
          return ds;
      }
   } 
}
