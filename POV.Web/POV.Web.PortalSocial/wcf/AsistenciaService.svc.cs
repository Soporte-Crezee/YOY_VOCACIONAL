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
    public class AsistenciaService
    {
        // Para usar HTTP GET, agregue el atributo [WebGet]. (El valor predeterminado de ResponseFormat es WebMessageFormat.Json)
        // Para crear una operación que devuelva XML,
        //     agregue [WebGet(ResponseFormat=WebMessageFormat.Xml)]
        //     e incluya la siguiente línea en el cuerpo de la operación:
        //         WebOperationContext.Current.OutgoingResponse.ContentType = "text/xml";
        /// <summary>
        /// Obtiene las asistencias dadas de alta en la base de datos.
        /// </summary>
        /// <param name="dto">Información proporcionada por el usuario por que se realiza la búsqueda.</param>
        /// <returns>Regresa una lista de asistencias encontradas.</returns>
        [OperationContract]
        public List<asistenciadto> SearchAsistencia(asistenciainputdto dto)
        {
            AsistenciaDTOCtrl asistenciaDtoCtrl = new AsistenciaDTOCtrl();
            return asistenciaDtoCtrl.SearchAsistencia(dto);
        }
        /// <summary>
        /// Obtiene los contenidos digitales asociados a la asistencia en cuestión.
        /// </summary>
        /// <param name="dto">Información proporcionada por el usuario por que se realiza la búsqueda.</param>
        /// <returns>Regresa una lista de asistencias encontradas.</returns>
        [OperationContract]
        public contenidodigitaldto GetInformacionContenido(asistenciainputdto dto)
        {
            AsistenciaDTOCtrl asistenciaDtoCtrl = new AsistenciaDTOCtrl();
            return asistenciaDtoCtrl.GetInformaciónAsistencia(dto);
        }
        // Agregue aquí más operaciones y márquelas con [OperationContract]
    }
}
