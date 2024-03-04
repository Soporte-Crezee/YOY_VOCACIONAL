using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using POV.Web.DTO;
using POV.Web.DTO.Services;
using POV.ServiciosActividades.Controllers;
using POV.CentroEducativo.BO;
using POV.Core.RedSocial.Implement;

namespace POV.Web.PortalSocial.wcf
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class NotificacionService
    {
        [OperationContract]
        public notificacionsummarydto GetTotalNotificaciones()
        {
            NotificacionDTOCtrl dtoCtrl = new NotificacionDTOCtrl();
            return dtoCtrl.GetTotalNotificaciones();
        }

        [OperationContract]
        public List<notificaciondto> GetNotificaciones(notificacioninputdto dto)
        {
            NotificacionDTOCtrl dtoCtrl = new NotificacionDTOCtrl();
            return dtoCtrl.GetNotificacionesSocial(dto);
        }

        [OperationContract]
        public notificaciondto DeleteNotificacion(notificaciondto dto)
        {
            NotificacionDTOCtrl dtoCtrl = new NotificacionDTOCtrl();
            return dtoCtrl.DeleteNotificacion(dto);
        }

        [OperationContract]
        public void ConfirmNotificacion()
        {
            NotificacionDTOCtrl dtoCtrl = new NotificacionDTOCtrl();
            dtoCtrl.ConfirmNotificacion();
        }

        [OperationContract]
        public asignacionactividaddto GetTotalAsignacionActividad()
        {
            AsignacionActividadDTOCtrl dtoCtrl = new AsignacionActividadDTOCtrl();
            return dtoCtrl.GetTotalAsignacionActividad();
        }
    }
}
