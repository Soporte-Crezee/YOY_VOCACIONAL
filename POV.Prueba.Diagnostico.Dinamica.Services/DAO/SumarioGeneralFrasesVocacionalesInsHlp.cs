using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Prueba.Diagnostico.Dinamica.BO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace POV.Prueba.Diagnostico.Dinamica.DAO
{
    internal class SumarioGeneralFrasesVocacionalesInsHlp
    {
        /// <summary>
        /// Crea un registro de SumarioGeneralSacks en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="sumarioGeneralSacks">SumarioGeneralSacks que desea crear</param>
        public void Action(IDataContext dctx, SumarioGeneralFrasesVocacionales sumarioGeneralFrasesVocacionales)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (sumarioGeneralFrasesVocacionales == null)
                sError += ", SumarioGeneralFrasesVocacionales";
            if (sError.Length > 0)
                throw new Exception("SumarioGeneralFrasesVocacionalesInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (sumarioGeneralFrasesVocacionales.Prueba == null)
                sError += ", Prueba";
            if (sumarioGeneralFrasesVocacionales.Alumno == null)
                sError += "Alumno";
            if (sError.Length > 0)
                throw new Exception("SumarioGeneralFrasesVocacionalesInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (sumarioGeneralFrasesVocacionales.Prueba.PruebaID == null)
                sError += ", PruebaID";
            if (sumarioGeneralFrasesVocacionales.Alumno.AlumnoID == null)
                sError += ", AlumnoID";
            if (sumarioGeneralFrasesVocacionales.SumarioOrganizacionPersonalidad == null)
                sError += ", SumarioOrganizacionPersonalidad";
            if (sumarioGeneralFrasesVocacionales.SumarioPerspectivaOpciones == null)
                sError += "SumarioPerspectivaOpciones";
            if (sumarioGeneralFrasesVocacionales.SumarioFuentesConflicto == null)
                sError += "SumarioFuentesConflicto";
            if (sumarioGeneralFrasesVocacionales.FechaRegistro == null)
                sError += ", FechaRegistro";
            if (sError.Length > 0)
                throw new Exception("SumarioGeneralFrasesVocacionalesInsHlp:Los siguientes campos no pueden ser vacios" + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Prueba.Diagnostico.Dinamica.DAO",
                   "SumarioGeneralFrasesVocacionalesInsHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "SumarioGeneralSacksInsHlp: No se pudo conectar a la base de datos", "POV.Prueba.Diagnostico.Dinamica.DAO",
                   "SumarioGeneralFrasesVocacionalesInsHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" INSERT INTO SumarioGeneralFrasesVocacionales(PruebaID,AlumnoID,SumarioOrganizacionPersonalidad,SumarioPerspectivaOpciones,SumarioFuentesConflicto,FechaRegistro) ");
            sCmd.Append(" VALUES ");
            sCmd.Append(" ( ");
            // sumarioGeneralSacks.Prueba.PruebaID
            sCmd.Append(" @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            if (sumarioGeneralFrasesVocacionales.Prueba.PruebaID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = sumarioGeneralFrasesVocacionales.Prueba.PruebaID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            // sumarioGeneralSacks.Alumno.AlumnoID
            sCmd.Append(" ,@dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            if (sumarioGeneralFrasesVocacionales.Alumno.AlumnoID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = sumarioGeneralFrasesVocacionales.Alumno.AlumnoID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
            // sumarioGeneralSacks.SumarioMadurez
            sCmd.Append(" ,@dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            if (sumarioGeneralFrasesVocacionales.SumarioOrganizacionPersonalidad == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = sumarioGeneralFrasesVocacionales.SumarioOrganizacionPersonalidad;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            // sumarioGeneralSacks.SumarioNivelRealida
            sCmd.Append(" ,@dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            if (sumarioGeneralFrasesVocacionales.SumarioPerspectivaOpciones == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = sumarioGeneralFrasesVocacionales.SumarioPerspectivaOpciones;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            // sumarioGeneralSacks.SumarioConflictoExpresados
            sCmd.Append(" ,@dbp4ram6 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram6";
            if (sumarioGeneralFrasesVocacionales.SumarioFuentesConflicto == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = sumarioGeneralFrasesVocacionales.SumarioFuentesConflicto;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            // sumarioGeneralSacks.FechaRegistro
            sCmd.Append(" ,@dbp4ram7 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram7";
            if (sumarioGeneralFrasesVocacionales.FechaRegistro == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = sumarioGeneralFrasesVocacionales.FechaRegistro;
            sqlParam.DbType = DbType.DateTime;
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
                throw new Exception("SumarioGeneralFrasesVocacionalesInsHlp: Ocurrio un error al ingresar el registro. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("SumarioGeneralFrasesVocacionalesInsHlp: Ocurrio un error al ingresar el registro.");
        }
    }
}
