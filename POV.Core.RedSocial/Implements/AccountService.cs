using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Framework.Base.DataAccess;
using POV.Seguridad.BO;
using POV.Seguridad.Service;
using POV.Seguridad.Utils;
using POV.Core.RedSocial.Interfaces;
using POV.CentroEducativo.BO;
using GP.SocialEngine.BO;
using GP.SocialEngine.Service;
using POV.CentroEducativo.Service;
using System.Security.Cryptography;
using System.Text;
using POV.Licencias.Service;
using POV.Licencias.BO;
using POV.Localizacion.Service;
using POV.Expediente.Service;
using POV.CentroEducativo.BO;
using POV.Prueba.Diagnostico.Dinamica.BO;
using POV.Expediente.BO;
using POV.Modelo.BO;

namespace POV.Core.RedSocial.Implement
{
    public class AccountService : IAccountService
    {
        #region Members
        private LicenciaEscuelaCtrl licenciaEscuelaCtrl;
        private AlumnoCtrl alumnoCtrl;
        private DocenteCtrl docenteCtrl;
        private EscuelaCtrl escuelaCtrl;
        private UsuarioCtrl usuarioCtrl;
        private SocialHubCtrl socialHubCtrl;
        private IUserSession userSession;
        private IWebContext webContext;
        private IRedirector redirector;
        private UsuarioSocialCtrl usuarioSocialCtrl;
        private GrupoSocialCtrl grupoSocialCtrl;
        private RSACryptoServiceProvider sec;
        private GrupoCicloEscolarCtrl grupoCicloEscolarCtrl;
        private UbicacionCtrl ubicacionCtrl;

        #endregion

        public AccountService()
        {
            licenciaEscuelaCtrl = new LicenciaEscuelaCtrl();
            userSession = new UserSession();
            webContext = new WebContext();
            redirector = new Redirector();
            socialHubCtrl = new SocialHubCtrl();
            grupoSocialCtrl = new GrupoSocialCtrl();
            usuarioSocialCtrl = new UsuarioSocialCtrl();
            usuarioCtrl = new UsuarioCtrl();
            sec = new RSACryptoServiceProvider();
            alumnoCtrl = new AlumnoCtrl();
            docenteCtrl = new DocenteCtrl();
            escuelaCtrl = new EscuelaCtrl();
            grupoCicloEscolarCtrl = new GrupoCicloEscolarCtrl();
            ubicacionCtrl = new UbicacionCtrl();

        }

        public string Login(IDataContext dctx, string username, string password, byte[] binaryPassword = null, string tipoUsuario="all")
        {
            if (tipoUsuario == "asp" || tipoUsuario == "ori")
                return LoginAlt(dctx, username, password, binaryPassword: binaryPassword, tipoUsuario: tipoUsuario);

            Usuario usuario = new Usuario { NombreUsuario = username.Trim() };

            DataSet dsUsuario = usuarioCtrl.Retrieve(dctx, usuario);

            if (dsUsuario.Tables[0].Rows.Count > 0) // si existe el usuario con el username
            {
                usuario = usuarioCtrl.LastDataRowToUsuario(dsUsuario);

                if (!(bool)usuario.EsActivo)
                    return "Tu cuenta está desactivada.";
                //validacion del password
                Byte[] bPassword = binaryPassword ?? EncryptHash.SHA1encrypt(password);

                if (EncryptHash.compareByteArray(usuario.Password, bPassword)) // password correcto
                {
                    Usuario usuarioAux = usuario.Clone() as Usuario;
                    usuarioAux.FechaUltimoAcceso = DateTime.Now;
                    usuarioCtrl.Update(dctx, usuarioAux, usuario);

                    List<LicenciaEscuela> licenciasEscuela = licenciaEscuelaCtrl.RetrieveLicencia(dctx, usuario);
                    List<LicenciaEscuela> licenciasDocente = new List<LicenciaEscuela>();

                    if (licenciasEscuela.Count <= 0)
                        return "No tienes licencias activas o han expirado.";

                    if (licenciasEscuela.Count(item => (bool)item.Activo) <= 0)
                        return "No tienes licencias activas o han expirado.";

                    UsuarioPrivilegiosCtrl usuarioPrivilegiosCtrl = new UsuarioPrivilegiosCtrl();

                    #region seleccionar licencias
                    foreach (LicenciaEscuela licenciaEscuela in licenciasEscuela) // se recorre la lista para encontrar las del docente, solo se recorre una vez para alumnos.
                    {

                        //buscamos la licencia
                        ALicencia licencia = licenciaEscuela.ListaLicencia.Find(item => (bool)item.Activo && item.Tipo != ETipoLicencia.DIRECTOR);

                        if (licencia == null)
                            continue;
                        userSession.Contrato = licenciaEscuela.Contrato;
                        if (licencia.Tipo == ETipoLicencia.ALUMNO) // si es alumno solo puede tener una licencia por escuela
                        {
                            LicenciaAlumno licenciaAlumno = (LicenciaAlumno)licencia;
                            userSession.CurrentAlumno = alumnoCtrl.LastDataRowToAlumno(alumnoCtrl.Retrieve(dctx, licenciaAlumno.Alumno));

                            userSession.CurrentAlumno.AreasConocimiento = new List<AreaConocimiento>();
                            ExpedienteEscolarCtrl expCtrl = new ExpedienteEscolarCtrl();
                            var alumno = (Alumno)userSession.CurrentAlumno;
                            PruebaDinamica pruebaDinamica = new PruebaDinamica();
                            var interesesAspirante = expCtrl.RetrieveInteresesAspirante(dctx, alumno, pruebaDinamica);
                            userSession.CurrentAlumno.AreasConocimiento = (from area in interesesAspirante
                                                                           select new AreaConocimiento()
                                                                           {
                                                                               AreaConocimentoID = area.clasificador.ClasificadorID,
                                                                               Nombre = area.clasificador.Nombre,
                                                                               Descripcion = area.clasificador.Descripcion
                                                                           }).GroupBy(x => x.AreaConocimentoID).Select(grp => grp.First()).OrderBy(x => x.AreaConocimentoID).ToList();
                            //.GroupBy(x => x.clasificador).Select(grp => grp.First()).ToList();

                            //userSession.CurrentAlumno.AreasConocimiento = null;
                            userSession.CurrentEscuela = escuelaCtrl.LastDataRowToEscuela(escuelaCtrl.Retrieve(dctx, licenciaEscuela.Escuela));
                            userSession.CurrentUsuarioSocial = usuarioSocialCtrl.LastDataRowToUsuarioSocial(usuarioSocialCtrl.Retrieve(dctx, licenciaAlumno.UsuarioSocial));
                            userSession.SocialHub = socialHubCtrl.LastDataRowToSocialHub(socialHubCtrl.RetrieveSocialHubUsuario(dctx, new SocialHub { SocialProfileType = ESocialProfileType.USUARIOSOCIAL, SocialProfile = new UsuarioSocial { UsuarioSocialID = userSession.CurrentUsuarioSocial.UsuarioSocialID } }));
                            userSession.CurrentCicloEscolar = licenciaEscuela.CicloEscolar;

                            List<GrupoSocial> grupos = grupoSocialCtrl.RetrieveGruposSocialSocialHub(dctx, userSession.SocialHub);

                            if (grupos.Count <= 0)
                            {
                                userSession.Logout();
                                return "No perteneces a ningun grupo escolar";
                            }

                            GrupoCicloEscolar grupoCicloEscolar = grupoCicloEscolarCtrl.RetrieveGrupoCicloEscolar(dctx, userSession.CurrentAlumno, userSession.CurrentCicloEscolar);

                            if (grupoCicloEscolar.GrupoCicloEscolarID == null)
                                return "No perteneces a ningun grupo escolar";

                            UsuarioPrivilegios usuarioPrivilegios = new UsuarioEscolarPrivilegios();
                            usuarioPrivilegios.Usuario = usuario;
                            (usuarioPrivilegios as UsuarioEscolarPrivilegios).Escuela = userSession.CurrentEscuela;
                            (usuarioPrivilegios as UsuarioEscolarPrivilegios).CicloEscolar = userSession.CurrentCicloEscolar;

                            DataSet dsPrivilegios = usuarioPrivilegiosCtrl.Retrieve(dctx, usuarioPrivilegios);
                            if (dsPrivilegios.Tables[0].Rows.Count < 1)
                                return "No tienes privilegios para acceder al portal";

                            usuarioPrivilegios = usuarioPrivilegiosCtrl.RetrieveComplete(dctx, usuarioPrivilegios);

                            if (usuarioPrivilegios.UsuarioAccesos.Count < 1)
                                return "No tienes privilegios para acceder al portal";

                            userSession.UsuarioPrivilegios = usuarioPrivilegios;
                            userSession.CurrentGrupoCicloEscolar = grupoCicloEscolar;
                            userSession.CurrentGrupoSocial = grupos.First();
                            userSession.CurrentUser = usuario;
                            userSession.Perfil = "A";
                            userSession.LoggedIn = true;
                            userSession.ModulosFuncionales = licenciaEscuela.ModulosFuncionales;
                            //userSession.CurrentAlumno.EstatusIdentificacion = true; 
                            //userSession.CurrentAlumno.CorreoConfirmado = true; 

                            if (userSession.CurrentAlumno.EstatusIdentificacion == false || userSession.CurrentAlumno.EstatusIdentificacion == null)
                            {
                                redirector.GoToConfirmarAlumno(false);
                                return "";
                            }

                            redirector.GoToValidarDiagnostico(false);

                            return "";
                        }
                        else
                        {// es docente, se agrega la licencia en la lista, siempre y cuando tenga privilegios

                            UsuarioPrivilegios usuarioPrivilegios = new UsuarioEscolarPrivilegios();
                            usuarioPrivilegios.Usuario = usuario;
                            (usuarioPrivilegios as UsuarioEscolarPrivilegios).Escuela = licenciaEscuela.Escuela;
                            (usuarioPrivilegios as UsuarioEscolarPrivilegios).CicloEscolar = licenciaEscuela.CicloEscolar;

                            DataSet dsPrivilegios = usuarioPrivilegiosCtrl.Retrieve(dctx, usuarioPrivilegios);
                            if (dsPrivilegios.Tables[0].Rows.Count < 1)
                                continue;

                            usuarioPrivilegios = usuarioPrivilegiosCtrl.RetrieveComplete(dctx, usuarioPrivilegios);

                            if (usuarioPrivilegios.UsuarioAccesos.Count < 1)
                                continue;

                            //solo insertamos si el usuario tiene privilegios asignados
                            licenciasDocente.Add(licenciaEscuela);
                        }
                    }
                    #endregion

                    if (licenciasDocente.Count <= 0)
                        return "No tienes licencias para acceder al portal.";
                    //se agregan los datos para el docente en session
                    userSession.LicenciasDocente = licenciasDocente;
                    userSession.CurrentUser = usuario;
                    userSession.LoggedIn = true;
                    userSession.Perfil = "D";

                    //primero preguntar si esta confirmado si es false, mandar a confirmar datos, si es true continuar preguntando si acepto terminos

                    LicenciaEscuela MilicenciaEscuela = userSession.LicenciasDocente.First();
                    LicenciaDocente licenciaDocente = (LicenciaDocente)MilicenciaEscuela.ListaLicencia.First(item => item.Tipo == ETipoLicencia.DOCENTE);
                    userSession.CurrentDocente = docenteCtrl.LastDataRowToDocente(docenteCtrl.Retrieve(dctx, licenciaDocente.Docente));
                    if (!(bool)userSession.CurrentDocente.EstatusIdentificacion)
                    {
                        redirector.GoToConfirmarMaestro(false);
                        return "";
                    }

                    //si esta confirmado preguntar si acepto terminos

                    if ((bool)usuario.AceptoTerminos)
                        redirector.GoToCambiarEscuela(false);
                    else
                        redirector.GoToAceptarTerminos(false);

                    return "";

                }
                else
                    return "Usuario y contraseña incorrectos";
            }
            else
                return "Usuario y contraseña incorrectos.";

        }

        public string LoginAlt(IDataContext dctx, string username, string password, byte[] binaryPassword = null, string tipoUsuario = "all")
        {

            Usuario usuario = new Usuario { NombreUsuario = username.Trim() };

            DataSet dsUsuario = usuarioCtrl.Retrieve(dctx, usuario);

            if (dsUsuario.Tables[0].Rows.Count > 0) // si existe el usuario con el username
            {
                usuario = usuarioCtrl.LastDataRowToUsuario(dsUsuario);

                if (!(bool)usuario.EsActivo)
                    return "Tu cuenta está desactivada.";
                //validacion del password
                Byte[] bPassword = binaryPassword ?? EncryptHash.SHA1encrypt(password);

                if (EncryptHash.compareByteArray(usuario.Password, bPassword)) // password correcto
                {
                    Usuario usuarioAux = usuario.Clone() as Usuario;
                    usuarioAux.FechaUltimoAcceso = DateTime.Now;
                    usuarioCtrl.Update(dctx, usuarioAux, usuario);

                    List<LicenciaEscuela> licenciasEscuela = licenciaEscuelaCtrl.RetrieveLicencia(dctx, usuario);
                    List<LicenciaEscuela> licenciasDocente = new List<LicenciaEscuela>();

                    if (licenciasEscuela.Count <= 0)
                        return "No tienes licencias activas o han expirado.";

                    if (licenciasEscuela.Count(item => (bool)item.Activo) <= 0)
                        return "No tienes licencias activas o han expirado.";

                    UsuarioPrivilegiosCtrl usuarioPrivilegiosCtrl = new UsuarioPrivilegiosCtrl();

                    #region seleccionar licencias
                    foreach (LicenciaEscuela licenciaEscuela in licenciasEscuela) // se recorre la lista para encontrar las del docente, solo se recorre una vez para alumnos.
                    {

                        //buscamos la licencia
                        ALicencia licencia = licenciaEscuela.ListaLicencia.Find(item => (bool)item.Activo && item.Tipo != ETipoLicencia.DIRECTOR);

                        if (licencia == null)
                            continue;
                        userSession.Contrato = licenciaEscuela.Contrato;
                        if (licencia.Tipo == ETipoLicencia.ALUMNO && tipoUsuario == "asp") // si es alumno solo puede tener una licencia por escuela
                        {
                            LicenciaAlumno licenciaAlumno = (LicenciaAlumno)licencia;
                            userSession.CurrentAlumno = alumnoCtrl.LastDataRowToAlumno(alumnoCtrl.Retrieve(dctx, licenciaAlumno.Alumno));

                            userSession.CurrentAlumno.AreasConocimiento = new List<AreaConocimiento>();
                            ExpedienteEscolarCtrl expCtrl = new ExpedienteEscolarCtrl();
                            var alumno = (Alumno)userSession.CurrentAlumno;
                            PruebaDinamica pruebaDinamica = new PruebaDinamica();
                            var interesesAspirante = expCtrl.RetrieveInteresesAspirante(dctx, alumno, pruebaDinamica);
                            userSession.CurrentAlumno.AreasConocimiento = (from area in interesesAspirante
                                                                           select new AreaConocimiento()
                                                                           {
                                                                               AreaConocimentoID = area.clasificador.ClasificadorID,
                                                                               Nombre = area.clasificador.Nombre,
                                                                               Descripcion = area.clasificador.Descripcion
                                                                           }).GroupBy(x => x.AreaConocimentoID).Select(grp => grp.First()).OrderBy(x => x.AreaConocimentoID).ToList();

                            
                            userSession.CurrentEscuela = escuelaCtrl.LastDataRowToEscuela(escuelaCtrl.Retrieve(dctx, licenciaEscuela.Escuela));
                            userSession.CurrentUsuarioSocial = usuarioSocialCtrl.LastDataRowToUsuarioSocial(usuarioSocialCtrl.Retrieve(dctx, licenciaAlumno.UsuarioSocial));
                            userSession.SocialHub = socialHubCtrl.LastDataRowToSocialHub(socialHubCtrl.RetrieveSocialHubUsuario(dctx, new SocialHub { SocialProfileType = ESocialProfileType.USUARIOSOCIAL, SocialProfile = new UsuarioSocial { UsuarioSocialID = userSession.CurrentUsuarioSocial.UsuarioSocialID } }));
                            userSession.CurrentCicloEscolar = licenciaEscuela.CicloEscolar;

                            List<GrupoSocial> grupos = grupoSocialCtrl.RetrieveGruposSocialSocialHub(dctx, userSession.SocialHub);

                            if (grupos.Count <= 0)
                            {
                                userSession.Logout();
                                return "No perteneces a ningun grupo escolar";
                            }

                            GrupoCicloEscolar grupoCicloEscolar = grupoCicloEscolarCtrl.RetrieveGrupoCicloEscolar(dctx, userSession.CurrentAlumno, userSession.CurrentCicloEscolar);

                            if (grupoCicloEscolar.GrupoCicloEscolarID == null)
                                return "No perteneces a ningun grupo escolar";

                            UsuarioPrivilegios usuarioPrivilegios = new UsuarioEscolarPrivilegios();
                            usuarioPrivilegios.Usuario = usuario;
                            (usuarioPrivilegios as UsuarioEscolarPrivilegios).Escuela = userSession.CurrentEscuela;
                            (usuarioPrivilegios as UsuarioEscolarPrivilegios).CicloEscolar = userSession.CurrentCicloEscolar;

                            DataSet dsPrivilegios = usuarioPrivilegiosCtrl.Retrieve(dctx, usuarioPrivilegios);
                            if (dsPrivilegios.Tables[0].Rows.Count < 1)
                                return "No tienes privilegios para acceder al portal";

                            usuarioPrivilegios = usuarioPrivilegiosCtrl.RetrieveComplete(dctx, usuarioPrivilegios);

                            if (usuarioPrivilegios.UsuarioAccesos.Count < 1)
                                return "No tienes privilegios para acceder al portal";

                            userSession.UsuarioPrivilegios = usuarioPrivilegios;
                            userSession.CurrentGrupoCicloEscolar = grupoCicloEscolar;
                            userSession.CurrentGrupoSocial = grupos.First();
                            userSession.CurrentUser = usuario;
                            userSession.Perfil = "A";
                            userSession.LoggedIn = true;
                            userSession.ModulosFuncionales = licenciaEscuela.ModulosFuncionales;
                            return "";
                        }
                        else
                        {// es docente, se agrega la licencia en la lista, siempre y cuando tenga privilegios
                            if (licencia.Tipo == ETipoLicencia.DOCENTE && tipoUsuario == "ori")
                            {

                                UsuarioPrivilegios usuarioPrivilegios = new UsuarioEscolarPrivilegios();
                                usuarioPrivilegios.Usuario = usuario;
                                (usuarioPrivilegios as UsuarioEscolarPrivilegios).Escuela = licenciaEscuela.Escuela;
                                (usuarioPrivilegios as UsuarioEscolarPrivilegios).CicloEscolar = licenciaEscuela.CicloEscolar;

                                DataSet dsPrivilegios = usuarioPrivilegiosCtrl.Retrieve(dctx, usuarioPrivilegios);
                                if (dsPrivilegios.Tables[0].Rows.Count < 1)
                                    continue;

                                usuarioPrivilegios = usuarioPrivilegiosCtrl.RetrieveComplete(dctx, usuarioPrivilegios);

                                if (usuarioPrivilegios.UsuarioAccesos.Count < 1)
                                    continue;

                                //solo insertamos si el usuario tiene privilegios asignados
                                licenciasDocente.Add(licenciaEscuela);
                            }
                            else
                            {
                                string portal = tipoUsuario == "ori" ? "orientadores" : "aspirantes";
                                userSession.LoggedIn = false;
                                userSession.CurrentUser = null;
                                userSession.CurrentUsuarioSocial = null;
                                userSession.SocialHub = null;
                                userSession.CurrentAlumno = null;
                                userSession.CurrentDocente = null;
                                userSession.Contrato = null;
                                userSession.CurrentCicloEscolar = null;
                                userSession.CurrentGrupoCicloEscolar = null;
                                userSession.ModulosFuncionales = null;
                                return "No tienes licencias para acceder al portal de " + portal + ".";
                            }
                        }
                    }
                    #endregion

                    if (licenciasDocente.Count <= 0)
                        return "No tienes licencias para acceder al portal.";
                    //se agregan los datos para el docente en session
                    userSession.LicenciasDocente = licenciasDocente;
                    userSession.CurrentUser = usuario;
                    userSession.LoggedIn = true;
                    userSession.Perfil = "D";

                    //primero preguntar si esta confirmado si es false, mandar a confirmar datos, si es true continuar preguntando si acepto terminos

                    LicenciaEscuela MilicenciaEscuela = userSession.LicenciasDocente.First();
                    LicenciaDocente licenciaDocente = (LicenciaDocente)MilicenciaEscuela.ListaLicencia.First(item => item.Tipo == ETipoLicencia.DOCENTE);
                    userSession.CurrentDocente = docenteCtrl.LastDataRowToDocente(docenteCtrl.Retrieve(dctx, licenciaDocente.Docente));
                    if (!(bool)userSession.CurrentDocente.EstatusIdentificacion)
                    {
                        redirector.GoToConfirmarMaestro(false);
                        return "";
                    }

                    //si esta confirmado preguntar si acepto terminos

                    if ((bool)usuario.AceptoTerminos)
                        redirector.GoToCambiarEscuela(false);
                    else
                        redirector.GoToAceptarTerminos(false);

                    return "";

                }
                else
                    return "Usuario y contraseña incorrectos";
            }
            else
                return "Usuario y contraseña incorrectos.";

        }

        public string LoginPruebas(IDataContext dctx, string username, string password, byte[] binaryPassword = null, string tipoUsuario = "all")
        {

            Usuario usuario = new Usuario { NombreUsuario = username.Trim() };

            DataSet dsUsuario = usuarioCtrl.Retrieve(dctx, usuario);

            if (dsUsuario.Tables[0].Rows.Count > 0) // si existe el usuario con el username
            {
                usuario = usuarioCtrl.LastDataRowToUsuario(dsUsuario);

                if (!(bool)usuario.EsActivo)
                    return "Tu cuenta está desactivada.";
                //validacion del password
                Byte[] bPassword = binaryPassword ?? EncryptHash.SHA1encrypt(password);

                if (EncryptHash.compareByteArray(usuario.Password, bPassword)) // password correcto
                {
                    Usuario usuarioAux = usuario.Clone() as Usuario;
                    usuarioAux.FechaUltimoAcceso = DateTime.Now;
                    usuarioCtrl.Update(dctx, usuarioAux, usuario);

                    List<LicenciaEscuela> licenciasEscuela = licenciaEscuelaCtrl.RetrieveLicencia(dctx, usuario);
                    List<LicenciaEscuela> licenciasDocente = new List<LicenciaEscuela>();

                    if (licenciasEscuela.Count <= 0)
                        return "No tienes licencias activas o han expirado.";

                    if (licenciasEscuela.Count(item => (bool)item.Activo) <= 0)
                        return "No tienes licencias activas o han expirado.";

                    UsuarioPrivilegiosCtrl usuarioPrivilegiosCtrl = new UsuarioPrivilegiosCtrl();

                    #region seleccionar licencias
                    foreach (LicenciaEscuela licenciaEscuela in licenciasEscuela) // se recorre la lista para encontrar las del docente, solo se recorre una vez para alumnos.
                    {

                        //buscamos la licencia
                        ALicencia licencia = licenciaEscuela.ListaLicencia.Find(item => (bool)item.Activo && item.Tipo != ETipoLicencia.DIRECTOR);

                        if (licencia == null)
                            continue;
                        userSession.Contrato = licenciaEscuela.Contrato;
                        if (licencia.Tipo == ETipoLicencia.ALUMNO && (tipoUsuario == "pruebas" || tipoUsuario == "bullying")) // si es alumno solo puede tener una licencia por escuela
                        {
                            LicenciaAlumno licenciaAlumno = (LicenciaAlumno)licencia;
                            userSession.CurrentAlumno = alumnoCtrl.LastDataRowToAlumno(alumnoCtrl.Retrieve(dctx, licenciaAlumno.Alumno));

                            userSession.CurrentAlumno.AreasConocimiento = new List<AreaConocimiento>();
                            ExpedienteEscolarCtrl expCtrl = new ExpedienteEscolarCtrl();
                            var alumno = (Alumno)userSession.CurrentAlumno;
                            PruebaDinamica pruebaDinamica = new PruebaDinamica();
                            var interesesAspirante = expCtrl.RetrieveInteresesAspirante(dctx, alumno, pruebaDinamica);
                            userSession.CurrentAlumno.AreasConocimiento = (from area in interesesAspirante
                                                                           select new AreaConocimiento()
                                                                           {
                                                                               AreaConocimentoID = area.clasificador.ClasificadorID,
                                                                               Nombre = area.clasificador.Nombre,
                                                                               Descripcion = area.clasificador.Descripcion
                                                                           }).GroupBy(x => x.AreaConocimentoID).Select(grp => grp.First()).OrderBy(x => x.AreaConocimentoID).ToList();


                            userSession.CurrentEscuela = escuelaCtrl.LastDataRowToEscuela(escuelaCtrl.Retrieve(dctx, licenciaEscuela.Escuela));
                            userSession.CurrentUsuarioSocial = usuarioSocialCtrl.LastDataRowToUsuarioSocial(usuarioSocialCtrl.Retrieve(dctx, licenciaAlumno.UsuarioSocial));
                            userSession.SocialHub = socialHubCtrl.LastDataRowToSocialHub(socialHubCtrl.RetrieveSocialHubUsuario(dctx, new SocialHub { SocialProfileType = ESocialProfileType.USUARIOSOCIAL, SocialProfile = new UsuarioSocial { UsuarioSocialID = userSession.CurrentUsuarioSocial.UsuarioSocialID } }));
                            userSession.CurrentCicloEscolar = licenciaEscuela.CicloEscolar;

                            List<GrupoSocial> grupos = grupoSocialCtrl.RetrieveGruposSocialSocialHub(dctx, userSession.SocialHub);

                            if (grupos.Count <= 0)
                            {
                                userSession.Logout();
                                return "No perteneces a ningun grupo escolar";
                            }

                            GrupoCicloEscolar grupoCicloEscolar = grupoCicloEscolarCtrl.RetrieveGrupoCicloEscolar(dctx, userSession.CurrentAlumno, userSession.CurrentCicloEscolar);

                            if (grupoCicloEscolar.GrupoCicloEscolarID == null)
                                return "No perteneces a ningun grupo escolar";

                            UsuarioPrivilegios usuarioPrivilegios = new UsuarioEscolarPrivilegios();
                            usuarioPrivilegios.Usuario = usuario;
                            (usuarioPrivilegios as UsuarioEscolarPrivilegios).Escuela = userSession.CurrentEscuela;
                            (usuarioPrivilegios as UsuarioEscolarPrivilegios).CicloEscolar = userSession.CurrentCicloEscolar;

                            DataSet dsPrivilegios = usuarioPrivilegiosCtrl.Retrieve(dctx, usuarioPrivilegios);
                            if (dsPrivilegios.Tables[0].Rows.Count < 1)
                                return "No tienes privilegios para acceder al portal";

                            usuarioPrivilegios = usuarioPrivilegiosCtrl.RetrieveComplete(dctx, usuarioPrivilegios);

                            if (usuarioPrivilegios.UsuarioAccesos.Count < 1)
                                return "No tienes privilegios para acceder al portal";

                            userSession.UsuarioPrivilegios = usuarioPrivilegios;
                            userSession.CurrentGrupoCicloEscolar = grupoCicloEscolar;
                            userSession.CurrentGrupoSocial = grupos.First();
                            userSession.CurrentUser = usuario;
                            userSession.Perfil = "A";
                            userSession.LoggedIn = true;
                            userSession.ModulosFuncionales = licenciaEscuela.ModulosFuncionales;
                            if (tipoUsuario == "pruebas")
                                redirector.GoToPruebas(true);

                            if (tipoUsuario == "bullying")
                                redirector.GoToValidarBateriaBullying(true);
                            return "";
                        }
                        else
                        {// es docente, se agrega la licencia en la lista, siempre y cuando tenga privilegios
                            if (licencia.Tipo == ETipoLicencia.DOCENTE && tipoUsuario == "ori")
                            {

                                UsuarioPrivilegios usuarioPrivilegios = new UsuarioEscolarPrivilegios();
                                usuarioPrivilegios.Usuario = usuario;
                                (usuarioPrivilegios as UsuarioEscolarPrivilegios).Escuela = licenciaEscuela.Escuela;
                                (usuarioPrivilegios as UsuarioEscolarPrivilegios).CicloEscolar = licenciaEscuela.CicloEscolar;

                                DataSet dsPrivilegios = usuarioPrivilegiosCtrl.Retrieve(dctx, usuarioPrivilegios);
                                if (dsPrivilegios.Tables[0].Rows.Count < 1)
                                    continue;

                                usuarioPrivilegios = usuarioPrivilegiosCtrl.RetrieveComplete(dctx, usuarioPrivilegios);

                                if (usuarioPrivilegios.UsuarioAccesos.Count < 1)
                                    continue;

                                //solo insertamos si el usuario tiene privilegios asignados
                                licenciasDocente.Add(licenciaEscuela);
                            }
                            else
                            {
                                string portal = tipoUsuario == "ori" ? "orientadores" : "aspirantes";
                                userSession.LoggedIn = false;
                                userSession.CurrentUser = null;
                                userSession.CurrentUsuarioSocial = null;
                                userSession.SocialHub = null;
                                userSession.CurrentAlumno = null;
                                userSession.CurrentDocente = null;
                                userSession.Contrato = null;
                                userSession.CurrentCicloEscolar = null;
                                userSession.CurrentGrupoCicloEscolar = null;
                                userSession.ModulosFuncionales = null;
                                return "No tienes licencias para acceder al portal de " + portal + ".";
                            }
                        }
                    }
                    #endregion

                    if (licenciasDocente.Count <= 0)
                        return "No tienes licencias para acceder al portal.";
                    //se agregan los datos para el docente en session
                    userSession.LicenciasDocente = licenciasDocente;
                    userSession.CurrentUser = usuario;
                    userSession.LoggedIn = true;
                    userSession.Perfil = "D";

                    //primero preguntar si esta confirmado si es false, mandar a confirmar datos, si es true continuar preguntando si acepto terminos

                    LicenciaEscuela MilicenciaEscuela = userSession.LicenciasDocente.First();
                    LicenciaDocente licenciaDocente = (LicenciaDocente)MilicenciaEscuela.ListaLicencia.First(item => item.Tipo == ETipoLicencia.DOCENTE);
                    userSession.CurrentDocente = docenteCtrl.LastDataRowToDocente(docenteCtrl.Retrieve(dctx, licenciaDocente.Docente));
                    if (!(bool)userSession.CurrentDocente.EstatusIdentificacion)
                    {
                        redirector.GoToConfirmarMaestro(false);
                        return "";
                    }

                    //si esta confirmado preguntar si acepto terminos

                    //if ((bool)usuario.AceptoTerminos)
                    //    redirector.GoToCambiarEscuela(false);
                    //else
                    //    redirector.GoToAceptarTerminos(false);
                    if (tipoUsuario == "pruebas")
                        redirector.GoToPruebas(true);

                    if (tipoUsuario == "bullying")
                        redirector.GoToValidarBateriaBullying(true);

                    return "";

                }
                else
                    return "Usuario y contraseña incorrectos";
            }
            else
                return "Usuario y contraseña incorrectos.";

        }

        public void AutoLogin(IDataContext dctx, string username)
        {
            Usuario usuario = new Usuario { NombreUsuario = username.Trim() };

            DataSet dsUsuario = usuarioCtrl.Retrieve(dctx, usuario);
            if (dsUsuario.Tables[0].Rows.Count > 0)
            {
                usuario = usuarioCtrl.LastDataRowToUsuario(dsUsuario);
            }
            else
            {
                //TODO: ERROR
            }
        }

        /// <summary>
        /// Finallizar la session y redireccionar a la pagina de logueo
        /// </summary>
        public void Logout()
        {
            bool portalSocial = true;

            if (userSession.CurrentDocente != null)
                portalSocial = false;

            userSession.LoggedIn = false;
            userSession.CurrentUser = null;
            userSession.CurrentUsuarioSocial = null;
            userSession.SocialHub = null;
            userSession.CurrentAlumno = null;
            userSession.CurrentDocente = null;
            userSession.Contrato = null;
            userSession.CurrentCicloEscolar = null;
            userSession.CurrentGrupoCicloEscolar = null;
            userSession.ModulosFuncionales = null;

            if (portalSocial)
                redirector.GoToLoginPageAspirante(false);
            else
                redirector.GoToLoginPageOrientador(false);
        }
    }
}
