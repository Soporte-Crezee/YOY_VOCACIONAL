using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POV.Prueba.DTO.BO;
using POV.Prueba.BO;
using POV.Expediente.BO;
using POV.Reactivos.BO;
using POV.Prueba.Diagnostico.Dinamica.BO;
using POV.Prueba.Diagnostico.Service;
using POV.Prueba.Diagnostico.Dinamica.Service;
using POV.Logger.Service;
using POV.Seguridad.Utils;
using POV.CentroEducativo.BO;
using Framework.Base.DataAccess;
using POV.Modelo.BO;


namespace POV.Prueba.DTO.Service
{
    public class PruebaDTOCtrl
    {
        private IResultadoPrueba resultadoPrueba;
        private string urlImagenes;

        public PruebaDTOCtrl(string urlImagenes, IResultadoPrueba prueba)
        {
            this.urlImagenes = urlImagenes;
            this.resultadoPrueba = prueba;
        }

        /// <summary>
        /// Obtiene el siguiente reactivo para presentar, si no existen más reactivos para presentar el sistema finaliza la prueba
        /// </summary>
        /// <param name="dctx">El datacontext que provee la conexion a la base de datos</param>
        /// <param name="dto">DTO</param>
        /// <returns>DTO de reactivo</returns>
        public List<reactivodto> GetNextReactivo(IDataContext dctx, reactivodto dto, APrueba Session_Prueba)
        {
            BancoReactivosDinamicoCtrl bancoReactivosDinamicoCtrl = new BancoReactivosDinamicoCtrl();

            List<reactivodto> reactivoDto = new List<reactivodto>();
            ARegistroPrueba registroPrueba = this.resultadoPrueba.RegistroPrueba as ARegistroPrueba;

            int pregunta;// = new Pregunta();
            List<reactivodto> personalizada = new List<reactivodto>();
            List<int> buscar;// = new List<preguntadto>();
            List<int> existen = new List<int>();
            if (registroPrueba != null)
            {
                var aBancoReactivo = bancoReactivosDinamicoCtrl.LastDataRowToBancoReactivosDinamico(bancoReactivosDinamicoCtrl.Retrieve(dctx, new BancoReactivosDinamico() { Prueba = Session_Prueba }));
                List<ARespuestaReactivo> respuestaReactivos = new List<ARespuestaReactivo>();
                List<ARespuestaReactivo> tmp = new List<ARespuestaReactivo>();
                if (Session_Prueba.TipoPruebaPresentacion == ETipoPruebaPresentacion.TermanMerrill)
                {
                    respuestaReactivos = registroPrueba.GetNextReactivos((Int32)aBancoReactivo.NumeroReactivos);

                    if (respuestaReactivos.Count > 0)
                    {
                        foreach (var respuestaReactivo in respuestaReactivos)
                        {
                            reactivoDto.Add(this.ReactivoToDto(respuestaReactivo, Session_Prueba));
                        }

                        if (reactivoDto.Count > 0)
                        {
                            for (int i = 0; i < reactivoDto.Count; i++)
                            {
                                pregunta = new int();
                                buscar = new List<int>();
                                pregunta = reactivoDto[i].preguntas[0].clasificadorid.Value;
                                buscar.Add(pregunta);
                                if (buscar.Count > 0)
                                {
                                    for (int bus = 0; bus < buscar.Count; bus++)
                                    {
                                        if (personalizada.Count > 0)
                                        {
                                            if (existen.Count>0)
                                            {
                                                if (existen.Contains(buscar[bus]))
                                                    {
                                                        personalizada.Add(reactivoDto[i]);
                                                        pregunta = buscar[bus];
                                                        existen.Add(pregunta);
                                                    }
                                                    else
                                                    {
                                                        reactivoDto = personalizada;
                                                        break;
                                                    }                                             
                                            }                                            
                                        }
                                        else
                                        {
                                            if (!existen.Contains(buscar[bus]))
                                            {
                                                    personalizada.Add(reactivoDto[i]);
                                                    pregunta = buscar[bus];
                                                    existen.Add(pregunta);
                                            }
                                        }

                                    }
                                }
                            }
                        }

                        return reactivoDto;
                    }
                }
                else
                {
                    respuestaReactivos = registroPrueba.GetNextReactivos((Int32)aBancoReactivo.ReactivosPorPagina);
                    if (respuestaReactivos.Count > 0)
                    {
                        foreach (var respuestaReactivo in respuestaReactivos)
                        {
                            reactivoDto.Add(this.ReactivoToDto(respuestaReactivo, Session_Prueba));
                        }
                        return reactivoDto;
                    }
                }
                

                try
                {
                    UpdateEstadoPrueba(dctx);
                    reactivoDto.Add(new reactivodto { esfinal = true, reactivoid = null, tipopruebapresentacion = (byte)Session_Prueba.TipoPruebaPresentacion });
                    return reactivoDto;
                }
                catch (Exception ex)
                {
                    LoggerHlp.Default.Error(this, ex);
                }
                return null;
            }

            return null;
        }

        /// <summary>
        /// Registra una respuesta de un usuario
        /// </summary>
        /// <param name="dctx">El datacontext que provee la conexion a la base de datos</param>
        /// <param name="dto">DTO</param>
        /// <returns>un entero que indica si el proceso fue correcto, -1: error fatal, 0: error, 1:correcto</returns>
        public int RegistrarRespuesta(IDataContext dctx, respuestareactivodto dto)
        {      
            if (dto.tipopruebapresentacion == Convert.ToInt32(ETipoPruebaPresentacion.TermanMerrill))
            {
                Guid rGuid;
                if (!Guid.TryParse(dto.reactivoid, out  rGuid))
                    return 0;


                if (this.resultadoPrueba == null)
                    return -1;

                string hashDTO = GetHash(dto);
                if (hashDTO.CompareTo(dto.hash) != 0)
                    return -1;
                //buscar respuestaReactivo
                ARespuestaReactivo respuestaReactivo = this.resultadoPrueba.RegistroPrueba.ListaRespuestaReactivos.FirstOrDefault(item => item.RespuestaReactivoID == dto.respuestareactivoid);
                //validar hash

                if (respuestaReactivo != null)
                {

                    string hashOriginal = GetHash(respuestaReactivo);

                    if (hashOriginal.CompareTo(dto.hash) != 0)
                        return -1;

                    //actualizar respuesta pregunta
                    return RegistrarRespuestaPruebaDinamica(dctx, respuestaReactivo, dto);


                }
            }
            else
            {
                //validar datos del dto
                if (dto == null || string.IsNullOrEmpty(dto.reactivoid) || dto.respuestareactivoid == null || string.IsNullOrEmpty(dto.reactivoid)
                        || dto.respuestas == null || dto.respuestas.Count == 0 || dto.tipopruebapresentacion == null)
                    return 0;

                Guid rGuid;
                if (!Guid.TryParse(dto.reactivoid, out  rGuid))
                    return 0;


                if (this.resultadoPrueba == null)
                    return -1;

                string hashDTO = GetHash(dto);
                if (hashDTO.CompareTo(dto.hash) != 0)
                    return -1;
                //buscar respuestaReactivo
                ARespuestaReactivo respuestaReactivo = this.resultadoPrueba.RegistroPrueba.ListaRespuestaReactivos.FirstOrDefault(item => item.RespuestaReactivoID == dto.respuestareactivoid);
                //validar hash

                if (respuestaReactivo != null)
                {

                    string hashOriginal = GetHash(respuestaReactivo);

                    if (hashOriginal.CompareTo(dto.hash) != 0)
                        return -1;

                    //actualizar respuesta pregunta
                    return RegistrarRespuestaPruebaDinamica(dctx, respuestaReactivo, dto);


                }
            }
                return -1;
            
        }

        /// <summary>
        /// Registrar una respuesta de prueba dinamica
        /// </summary>
        /// <param name="dctx">El datacontext que provee la conexion a la base de datos</param>
        /// <param name="respuestaReactivo">RespuestaReactivo</param>
        /// <param name="dto">DTO</param>
        /// <returns>un entero que indica si el proceso fue correcto, 0: error, 1:correcto</returns>
        private int RegistrarRespuestaPruebaDinamica(IDataContext dctx, ARespuestaReactivo respuestaReactivo, respuestareactivodto dto)
        {
            RespuestaReactivoDinamicaCtrl respuestaReactivoCtrl = new RespuestaReactivoDinamicaCtrl();
            RespuestaReactivoDinamica respuestaReactivoCopy = (RespuestaReactivoDinamica)respuestaReactivo.Clone();
            respuestaReactivoCopy.EstadoReactivo = EEstadoReactivo.CERRADO;
            respuestaReactivoCopy.Tiempo = dto.tiempo;

            DataSet dsRespuesta = respuestaReactivoCtrl.Retrieve(dctx, this.resultadoPrueba.RegistroPrueba as RegistroPruebaDinamica,
               new RespuestaReactivoDinamica { RespuestaReactivoID = respuestaReactivo.RespuestaReactivoID });
            //validamos que exista el registro
            if (dsRespuesta.Tables[0].Rows.Count > 0)
            {
                RespuestaReactivoDinamica respuestaReactivoActual = respuestaReactivoCtrl.LastDataRowToRespuestaReactivoDinamica(dsRespuesta);
                //validamos que no este cerrado
                if (respuestaReactivoActual.EstadoReactivo == EEstadoReactivo.CERRADO)
                    return -1; //error fatal                                    
            }
            else
            {
                return -1;//error fatal
            }

            #region ** creacion de respuestas **
            foreach (ARespuestaPregunta respuestaPregunta in respuestaReactivoCopy.ListaRespuestaPreguntas)
            {
                respuestapreguntadto respuestadto = dto.respuestas.FirstOrDefault(item => item.respuestapreguntaid == respuestaPregunta.RespuestaPreguntaID);
                respuestaPregunta.EstadoRespuesta = EEstadoRespuesta.CONTESTADA;
                respuestaPregunta.RespuestaAlumno.Tiempo = 0;
                if (respuestadto.opciones != null)
                {
                    if (respuestaPregunta.RespuestaAlumno.TipoRespuestaPlantilla == ETipoRespuestaPlantilla.OPCION_MULTIPLE)
                    {

                        (respuestaPregunta.RespuestaAlumno as RespuestaDinamicaOpcionMultiple).ListaOpcionesRespuesta = new List<OpcionRespuestaPlantilla>();
                        foreach (opciondto opciondto in respuestadto.opciones)
                        {
                            (respuestaPregunta.RespuestaAlumno as RespuestaDinamicaOpcionMultiple).ListaOpcionesRespuesta.Add(new OpcionRespuestaModeloGenerico { OpcionRespuestaPlantillaID = opciondto.opcionid });
                        }

                    }
                    else if (respuestaPregunta.RespuestaAlumno.TipoRespuestaPlantilla == ETipoRespuestaPlantilla.ABIERTA)
                    {
                        (respuestaPregunta.RespuestaAlumno as RespuestaDinamicaAbierta).TextoRespuesta = respuestadto.textorespuesta;
                    }
                    else if (respuestaPregunta.RespuestaAlumno.TipoRespuestaPlantilla == ETipoRespuestaPlantilla.ABIERTA_NUMERICO)
                    {
                        (respuestaPregunta.RespuestaAlumno as RespuestaDinamicaAbierta).ValorRespuesta = respuestadto.valorespuesta;
                    }
                }
                else 
                {
                    if (respuestaPregunta.RespuestaAlumno.TipoRespuestaPlantilla == ETipoRespuestaPlantilla.ABIERTA)
                    {
                        (respuestaPregunta.RespuestaAlumno as RespuestaDinamicaAbierta).TextoRespuesta = respuestadto.textorespuesta;
                    }
                    else if (respuestaPregunta.RespuestaAlumno.TipoRespuestaPlantilla == ETipoRespuestaPlantilla.ABIERTA_NUMERICO)
                    {
                        (respuestaPregunta.RespuestaAlumno as RespuestaDinamicaAbierta).ValorRespuesta = respuestadto.valorespuesta;
                    }
                }
            }
            #endregion

            object myFirm = new object();
            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);
            try
            {


                respuestaReactivoCtrl.UpdateComplete(dctx, respuestaReactivoCopy, respuestaReactivo as RespuestaReactivoDinamica);

                EEstadoPrueba estadoPrueba = (this.resultadoPrueba.RegistroPrueba as ARegistroPrueba).GetEstadoPruebaActual();
                if (this.resultadoPrueba.RegistroPrueba.EstadoPrueba != estadoPrueba)
                    if (estadoPrueba == EEstadoPrueba.ENCURSO)
                    {
                        RegistroPruebaDinamicaCtrl registroPruebaCtrl = new RegistroPruebaDinamicaCtrl();

                        ResultadoPruebaDinamica resultado = new ResultadoPruebaDinamica();
                        resultado.ResultadoPruebaID = (this.resultadoPrueba as ResultadoPruebaDinamica).ResultadoPruebaID;
                        resultado.RegistroPrueba = new RegistroPruebaDinamica { RegistroPruebaID = this.resultadoPrueba.RegistroPrueba.RegistroPruebaID };


                        DataSet ds = registroPruebaCtrl.Retrieve(dctx, resultado, (resultado.RegistroPrueba as RegistroPruebaDinamica));

                        RegistroPruebaDinamica registroPrueba = registroPruebaCtrl.LastDataRowToRegistroPruebaDinamica(ds);
                        RegistroPruebaDinamica previo = registroPruebaCtrl.LastDataRowToRegistroPruebaDinamica(ds);

                        registroPrueba.EstadoPrueba = estadoPrueba;
                        registroPruebaCtrl.Update(dctx, registroPrueba, previo);
                    }

                /*actualizar estado de la prueba en sesión*/
                this.resultadoPrueba.RegistroPrueba.EstadoPrueba = estadoPrueba;
                //*Actualizar los datos del reactivo en sesión*//
                respuestaReactivo.EstadoReactivo = EEstadoReactivo.CERRADO;

                //habilitar LA:
                dctx.CommitTransaction(myFirm);
                return 1;
            }
            catch (Exception ex)
            {
                dctx.RollbackTransaction(myFirm);
                dctx.CloseConnection(myFirm);
                throw ex;
            }
            finally
            {
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(myFirm);
            }
        }

        /// <summary>
        /// Actualiza el estado de la prueba y finaliza la prueba en la base de datos
        /// </summary>
        /// <param name="dctx"></param>
        private void UpdateEstadoPrueba(IDataContext dctx)
        {
            if (this.resultadoPrueba.RegistroPrueba != null)
            {
                PruebaDiagnosticoCtrl pruebaDiagnosticoCtrl = new PruebaDiagnosticoCtrl();
                pruebaDiagnosticoCtrl.FinalizarPrueba(dctx, this.resultadoPrueba as AResultadoPrueba);

                //actualizar estado de la prueba
                (this.resultadoPrueba.RegistroPrueba).UpdateEstadoPrueba();
            }
        }


        private reactivodto ReactivoToDto(ARespuestaReactivo respuestaReactivo, APrueba Session_Prueba)
        {
            if (respuestaReactivo == null && respuestaReactivo.Reactivo == null)
                return null;

            reactivodto dto = new reactivodto();

            dto.hash = GetHash(respuestaReactivo);

            dto.tipopruebapresentacion = (byte)Session_Prueba.TipoPruebaPresentacion;

            dto.esfinal = false;

            dto.reactivoid = respuestaReactivo.Reactivo.ReactivoID.ToString();
            dto.respuestareactivoid = respuestaReactivo.RespuestaReactivoID;
            if (respuestaReactivo.Reactivo.PresentacionPlantilla == EPresentacionPlantilla.TEXTO)
            {
                dto.imagenurl = "";
                dto.texto = respuestaReactivo.Reactivo.Descripcion;
            }
            else if (respuestaReactivo.Reactivo.PresentacionPlantilla == EPresentacionPlantilla.IMAGEN)
            {
                dto.imagenurl = String.Format("{0}{1}", urlImagenes, respuestaReactivo.Reactivo.PlantillaReactivo);
                dto.texto = "";
            }
            else if (respuestaReactivo.Reactivo.PresentacionPlantilla == EPresentacionPlantilla.TEXTOIMAGEN)
            {
                dto.imagenurl = String.Format("{0}{1}", urlImagenes, respuestaReactivo.Reactivo.PlantillaReactivo);
                dto.texto = respuestaReactivo.Reactivo.Descripcion;
            }
            dto.tipopresentacion = (byte?)respuestaReactivo.Reactivo.PresentacionPlantilla;
            dto.tipo = (byte?)respuestaReactivo.Reactivo.TipoReactivo;
            dto.preguntas = new List<preguntadto>();


            foreach (ARespuestaPregunta respuestaPregunta in respuestaReactivo.ListaRespuestaPreguntas)
            {
                preguntadto preguntadto = new preguntadto();
                preguntadto.tipopruebapresentacion = (byte)Session_Prueba.TipoPruebaPresentacion;
                preguntadto.preguntaid = respuestaPregunta.Pregunta.PreguntaID;
                preguntadto.respuestapreguntaid = respuestaPregunta.RespuestaPreguntaID;
                preguntadto.respuestareactivoid = respuestaReactivo.RespuestaReactivoID;

                if (respuestaPregunta.Pregunta.PresentacionPlantilla == EPresentacionPlantilla.TEXTO)
                {
                    preguntadto.texto = respuestaPregunta.Pregunta.TextoPregunta;
                    preguntadto.imagenurl = "";
                }
                else if (respuestaPregunta.Pregunta.PresentacionPlantilla == EPresentacionPlantilla.IMAGEN)
                {
                    preguntadto.texto = "";
                    preguntadto.imagenurl = String.Format("{0}{1}", urlImagenes, respuestaPregunta.Pregunta.PlantillaPregunta);
                }
                else if (respuestaPregunta.Pregunta.PresentacionPlantilla == EPresentacionPlantilla.TEXTOIMAGEN)
                {
                    preguntadto.texto = respuestaPregunta.Pregunta.TextoPregunta;
                    preguntadto.imagenurl = String.Format("{0}{1}", urlImagenes, respuestaPregunta.Pregunta.PlantillaPregunta);
                }

                preguntadto.tipopresentacion = (byte?)respuestaPregunta.Pregunta.PresentacionPlantilla;
                preguntadto.tiporespuesta = (short?)respuestaPregunta.Pregunta.RespuestaPlantilla.TipoRespuestaPlantilla;

                if (respuestaPregunta.Pregunta.RespuestaPlantilla.TipoRespuestaPlantilla == ETipoRespuestaPlantilla.OPCION_MULTIPLE)
                {
                    preguntadto.opciones = new List<opciondto>();
                    RespuestaPlantillaOpcionMultiple respuestaOpcionMultiple = respuestaPregunta.Pregunta.RespuestaPlantilla as RespuestaPlantillaOpcionMultiple;

                    preguntadto.tiposeleccion = (short?)respuestaOpcionMultiple.ModoSeleccion;
                    preguntadto.presentacionopcion = (byte?)respuestaOpcionMultiple.PresentacionOpcion;


                    foreach (OpcionRespuestaPlantilla opcion in respuestaOpcionMultiple.ListaOpcionRespuestaPlantilla)
                    {
                        opciondto opciondto = new opciondto();
                        opciondto.opcionid = opcion.OpcionRespuestaPlantillaID;
                        opciondto.tipopresentacion = preguntadto.presentacionopcion;
                        opciondto.preguntaid = preguntadto.preguntaid;
                        var opciondtotmp = opcion as OpcionRespuestaModeloGenerico;
                        opciondto.clasificadorid = opciondtotmp.Clasificador.ClasificadorID;
                        preguntadto.clasificadorid = opciondto.clasificadorid;


                        if (respuestaOpcionMultiple.PresentacionOpcion == EPresentacionOpcion.TEXTO)
                        {
                            opciondto.texto = opcion.Texto;
                            opciondto.imagenurl = "";
                        }
                        else if (respuestaOpcionMultiple.PresentacionOpcion == EPresentacionOpcion.IMAGEN)
                        {
                            opciondto.texto = "";
                            opciondto.imagenurl = String.Format("{0}{1}", urlImagenes, opcion.ImagenUrl);
                        }

                        preguntadto.opciones.Add(opciondto);
                    }

                }
                else if (respuestaPregunta.Pregunta.RespuestaPlantilla.TipoRespuestaPlantilla == ETipoRespuestaPlantilla.ABIERTA)
                {
                    RespuestaPlantillaTexto respuestaTexto = respuestaPregunta.Pregunta.RespuestaPlantilla as RespuestaPlantillaTexto;
                    preguntadto.escorta = respuestaTexto.EsRespuestaCorta;
                    preguntadto.maximocaracteres = respuestaTexto.MaximoCaracteres;
                    preguntadto.minimocaracteres = respuestaTexto.MinimoCaracteres;

                }
                else if (respuestaPregunta.Pregunta.RespuestaPlantilla.TipoRespuestaPlantilla == ETipoRespuestaPlantilla.ABIERTA_NUMERICO)
                {

                }

                dto.preguntas.Add(preguntadto);

            }

            return dto;

        }

        private string GetHash(respuestareactivodto dto)
        {
            string cadenaToken = dto.reactivoid + dto.respuestareactivoid + this.resultadoPrueba.Alumno.AlumnoID;
            byte[] bytes = EncryptHash.SHA1encrypt(cadenaToken);
            string token = EncryptHash.byteArrayToStringBase64(bytes);

            return token;
        }

        private string GetHash(ARespuestaReactivo reactivo)
        {
            string cadenaToken = reactivo.Reactivo.ReactivoID.ToString() + reactivo.RespuestaReactivoID + this.resultadoPrueba.Alumno.AlumnoID;
            byte[] bytes = EncryptHash.SHA1encrypt(cadenaToken);
            string token = EncryptHash.byteArrayToStringBase64(bytes);

            return token;
        }

        public reactivoesatusbardto GetEstatusReactivos()
        {
            reactivoesatusbardto reactivosEstatusBar = new reactivoesatusbardto();
            ARegistroPrueba registroPrueba = this.resultadoPrueba.RegistroPrueba as ARegistroPrueba;

            if (registroPrueba != null)
            {
                reactivosEstatusBar.TotalReactivos = registroPrueba.ListaRespuestaReactivos.Count;
                reactivosEstatusBar.TotalReactivosContestados = registroPrueba.GetTotalReactivosContestados();
                reactivosEstatusBar.TotalReactivosNoContestados = registroPrueba.GetTotalReactivosNoContestados();
            }

            return reactivosEstatusBar;
        }
    }
}
