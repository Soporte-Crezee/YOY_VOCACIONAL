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
    public class MensajesService
    {
        // Para usar HTTP GET, agregue el atributo [WebGet]. (El valor predeterminado de ResponseFormat es WebMessageFormat.Json)
        // Para crear una operación que devuelva XML,
        //     agregue [WebGet(ResponseFormat=WebMessageFormat.Xml)]
        //     e incluya la siguiente línea en el cuerpo de la operación:
        //         WebOperationContext.Current.OutgoingResponse.ContentType = "text/xml";
        [OperationContract]
        public List<mensajedto> GetMensajes(mensajedtoinput dto)
        {
            MensajeDTOCtrl mensajeDtoCtrl = new MensajeDTOCtrl();
            List<mensajedto> ls = mensajeDtoCtrl.GetMensajes(dto);
           
            return ls;
        }

        // Agregue aquí más operaciones y márquelas con [OperationContract]


        [OperationContract]
        public List<mensajedto> GetLastMensajes(mensajedtoinput dto)
        {
            // Agregue aquí la implementación de la operación
            MensajeDTOCtrl mensajeDtoCtrl = new MensajeDTOCtrl();
            List<mensajedto> ls = mensajeDtoCtrl.GetLastMensajes(dto);
            
            return ls;
        }

        [OperationContract]
        public List<mensajedto> GetMensajesPaginados(mensajedtoinput dto)
        {
            MensajeDTOCtrl mensajeDtoCtrl = new MensajeDTOCtrl();
            List<mensajedto> lsdto = mensajeDtoCtrl.GetMensajesPaginados(dto);
            return lsdto;
        }

        [OperationContract]
        public mensajedto SaveMensaje(mensajedto dto)
        {
            // Agregue aquí la implementación de la operación
            MensajeDTOCtrl mensajeDtoCtrl = new MensajeDTOCtrl();
            mensajedto ls = mensajeDtoCtrl.SaveMensaje(dto);

            return ls;
        }

        [OperationContract]
        public mensajedto RemoveAsociadoMensaje(mensajedto dto)
        {
            // Agregue aquí la implementación de la operación
            MensajeDTOCtrl mensajeDtoCtrl = new MensajeDTOCtrl();
            mensajedto ls = mensajeDtoCtrl.RemoveAsociadoMensaje(dto);

            return ls;
        }
         [OperationContract]
        public mensajedto SaveRespuestaMensaje(mensajedto dto)
        {
            MensajeDTOCtrl mensajeDtoCtrl = new MensajeDTOCtrl();
            mensajedto ls = mensajeDtoCtrl.SaveRespuestaMensaje(dto);
            return ls;
        }


    }
}
