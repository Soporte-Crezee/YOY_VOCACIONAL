using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Licencias.BO;
using POV.Licencias.DAO;


namespace POV.Licencias.Service
{
    /// <summary>
    /// Controlador de los modulos funcionales
    /// </summary>
    public class ModuloFuncionalCtrl
    {
        /// <summary>
        /// Recupera los registros de los modulos funcionales registrados en el sistema
        /// </summary>
        /// <param name="dctx">Data context que provee la conexion a la base de datos</param>
        /// <param name="moduloFuncional">Modulo funcional que se usara como filtro de consulta</param>
        /// <returns>DataSet que contiene los resultados de la consulta</returns>
        public DataSet Retrieve(IDataContext dctx, ModuloFuncional moduloFuncional)
        {
            ModeloFuncionalRetHlp da = new ModeloFuncionalRetHlp();
            DataSet ds = da.Action(dctx, moduloFuncional);
            return ds;
        }

        /// Crea un objeto de Contrato a partir de los datos del último DataRow de la primera DataTable del DataSet.
        /// </summary>
        /// <param name="ds">El DataSet que contiene la información de Contrato</param>
        /// <returns>Un objeto Contrato creado a partir del DataSet</returns>
        public ModuloFuncional LastDataRowToModuloFuncional(DataSet ds)
        {
            if (!ds.Tables.Contains("ModuloFuncional"))
                throw new Exception("LastDataRowToModuloFuncional: DataSet no tiene la tabla Ubicacion");
            int index = ds.Tables["ModuloFuncional"].Rows.Count;
            if (index < 1)
                throw new Exception("LastDataRowToLicenciaEscuela: El DataSet no tiene filas");
            return this.DataRowToModuloFuncional(ds.Tables["ModuloFuncional"].Rows[index - 1]);
        
        }

        /// <summary>
        /// Crea un objeto Contrato a partir de los datos de un DataRow.
        /// </summary>
        /// <param name="row">El DataRow que contiene la información de Contrato</param>
        /// <returns>Un objeto Contrato creado a partir de los datos del DataRow</returns>
        public ModuloFuncional DataRowToModuloFuncional(DataRow row)
        {
            ModuloFuncional moduloFuncional = new ModuloFuncional();
            if (row.IsNull("ModuloFuncionalID"))
                moduloFuncional.ModuloFuncionalId = null;
            else
                moduloFuncional.ModuloFuncionalId = (int)Convert.ChangeType(row["ModuloFuncionalID"], typeof(int));
            if (row.IsNull("Clave"))
                moduloFuncional.Clave = null;
            else
                moduloFuncional.Clave = (String)Convert.ChangeType(row["Clave"], typeof(String));
            if (row.IsNull("Nombre"))
                moduloFuncional.Nombre = null;
            else
                moduloFuncional.Nombre = (String)Convert.ChangeType(row["Nombre"], typeof(String));
            if (row.IsNull("Descripcion"))
                moduloFuncional.Descripcion = null;
            else
                moduloFuncional.Descripcion = (String)Convert.ChangeType(row["Descripcion"], typeof(String));
            return moduloFuncional;
            
        }
    }
}
