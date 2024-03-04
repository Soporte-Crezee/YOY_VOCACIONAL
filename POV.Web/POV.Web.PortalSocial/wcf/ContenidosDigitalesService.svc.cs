using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using POV.ContenidosDigital.DTO.BO;
using POV.ContenidosDigital.DTO.Service;
using POV.Profesionalizacion.DTO.BO;

namespace POV.Web.PortalSocial.wcf
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ContenidosDigitalesService
    {
        [OperationContract]
        public List<contenidodto> SearchContenidoDigital(contenidoinputdto dto)
        {
            ContenidoDTOCtrl ctrl = new ContenidoDTOCtrl();
            return ctrl.SearchContenidosDigitales(dto);
        }
       
        [OperationContract]
        public List<contenidodto> ConsultarContenidoInformacionSituacion(agrupadorinputdto dto)
        {
            ContenidoDTOCtrl contenidoDtoCtrl = new ContenidoDTOCtrl();
            return contenidoDtoCtrl.ConsultarSituacionContenido(dto);
        }
    }

}
