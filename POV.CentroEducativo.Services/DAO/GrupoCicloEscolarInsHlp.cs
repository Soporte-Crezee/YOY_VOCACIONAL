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
    /// Inserta un GrupoCicloEscolar en la base de datos
    /// </summary>
    public class GrupoCicloEscolarInsHlp
    {
        /// <summary>
        /// Crea un registro de GrupoCicloEscolar en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="grupoCicloEscolar">GrupoCicloEscolar que desea crear</param>
        public void Action(IDataContext dctx, GrupoCicloEscolar grupoCicloEscolar)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (grupoCicloEscolar == null)
                sError += ", grupoCicloEscolar";
            if (sError.Length > 0)
                throw new Exception("GrupoCicloEscolarInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
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
          
            if (sError.Length > 0)
                throw new Exception("GrupoCicloEscolarInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (grupoCicloEscolar.CicloEscolar.CicloEscolarID == null)
                sError += ", CicloEscolar.CicloEscolarID";
            if (grupoCicloEscolar.Escuela.EscuelaID == null)
                sError += ", Escuela.EscuelaID";
            if (grupoCicloEscolar.Grupo.GrupoID == null)
                sError += ", Grupo.GrupoID";
            if (grupoCicloEscolar.PlanEducativo.PlanEducativoID == null)
                sError += ", PlanEducativo.PlanEducativoID";
            if (sError.Length > 0)
                throw new Exception("GrupoCicloEscolarInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO", "GrupoCicloEscolarInsHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "GrupoCicloEscolarInsHlp: No se pudo conectar a la base de datos", "POV.CentroEducativo.DAO", "GrupoCicloEscolarInsHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" INSERT INTO GRUPOCICLOESCOLAR (GRUPOCICLOESCOLARID,GRUPOSOCIALID,CLAVE,CICLOESCOLARID,ESCUELAID,GRUPOID,PLANEDUCATIVOID,ACTIVO) ");
            // grupoCicloEscolar.GrupoCicloEscolarID
            sCmd.Append(" VALUES (@dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            if (grupoCicloEscolar.GrupoCicloEscolarID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = grupoCicloEscolar.GrupoCicloEscolarID;
            sqlParam.DbType = DbType.Guid;
            sqlCmd.Parameters.Add(sqlParam);
            // grupoCicloEscolar.GrupoSocialID
            sCmd.Append(" ,@dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            if (grupoCicloEscolar.GrupoSocialID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = grupoCicloEscolar.GrupoSocialID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
            // grupoCicloEscolar.Clave
            sCmd.Append(" ,@dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            if (grupoCicloEscolar.Clave == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = grupoCicloEscolar.Clave;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            // grupoCicloEscolar.CicloEscolar.CicloEscolarID
            sCmd.Append(" ,@dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            if (grupoCicloEscolar.CicloEscolar.CicloEscolarID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = grupoCicloEscolar.CicloEscolar.CicloEscolarID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            // grupoCicloEscolar.Escuela.EscuelaID
            sCmd.Append(" ,@dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            if (grupoCicloEscolar.Escuela.EscuelaID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = grupoCicloEscolar.Escuela.EscuelaID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            // grupoCicloEscolar.Grupo.GrupoID
            sCmd.Append(" ,@dbp4ram6 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram6";
            if (grupoCicloEscolar.Grupo.GrupoID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = grupoCicloEscolar.Grupo.GrupoID;
            sqlParam.DbType = DbType.Guid;
            sqlCmd.Parameters.Add(sqlParam);
            // grupoCicloEscolar.PlanEducativo.PlanEducativoID
            sCmd.Append(" ,@dbp4ram7 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram7";
            if (grupoCicloEscolar.PlanEducativo.PlanEducativoID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = grupoCicloEscolar.PlanEducativo.PlanEducativoID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);

            //grupoCicloEscolar.Activo
            sCmd.Append(" ,@dbp4ram8 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram8";
            if (grupoCicloEscolar.Activo == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = grupoCicloEscolar.Activo;
            sqlParam.DbType = DbType.Boolean;
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
                throw new Exception("GrupoCicloEscolarInsHlp: Ocurrio un error al ingresar el registro. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("GrupoCicloEscolarInsHlp: Ocurrio un error al ingresar el registro.");
        }
    }
}
