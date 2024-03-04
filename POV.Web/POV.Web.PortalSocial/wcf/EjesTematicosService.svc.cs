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
    public class EjesTematicosService
    {

        [OperationContract]
        public List<ejetematicodto> SearchEjesTematico(ejetematicoinputdto dto)
        {
            EjesTematicoDTOCtrl ejeDTOCtrl = new EjesTematicoDTOCtrl();
            return ejeDTOCtrl.SearchEjesTematico(dto);
        }
        /// <summary>
        /// Obtiene las situaciones de aprendizaje que están activas y  que pertenecen a un eje temático 
        /// </summary>
        /// <param name="dto">El dto que proporciona los parametros de consulta</param>
        /// <returns>un dto que contiene la informacion de las situaciones de aprendizaje que cumplen los criterios de consulta</returns>
        [OperationContract]
        public situacionaprendizajeoutputdto GetInformacionEjeTematico(ejetematicoinputdto dto)
        {
            EjesTematicoDTOCtrl ejeDTOCtrl = new EjesTematicoDTOCtrl();
            return ejeDTOCtrl.GetInformacionEjeTematico(dto);
        }
    }
}
