using POV.Core.RedSocial.Implement;
using POV.Core.RedSocial.Interfaces;
using POV.Web.DTO;
using POV.Web.DTO.Services;
using POV.Web.PortalSocial.AppCode;
using Framework.Base.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;

namespace POV.Web.PortalSocial.wcf
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class FavoritosService
    {        
        [OperationContract]
        public postfavoritodto GuardarFavorito(postfavoritodto dto)
        {
            PostFavoritoDTOCtrl postFavoritoDTOCtrl = new PostFavoritoDTOCtrl();
            var result = postFavoritoDTOCtrl.GuardarFavorito(dto);

            return result;
        }

        [OperationContract]
        public postfavoritodto EliminarFavorito(postfavoritodto dto)
        {
            PostFavoritoDTOCtrl postFavoritoDTOCtrl = new PostFavoritoDTOCtrl();
            var result = postFavoritoDTOCtrl.EliminarFavorito(dto);

            return result;
        } 
    }
}
