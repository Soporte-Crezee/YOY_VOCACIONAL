using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POV.Web.DTO;
using GP.SocialEngine.BO;
using GP.SocialEngine.Utils;
using GP.SocialEngine.Service;
using POV.Seguridad.Service;
using POV.Seguridad.Utils;
using POV.Seguridad.BO;
using Framework.Base.DataAccess;
using POV.Reactivos.BO;
using POV.Reactivos.Service;
using POV.Core.RedSocial.Interfaces;
using POV.Core.RedSocial.Implement;
using POV.ContenidosDigital.BO;
using POV.ContenidosDigital.Service;
using System.Data;
using POV.Licencias.BO;
using POV.Licencias.Service;
using POV.CentroEducativo.BO;
using System.Web;
using System.IO;
using POV.Comun.Service;

namespace POV.Web.DTO.Services
{
    public class PublicacionDTOCtrl
    {
        private IDataContext dctx;

        private IUserSession userSession;
        private PublicacionCtrl publicacionCtrl;
        private GrupoSocialCtrl grupoSocialCtrl;
        private SocialHubCtrl socialHubCtrl;
        private UsuarioSocialCtrl usuarioSocialCtrl;

        public PublicacionDTOCtrl()
        {
            dctx = new DataContext((new DataProviderFactory()).GetDataProvider("POV"));
            publicacionCtrl = new PublicacionCtrl();
            grupoSocialCtrl = new GrupoSocialCtrl();
            socialHubCtrl = new SocialHubCtrl();
            userSession = new UserSession();
            usuarioSocialCtrl = new UsuarioSocialCtrl();
        }

        public List<publicaciondto> GetPublicacionesMuroDocente(publicaciondtoinput dto)
        {
            List<Int64?> murosContactos = new List<Int64?>();

            murosContactos.Add(dto.socialhubid);

            List<IPrivacidad> privacidades = new List<IPrivacidad>();

            privacidades.Add(userSession.CurrentGrupoSocial);

            List<publicaciondto> listPublicaciones = new List<publicaciondto>();
            if (murosContactos.Count > 0)
            {
                try
                {
                    dto.sort = "FechaPublicacion";
                    dto.order = "DESC";
                    dto.estatus = 1;
                    Dictionary<string, string> parametros = dtoInputToParemeters(dto);
                    parametros.Add("SoloPropietario", "1");

                    List<Publicacion> pubs = publicacionCtrl.RetrieveListPublicacionMuros(dctx, dto.pagesize, dto.currentpage, dto.sort, dto.order, parametros, murosContactos, privacidades);
                    foreach (Publicacion publicacion in pubs)
                    {
                        publicaciondto pub = PublicacionToDto(publicacion, false);
                        listPublicaciones.Add(pub);
                    }
                }
                catch (Exception ex)
                {
                    POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
                    throw ex;
                }

            }
            return listPublicaciones;
        }
        public List<publicaciondto> GetPublicaciones(publicaciondtoinput dto)
        {
            List<publicaciondto> listPublicaciones = new List<publicaciondto>();

            dto.sort = "FechaPublicacion";
            dto.order = "DESC";
            try
            {
                List<IPrivacidad> privacidades = new List<IPrivacidad>();

                privacidades.Add(userSession.CurrentGrupoSocial);

                List<Publicacion> pubs = publicacionCtrl.RetrieveListPublicaciones(dctx, dto.pagesize, dto.currentpage, dto.sort, dto.order, dtoInputToParemeters(dto), privacidades);
                foreach (Publicacion publicacion in pubs)
                {
                    publicaciondto pub = PublicacionToDto(publicacion, false);
                    listPublicaciones.Add(pub);
                }
            }
            catch (Exception ex)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
                throw ex;
            }
            return listPublicaciones;
        }

        public List<publicaciondto> GetSugerencias(publicaciondtoinput dto)
        {
            List<publicaciondto> listPublicaciones = new List<publicaciondto>();

            dto.sort = "FechaPublicacion";
            dto.order = "DESC";
            try
            {
                List<IPrivacidad> privacidades = new List<IPrivacidad>();
                if (userSession.IsAlumno())
                {
                    privacidades.Add(userSession.CurrentGrupoSocial);
                }
                else
                {
                    List<GrupoSocial> grupos = grupoSocialCtrl.RetrieveGruposSocialSocialHub(dctx, userSession.SocialHub);

                    foreach (GrupoSocial gs in grupos)
                    {
                        privacidades.Add(gs);
                    }

                }

                List<Publicacion> pubs = publicacionCtrl.RetrieveListPublicaciones(dctx, dto.pagesize, dto.currentpage, dto.sort, dto.order, dtoInputToParemeters(dto), privacidades);
                foreach (Publicacion publicacion in pubs)
                {
                    publicaciondto pub = PublicacionToDto(publicacion, false);
                    listPublicaciones.Add(pub);
                }
            }
            catch (Exception ex)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
                throw ex;
            }
            return listPublicaciones;
        }

        public List<publicaciondto> GetDudas(publicaciondtoinput dto)
        {
            // se obtiene los usuarios sociales
            List<Int64?> murosContactos = new List<Int64?>();

            murosContactos.Add(dto.socialhubid);

            long universidadID = 0;
            if (userSession.IsDocente())
                universidadID = (long)userSession.CurrentUser.UniversidadId;
            else
                universidadID = (long)userSession.CurrentAlumno.Universidades[0].UniversidadID;

            GrupoSocial grupoSocial = grupoSocialCtrl.RetrieveComplete(dctx, new GrupoSocial { GrupoSocialID = userSession.CurrentGrupoSocial.GrupoSocialID }, userSession.CurrentAlumno.AreasConocimiento, null, universidadID, true);


            List<IPrivacidad> privacidades = new List<IPrivacidad>();

            privacidades.Add(userSession.CurrentGrupoSocial);

            List<publicaciondto> listPublicaciones = new List<publicaciondto>();
            if (murosContactos.Count > 0)
            {

                dto.sort = "FechaPublicacion";
                dto.order = "DESC";
                dto.estatus = 1;
                Dictionary<string, string> parametros = dtoInputToParemeters(dto);

                List<Publicacion> pubs = publicacionCtrl.RetrieveListPublicacionMuros(dctx, dto.pagesize, dto.currentpage, dto.sort, dto.order, parametros, murosContactos, privacidades);
                foreach (Publicacion publicacion in pubs)
                {
                    publicaciondto pub = PublicacionToDto(publicacion, false);
                    listPublicaciones.Add(pub);
                }
            }
            return listPublicaciones;
        }

        public publicaciondto GetPublicacion(publicaciondto dto)
        {
            PublicacionCtrl publicacionCtrl = new PublicacionCtrl();
            Publicacion publicacion = DtoToPublicacion(dto);

            Publicacion publicacioncompleto = publicacionCtrl.RetrieveComplete(dctx, publicacion);
            publicaciondto pub = PublicacionToDto(publicacioncompleto, dto.complete.Value);

            return pub;
        }

        public List<publicaciondto> GetPublicacionesContactos(publicaciondtoinput dto, bool onlyModerador)
        {
            // se obtiene los usuarios sociales
            List<Int64?> usuariosSociales = new List<Int64?>();
            GrupoSocial grupoSocial;
            if (userSession.IsDocente())
            {
                grupoSocial = grupoSocialCtrl.RetrieveComplete(dctx, new GrupoSocial { GrupoSocialID = userSession.CurrentGrupoSocial.GrupoSocialID }, userSession.CurrentAlumno.AreasConocimiento, userSession.CurrentDocente.DocenteID, (long)userSession.CurrentUser.UniversidadId);
            }
            else
            {
                grupoSocial = grupoSocialCtrl.RetrieveComplete(dctx, new GrupoSocial { GrupoSocialID = userSession.CurrentGrupoSocial.GrupoSocialID }, userSession.CurrentAlumno.AreasConocimiento, null, (long)userSession.CurrentAlumno.Universidades[0].UniversidadID, true);
            }
            foreach (UsuarioGrupo usuarioGrupo in grupoSocial.ListaUsuarioGrupo)
            {
                if (onlyModerador && (bool)usuarioGrupo.EsModerador)
                    usuariosSociales.Add(usuarioGrupo.UsuarioSocial.UsuarioSocialID);
                else if (!onlyModerador && !(bool)usuarioGrupo.EsModerador)
                    usuariosSociales.Add(usuarioGrupo.UsuarioSocial.UsuarioSocialID);
            }

            List<IPrivacidad> privacidades = new List<IPrivacidad>();

            privacidades.Add(userSession.CurrentGrupoSocial);

            List<publicaciondto> listPublicaciones = new List<publicaciondto>();
            try
            {
                if (usuariosSociales.Count > 0)
                {

                    dto.sort = "FechaPublicacion";
                    dto.order = "DESC";
                    dto.estatus = 1;

                    List<Publicacion> pubs = publicacionCtrl.RetrieveListPublicacionesContactos(dctx, dto.pagesize, dto.currentpage, dto.sort, dto.order, dtoInputToParemeters(dto), usuariosSociales, privacidades);
                    foreach (Publicacion publicacion in pubs)
                    {
                        publicaciondto pub = PublicacionToDto(publicacion, false);
                        listPublicaciones.Add(pub);
                    }
                }
            }
            catch (Exception ex)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
                throw ex;
            }
            return listPublicaciones;
        }

        public List<publicaciondto> GetPublicacionesMurosContactos(publicaciondtoinput dto, bool onlyModerador)
        {
            // se obtiene los usuarios sociales
            List<Int64?> murosContactos = new List<Int64?>();
            List<publicaciondto> listPublicaciones = new List<publicaciondto>();
            try
            {

                long universidadID = 0;
                if (userSession.IsDocente())
                    universidadID = (long)userSession.CurrentUser.UniversidadId;
                else
                    universidadID = (long)userSession.CurrentAlumno.Universidades[0].UniversidadID;

                GrupoSocial grupoSocial = grupoSocialCtrl.RetrieveComplete(dctx, new GrupoSocial { GrupoSocialID = userSession.CurrentGrupoSocial.GrupoSocialID }, userSession.CurrentAlumno.AreasConocimiento, null, universidadID);

                var usrSocialDocente = new LicenciaEscuelaCtrl().RetrieveUsuarioSocial(dctx, new Docente() { DocenteID = userSession.CurrentDocente.DocenteID });

                List<UsuarioGrupo> lstDocente = grupoSocial.ListaUsuarioGrupo.Where(x => x.UsuarioSocial.UsuarioSocialID == usrSocialDocente.UsuarioSocialID).ToList();

                foreach (UsuarioGrupo usuarioGrupo in lstDocente)
                {
                    SocialHub socialHub = socialHubCtrl.LastDataRowToSocialHub(
                        socialHubCtrl.RetrieveSocialHubUsuario(dctx,
                        new SocialHub
                        {
                            SocialProfile = usuarioGrupo.UsuarioSocial,
                            SocialProfileType = ESocialProfileType.USUARIOSOCIAL
                        }
                        ));

                    if (onlyModerador && (bool)usuarioGrupo.EsModerador)
                    {
                        murosContactos.Add(socialHub.SocialHubID);
                    }
                    else if (!onlyModerador && !(bool)usuarioGrupo.EsModerador)
                    {
                        murosContactos.Add(socialHub.SocialHubID);
                    }
                }

                List<IPrivacidad> privacidades = new List<IPrivacidad>();

                privacidades.Add(userSession.CurrentGrupoSocial);


                if (murosContactos.Count > 0)
                {

                    dto.sort = "FechaPublicacion";
                    dto.order = "DESC";
                    dto.estatus = 1;
                    Dictionary<string, string> parametros = dtoInputToParemeters(dto);
                    parametros.Add("SoloPropietario", "1");

                    List<Publicacion> pubs = publicacionCtrl.RetrieveListPublicacionMuros(dctx, dto.pagesize, dto.currentpage, dto.sort, dto.order, parametros, murosContactos, privacidades);
                    foreach (Publicacion publicacion in pubs)
                    {
                        publicaciondto pub = PublicacionToDto(publicacion, false);
                        listPublicaciones.Add(pub);
                    }
                }
            }
            catch (Exception ex)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
                throw ex;
            }
            return listPublicaciones;
        }

        private Dictionary<string, string> dtoInputToParemeters(publicaciondtoinput dto)
        {
            Dictionary<string, string> parametros = new Dictionary<string, string>();
            parametros.Add("EsActivo", "1");

            if (dto.socialhubid > 0)
                parametros.Add("SocialHubID", dto.socialhubid.ToString());
            if (dto.estatus != null)
                parametros.Add("Estatus", dto.estatus.ToString());

            return parametros;
        }
        public publicaciondto SavePublicacion(publicaciondto dto)
        {
            try
            {
                //creacion de la publicacion
                Publicacion publicacion = DtoToPublicacion(dto);
                publicacion.PublicacionID = System.Guid.NewGuid();
                publicacion.FechaPublicacion = System.DateTime.Now;
                publicacion.Ranking = new Ranking { RankingID = Guid.NewGuid() };


                //privacidad de la publicacion
                List<IPrivacidad> privacidades = new List<IPrivacidad>();

                //Validad de donde se toma la universidad
                long universidadID = 0;
                bool isAlumno = true;
                if (userSession.IsDocente())
                {
                    universidadID = (long)userSession.CurrentUser.UniversidadId;
                    isAlumno = false;
                }
                else
                    universidadID = (long)userSession.CurrentAlumno.Universidades[0].UniversidadID;

                //si es alumno se le asigna su grupo en sesion, si es docente se asigna el grupo propietario del muro
                if (userSession.IsAlumno())
                {
                    if (publicacion.TipoPublicacion == ETipoPublicacion.APPSOCIAL) throw new ArgumentException("No tiene los permisos para publicar contenido.");

                    privacidades.Add(userSession.CurrentGrupoSocial);
                }
                else
                {
                    if (publicacion.SocialHub.SocialHubID == userSession.SocialHub.SocialHubID)
                    {
                        privacidades.Add(userSession.CurrentGrupoSocial);
                    }
                    else
                    {
                        List<GrupoSocial> grupos = grupoSocialCtrl.RetrieveGruposSocialSocialHub(dctx, userSession.SocialHub);
                        SocialHub socialHub = socialHubCtrl.LastDataRowToSocialHub(socialHubCtrl.Retrieve(dctx, publicacion.SocialHub));
                        foreach (GrupoSocial gs in grupos)
                        {
                            UsuarioGrupo usuarioGrupo = grupoSocialCtrl.RetrieveFriend(dctx, gs, socialHub.SocialProfile as UsuarioSocial, userSession.CurrentAlumno.AreasConocimiento, null, universidadID);
                            if (usuarioGrupo != null)
                            {
                                privacidades.Add(gs);
                                break;
                            }
                        }
                    }

                }

                publicacion.Privacidades = privacidades;

                PublicacionCtrl publicacionCtrl = new PublicacionCtrl();

                publicacionCtrl.InsertComplete(dctx, publicacion, userSession.CurrentAlumno.AreasConocimiento, universidadID, isAlumno);
                publicacion = publicacionCtrl.LastDataRowToPublicacion(publicacionCtrl.Retrieve(dctx, publicacion));
                publicacion = publicacionCtrl.RetrieveComplete(dctx, publicacion);


                UsuarioCtrl usuarioCtrl = new UsuarioCtrl();
                LicenciaEscuelaCtrl licenciaEscuelaCtrl = new LicenciaEscuelaCtrl();
                CorreoCtrl correoCtrl = new CorreoCtrl();
                //Solo se envia la notificación cuando se publica a otro muro, y no asi mismo.
                if (publicacion.SocialHub != null && publicacion.SocialHub.SocialHubID != null)
                {
                    if (publicacion.SocialHub.SocialHubID != userSession.SocialHub.SocialHubID)
                    {
                        #region Datos del destinatario
                        var destinatario = publicacion.SocialHub.SocialProfile as UsuarioSocial;
                        var userSocial = licenciaEscuelaCtrl.RetrieveUsuario(dctx, new UsuarioSocial { UsuarioSocialID = destinatario.UsuarioSocialID }, false);
                        Usuario usuario = usuarioCtrl.LastDataRowToUsuario(usuarioCtrl.Retrieve(dctx, new Usuario() { UsuarioID = userSocial.UsuarioID }));
                        #endregion

                        //Consultar el usuario, para obtener el Correo
                        #region variables
                        string urllogo = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgLogo"];
                        string urlimgbackground = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgBackground"];
                        string urlimgface = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgFace"];
                        string urlimgtwit = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgTwit"];
                        string urlimglinked = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgLinked"];
                        string urlimginstagram = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgInstagram"];
                        const string altimg = "YOY - Email";
                        const string titulo = "Publicación";
                        string dominio = String.Format("{0}://{1}:{2}/", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.Port);
                        string linkportal = System.Configuration.ConfigurationManager.AppSettings["POVUrlAspirante"];
                        #endregion variables

                        #region Template HTML
                        string cuerpo = string.Empty;
                        var serverPath = System.Web.Hosting.HostingEnvironment.MapPath(System.Configuration.ConfigurationManager.AppSettings["POVUrlEmailNotificacionMP"]);
                        using (StreamReader reader = new StreamReader(serverPath))
                        {
                            cuerpo = reader.ReadToEnd();
                        }

                        cuerpo = cuerpo.Replace("{title}", titulo);
                        cuerpo = cuerpo.Replace("{altimg}", altimg);
                        cuerpo = cuerpo.Replace("{urlimgbackground}", urlimgbackground);
                        cuerpo = cuerpo.Replace("{urllogo}", urllogo);
                        cuerpo = cuerpo.Replace("{linkportal}", linkportal);
                        cuerpo = cuerpo.Replace("{mensaje}", "Ha sido publicado en su muro un mensaje de {remitente}");
                        cuerpo = cuerpo.Replace("{remitente}", publicacion.UsuarioSocial.ScreenName);
                        cuerpo = cuerpo.Replace("{urlimgface}", urlimgface);
                        cuerpo = cuerpo.Replace("{urlimgtwit}", urlimgtwit);
                        cuerpo = cuerpo.Replace("{urlimglinked}", urlimglinked);
                        cuerpo = cuerpo.Replace("{urlimginstagram}", urlimginstagram);
                        cuerpo = cuerpo.Replace("{destinatario}", destinatario.ScreenName);
                        #endregion Template HTML

                        #region Envio del Correo
                        List<string> tos = new List<string>();
                        if (!String.IsNullOrEmpty(usuario.Email))
                        {
                            tos.Add(usuario.Email);
                            try
                            {
                                correoCtrl.sendMessage(tos, "YOY - Publicación", cuerpo, null, new List<string>(), new List<string>());
                            }
                            catch { }
                        }
                        #endregion
                    }
                }

                publicaciondto pub = PublicacionToDto(publicacion, true);

                return pub;
            }
            catch (Exception ex)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
                throw ex;
            }
        }

        public publicaciondto SaveSugerencia(publicaciondto dto)
        {
            try
            {
                //creacion de la publicacion
                Publicacion publicacion = DtoToPublicacion(dto);
                publicacion.PublicacionID = System.Guid.NewGuid();
                publicacion.UsuarioSocial = userSession.CurrentUsuarioSocial;
                publicacion.FechaPublicacion = System.DateTime.Now;
                publicacion.Ranking = new Ranking { RankingID = Guid.NewGuid() };
                List<IPrivacidad> privacidades = new List<IPrivacidad>();

                if (userSession.IsAlumno())
                    privacidades.Add(userSession.CurrentGrupoSocial);
                else
                {
                    List<GrupoSocial> grupos = grupoSocialCtrl.RetrieveGruposSocialSocialHub(dctx, userSession.SocialHub);

                    foreach (GrupoSocial gs in grupos)
                    {
                        privacidades.Add(gs);
                    }

                }

                publicacion.Privacidades = privacidades;
                PublicacionCtrl publicacionCtrl = new PublicacionCtrl();

                long universidadID = 0;
                if (userSession.IsDocente())
                    universidadID = (long)userSession.CurrentUser.UniversidadId;
                else
                    universidadID = (long)userSession.CurrentAlumno.Universidades[0].UniversidadID;

                publicacionCtrl.InsertComplete(dctx, publicacion, userSession.CurrentAlumno.AreasConocimiento, universidadID);
                publicacion = publicacionCtrl.LastDataRowToPublicacion(publicacionCtrl.Retrieve(dctx, publicacion));
                publicacion = publicacionCtrl.RetrieveComplete(dctx, publicacion);

                publicaciondto pub = PublicacionToDto(publicacion, true);

                return pub;
            }
            catch (Exception ex)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
                throw ex;
            }
        }

        public publicaciondto MasUnoPublicacion(publicaciondto dto)
        {
            try
            {
                Publicacion publicacion = DtoToPublicacion(dto);

                PublicacionCtrl publicacionCtrl = new PublicacionCtrl();

                RankingCtrl ratingCtrl = new RankingCtrl();
                UsuarioSocialRankingCtrl usuarioSocialRankingCtrl = new UsuarioSocialRankingCtrl();

                publicacion = publicacionCtrl.RetrieveComplete(dctx, publicacion);

                bool exist = false;

                exist = publicacion.Ranking.ListaPuntuaciones.Exists(item => item.UsuarioSocial.UsuarioSocialID == userSession.CurrentUsuarioSocial.UsuarioSocialID);

                if (!exist)
                {
                    UsuarioSocialRanking usuarioSocialRanking = new UsuarioSocialRanking();
                    usuarioSocialRanking.FechaRegistro = DateTime.Now;
                    usuarioSocialRanking.AsignarPuntuacion(dto.vote.Value);
                    usuarioSocialRanking.UsuarioSocial = new UsuarioSocial { UsuarioSocialID = userSession.CurrentUsuarioSocial.UsuarioSocialID };
                    usuarioSocialRanking.RankingID = publicacion.Ranking.RankingID;
                    usuarioSocialRankingCtrl.InsertCompleteForPublicacion(dctx, usuarioSocialRanking, publicacion);

                }

                publicacion = publicacionCtrl.RetrieveComplete(dctx, new Publicacion { PublicacionID = publicacion.PublicacionID });

                dto = PublicacionToDto(publicacion, false);

                return dto;
            }
            catch (Exception ex)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
                throw ex;
            }
        }

        private Publicacion DtoToPublicacion(publicaciondto dto)
        {
            Publicacion publicacion = new Publicacion();

            if (dto.contenido != null)
            {
                if (dto.contenido.Trim().Length > 400)
                {
                    publicacion.Contenido = dto.contenido.Trim().Substring(0, 400);
                }
                else
                {
                    publicacion.Contenido = dto.contenido.Trim();
                }
            }

            publicacion.Estatus = true;

            if (dto.socialhubid != null)
                publicacion.SocialHub = new SocialHub { SocialHubID = dto.socialhubid };
            if (dto.usuariosocialid != null)
                publicacion.UsuarioSocial = new UsuarioSocial { UsuarioSocialID = dto.usuariosocialid };
            else
                publicacion.UsuarioSocial = new UsuarioSocial();
            if (string.IsNullOrEmpty(dto.rankingid))
            {
                publicacion.Ranking = new Ranking();
                publicacion.Ranking.RankingID = null;
            }
            else
            {
                publicacion.Ranking = new Ranking();
                publicacion.Ranking.RankingID = Guid.Parse(dto.rankingid);
            }
            if (dto.publicacionid != null)
                publicacion.PublicacionID = new Guid(dto.publicacionid);

            if (dto.tipo != null)
            {
                if (dto.tipo == (short)ETipoPublicacion.REACTIVO)
                {
                    publicacion.TipoPublicacion = ETipoPublicacion.REACTIVO;

                    if (dto.reactivo != null)
                    {
                        Reactivo reactivo = new Reactivo();
                        reactivo.ReactivoID = Guid.Parse(dto.reactivo.reactivoid);

                        publicacion.AppSocial = reactivo;
                    }
                }
                else if (dto.tipo == (short)ETipoPublicacion.APPSOCIAL)
                {
                    publicacion.TipoPublicacion = ETipoPublicacion.APPSOCIAL;
                    if (!string.IsNullOrEmpty(dto.tipocompartido))
                    {
                        if (dto.tipocompartido.CompareTo("reactivo") == 0)
                        {
                            Reactivo reactivo = new Reactivo();
                            reactivo.ReactivoID = Guid.Parse(dto.compartidoid);

                            publicacion.AppSocial = reactivo;
                        }
                        else if (dto.tipocompartido.CompareTo("contenido") == 0)
                        {
                            ContenidoDigital contenido = new ContenidoDigital();
                            contenido.ContenidoDigitalID = long.Parse(dto.compartidoid);

                            publicacion.AppSocial = contenido;
                        }
                    }
                }
                else if (dto.tipo == (short)ETipoPublicacion.TEXTO)
                    publicacion.TipoPublicacion = ETipoPublicacion.TEXTO;
            }

            return publicacion;
        }
        private publicaciondto PublicacionToDto(Publicacion publicacion, bool complete)
        {
            publicaciondto dto = new publicaciondto();
            dto.renderfor = false;
            dto.renderdelete = false;
            dto.renderreporteabuso = false;
            dto.complete = complete;
            dto.publicacionid = publicacion.PublicacionID.ToString();
            UsuarioSocial currentUsuarioSocial = userSession.CurrentUsuarioSocial;


            UsuarioGrupoCtrl usuarioGrupoCtrl = new UsuarioGrupoCtrl();

            bool isLoggedDocente = !userSession.IsAlumno();

            if (publicacion.FechaPublicacion != null)
            {
                dto.fechapublicacion = String.Format("{0:dd/MM/yyyy a la(\\s) HH:mm}", publicacion.FechaPublicacion.Value);
                dto.fechaformated = new TimeAgoDateFormat().Format(publicacion.FechaPublicacion.Value);
            }

            dto.contenido = publicacion.Contenido;

            if (publicacion.UsuarioSocial.UsuarioSocialID != null)
            {
                dto.usuariosocialid = (long)publicacion.UsuarioSocial.UsuarioSocialID;
                dto.renderreporteabuso = false; // publicacion.UsuarioSocial.UsuarioSocialID != userSession.CurrentUsuarioSocial.UsuarioSocialID || !isLoggedDocente;
                long universidadID = 0;
                if (userSession.IsDocente())
                    universidadID = (long)userSession.CurrentUser.UniversidadId;
                else
                    universidadID = (long)userSession.CurrentAlumno.Universidades[0].UniversidadID;
                UsuarioGrupo usuarioGrupo = grupoSocialCtrl.RetrieveFriend(dctx, currentUsuarioSocial, publicacion.UsuarioSocial, userSession.CurrentAlumno.AreasConocimiento, universidadID);
                if (publicacion.SocialHub.SocialProfileType == ESocialProfileType.USUARIOSOCIAL)
                {

                    UsuarioSocial propietarioMuro = (publicacion.SocialHub.SocialProfile as UsuarioSocial);
                    //si la publicacion se realiza en un muro diferente del que publica, se muestra la leyenda "para"
                    if (propietarioMuro.UsuarioSocialID != publicacion.UsuarioSocial.UsuarioSocialID)
                    {
                        propietarioMuro = usuarioSocialCtrl.LastDataRowToUsuarioSocial(usuarioSocialCtrl.Retrieve(dctx, propietarioMuro));
                        dto.renderfor = true;
                        dto.destinatarioid = propietarioMuro.UsuarioSocialID;
                        dto.destinatarionombre = propietarioMuro.ScreenName;
                        //verificamos si es un docente el propietario
                        UsuarioGrupo usuarioGrupoPropietario = grupoSocialCtrl.RetrieveFriend(dctx, propietarioMuro, propietarioMuro, userSession.CurrentAlumno.AreasConocimiento, universidadID);
                        if (usuarioGrupoPropietario != null)
                            dto.renderforlink = !isLoggedDocente || !(bool)usuarioGrupoPropietario.EsModerador;
                        else
                            dto.renderforlink = false;
                    }

                    if (usuarioGrupo != null)
                        if ((bool)usuarioGrupo.EsModerador) // el que publico es moderador?
                        {
                            //la publico un docente, por lo tanto solo podra eliminarlo el mismo
                            dto.renderdelete = publicacion.UsuarioSocial.UsuarioSocialID == currentUsuarioSocial.UsuarioSocialID;
                            //la publicación de un docente no podrá ser reportada
                            dto.renderreporteabuso = false;
                        }

                        else
                        //publico un alumno, por lo tanto solo se podra eliminar si fue publicada en su muro o el alumno la publico
                        // o un docente esta logueado
                        {
                            dto.renderdelete = publicacion.UsuarioSocial.UsuarioSocialID == currentUsuarioSocial.UsuarioSocialID ||
                                   publicacion.SocialHub.SocialHubID == userSession.SocialHub.SocialHubID || isLoggedDocente;

                            //la publicación no es de un docente, podrá ser reportada siempre que no le pertenece al usuario actual o un docente
                            dto.renderreporteabuso = false; // publicacion.UsuarioSocial.UsuarioSocialID != currentUsuarioSocial.UsuarioSocialID && !isLoggedDocente;
                        }

                }
                else if (publicacion.SocialHub.SocialProfileType == ESocialProfileType.REACTIVO)
                {
                    //sugerencias en el muro de reactivo solo pueden eliminarlas los docentes
                    if (usuarioGrupo != null)
                        if ((bool)usuarioGrupo.EsModerador) // el que publico es moderador?
                        {
                            //la publico un docente, por lo tanto solo podrá eliminarlo el mismo
                            dto.renderdelete = publicacion.UsuarioSocial.UsuarioSocialID == currentUsuarioSocial.UsuarioSocialID;
                            //la publico un docente, por lo tanto no podrá ser reportada
                            dto.renderreporteabuso = false;
                        }
                        else
                        {
                            dto.renderdelete = isLoggedDocente;
                            //la publicación de un reactivo no es reportada
                            dto.renderreporteabuso = false;
                        }

                }
                if (usuarioGrupo != null)
                    dto.renderlink = !isLoggedDocente || !(bool)usuarioGrupo.EsModerador;
                else
                    dto.renderlink = false;
            }
            if (publicacion.SocialHub.SocialHubID != null)
                dto.socialhubid = (long)publicacion.SocialHub.SocialHubID;

            if (publicacion.UsuarioSocial != null)
                dto.usuariosocialnombre = publicacion.UsuarioSocial.ScreenName;

            if (publicacion.Ranking != null && publicacion.Ranking.RankingID != null)
            {
                dto.rankingid = publicacion.Ranking.RankingID.ToString();
                dto.numvotes1 = publicacion.Ranking.ObtenerPuntuacion(EPuntuacionRanking.EXCELENTE);
                dto.numvotes2 = publicacion.Ranking.ObtenerPuntuacion(EPuntuacionRanking.BIEN);
                dto.numvotes3 = publicacion.Ranking.ObtenerPuntuacion(EPuntuacionRanking.REGULAR);

            }

            if (publicacion.TipoPublicacion != null)
            {
                dto.tipo = (short)publicacion.TipoPublicacion;

                if (publicacion.TipoPublicacion == ETipoPublicacion.REACTIVO && publicacion.AppSocial != null)
                {
                    ReactivoCtrl reactivoCtrl = new ReactivoCtrl();
                    ReactivoDTOCtrl reactivoDTOCtrl = new ReactivoDTOCtrl();
                    Reactivo reactivo = (Reactivo)publicacion.AppSocial;
                    reactivo.TipoReactivo = ETipoReactivo.Estandarizado;
                    reactivo = reactivoCtrl.RetrieveComplete(dctx, reactivo);

                    //la publicación de un reactivo no podrá ser eliminada

                    dto.reactivo = reactivoDTOCtrl.ReactivoToDto(reactivo, false);
                    dto.renderreporteabuso = false;
                }
                else if (publicacion.TipoPublicacion == ETipoPublicacion.APPSOCIAL && publicacion.AppSocial != null)
                {
                    dto.recursocompartido = new recursocompartidodto();
                    if (publicacion.AppSocial is Reactivo)
                    {
                        ReactivoCtrl reactivoCtrl = new ReactivoCtrl();
                        ReactivoDTOCtrl reactivoDTOCtrl = new ReactivoDTOCtrl();
                        Reactivo reactivo = (Reactivo)publicacion.AppSocial;
                        reactivo.TipoReactivo = ETipoReactivo.Estandarizado;
                        reactivo = reactivoCtrl.RetrieveComplete(dctx, reactivo);

                        dto.recursocompartido.etiqueta = reactivo.NombreReactivo;
                        dto.recursocompartido.recursoid = reactivo.ReactivoID.Value.ToString("N");
                        dto.recursocompartido.tipo = 1;
                    }
                    else if (publicacion.AppSocial is ContenidoDigital)
                    {
                        ContenidoDigitalCtrl contenidoCtrl = new ContenidoDigitalCtrl();
                        ContenidoDigital contenidoDigital = contenidoCtrl.RetrieveComplete(dctx, publicacion.AppSocial as ContenidoDigital);

                        dto.recursocompartido.etiqueta = contenidoDigital.Nombre;
                        dto.recursocompartido.recursoid = contenidoDigital.ContenidoDigitalID.Value.ToString();
                        dto.recursocompartido.tipo = 3;

                        string cadenaToken = dto.recursocompartido.recursoid + userSession.CurrentUsuarioSocial.ScreenName.ToLower().Replace(" ", "");
                        byte[] bytes = EncryptHash.SHA1encrypt(cadenaToken);
                        string token = EncryptHash.byteArrayToStringBase64(bytes);

                        dto.recursocompartido.token = token;
                    }
                }
            }

            if (publicacion.ComentariosPublicacion != null)
            {
                dto.comentarios = new List<comentariodto>();
                dto.totalcomentarios = 0;
                if (publicacion.ComentariosPublicacion.Count > 0)
                {
                    ComentarioDTOCtrl comentarioDtoCtrl = new ComentarioDTOCtrl();
                    dto.totalcomentarios = publicacion.ComentariosPublicacion.Count;
                    //maximo de comentarios que se podran ver
                    int max = 3;
                    if (complete || dto.totalcomentarios <= max)
                    {
                        foreach (var com in publicacion.ComentariosPublicacion)
                        {
                            comentariodto dtocomentario = comentarioDtoCtrl.ObjectToDto(com);
                            dtocomentario.renderdelete = false;
                            dtocomentario.publicacionid = publicacion.PublicacionID.ToString();
                            long universidadID = 0;
                            if (userSession.IsDocente())
                                universidadID = (long)userSession.CurrentUser.UniversidadId;
                            else
                                universidadID = (long)userSession.CurrentAlumno.Universidades[0].UniversidadID;

                            UsuarioGrupo usuarioGrupo = grupoSocialCtrl.RetrieveFriend(dctx, com.UsuarioSocial, com.UsuarioSocial, userSession.CurrentAlumno.AreasConocimiento, universidadID);
                            if (usuarioGrupo != null)
                            {
                                if ((bool)usuarioGrupo.EsModerador) // el que comento es moderador?
                                {  //el comentario es de un docente, por lo tanto solo podra eliminarlo el mismo
                                    dtocomentario.renderdelete = com.UsuarioSocial.UsuarioSocialID == currentUsuarioSocial.UsuarioSocialID;
                                    //los comentarios son de un docente por lo tanto no podran ser reportados como abuso
                                    dtocomentario.renderreporteabuso = false;
                                }
                                else
                                {    //comento un alumno, por lo tanto solo se podra eliminar si fue comentado en su muro o el alumno comento
                                    // o un docente esta logueado
                                    dtocomentario.renderdelete = dtocomentario.usuariosocialidcom == currentUsuarioSocial.UsuarioSocialID ||
                                        publicacion.SocialHub.SocialHubID == userSession.SocialHub.SocialHubID || isLoggedDocente;

                                    //podrá reportar los comentarios que no le pertenecen
                                    dtocomentario.renderreporteabuso = false; // dtocomentario.usuariosocialidcom != currentUsuarioSocial.UsuarioSocialID && !isLoggedDocente;
                                }

                                dtocomentario.renderlink = !isLoggedDocente || !(bool)usuarioGrupo.EsModerador;
                            }
                            dto.comentarios.Add(dtocomentario);
                        }
                    }
                    else
                    {
                        int index = dto.totalcomentarios.Value - max;
                        for (; index < dto.totalcomentarios.Value; index++)
                        {
                            comentariodto dtocomentario = comentarioDtoCtrl.ObjectToDto(publicacion.ComentariosPublicacion[index]);
                            dtocomentario.renderdelete = false;
                            dtocomentario.publicacionid = publicacion.PublicacionID.ToString();

                            long universidadID = 0;
                            if (userSession.IsDocente())
                                universidadID = (long)userSession.CurrentUser.UniversidadId;
                            else
                                universidadID = (long)userSession.CurrentAlumno.Universidades[0].UniversidadID;

                            UsuarioGrupo usuarioGrupo = grupoSocialCtrl.RetrieveFriend(dctx, currentUsuarioSocial, publicacion.ComentariosPublicacion[index].UsuarioSocial, userSession.CurrentAlumno.AreasConocimiento, universidadID);
                            if (usuarioGrupo != null)
                            {
                                if ((bool)usuarioGrupo.EsModerador) // el que comento es moderador?
                                {  //el comentario es de un docente, por lo tanto solo podra eliminarlo el mismo
                                    dtocomentario.renderdelete = publicacion.ComentariosPublicacion[index].UsuarioSocial.UsuarioSocialID == currentUsuarioSocial.UsuarioSocialID;
                                    //el comentario es de un docente, por lo tanto no podrá ser reportado.
                                    dtocomentario.renderreporteabuso = false;
                                }
                                else
                                {    //comento un alumno, por lo tanto solo se podra eliminar si fue comentado en su muro o el alumno comento
                                    // o un docente esta logueado
                                    dtocomentario.renderdelete = dtocomentario.usuariosocialidcom == currentUsuarioSocial.UsuarioSocialID ||
                                        publicacion.SocialHub.SocialHubID == userSession.SocialHub.SocialHubID || isLoggedDocente;

                                    //el comentario fue realizado por un alumno por lo que podrá ser reportado siempre que no le pertenezca
                                    dtocomentario.renderreporteabuso = false; // dtocomentario.usuariosocialidcom != currentUsuarioSocial.UsuarioSocialID && !isLoggedDocente;
                                }

                                dtocomentario.renderlink = !isLoggedDocente || !(bool)usuarioGrupo.EsModerador;
                            }
                            dto.comentarios.Add(dtocomentario);
                        }
                    }

                }
            }

            return dto;
        }
        private List<Publicacion> DataSetToListPublicacion(DataSet ds)
        {
            List<Publicacion> listaPublicaciones = new List<Publicacion>();
            PublicacionCtrl publicacionCtrl = new PublicacionCtrl();
            foreach (DataRow row in ds.Tables["Publicacion"].Rows)
            {
                Publicacion publicacion = publicacionCtrl.DataRowToPublicacion(row);
                listaPublicaciones.Add(publicacion);
            }
            return listaPublicaciones;
        }

        public publicaciondto RemovePublicacion(publicaciondto dto)
        {

            Publicacion publicacion = DtoToPublicacion(dto);

            if (publicacion.PublicacionID != null)
            {
                PublicacionCtrl publicacionCtrl = new PublicacionCtrl();
                DataSet ds = publicacionCtrl.Retrieve(dctx, publicacion);

                if (ds.Tables["Publicacion"].Rows.Count > 0)
                {
                    publicacion = publicacionCtrl.LastDataRowToPublicacion(ds);
                    IUserSession userSession = new UserSession();
                    UsuarioSocial currentUsuarioSocial = userSession.CurrentUsuarioSocial;

                    #region Validacion del tipo de usuario que realizó el comentario
                    // Validacion para identificar si es un docente el dueño de la publicacion
                    // y buscar su usuariosocial directo y no desde la vista
                    LicenciaEscuelaCtrl lic = new LicenciaEscuelaCtrl();
                    UsuarioCtrl usuarioCtrl = new UsuarioCtrl();

                    Usuario propietariocomentario = usuarioCtrl.RetrieveComplete(dctx, lic.RetrieveUsuario(dctx, new UsuarioSocial { UsuarioSocialID = publicacion.UsuarioSocial.UsuarioSocialID }, false));

                    List<LicenciaEscuela> licenciasEscuela = lic.RetrieveLicencia(dctx, propietariocomentario);
                    ALicencia licencia = null;
                    foreach (LicenciaEscuela licenciaEscuela in licenciasEscuela)
                    {
                        // buscamos la licencia
                        licencia = licenciaEscuela.ListaLicencia.Find(item => (bool)item.Activo && item.Tipo != ETipoLicencia.DIRECTOR);
                    }
                    bool esPublicacionAlumno = true;
                    if (licencia.Tipo == ETipoLicencia.DOCENTE)
                        esPublicacionAlumno = false;
                    #endregion

                    if (publicacion.UsuarioSocial.UsuarioSocialID == currentUsuarioSocial.UsuarioSocialID ||
                                publicacion.SocialHub.SocialHubID == userSession.SocialHub.SocialHubID || !userSession.IsAlumno())
                    {
                        try
                        {
                            long universidadID = 0;
                            bool esAlumno = true;
                            if (userSession.IsDocente())
                            {
                                universidadID = (long)userSession.CurrentUser.UniversidadId;
                                esAlumno = false;
                            }
                            else
                                universidadID = (long)userSession.CurrentAlumno.Universidades[0].UniversidadID;

                            publicacionCtrl.RemoveComplete(dctx, publicacion, userSession.CurrentUsuarioSocial, userSession.CurrentAlumno.AreasConocimiento, universidadID, esAlumno, esPublicacionAlumno);
                            dto.success = true;
                        }
                        catch (Exception exception)
                        {
                            dto.success = false;
                            dto.error = "Ocurrió un error al eliminar la publicación";
                        }
                    }
                    else
                    {
                        dto.success = false;
                        dto.error = "No tiene permiso para eliminar la publicación.";
                    }
                }
                else
                {
                    dto.success = false;
                    dto.error = "No existe la publicación.";
                }
            }
            else
            {
                dto.success = false;
                dto.error = "No existe la publicación.";
            }
            return dto;
        }

        public List<comentariodto> RemoveComentariosBorrados(publicaciondto pub)
        {
            List<comentariodto> listaComentarios = new List<comentariodto>();
            foreach (var comen in pub.comentarios)
            {
                if (comen.estatuscom == 1)
                    listaComentarios.Add(comen);
            }
            return listaComentarios;
        }


        public publicaciondto GetPublicacionReportada(publicaciondto dto)
        {
            if (dto.publicacionid == null)
                return null;

            PublicacionCtrl publicacionCtrl = new PublicacionCtrl();
            Publicacion publicacion = new Publicacion { PublicacionID = Guid.Parse(dto.publicacionid) };

            Publicacion publicacioncompleto = publicacionCtrl.LastDataRowToPublicacion(publicacionCtrl.Retrieve(dctx, publicacion));

            publicacioncompleto.SocialHub = socialHubCtrl.LastDataRowToSocialHub(socialHubCtrl.Retrieve(dctx, new SocialHub { SocialHubID = publicacioncompleto.SocialHub.SocialHubID }));
            if (publicacioncompleto.SocialHub.SocialProfileType == ESocialProfileType.USUARIOSOCIAL)
            {
                publicacioncompleto.SocialHub.SocialProfile = usuarioSocialCtrl.LastDataRowToUsuarioSocial(usuarioSocialCtrl.Retrieve(dctx, new UsuarioSocial { UsuarioSocialID = (publicacioncompleto.SocialHub.SocialProfile as UsuarioSocial).UsuarioSocialID }));
            }
            publicacioncompleto.UsuarioSocial = usuarioSocialCtrl.LastDataRowToUsuarioSocial(usuarioSocialCtrl.Retrieve(dctx, new UsuarioSocial { UsuarioSocialID = publicacioncompleto.UsuarioSocial.UsuarioSocialID }));
            publicacioncompleto.ComentariosPublicacion = new List<Comentario>();
            publicacioncompleto.Privacidades = publicacionCtrl.RetrievePrivacidadPublicacion(dctx, publicacioncompleto);

            //consultar los comentarios activos e inactivos
            ComentarioCtrl comentarioCtrl = new ComentarioCtrl();
            DataSet dsComentarios = comentarioCtrl.Retrieve(dctx, new Comentario(), new Publicacion { PublicacionID = publicacioncompleto.PublicacionID });
            foreach (DataRow rowComentario in dsComentarios.Tables["Comentario"].Rows)
            {
                publicacioncompleto.ComentariosPublicacion.Add(comentarioCtrl.RetrieveComplete(dctx, new Comentario { ComentarioID = Guid.Parse(rowComentario["ComentarioID"].ToString()) }, new Publicacion { PublicacionID = Guid.Parse(rowComentario["PublicacionID"].ToString()) }));
            }

            publicaciondto pub = PublicacionToDto(publicacioncompleto, true);

            return pub;
        }
    }
}
