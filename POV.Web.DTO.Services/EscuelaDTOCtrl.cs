using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POV.CentroEducativo.BO;
namespace POV.Web.DTO.Services
{
    public class EscuelaDTOCtrl
    {

        public escueladto ObjectToDto(Escuela escuela)
        {
            escueladto dto = new escueladto ();
            dto.escuelaid = escuela.EscuelaID;
            dto.clave = escuela.Clave;
            dto.turno = escuela.Turno.Value.ToString();
            dto.nombre = escuela.NombreEscuela;
            dto.ambito = escuela.Ambito.Value.ToString();
            dto.control = escuela.Control.Value.ToString();
            dto.tiposervicio = escuela.TipoServicio.Nombre;
            dto.nivel = escuela.TipoServicio.NivelEducativoID.Titulo;
            dto.zona = escuela.ZonaID.Nombre;
            dto.ubicacion = escuela.Ubicacion.Ciudad.Nombre + ", " + escuela.Ubicacion.Localidad.Nombre + ", " + escuela.Ubicacion.Estado.Nombre + ", " + escuela.Ubicacion.Pais.Nombre;

            centrocomputodto centrodto = new centrocomputodto();
            if (escuela.CentroComputo != null && escuela.CentroComputo.CentroComputoID != null)
            {
                

                centrodto.anchobanda = escuela.CentroComputo.AnchoBanda;
                centrodto.centrocomputoid = escuela.CentroComputo.CentroComputoID;
                centrodto.proveedor = escuela.CentroComputo.NombreProveedor;
                centrodto.responsable = escuela.CentroComputo.Responsable;
                centrodto.telefono = escuela.CentroComputo.TelefonoResponsable;
                centrodto.tienecentro = escuela.CentroComputo.TieneCentroComputo;
                centrodto.tieneinternet = escuela.CentroComputo.TieneInternet;
                centrodto.tipocontrato = escuela.CentroComputo.TipoContrato;
                centrodto.numpcs = escuela.CentroComputo.NumeroComputadoras;
            }
            dto.centrocomputo = centrodto;
            return dto;
        }

        public Escuela DtoToObject(escueladto dto)
        {
            Escuela escuela = new Escuela();

            escuela.EscuelaID = dto.escuelaid;
            escuela.CentroComputo = new CentroComputo();

            escuela.CentroComputo.CentroComputoID = dto.centrocomputo.centrocomputoid;
            escuela.CentroComputo.AnchoBanda = dto.centrocomputo.anchobanda;
            escuela.CentroComputo.NombreProveedor = dto.centrocomputo.proveedor;
            escuela.CentroComputo.Responsable = dto.centrocomputo.responsable;
            escuela.CentroComputo.TelefonoResponsable = dto.centrocomputo.telefono;
            escuela.CentroComputo.TieneCentroComputo = dto.centrocomputo.tienecentro;
            escuela.CentroComputo.TieneInternet = dto.centrocomputo.tieneinternet;
            escuela.CentroComputo.NumeroComputadoras = dto.centrocomputo.numpcs;
            escuela.CentroComputo.TipoContrato = dto.centrocomputo.tipocontrato;
            return escuela;
        }
    }
}
