﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using POV.Web.DTO;
using POV.Web.DTO.Services;
namespace POV.Web.Administracion.wcf
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class AlumnoService
    {
        // Para usar HTTP GET, agregue el atributo [WebGet]. (El valor predeterminado de ResponseFormat es WebMessageFormat.Json)
        // Para crear una operación que devuelva XML,
        //     agregue [WebGet(ResponseFormat=WebMessageFormat.Xml)]
        //     e incluya la siguiente línea en el cuerpo de la operación:
        //         WebOperationContext.Current.OutgoingResponse.ContentType = "text/xml";
        [OperationContract]
        public alumnodto ValidateAlumnoAsignadoEscuela(alumnodto dto)
        {
            AlumnoDTOCtrl alumnoDtoCtrl = new AlumnoDTOCtrl();
            return alumnoDtoCtrl.ValidateAlumnoAsignadoEscuela(dto);
        }

        [OperationContract]
        public alumnodto ValidateAspirante(alumnodto dto)
        {
            AlumnoDTOCtrl alumnoDtoCtrl = new AlumnoDTOCtrl();
            return alumnoDtoCtrl.ValidateAspirante(dto);
        }

        // Agregue aquí más operaciones y márquelas con [OperationContract]
    }
}
