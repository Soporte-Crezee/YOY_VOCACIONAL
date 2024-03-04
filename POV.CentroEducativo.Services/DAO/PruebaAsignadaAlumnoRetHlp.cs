using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace POV.CentroEducativo.DAO
{
    internal class PruebaAsignadaAlumnoRetHlp
    {
        public DataSet Action(IDataContext dctx, Alumno alumno) 
        {
            object myFirm = new object();
            string sError = String.Empty;

            if (alumno == null)
                sError += ",Alumno";
            if (sError.Length > 0)
                throw new Exception("PruebaAsignadaAlumnoRetHlp: Los siguientes datos no pueden ser vacios " + sError.Substring(2));
            if(dctx==null)
                throw new StandardException(MessageType.Error,"","DataContext no puede ser nulo","POV.Prueba.Diagnostico.DAO",
                    "PruebaAsignadaAlumnoRetHlp","Action",null,null);

            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "PruebaAsignadaAlumnoRetHlp: No se pudo conectar a la base de datos", "POV.Prueba.Diagnostico.DAO",
                   "PruebaAsignadaAlumnoRetHlp", "Action", null, null);
            }

            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();

            sCmd.Append("select pc.PruebaID, rpd.EstadoPrueba, p.TipoPruebaPresentacion");
            sCmd.Append(" from dbo.ExpedienteEscolar e");
			sCmd.Append(" left outer join dbo.ResultadoPruebaDinamica repd on repd.AlumnoID = e.AlumnoID");
			sCmd.Append(" left outer join dbo.RegistroPruebaDinamica rpd on rpd.ResultadoPruebaID = repd.ResultadoPruebaID");
			sCmd.Append(" left outer join dbo.resultadoprueba rp on rp.ResultadoPruebaID = rpd.ResultadoPruebaID");
			sCmd.Append(" left outer join dbo.PruebaContrato pc on pc.PruebaID = rp.PruebaID");
            sCmd.Append(" left outer join dbo.prueba p on p.pruebaid = pc.PruebaID");

            StringBuilder s_VarWHERE = new StringBuilder();
            if (alumno.AlumnoID != null) 
            {
                s_VarWHERE.Append(" e.alumnoid = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = alumno.AlumnoID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }

            string s_VarWHEREres = s_VarWHERE.ToString().Trim();
            string s_VarGroup = " GROUP BY pc.PruebaID, rpd.EstadoPrueba,  p.TipoPruebaPresentacion";
            if (s_VarWHEREres.Length > 0) 
            {
                if (s_VarWHEREres.StartsWith("AND "))
                    s_VarWHEREres = s_VarWHEREres.Substring(4);
                else if (s_VarWHEREres.StartsWith("OR "))
                    s_VarWHEREres = s_VarWHEREres.Substring(3);
                else if (s_VarWHEREres.StartsWith(","))
                    s_VarWHEREres = s_VarWHEREres.Substring(1);
                sCmd.Append(" WHERE " + s_VarWHEREres + s_VarGroup);
            }
            DataSet ds = new DataSet();
            DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
            sqlAdapter.SelectCommand = sqlCmd;

            try
            {
                sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
                sqlAdapter.Fill(ds, "AsignacionRecurso");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("PruebaAsignadaAlumnoRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
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
