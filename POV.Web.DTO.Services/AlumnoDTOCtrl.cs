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

namespace POV.Web.DTO.Services
{
    public class AlumnoDTOCtrl
    {
        private AlumnoCtrl alumnoCtrl;
        private IUserSession userSession;
        private CatalogoAlumnosCtrl catalogoAlumnosCtrl;
        private IDataContext dctx;
        private UsuarioCtrl usuarioCtrl;
       
        public  AlumnoDTOCtrl()
        {
            alumnoCtrl = new AlumnoCtrl();
            userSession = new UserSession();
            catalogoAlumnosCtrl = new CatalogoAlumnosCtrl();
            dctx = new DataContext(new DataProviderFactory().GetDataProvider("POV"));
            usuarioCtrl = new UsuarioCtrl();
        }

        public alumnodto ValidateAlumnoAsignadoEscuela(alumnodto dto)
        {
            Alumno alumno = DtoToAlumno(dto);
            bool estransferencia = catalogoAlumnosCtrl.EsTransferenciaDeEscuela(dctx, alumno,userSession.CurrentEscuela,userSession.CurrentCicloEscolar);
            if (estransferencia)
            {
                alumno = alumnoCtrl.LastDataRowToAlumno(alumnoCtrl.Retrieve(dctx, new Alumno { Curp = alumno.Curp }));
                return AlumnoToDto(alumno, null);
            }
            return null;
        }

        public alumnodto ValidateAspirante(alumnodto dto)
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

        public alumnodto AlumnoToDto(Alumno alumno,Usuario usuario)
        {
            alumnodto dto = new alumnodto();

            if (alumno == null)
                return null;
            if (alumno.AlumnoID != null && alumno.AlumnoID > 0)
                dto.alumnoid = alumno.AlumnoID;
            if (!string.IsNullOrEmpty(alumno.Nombre))
                dto.nombre = alumno.Nombre;
            if (!string.IsNullOrEmpty(alumno.PrimerApellido))
                dto.primerapellido = alumno.PrimerApellido;
            if (!string.IsNullOrEmpty(alumno.SegundoApellido))
                dto.segundoapellido = alumno.SegundoApellido;
            if (!string.IsNullOrEmpty(alumno.Curp))
                dto.curp = alumno.Curp;
            if (alumno.Sexo != null)
                dto.sexo = alumno.Sexo.Value ? 1 : 0;
            if (alumno.Estatus != null)
                dto.estatus = alumno.Estatus.Value ? 1 : 0;

            dto.nombrecompleto = string.Format("{0} {1} {2}", alumno.Nombre, alumno.PrimerApellido, alumno.SegundoApellido);
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
        public Alumno DtoToAlumno(alumnodto dto)
        {
            Alumno alumno = new Alumno();
            if (dto == null)
                return null;

            if (dto.alumnoid != null && dto.alumnoid > 0)
                alumno.AlumnoID = dto.alumnoid;

            if (!string.IsNullOrEmpty(dto.curp))
                alumno.Curp = dto.curp;

            if (!string.IsNullOrEmpty(dto.nombre))
                alumno.Nombre = dto.nombre;
            if (!string.IsNullOrEmpty(dto.primerapellido))
                alumno.PrimerApellido = dto.primerapellido;
            if (!string.IsNullOrEmpty(dto.segundoapellido))
                alumno.SegundoApellido = dto.segundoapellido;
            if (dto.sexo!=null)
                alumno.Sexo = bool.Parse(dto.sexo.ToString());
            if (dto.estatus != null)
                alumno.Estatus = bool.Parse(dto.estatus.ToString());
               

            return alumno;
        }

        public Usuario DtoToUsuario(alumnodto dto)
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
