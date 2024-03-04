using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Base.DataAccess;
using GP.SocialEngine.BO;
using GP.SocialEngine.DA;
using POV.Seguridad.BO;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.CentroEducativo.DA;
using GP.SocialEngine.Service;
using POV.Core.RedSocial.Implement;
using POV.Core.RedSocial.Interfaces;

namespace POV.Web.DTO.Services
{
    public class ContactoDTOCtrl
    {
        private IDataContext dctx;

        private IUserSession userSession;

        private InvitacionCtrl invitacionCtrl;

        public ContactoDTOCtrl()
        {
            dctx = new DataContext((new DataProviderFactory()).GetDataProvider("POV"));
            userSession = new UserSession();
            invitacionCtrl = new InvitacionCtrl();
        }

        /// <summary>
        /// Listar los contactos del grupo social que no son moderador
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public List<contactodto> GetContactos(contactoinputdto dto)
        {
            List<contactodto> contactosdto = new List<contactodto>();


            GrupoSocialCtrl grupoSocialCtrl = new GrupoSocialCtrl();
            GrupoSocial grupoSocial = new GrupoSocial();
            long universidadID = 0;
            if (userSession.IsDocente())
            {
                if (userSession.CurrentUser.UniversidadId != null)
                    universidadID = (long)userSession.CurrentUser.UniversidadId;
                grupoSocial = grupoSocialCtrl.RetrieveComplete(dctx, userSession.CurrentGrupoSocial, userSession.CurrentAlumno.AreasConocimiento, userSession.CurrentDocente.DocenteID, universidadID, dto.pagesize, dto.currentpage);
            }
            else
            {
                if ((long)userSession.CurrentAlumno.Universidades.Count > 0)
                    universidadID = (long)userSession.CurrentAlumno.Universidades[0].UniversidadID;
                grupoSocial = grupoSocialCtrl.RetrieveComplete(dctx, userSession.CurrentGrupoSocial, userSession.CurrentAlumno.AreasConocimiento, null, universidadID, dto.pagesize, dto.currentpage);
            }

            if (userSession.IsDocente())
                grupoSocial.ListaUsuarioGrupo = grupoSocial.ListaUsuarioGrupo.Where(x => x.DocenteID == userSession.CurrentDocente.DocenteID).ToList();

            foreach (UsuarioGrupo usuarioGrupo in grupoSocial.ListaUsuarioGrupo)
            {
                bool esmoderador = (bool)(usuarioGrupo.EsModerador != null ? usuarioGrupo.EsModerador : false);

                if (!esmoderador && usuarioGrupo.UsuarioSocial.UsuarioSocialID != userSession.CurrentUsuarioSocial.UsuarioSocialID && (bool)usuarioGrupo.Estatus)
                    contactosdto.Add(UsuarioSocialToContactoDTO(usuarioGrupo.UsuarioSocial, esmoderador));
            }

            return contactosdto;
        }

        public List<contactodto> ListUsuarioSocialToListContactoDTO(List<UsuarioSocial> usuarios)
        {
            List<contactodto> contactosdto = new List<contactodto>();
            foreach (UsuarioSocial usuarioSocial in usuarios)
            {
                contactosdto.Add(UsuarioSocialToContactoDTO(usuarioSocial));
            }
            return contactosdto;
        }

        private contactodto UsuarioSocialToContactoDTO(UsuarioSocial usuarioSocial, bool esmoderador = false)
        {
            contactodto dto = new contactodto();

            if (usuarioSocial.UsuarioSocialID != null)
            {
                dto.usuariosocialid = usuarioSocial.UsuarioSocialID;
                bool esModerador = esmoderador;

                dto.renderlink = userSession.IsAlumno() || !esModerador;
            }
            if (!string.IsNullOrEmpty(usuarioSocial.ScreenName))
                dto.screenname = usuarioSocial.ScreenName;


            return dto;
        }
    }
}
