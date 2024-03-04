using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.ContenidosDigital.BO;
using POV.ContenidosDigital.DAO;

namespace POV.ContenidosDigital.Service
{
    /// <summary>
    /// Controlador del objeto TipoDocumento
    /// </summary>
    public class TipoDocumentoCtrl
    {
        /// <summary>
        /// Consulta registros de TipoDocumentoRetHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="tipoDocumentoRetHlp">TipoDocumentoRetHlp que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de TipoDocumentoRetHlp generada por la consulta</returns>
        public DataSet Retrieve(IDataContext dctx, TipoDocumento tipoDocumento)
        {
            TipoDocumentoRetHlp da = new TipoDocumentoRetHlp();
            DataSet ds = da.Action(dctx, tipoDocumento);
            return ds;
        }
        /// <summary>
        /// Crea un registro de TipoDocumentoInsHlp en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="tipoDocumentoInsHlp">TipoDocumentoInsHlp que desea crear</param>
        public void Insert(IDataContext dctx, TipoDocumento tipoDocumento)
        {
            if (ValidateDataForInsertOrUpdate(dctx, tipoDocumento))
            {
                TipoDocumentoInsHlp da = new TipoDocumentoInsHlp();
                tipoDocumento.FechaRegistro = DateTime.Now;
                tipoDocumento.Activo = true;
                da.Action(dctx, tipoDocumento);
              
            }
            else
            {
                throw new DuplicateNameException("El nombre del Tipo de Documento ya se encuentra registrado en el sistema, por favor verifique.");

            }

        }
        /// <summary>
        /// Actualiza de manera optimista un registro de TipoDocumentoUpdHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="tipoDocumentoUpdHlp">TipoDocumentoUpdHlp que tiene los datos nuevos</param>
        /// <param name="anterior">TipoDocumentoUpdHlp que tiene los datos anteriores</param>
        public void Update(IDataContext dctx, TipoDocumento tipoDocumento, TipoDocumento previous)
        {
            TipoDocumentoUpdHlp da = new TipoDocumentoUpdHlp();
            if (tipoDocumento.Nombre != previous.Nombre)
            {
                if (ValidateDataForInsertOrUpdate(dctx, tipoDocumento))
                    da.Action(dctx, tipoDocumento, previous);
                else
                    throw new DuplicateNameException("El nombre del Tipo de Documento ya se encuentra registrado en el sistema, por favor verifique.");
            }
            else
            {
                da.Action(dctx, tipoDocumento, previous);
            }
        }

        /// <summary>
        /// Elimina un registro de TipoDocumentoDelHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="tipoDocumentoDelHlp">TipoDocumentoDelHlp que desea eliminar</param>
        public void Delete(IDataContext dctx, TipoDocumento tipoDocumento)
        {
            TipoDocumentoDelHlp da = new TipoDocumentoDelHlp();
            da.Action(dctx, tipoDocumento);
        }
        /// <summary>
        /// Crea un objeto de TipoDocumento a partir de los datos del último DataRow del DataSet.
        /// </summary>
        /// <param name="ds">El DataSet que contiene la información de TipoDocumento</param>
        /// <returns>Un objeto de TipoDocumento creado a partir de los datos</returns>
        public TipoDocumento LastDataRowToTipoDocumento(DataSet ds)
        {
            if (!ds.Tables.Contains("TipoDocumento"))
                throw new Exception("LastDataRowToTipoDocumento: DataSet no tiene la tabla TipoDocumento");
            int index = ds.Tables["TipoDocumento"].Rows.Count;
            if (index < 1)
                throw new Exception("LastDataRowToTipoDocumento: El DataSet no tiene filas");
            return this.DataRowToTipoDocumento(ds.Tables["TipoDocumento"].Rows[index - 1]);
        }
        /// <summary>
        /// Crea un objeto de TipoDocumento a partir de los datos de un DataRow.
        /// </summary>
        /// <param name="row">El DataRow que contiene la información de TipoDocumento</param>
        /// <returns>Un objeto de TipoDocumento creado a partir de los datos</returns>
        public TipoDocumento DataRowToTipoDocumento(DataRow row)
        {
            TipoDocumento tipoDocumento = new TipoDocumento();
            if (row.IsNull("TipoDocumentoID"))
                tipoDocumento.TipoDocumentoID = null;
            else
                tipoDocumento.TipoDocumentoID = (int)Convert.ChangeType(row["TipoDocumentoID"], typeof(int));
            if (row.IsNull("Nombre"))
                tipoDocumento.Nombre = null;
            else
                tipoDocumento.Nombre = (string)Convert.ChangeType(row["Nombre"], typeof(string));
            if (row.IsNull("ImagenDocumento"))
                tipoDocumento.ImagenDocumento = null;
            else
                tipoDocumento.ImagenDocumento = (string)Convert.ChangeType(row["ImagenDocumento"], typeof(string));
            if (row.IsNull("Extension"))
                tipoDocumento.Extension = null;
            else
                tipoDocumento.Extension = (string)Convert.ChangeType(row["Extension"], typeof(string));
            if (row.IsNull("MIME"))
                tipoDocumento.MIME = null;
            else
                tipoDocumento.MIME = (string)Convert.ChangeType(row["MIME"], typeof(string));
            if (row.IsNull("EsEditable"))
                tipoDocumento.EsEditable = null;
            else
                tipoDocumento.EsEditable = (Boolean)Convert.ChangeType(row["EsEditable"], typeof(Boolean));
            if (row.IsNull("Fuente"))
                tipoDocumento.Fuente = null;
            else
                tipoDocumento.Fuente = (string)Convert.ChangeType(row["Fuente"], typeof(string));
            if (row.IsNull("Activo"))
                tipoDocumento.Activo = null;
            else
                tipoDocumento.Activo = (Boolean)Convert.ChangeType(row["Activo"], typeof(Boolean));
            if (row.IsNull("FechaRegistro"))
                tipoDocumento.FechaRegistro = null;
            else
                tipoDocumento.FechaRegistro = (DateTime)Convert.ChangeType(row["FechaRegistro"], typeof(DateTime));
            return tipoDocumento;
        }

        /// <summary>
        /// Valida si existe un tipo de documento con el mismo nombre en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="tipoDocumento">Tipo de documento</param>
        /// <returns></returns>
        private bool ValidateDataForInsertOrUpdate(IDataContext dctx, TipoDocumento tipoDocumento)
        {
            DataSet ds = Retrieve(dctx, new TipoDocumento { Nombre = tipoDocumento.Nombre });
            return ds.Tables["TipoDocumento"].Rows.Count <= 0;
        }
    }
}
