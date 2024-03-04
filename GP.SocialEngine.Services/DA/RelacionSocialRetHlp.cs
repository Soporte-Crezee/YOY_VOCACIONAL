using System.Data;
using Framework.Base.DataAccess;
using System.Collections.Generic;
using System;
using Framework.Base.Exceptions;
using System.Data.Common;
using System.Text;

namespace GP.SocialEngine.DA { 
   /// <summary>
   /// Consulta las Relaciones de los Usuarios en el Sistema
   /// </summary>
   public class RelacionSocialRetHlp { 
	  /// <summary>
	  /// Consulta registros de Consulta las Relaciones de los Usuarios en el Sistema en la base de datos.
	  /// </summary>
	  /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
	  /// <param name="consultalasRelacionesdelosUsuariosenelSistema">Consulta las Relaciones de los Usuarios en el Sistema que provee el criterio de selección para realizar la consulta</param>
	  /// <returns>El DataSet que contiene la información de Consulta las Relaciones de los Usuarios en el Sistema generada por la consulta</returns>
	  public DataSet Action(IDataContext dctx, Dictionary<string, string> DetalleRelacionSocial){
		 object myFirm = new object();
		 string sError = string.Empty;
		 if (DetalleRelacionSocial == null)
			sError += ", DetalleRelacionSocial";
		 if (sError.Length > 0)
			throw new Exception("RelacionSocialRetlHlp: Los siguientes Campos no pueden ser vacios: " + sError.Substring(2) + " ");
	  if (!DetalleRelacionSocial.ContainsKey("PropietarioID")) {
						sError+= "PropietarioID";
	  }
		 if (sError.Length > 0)
			throw new Exception("RelacionSocialRetlHlp: Los siguientes Campos no pueden ser vacios: " + sError.Substring(2) + " ");
					long PropietarioID = Convert.ToInt64(DetalleRelacionSocial["PropietarioID"]);
					string EmailContacto = null;
					string Etiqueta = null;
	  if (DetalleRelacionSocial.ContainsKey("EmailContacto")) {
						EmailContacto = DetalleRelacionSocial["EmailContacto"];
	  }
	  if (DetalleRelacionSocial.ContainsKey("Etiqueta")) {
						Etiqueta = DetalleRelacionSocial["Etiqueta"];
	  }
		 if (dctx == null)
	  throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DA", 
		 "RelacionSocialRetHlp", "Action", null, null);
		 DbCommand sqlCmd = null;
		 try{
			dctx.OpenConnection(myFirm);
			sqlCmd = dctx.CreateCommand();
		 } catch(Exception ex){
	  throw new StandardException(MessageType.Error, "", "RelacionSocialRetHlp: Ocurrio un error al Conectarse a la Base de Datos", "GP.SocialEngine.DA", 
		 "RelacionSocialRetHlp", "Action", null, null);
		 }
		 DbParameter sqlParam;
		 StringBuilder sCmd = new StringBuilder();
		 sCmd.Append(" SELECT Etiqueta,PropietarioID,ContactoID,UsuarioContacto,EmailContacto,GrupoSocialID,NombreContacto ");
		 sCmd.Append(" FROM VistaRelacionSocial ");
		 StringBuilder s_VarWHERE = new StringBuilder();
		 if (PropietarioID != null){
			s_VarWHERE.Append(" PropietarioID = @PropietarioID ");
			sqlParam = sqlCmd.CreateParameter();
			sqlParam.ParameterName = "PropietarioID";
			sqlParam.Value = PropietarioID;
			sqlParam.DbType = DbType.Int64;
			sqlCmd.Parameters.Add(sqlParam);
		 }
		 if (EmailContacto != null){
			s_VarWHERE.Append(" AND EmailContacto LIKE @EmailContacto ");
			sqlParam = sqlCmd.CreateParameter();
			sqlParam.ParameterName = "EmailContacto";
			sqlParam.Value = EmailContacto;
			sqlParam.DbType = DbType.String;
			sqlCmd.Parameters.Add(sqlParam);
		 }
		 if (Etiqueta != null){
			s_VarWHERE.Append(" AND Etiqueta = @Etiqueta ");
			sqlParam = sqlCmd.CreateParameter();
			sqlParam.ParameterName = "Etiqueta";
			sqlParam.Value = Etiqueta;
			sqlParam.DbType = DbType.String;
			sqlCmd.Parameters.Add(sqlParam);
		 }
		 string s_VarWHEREres = s_VarWHERE.ToString().Trim();
		 if (s_VarWHEREres.Length > 0){
			if (s_VarWHEREres.StartsWith("AND "))
			   s_VarWHEREres = s_VarWHEREres.Substring(4);
			else if (s_VarWHEREres.StartsWith("OR "))
			   s_VarWHEREres = s_VarWHEREres.Substring(3);
			else if (s_VarWHEREres.StartsWith(","))
			   s_VarWHEREres = s_VarWHEREres.Substring(1);
			sCmd.Append(" WHERE " + s_VarWHEREres);
		 }
		 DataSet ds = new DataSet();
		 DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
		 sqlAdapter.SelectCommand = sqlCmd;
		 try{
			sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
			sqlAdapter.Fill(ds, "RelacionSocial");
		 } catch(Exception ex){
			string exmsg = ex.Message;
			try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
			throw new Exception("RelacionSocialRetHlp: Ocurrio un Error al Consultar en la Base de Datos. " + exmsg);
		 } finally{
			try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
		 }
		 return ds;
	  }
   } 
}
