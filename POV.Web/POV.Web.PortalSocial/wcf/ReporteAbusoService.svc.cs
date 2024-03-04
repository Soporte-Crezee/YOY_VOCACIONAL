using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using GP.SocialEngine.BO;
using POV.Core.RedSocial.Implement;
using POV.Core.RedSocial.Interfaces;
using POV.Web.DTO;
using POV.Web.DTO.Services;

namespace POV.Web.PortalSocial.wcf
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ReporteAbusoService
    {
        // Para usar HTTP GET, agregue el atributo [WebGet]. (El valor predeterminado de ResponseFormat es WebMessageFormat.Json)
        // Para crear una operación que devuelva XML,
        //     agregue [WebGet(ResponseFormat=WebMessageFormat.Xml)]
        //     e incluya la siguiente línea en el cuerpo de la operación:
        //         WebOperationContext.Current.OutgoingResponse.ContentType = "text/xml";

        private ReporteAbusoDTOCtrl dtoCtrl;
        public ReporteAbusoService()
        {

            dtoCtrl = new ReporteAbusoDTOCtrl
                {
                    ConfiguracionReporte = new ConfiguracionReporteAbuso
                        {
                            MaximoReportes = ConfigurationManager.AppSettings["POVMaximoReporteAbuso"] != null ? int.Parse(ConfigurationManager.AppSettings["POVMaximoReporteAbuso"]) : (int?)null,
                            TiempoCancelacionMinutos = ConfigurationManager.AppSettings["POVCancelarReporteAbuso"] != null ? int.Parse(ConfigurationManager.AppSettings["POVCancelarReporteAbuso"]) : (int?)null
                        }
                };
        }
        [OperationContract]
        public reporteabusodto ConfirmReporteAbuso(reporteabusodto dto)
        {
            if (IsDocente())
            {
              return dtoCtrl.ConfirmReporteAbuso(dto);
            }
            return new reporteabusodto {success = false, error = "ocurrió un error al procesar su solicitud"};
        }
      
        [OperationContract]
        public reporteabusodto DeleteReporteAbuso(reporteabusodto dto)
        {
            if (IsDocente())
            {
              return  dtoCtrl.DeleteReporteAbuso(dto);
            }
            return new reporteabusodto { success = false, error = "ocurrió un error al procesar su solicitud" };
        }
        [OperationContract]
        public List<reporteabusodto> GetReportesAbuso(notificacioninputdto dto)
        {
            if (IsDocente())
            {
                return dtoCtrl.GetReportesAbusoDocente(dto);
            }
            return new List<reporteabusodto>();
        }

        [OperationContract]
        public publicaciondto GetCompleteReporteAbuso(reporteabusodto dto)
        {
            if (IsDocente())
            {
                return dtoCtrl.GetPublicacionReporteAbuso(dto);
            }
            return new publicaciondto { success = false, error = "ocurrió un error al procesar su solicitud" };
        }

        [OperationContract]
        public reporteabusodto InsertReporteAbuso(reporteabusodto dto)
        {
            if (!IsDocente())
            {
                return dtoCtrl.InsertReporteAbuso(dto);
            }
            return new reporteabusodto { success = false, error = "ocurrió un error al procesar su solicitud" };
        }

        [OperationContract]
        private reporteabusodto ValidateInsertReporteAbuso()
        {
         
            return dtoCtrl.validateInsertReporteAbuso();
        }
        private bool IsDocente()
        {
            IUserSession userSession = new UserSession();

            if (!userSession.IsLogin())
                return false;

            if (userSession.IsAlumno())
                return false;

            return true;
        }

        // Agregue aquí más operaciones y márquelas con [OperationContract]
    }
}
