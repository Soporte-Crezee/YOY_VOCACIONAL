using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using POV.Reactivos.BO;
using POV.Reactivos.Service;
using POV.ReactivosUsuario.BO;
using POV.ReactivosUsuario.Service;
using POV.Reactivos.DA;
using GP.SocialEngine.Service;
using GP.SocialEngine.BO;
using POV.Core.RedSocial.Implement;
using POV.Core.RedSocial.Interfaces;
using Framework.Base.DataAccess;

namespace POV.Web.DTO.Services
{
    public class PreguntaDTOCtrl
    {
        readonly IDataContext dctx = new DataContext((new DataProviderFactory()).GetDataProvider("POV"));

        public preguntadto DeletePregunta(preguntadto dto)
        {
            PreguntaCtrl preguntaCtrl = new PreguntaCtrl();
            Pregunta pregunta = DTOToPregunta(dto);

            pregunta = preguntaCtrl.RetrieveComplete(dctx, pregunta, new Reactivo { ReactivoID = Guid.Parse(dto.reactivoid) ,TipoReactivo = ETipoReactivo.Estandarizado});

            preguntaCtrl.Delete(dctx, pregunta, new Reactivo { ReactivoID = Guid.Parse(dto.reactivoid), TipoReactivo = ETipoReactivo.Estandarizado });

            return dto;
        }

        public opcionrespuestadto DeleteOpcionPregunta(opcionrespuestadto dto)
        {
            OpcionRespuestaPlantillaCtrl opcionRespuestaCtrl = new OpcionRespuestaPlantillaCtrl();
            OpcionRespuestaPlantilla opcion = DTOToOpcionRespuesta(dto);
            opcionRespuestaCtrl.Delete(dctx, opcion,ETipoReactivo.Estandarizado);

            return dto;
        }

        public Pregunta DTOToPregunta(preguntadto dto)
        {
            Pregunta pregunta = new Pregunta();
            
            if (!string.IsNullOrEmpty(dto.textopregunta))
                pregunta.TextoPregunta = dto.textopregunta;
            if (dto.fecharegistroD != null)
                pregunta.FechaRegistro = dto.fecharegistroD;
            
            if (dto.orden != null)
                pregunta.Orden = dto.orden;
            if (!string.IsNullOrEmpty(dto.plantilla))
                pregunta.PlantillaPregunta = dto.plantilla;
            
            if (dto.preguntaid != null)
                pregunta.PreguntaID = dto.preguntaid;
            if (dto.tipoplantilla != null)
            {
                switch ((ETipoRespuestaPlantilla)dto.tipoplantilla)
                {
                    case ETipoRespuestaPlantilla.ABIERTA:
                        RespuestaPlantillaTexto plantillaAbierta = new RespuestaPlantillaTexto();

                        if (dto.plantillaid != null)
                            plantillaAbierta.RespuestaPlantillaID = dto.plantillaid;

                        plantillaAbierta.TipoRespuestaPlantilla = ETipoRespuestaPlantilla.ABIERTA;
                        plantillaAbierta.MaximoCaracteres = 200;
                        plantillaAbierta.Estatus = true;
                        pregunta.RespuestaPlantilla = plantillaAbierta;

                        break;
                    case ETipoRespuestaPlantilla.OPCION_MULTIPLE:
                        RespuestaPlantillaOpcionMultiple plantillaOpcionMultiple = new RespuestaPlantillaOpcionMultiple();
                        if (dto.plantillaid != null)
                            plantillaOpcionMultiple.RespuestaPlantillaID = dto.plantillaid;

                        plantillaOpcionMultiple.TipoRespuestaPlantilla = ETipoRespuestaPlantilla.OPCION_MULTIPLE;
                        plantillaOpcionMultiple.ListaOpcionRespuestaPlantilla = new List<OpcionRespuestaPlantilla>();
                        plantillaOpcionMultiple.NumeroSeleccionablesMinimo = 1;
                        plantillaOpcionMultiple.NumeroSeleccionablesMaximo = 1;
                        plantillaOpcionMultiple.Estatus = true;
                        plantillaOpcionMultiple.ModoSeleccion = EModoSeleccion.UNICA;
                        foreach (opcionrespuestadto opcion in dto.opciones)
                        {
                            plantillaOpcionMultiple.ListaOpcionRespuestaPlantilla.Add(DTOToOpcionRespuesta(opcion));
                            
                        }
                        pregunta.RespuestaPlantilla = plantillaOpcionMultiple;
                        break;
                }
            }
            return pregunta;
        }

        public OpcionRespuestaPlantilla DTOToOpcionRespuesta(opcionrespuestadto dto)
        {
            OpcionRespuestaPlantilla opcion = new OpcionRespuestaPlantilla();
            opcion.Activo = true;
            if (dto.opcionid != null)
                opcion.OpcionRespuestaPlantillaID = dto.opcionid;
            if (dto.predeterminado != null)
                opcion.EsPredeterminado = dto.predeterminado;
            else
                opcion.EsPredeterminado = false;
            if (!string.IsNullOrEmpty(dto.texto))
                opcion.Texto = dto.texto;
            if (dto.check != null)
                opcion.EsOpcionCorrecta = dto.check;
            return opcion;
        }
        public preguntadto PreguntaToDTO(Pregunta pregunta)
        {
            preguntadto dto = new preguntadto();

            if (pregunta.Orden != null)
                dto.orden = pregunta.Orden;
            if (pregunta.PreguntaID != null)
                dto.preguntaid = pregunta.PreguntaID;
            
            if (pregunta.TextoPregunta != null)
                dto.textopregunta = pregunta.TextoPregunta;
            
            if (pregunta.FechaRegistro != null)
                dto.fecharegistro = String.Format("{0:dd/MM/yyyy}", pregunta.FechaRegistro);
            if (pregunta.PlantillaPregunta != null)
                dto.plantilla = pregunta.PlantillaPregunta;

            RespuestaPlantilla respuestaPlantilla = pregunta.RespuestaPlantilla;
            switch (respuestaPlantilla.TipoRespuestaPlantilla)
            {
                case ETipoRespuestaPlantilla.ABIERTA:
                    RespuestaPlantillaTexto plantillaAbierta = (RespuestaPlantillaTexto)respuestaPlantilla;
                    dto.plantillaid = plantillaAbierta.RespuestaPlantillaID;
                    dto.tipoplantilla = plantillaAbierta.ToShortTipoRespuestaPlantilla;
                    dto.maximocarateres = plantillaAbierta.MaximoCaracteres;
                    break;
                case ETipoRespuestaPlantilla.ABIERTA_NUMERICO:
                    RespuestaPlantillaNumerico plantillaNumerico = (RespuestaPlantillaNumerico) respuestaPlantilla;
                    dto.plantillaid = plantillaNumerico.RespuestaPlantillaID;
                    dto.tipoplantilla = plantillaNumerico.ToShortTipoRespuestaPlantilla;
                    break;
                case ETipoRespuestaPlantilla.OPCION_MULTIPLE:
                    RespuestaPlantillaOpcionMultiple plantillaOpcionMultiple = (RespuestaPlantillaOpcionMultiple)respuestaPlantilla;
                    dto.plantillaid = plantillaOpcionMultiple.RespuestaPlantillaID;
                    dto.tipoplantilla = plantillaOpcionMultiple.ToShortTipoRespuestaPlantilla;
                    dto.opciones = new List<opcionrespuestadto>();
                    foreach (OpcionRespuestaPlantilla opcion in plantillaOpcionMultiple.ListaOpcionRespuestaPlantilla)
                    {
                        opcionrespuestadto opcionrespuestadto = OpcionRespuestaToDTO(opcion);
                        opcionrespuestadto.preguntaid = dto.preguntaid;
                        dto.opciones.Add(opcionrespuestadto);
                    }
                    break;
            }

            return dto;
        }

        public opcionrespuestadto OpcionRespuestaToDTO(OpcionRespuestaPlantilla opcionRespuesta)
        {
            opcionrespuestadto dto = new opcionrespuestadto();
            if (opcionRespuesta.OpcionRespuestaPlantillaID != null)
                dto.opcionid = opcionRespuesta.OpcionRespuestaPlantillaID;
            if (opcionRespuesta.Texto != null)
                dto.texto = opcionRespuesta.Texto;
            if (opcionRespuesta.ImagenUrl != null)
                dto.imagen = opcionRespuesta.ImagenUrl;
            if (opcionRespuesta.EsPredeterminado != null)
                dto.predeterminado = opcionRespuesta.EsPredeterminado;
            if (opcionRespuesta.EsOpcionCorrecta != null)
                dto.check = opcionRespuesta.EsOpcionCorrecta;
            return dto;
        }

    }
}
