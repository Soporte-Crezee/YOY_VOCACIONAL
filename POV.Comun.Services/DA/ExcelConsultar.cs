using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;

namespace POV.Comun.DA
{
    /// <summary>
    /// Consulta la información de un libro de excel.
    /// </summary>
    internal class ExcelConsultar
    {
        /// <summary>
        /// Consultat la información de las hojas del archivo de excel
        /// </summary>
        /// <param name="conn">El OleDbConnection que proveerá acceso a la libro de excel</param>
        /// <returns>DataSet con la inforación consultada</returns>
        public DataSet Accion(OleDbConnection conn)
        {
            DataSet dsExcel = new DataSet();
            DataTable datatable = new DataTable();
            string workSheetName = "";
            try
            {
                conn.Open();
            }
            catch (Exception ex)
            {
                throw new Exception("ExcelConsultar: Hubo un error al conectarse al Excel" + Environment.NewLine + ex.Message);
            }
            try
            {
                //se obtiene las hojas del libro de excel.
                datatable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                foreach (DataRow row in datatable.Rows)
                {
                    workSheetName = row["TABLE_NAME"].ToString().Replace("$", "").Replace("'", "");
                    DataTable dt = this.GetWorksheet(workSheetName, conn);
                    if (dt != null)
                    {
                        dsExcel.Tables.Add(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try
                {
                    if (conn != null)
                    {
                        conn.Close();
                        conn.Dispose();
                        if (datatable != null)
                            datatable.Dispose();
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                throw new Exception("ExcelConsultar: Hubo un error al consultar los registros. " + exmsg);
            }
            finally
            {
                try
                {
                    conn.Close();
                    conn.Dispose();
                    datatable.Dispose();
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return dsExcel;
        }
        /// <summary>
        /// Consulta los registros de una hoja de excel
        /// </summary>
        /// <param name="worksheetName">String con el nombre de la hoja de excel</param>
        /// <param name="conn">El OleDbConnection que proveerá acceso a la libro de excel</param>
        /// <returns>DataTable con la información de los registro consultados</returns>
        public DataTable GetWorksheet(string worksheetName, OleDbConnection conn)
        {
            OleDbDataAdapter cmd = new System.Data.OleDb.OleDbDataAdapter(
                "select * from [" + worksheetName + "$]", conn);
            DataSet excelDataSet = new DataSet();
            cmd.Fill(excelDataSet, worksheetName);
            return excelDataSet.Tables[0].Copy();
        }
    }
}
