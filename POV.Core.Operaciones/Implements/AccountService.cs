using System;
using System.Web;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Framework.Base.DataAccess;
using POV.Seguridad.BO;
using POV.Seguridad.Service;
using POV.Seguridad.Utils;
using System.Security.Cryptography;
using System.Text;
using POV.Core.Operaciones.Interfaces;

namespace POV.Core.Operaciones.Implements
{
    public class AccountService : IAccountService
    {
        #region Members
        private UsuarioCtrl usuarioCtrl;
        private IUserSession userSession;
        private IWebContext webContext;
        private IRedirector redirector;
        private RSACryptoServiceProvider sec;
        private UsuarioPrivilegiosCtrl usuarioPrivilegiosCtrl;

        #endregion

        public AccountService()
        {
            userSession = new UserSession();
            webContext = new WebContext();
            redirector = new Redirector();
            usuarioCtrl = new UsuarioCtrl();
            sec = new RSACryptoServiceProvider();
            usuarioPrivilegiosCtrl = new UsuarioPrivilegiosCtrl();
        }

        public string Login(IDataContext dctx, string username, string password)
        {
            Usuario usuario = new Usuario { NombreUsuario = username.Trim() };

            DataSet dsUsuario = usuarioCtrl.Retrieve(dctx, usuario);

            if (dsUsuario.Tables[0].Rows.Count > 0) // si existe el usuario con el username
            {
                usuario = usuarioCtrl.LastDataRowToUsuario(dsUsuario);

                if (!(bool)usuario.EsActivo)
                    return "Tu cuenta está desactivada.";
                //validacion del password
                Byte[] bPassword = EncryptHash.SHA1encrypt(password);

                if (EncryptHash.compareByteArray(usuario.Password, bPassword)) // password correcto
                {
                    Usuario usuarioAux = usuario.Clone() as Usuario;
                    usuarioAux.FechaUltimoAcceso = DateTime.Now;
                    usuarioCtrl.Update(dctx, usuarioAux, usuario);
                                    
                    DataSet dsPrivilegios = usuarioPrivilegiosCtrl.Retrieve(dctx, new UsuarioPrivilegios { Usuario = usuario });


                    UsuarioPrivilegios privilegiosOperaciones = null;

                    #region privilegios de operaciones
                    if (dsPrivilegios.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow drPrivilegios in dsPrivilegios.Tables[0].Rows)
                        {
                            UsuarioPrivilegios usuarioPrivilegios = usuarioPrivilegiosCtrl.DataRowToUsuarioPrivilegios(drPrivilegios);

                            usuarioPrivilegios = usuarioPrivilegiosCtrl.RetrieveComplete(dctx, usuarioPrivilegios);
                            //si el usuario privilegios tiene un perfil de operaciones

                            Permiso permisoAccesoOperacion = usuarioPrivilegios.GetPermisos().FirstOrDefault(per => per.PermisoID == (int)EPermiso.POACCESO);

                            if (permisoAccesoOperacion != null)
                            {
                                privilegiosOperaciones = usuarioPrivilegios;
                                break;
                            }

                        }
                    }

                    #endregion

                    if (privilegiosOperaciones != null)
                    {
                        userSession.CurrentUser = usuario;
                        userSession.CurrentPrivilegios = privilegiosOperaciones;
                        userSession.LoggedIn = true;

                        redirector.GoToHomePage(false);

                        return "";
                    }
                    else
                        return "No tienes los privilegios para acceder al portal";
                }
                else
                    return "Usuario o contraseña incorrectos";
            }
            else
                return "Usuario o contraseña incorrectos.";

        }

        public void Logout()
        {
            userSession.Logout();
            redirector.GoToLoginPage(false);
        }
    }
}
