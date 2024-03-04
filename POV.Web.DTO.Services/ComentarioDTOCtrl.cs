using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Base.DataAccess;
using GP.SocialEngine.BO;
using GP.SocialEngine.Utils;
using GP.SocialEngine.Service;
using System.Data;
using POV.Core.RedSocial.Interfaces;
using POV.Core.RedSocial.Implement;
using POV.Reactivos.BO;
using POV.ContenidosDigital.BO;
using POV.Reactivos.Service;
using POV.ContenidosDigital.Service;
using POV.Seguridad.Utils;
using POV.Licencias.BO;
using POV.Licencias.Service;
using POV.Seguridad.Service;
using POV.Seguridad.BO;

namespace POV.Web.DTO.Services
{
    public class ComentarioDTOCtrl
    {
        private IDataContext dctx;
        private IUserSession userSession;
        private GrupoSocialCtrl grupoSocialCtrl;

        public ComentarioDTOCtrl()
        {
            dctx = new DataContext((new DataProviderFactory()).GetDataProvider("POV"));
            grupoSocialCtrl = new GrupoSocialCtrl();
            userSession = new UserSession();
        }

        public Comentario DtoToObject(comentariodto dto)
        {
            Comentario comentario = new Comentario();

            if (!string.IsNullOrEmpty(dto.comentarioid))
                comentario.ComentarioID = Guid.Parse(dto.comentarioid);
            if (!string.IsNullOrEmpty(dto.contenidocom))
            {
                if (dto.contenidocom.Trim().Length > 200)
                    comentario.Cuerpo = dto.contenidocom.Trim().Substring(0, 200);
                else
                    comentario.Cuerpo = dto.contenidocom.Trim();
            }
            if (dto.estatuscom != null)
                comentario.Estatus = Convert.ToBoolean(dto.estatuscom.Value);


            if (dto.usuariosocialidcom != null)
                comentario.UsuarioSocial = new UsuarioSocial { UsuarioSocialID = dto.usuariosocialidcom };

            if (string.IsNullOrEmpty(dto.rankingid))
            {
                comentario.Ranking = new Ranking();
                comentario.Ranking.RankingID = null;
            }
            else
            {
                comentario.Ranking = new Ranking();
                comentario.Ranking.RankingID = Guid.Parse(dto.rankingid);
            }

            if (dto.tipo != null)
            {
                if (dto.tipo == (short)ETipoPublicacion.APPSOCIAL)
                {
                    comentario.TipoPublicacion = ETipoPublicacion.APPSOCIAL;
                    if (!string.IsNullOrEmpty(dto.tipocompartido))
                    {
                        if (dto.tipocompartido.CompareTo("reactivo") == 0)
                        {
                            Reactivo reactivo = new Reactivo();
                            reactivo.ReactivoID = Guid.Parse(dto.compartidoid);

                            comentario.AppSocial = reactivo;
                        }
                        else if (dto.tipocompartido.CompareTo("contenido") == 0)
                        {
                            ContenidoDigital contenido = new ContenidoDigital();
                            contenido.ContenidoDigitalID = long.Parse(dto.compartidoid);

                            comentario.AppSocial = contenido;
                        }
                    }
                }
                else if (dto.tipo == (short)ETipoPublicacion.TEXTO)
                    comentario.TipoPublicacion = ETipoPublicacion.TEXTO;
            }
            else
                comentario.TipoPublicacion = ETipoPublicacion.TEXTO;


            return comentario;
        }

        public comentariodto ObjectToDto(Comentario comentario)
        {
            comentariodto dto = new comentariodto();
            if (comentario.ComentarioID != null)
                dto.comentarioid = comentario.ComentarioID.ToString();
            if (!string.IsNullOrEmpty(comentario.Cuerpo))
                dto.contenidocom = comentario.Cuerpo;
            if (comentario.Estatus != null)
                dto.estatuscom = Convert.ToInt32(comentario.Estatus);
            if (comentario.FechaComentario != null)
            {
                dto.fechacom = String.Format("{0:dd/MM/yyyy a la(\\s) HH:mm}", comentario.FechaComentario.Value);
                dto.fechacomformated = new TimeAgoDateFormat().Format(comentario.FechaComentario.Value);
            }
            if (comentario.UsuarioSocial != null && comentario.UsuarioSocial.UsuarioSocialID != null)
            {
                dto.usuariosocialidcom = comentario.UsuarioSocial.UsuarioSocialID;
                dto.usuariosocialnombrecom = comentario.UsuarioSocial.ScreenName;
            }

            if (comentario.Ranking != null && comentario.Ranking.RankingID != null)
            {
                dto.rankingid = comentario.Ranking.RankingID.ToString();
                dto.numvotes1 = comentario.Ranking.ObtenerPuntuacion(EPuntuacionRanking.EXCELENTE);
                dto.numvotes2 = comentario.Ranking.ObtenerPuntuacion(EPuntuacionRanking.BIEN);
                dto.numvotes3 = comentario.Ranking.ObtenerPuntuacion(EPuntuacionRanking.REGULAR);
            }

            if (comentario.TipoPublicacion != null && comentario.TipoPublicacion == ETipoPublicacion.APPSOCIAL && comentario.AppSocial != null)
            {
                dto.tipo = (short)comentario.TipoPublicacion;
                dto.recursocompartido = new recursocompartidodto();
                if (comentario.AppSocial is Reactivo)
                {
                    ReactivoCtrl reactivoCtrl = new ReactivoCtrl();
                    ReactivoDTOCtrl reactivoDTOCtrl = new ReactivoDTOCtrl();
                    Reactivo reactivo = (Reactivo)comentario.AppSocial;
                    reactivo.TipoReactivo = ETipoReactivo.Estandarizado;
                    reactivo = reactivoCtrl.RetrieveComplete(dctx, reactivo);

                    dto.recursocompartido.etiqueta = reactivo.NombreReactivo;
                    dto.recursocompartido.recursoid = reactivo.ReactivoID.Value.ToString("N");
                    dto.recursocompartido.tipo = 1;
                }
                else if (comentario.AppSocial is ContenidoDigital)
                {
                    ContenidoDigitalCtrl contenidoCtrl = new ContenidoDigitalCtrl();
                    ContenidoDigital contenidoDigital = contenidoCtrl.RetrieveComplete(dctx, comentario.AppSocial as ContenidoDigital);

                    dto.recursocompartido.etiqueta = contenidoDigital.Nombre;
                    dto.recursocompartido.recursoid = contenidoDigital.ContenidoDigitalID.Value.ToString();
                    dto.recursocompartido.tipo = 3;

                    string cadenaToken = dto.recursocompartido.recursoid + userSession.CurrentUsuarioSocial.ScreenName.ToLower().Replace(" ", "");
                    byte[] bytes = EncryptHash.SHA1encrypt(cadenaToken);
                    string token = EncryptHash.byteArrayToStringBase64(bytes);

                    dto.recursocompartido.token = token;
                }
            }
            else if (comentario.TipoPublicacion == null)
                dto.tipo = (short)ETipoPublicacion.TEXTO;

            return dto;
        }

        public comentariodto SaveComentario(comentariodto dto)
        {
            try
            {
                ComentarioCtrl comentarioCtrl = new ComentarioCtrl();
                Comentario comentario = DtoToObject(dto);
                comentario.ComentarioID = Guid.NewGuid();
                comentario.FechaComentario = DateTime.Now;
                comentario.Estatus = true;

                Publicacion publicacion = new Publicacion();
                Guid newGuid = Guid.Parse(dto.publicacionid);

                //Crear un rating para este comentario
                RankingCtrl ratingCtrl = new RankingCtrl();
                Ranking rating = new Ranking();
                rating.RankingID = Guid.NewGuid();

                ratingCtrl.Insert(dctx, rating);

                comentario.Ranking = rating;

                comentarioCtrl.InsertComplete(dctx, comentario, new Publicacion { PublicacionID = Guid.Parse(dto.publicacionid) });
                comentario = comentarioCtrl.RetrieveComplete(dctx, comentario);

                comentariodto dtoinsertado = new comentariodto();
                dtoinsertado = ObjectToDto(comentario);
                dtoinsertado.publicacionid = dto.publicacionid;
                dtoinsertado.renderdelete = true;
                dtoinsertado.renderlink = true;
                return dtoinsertado;
            }
            catch (Exception ex)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
                throw ex;
            }
        }

        public comentariodto RemoveComentario(comentariodto dto)
        {
            try
            {
                Comentario comentario = DtoToObject(dto);

                if (comentario.ComentarioID != null)
                {
                    comentario.TipoPublicacion = null;
                    ComentarioCtrl comentarioCtrl = new ComentarioCtrl();
                    DataSet ds = comentarioCtrl.Retrieve(dctx, comentario, new Publicacion());

                    if (ds.Tables["Comentario"].Rows.Count > 0)
                    {
                        comentario = comentarioCtrl.LastDataRowToComentario(ds);
                        #region Validacion del tipo de usuario que realizó el comentario
                        // Validacion para identificar si es un docente el dueño del comentario
                        // y buscar su usuariosocial directo y no desde la vista
                        LicenciaEscuelaCtrl lic = new LicenciaEscuelaCtrl();
                        UsuarioCtrl usuarioCtrl = new UsuarioCtrl();

                        Usuario propietariocomentario = usuarioCtrl.RetrieveComplete(dctx, lic.RetrieveUsuario(dctx, new UsuarioSocial { UsuarioSocialID = comentario.UsuarioSocial.UsuarioSocialID }, false));

                        List<LicenciaEscuela> licenciasEscuela = lic.RetrieveLicencia(dctx, propietariocomentario);
                        ALicencia licencia = null;
                        foreach (LicenciaEscuela licenciaEscuela in licenciasEscuela)
                        {
                            // buscamos la licencia
                            licencia = licenciaEscuela.ListaLicencia.Find(item => (bool)item.Activo && item.Tipo != ETipoLicencia.DIRECTOR);
                        }
                        bool esComentarioAlumno = true;
                        if (licencia.Tipo == ETipoLicencia.DOCENTE)
                            esComentarioAlumno = false;
                        #endregion
                        int index = ds.Tables["Comentario"].Rows.Count;
                        Guid publicacionID = (Guid)Convert.ChangeType(ds.Tables["Comentario"].Rows[index - 1]["PublicacionID"], typeof(Guid));
                        PublicacionCtrl publicacionCtrl = new PublicacionCtrl();
                        Publicacion publicacion = publicacionCtrl.LastDataRowToPublicacion(publicacionCtrl.Retrieve(dctx, new Publicacion { PublicacionID = publicacionID }));
                        IUserSession userSession = new UserSession();
                        UsuarioSocial currentUsuarioSocial = userSession.CurrentUsuarioSocial;
                        long universidadID = 0;

                        if (comentario.UsuarioSocial.UsuarioSocialID == currentUsuarioSocial.UsuarioSocialID ||
                                    publicacion.SocialHub.SocialHubID == userSession.SocialHub.SocialHubID || !userSession.IsAlumno())
                        {
                            try
                            {
                                if (userSession.IsDocente())
                                {
                                    if (userSession.CurrentUser.UniversidadId != null)
                                        universidadID = (long)userSession.CurrentUser.UniversidadId;
                                    comentarioCtrl.RemoveComplete(dctx, comentario, userSession.CurrentUsuarioSocial, userSession.CurrentAlumno.AreasConocimiento, universidadID, false, esComentarioAlumno);
                                    dto.success = true;
                                }
                                else
                                {
                                    if ((long)userSession.CurrentAlumno.Universidades.Count > 0)
                                        universidadID = (long)userSession.CurrentAlumno.Universidades[0].UniversidadID;
                                    comentarioCtrl.RemoveComplete(dctx, comentario, userSession.CurrentUsuarioSocial, userSession.CurrentAlumno.AreasConocimiento, universidadID, true, esComentarioAlumno);
                                    dto.success = true;
                                }
                               
                            }
                            catch (Exception ex)
                            {
                                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
                                dto.error = "Ocurrió un error al eliminar el comentario";
                                dto.success = false;
                            }

                        }
                        else
                        {
                            dto.success = false;
                            dto.error = "No tiene permiso para eliminar el comentario.";
                        }
                    }
                    else
                    {
                        dto.success = false;
                        dto.error = "No existe el comentario.";
                    }
                }
                else
                {
                    dto.success = false;
                    dto.error = "No existe el comentario.";
                }
                return dto;
            }
            catch (Exception ex)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
                throw ex;
            }
        }

        public comentariodto MasUnoComentario(comentariodto dto)
        {
            try
            {
                Comentario comentario = new Comentario { ComentarioID = Guid.Parse(dto.comentarioid) };
                ComentarioCtrl comentarioCtrl = new ComentarioCtrl();
                UsuarioSocialRankingCtrl usuarioSocialRankingCtrl = new UsuarioSocialRankingCtrl();

                comentario = comentarioCtrl.RetrieveComplete(dctx, comentario, new Publicacion());


                UsuarioSocialRanking uSocialRanking = comentario.Ranking.ListaPuntuaciones.Find(item => item.UsuarioSocial.UsuarioSocialID == userSession.CurrentUsuarioSocial.UsuarioSocialID);
                //se valida que el usuario no haya dado votacion al comentario
                if (uSocialRanking == null)
                {
                    //se crea el ranking
                    UsuarioSocialRanking usuarioSocialRanking = new UsuarioSocialRanking();
                    usuarioSocialRanking.FechaRegistro = DateTime.Now;
                    usuarioSocialRanking.AsignarPuntuacion(dto.vote.Value);
                    usuarioSocialRanking.UsuarioSocial = new UsuarioSocial { UsuarioSocialID = userSession.CurrentUsuarioSocial.UsuarioSocialID };
                    usuarioSocialRanking.RankingID = comentario.Ranking.RankingID;
                    usuarioSocialRankingCtrl.InsertCompleteForComentario(dctx, usuarioSocialRanking, comentario);
                    comentario = comentarioCtrl.RetrieveComplete(dctx, comentario, new Publicacion());
                }



                dto = ObjectToDto(comentario);

                return dto;
            }
            catch (Exception ex)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
                throw ex;
            }
        }
    }
}
