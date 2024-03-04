using POV.Web.DTO;
using POV.Web.DTO.Services;
using System.ServiceModel;
using System.ServiceModel.Activation;

namespace POV.Web.PortalUniversidad.wcf
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class UsuarioService
    {
        // Para usar HTTP GET, agregue el atributo [WebGet]. (El valor predeterminado de ResponseFormat es WebMessageFormat.Json)
        // Para crear una operación que devuelva XML,
        //     agregue [WebGet(ResponseFormat=WebMessageFormat.Xml)]
        //     e incluya la siguiente línea en el cuerpo de la operación:
        //         WebOperationContext.Current.OutgoingResponse.ContentType = "text/xml";
        [OperationContract]
        public usuariodto ValidateUsuario(usuariodto dto)
        {
            UsuarioDTOCtrl usuarioDtoCtrl = new UsuarioDTOCtrl();
            return usuarioDtoCtrl.ValidateUsuario(dto);
        }

        // Agregue aquí más operaciones y márquelas con [OperationContract]
    }
}
