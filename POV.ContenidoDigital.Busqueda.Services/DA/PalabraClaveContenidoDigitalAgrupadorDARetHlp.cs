using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.ContenidosDigital.Busqueda.BO;

namespace POV.ContenidosDigital.Busqueda.DA { 
   /// <summary>
   /// Consulta un registro de PalabraClaveContenidoDigital en la BD
   /// </summary>
   internal class PalabraClaveContenidoDigitalAgrupadorDARetHlp { 
      /// <summary>
      /// Consulta registros de PalabraClaveContenidoDigital en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="palabraClaveContenidoDigital">PalabraClaveContenidoDigital que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de PalabraClaveContenidoDigital generada por la consulta</returns>
      public DataSet Action(IDataContext dctx, PalabraClaveContenidoDigital palabraClaveContenido){
         object myFirm = new object();
         string sError = String.Empty;
         if (palabraClaveContenido == null)
            sError += ", PalabraClaveContenidoDigital";
         if (sError.Length > 0)
            throw new Exception("PalabraClaveContenidoDigitalAgrupadorDARetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.ContenidosDigital.Busqueda.DA", 
         "PalabraClaveContenidoDigitalAgrupadorDARetHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "PalabraClaveContenidoDigitalAgrupadorDARetHlp: No se pudo conectar a la base de datos", "POV.ContenidosDigital.Busqueda.DA", 
         "PalabraClaveContenidoDigitalAgrupadorDARetHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" SELECT cda.ContenidoDigitalAgrupadorID, cda.EjeTematicoID, cda.SituacionAprendizajeID, cda.AgrupadorContenidoDigitalID, cda.ContenidoDigitalID, cda.Activo, cda.FechaRegistro, agd.TipoAgrupador ");
         sCmd.Append(" FROM PalabraClaveContenido pcc ");
         sCmd.Append(" JOIN PalabraClaveContenidoDigital pccd ON pccd.PalabraClaveContenidoID = pcc.PalabraClaveContenidoID ");
         sCmd.Append(" JOIN ContenidoDigitalAgrupador cda ON cda.ContenidoDigitalAgrupadorID = pccd.ContenidoDigitalAgrupadorID ");
         sCmd.Append(" JOIN SituacionAprendizaje sit ON sit.SituacionAprendizajeID = cda.SituacionAprendizajeID AND sit.EstatusProfesionalizacion <> 0 ");
         sCmd.Append(" JOIN EjeTematico eje ON eje.EjeTematicoID = cda.EjeTematicoID AND eje.EstatusProfesionalizacion <> 0 ");
         sCmd.Append(" JOIN AgrupadorContenidoDigital agd on agd.AgrupadorContenidoDigitalID = cda.AgrupadorContenidoDigitalID and agd.EstatusProfesionalizacion <> 0 ");
         sCmd.Append(" WHERE ");
         StringBuilder s_Var = new StringBuilder();
         if (palabraClaveContenido.PalabraClaveContenidoID != null){
            s_Var.Append(" pcc.PalabraClaveContenidoID = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = palabraClaveContenido.PalabraClaveContenidoID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
         }
         s_Var.Append("  ");
         string s_Varres = s_Var.ToString().Trim();
         if (s_Varres.Length > 0){
            if (s_Varres.StartsWith("AND "))
               s_Varres = s_Varres.Substring(4);
            else if (s_Varres.StartsWith("OR "))
               s_Varres = s_Varres.Substring(3);
            else if (s_Varres.StartsWith(","))
               s_Varres = s_Varres.Substring(1);
            sCmd.Append("  " + s_Varres);
         }
         DataSet ds = new DataSet();
         DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
         sqlAdapter.SelectCommand = sqlCmd;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            sqlAdapter.Fill(ds, "PalabraClaveContenido");
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("PalabraClaveContenidoDigitalAgrupadorDARetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }
   } 
}
