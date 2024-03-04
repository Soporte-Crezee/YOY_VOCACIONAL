using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using POV.Profesionalizacion.DTO.BO;
using POV.Profesionalizacion.DTO.Service;

namespace POV.Web.PortalSocial.wcf
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class SituacionesService
    {
        [OperationContract]
        public List<situacionaprendizajedto> SearchSituacionesAprendizaje(situacionaprendizajeinputdto dto)
        {
            SituacionAprendizajeDTOCtrl situacionDTOCtrl = new SituacionAprendizajeDTOCtrl();
            return situacionDTOCtrl.SearchSituacionesAprendizaje(dto);
        }
    }
}
