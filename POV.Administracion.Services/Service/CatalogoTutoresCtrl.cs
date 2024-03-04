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
using POV.Modelo.Context;

namespace POV.Administracion.Service
{
    public class CatalogoTutoresCtrl
    {
        private LicenciaEscuelaCtrl licenciaEscuelaCtrl;
        private UsuarioCtrl usuarioCtrl;
        private Tutor antTutor;
        //private object myFirm;
        //private Contexto ctx;
        private TutorCtrl tutorCtrl;

        public CatalogoTutoresCtrl()
        {
            //myFirm = new object();
            //ctx = new Contexto(myFirm);
            tutorCtrl = new TutorCtrl(null);
            
            licenciaEscuelaCtrl = new LicenciaEscuelaCtrl();
            usuarioCtrl = new UsuarioCtrl();
            antTutor = new Tutor();
        }
        public bool InsertTutor(IDataContext dctx, Tutor tutor, Usuario usuario)
        {
            object firm = new object();

            try
            {
                dctx.OpenConnection(firm);
                dctx.BeginTransaction(firm);

                PasswordProvider passwordProvider = new PasswordProvider();
                passwordProvider.LongitudPassword = 10;
                passwordProvider.PorcentajeMayusculas = 50;
                passwordProvider.PorcentajeNumeros = 50;

                tutor.Estatus = true;
                tutor.FechaRegistro = DateTime.Now;
                tutor.CorreoConfirmado = false;
                tutor.Credito = 0;
                tutor.CreditoUsado = 0;
                tutor.Saldo = 0;
                tutor.DatosCompletos = false;
                tutor.Codigo = passwordProvider.GetNewPassword();

                usuario.EsActivo = true;
                usuario.FechaCreacion = DateTime.Now;
                usuario.Termino = new Termino() { TerminoID = 1, Cuerpo = "Terminos legales", Estatus = true };
                usuario.AceptoTerminos = false;
                usuario.PasswordTemp = true;

                if (!string.IsNullOrEmpty(usuario.NombreUsuario))
                {
                    if (UsuarioExiste(dctx, usuario))
                    {
                        Exception ex = new Exception("El nombre de usuario ya se encuentra registrado") { Source = "Usuario" };
                        throw ex;
                    }
                }

                if (!string.IsNullOrEmpty(usuario.Email))
                {
                    if (!EmailDisponible(dctx, usuario))
                    {
                        Exception ex = new Exception("El correo electrónico proporcionado no se encuentra disponible") { Source = "Email" };
                        throw ex;
                    }
                }

                if (!string.IsNullOrEmpty(usuario.TelefonoReferencia))
                {
                    if (!TelefonoDisponible(dctx, usuario))
                    {
                        Exception ex = new Exception("El Teléfono proporcionado no se encuentra disponible ") { Source = "Telefono" };
                        throw ex;
                    }
                }

                //Consulta de usuario y tutor existente               

                //Guarda el Tutor y recupera el elemento guardado
                var tutorRegistroCorrecto = tutorCtrl.Insert(tutor);
                antTutor = tutorCtrl.Retrieve(tutor, false).FirstOrDefault();

                //Guarda el Usuario y recupera el elemento guardado
                usuarioCtrl.Insert(dctx, usuario);
                Usuario antusuario = usuarioCtrl.LastDataRowToUsuario(usuarioCtrl.Retrieve(dctx, usuario));

                #region registrar usuario privilegios
                UsuarioPrivilegiosCtrl usuarioPrivilegiosCtrl = new UsuarioPrivilegiosCtrl();

                //asignamos el perfil tutor a la lista de privilegios
                Perfil perfil = new Perfil { PerfilID = (int)EPerfil.TUTOR };

                List<IPrivilegio> privilegios = new List<IPrivilegio>();
                privilegios.Add(perfil);

                usuarioPrivilegiosCtrl.InsertUsuarioEscolarPrivilegios(dctx, antusuario, new Escuela() { EscuelaID = 1 }, new CicloEscolar() { CicloEscolarID = 3 }, privilegios);

                #endregion

                #region Licencia Alumno Escuela
                //Consultar licencia del tutor
                LicenciaEscuela licenciaEscuela = new LicenciaEscuela { LicenciaEscuelaID = 1 };
                LicenciaTutor licenciaTutor = licenciaEscuelaCtrl.RetrieveLicenciaTutor(dctx, licenciaEscuela, antTutor, antusuario);

                //Registrar licencia.
                licenciaEscuelaCtrl.InsertLicenciaTutor(dctx, licenciaEscuela, antTutor, antusuario);
                #endregion

                dctx.CommitTransaction(firm);
                //ctx.Commit(myFirm);
                //ctx.Dispose();

            }
            catch (Exception ex)
            {
                dctx.RollbackTransaction(firm);
                //ctx.Database.BeginTransaction().Rollback();
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(firm);

                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);

                if (ex.Source == "Telefono" || ex.Source == "Email" || ex.Source == "Usuario")
                    throw new Exception("El " + ex.Source + " ya está en uso");
                else
                    throw new Exception(ex.Message);

                return false;             
            }
            finally
            {
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(firm);
                //ctx.Dispose();
            }
            return true;
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
