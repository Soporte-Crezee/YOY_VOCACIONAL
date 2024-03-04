using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using POV.Web.DTO.Services;
using POV.Web.DTO;

namespace POV.Web.PortalSocial.wcf
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ReactivosService
    {

        [OperationContract]
        public reactivooutputdto SearchReactivos(reactivoinputdto dto)
        {
            ReactivoDTOCtrl dtoCtrl = new ReactivoDTOCtrl();
            return dtoCtrl.SearchReactivos(dto);

        }

        [OperationContract]
        public reactivodto GetReactivo(reactivodto dto)
        {
            RespuestaReactivoDTOCtrl dtoCtrl = new RespuestaReactivoDTOCtrl();
            return dtoCtrl.GetReactivo(dto);

        }

        [OperationContract]
        public reactivodto SuscribirReactivo(reactivodto dto)
        {
            ReactivoDTOCtrl dtoCtrl = new ReactivoDTOCtrl();
            return dtoCtrl.SuscribirReactivo(dto);

        }

        [OperationContract]
        public reactivodto QuitarSuscripcion(reactivodto dto)
        {
            ReactivoDTOCtrl dtoCtrl = new ReactivoDTOCtrl();
            return dtoCtrl.QuitarSuscripcion(dto);

        }

        [OperationContract]
        public respuestareactivodto RegistrarRespuestas(respuestareactivodto dto)
        {
            RespuestaReactivoDTOCtrl dtoCtrl = new RespuestaReactivoDTOCtrl();
            return dtoCtrl.RegistrarRespuestas(dto);
        }

        [OperationContract]
        public List<areaaplicaciondto> GetAreaAplicacion(areaaplicaciondto dto)
        {
            RespuestaReactivoDTOCtrl dtoCtrl = new RespuestaReactivoDTOCtrl();
            return new List<areaaplicaciondto>();
        }
    }
}
