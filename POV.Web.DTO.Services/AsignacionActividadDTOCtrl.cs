using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Base.DataAccess;
using POV.Web.DTO;
using GP.SocialEngine.BO;
using GP.SocialEngine.Service;
using GP.SocialEngine.DA;
using GP.SocialEngine.Utils;
using POV.Core.RedSocial.Implement;
using POV.Core.RedSocial.Interfaces;
using POV.CentroEducativo.BO;
using POV.ConfiguracionActividades.BO;
using POV.ServiciosActividades.Controllers;

namespace POV.Web.DTO.Services
{
    public class AsignacionActividadDTOCtrl
    {
        private IDataContext dctx;
        private IUserSession userSession;
        private RealizarActividadesController controlador;
        private Docente docente;
        public AsignacionActividadDTOCtrl()
        {
            dctx = new DataContext((new DataProviderFactory()).GetDataProvider("POV"));
            userSession = new UserSession();
            controlador = new RealizarActividadesController();
            docente = new Docente();
        }

        public asignacionactividaddto GetTotalAsignacionActividad()        
        {             
            asignacionactividaddto dto = new asignacionactividaddto();
            dto.totalactividades= 0;
            dto.orientador = 0;
            dto.areasconocimiento = 0;            

            List<AsignacionActividad> asignaciones = controlador.ConsultarActividadesAsignadas(userSession.CurrentAlumno);
            List<AsignacionActividad> asignacionesUniversidad = new List<AsignacionActividad>();
            List<AsignacionActividad> asignacionesDocente = new List<AsignacionActividad>();
            List<AsignacionActividad> asignacionesAreasConocimiento = new List<AsignacionActividad>();

            if (docente != null && docente.DocenteID != null)
            {
                asignacionesDocente = asignaciones.Where(item => item.Actividad is ActividadDocente && (item.Actividad as ActividadDocente).DocenteId == docente.DocenteID && item.Actividad.EscuelaId == userSession.CurrentEscuela.EscuelaID && item.AsignadoPor == 2).ToList();
                asignacionesAreasConocimiento = asignaciones.Where(item => item.Actividad is ActividadDocente && (item.Actividad as ActividadDocente).DocenteId == docente.DocenteID && item.Actividad.EscuelaId == userSession.CurrentEscuela.EscuelaID && item.AsignadoPor == 3).ToList();
            }
            else
            {
                asignacionesDocente = asignaciones.Where(item => item.Actividad is ActividadDocente && item.Actividad.EscuelaId == userSession.CurrentEscuela.EscuelaID && item.AsignadoPor == 2).ToList();
                asignacionesAreasConocimiento = asignaciones.Where(item => item.Actividad is ActividadDocente && item.Actividad.EscuelaId == userSession.CurrentEscuela.EscuelaID && item.AsignadoPor == 3).ToList();
            }
            int asignacionOrientador = int.Parse(asignacionesDocente.Where(item => item.FechaInicio <= DateTime.Now && item.FechaFin >= DateTime.Now).Count(item => item.TareasRealizadas.Any(item2 => item2.Estatus != EEstatusTarea.Finalizado)).ToString());
            int areasConocimiento = int.Parse(asignacionesAreasConocimiento.Where(item => item.FechaInicio <= DateTime.Now && item.FechaFin >= DateTime.Now).Count(item => item.TareasRealizadas.Any(item2 => item2.Estatus != EEstatusTarea.Finalizado)).ToString());
            dto.totalactividades = (asignacionOrientador + areasConocimiento);

            dto.universidad = 0;
            dto.orientador = asignacionOrientador;
            dto.areasconocimiento = areasConocimiento;
                        
            return dto;
        }
    }
}
