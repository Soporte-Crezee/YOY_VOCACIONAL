using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.ContenidosDigital.BO;
using POV.ContenidosDigital.DAO;
using POV.ContenidosDigital.DA;
namespace POV.ContenidosDigital.Service
{
    /// <summary>
    /// Controlador del objeto AVisor
    /// </summary>
    public class VisorCtrl
    {
        /// <summary>
        /// Consulta registros de VisorRetHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="visorRetHlp">VisorRetHlp que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de VisorRetHlp generada por la consulta</returns>
        public DataSet Retrieve(IDataContext dctx, AVisor visor)
        {
            VisorRetHlp da = new VisorRetHlp();
            DataSet ds = da.Action(dctx, visor);
            return ds;
        }

        /// <summary>
        /// Devuelve la lista de visores registradas en el sistema
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <returns>Lista de visores registradas en el sistema</returns>
        public List<AVisor> RetrieveAllVisores(IDataContext dctx)
        {
            List<AVisor> visores = new List<AVisor>();

            DataSet dsVisorInt = Retrieve(dctx, new VisorInterno { Activo = true });
            foreach (DataRow dr in dsVisorInt.Tables[0].Rows)
            {
                visores.Add(RetrieveComplete(dctx,DataRowToAVisor(dr)));
            }

            DataSet dsVisorExt = Retrieve(dctx, new VisorExterno { Activo = true });
            foreach (DataRow dr in dsVisorExt.Tables[0].Rows)
            {
                visores.Add(RetrieveComplete(dctx, DataRowToAVisor(dr)));
            }
            return visores;
        }

        /// <summary>
        /// Devuelve los datos de un visor completo a partir de los parametros de busqueda
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="visor"></param>
        /// <returns></returns>
        public AVisor RetrieveComplete(IDataContext dctx, AVisor visor)
        {

            if (visor == null) new ArgumentNullException("VisorCtrl: El visor no puede ser nulo.");
            AVisor visorComplete = null;

            DataSet dsVisor = Retrieve(dctx, visor);
            if (dsVisor.Tables[0].Rows.Count > 0)
            {
                visorComplete = LastDataRowToAVisor(dsVisor);
                visorComplete.ListaTiposDocumento = RetrieveListTipoDocumento(dctx, visorComplete);
               
            }

            return visorComplete;
        }

        /// <summary>
        /// Devuelve una lista de tipos de documento a partir de un visor
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="visor"></param>
        /// <returns></returns>
        private List<TipoDocumento> RetrieveListTipoDocumento(IDataContext dctx, AVisor visor)
        {
            List<TipoDocumento> tipos = new List<TipoDocumento>();
            TipoDocumentoCtrl tipoDocumentoCtrl = new TipoDocumentoCtrl();
            VisorTipoDocumentoRetHlp da = new VisorTipoDocumentoRetHlp();
            DataSet dsTipos = da.Action(dctx, visor.VisorID, null);
            foreach (DataRow dr in dsTipos.Tables[0].Rows)
            {
                TipoDocumento tipoDocumento = new TipoDocumento();
                tipoDocumento.TipoDocumentoID = (int)Convert.ChangeType(dr["TipoDocumentoID"], typeof(int));
                tipoDocumento = tipoDocumentoCtrl.LastDataRowToTipoDocumento(tipoDocumentoCtrl.Retrieve(dctx, tipoDocumento));
                tipos.Add(tipoDocumento);
            }

            return tipos;
        }
        /// <summary>
        /// Verifica si existe un visor para un determinado tipo de documento
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="tipoDocumento">Tipo de documento</param>
        /// <returns>true si existe un visor para el tipo de documento, false en caso contrario</returns>
        public bool ExisteVisorTipoDocumento(IDataContext dctx, TipoDocumento tipoDocumento)
        {
            if (tipoDocumento == null) new ArgumentNullException("VisorCtrl: El tipoDocumento no puede ser nulo.");
            if (tipoDocumento.TipoDocumentoID == null) new ArgumentNullException("VisorCtrl: El identificador de tipoDocumento no puede ser nulo.");
            VisorTipoDocumentoRetHlp da = new VisorTipoDocumentoRetHlp();
            DataSet dsTipos = da.Action(dctx, null, tipoDocumento.TipoDocumentoID);

            return dsTipos.Tables[0].Rows.Count > 0;
        }
        /// <summary>
        /// Crea un objeto de AVisor a partir de los datos del último DataRow del DataSet.
        /// </summary>
        /// <param name="ds">El DataSet que contiene la información de AVisor</param>
        /// <returns>Un objeto de AVisor creado a partir de los datos</returns>
        public AVisor LastDataRowToAVisor(DataSet ds)
        {
            if (!ds.Tables.Contains("Visor"))
                throw new Exception("LastDataRowToAVisor: DataSet no tiene la tabla AVisor");
            int index = ds.Tables["Visor"].Rows.Count;
            if (index < 1)
                throw new Exception("LastDataRowToAVisor: El DataSet no tiene filas");
            return this.DataRowToAVisor(ds.Tables["Visor"].Rows[index - 1]);
        }
        /// <summary>
        /// Crea un objeto de AVisor a partir de los datos de un DataRow.
        /// </summary>
        /// <param name="row">El DataRow que contiene la información de AVisor</param>
        /// <returns>Un objeto de AVisor creado a partir de los datos</returns>
        public AVisor DataRowToAVisor(DataRow row)
        {
            AVisor aVisor = null;
            if ((bool)Convert.ChangeType(row["EsInterno"], typeof(bool)))
            {
                aVisor = new VisorInterno();
                if (row.IsNull("Extension"))
                    (aVisor as VisorInterno).Extension = null;
                else
                    (aVisor as VisorInterno).Extension = (string)Convert.ChangeType(row["Extension"], typeof(string));
            }
            else
            {
                aVisor = new VisorExterno();
                if (row.IsNull("Fuente"))
                    (aVisor as VisorExterno).Fuente = null;
                else
                    (aVisor as VisorExterno).Fuente = (string)Convert.ChangeType(row["Fuente"], typeof(string));
            }
            aVisor.ListaTiposDocumento = new List<TipoDocumento>();
            if (row.IsNull("VisorID"))
                aVisor.VisorID = null;
            else
                aVisor.VisorID = (int)Convert.ChangeType(row["VisorID"], typeof(int));
            if (row.IsNull("Clave"))
                aVisor.Clave = null;
            else
                aVisor.Clave = (string)Convert.ChangeType(row["Clave"], typeof(string));
            if (row.IsNull("FechaRegistro"))
                aVisor.FechaRegistro = null;
            else
                aVisor.FechaRegistro = (DateTime)Convert.ChangeType(row["FechaRegistro"], typeof(DateTime));
            if (row.IsNull("Activo"))
                aVisor.Activo = null;
            else
                aVisor.Activo = (bool)Convert.ChangeType(row["Activo"], typeof(bool));



            return aVisor;
        }
    }
}
