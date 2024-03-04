using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;

namespace POV.CentroEducativo.DAO
{
    /// <summary>
    /// Actualiza un grupoCicloEscolar en la base de datos
    /// </summary>
    public class GrupoCicloEscolarUpdHlp
    {
        /// <summary>
        /// Actualiza de manera optimista un registro de Actualizar GrupoCicloEscolar en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="actualizarGrupoCicloEscolar">Actualizar GrupoCicloEscolar que tiene los datos nuevos</param>
        /// <param name="anterior">Actualizar GrupoCicloEscolar que tiene los datos anteriores</param>
        public void Action(IDataContext dctx, GrupoCicloEscolar grupoCicloEscolar, GrupoCicloEscolar anterior)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (grupoCicloEscolar == null)
                sError += ", GrupoCicloEscolar";
            if (anterior == null)
                sError += ", Anterior";
            if (sError.Length > 0)
                throw new Exception("GrupoCicloEscolarUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (grupoCicloEscolar.GrupoCicloEscolarID == null)
                sError += ", GrupoCicloEscolarID";
            if (grupoCicloEscolar.GrupoSocialID == null)
                sError += ", GrupoSocialID";
            if (grupoCicloEscolar.Clave == null || grupoCicloEscolar.Clave.Trim().Length == 0)
                sError += ", Clave";
            if (grupoCicloEscolar.CicloEscolar == null)
                sError += ", CicloEscolar";
            if (grupoCicloEscolar.Escuela == null)
                sError += ", Escuela";
            if (grupoCicloEscolar.Grupo == null)
                sError += ", Grupo";
            if (grupoCicloEscolar.PlanEducativo == null)
                sError += ", PlanEducativo";
            if (anterior.GrupoCicloEscolarID == null)
                sError += ", GrupoCicloEscolarID anterior";
            if (anterior.GrupoSocialID == null)
                sError += ", GrupoSocialID  anterior";
            if (anterior.Clave == null || anterior.Clave.Trim().Length == 0)
                sError += ", Clave anterior";
            if (anterior.CicloEscolar == null)
                sError += ", CicloEscolar  anterior";
            if (anterior.Escuela == null)
                sError += ", Escuela  anterior";
            if (anterior.Grupo == null)
                sError += ", Grupo  anterior";
            if (anterior.PlanEducativo == null)
                sError += ", PlanEducativo anterior";
            if (sError.Length > 0)
                throw new Exception("GrupoCicloEscolarUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (grupoCicloEscolar.CicloEscolar.CicloEscolarID == null)
                sError += ", CicloEscolar.CicloEscolarID";
            if (grupoCicloEscolar.Escuela.EscuelaID == null)
                sError += ", Escuela.EscuelaID";
            if (grupoCicloEscolar.Grupo.GrupoID == null)
                sError += ", Grupo.GrupoID";
            if (grupoCicloEscolar.PlanEducativo.PlanEducativoID == null)
                sError += ", PlanEducativo.PlanEducativoID";
            if (anterior.CicloEscolar.CicloEscolarID == null)
                sError += ", CicloEscolar.CicloEscolarID anterior";
            if (anterior.Escuela.EscuelaID == null)
                sError += ", Escuela.EscuelaID anterior";
            if (anterior.Grupo.GrupoID == null)
                sError += ", Grupo.GrupoID anterior";
            if (anterior.PlanEducativo.PlanEducativoID == null)
                sError += ", PlanEducativo.PlanEducativoID anterior";
            if (sError.Length > 0)
                throw new Exception("GrupoCicloEscolarUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (grupoCicloEscolar.GrupoCicloEscolarID != anterior.GrupoCicloEscolarID)
                sError = "Los parametros no coinciden";
            if (sError.Length > 0)
                throw new Exception("GrupoCicloEscolarUpdHlp: " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO", "GrupoCicloEscolarUpdHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "GrupoCicloEscolarUpdHlp: Ocurrió un error al conectarse a la base de datos", "POV.CentroEducativo.DAO", "GrupoCicloEscolarUpdHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" UPDATE GRUPOCICLOESCOLAR ");
            sCmd.Append(" SET CLAVE =@dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            if (grupoCicloEscolar.Clave == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = grupoCicloEscolar.Clave;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);

            if (grupoCicloEscolar.Activo == null)
                sCmd.Append(" ,ACTIVO = NULL ");
            else
            {
                sCmd.Append(" ,ACTIVO = @dbp4ram10 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram10";
                sqlParam.Value = grupoCicloEscolar.Activo;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (grupoCicloEscolar.PlanEducativo == null)
            {
                sCmd.Append(" ,PLANEDUCATIVOID = NULL ");
            }
            else
            {
                sCmd.Append(" ,PLANEDUCATIVOID = @dbp4ram11");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram11";
                sqlParam.Value = grupoCicloEscolar.PlanEducativo.PlanEducativoID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }

            if (anterior.GrupoCicloEscolarID == null)
            {
                sCmd.Append(" WHERE GRUPOCICLOESCOLARID IS NULL ");
            }
            else
            {
                sCmd.Append(" WHERE GRUPOCICLOESCOLARID = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = anterior.GrupoCicloEscolarID;
                sqlParam.DbType = DbType.Guid;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.GrupoSocialID == null)
            {
                sCmd.Append(" AND GRUPOSOCIALID IS NULL ");
            }
            else
            {
                sCmd.Append(" AND GRUPOSOCIALID = @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = anterior.GrupoSocialID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.Clave == null)
            {
                sCmd.Append(" AND CLAVE IS NULL ");
            }
            else
            {
                sCmd.Append(" AND CLAVE = @dbp4ram4 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = anterior.Clave;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.CicloEscolar.CicloEscolarID == null)
            {
                sCmd.Append(" AND CICLOESCOLARID IS NULL ");
            }
            else
            {
                sCmd.Append(" AND CICLOESCOLARID = @dbp4ram5 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram5";
                sqlParam.Value = anterior.CicloEscolar.CicloEscolarID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.Escuela.EscuelaID == null)
            {
                sCmd.Append(" AND ESCUELAID IS NULL ");
            }
            else
            {
                sCmd.Append(" AND ESCUELAID = @dbp4ram6 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram6";
                sqlParam.Value = anterior.Escuela.EscuelaID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.Grupo.GrupoID == null)
            {
                sCmd.Append(" AND GRUPOID IS NULL ");
            }
            else
            {
                sCmd.Append(" AND GRUPOID = @dbp4ram7 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram7";
                sqlParam.Value = anterior.Grupo.GrupoID;
                sqlParam.DbType = DbType.Guid;
                sqlCmd.Parameters.Add(sqlParam);
            }
            
            if (anterior.Activo == null)
            {
                sCmd.Append(" AND ACTIVO IS NULL ");
            }
            else
            {
                sCmd.Append(" AND ACTIVO = @dbp4ram8 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram8";
                sqlParam.Value = anterior.Activo;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.PlanEducativo.PlanEducativoID == null)
            {
                sCmd.Append(" AND PLANEDUCATIVOID IS NULL ");
            }
            else
            {
                sCmd.Append(" AND PLANEDUCATIVOID = @dbp4ram9 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram9";
                sqlParam.Value = anterior.PlanEducativo.PlanEducativoID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
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
                throw new Exception("GrupoCicloEscolarUpdHlp: Ocurrio un error al actualizar el objeto o fue modificado mientras era editado. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("GrupoCicloEscolarUpdHlp: Ocurrio un error al actualizar el objeto o fue modificado mientras era editado.");
        }
    }
}
