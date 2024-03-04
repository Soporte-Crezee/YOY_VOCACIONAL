using POV.Web.DTO;
using POV.Web.DTO.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;

namespace POV.Web.Portal.Pruebas.wcf
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class OrientacionVocacionalService
    {
        // Para usar HTTP GET, agregue el atributo [WebGet]. (El valor predeterminado de ResponseFormat es WebMessageFormat.Json)
        // Para crear una operación que devuelva XML,
        //     agregue [WebGet(ResponseFormat=WebMessageFormat.Xml)]
        //     e incluya la siguiente línea en el cuerpo de la operación:
        //         WebOperationContext.Current.OutgoingResponse.ContentType = "text/xml";
        [OperationContract]
        public List<sesionorientaciondto> GetSesionOrientacionAlumno(sesionorientaciondto dto)
        {
            OrientacionVocacionalDTOCtrl orientacionVocacionalDTOCtrl = new OrientacionVocacionalDTOCtrl();
            var result = orientacionVocacionalDTOCtrl.GetSesionOrientacionAlumno(dto);

            return result;
        }

        // Agregue aquí más operaciones y márquelas con [OperationContract]
    }
}
