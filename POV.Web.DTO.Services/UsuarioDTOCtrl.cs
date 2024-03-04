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
    public class UsuarioDTOCtrl
    {
        private IDataContext dctx;
        private UsuarioCtrl usuarioCtrl;
       
        public  UsuarioDTOCtrl()
        {
            dctx = new DataContext(new DataProviderFactory().GetDataProvider("POV"));
            usuarioCtrl = new UsuarioCtrl();
        }
        
        public usuariodto ValidateUsuario(usuariodto dto)
        {
            Usuario usuario = DtoToUsuario(dto);              
            DataSet ds = usuarioCtrl.Retrieve(dctx, usuario);
            if (ds.Tables["Usuario"].Rows.Count == 0)
                return null;
            usuario = usuarioCtrl.LastDataRowToUsuario(ds);
            
            if (usuario.UsuarioID != null)
            {
                if (usuario.NombreUsuario != dto.nombreantusuario)
                {
                    dto.usuarioid = usuario.UsuarioID;
                    dto.nombreusuario = usuario.NombreUsuario;
                    dto.email = usuario.Email;
                    //dto.esactivo = usuario.EsActivo;
                    dto.universidadid = usuario.UniversidadId;
                    return dto;
                }
                return null;
            }
            return null;
        }

        public Usuario DtoToUsuario(usuariodto dto)
        {
            Usuario usuario = new Usuario();
            if (dto == null)
                return null;

            if (!string.IsNullOrEmpty(dto.nombreusuario))
                usuario.NombreUsuario = dto.nombreusuario;
           
            if (!string.IsNullOrEmpty(dto.telefono))
                usuario.TelefonoReferencia = dto.telefono;  

            if (!string.IsNullOrEmpty(dto.email))
                usuario.Email = dto.email;

            if (dto.universidadid != null)
                usuario.UniversidadId = dto.universidadid;

            return usuario;
        }
    }
}
