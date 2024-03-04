using Framework.Base.DataAccess;
using GP.SocialEngine.BO;
using GP.SocialEngine.Service;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.Comun.Service;
using POV.Licencias.BO;
using POV.Licencias.Service;
using POV.Seguridad.BO;
using POV.Seguridad.Service;
using POV.Seguridad.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Mail;
namespace POV.Administracion.Service
{
    using System.Linq;

    public class CatalogoEspecialistasCtrl
    {
        private CicloEscolarCtrl cicloEscolarCtrl;
        private EscuelaCtrl escuelaCtrl;
        private EspecialistaCtrl especialistaCtrl;
        private UsuarioCtrl usuarioCtrl;
        private LicenciaEscuelaCtrl licenciaEscuelaCtrl;


        public string UrlImgNatware { get; set; }
        public string UrlPortalAptitudes { get; set; }


        public CatalogoEspecialistasCtrl()
        {
            cicloEscolarCtrl = new CicloEscolarCtrl();
            escuelaCtrl = new EscuelaCtrl();
            especialistaCtrl = new EspecialistaCtrl();
            usuarioCtrl = new UsuarioCtrl();
            licenciaEscuelaCtrl = new LicenciaEscuelaCtrl();
        }

        public void InsertEspecialistaEscuela(IDataContext dctx, Escuela escuela, CicloEscolar ciclo, EspecialistaPruebas especialista)
        {
            if (escuela == null || escuela.EscuelaID == null || escuela.EscuelaID <= 0)
                throw new Exception("Escuela es requerido");

            if (ciclo == null || ciclo.CicloEscolarID == null || ciclo.CicloEscolarID <= 0)
                throw new Exception("CicloEscolar es requerido");

            if (especialista == null)
                throw new Exception("Especialistas es requerido");
            object firm = new object();
            try
            {
                dctx.OpenConnection(firm);
                dctx.BeginTransaction(firm);

                #region *** Se verifica la Licencia de la Escuela ***
                LicenciaEscuela licenciaEscuela = new LicenciaEscuela { CicloEscolar = ciclo, Escuela = escuela, Activo = true };

                DataSet ds = licenciaEscuelaCtrl.Retrieve(dctx, licenciaEscuela);
                if (ds.Tables[0].Rows.Count == 0)
                    throw new Exception("La escuela no cuenta con licencia para el ciclo escolar especificado.");

                licenciaEscuela = licenciaEscuelaCtrl.LastDataRowToLicenciaEscuela(ds);
                Escuela escuelaActual = escuelaCtrl.LastDataRowToEscuela(escuelaCtrl.Retrieve(dctx, licenciaEscuela.Escuela));
                #endregion

                #region *** Verificar Informacion del Especialista
                ds = especialistaCtrl.Retrieve(dctx, new EspecialistaPruebas { Curp = especialista.Curp });
                int index = ds.Tables["EspecialistaPruebas"].Rows.Count;

                #region *** Insertar o Actualizar Docente ***
                if (index == 1)
                {
                    EspecialistaPruebas anterior = especialistaCtrl.DataRowToEspecialista(ds.Tables["EspecialistaPruebas"].Rows[index - 1]);
                    especialista.Estatus = true;
                    especialista.EspecialistaPruebaID = anterior.EspecialistaPruebaID;
                    especialista.FechaRegistro = anterior.FechaRegistro;
                    especialista.Clave = anterior.Clave;
                    especialista.EstatusIdentificacion = anterior.EstatusIdentificacion;
                    especialistaCtrl.Update(dctx, especialista, anterior);
                }
                else
                {
                    especialista.FechaRegistro = DateTime.Now;
                    especialista.Clave = new PasswordProvider(5).GetNewPassword();
                    especialista.EstatusIdentificacion = false;// era false
                    especialista.Estatus = true;
                    especialistaCtrl.Insert(dctx, especialista);
                }
                #endregion

                #region *** Registrar Asignación Especialista-Escuela ***

                ds = especialistaCtrl.Retrieve(dctx, new EspecialistaPruebas { Curp = especialista.Curp });
                index = ds.Tables["EspecialistaPruebas"].Rows.Count;
                if (index == 1)
                {
                    especialista = especialistaCtrl.DataRowToEspecialista(ds.Tables["EspecialistaPruebas"].Rows[index - 1]);
                    //Registrar docente en la escuela
                    escuelaCtrl.InsertAsignacionEspecialista(dctx, especialista, escuelaActual);
                }
                else
                    throw new Exception("InsertEspecialistaEscuela:Ocurrió un error al registrar al Especialista");

                #endregion

                #region Registro de Usuario

                Usuario usuario = licenciaEscuelaCtrl.RetrieveUsuario(dctx, especialista);

                string paswordtemp = string.Empty;
                bool envioCorreo = false;
                if (usuario.UsuarioID != null)
                {
                    usuario = usuarioCtrl.LastDataRowToUsuario(usuarioCtrl.Retrieve(dctx, usuario));

                    if (usuario.EsActivo == false)
                    {
                        Usuario original = (Usuario)usuario.Clone();
                        usuario.EsActivo = true;
                        usuario.Email = especialista.Correo;

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

                        usuarioCtrl.Update(dctx, usuario, original);
                    }
                }
                else
                {
                    usuario.NombreUsuario = usuarioCtrl.GenerarNombreUsuarioUnico(dctx, especialista.Nombre, especialista.PrimerApellido, especialista.FechaNacimiento.Value);
                    usuario.Email = especialista.Correo;
                    paswordtemp = new PasswordProvider(8).GetNewPassword();
                    byte[] pws = EncryptHash.SHA1encrypt(paswordtemp);
                    usuario.Password = pws;
                    usuario.EsActivo = true;
                    usuario.FechaCreacion = DateTime.Now;
                    usuario.PasswordTemp = true;

                    //Consultar Termino Activo
                    TerminoCtrl terminoCtrl = new TerminoCtrl();
                    DataSet dsTermino = (terminoCtrl.Retrieve(dctx, new Termino { Estatus = true }));
                    usuario.Termino = dsTermino.Tables[0].Rows.Count >= 1 ? terminoCtrl.LastDataRowToTermino(dsTermino) : new Termino();
                    usuario.AceptoTerminos = false;

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
                }
                #endregion

                UsuarioPrivilegiosCtrl usuarioPrivilegiosCtrl = new UsuarioPrivilegiosCtrl();

                #region registrar usuario privilegios

                //asignamos el perfil alumno a la lista de privilegios
                Perfil perfil = new Perfil { PerfilID = (int)EPerfil.ESPECIALISTA };

                List<IPrivilegio> privilegios = new List<IPrivilegio>();
                privilegios.Add(perfil);

                usuarioPrivilegiosCtrl.InsertUsuarioEscolarPrivilegios(dctx, usuario, licenciaEscuela.Escuela, licenciaEscuela.CicloEscolar, privilegios);

                #endregion

                LicenciaEspecialistaPruebas licenciaEspecialista = licenciaEscuelaCtrl.RetrieveLicenciaEspecialista(dctx, licenciaEscuela, especialista, usuario);

                if (licenciaEspecialista.LicenciaID != null)
                    if (licenciaEspecialista.Activo.Value)
                        throw new Exception("Licencia de docente ya se encuentra registrada.");

                licenciaEscuelaCtrl.InsertLicenciaEspecialista(dctx, licenciaEscuela, especialista, usuario);

                dctx.CommitTransaction(firm);
                /*Envió Correo electrónico*/
                if (envioCorreo)
                    EnviarCorreo(usuario, paswordtemp, especialista.Clave);
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

                throw new Exception("Ocurrió un error al registrar al Docente");
            }
            finally
            {
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(firm);
            }
        }

        public void UpdateEspecialistaEscuela(IDataContext dctx, Escuela escuela, CicloEscolar ciclo, EspecialistaPruebas especialista)
        {
            if (escuela == null || escuela.EscuelaID == null || escuela.EscuelaID <= 0)
                throw new Exception("Escuela es requerido");
            if (especialista == null)
                throw new Exception("Especialista es requerido");

            if (ciclo == null || ciclo.CicloEscolarID == null || ciclo.CicloEscolarID <= 0)
                throw new Exception("CicloEscolar es requerido");



            object firm = new object();
            try
            {
                dctx.OpenConnection(firm);
                Escuela descuela = escuelaCtrl.RetrieveComplete(dctx, escuela);

                #region Verificar licencia de la escuela


                LicenciaEscuela licenciaEscuela = new LicenciaEscuela { CicloEscolar = ciclo, Escuela = descuela, Activo = true };

                DataSet ds = licenciaEscuelaCtrl.Retrieve(dctx, licenciaEscuela);
                if (ds.Tables[0].Rows.Count == 0)
                    throw new Exception("La escuela no cuenta con licencia para el ciclo escolar especificado.");

                licenciaEscuela = licenciaEscuelaCtrl.LastDataRowToLicenciaEscuela(ds);
                if (licenciaEscuela.ListaLicencia == null)
                    return;

                CicloEscolar cicloEscolar = cicloEscolarCtrl.LastDataRowToCicloEscolar(cicloEscolarCtrl.Retrieve(dctx, licenciaEscuela.CicloEscolar));
                Escuela escuelaActual = escuelaCtrl.LastDataRowToEscuela(escuelaCtrl.Retrieve(dctx, licenciaEscuela.Escuela));
                #endregion

                #region Verificar docente

                ds = especialistaCtrl.Retrieve(dctx, new EspecialistaPruebas { EspecialistaPruebaID = especialista.EspecialistaPruebaID, Estatus = true });
                if (ds.Tables[0].Rows.Count != 1)
                    throw new Exception("Docente no encontrado");
                EspecialistaPruebas danterior = especialistaCtrl.LastDataRowToEspecialista(ds);

                #endregion

                #region Verificar Usuario y Usuario Social  docente

                Usuario uanterior = licenciaEscuelaCtrl.RetrieveUsuario(dctx, danterior);
                if (uanterior.UsuarioID != null)
                    uanterior = usuarioCtrl.LastDataRowToUsuario(usuarioCtrl.Retrieve(dctx, uanterior));

                LicenciaEspecialistaPruebas licenciaEspecialista = licenciaEscuelaCtrl.RetrieveLicenciaEspecialista(dctx, licenciaEscuela, danterior, uanterior);
                if (licenciaEspecialista.LicenciaID == null)
                    throw new Exception("Licencia del especialista no encontrada");
                #endregion

                #region Actualizar Docente
                dctx.BeginTransaction(firm);

                especialista.Estatus = danterior.Estatus;
                especialista.EstatusIdentificacion = danterior.EstatusIdentificacion;
                especialista.FechaRegistro = danterior.FechaRegistro;
                especialista.Clave = danterior.Clave;

                especialistaCtrl.Update(dctx, especialista, danterior);

                //Actualizar Usuario (Correo)
                Usuario usuario = (Usuario)uanterior.Clone();
                usuario.Email = especialista.Correo;
                usuarioCtrl.Update(dctx, usuario, uanterior);

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

                throw new Exception("Ocurrió un error al actualizar el especialista");
            }
            finally
            {
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(firm);
            }
        }

        public void DeleteEspecialistaEscuela(IDataContext dctx, Escuela escuela, CicloEscolar ciclo, EspecialistaPruebas especialista)
        {
            if (escuela == null || escuela.EscuelaID == null || escuela.EscuelaID <= 0)
                throw new Exception("Escuela es requerido");

            if (especialista == null)
                throw new Exception("Especialista es requerido");

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

                Escuela escuelaActual = escuelaCtrl.RetrieveComplete(dctx, licenciaEscuela.Escuela);
                #endregion

                #region Verificar docente
                ds = especialistaCtrl.Retrieve(dctx, especialista);
                int index = ds.Tables["EspecialistaPruebas"].Rows.Count;

                if (index <= 0)
                    throw new Exception("Especialista no encontrado");

                especialista = especialistaCtrl.DataRowToEspecialista(ds.Tables["EspecialistaPruebas"].Rows[index - 1]);
                Usuario usuario = licenciaEscuelaCtrl.RetrieveUsuario(dctx, especialista);

                #endregion

                escuelaCtrl.DeleteAignacionesEspecialista(dctx, new AsignacionEspecialistaEscuela { Especialista = especialista }, escuelaActual);
                licenciaEscuelaCtrl.DeleteLicenciasEspecialista(dctx, licenciaEscuela, especialista, usuario);

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

                throw new Exception("Ocurrió un error mientras se eliminaba el docente");
            }
            finally
            {
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(firm);

            }
        }


        private void EnviarCorreo(Usuario usuario, string pws, string clave)
        {


            #region Variables
            string urlimg = UrlImgNatware;
            const string imgalt = "Sistema Natware";
            const string titulo = "SISTEMA NATWARE";
            string linkportal = UrlPortalAptitudes;
            #endregion
            CorreoCtrl correoCtrl = new CorreoCtrl();

            string cuerpo = string.Format(@"<table width='600'><tr><td>
                                            <img src='{0}' alt='{1}' /></td></tr>
                                            <tr><td><h2 style='color:#A5439A'>{2}</h2>
                                            </p><p>Estos son los datos para que accedas a tu portal.</p>
                                            <p><b>Usuario:</b> {3}</p>
                                            <p><b>Contraseña:</b> {4}</p><p>Una vez que entres al portal, te recomendamos cambiar tu contraseña.</p>
                                            <p>Clave de activación: {5}</p></td>
                                            </tr>
                                            <tr><td>
                                            <a href='{6}'>Natware - Portal Aptitudes Sobresalientes</a>
                                            </td></tr>
                                          </table>"
                                            , urlimg, imgalt, titulo, usuario.NombreUsuario, pws, clave, linkportal);
            List<string> tos = new List<string>();
            tos.Add(usuario.Email);

            try
            {
                correoCtrl.sendMessage(tos, "Confirmación cuenta", cuerpo, (AlternateView)null, new List<string>(), new List<string>());

            }
            catch (Exception ex)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
            }
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
