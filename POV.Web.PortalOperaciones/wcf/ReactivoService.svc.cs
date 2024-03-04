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
using System.Web;

namespace POV.Web.PortalOperaciones.wcf
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ReactivoService
    {
        [OperationContract]
        public reactivodto SaveReactivo(reactivodto dto)
        {
            ReactivoDTOCtrl dtoCtrl = new ReactivoDTOCtrl();

            return dtoCtrl.SaveReactivo(dto);
        }

        [OperationContract]
        public reactivodto GetReactivo(reactivodto dto)
        {
            ReactivoDTOCtrl dtoCtrl = new ReactivoDTOCtrl();

            return dtoCtrl.GetReactivo(dto);
        }

        [OperationContract]
        public preguntadto DeletePregunta(preguntadto dto)
        {
            PreguntaDTOCtrl dtoCtrl = new PreguntaDTOCtrl();

            return dtoCtrl.DeletePregunta(dto);
        }

        [OperationContract]
        public opcionrespuestadto DeleteOpcionPregunta(opcionrespuestadto dto)
        {
            PreguntaDTOCtrl dtoCtrl = new PreguntaDTOCtrl();

            return dtoCtrl.DeleteOpcionPregunta(dto);
        }

    }
}
