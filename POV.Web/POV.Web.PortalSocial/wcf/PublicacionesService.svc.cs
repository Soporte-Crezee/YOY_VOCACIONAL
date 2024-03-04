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
    public class PublicacionesService
    {

        [OperationContract]
        public List<publicaciondto> GetPublicaciones(publicaciondtoinput dto)
        {
            PublicacionDTOCtrl publicacionDtoCtrl = new PublicacionDTOCtrl();
            return publicacionDtoCtrl.GetPublicaciones(dto);

        }

        [OperationContract]
        public List<publicaciondto> GetPublicacionesMuroDocente(publicaciondtoinput dto)
        {
            PublicacionDTOCtrl publicacionDtoCtrl = new PublicacionDTOCtrl();
            return publicacionDtoCtrl.GetPublicacionesMuroDocente(dto);

        }

        [OperationContract]
        public List<publicaciondto> GetDudasMuroDocente(publicaciondtoinput dto)
        {
            PublicacionDTOCtrl publicacionDtoCtrl = new PublicacionDTOCtrl();
            return publicacionDtoCtrl.GetDudas(dto);

        }



        [OperationContract]
        public List<publicaciondto> GetPublicacionesContactos(publicaciondtoinput dto)
        {
            PublicacionDTOCtrl publicacionDtoCtrl = new PublicacionDTOCtrl();
            return publicacionDtoCtrl.GetPublicacionesContactos(dto, false);

        }

        [OperationContract]
        public List<publicaciondto> GetPublicacionesDocentes(publicaciondtoinput dto)
        {
            PublicacionDTOCtrl publicacionDtoCtrl = new PublicacionDTOCtrl();
            return publicacionDtoCtrl.GetPublicacionesMurosContactos(dto, true);

        }

        [OperationContract]
        public publicaciondto GetPublicacion(publicaciondto dto)
        {
            PublicacionDTOCtrl publicacionDtoCtrl = new PublicacionDTOCtrl();
            return publicacionDtoCtrl.GetPublicacion(dto);

        }

        [OperationContract]
        public publicaciondto SavePublicacion(publicaciondto publicaciondto)
        {
            PublicacionDTOCtrl publicacionDtoCtrl = new PublicacionDTOCtrl();
            return publicacionDtoCtrl.SavePublicacion(publicaciondto);

        }

        [OperationContract]
        public publicaciondto SaveSugerencia(publicaciondto publicaciondto)
        {
            PublicacionDTOCtrl publicacionDtoCtrl = new PublicacionDTOCtrl();
            return publicacionDtoCtrl.SaveSugerencia(publicaciondto);

        }

        [OperationContract]
        public List<publicaciondto> GetSugerencias(publicaciondtoinput dto)
        {
            PublicacionDTOCtrl publicacionDtoCtrl = new PublicacionDTOCtrl();
            return publicacionDtoCtrl.GetSugerencias(dto);

        }

        [OperationContract]
        public publicaciondto RemovePublicacion(publicaciondto dto)
        {
            PublicacionDTOCtrl publicacionDtoCtrl = new PublicacionDTOCtrl();
            return publicacionDtoCtrl.RemovePublicacion(dto);

        }

        [OperationContract]
        public comentariodto SaveComentario(comentariodto dto)
        {
            ComentarioDTOCtrl comentarioDtoCtrl = new ComentarioDTOCtrl();
            return comentarioDtoCtrl.SaveComentario(dto);
        }


        [OperationContract]
        public comentariodto RemoveComentario(comentariodto dto)
        {
            ComentarioDTOCtrl comentarioDtoCtrl = new ComentarioDTOCtrl();
            return comentarioDtoCtrl.RemoveComentario(dto);

        }

        [OperationContract]
        public publicaciondto MasUnoPublicacion(publicaciondto dto)
        {
            PublicacionDTOCtrl ctrl = new PublicacionDTOCtrl();
            return ctrl.MasUnoPublicacion(dto);
        }

        [OperationContract]
        public comentariodto MasUnoComentario(comentariodto dto)
        {
            ComentarioDTOCtrl comentarioDtoCtrl = new ComentarioDTOCtrl();
            return comentarioDtoCtrl.MasUnoComentario(dto);
        }

        [OperationContract]
        public rankingdto GetPeople(rankingdto dto)
        {
            RankingDTOCtrl rankingDTOCtrl = new RankingDTOCtrl();
            return rankingDTOCtrl.GetPeople(dto);

        }
    }
}
