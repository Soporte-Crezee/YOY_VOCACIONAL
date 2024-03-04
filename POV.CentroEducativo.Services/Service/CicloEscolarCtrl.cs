using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.CentroEducativo.DAO;
using POV.CentroEducativo.BO;
using POV.Localizacion.BO;
using POV.Localizacion.Service;

namespace POV.CentroEducativo.Service
{
    /// <summary>
    /// Controlador del objeto CicloEscolar
    /// </summary>
    public class CicloEscolarCtrl
    {
        /// <summary>
        /// Consulta registros de CicloEscolar en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="cicloEscolar">CicloEscolar que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de CicloEscolar generada por la consulta</returns>
        public DataSet Retrieve(IDataContext dctx, CicloEscolar cicloEscolar)
        {
            CicloEscolarRetHlp da = new CicloEscolarRetHlp();
            DataSet ds = da.Action(dctx, cicloEscolar);
            return ds;
        }

        /// <summary>
        /// Crea un registro de CicloEscolarInsHlp en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="cicloEscolarInsHlp">CicloEscolarInsHlp que desea crear</param>
        public void Insert(IDataContext dctx, CicloEscolar cicloEscolar)
        {
            CicloEscolarInsHlp da = new CicloEscolarInsHlp();
            da.Action(dctx, cicloEscolar);
        }
        /// <summary>
        /// Devuelve un registro completo de un cicloEscolar. Nulo de no encontrarlo
        /// </summary>
        public CicloEscolar RetrieveComplete(IDataContext dctx,CicloEscolar cicloEscolar)
        {
            CicloEscolar ci = null;
            DataSet ds = Retrieve(dctx, cicloEscolar);
            if (ds.Tables[0].Rows.Count == 1)
            {
                ci = this.LastDataRowToCicloEscolar(ds);
                UbicacionCtrl ubicacionCtrl = new UbicacionCtrl();
                ci.UbicacionID = ubicacionCtrl.RetrieveComplete(dctx, new Ubicacion { UbicacionID = ci.UbicacionID.UbicacionID });
            }
            return ci;
        }
        /// <summary>
        /// Actualiza de manera optimista un registro de CicloEscolarUpdHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="cicloEscolarUpdHlp">CicloEscolarUpdHlp que tiene los datos nuevos</param>
        /// <param name="anterior">CicloEscolarUpdHlp que tiene los datos anteriores</param>
        public void Update(IDataContext dctx, CicloEscolar cicloEscolar, CicloEscolar previous)
        {
            CicloEscolarUpdHlp da = new CicloEscolarUpdHlp();
            da.Action(dctx, cicloEscolar, previous);
        }
        
        /// <summary>
        /// Crea un objeto de CicloEscolar a partir de los datos del último DataRow del DataSet.
        /// </summary>
        /// <param name="ds">El DataSet que contiene la información de CicloEscolar</param>
        /// <returns>Un objeto de CicloEscolar creado a partir de los datos</returns>
        public CicloEscolar LastDataRowToCicloEscolar(DataSet ds)
        {
            if (!ds.Tables.Contains("CicloEscolar"))
                throw new Exception("LastDataRowToZona: DataSet no tiene la tabla CicloEscolar");
            int index = ds.Tables["CicloEscolar"].Rows.Count;
            if (index < 1)
                throw new Exception("LastDataRowToCicloEscolar: El DataSet no tiene filas");
            return this.DataRowToCicloEscolar(ds.Tables["CicloEscolar"].Rows[index - 1]);
        }
        /// <summary>
        /// Crea un objeto de Zona a partir de los datos de un DataRow.
        /// </summary>
        /// <param name="row">El DataRow que contiene la información de Zona</param>
        /// <returns>Un objeto de Zona creado a partir de los datos</returns>
        public CicloEscolar DataRowToCicloEscolar(DataRow row)
        {
            CicloEscolar cicloEscolar = new CicloEscolar();
            cicloEscolar.UbicacionID = new Ubicacion();

            if (row.IsNull("CicloEscolarID"))
                cicloEscolar.CicloEscolarID = null;
            else
                cicloEscolar.CicloEscolarID = (int)Convert.ChangeType(row["CicloEscolarID"], typeof(int));
            if (row.IsNull("Titulo"))
                cicloEscolar.Titulo = null;
            else
                cicloEscolar.Titulo = (string)Convert.ChangeType(row["Titulo"], typeof(string));
            if (row.IsNull("Descripcion"))
                cicloEscolar.Descripcion = null;
            else
                cicloEscolar.Descripcion = (string)Convert.ChangeType(row["Descripcion"], typeof(string));
            if (row.IsNull("InicioCiclo"))
                cicloEscolar.InicioCiclo = null;
            else
                cicloEscolar.InicioCiclo = (DateTime)Convert.ChangeType(row["InicioCiclo"], typeof(DateTime));
            if (row.IsNull("FinCiclo"))
                cicloEscolar.FinCiclo = null;
            else
                cicloEscolar.FinCiclo = (DateTime)Convert.ChangeType(row["FinCiclo"], typeof(DateTime));
            if (row.IsNull("UbicacionID"))
                cicloEscolar.UbicacionID.UbicacionID = null;
            else
                cicloEscolar.UbicacionID.UbicacionID = (long)Convert.ChangeType(row["UbicacionID"], typeof(long));

            if (row.IsNull("Activo"))
                cicloEscolar.Activo = null;
            else
                cicloEscolar.Activo = (bool)Convert.ChangeType(row["Activo"], typeof(bool));
            return cicloEscolar;
        }
    }
}
