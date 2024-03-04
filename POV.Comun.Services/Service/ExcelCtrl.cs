using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using POV.Comun.DA;

namespace POV.Comun.Service
{
    /// <summary>
    /// Controlador para el acceso a los datos de un archivo de excel.
    /// </summary>
    public class ExcelCtrl
    {
        /// <summary>
        /// Consultar los registro de un archivo de excel.
        /// </summary>
        /// <param name="urlExcel">String con la url del archivo excel a consultar</param>
        /// <returns></returns>
        public DataSet Consultar(string urlExcel)
        {
            string connectionString = "";
            string extension = urlExcel.Substring(urlExcel.LastIndexOf("."));

            if (extension.CompareTo(".xlsx") == 0)
                //read a 2007 file  
                connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + urlExcel + ";Extended Properties=\"Excel 8.0;HDR=YES;\"";
            else if (extension.CompareTo(".xls") == 0)
                //read a 97-2003 file  
                connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + urlExcel + ";Extended Properties=Excel 8.0;";
            else
                throw new Exception("Formato de excel incorrecto");
            try
            {
                OleDbConnection SQLConexion = new OleDbConnection(connectionString);
                ExcelConsultar da = new ExcelConsultar();
                return da.Accion(SQLConexion);
            }
            catch (Exception e)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, e);
                throw e;
            }
        }
    }
}
