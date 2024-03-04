using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Comun.BO;
using POV.Comun.DAO;

namespace POV.Comun.Service
{
    /// <summary>
    /// Servicios para acceder a Usuarios
    /// </summary>
    public class AdjuntoImagenCtrl
    {
        /// <summary>
        /// Consulta registros de adjuntoImagen en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="adjuntoImagen">adjuntoImagen que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de adjuntoImagen generada por la consulta</returns>
        public DataSet Retrieve(IDataContext dctx, AdjuntoImagen adjuntoImagen)
        {
            AdjuntoImagenRetHlp da = new AdjuntoImagenRetHlp();
            DataSet ds = da.Action(dctx, adjuntoImagen);
            return ds;
        }
        /// <summary>
        /// Crea un registro de adjuntoImagen en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="adjuntoImagen">adjuntoImagen que desea crear</param>
        public void Insert(IDataContext dctx, AdjuntoImagen adjuntoImagen)
        {
            AdjuntoImagenInsHlp da = new AdjuntoImagenInsHlp();
            da.Action(dctx, adjuntoImagen);
        }
        /// <summary>
        /// Actualiza de manera optimista un registro de adjuntoImagen en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="adjuntoImagen">adjuntoImagen que tiene los datos nuevos</param>
        /// <param name="anterior">adjuntoImagen que tiene los datos anteriores</param>
        public void Update(IDataContext dctx, AdjuntoImagen adjuntoImagen, AdjuntoImagen previous)
        {
            AdjuntoImagenUpdHlp da = new AdjuntoImagenUpdHlp();
            da.Action(dctx, adjuntoImagen, previous);
        }
        /// <summary>
        /// Elimina un registro de adjuntoImagen en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="adjuntoImagen">adjuntoImagen que desea eliminar</param>
        public void Delete(IDataContext dctx, AdjuntoImagen adjuntoImagen)
        {
            AdjuntoImagenDelHlp da = new AdjuntoImagenDelHlp();
            da.Action(dctx, adjuntoImagen);
        }
        /// <summary>
        /// Crea un objeto de adjuntoImagen a partir de los datos del último DataRow del DataSet.
        /// </summary>
        /// <param name="ds">El DataSet que contiene la información de adjuntoImagen</param>
        /// <returns>Un objeto de adjuntoImagen creado a partir de los datos</returns>
        public AdjuntoImagen LastDataRowToAdjuntoImagen(DataSet ds)
        {
            if (!ds.Tables.Contains("AdjuntoImagen"))
                throw new Exception("LastDataRowToAdjuntoImagen: DataSet no tiene la tabla AdjuntoImagen");
            int index = ds.Tables["AdjuntoImagen"].Rows.Count;
            if (index < 1)
                throw new Exception("LastDataRowToAdjuntoImagen: El DataSet no tiene filas");
            return this.DataRowToAdjuntoImagen(ds.Tables["AdjuntoImagen"].Rows[index - 1]);
        }
        /// <summary>
        /// Crea un objeto de adjuntoImagen a partir de los datos de un DataRow.
        /// </summary>
        /// <param name="row">El DataRow que contiene la información de adjuntoImagen</param>
        /// <returns>Un objeto de adjuntoImagen creado a partir de los datos</returns>
        public AdjuntoImagen DataRowToAdjuntoImagen(DataRow row)
        {
            AdjuntoImagen adjuntoImagen = new AdjuntoImagen();
            if (row.IsNull("AdjuntoImagenID"))
                adjuntoImagen.AdjuntoImagenID = null;
            else
                adjuntoImagen.AdjuntoImagenID = (long)Convert.ChangeType(row["AdjuntoImagenID"], typeof(long));
            if (row.IsNull("Extension"))
                adjuntoImagen.Extension = null;
            else
                adjuntoImagen.Extension = (string)Convert.ChangeType(row["Extension"], typeof(string));
            if (row.IsNull("MIME"))
                adjuntoImagen.MIME = null;
            else
                adjuntoImagen.MIME = (string)Convert.ChangeType(row["MIME"], typeof(string));
            if (row.IsNull("NombreImagen"))
                adjuntoImagen.NombreImagen = null;
            else
                adjuntoImagen.NombreImagen = (string)Convert.ChangeType(row["NombreImagen"], typeof(string));
            if (row.IsNull("NombreThumb"))
                adjuntoImagen.NombreThumb = null;
            else
                adjuntoImagen.NombreThumb = (string)Convert.ChangeType(row["NombreThumb"], typeof(string));
            if (row.IsNull("CarpetaID"))
                adjuntoImagen.CarpetaID = null;
            else
                adjuntoImagen.CarpetaID = (ECarpetaSistema)Convert.ChangeType(row["CarpetaID"], typeof(int));
            if (row.IsNull("Carpeta"))
                adjuntoImagen.FolderUrl = null;
            else
                adjuntoImagen.FolderUrl = (string)Convert.ChangeType(row["Carpeta"], typeof(string));
            return adjuntoImagen;
        }
        public AdjuntoImagen LastDataRowToAdjuntoImagenFromImagenPerfil(DataSet ds)
        {
            if (!ds.Tables.Contains("ImagenPerfil"))
                throw new Exception("LastDataRowToAdjuntoImagen: DataSet no tiene la tabla AdjuntoImagen");
            int index = ds.Tables["ImagenPerfil"].Rows.Count;
            if (index < 1)
                throw new Exception("LastDataRowToAdjuntoImagenFromImagenPerfil: El DataSet no tiene filas");
            return this.DataRowToAdjuntoImagen(ds.Tables["ImagenPerfil"].Rows[index - 1]);
        }
    }
}
