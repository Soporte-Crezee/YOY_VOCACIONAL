using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace POV.Prueba.Diagnostico.DAO.Bullying
{
    public class ViewResultadoPruebaComunicacionRetHlp
    {
        public DataSet Action(IDataContext dctx, Alumno alumno)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (alumno == null)
                sError += ", Alumno";
            
            if (sError.Length > 0)
                throw new Exception("ViewResultadoPruebaComunicacionRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));

            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Administracion.DAO", "ViewResultadoPruebaComunicacionRetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "ViewResultadoPruebaComunicacionRetHlp: No se pudo conectar a la base de datos", "POV.Administracion.DAO", "ViewResultadoPruebaComunicacionRetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT Clave, ClasificadorID, Clasificador, NombreReactivo, Respuesta, Valor ");
            sCmd.Append(" FROM ViewResultadoBullyingComunicacion ");
            StringBuilder s_Var = new StringBuilder();
            if (alumno.AlumnoID != null)
            {
                s_Var.Append(" AlumnoID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = alumno.AlumnoID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }           

            s_Var.Append("  ");
            string s_Varres = s_Var.ToString().Trim();
            if (s_Varres.Length > 0)
            {
                if (s_Varres.StartsWith("AND "))
                    s_Varres = s_Varres.Substring(4);
                else if (s_Varres.StartsWith("OR "))
                    s_Varres = s_Varres.Substring(3);
                else if (s_Varres.StartsWith(","))
                    s_Varres = s_Varres.Substring(1);
                sCmd.Append(" WHERE " + s_Varres);
            }
            DataSet ds = new DataSet();
            DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
            sqlAdapter.SelectCommand = sqlCmd;
            try
            {
                sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
                sqlAdapter.Fill(ds, "ViewResultadoBullyingComunicacion");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("ViewResultadoPruebaComunicacionRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            return ds;
        }
    }
}
