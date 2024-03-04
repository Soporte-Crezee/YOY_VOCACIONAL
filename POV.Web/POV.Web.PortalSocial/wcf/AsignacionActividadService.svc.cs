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

namespace POV.Web.PortalSocial.wcf
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    
    public class AsignacionActividadService
    {
        [OperationContract]
        public asignacionactividaddto GetTotalAsignacionActividad()
        {
            AsignacionActividadDTOCtrl dtoCtrl = new AsignacionActividadDTOCtrl();
            return dtoCtrl.GetTotalAsignacionActividad();
        }
    }
}
