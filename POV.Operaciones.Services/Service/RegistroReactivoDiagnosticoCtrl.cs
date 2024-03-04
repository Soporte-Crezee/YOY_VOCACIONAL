using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POV.Reactivos.BO;
using POV.Operaciones.BO;
using POV.Reactivos.Service;
using Framework.Base.DataAccess;
using POV.Comun.Service;
using POV.Logger.Service;

namespace POV.Operaciones.Service
{
    public class RegistroReactivoDiagnosticoCtrl
    {
        
        public void InsertReactivoDiagnostico(IDataContext dctx, ReactivoTemporal reactivoTemporal,
            MatrizCalificacionTemporal matriz, string rutaImagenes, string rutaAudios)
        {
            object myFirm = new object();

            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);
            try
            {
                ReactivoCtrl reactivoCtrl = new ReactivoCtrl();
                FileManagerCtrl fileManager = new FileManagerCtrl();
                Reactivo reactivo = TemporalToReactivoDiagnostico(reactivoTemporal, matriz);

                reactivoCtrl.InsertComplete(dctx, reactivo);

                fileManager.CreateFolder(rutaImagenes);
                fileManager.CreateFolder(rutaAudios);
                foreach (PreguntaTemporal pregunta in reactivoTemporal.Preguntas)
                {
                    if (!string.IsNullOrEmpty(pregunta.ImagenEnunciado.ImageName))
                    {
                        byte[] imageDataEnun = pregunta.ImagenEnunciado.ImageData;

                        if (pregunta.ImagenEnunciado.ImageType.StartsWith("audio"))
                            fileManager.WriteFile(string.Format("{0}{1}", rutaAudios, pregunta.ImagenEnunciado.ImageName), ref imageDataEnun);
                        else
                            fileManager.WriteFile(string.Format("{0}{1}", rutaImagenes, pregunta.ImagenEnunciado.ImageName), ref imageDataEnun);
                    }

                    byte[] imageData = pregunta.ImagenOpcion1.ImageData;

                    fileManager.WriteFile(string.Format("{0}{1}", rutaImagenes, pregunta.ImagenOpcion1.ImageName), ref imageData);

                    byte[] imageData1 = pregunta.ImagenOpcion2.ImageData;

                    fileManager.WriteFile(string.Format("{0}{1}", rutaImagenes, pregunta.ImagenOpcion2.ImageName), ref imageData1);

                    byte[] imageData2 = pregunta.ImagenOpcion3.ImageData;

                    fileManager.WriteFile(string.Format("{0}{1}", rutaImagenes, pregunta.ImagenOpcion3.ImageName), ref imageData2);
                }



                dctx.CommitTransaction(myFirm);

            }
            catch (Exception ex)
            {
                dctx.RollbackTransaction(myFirm);
                dctx.CloseConnection(myFirm);
                LoggerHlp.Default.Error(this, ex);
                throw ex;
            }
            finally
            {
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(myFirm);
            }
        }

        public void UpdateReactivoDiagnostico(IDataContext dctx,ReactivoTemporal reactivoTemporal, PreguntaTemporal preguntaUpdate, 
            MatrizCalificacionTemporal matriz, string rutaImagenes, string rutaAudios)
        {
            object myFirm = new object();

            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);
            try
            {
                FileManagerCtrl fileManager = new FileManagerCtrl();
                ReactivoCtrl reactivoCtrl = new ReactivoCtrl();

                Reactivo previous = reactivoTemporal.Reactivo;
                Reactivo reactivoComplete = reactivoCtrl.RetrieveComplete(dctx, previous);


                reactivoComplete = CrearCopiaReactivoDiagnostico(reactivoComplete, preguntaUpdate, matriz);
                reactivoComplete.ReactivoID = reactivoTemporal.newVersionID;

                reactivoCtrl.InsertComplete(dctx, reactivoComplete);
                fileManager.CreateFolder(rutaImagenes);
                fileManager.CreateFolder(rutaAudios);
                if (preguntaUpdate.ImagenEnunciado != null && !string.IsNullOrEmpty(preguntaUpdate.ImagenEnunciado.ImageName))
                {
                    byte[] imageDataEnun = preguntaUpdate.ImagenEnunciado.ImageData;

                    if (preguntaUpdate.ImagenEnunciado.ImageType.StartsWith("audio"))
                        fileManager.WriteFile(string.Format("{0}{1}", rutaAudios, preguntaUpdate.ImagenEnunciado.ImageName), ref imageDataEnun);
                    else
                        fileManager.WriteFile(string.Format("{0}{1}", rutaImagenes, preguntaUpdate.ImagenEnunciado.ImageName), ref imageDataEnun);
                }
                if (preguntaUpdate.ImagenOpcion1 != null)
                {
                    byte[] imageData = preguntaUpdate.ImagenOpcion1.ImageData;

                    fileManager.WriteFile(string.Format("{0}{1}", rutaImagenes, preguntaUpdate.ImagenOpcion1.ImageName), ref imageData);
                }
                if (preguntaUpdate.ImagenOpcion2 != null)
                {
                    byte[] imageData1 = preguntaUpdate.ImagenOpcion2.ImageData;

                    fileManager.WriteFile(string.Format("{0}{1}", rutaImagenes, preguntaUpdate.ImagenOpcion2.ImageName), ref imageData1);
                }
                if (preguntaUpdate.ImagenOpcion3 != null)
                {
                    byte[] imageData2 = preguntaUpdate.ImagenOpcion3.ImageData;

                    fileManager.WriteFile(string.Format("{0}{1}", rutaImagenes, preguntaUpdate.ImagenOpcion3.ImageName), ref imageData2);
                }

                reactivoCtrl.Delete(dctx, previous);

                dctx.CommitTransaction(myFirm);

            }
            catch (Exception ex)
            {
                dctx.RollbackTransaction(myFirm);
                dctx.CloseConnection(myFirm);
                LoggerHlp.Default.Error(this, ex);
                throw ex;
            }
            finally
            {
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(myFirm);
            }
        }

        public void DeletePreguntaReactivoDiagnostico(IDataContext dctx, ReactivoTemporal reactivoTemporal, Pregunta preguntaRemoved, MatrizCalificacionTemporal nuevaMatriz)
        { 
            object myFirm = new object();

            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);
            try
            {
                ReactivoCtrl reactivoCtrl = new ReactivoCtrl();
                //consultamos el reactivo
                Reactivo previous = reactivoTemporal.Reactivo;
                Reactivo reactivoComplete = reactivoCtrl.RetrieveComplete(dctx, previous);

                //removemos la pregunta del reactivo
                Pregunta pregunta = reactivoComplete.Preguntas.FirstOrDefault(item => item.PreguntaID == preguntaRemoved.PreguntaID);

                reactivoComplete.Preguntas.Remove(pregunta);
                reactivoComplete = CrearCopiaReactivoDiagnostico(reactivoComplete, nuevaMatriz);
                reactivoComplete.ReactivoID = reactivoTemporal.newVersionID;

                reactivoCtrl.InsertComplete(dctx, reactivoComplete);

                reactivoCtrl.Delete(dctx, previous);

                dctx.CommitTransaction(myFirm);
            }
            catch (Exception ex)
            {
                dctx.RollbackTransaction(myFirm);
                dctx.CloseConnection(myFirm);
                LoggerHlp.Default.Error(this, ex);
                throw ex;
            }
            finally
            {
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(myFirm);
            }

        }

        public void UpdateMatrizReactivoDiagnostico(IDataContext dctx,ReactivoTemporal reactivoTemporal, MatrizCalificacionTemporal nuevaMatriz)
        {
        object myFirm = new object();

            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);
            try
            {
                ReactivoCtrl reactivoCtrl = new ReactivoCtrl();
                Reactivo previous = reactivoTemporal.Reactivo;
                //consultamos el reactivo
                Reactivo reactivoComplete = reactivoCtrl.RetrieveComplete(dctx, previous);

                reactivoComplete = CrearCopiaReactivoDiagnostico(reactivoComplete, nuevaMatriz);
                reactivoComplete.ReactivoID = reactivoTemporal.newVersionID;

                reactivoCtrl.InsertComplete(dctx, reactivoComplete);

                reactivoCtrl.Delete(dctx, previous);

                dctx.CommitTransaction(myFirm);
            }
            catch (Exception ex)
            {
                dctx.RollbackTransaction(myFirm);
                dctx.CloseConnection(myFirm);
                LoggerHlp.Default.Error(this, ex);
                throw ex;
            }
            finally
            {
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(myFirm);
            }
        }
        
        private Reactivo TemporalToReactivoDiagnostico(ReactivoTemporal reactivoTemporal, MatrizCalificacionTemporal matriz)
        {

            Reactivo reactivo = reactivoTemporal.Reactivo;
            reactivo.Preguntas = new List<Pregunta>();
            List<PreguntaTemporal> preguntasTemporal = reactivoTemporal.Preguntas;

            int index = 0;
            foreach (PreguntaTemporal preguntaTemporal in preguntasTemporal)
            {
                Pregunta pregunta = preguntaTemporal.Pregunta;
                pregunta.PlantillaPregunta = preguntaTemporal.ImagenEnunciado.ImageName;

                List<OpcionRespuestaPlantilla> opciones = new List<OpcionRespuestaPlantilla>();

                OpcionRespuestaPlantilla opcion1 = (pregunta.RespuestaPlantilla as RespuestaPlantillaOpcionMultiple).ListaOpcionRespuestaPlantilla.ElementAtOrDefault(0);

                if (opcion1.EsOpcionCorrecta.Value)
                    opcion1.PorcentajeCalificacion = (matriz.PorcentajeOpcionCorrecta / 100);
                else
                    opcion1.PorcentajeCalificacion = 0;

                opcion1.ImagenUrl = preguntaTemporal.ImagenOpcion1.ImageName;
                opciones.Add(opcion1);

                OpcionRespuestaPlantilla opcion2 = (pregunta.RespuestaPlantilla as RespuestaPlantillaOpcionMultiple).ListaOpcionRespuestaPlantilla.ElementAtOrDefault(1);

                if (opcion2.EsOpcionCorrecta.Value)
                    opcion2.PorcentajeCalificacion = (matriz.PorcentajeOpcionCorrecta / 100);
                else
                    opcion2.PorcentajeCalificacion = 0;

                opcion2.ImagenUrl = preguntaTemporal.ImagenOpcion2.ImageName;
                opciones.Add(opcion2);

                OpcionRespuestaPlantilla opcion3 = (pregunta.RespuestaPlantilla as RespuestaPlantillaOpcionMultiple).ListaOpcionRespuestaPlantilla.ElementAtOrDefault(2);

                if (opcion3.EsOpcionCorrecta.Value)
                    opcion3.PorcentajeCalificacion = (matriz.PorcentajeOpcionCorrecta / 100);
                else
                    opcion3.PorcentajeCalificacion = 0;

                opcion3.ImagenUrl = preguntaTemporal.ImagenOpcion3.ImageName;
                opciones.Add(opcion3);

                (pregunta.RespuestaPlantilla as RespuestaPlantillaOpcionMultiple).ListaOpcionRespuestaPlantilla = opciones;


                pregunta.Valor = matriz.bloques.ElementAtOrDefault(index).ValorBloque;

                reactivo.Preguntas.Add(pregunta);
                index++;
            }

            List<EvaluacionRango> rangos = new List<EvaluacionRango>();

            EvaluacionRango rango1 = reactivoTemporal.Rangos.ElementAtOrDefault(0);
            rango1.Fin = matriz.FinIntervalo1;
            rango1.PorcentajeCalificacion = matriz.PorcentajeIntervalo1 / 100;
            rangos.Add(rango1);

            EvaluacionRango rango2 = reactivoTemporal.Rangos.ElementAtOrDefault(1);
            rango2.Inicio = matriz.FinIntervalo1;
            rango2.Fin = matriz.FinIntervalo2;
            rango2.PorcentajeCalificacion = matriz.PorcentajeIntervalo2 / 100;
            rangos.Add(rango2);

            EvaluacionRango rango3 = reactivoTemporal.Rangos.ElementAtOrDefault(2);
            rango3.Inicio = matriz.FinIntervalo2;
            rango3.PorcentajeCalificacion = matriz.PorcentajeIntervalo3 / 100;
            rangos.Add(rango3);

            (reactivo.Caracteristicas as CaracteristicasDiagnostico).ListaEvaluacionRango = rangos;
            (reactivo.Caracteristicas as CaracteristicasDiagnostico).Version = 1;
            (reactivo.Caracteristicas as CaracteristicasDiagnostico).ReactivoBaseID = reactivo.ReactivoID;

            return reactivo;
        }

        private Reactivo CrearCopiaReactivoDiagnostico(Reactivo reactivo, PreguntaTemporal preguntaTemporal, MatrizCalificacionTemporal matriz)
        {
            if (preguntaTemporal.Pregunta.PreguntaID != null)
            {
                //se recupera el indice de la pregunta
                int indexPregunta = reactivo.Preguntas.FindIndex(item => item.PreguntaID == preguntaTemporal.Pregunta.PreguntaID);

                // la pregunta editada se inserta en el arreglo
                reactivo.Preguntas[indexPregunta] = preguntaTemporal.Pregunta;
            }
            else
            {
                reactivo.Preguntas.Add(preguntaTemporal.Pregunta);
            }
            List<Pregunta> preguntasTemporal = reactivo.Preguntas;
            List<Pregunta> nuevasPreguntas = new List<Pregunta>();
            int index = 0;
            foreach (Pregunta pregunta in preguntasTemporal)
            {


                List<OpcionRespuestaPlantilla> opciones = new List<OpcionRespuestaPlantilla>();

                OpcionRespuestaPlantilla opcion1 = (pregunta.RespuestaPlantilla as RespuestaPlantillaOpcionMultiple).ListaOpcionRespuestaPlantilla.ElementAtOrDefault(0);
                opcion1.OpcionRespuestaPlantillaID = null;
                if (opcion1.EsOpcionCorrecta.Value)
                    opcion1.PorcentajeCalificacion = (matriz.PorcentajeOpcionCorrecta / 100);
                else
                    opcion1.PorcentajeCalificacion = 0;

                opciones.Add(opcion1);

                OpcionRespuestaPlantilla opcion2 = (pregunta.RespuestaPlantilla as RespuestaPlantillaOpcionMultiple).ListaOpcionRespuestaPlantilla.ElementAtOrDefault(1);
                opcion2.OpcionRespuestaPlantillaID = null;
                if (opcion2.EsOpcionCorrecta.Value)
                    opcion2.PorcentajeCalificacion = (matriz.PorcentajeOpcionCorrecta / 100);
                else
                    opcion2.PorcentajeCalificacion = 0;

                opciones.Add(opcion2);

                OpcionRespuestaPlantilla opcion3 = (pregunta.RespuestaPlantilla as RespuestaPlantillaOpcionMultiple).ListaOpcionRespuestaPlantilla.ElementAtOrDefault(2);
                opcion3.OpcionRespuestaPlantillaID = null;
                if (opcion3.EsOpcionCorrecta.Value)
                    opcion3.PorcentajeCalificacion = (matriz.PorcentajeOpcionCorrecta / 100);
                else
                    opcion3.PorcentajeCalificacion = 0;

                opciones.Add(opcion3);

                (pregunta.RespuestaPlantilla as RespuestaPlantillaOpcionMultiple).ListaOpcionRespuestaPlantilla = opciones;


                pregunta.Valor = matriz.bloques.ElementAtOrDefault(index).ValorBloque;
                pregunta.PreguntaID = null;
                pregunta.RespuestaPlantilla.RespuestaPlantillaID = null;
                nuevasPreguntas.Add(pregunta);
                index++;
            }
            CaracteristicasDiagnostico caracteristicas = (reactivo.Caracteristicas as CaracteristicasDiagnostico);
            List<EvaluacionRango> rangos = new List<EvaluacionRango>();

            EvaluacionRango rango1 = caracteristicas.ListaEvaluacionRango.ElementAtOrDefault(0);
            rango1.EvaluacionID = Guid.NewGuid();
            rango1.Fin = matriz.FinIntervalo1;
            rango1.PorcentajeCalificacion = matriz.PorcentajeIntervalo1 / 100;
            rangos.Add(rango1);

            EvaluacionRango rango2 = caracteristicas.ListaEvaluacionRango.ElementAtOrDefault(1);
            rango2.EvaluacionID = Guid.NewGuid();
            rango2.Inicio = matriz.FinIntervalo1;
            rango2.Fin = matriz.FinIntervalo2;
            rango2.PorcentajeCalificacion = matriz.PorcentajeIntervalo2 / 100;
            rangos.Add(rango2);

            EvaluacionRango rango3 = caracteristicas.ListaEvaluacionRango.ElementAtOrDefault(2);
            rango3.EvaluacionID = Guid.NewGuid();
            rango3.Inicio = matriz.FinIntervalo2;
            rango3.PorcentajeCalificacion = matriz.PorcentajeIntervalo3 / 100;
            rangos.Add(rango3);
            
            caracteristicas.ListaEvaluacionRango = rangos;
            caracteristicas.Version = caracteristicas.Version + 1;
            caracteristicas.ReactivoBaseID = reactivo.ReactivoID;

            reactivo.Caracteristicas = caracteristicas;

            return reactivo;
        }

        private Reactivo CrearCopiaReactivoDiagnostico(Reactivo reactivo, MatrizCalificacionTemporal matriz)
        {

            List<Pregunta> preguntasTemporal = reactivo.Preguntas;
            List<Pregunta> nuevasPreguntas = new List<Pregunta>();
            int index = 0;
            foreach (Pregunta pregunta in preguntasTemporal)
            {


                List<OpcionRespuestaPlantilla> opciones = new List<OpcionRespuestaPlantilla>();

                OpcionRespuestaPlantilla opcion1 = (pregunta.RespuestaPlantilla as RespuestaPlantillaOpcionMultiple).ListaOpcionRespuestaPlantilla.ElementAtOrDefault(0);
                opcion1.OpcionRespuestaPlantillaID = null;
                if (opcion1.EsOpcionCorrecta.Value)
                    opcion1.PorcentajeCalificacion = (matriz.PorcentajeOpcionCorrecta / 100);
                else
                    opcion1.PorcentajeCalificacion = 0;

                opciones.Add(opcion1);

                OpcionRespuestaPlantilla opcion2 = (pregunta.RespuestaPlantilla as RespuestaPlantillaOpcionMultiple).ListaOpcionRespuestaPlantilla.ElementAtOrDefault(1);
                opcion2.OpcionRespuestaPlantillaID = null;
                if (opcion2.EsOpcionCorrecta.Value)
                    opcion2.PorcentajeCalificacion = (matriz.PorcentajeOpcionCorrecta / 100);
                else
                    opcion2.PorcentajeCalificacion = 0;

                opciones.Add(opcion2);

                OpcionRespuestaPlantilla opcion3 = (pregunta.RespuestaPlantilla as RespuestaPlantillaOpcionMultiple).ListaOpcionRespuestaPlantilla.ElementAtOrDefault(2);
                opcion3.OpcionRespuestaPlantillaID = null;
                if (opcion3.EsOpcionCorrecta.Value)
                    opcion3.PorcentajeCalificacion = (matriz.PorcentajeOpcionCorrecta / 100);
                else
                    opcion3.PorcentajeCalificacion = 0;

                opciones.Add(opcion3);

                (pregunta.RespuestaPlantilla as RespuestaPlantillaOpcionMultiple).ListaOpcionRespuestaPlantilla = opciones;


                pregunta.Valor = matriz.bloques.ElementAtOrDefault(index).ValorBloque;
                pregunta.PreguntaID = null;
                pregunta.RespuestaPlantilla.RespuestaPlantillaID = null;
                nuevasPreguntas.Add(pregunta);
                index++;
            }
            CaracteristicasDiagnostico caracteristicas = (reactivo.Caracteristicas as CaracteristicasDiagnostico);
            List<EvaluacionRango> rangos = new List<EvaluacionRango>();

            EvaluacionRango rango1 = caracteristicas.ListaEvaluacionRango.ElementAtOrDefault(0);
            rango1.EvaluacionID = Guid.NewGuid();
            rango1.Fin = matriz.FinIntervalo1;
            rango1.PorcentajeCalificacion = matriz.PorcentajeIntervalo1 / 100;

            rangos.Add(rango1);

            EvaluacionRango rango2 = caracteristicas.ListaEvaluacionRango.ElementAtOrDefault(1);
            rango2.EvaluacionID = Guid.NewGuid();
            rango2.Inicio = matriz.FinIntervalo1;
            rango2.Fin = matriz.FinIntervalo2;
            rango2.PorcentajeCalificacion = matriz.PorcentajeIntervalo2 / 100;

            rangos.Add(rango2);

            EvaluacionRango rango3 = caracteristicas.ListaEvaluacionRango.ElementAtOrDefault(2);
            rango3.EvaluacionID = Guid.NewGuid();
            rango3.Inicio = matriz.FinIntervalo2;
            rango3.PorcentajeCalificacion = matriz.PorcentajeIntervalo3 / 100;

            rangos.Add(rango3);

            caracteristicas.ListaEvaluacionRango = rangos;
            caracteristicas.Version = caracteristicas.Version + 1;
            caracteristicas.ReactivoBaseID = reactivo.ReactivoID;

            reactivo.Caracteristicas = caracteristicas;

            return reactivo;
        }
    }
}
