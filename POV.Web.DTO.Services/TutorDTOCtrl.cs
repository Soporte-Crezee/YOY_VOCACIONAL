using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Framework.Base.DataAccess;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.Core.RedSocial.Implement;
using POV.Core.RedSocial.Interfaces;
using POV.Seguridad.BO;
using POV.Seguridad.Service;
using POV.Administracion.Service;
using POV.CentroEducativo.Services;

namespace POV.Web.DTO.Services
{
    public class TutorDTOCtrl
    {
        private TutorCtrl tutorCtrl;
        private IUserSession userSession;
        private CatalogoTutoresCtrl catalogoTutoresCtrl;
        private IDataContext dctx;
        private UsuarioCtrl usuarioCtrl;
       
        public  TutorDTOCtrl()
        {
            tutorCtrl = new TutorCtrl(null);
            userSession = new UserSession();
            catalogoTutoresCtrl = new CatalogoTutoresCtrl();
            dctx = new DataContext(new DataProviderFactory().GetDataProvider("POV"));
            usuarioCtrl = new UsuarioCtrl();
        }
        
        public tutordto ValidateTutor(tutordto dto)
        {
            Usuario usuario = DtoToUsuario(dto);              
            DataSet ds = usuarioCtrl.Retrieve(dctx, usuario);
            if (ds.Tables["Usuario"].Rows.Count == 0)
                return null;
            usuario = usuarioCtrl.LastDataRowToUsuario(ds);

            if (usuario.UsuarioID != null)
            {
                dto.usuarioid = usuario.UsuarioID;
                dto.nombreusuario = usuario.NombreUsuario;
                dto.email = usuario.Email;
                return dto;
            }
            return null;
        }

        public tutordto TutorToDto(Tutor tutor,Usuario usuario)
        {
            tutordto dto = new tutordto();

            if (tutor == null)
                return null;
            if (tutor.TutorID != null && tutor.TutorID > 0)
                dto.tutorid = tutor.TutorID;
            if (!string.IsNullOrEmpty(tutor.Nombre))
                dto.nombre = tutor.Nombre;
            if (!string.IsNullOrEmpty(tutor.PrimerApellido))
                dto.primerapellido = tutor.PrimerApellido;
            if (!string.IsNullOrEmpty(tutor.SegundoApellido))
                dto.segundoapellido = tutor.SegundoApellido;            
            if (tutor.Sexo != null)
                dto.sexo = tutor.Sexo.Value ? 1 : 0;
            if (tutor.Estatus != null)
                dto.estatus = tutor.Estatus.Value ? 1 : 0;

            dto.nombrecompleto = string.Format("{0} {1} {2}", tutor.Nombre, tutor.PrimerApellido, tutor.SegundoApellido);
            dto.nombrecompleto = dto.nombrecompleto.Trim();

            if (usuario != null)
            {
                if (!string.IsNullOrEmpty(usuario.Email))
                    dto.email = usuario.Email;
                if (!string.IsNullOrEmpty(usuario.TelefonoReferencia))
                    dto.telefono = usuario.TelefonoReferencia;
            }
           

            return dto;
        }
        public Tutor DtoToTutor(tutordto dto)
        {
            Tutor tutor = new Tutor();
            if (dto == null)
                return null;

            if (dto.tutorid != null && dto.tutorid > 0)
                tutor.TutorID = dto.tutorid;

            if (!string.IsNullOrEmpty(dto.nombre))
                tutor.Nombre = dto.nombre;
            if (!string.IsNullOrEmpty(dto.primerapellido))
                tutor.PrimerApellido = dto.primerapellido;
            if (!string.IsNullOrEmpty(dto.segundoapellido))
                tutor.SegundoApellido = dto.segundoapellido;
            if (dto.sexo!=null)
                tutor.Sexo = bool.Parse(dto.sexo.ToString());
            if (dto.estatus != null)
                tutor.Estatus = bool.Parse(dto.estatus.ToString());
               

            return tutor;
        }

        public Usuario DtoToUsuario(tutordto dto)
        {
            Usuario usuario = new Usuario();
            if (dto == null)
                return null;

            if (!string.IsNullOrEmpty(dto.nombreusuario))
                usuario.NombreUsuario = dto.nombreusuario;

            if (!string.IsNullOrEmpty(dto.email))
                usuario.Email = dto.email;  

            return usuario;
        }
    }
}
