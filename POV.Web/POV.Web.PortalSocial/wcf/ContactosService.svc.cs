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
using POV.Logger.Service;

namespace POV.Web.PortalSocial.wcf
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ContactosService
    {
      
        [OperationContract]
        public List<contactodto> GetContactos(contactoinputdto dto)
        {
            ContactoDTOCtrl contactoDTOCtrl = new ContactoDTOCtrl();

            return contactoDTOCtrl.GetContactos(dto).GroupBy(x => x.usuariosocialid).Select(grp => grp.First()).ToList();
            
        }
    }
}
