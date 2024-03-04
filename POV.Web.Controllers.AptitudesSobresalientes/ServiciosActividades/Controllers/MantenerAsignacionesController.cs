using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POV.ConfiguracionActividades.BO;
using POV.ConfiguracionActividades.Services;
using POV.CentroEducativo.BO;
using POV.Modelo.Context;
namespace POV.ServiciosActividades.Controllers
{
    public class MantenerAsignacionesController : IDisposable
    {

        private Contexto _ctx;
        private object _sign;

        public MantenerAsignacionesController()
        {
            _sign = new object();
            _ctx = new Contexto(_sign);
        }

        public List<AsignacionActividad> ConsultarAsignaciones(Alumno alumno, ActividadDocente actividad, AsignacionActividad asignacion)
        {
            AsignacionActividadCtrl ctrl = new AsignacionActividadCtrl(_ctx);
            return ctrl.Retrieve(alumno, actividad, asignacion, false);
        }

        public List<BloqueActividad> ConsultarBloques(BloqueActividad filtro)
        {
            BloqueActividadCtrl bloqueCtrl = new BloqueActividadCtrl(_ctx);
            return bloqueCtrl.Retrieve(filtro, false);
        }

        public bool EliminarAsignaciones(List<AsignacionActividad> eliminadas)
        {
            bool resp = false;

            AsignacionActividadCtrl ctrl = new AsignacionActividadCtrl(_ctx);
            List<AsignacionActividad> asignaciones = new List<AsignacionActividad>();
            foreach (AsignacionActividad a in eliminadas)
            {
                AsignacionActividad asig = ctrl.RetrieveWithRelationship(a, true).FirstOrDefault();
                if (asig != null)
                    asignaciones.Add(asig);
            }

            foreach (AsignacionActividad asignacion in asignaciones)
            {
                ctrl.Delete(asignacion);
            }

            int afectados = _ctx.Commit(_sign);
            if (afectados != 0) resp = true;


            return resp;
        }

        public void Dispose()
        {
            _ctx.Disposing(_sign);
        }
    }
}
