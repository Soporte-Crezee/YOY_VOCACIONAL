using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using POV.Profesionalizacion.DTO.Service;
using POV.Profesionalizacion.DTO.BO;

namespace POV.Web.PortalSocial.wcf
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class CursosService
    {
        /// <summary>
        /// Obtiene los cursos que cumplen los criterios  de búsqueda
        /// </summary>
        /// <param name="dto">El dto que proporciona los parametros de consulta</param>
        /// <returns>un dto que contiene la informacion de las situaciones de aprendizaje que cumplen los criterios de consulta</returns>        
        [OperationContract]
        public List<cursodto> GetCursos(cursoinputdto dto)
        {
            CursoDTOCtrl ctrl = new CursoDTOCtrl();
            return ctrl.GetCursos(dto);
        }
        /// <summary>
        /// Obtiene la informacion de un curso y sus contenidos digitales uaciones de aprendizaje que pertenecen a un eje temático y que están activas
        /// </summary>
        /// <param name="dto">El dto que proporciona los parametros de consulta</param>
        /// <returns>un dto que contiene la informacion de las situaciones de aprendizaje que cumplen los criterios de consulta</returns>
        [OperationContract]
        public cursodetalledto GetInformacionCurso(cursoinputdto dto)
        {
            CursoDTOCtrl ctrl = new CursoDTOCtrl();
            return ctrl.GetInformacionCurso(dto);
        }

    }
}
