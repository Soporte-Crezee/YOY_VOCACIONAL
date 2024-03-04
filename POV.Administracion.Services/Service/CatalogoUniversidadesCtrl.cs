using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using Framework.Base.DataAccess;
using GP.SocialEngine.BO;
using GP.SocialEngine.Service;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.Licencias.BO;
using POV.Licencias.Service;
using POV.Seguridad.BO;
using POV.Seguridad.Service;
using POV.Seguridad.Utils;
using POV.CentroEducativo.Services;
using POV.Localizacion.BO;

namespace POV.Administracion.Service
{
    public class CatalogoUniversidadesCtrl
    {

        private UniversidadCtrl universidadCtrl;
        private LicenciaEscuelaCtrl licenciaEscuelaCtrl;
        private UsuarioCtrl usuarioCtrl;
        private Universidad antUniversidad;
        private EscuelaCtrl escuelaCtrl;
        private CicloEscolarCtrl cicloEscolarCtrl;
        public CatalogoUniversidadesCtrl()
        {
            universidadCtrl = new UniversidadCtrl(null);
            licenciaEscuelaCtrl = new LicenciaEscuelaCtrl();
            usuarioCtrl = new UsuarioCtrl();
            antUniversidad = new Universidad();
            escuelaCtrl = new EscuelaCtrl();
            cicloEscolarCtrl = new CicloEscolarCtrl();
        }


        public string UrlPortalSocial { get; set; }
        public string UrlImgPOV { get; set; }

        public Usuario InsertUniversidad(IDataContext dctx, Universidad universidad, Escuela escuela, CicloEscolar ciclo, Usuario usuario, string password)
        {
            if (escuela == null || escuela.EscuelaID == null || escuela.EscuelaID <= 0)
                throw new Exception("Escuela es requerido");

            if (ciclo == null || ciclo.CicloEscolarID == null || ciclo.CicloEscolarID <= 0)
                throw new Exception("CicloEscolar es requerido");

            if (universidad == null)
                throw new Exception("Universidad es requerido");

            #region Insertar Universidad.
            object firm = new object();
            try
            {
                dctx.OpenConnection(firm);
                dctx.BeginTransaction(firm);

                #region Verificar licencia de la escuela
                LicenciaEscuela licenciaEscuela = new LicenciaEscuela { CicloEscolar = ciclo, Escuela = escuela, Activo = true };

                DataSet ds = licenciaEscuelaCtrl.Retrieve(dctx, licenciaEscuela);
                if (ds.Tables[0].Rows.Count == 0)
                    throw new Exception("La escuela no cuenta con licencia para el ciclo escolar especificado.");

                licenciaEscuela = licenciaEscuelaCtrl.LastDataRowToLicenciaEscuela(ds);
                Escuela escuelaActual = escuelaCtrl.LastDataRowToEscuela(escuelaCtrl.Retrieve(dctx, licenciaEscuela.Escuela));

                #endregion

                #region Verificar Informacion de la Universidad
                List<Universidad> existe = universidadCtrl.Retrieve(new Universidad { ClaveEscolar = universidad.ClaveEscolar }, false);

                Universidad actualizada = null;
                #region Insertar Universidad
                if (existe.Count > 0)
                {
                    Universidad universidadUpd = existe.First();
                    Universidad anterior = universidadCtrl.Retrieve(new Universidad { UniversidadID = universidadUpd.UniversidadID }, true).First();
                    anterior.Activo = true;
                    anterior.NombreUniversidad = universidad.NombreUniversidad;
                    anterior.ClaveEscolar = anterior.ClaveEscolar;
                    anterior.Direccion = universidad.Direccion;
                    anterior.Siglas = universidad.Siglas;
                    anterior.UbicacionID = universidad.UbicacionID;
                    universidadCtrl.Update(anterior);

                    universidad = universidadCtrl.Retrieve(new Universidad { UniversidadID = anterior.UniversidadID }, false).First();

                }
                else
                {
                    universidad.Activo = true;
                    universidadCtrl.Insert(universidad);
                    universidad = universidadCtrl.Retrieve(new Universidad { UniversidadID = universidad.UniversidadID }, false).First();

                }
                #endregion
                Usuario crearUsuario = licenciaEscuelaCtrl.RetrieveUsuarioUniversidad(dctx, universidad);
                #region Registro de Usuario
                bool envioCorreo = false;
                bool returnUsuario = false;
                if (crearUsuario.UsuarioID != null)
                {
                    returnUsuario = true;
                    crearUsuario = usuarioCtrl.LastDataRowToUsuario(usuarioCtrl.Retrieve(dctx, crearUsuario));

                    if (crearUsuario.EsActivo == false)
                    {
                        Usuario original = (Usuario)crearUsuario.Clone();
                        crearUsuario.NombreUsuario = usuario.NombreUsuario;
                        crearUsuario.EsActivo = true;
                        crearUsuario.Email = usuario.Email;
                        crearUsuario.TelefonoReferencia = usuario.TelefonoReferencia;
                        byte[] pws = EncryptHash.SHA1encrypt(password);
                        crearUsuario.Password = pws;

                        if (!string.IsNullOrEmpty(crearUsuario.NombreUsuario))
                            if (UsuarioExiste(dctx, crearUsuario))
                            {
                                Exception ex = new Exception("El nombre de usuario ya se encuentra registrado") { Source = "Usuario" };
                                throw ex;
                            }

                        if (!string.IsNullOrEmpty(crearUsuario.Email))
                            if (!EmailDisponible(dctx, crearUsuario))
                            {
                                Exception ex = new Exception("El correo electrónico proporcionado no se encuentra disponible") { Source = "Email" };
                                throw ex;
                            }

                        if (!string.IsNullOrEmpty(crearUsuario.TelefonoReferencia))
                            if (!TelefonoDisponible(dctx, crearUsuario))
                            {
                                Exception ex = new Exception("El teléfono proporcionado no se encuentra disponible ") { Source = "Telefono" };
                                throw ex;
                            }

                        usuarioCtrl.Update(dctx, crearUsuario, original);

                        UsuarioPrivilegiosCtrl usuarioPrivilegiosCtrl = new UsuarioPrivilegiosCtrl();

                        #region registrar usuario privilegios

                        //asignamos el perfil alumno a la lista de privilegios
                        Perfil perfil = new Perfil { PerfilID = (int)EPerfil.UNIVERSIDAD };

                        List<IPrivilegio> privilegios = new List<IPrivilegio>();
                        privilegios.Add(perfil);

                        usuarioPrivilegiosCtrl.InsertUsuarioEscolarPrivilegios(dctx, crearUsuario, licenciaEscuela.Escuela, licenciaEscuela.CicloEscolar, privilegios);

                        #endregion
                        

                        #region Registro de UsuarioSocial
                        LicenciaUniversidad licenciaUniversidad = licenciaEscuelaCtrl.RetrieveLicenciaUniversidad(dctx, licenciaEscuela, universidad, crearUsuario);
                        if(licenciaUniversidad != null)
                            licenciaEscuelaCtrl.UpdateLicenciasUniversidad(dctx, licenciaEscuela, universidad, crearUsuario);
                        else
                         licenciaEscuelaCtrl.InsertLicenciaUniversidad(dctx, licenciaEscuela, universidad, crearUsuario);
                        #endregion

                        dctx.CommitTransaction(firm);
                    }
                }
                else
                {
                    returnUsuario = false;
                    usuario.Email = usuario.Email;
                    usuario.TelefonoReferencia = usuario.TelefonoReferencia;
                    byte[] pws = EncryptHash.SHA1encrypt(password);
                    usuario.Password = pws;
                    usuario.EsActivo = true;
                    usuario.FechaCreacion = DateTime.Now;
                    usuario.PasswordTemp = true;

                    //Consultar Termino Activo
                    TerminoCtrl terminoCtrl = new TerminoCtrl();
                    DataSet dsTermino = (terminoCtrl.Retrieve(dctx, new Termino { Estatus = true }));
                    usuario.Termino = dsTermino.Tables[0].Rows.Count >= 1 ? terminoCtrl.LastDataRowToTermino(dsTermino) : new Termino();
                    usuario.AceptoTerminos = true;

                    if (!string.IsNullOrEmpty(usuario.NombreUsuario))
                        if (UsuarioExiste(dctx, usuario))
                        {
                            Exception ex = new Exception("El nombre de usuario ya se encuentra registrado") { Source = "Usuario" };
                            throw ex;
                        }

                    if (!string.IsNullOrEmpty(usuario.Email))
                        if (!EmailDisponible(dctx, usuario))
                        {
                            Exception ex = new Exception("El correo electrónico proporcionado no se encuentra disponible") { Source = "Email" };
                            throw ex;
                        }

                    if (!string.IsNullOrEmpty(usuario.TelefonoReferencia))
                        if (!TelefonoDisponible(dctx, usuario))
                        {
                            Exception ex = new Exception("El teléfono proporcionado no se encuentra disponible ") { Source = "Telefono" };
                            throw ex;
                        }

                    usuarioCtrl.Insert(dctx, usuario);
                    usuario = usuarioCtrl.LastDataRowToUsuario(usuarioCtrl.Retrieve(dctx, usuario));
                    envioCorreo = true;

                    UsuarioPrivilegiosCtrl usuarioPrivilegiosCtrl = new UsuarioPrivilegiosCtrl();

                    #region registrar usuario privilegios

                    //asignamos el perfil alumno a la lista de privilegios
                    Perfil perfil = new Perfil { PerfilID = (int)EPerfil.UNIVERSIDAD };

                    List<IPrivilegio> privilegios = new List<IPrivilegio>();
                    privilegios.Add(perfil);

                    usuarioPrivilegiosCtrl.InsertUsuarioEscolarPrivilegios(dctx, usuario, licenciaEscuela.Escuela, licenciaEscuela.CicloEscolar, privilegios);

                    #endregion
                    LicenciaUniversidad licenciaUniversidad = licenciaEscuelaCtrl.RetrieveLicenciaUniversidad(dctx, licenciaEscuela, universidad, usuario);

                    if (licenciaUniversidad.LicenciaID != null)
                        if (licenciaUniversidad.Activo.Value)
                            throw new Exception("Licencia de universidad ya se encuentra registrada.");

                    #region Registro de UsuarioSocial

                    //Registrar licencia.
                    licenciaEscuelaCtrl.InsertLicenciaUniversidad(dctx, licenciaEscuela, universidad, usuario);

                    #endregion

                    dctx.CommitTransaction(firm);
                #endregion
                }
                #endregion


                if (returnUsuario == true)
                    return crearUsuario;
                else
                    return usuario;
            }
            catch (Exception ex)
            {

                dctx.RollbackTransaction(firm);
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(firm);
                Logger.Service.LoggerHlp.Default.Error(this, ex);

                if (ex.Source == "Usuario" || ex.Source == "Telefono" || ex.Source == "Email")
                    throw new Exception("El " + ex.Source + " ya está en uso");

                throw new Exception("Ocurrió un error al registrar la universidad");
            }
            finally
            {
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(firm);
            }
            #endregion
        }

        /// <summary>
        /// Actualiza la información de la universidad.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="escuela">Escuela a consultar</param>
        /// <param name="ciclo">Ciclo escolar a consultar</param>
        /// <param name="universidad">Información de la universidad a la escuela</param>
        public void UpdateUniversidadEscuela(IDataContext dctx, Escuela escuela, CicloEscolar ciclo, Universidad universidad, Usuario usuario)
        {
            if (escuela == null || escuela.EscuelaID == null || escuela.EscuelaID <= 0)
                throw new Exception("Escuela es requerido");
            if (universidad == null)
                throw new Exception("Universidad es requerido");

            if (ciclo == null || ciclo.CicloEscolarID == null || ciclo.CicloEscolarID <= 0)
                throw new Exception("CicloEscolar es requerido");

            object firm = new object();
            try
            {
                dctx.OpenConnection(firm);
                Escuela descuela = escuelaCtrl.RetrieveComplete(dctx, escuela);

                #region Verificar licencia de la escuela


                LicenciaEscuela licenciaEscuela = new LicenciaEscuela { CicloEscolar = ciclo, Escuela = escuela, Activo = true };

                DataSet ds = licenciaEscuelaCtrl.Retrieve(dctx, licenciaEscuela);
                if (ds.Tables[0].Rows.Count == 0)
                    throw new Exception("La escuela no cuenta con licencia para el ciclo escolar especificado.");

                licenciaEscuela = licenciaEscuelaCtrl.LastDataRowToLicenciaEscuela(ds);
                if (licenciaEscuela.ListaLicencia == null || !descuela.AsignacionDocentes.Any())
                    return;

                CicloEscolar cicloEscolar = cicloEscolarCtrl.LastDataRowToCicloEscolar(cicloEscolarCtrl.Retrieve(dctx, licenciaEscuela.CicloEscolar));
                Escuela escuelaActual = escuelaCtrl.LastDataRowToEscuela(escuelaCtrl.Retrieve(dctx, licenciaEscuela.Escuela));
                #endregion

                #region Verificar universidad
                var uniAnterior = universidadCtrl.Retrieve(new Universidad { UniversidadID = universidad.UniversidadID }, true).First();
                if (uniAnterior == null)
                    throw new Exception("Universidad no encontrada");
                #endregion

                #region Verificar Usuario y Usuario Social  universidad

                Usuario uanterior = licenciaEscuelaCtrl.RetrieveUsuarioUniversidad(dctx, uniAnterior);
                if (uanterior.UsuarioID != null)
                    uanterior = usuarioCtrl.LastDataRowToUsuario(usuarioCtrl.Retrieve(dctx, uanterior));

                LicenciaUniversidad licenciaUniversidad = licenciaEscuelaCtrl.RetrieveLicenciaUniversidad(dctx, licenciaEscuela, uniAnterior, uanterior);
                if (licenciaUniversidad.LicenciaID == null)
                    throw new Exception("Licencia de la universidad no encontrada");
                #endregion

                Usuario usuarioClone = (Usuario)uanterior.Clone();
                usuarioClone.Email = usuario.Email;
                usuarioClone.TelefonoReferencia = usuario.TelefonoReferencia;
                usuarioCtrl.Update(dctx, usuarioClone, uanterior);

                #region Actualizar Universidad
                dctx.BeginTransaction(firm);

                uniAnterior.Activo = universidad.Activo;
                uniAnterior.ClaveEscolar = universidad.ClaveEscolar;
                uniAnterior.Direccion = universidad.Direccion;
                uniAnterior.Siglas = universidad.Siglas;
                uniAnterior.NombreUniversidad = universidad.NombreUniversidad;
                uniAnterior.UbicacionID = universidad.UbicacionID;
                uniAnterior.NivelEscolar = universidad.NivelEscolar;

                universidadCtrl.Update(uniAnterior);

                dctx.CommitTransaction(firm);
                #endregion

            }
            catch (Exception ex)
            {
                dctx.RollbackTransaction(firm);
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(firm);

                Logger.Service.LoggerHlp.Default.Error(this, ex);

                if (ex.Source == "Usuario" || ex.Source == "Telefono" || ex.Source == "Email")
                    throw new Exception("El " + ex.Source + " ya está en uso");

                throw new Exception("Ocurrió un error al actualizar la universidad");
            }
            finally
            {
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(firm);
            }
        }

        public void DeleteUniversidadEscuela(IDataContext dctx, Escuela escuela, CicloEscolar ciclo, Universidad universidad)
        {
            if (escuela == null || escuela.EscuelaID == null || escuela.EscuelaID <= 0)
                throw new Exception("Escuela es requerido");

            if (universidad == null)
                throw new Exception("Universidad es requerido");

            if (ciclo == null || ciclo.CicloEscolarID == null || ciclo.CicloEscolarID <= 0)
                throw new Exception("CicloEscolar es requerido");
            object firm = new object();
            try
            {
                dctx.OpenConnection(firm);
                dctx.BeginTransaction(firm);

                #region Verificar licencia de la escuela


                LicenciaEscuela licenciaEscuela = new LicenciaEscuela { CicloEscolar = ciclo, Escuela = escuela, Activo = true };
                DataSet ds = licenciaEscuelaCtrl.Retrieve(dctx, licenciaEscuela);
                if (ds.Tables[0].Rows.Count == 0)
                    throw new Exception("La escuela no cuenta con licencia para el ciclo escolar especificado.");

                licenciaEscuela = licenciaEscuelaCtrl.LastDataRowToLicenciaEscuela(ds);

                CicloEscolar cicloEscolar = cicloEscolarCtrl.LastDataRowToCicloEscolar(cicloEscolarCtrl.Retrieve(dctx, licenciaEscuela.CicloEscolar));
                Escuela escuelaActual = escuelaCtrl.RetrieveComplete(dctx, licenciaEscuela.Escuela);
                #endregion

                #region Verificar universidad
                var uniAterior = universidadCtrl.Retrieve(new Universidad { UniversidadID = universidad.UniversidadID }, true).First();

                if (uniAterior.UniversidadID == null)
                    throw new Exception("Universidad no encontrado");

                Usuario usuario = licenciaEscuelaCtrl.RetrieveUsuarioUniversidad(dctx, universidad);

                licenciaEscuelaCtrl.DeleteLicenciasUniversidad(dctx, licenciaEscuela, universidad, usuario);

                #endregion
                #region Baja de la Red Social en la escuela y ciclo Escolar
                try
                {
                    DeleteUniversidadSocial(dctx, licenciaEscuela, new GrupoCicloEscolar { Escuela = escuelaActual, CicloEscolar = cicloEscolar }, universidad);
                }
                catch (Exception ex)
                {
                    Logger.Service.LoggerHlp.Default.Error(this, ex);
                    throw new Exception("Ocurrió un error al eliminar a la universidad de la red social");
                }

                #endregion
                #region Actualizar estatus universidad y usuario

                uniAterior.Activo = false;
                universidadCtrl.Update(uniAterior);

                #endregion

                dctx.CommitTransaction(firm);
            }
            catch (Exception ex)
            {
                dctx.RollbackTransaction(firm);
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(firm);

                Logger.Service.LoggerHlp.Default.Error(this, ex);
                if (ex.Source == "Usuario" || ex.Source == "Telefono" || ex.Source == "Email")
                    throw;

                throw new Exception("Ocurrió un error mientras se eliminaba la universidad");
            }
            finally
            {
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(firm);

            }
        }

        private void DeleteUniversidadSocial(IDataContext dctx, LicenciaEscuela licenciaEscuela, GrupoCicloEscolar grupoCicloEscolar, Universidad universidad)
        {
            if (licenciaEscuela == null || universidad == null || grupoCicloEscolar == null)
                return;

            Usuario usuario = licenciaEscuelaCtrl.RetrieveUsuarioUniversidad(dctx, universidad);
            usuario = usuarioCtrl.LastDataRowToUsuario(usuarioCtrl.Retrieve(dctx, usuario));

            LicenciaUniversidad licenciaUniversidad = licenciaEscuelaCtrl.RetrieveLicenciaUniversidad(dctx, licenciaEscuela, universidad, usuario);

            if (licenciaUniversidad.LicenciaID != null)
            {
                Usuario usanterior = (Usuario)usuario.Clone();
                usuario.EsActivo = false;
                usuarioCtrl.Update(dctx, usuario, usanterior);

            }
        }


        public bool UsuarioExiste(IDataContext dctx, Usuario usuario)
        {
            DataSet ds = usuarioCtrl.Retrieve(dctx, new Usuario { NombreUsuario = usuario.NombreUsuario, EsActivo = true });
            int index = ds.Tables[0].Rows.Count;
            if (index == 1 && usuario.NombreUsuario != null)
            {
                Usuario usr = usuarioCtrl.LastDataRowToUsuario(ds);
                if (usr.NombreUsuario == usuario.NombreUsuario && usr.UsuarioID != null)
                    return true;
            }

            else if (index <= 0)
                return false;
            return false;
        }

        public bool EmailDisponible(IDataContext dctx, Usuario usuario)
        {
            if (usuario == null || string.IsNullOrEmpty(usuario.Email))
                throw new ArgumentException("Email requerido", "usuario");

            DataSet ds = usuarioCtrl.Retrieve(dctx, new Usuario { Email = usuario.Email, EsActivo = true });
            int index = ds.Tables[0].Rows.Count;
            if (index == 1 && usuario.UsuarioID != null)
            {
                Usuario usr = usuarioCtrl.LastDataRowToUsuario(ds);
                return usr.UsuarioID == usuario.UsuarioID;
            }

            if (index <= 0)
                return true;

            return false;
        }

        public bool TelefonoDisponible(IDataContext dctx, Usuario usuario)
        {
            if (usuario == null || string.IsNullOrEmpty(usuario.TelefonoReferencia))
                throw new ArgumentException("Teléfono requerido", "usuario");

            DataSet ds = usuarioCtrl.Retrieve(dctx, new Usuario { TelefonoReferencia = usuario.TelefonoReferencia });
            int index = ds.Tables[0].Rows.Count;
            if (index == 1 && usuario.UsuarioID != null)
            {
                Usuario usr = usuarioCtrl.LastDataRowToUsuario(ds);
                return usr.UsuarioID == usuario.UsuarioID;
            }

            if (index <= 0)
                return true;

            return false;
        }
    }

}
