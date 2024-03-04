using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Seguridad.BO;
using POV.Seguridad.DAO;
using GP.SocialEngine.BO;
using GP.SocialEngine.Service;
using POV.Seguridad.Utils;

namespace POV.Seguridad.Service
{
    /// <summary>
    /// Servicios para acceder a Usuarios
    /// </summary>
    public class UsuarioCtrl
    {
        /// <summary>
        /// Consulta registros de usuario en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerÃ¡ acceso a la base de datos</param>
        /// <param name="usuario">usuario que provee el criterio de selecciÃ³n para realizar la consulta</param>
        /// <returns>El DataSet que contiene la informaciÃ³n de usuario generada por la consulta</returns>
        public DataSet Retrieve(IDataContext dctx, Usuario usuario, bool isService = false)
        {
            UsuarioRetHlp da = new UsuarioRetHlp();
            DataSet ds = da.Action(dctx, usuario, isService);
            return ds;
        }
        /// <summary>
        /// Actualiza de manera optimista un registro de usuario en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerÃ¡ acceso a la base de datos</param>
        /// <param name="usuario">usuario que tiene los datos nuevos</param>
        /// <param name="anterior">usuario que tiene los datos anteriores</param>
        public void Update(IDataContext dctx, Usuario usuario, Usuario previous)
        {
            if (previous.TelefonoReferencia == null)
                previous.TelefonoReferencia = "";
            if (usuario.Email != previous.Email || usuario.TelefonoReferencia != previous.TelefonoReferencia || usuario.NombreUsuario!=previous.NombreUsuario)
            {
                if ((bool)!UsuarioRepetido(dctx, usuario, previous))
                {
                    UsuarioUpdHlp da = new UsuarioUpdHlp();
                    da.Action(dctx, usuario, previous);
                }
                else
                    throw new Exception("UsuarioCtrl: No se pudo realizar la actualización");
            }
            else
            {
                UsuarioUpdHlp da = new UsuarioUpdHlp();
                da.Action(dctx, usuario, previous);
            }
            
        }

        public string GenerarNombreUsuarioUnico(IDataContext dctx, string firstName, string lastname, DateTime birthday)
        {
            if (string.IsNullOrEmpty(firstName)) throw new Exception("El primer nombre es requerido");
            if (string.IsNullOrEmpty(lastname)) throw new Exception("El apellido es requerido");

            if (birthday == null) birthday = DateTime.Now;

            string nombreUsuario = string.Empty;

            UsernameProvider provider = new UsernameProvider(firstName, lastname, birthday);

            bool exist = true;
            //mientras exista seguiremos generando nombres de usuario
            while (exist)
            {
                Usuario usuario = new Usuario();
                usuario.NombreUsuario = provider.GenerarUsername();

                DataSet ds = Retrieve(dctx, usuario);

                if (ds.Tables[0].Rows.Count < 1)
                {
                    nombreUsuario = usuario.NombreUsuario;
                    exist = false;
                }
            }

            return nombreUsuario;
        }
        private bool UsuarioRepetido(IDataContext dctx, Usuario usuario, Usuario previous)
        {
            bool repetido = false;
            Usuario consultar = new Usuario();

            if (usuario.NombreUsuario != previous.NombreUsuario)
            {
                consultar.NombreUsuario = usuario.NombreUsuario;
                DataSet dsUsuario = this.Retrieve(dctx, consultar);
                if (dsUsuario.Tables["Usuario"].Rows.Count > 0)
                {
                    repetido = true;
                    var ex = new Exception("Usuario repetido.");
                    ex.Source = "Usuario";
                    throw (ex);
                }

            }
            if (usuario.TelefonoReferencia != "" && usuario.TelefonoReferencia != null)
            {
                if (usuario.TelefonoReferencia != previous.TelefonoReferencia)
                {
                    consultar.TelefonoReferencia = usuario.TelefonoReferencia;
                    DataSet dsUsuario = this.Retrieve(dctx, consultar);
                    if (dsUsuario.Tables["Usuario"].Rows.Count > 0)
                    {
                        repetido = true;
                        var ex = new Exception("El número de teléfono no se encuentra disponible") {Source ="Telefono"};
                        throw (ex);
                    }
                }
            }
           
           
            if (!string.IsNullOrEmpty(usuario.Email) &&  usuario.Email!= previous.Email)
            {
                consultar = new Usuario();
                consultar.Email = usuario.Email;
                consultar.EsActivo = true;
                DataSet dsUsuario = this.Retrieve(dctx, consultar);
                if (dsUsuario.Tables["Usuario"].Rows.Count > 0)
                {
                    repetido = true;
                    var ex = new Exception();
                    ex.Source = "Email";
                    throw (ex);
                }
            }
            return repetido;
        }

        
        /// <summary>
        /// Elimina un registro de Usuario en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerÃ¡ acceso a la base de datos</param>
        /// <param name="usuario">Usuario que desea eliminar</param>
        public void Delete(IDataContext dctx, Usuario usuario)
        {
            UsuarioDelHlp da = new UsuarioDelHlp();
            da.Action(dctx, usuario);
        }
        public void InsertComplete(IDataContext dctx, Usuario usuario)
        {
            object myFirm = new object();
            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);
            try
            {
                Insert(dctx, usuario);
            }
            catch (Exception ex)
            {
                dctx.RollbackTransaction(myFirm);
                dctx.CloseConnection(myFirm);
                throw;
            }
            dctx.CommitTransaction(myFirm);
            dctx.CloseConnection(myFirm);
        }
        public void Insert(IDataContext dctx, Usuario usuario)
        {
            if (!ValidateForInsert(dctx, new Usuario { NombreUsuario = usuario.NombreUsuario }))
                throw new DuplicateNameException("Ya existe un usuario con este nombre, por favor, asigne otro.");
            UsuarioInsHlp da = new UsuarioInsHlp();
            da.Action(dctx, usuario);
        }
        public Usuario RetrieveComplete(IDataContext dctx, Usuario usuario)
        {
            DataSet ds = Retrieve(dctx, usuario);
            TerminoCtrl terminoCtrl = new TerminoCtrl();
            if (ds.Tables["Usuario"].Rows.Count == 0)
                throw new Exception("UsuarioCtrl: No se encontrÃ³ ningÃºn usuario con los parÃ¡metros proporcionados");
            usuario = LastDataRowToUsuario(ds);
            if (usuario.Termino.TerminoID != null)
                usuario.Termino = terminoCtrl.LastDataRowToTermino(terminoCtrl.Retrieve(dctx, usuario.Termino));
            return usuario;
        }
        public bool ValidateForInsert(IDataContext dctx, Usuario usuario)
        {
            DataSet ds = Retrieve(dctx, usuario);
            return ds.Tables["Usuario"].Rows.Count <= 0;
        }
        /// <summary>
        /// Crea un objeto de usuario a partir de los datos del Ãºltimo DataRow del DataSet.
        /// </summary>
        /// <param name="ds">El DataSet que contiene la informaciÃ³n de usuario</param>
        /// <returns>Un objeto de usuario creado a partir de los datos</returns>
        public Usuario LastDataRowToUsuario(DataSet ds)
        {
            if (!ds.Tables.Contains("Usuario"))
                throw new Exception("LastDataRowToUsuario: DataSet no tiene la tabla Usuario");
            int index = ds.Tables["Usuario"].Rows.Count;
            if (index < 1)
                throw new Exception("LastDataRowToUsuario: El DataSet no tiene filas");
            return this.DataRowToUsuario(ds.Tables["Usuario"].Rows[index - 1]);
        }
        /// <summary>
        /// Crea un objeto de usuario a partir de los datos de un DataRow.
        /// </summary>
        /// <param name="row">El DataRow que contiene la informaciÃ³n de usuario</param>
        /// <returns>Un objeto de usuario creado a partir de los datos</returns>
        public Usuario DataRowToUsuario(DataRow row)
        {
            Usuario usuario = new Usuario();
            usuario.Termino = new Termino();
            if (row.IsNull("UsuarioID"))
                usuario.UsuarioID = null;
            else
                usuario.UsuarioID = (int)Convert.ChangeType(row["UsuarioID"], typeof(int));
            if (row.IsNull("NombreUsuario"))
                usuario.NombreUsuario = null;
            else
                usuario.NombreUsuario = (string)Convert.ChangeType(row["NombreUsuario"], typeof(string));
            if (row.IsNull("Password"))
                usuario.Password = null;
            else
                usuario.Password = (byte[])Convert.ChangeType(row["Password"], typeof(byte[]));
            if (row.IsNull("EsActivo"))
                usuario.EsActivo = null;
            else
                usuario.EsActivo = (bool)Convert.ChangeType(row["EsActivo"], typeof(bool));
            if (row.IsNull("FechaCreacion"))
                usuario.FechaCreacion = null;
            else
                usuario.FechaCreacion = (DateTime)Convert.ChangeType(row["FechaCreacion"], typeof(DateTime));
            if (row.IsNull("FechaUltimoAcceso"))
                usuario.FechaUltimoAcceso = null;
            else
                usuario.FechaUltimoAcceso = (DateTime)Convert.ChangeType(row["FechaUltimoAcceso"], typeof(DateTime));
            if (row.IsNull("FechaUltimoCambioPassword"))
                usuario.FechaUltimoCambioPassword = null;
            else
                usuario.FechaUltimoCambioPassword = (DateTime)Convert.ChangeType(row["FechaUltimoCambioPassword"], typeof(DateTime));
            if (row.IsNull("Comentario"))
                usuario.Comentario = null;
            else
                usuario.Comentario = (string)Convert.ChangeType(row["Comentario"], typeof(string));
            if (row.IsNull("PasswordTemp"))
                usuario.PasswordTemp = null;
            else
                usuario.PasswordTemp = (bool)Convert.ChangeType(row["PasswordTemp"], typeof(bool));
            if (row.IsNull("Email"))
                usuario.Email = null;
            else
                usuario.Email = (string)Convert.ChangeType(row["Email"], typeof(string));
            if (row.IsNull("EmailAlternativo"))
                usuario.EmailAlternativo = null;
            else
                usuario.EmailAlternativo = (string)Convert.ChangeType(row["EmailAlternativo"], typeof(string));
            if (row.IsNull("TelefonoReferencia"))
                usuario.TelefonoReferencia = null;
            else
                usuario.TelefonoReferencia = (string)Convert.ChangeType(row["TelefonoReferencia"], typeof(string));
            if (row.IsNull("TelefonoCasa"))
                usuario.TelefonoCasa = null;
            else
                usuario.TelefonoCasa = (string)Convert.ChangeType(row["TelefonoCasa"], typeof(string));
            if (row.IsNull("EmailVerificado"))
                usuario.EmailVerificado = null;
            else
                usuario.EmailVerificado = (bool)Convert.ChangeType(row["EmailVerificado"], typeof(bool));
            if (row.IsNull("AceptoTerminos"))
                usuario.AceptoTerminos = null;
            else
                usuario.AceptoTerminos = (bool)Convert.ChangeType(row["AceptoTerminos"], typeof(bool));
            if (row.IsNull("TerminoID"))
                usuario.Termino.TerminoID = null;
            else
                usuario.Termino.TerminoID = (int)Convert.ChangeType(row["TerminoID"], typeof(int));
            if (row.IsNull("UniversidadID"))
                usuario.UniversidadId = null;
            else
                usuario.UniversidadId = (long)Convert.ChangeType(row["UniversidadID"], typeof(long));

            return usuario;
        }
    }
}
