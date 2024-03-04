using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.ContenidosDigital.BO;
using POV.ContenidosDigital.DAO;
using POV.Comun.BO;

namespace POV.ContenidosDigital.Service
{
    /// <summary>
    /// Controlador del objeto ContenidoDigital
    /// </summary>
    public class ContenidoDigitalCtrl
    {
        /// <summary>
        /// Consulta registros de ContenidoDigital en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="contenidoDigital">ContenidoDigital que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de ContenidoDigital generada por la consulta</returns>
        public DataSet Retrieve(IDataContext dctx, ContenidoDigital contenidoDigital)
        {
            ContenidoDigitalRetHlp da = new ContenidoDigitalRetHlp();
            DataSet ds = da.Action(dctx, contenidoDigital);
            return ds;
        }
        /// <summary>
        /// Inserta un registro de contenido digital en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="contenidoDigital"></param>
        private void Insert(IDataContext dctx, ContenidoDigital contenidoDigital)
        {
            if (!IsValidContenidoDigital(dctx, contenidoDigital))
                throw new Exception("ContenidoDigitalCtrl: La clave del contenido digital ya está registrada");
            ContenidoDigitalInsHlp da = new ContenidoDigitalInsHlp();
            da.Action(dctx, contenidoDigital);
        }
        /// <summary>
        /// Inserta un registro completo de contenido digital en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="contenidoDigital">Contenido digital que se desea registrar</param>
        public void InsertComplete(IDataContext dctx, ContenidoDigital contenidoDigital)
        {
            #region *** validaciones ***
            if (contenidoDigital == null) new ArgumentNullException("ContenidoDigitalCtrl: El contenido digital no puede ser nulo");
            if (contenidoDigital.TipoDocumento == null) new ArgumentNullException("ContenidoDigitalCtrl: El tipo de documento del contenido digital no puede ser nulo");
            if (contenidoDigital.TipoDocumento.TipoDocumentoID == null) new ArgumentNullException("ContenidoDigitalCtrl: El identificador tipo de documento del contenido digital no puede ser nulo");
            if (contenidoDigital.InstitucionOrigen == null) new ArgumentNullException("ContenidoDigitalCtrl: La institucion de origen del contenido digital no puede ser nulo");
            if (contenidoDigital.ListaURLContenido.FirstOrDefault(item => item.EsPredeterminada.Value) == null) new ArgumentNullException("ContenidoDigitalCtrl: Debe existir una url predeterminada en el contenido digital");
            #endregion

            object myFirm = new object();
            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);
            try
            {
                //insert simple
                Insert(dctx, contenidoDigital);
                //recuperacion del id
                contenidoDigital.ContenidoDigitalID = LastDataRowToContenidoDigital(Retrieve(dctx, contenidoDigital)).ContenidoDigitalID;
                //insertar las urls del contenido
                foreach (URLContenido urlContenido in contenidoDigital.ListaURLContenido)
                {
                    Insert(dctx, contenidoDigital, urlContenido);
                }

                dctx.CommitTransaction(myFirm);
            }
            catch (Exception ex)
            {
                dctx.RollbackTransaction(myFirm);
                dctx.CloseConnection(myFirm);
                throw ex;
            }
            finally
            {
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(myFirm);
            }
        }
        /// <summary>
        /// Actualiza un registro de contenido digital en la base de datos
        /// </summary>
        /// <param name="dctx"></param>
        /// <param name="contenidoDigital"></param>
        /// <param name="anterior"></param>
        private void Update(IDataContext dctx, ContenidoDigital contenidoDigital, ContenidoDigital anterior)
        {
            if (!IsValidContenidoDigital(dctx, contenidoDigital))
                throw new Exception("ContenidoDigitalCtrl: La clave del contenido digital ya está registrada");
            ContenidoDigitalUpdHlp da = new ContenidoDigitalUpdHlp();
            da.Action(dctx, contenidoDigital, anterior);
        }

        public void UpdateComplete(IDataContext dctx, ContenidoDigital contenidoDigital, ContenidoDigital anterior)
        {
            #region *** validaciones ***
            if (contenidoDigital == null) new ArgumentNullException("ContenidoDigitalCtrl: El contenido digital no puede ser nulo");
            if (anterior == null) new ArgumentNullException("ContenidoDigitalCtrl: El contenido digital anterior no puede ser nulo");
            if (anterior.ContenidoDigitalID == null) new ArgumentNullException("ContenidoDigitalCtrl: El identificador del contenido digital anterior no puede ser nulo");
            if (contenidoDigital.TipoDocumento == null) new ArgumentNullException("ContenidoDigitalCtrl: El tipo de documento del contenido digital no puede ser nulo");
            if (contenidoDigital.TipoDocumento.TipoDocumentoID == null) new ArgumentNullException("ContenidoDigitalCtrl: El identificador tipo de documento del contenido digital no puede ser nulo");
            if (contenidoDigital.InstitucionOrigen == null) new ArgumentNullException("ContenidoDigitalCtrl: La institucion de origen del contenido digital no puede ser nulo");
            if (contenidoDigital.ListaURLContenido.FirstOrDefault(item => item.EsPredeterminada.Value) == null) new ArgumentNullException("ContenidoDigitalCtrl: Debe existir una url predeterminada en el contenido digital");
            #endregion

            object myFirm = new object();
            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);
            try
            {
                Update(dctx, contenidoDigital, anterior);


                foreach (URLContenido contenido in contenidoDigital.ListaURLContenido)
                {
                    if (contenidoDigital.URLContenidoEstado(contenido) == EObjetoEstado.NUEVO)
                    {
                        Insert(dctx, contenidoDigital, contenido);
                    } 
                    else if (contenidoDigital.URLContenidoEstado(contenido) == EObjetoEstado.EDITADO)
                    {
                        Update(dctx, contenido, new URLContenido { URLContenidoID = contenido.URLContenidoID });
                    }
                    else if (contenidoDigital.URLContenidoEstado(contenido) == EObjetoEstado.ELIMINADO)
                    {
                        
                    }
                }

                dctx.CommitTransaction(myFirm);
            }
            catch (Exception ex)
            {
                dctx.RollbackTransaction(myFirm);
                dctx.CloseConnection(myFirm);
                throw ex;
            }
            finally
            {
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(myFirm);
            }
        }
        /// <summary>
        /// Valida si el contenido digital puede ser insertado o actualizado
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="contenidoDigital"></param>
        /// <returns>true si es valido, false en caso contrario</returns>
        private bool IsValidContenidoDigital(IDataContext dctx, ContenidoDigital contenidoDigital)
        {
            DataSet dsContenido = Retrieve(dctx, new ContenidoDigital { Clave = contenidoDigital.Clave });
            if (dsContenido.Tables[0].Rows.Count > 0)
            {
                if (contenidoDigital.ContenidoDigitalID != null)
                {
                    ContenidoDigital contenidoAux = LastDataRowToContenidoDigital(dsContenido);
                    return contenidoAux.EstatusContenido != EEstatusContenido.INACTIVO && contenidoAux.ContenidoDigitalID == contenidoDigital.ContenidoDigitalID;
                }
                else
                    return false;
            }
            return true;
        }
        /// <summary>
        /// Consulta un registro de contenido digital completo de la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="contenidoDigital">ContenidoDigital que provee el criterio de selección para realizar la consulta</param>
        /// <returns>Contenido digital o null en caso de no encontrarse</returns>
        public ContenidoDigital RetrieveComplete(IDataContext dctx, ContenidoDigital contenidoDigital)
        {
            if (contenidoDigital == null) throw new ArgumentNullException("ContenidoDigitalCtrl: El contenido digital no puede ser nulo");

            ContenidoDigital contenidoComplete = null;
            //recuperacion de la info. de contenido digital
            DataSet ds = Retrieve(dctx, contenidoDigital);

            if (ds.Tables[0].Rows.Count > 0)
            {
                int index = ds.Tables[0].Rows.Count;
                DataRow drContenido = ds.Tables[0].Rows[index - 1];
                contenidoComplete = DataRowToContenidoDigital(drContenido);

                //Recuperacion de la lista de urls
                List<URLContenido> urls = RetrieveListURLContenido(dctx, contenidoComplete);

                contenidoComplete = DataRowToContenidoDigital(drContenido, urls);
                //Recuperacion de la info. de tipo de documento
                TipoDocumentoCtrl tipoDocumentoCtrl = new TipoDocumentoCtrl();
                contenidoComplete.TipoDocumento = tipoDocumentoCtrl.LastDataRowToTipoDocumento(tipoDocumentoCtrl.Retrieve(dctx, contenidoComplete.TipoDocumento));
            
            }

            return contenidoComplete;
        }
        /// <summary>
        /// Consulta registros de URLContenido en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="uRLContenido">URLContenido que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de URLContenido generada por la consulta</returns>
        private DataSet Retrieve(IDataContext dctx, ContenidoDigital contenidoDigital, URLContenido uRlContenido)
        {
            URLContenidoRetHlp da = new URLContenidoRetHlp();
            DataSet ds = da.Action(dctx, contenidoDigital, uRlContenido);
            return ds;
        }
        /// <summary>
        /// Inserta un registro de urlcontenido en la base de datos
        /// </summary>
        /// <param name="dctx"></param>
        /// <param name="contenidoDigital"></param>
        /// <param name="urlContenido"></param>
        private void Insert(IDataContext dctx, ContenidoDigital contenidoDigital, URLContenido urlContenido)
        {
            URLContenidoInsHlp da = new URLContenidoInsHlp();
            da.Action(dctx, contenidoDigital, urlContenido);
        }
        /// <summary>
        /// Actualiza un registro de url contenido en la base de datos
        /// </summary>
        /// <param name="dctx"></param>
        /// <param name="urlContenido"></param>
        /// <param name="anterior"></param>
        private void Update(IDataContext dctx, URLContenido urlContenido, URLContenido anterior)
        {
            URLContenidoUpdHlp da = new URLContenidoUpdHlp();
            da.Action(dctx, urlContenido, anterior);
        }
        /// <summary>
        /// Realiza una baja lógica de un contenido digital.
        /// </summary>
        /// <param name="dctx">DataContext que proveerá los datos de acceso en la base de datos.</param>
        /// <param name="contenidoDigital">Contenido digital que será dado de baja.</param>
        /// <param name="anterior">Contenido digital original.</param>
        public void DeleteComplete(IDataContext dctx, ContenidoDigital contenidoDigital, ContenidoDigital anterior)
        {
            #region *** validaciones ***
            if (contenidoDigital == null) new ArgumentNullException("ContenidoDigitalCtrl: El contenido digital no puede ser nulo");
            if (anterior == null) new ArgumentNullException("ContenidoDigitalCtrl: El contenido digital anterior no puede ser nulo");
            if (anterior.ContenidoDigitalID == null) new ArgumentNullException("ContenidoDigitalCtrl: El identificador del contenido digital anterior no puede ser nulo");
            if (contenidoDigital.TipoDocumento == null) new ArgumentNullException("ContenidoDigitalCtrl: El tipo de documento del contenido digital no puede ser nulo");
            if (contenidoDigital.TipoDocumento.TipoDocumentoID == null) new ArgumentNullException("ContenidoDigitalCtrl: El identificador tipo de documento del contenido digital no puede ser nulo");
            if (contenidoDigital.InstitucionOrigen == null) new ArgumentNullException("ContenidoDigitalCtrl: La institucion de origen del contenido digital no puede ser nulo");
            if (contenidoDigital.ListaURLContenido.FirstOrDefault(item => item.EsPredeterminada.Value) == null) new ArgumentNullException("ContenidoDigitalCtrl: Debe existir una url predeterminada en el contenido digital");
            #endregion
            ContenidoDigitalDelHlp da = new ContenidoDigitalDelHlp();

            object myFirm = new object();
            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);
            try
            {
                da.Action(dctx, contenidoDigital, anterior);


                foreach (URLContenido contenido in contenidoDigital.ListaURLContenido)
                {
                    if (contenidoDigital.URLContenidoEstado(contenido) == EObjetoEstado.NUEVO)
                    {
                        Insert(dctx, contenidoDigital, contenido);
                    }
                    else if (contenidoDigital.URLContenidoEstado(contenido) == EObjetoEstado.EDITADO)
                    {
                        Update(dctx, contenido, new URLContenido { URLContenidoID = contenido.URLContenidoID });
                    }
                    else if (contenidoDigital.URLContenidoEstado(contenido) == EObjetoEstado.ELIMINADO)
                    {

                    }
                }

                dctx.CommitTransaction(myFirm);
            }
            catch (Exception ex)
            {
                dctx.RollbackTransaction(myFirm);
                dctx.CloseConnection(myFirm);
                throw ex;
            }
            finally
            {
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(myFirm);
            }
        }
        /// <summary>
        /// Recupera una lista de urls de un contenido digital
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="contenidoDigital">Contenido digital</param>
        /// <returns>Lista de url contenido</returns>
        private List<URLContenido> RetrieveListURLContenido(IDataContext dctx, ContenidoDigital contenidoDigital)
        {
            if (contenidoDigital == null) throw new ArgumentNullException("ContenidoDigitalCtrl: El contenido digital no puede ser nulo");
            if (contenidoDigital.ContenidoDigitalID == null) throw new ArgumentNullException("ContenidoDigitalCtrl: El ContenidoDigitalID no puede ser nulo");
            List<URLContenido> urlsContenido = new List<URLContenido>();

            DataSet ds = Retrieve(dctx, contenidoDigital, new URLContenido { Activo = true });
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    urlsContenido.Add(DataRowToURLContenido(dr));
                }
            }

            return urlsContenido;
        }
        /// <summary>
        /// Crea un objeto de ContenidoDigital a partir de los datos del último DataRow del DataSet.
        /// </summary>
        /// <param name="ds">El DataSet que contiene la información de ContenidoDigital</param>
        /// <returns>Un objeto de ContenidoDigital creado a partir de los datos</returns>
        public ContenidoDigital LastDataRowToContenidoDigital(DataSet ds)
        {
            if (!ds.Tables.Contains("ContenidoDigital"))
                throw new Exception("LastDataRowToContenidoDigital: DataSet no tiene la tabla ContenidoDigital");
            int index = ds.Tables["ContenidoDigital"].Rows.Count;
            if (index < 1)
                throw new Exception("LastDataRowToContenidoDigital: El DataSet no tiene filas");
            return this.DataRowToContenidoDigital(ds.Tables["ContenidoDigital"].Rows[index - 1]);
        }
        /// <summary>
        /// Crea un objeto de ContenidoDigital a partir de los datos de un DataRow.
        /// </summary>
        /// <param name="row">El DataRow que contiene la información de ContenidoDigital</param>
        /// <returns>Un objeto de ContenidoDigital creado a partir de los datos</returns>
        public ContenidoDigital DataRowToContenidoDigital(DataRow row)
        {
            ContenidoDigital contenidoDigital = DataRowToContenidoDigital(row, new List<URLContenido>());
            return contenidoDigital;
        }
        private ContenidoDigital DataRowToContenidoDigital(DataRow row, List<URLContenido> urls)
        {
            ContenidoDigital contenidoDigital = new ContenidoDigital(urls);
            contenidoDigital.TipoDocumento = new TipoDocumento();
            contenidoDigital.InstitucionOrigen = new InstitucionOrigen();
            if (row.IsNull("ContenidoDigitalID"))
                contenidoDigital.ContenidoDigitalID = null;
            else
                contenidoDigital.ContenidoDigitalID = (long)Convert.ChangeType(row["ContenidoDigitalID"], typeof(long));
            if (row.IsNull("TipoDocumentoID"))
                contenidoDigital.TipoDocumento.TipoDocumentoID = null;
            else
                contenidoDigital.TipoDocumento.TipoDocumentoID = (int)Convert.ChangeType(row["TipoDocumentoID"], typeof(int));
            if (row.IsNull("Clave"))
                contenidoDigital.Clave = null;
            else
                contenidoDigital.Clave = (string)Convert.ChangeType(row["Clave"], typeof(string));
            if (row.IsNull("Nombre"))
                contenidoDigital.Nombre = null;
            else
                contenidoDigital.Nombre = (string)Convert.ChangeType(row["Nombre"], typeof(string));
            if (row.IsNull("EsInterno"))
                contenidoDigital.EsInterno = null;
            else
                contenidoDigital.EsInterno = (bool)Convert.ChangeType(row["EsInterno"], typeof(bool));
            if (row.IsNull("EsDescargable"))
                contenidoDigital.EsDescargable = null;
            else
                contenidoDigital.EsDescargable = (bool)Convert.ChangeType(row["EsDescargable"], typeof(bool));
            if (row.IsNull("InstitucionOrigen"))
                contenidoDigital.InstitucionOrigen.Nombre = null;
            else
                contenidoDigital.InstitucionOrigen.Nombre = (string)Convert.ChangeType(row["InstitucionOrigen"], typeof(string));
            if (row.IsNull("Tags"))
                contenidoDigital.Tags = null;
            else
                contenidoDigital.Tags = (string)Convert.ChangeType(row["Tags"], typeof(string));
            if (row.IsNull("FechaRegistro"))
                contenidoDigital.FechaRegistro = null;
            else
                contenidoDigital.FechaRegistro = (DateTime)Convert.ChangeType(row["FechaRegistro"], typeof(DateTime));
            if (row.IsNull("EstatusContenido"))
                contenidoDigital.EstatusContenido = null;
            else
                contenidoDigital.EstatusContenido = (EEstatusContenido)Convert.ChangeType(row["EstatusContenido"], typeof(byte));
            return contenidoDigital;
        }
        /// <summary>
        /// Crea un objeto de URLContenido a partir de los datos del último DataRow del DataSet.
        /// </summary>
        /// <param name="ds">El DataSet que contiene la información de URLContenido</param>
        /// <returns>Un objeto de URLContenido creado a partir de los datos</returns>
        private URLContenido LastDataRowToURLContenido(DataSet ds)
        {
            if (!ds.Tables.Contains("URLContenido"))
                throw new Exception("LastDataRowToURLContenido: DataSet no tiene la tabla URLContenido");
            int index = ds.Tables["URLContenido"].Rows.Count;
            if (index < 1)
                throw new Exception("LastDataRowToURLContenido: El DataSet no tiene filas");
            return this.DataRowToURLContenido(ds.Tables["URLContenido"].Rows[index - 1]);
        }
        /// <summary>
        /// Crea un objeto de URLContenido a partir de los datos de un DataRow.
        /// </summary>
        /// <param name="row">El DataRow que contiene la información de URLContenido</param>
        /// <returns>Un objeto de URLContenido creado a partir de los datos</returns>
        private URLContenido DataRowToURLContenido(DataRow row)
        {
            URLContenido uRLContenido = new URLContenido();
            if (row.IsNull("URLContenidoID"))
                uRLContenido.URLContenidoID = null;
            else
                uRLContenido.URLContenidoID = (long)Convert.ChangeType(row["URLContenidoID"], typeof(long));
            if (row.IsNull("URL"))
                uRLContenido.URL = null;
            else
                uRLContenido.URL = (string)Convert.ChangeType(row["URL"], typeof(string));
            if (row.IsNull("EsPredeterminada"))
                uRLContenido.EsPredeterminada = null;
            else
                uRLContenido.EsPredeterminada = (bool)Convert.ChangeType(row["EsPredeterminada"], typeof(bool));
            if (row.IsNull("Nombre"))
                uRLContenido.Nombre = null;
            else
                uRLContenido.Nombre = (string)Convert.ChangeType(row["Nombre"], typeof(string));
            if (row.IsNull("FechaRegistro"))
                uRLContenido.FechaRegistro = null;
            else
                uRLContenido.FechaRegistro = (DateTime)Convert.ChangeType(row["FechaRegistro"], typeof(DateTime));
            if (row.IsNull("Activo"))
                uRLContenido.Activo = null;
            else
                uRLContenido.Activo = (bool)Convert.ChangeType(row["Activo"], typeof(bool));
            return uRLContenido;
        }
    }
}
