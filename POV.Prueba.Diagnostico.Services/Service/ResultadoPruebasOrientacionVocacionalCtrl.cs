using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Base.DataAccess;
using POV.Reactivos.BO;
using POV.Prueba.BO;
using POV.Expediente.BO;
using POV.Prueba.Diagnostico.Dinamica.BO;
using POV.Prueba.Diagnostico.Service;
using System.Data;
using POV.Modelo.BO;
using POV.Modelo.Service;
using POV.Prueba.Diagnostico.Dinamica.Service;
using POV.Reactivos;
using POV.CentroEducativo.BO;
using POV.Prueba.Diagnostico.DAO.Pruebas;

namespace POV.Prueba.Diagnostico.Service
{
    public class ResultadoPruebasOrientacionVocacionalCtrl
    {

        private ModeloCtrl modeloPruebaCtrl = new ModeloCtrl();
        private RespuestaPlantillaKuderCtrl plantillaKuderCtrl = new RespuestaPlantillaKuderCtrl();
        private RespuestaPlantillaAllportCtrl plantillaAllportCtrl = new RespuestaPlantillaAllportCtrl();

        /// <summary>
        /// Calcula el resultado de la prueba Dominos
        /// </summary>
        /// <param name="dctx">Provee el acceso a la base de datos</param>
        /// <param name="resultadoPruebaID">ID del resultado de la prueba</param>
        /// <param name="resultadoPruebaID">ID de la prueba</param>
        /// <returns>Percentil de acuerdo al resultado</returns>
        public Decimal RetrieveResultadoPruebaDomino(IDataContext dctx, int? resultadoPruebaID, int? pruebaID, Alumno alumno)
        {
            #region Se obtiene la Respuesta Prueba completa
            APrueba prueba = new PruebaDinamica() { PruebaID = pruebaID };

            AResultadoPrueba resultado = new ResultadoPruebaDinamica();
            resultado.ResultadoPruebaID = resultadoPruebaID;
            resultado.Prueba = prueba;

            resultado = new PruebaDiagnosticoCtrl().RetrieveCompleteResultadoPrueba(dctx, resultado);

            if (resultado == null)
                throw new Exception("No se encontró el resultado de la prueba");
            if (!(resultado is ResultadoPruebaDinamica))
                throw new Exception("El resultado no corresponde a una prueba dinámica");
            #endregion

            int totalPreguntas = 0;
            int totalPreguntasCorrectas = 0;
            int puntuacion = 0;

            #region Cálculo del total de preguntas y el total de preguntas correctas
            DataSet respuestasDominos = RetrieveRespuestasPruebaDominos(dctx, alumno);
            foreach (DataRow row in respuestasDominos.Tables[0].Rows)
            {
                puntuacion = 0;
                totalPreguntas++;
                if (!row.IsNull("Valor"))
                    puntuacion = (Int32)Convert.ChangeType(row["Valor"], typeof(Int32));

                if (puntuacion <= 0) continue;
                if (puntuacion == 2) totalPreguntasCorrectas++;


            }
            #endregion

            if (totalPreguntas > 0)
                return (Decimal.Divide(totalPreguntasCorrectas, totalPreguntas) * 100);

            return 0;
        }

        public Decimal RetrieveResultadoPruebaDomino(IDataContext dctx, ResultadoPruebaDinamica resultado, Alumno alumno)
        {
            #region Se obtiene la Respuesta Prueba completa
            if (resultado == null)
                throw new Exception("No se encontró el resultado de la prueba");
            if (!(resultado is ResultadoPruebaDinamica))
                throw new Exception("El resultado no corresponde a una prueba dinámica");

            #endregion

            int totalPreguntas = 0;
            int totalPreguntasCorrectas = 0;
            int puntuacion = 0;
            #region Cálculo del total de preguntas y el total de preguntas correctas
            DataSet respuestasDominos = RetrieveRespuestasPruebaDominos(dctx, alumno);
            foreach (DataRow row in respuestasDominos.Tables[0].Rows)
            {
                puntuacion = 0;
                totalPreguntas++;
                if (!row.IsNull("Valor"))
                    puntuacion = (Int32)Convert.ChangeType(row["Valor"], typeof(Int32));

                if (puntuacion <= 0) continue;
                if (puntuacion == 2) totalPreguntasCorrectas++;
            }
            #endregion

            if (totalPreguntas > 0)
                return (Decimal.Divide(totalPreguntasCorrectas, totalPreguntas) * 100);

            return 0;
        }

        public DataSet RetrieveRespuestasPruebaDominos(IDataContext dctx, Alumno alumno)
        {
            ViewResultadoPruebaDominosRetHlp da = new ViewResultadoPruebaDominosRetHlp();
            DataSet ds = da.Action(dctx, alumno);
            return ds;
        }

        /// <summary>
        /// Calcula el resultado de cada seccion de la prueba Terman
        /// </summary>
        /// <param name="dctx">Provee el acceso a la base de datos</param>
        /// <param name="resultadoPruebaID">ID del resultado de la prueba</param>
        /// <param name="resultadoPruebaID">ID de la prueba</param>
        /// <returns>Puntajes obtenidos en cada percetil</returns>
        public DataSet RetrieveResultadoPruebaTerman(IDataContext dctx, int? resultadoPruebaID, int? pruebaID, Alumno alumno)
        {
            #region Variables
            #region Valores d prueba
            int serieUno = 0, serieDos = 0, serieTres = 0, serieCuatro = 0, serieCinco = 0, serieSeis = 0, serieSiete = 0, serieOcho = 0, serieNueve = 0, serieDiez = 0;
            // Puntaje
            int puntajeSerieI = 0;
            int puntajeSerieII = 0;
            int puntajeSerieIII = 0;
            double puntajeSerieIV = 0;
            int puntajeSerieV = 0;
            int puntajeSerieVI = 0;
            int puntajeSerieVII = 0;
            int puntajeSerieVIII = 0;
            int puntajeSerieIX = 0;
            int puntajeSerieX = 0;

            double resCorrectas = 18.00;
            // Configuraciones por secciones
            int correctasSerieII = 0;
            int correctasSerieIII = 0;
            double correctasSerieIV = 0;
            int correctasSerieV = 0;
            int correctasSerieVI = 0;
            int correctasSerieVIII = 0;
            int correctasSerieX = 0;

            // Incorrectas 
            int incorrectasIII = 0;
            int incorrectasIV = 0;
            int incorrectasVI = 0;
            int incorrectasVIII = 0;
            #endregion
            AModelo LastObject;
            DataTable dtCompose = new DataTable();
            dtCompose.Columns.Add("ClasificadorID");
            dtCompose.Columns.Add("Resultado");

            DataSet dsCompose = new DataSet();
            #endregion

            #region Se obtiene la Respuesta Prueba completa
            APrueba prueba = new PruebaDinamica() { PruebaID = pruebaID };

            AResultadoPrueba resultado = new ResultadoPruebaDinamica();
            resultado.ResultadoPruebaID = resultadoPruebaID;
            resultado.Prueba = prueba;

            resultado = new PruebaDiagnosticoCtrl().RetrieveCompleteResultadoPrueba(dctx, resultado);
            AResultadoPrueba buscarResultado = new ResultadoPruebaDinamica();
            if (resultado == null)
                throw new Exception("No se encontró el resultado de la prueba");
            if (!(resultado is ResultadoPruebaDinamica))
                throw new Exception("El resultado no corresponde a una prueba dinámica");
            #endregion

            #region Cálculo del total de preguntas y el total de preguntas correctas
            DataSet respuestasTerman = RetrieveRespuestasPruebaTerman(dctx, alumno);
            #endregion

            #region Optimizacion
            LastObject = new ModeloDinamico { ModeloID = resultado.Prueba.Modelo.ModeloID };
            List<Clasificador> clasificadores = modeloPruebaCtrl.RetrieveClasificadoresModeloDinamico(dctx, LastObject as ModeloDinamico);


            if (respuestasTerman.Tables[0].Rows.Count > 0)
            {
                DataSet DsClasificadores = new DataSet();
                DsClasificadores = modeloPruebaCtrl.RetrieveClasificador(dctx, new Clasificador { Activo = true }, LastObject as ModeloDinamico);

                DsClasificadores.Tables[0].DefaultView.Sort = "ClasificadorID ASC";
                DataTable dtb = DsClasificadores.Tables[0].DefaultView.ToTable();
                DsClasificadores.Tables[0].Rows.Clear();
                foreach (DataRow row in dtb.Rows)
                    DsClasificadores.Tables[0].Rows.Add(row.ItemArray);

                // Series
                serieUno = Convert.ToInt32(DsClasificadores.Tables[0].Rows[0].ItemArray[0].ToString());
                serieDos = Convert.ToInt32(DsClasificadores.Tables[0].Rows[1].ItemArray[0].ToString());
                serieTres = Convert.ToInt32(DsClasificadores.Tables[0].Rows[2].ItemArray[0].ToString());
                serieCuatro = Convert.ToInt32(DsClasificadores.Tables[0].Rows[3].ItemArray[0].ToString());
                serieCinco = Convert.ToInt32(DsClasificadores.Tables[0].Rows[4].ItemArray[0].ToString());
                serieSeis = Convert.ToInt32(DsClasificadores.Tables[0].Rows[5].ItemArray[0].ToString());
                serieSiete = Convert.ToInt32(DsClasificadores.Tables[0].Rows[6].ItemArray[0].ToString());
                serieOcho = Convert.ToInt32(DsClasificadores.Tables[0].Rows[7].ItemArray[0].ToString());
                serieNueve = Convert.ToInt32(DsClasificadores.Tables[0].Rows[8].ItemArray[0].ToString());
                serieDiez = Convert.ToInt32(DsClasificadores.Tables[0].Rows[9].ItemArray[0].ToString());
                // Cantidad
                int r58 = 0, r59 = 0, r60 = 0, r61 = 0, r62 = 0, r63 = 0, r64 = 0, r65 = 0, r66 = 0, r67 = 0, r68 = 0, r69 = 0, r70 = 0, r71 = 0, r72 = 0, r73 = 0, r74 = 0, r75 = 0;
                #region Calificaciones
                foreach (DataRow row in respuestasTerman.Tables[0].Rows)
                {
                    // Un punto por respuesta correcta
                    serieUno = Convert.ToInt32(DsClasificadores.Tables[0].Rows[0].ItemArray[0].ToString());
                    if (serieUno == (int)Convert.ChangeType(row["ClasificadorID"], typeof(int)))
                    {
                        {
                            if (row["Calificacion"] != DBNull.Value && (int)Convert.ChangeType(row["Calificacion"], typeof(int)) == 1)
                                puntajeSerieI++;

                        }
                    }

                    // Numero de aciertos multiplicados por dos                       
                    if (serieDos == (int)Convert.ChangeType(row["ClasificadorID"], typeof(int)))
                    {
                        {


                            {
                                if (row["Calificacion"] != DBNull.Value && (int)Convert.ChangeType(row["Calificacion"], typeof(int)) == 1)
                                {
                                    correctasSerieII++;
                                    puntajeSerieII = correctasSerieII * 2;

                                }
                            }
                        }
                    }

                    // Numero de aciertos menos numero de incorrectas                        
                    if (serieTres == (int)Convert.ChangeType(row["ClasificadorID"], typeof(int)))
                    {
                        {
                            if (row["Calificacion"] != DBNull.Value && (int)Convert.ChangeType(row["Calificacion"], typeof(int)) == 1)
                            {
                                correctasSerieIII++;
                                incorrectasIII = 30 - correctasSerieIII;

                                puntajeSerieIII = correctasSerieIII - incorrectasIII;
                            }
                        }
                    }

                    // Necesita dos respuetas
                    // Numero de aciertos menos numero de incorrectas                           
                    if (serieCuatro == (int)Convert.ChangeType(row["ClasificadorID"], typeof(int)))
                    {
                        {
                            if (row["Calificacion"] != DBNull.Value)
                            {
                                #region Serie IV
                                if ((string)Convert.ChangeType(row["NombreReactivo"], typeof(string)) == "TERMAN.058")
                                {
                                    if ((int)Convert.ChangeType(row["Calificacion"], typeof(int)) == 1)
                                        r58++;
                                }
                                if ((string)Convert.ChangeType(row["NombreReactivo"], typeof(string)) == "TERMAN.059")
                                {
                                    if ((int)Convert.ChangeType(row["Calificacion"], typeof(int)) == 1)
                                        r59++;
                                }
                                if ((string)Convert.ChangeType(row["NombreReactivo"], typeof(string)) == "TERMAN.060")
                                {
                                    if ((int)Convert.ChangeType(row["Calificacion"], typeof(int)) == 1)
                                        r60++;
                                }
                                if ((string)Convert.ChangeType(row["NombreReactivo"], typeof(string)) == "TERMAN.061")
                                {
                                    if ((int)Convert.ChangeType(row["Calificacion"], typeof(int)) == 1)
                                        r61++;
                                }
                                if ((string)Convert.ChangeType(row["NombreReactivo"], typeof(string)) == "TERMAN.062")
                                {
                                    if ((int)Convert.ChangeType(row["Calificacion"], typeof(int)) == 1)
                                        r62++;
                                }
                                if ((string)Convert.ChangeType(row["NombreReactivo"], typeof(string)) == "TERMAN.063")
                                {
                                    if ((int)Convert.ChangeType(row["Calificacion"], typeof(int)) == 1)
                                        r63++;
                                }
                                if ((string)Convert.ChangeType(row["NombreReactivo"], typeof(string)) == "TERMAN.064")
                                {
                                    if ((int)Convert.ChangeType(row["Calificacion"], typeof(int)) == 1)
                                        r64++;
                                }
                                if ((string)Convert.ChangeType(row["NombreReactivo"], typeof(string)) == "TERMAN.065")
                                {
                                    if ((int)Convert.ChangeType(row["Calificacion"], typeof(int)) == 1)
                                        r65++;
                                }
                                if ((string)Convert.ChangeType(row["NombreReactivo"], typeof(string)) == "TERMAN.066")
                                {
                                    if ((int)Convert.ChangeType(row["Calificacion"], typeof(int)) == 1)
                                        r66++;
                                }
                                if ((string)Convert.ChangeType(row["NombreReactivo"], typeof(string)) == "TERMAN.067")
                                {
                                    if ((int)Convert.ChangeType(row["Calificacion"], typeof(int)) == 1)
                                        r67++;
                                }
                                if ((string)Convert.ChangeType(row["NombreReactivo"], typeof(string)) == "TERMAN.068")
                                {
                                    if ((int)Convert.ChangeType(row["Calificacion"], typeof(int)) == 1)
                                        r68++;
                                }
                                if ((string)Convert.ChangeType(row["NombreReactivo"], typeof(string)) == "TERMAN.069")
                                {
                                    if ((int)Convert.ChangeType(row["Calificacion"], typeof(int)) == 1)
                                        r69++;
                                }
                                if ((string)Convert.ChangeType(row["NombreReactivo"], typeof(string)) == "TERMAN.070")
                                {
                                    if ((int)Convert.ChangeType(row["Calificacion"], typeof(int)) == 1)
                                        r70++;
                                }
                                if ((string)Convert.ChangeType(row["NombreReactivo"], typeof(string)) == "TERMAN.071")
                                {
                                    if ((int)Convert.ChangeType(row["Calificacion"], typeof(int)) == 1)
                                        r71++;
                                }
                                if ((string)Convert.ChangeType(row["NombreReactivo"], typeof(string)) == "TERMAN.072")
                                {
                                    if ((int)Convert.ChangeType(row["Calificacion"], typeof(int)) == 1)
                                        r72++;
                                }
                                if ((string)Convert.ChangeType(row["NombreReactivo"], typeof(string)) == "TERMAN.073")
                                {
                                    if ((int)Convert.ChangeType(row["Calificacion"], typeof(int)) == 1)
                                        r73++;
                                }
                                if ((string)Convert.ChangeType(row["NombreReactivo"], typeof(string)) == "TERMAN.074")
                                {
                                    if ((int)Convert.ChangeType(row["Calificacion"], typeof(int)) == 1)
                                        r74++;
                                }
                                if ((string)Convert.ChangeType(row["NombreReactivo"], typeof(string)) == "TERMAN.075")
                                {
                                    if ((int)Convert.ChangeType(row["Calificacion"], typeof(int)) == 1)
                                        r75++;
                                }
                                #endregion
                            }
                        }
                    }

                    // Numero de aciertos multiplicados por dos
                    if (serieCinco == (int)Convert.ChangeType(row["ClasificadorID"], typeof(int)))
                    {
                        if (row["Calificacion"] != DBNull.Value && (int)Convert.ChangeType(row["Calificacion"], typeof(int)) == 1)
                        {
                            correctasSerieV++;
                            puntajeSerieV = correctasSerieV * 2;
                        }
                    }

                    // Un punto por respuesta correcta

                    if (serieSeis == (int)Convert.ChangeType(row["ClasificadorID"], typeof(int)))
                    {
                        if (row["Calificacion"] != DBNull.Value && (int)Convert.ChangeType(row["Calificacion"], typeof(int)) == 1)
                        {
                            correctasSerieVI++;
                            incorrectasVI = 20 - correctasSerieVI;
                            puntajeSerieVI = correctasSerieVI - incorrectasVI;
                        }
                    }

                    // Un punto por respuesta correcta

                    if (serieSiete == (int)Convert.ChangeType(row["ClasificadorID"], typeof(int)))
                    {
                        if (row["Calificacion"] != DBNull.Value && (int)Convert.ChangeType(row["Calificacion"], typeof(int)) == 1)
                        {
                            puntajeSerieVII++;
                        }
                    }

                    // Numero de aciertos menos numero de incorrectas
                    if (serieOcho == (int)Convert.ChangeType(row["ClasificadorID"], typeof(int)))
                    {
                        if (row["Calificacion"] != DBNull.Value && (int)Convert.ChangeType(row["Calificacion"], typeof(int)) == 1)
                        {
                            correctasSerieVIII++;
                            incorrectasVIII = 17 - correctasSerieVIII;
                            puntajeSerieVIII = correctasSerieVIII - incorrectasVIII;
                        }
                    }

                    // Un punto por respuesta correcta                        
                    if (serieNueve == (int)Convert.ChangeType(row["ClasificadorID"], typeof(int)))
                    {
                        if (row["Calificacion"] != DBNull.Value && (int)Convert.ChangeType(row["Calificacion"], typeof(int)) == 1)
                        {
                            puntajeSerieIX++;
                        }
                    }

                    // Numero de aciertos multiplicados por dos                        

                    if (serieDiez == (int)Convert.ChangeType(row["ClasificadorID"], typeof(int)))
                    {
                        if (row["Calificacion"] != DBNull.Value && (int)Convert.ChangeType(row["Calificacion"], typeof(int)) == 1)
                        {
                            correctasSerieX++;
                            puntajeSerieX = correctasSerieX * 2;
                        }
                    }
                }

                #region Respuesta Serie IV
                if (r58 == 2)
                    correctasSerieIV = correctasSerieIV + 1;
                else if (r58 == 1)
                    correctasSerieIV = correctasSerieIV + 0.5;
                else if (r58 == 0)
                    incorrectasIV++;

                if (r59 == 2)
                    correctasSerieIV = correctasSerieIV + 1;
                else if (r59 == 1)
                    correctasSerieIV = correctasSerieIV + 0.5;
                else if (r59 <= 0)
                    incorrectasIV++;

                if (r60 == 2)
                    correctasSerieIV = correctasSerieIV + 1;
                else if (r60 == 1)
                    correctasSerieIV = correctasSerieIV + 0.5;
                else if (r60 <= 0)
                    incorrectasIV++;

                if (r61 == 2)
                    correctasSerieIV = correctasSerieIV + 1;
                else if (r61 == 1)
                    correctasSerieIV = correctasSerieIV + 0.5;
                else if (r61 <= 0)
                    incorrectasIV++;

                if (r62 == 2)
                    correctasSerieIV = correctasSerieIV + 1;
                else if (r62 == 1)
                    correctasSerieIV = correctasSerieIV + 0.5;
                else if (r62 <= 0)
                    incorrectasIV++;

                if (r63 == 2)
                    correctasSerieIV = correctasSerieIV + 1;
                else if (r63 == 1)
                    correctasSerieIV = correctasSerieIV + 0.5;
                else if (r63 <= 0)
                    incorrectasIV++;

                if (r64 == 2)
                    correctasSerieIV = correctasSerieIV + 1;
                else if (r64 == 1)
                    correctasSerieIV = correctasSerieIV + 0.5;
                else if (r64 <= 0)
                    incorrectasIV++;

                if (r65 == 2)
                    correctasSerieIV = correctasSerieIV + 1;
                else if (r65 == 1)
                    correctasSerieIV = correctasSerieIV + 0.5;
                else if (r65 <= 0)
                    incorrectasIV++;

                if (r66 == 2)
                    correctasSerieIV = correctasSerieIV + 1;
                else if (r66 == 1)
                    correctasSerieIV = correctasSerieIV + 0.5;
                else if (r66 <= 0)
                    incorrectasIV++;

                if (r67 == 2)
                    correctasSerieIV = correctasSerieIV + 1;
                else if (r67 == 1)
                    correctasSerieIV = correctasSerieIV + 0.5;
                else if (r67 <= 0)
                    incorrectasIV++;

                if (r68 == 2)
                    correctasSerieIV = correctasSerieIV + 1;
                else if (r68 == 1)
                    correctasSerieIV = correctasSerieIV + 0.5;
                else if (r68 <= 0)
                    incorrectasIV++;

                if (r69 == 2)
                    correctasSerieIV = correctasSerieIV + 1;
                else if (r69 == 1)
                    correctasSerieIV = correctasSerieIV + 0.5;
                else if (r69 <= 0)
                    incorrectasIV++;

                if (r70 == 2)
                    correctasSerieIV = correctasSerieIV + 1;
                else if (r70 == 1)
                    correctasSerieIV = correctasSerieIV + 0.5;
                else if (r70 <= 0)
                    incorrectasIV++;

                if (r71 == 2)
                    correctasSerieIV = correctasSerieIV + 1;
                else if (r71 == 1)
                    correctasSerieIV = correctasSerieIV + 0.5;
                else if (r71 <= 0)
                    incorrectasIV++;

                if (r72 == 2)
                    correctasSerieIV = correctasSerieIV + 1;
                else if (r72 == 1)
                    correctasSerieIV = correctasSerieIV + 0.5;
                else if (r72 <= 0)
                    incorrectasIV++;

                if (r73 == 2)
                    correctasSerieIV = correctasSerieIV + 1;
                else if (r73 == 1)
                    correctasSerieIV = correctasSerieIV + 0.5;
                else if (r73 <= 0)
                    incorrectasIV++;

                if (r74 == 2)
                    correctasSerieIV = correctasSerieIV + 1;
                else if (r74 == 1)
                    correctasSerieIV = correctasSerieIV + 0.5;
                else if (r74 <= 0)
                    incorrectasIV++;

                if (r75 == 2)
                    correctasSerieIV = correctasSerieIV + 1;
                else if (r75 == 1)
                    correctasSerieIV = correctasSerieIV + 0.5;
                else if (r75 <= 0)
                    incorrectasIV++;
                resCorrectas = correctasSerieIV - incorrectasIV;
                puntajeSerieIV = resCorrectas / 2;
                #endregion


                if (serieTres == 0) puntajeSerieIII = -30;
                if (serieCuatro == 0) puntajeSerieIV = -18 / 2;
                if (serieSeis == 0) puntajeSerieVI = -20;
                if (serieSeis != 0)
                    if (correctasSerieVI == 0)
                        puntajeSerieVI = -20;
                if (serieOcho == 0) puntajeSerieVIII = -17;
                if (serieOcho != 0)
                    if (correctasSerieVIII == 0)
                        puntajeSerieVIII = -17;

                #endregion
                #region Elimininacion negativo
                if (puntajeSerieI < 0)
                    puntajeSerieI = 0;
                if (puntajeSerieII < 0)
                    puntajeSerieII = 0;
                if (puntajeSerieIII < 0)
                    puntajeSerieIII = 0;
                if (puntajeSerieIV < 0)
                    puntajeSerieIV = 0;
                if (puntajeSerieV < 0)
                    puntajeSerieV = 0;
                if (puntajeSerieVI < 0)
                    puntajeSerieVI = 0;
                if (puntajeSerieVII < 0)
                    puntajeSerieVII = 0;
                if (puntajeSerieVIII < 0)
                    puntajeSerieVIII = 0;
                if (puntajeSerieIX < 0)
                    puntajeSerieIX = 0;
                if (puntajeSerieX < 0)
                    puntajeSerieX = 0;
                #endregion
                #region Llenado DataSet
                DataRow drCompose;
                drCompose = dtCompose.NewRow();
                drCompose["ClasificadorID"] = serieUno;
                drCompose["Resultado"] = puntajeSerieI;
                dtCompose.Rows.Add(drCompose);

                drCompose = dtCompose.NewRow();
                drCompose["ClasificadorID"] = serieDos;
                drCompose["Resultado"] = puntajeSerieII;
                dtCompose.Rows.Add(drCompose);

                drCompose = dtCompose.NewRow();
                drCompose["ClasificadorID"] = serieTres;
                drCompose["Resultado"] = puntajeSerieIII;
                dtCompose.Rows.Add(drCompose);

                drCompose = dtCompose.NewRow();
                drCompose["ClasificadorID"] = serieCuatro;
                drCompose["Resultado"] = puntajeSerieIV;
                dtCompose.Rows.Add(drCompose);

                drCompose = dtCompose.NewRow();
                drCompose["ClasificadorID"] = serieCinco;
                drCompose["Resultado"] = puntajeSerieV;
                dtCompose.Rows.Add(drCompose);

                drCompose = dtCompose.NewRow();
                drCompose["ClasificadorID"] = serieSeis;
                drCompose["Resultado"] = puntajeSerieVI;
                dtCompose.Rows.Add(drCompose); ;

                drCompose = dtCompose.NewRow();
                drCompose["ClasificadorID"] = serieSiete;
                drCompose["Resultado"] = puntajeSerieVII;
                dtCompose.Rows.Add(drCompose);

                drCompose = dtCompose.NewRow();
                drCompose["ClasificadorID"] = serieOcho;
                drCompose["Resultado"] = puntajeSerieVIII;
                dtCompose.Rows.Add(drCompose);

                drCompose = dtCompose.NewRow();
                drCompose["ClasificadorID"] = serieNueve;
                drCompose["Resultado"] = puntajeSerieIX;
                dtCompose.Rows.Add(drCompose);

                drCompose = dtCompose.NewRow();
                drCompose["ClasificadorID"] = serieDiez;
                drCompose["Resultado"] = puntajeSerieX;
                dtCompose.Rows.Add(drCompose);


                dsCompose.Tables.Add(dtCompose);
                #endregion
            }
            #endregion

            return dsCompose;
        }

        public DataSet RetrieveResultadoPruebaTerman(IDataContext dctx, ResultadoPruebaDinamica resultado, Alumno alumno)
        {
            #region Variables
            #region Valores de prueba
            int serieUno = 0, serieDos = 0, serieTres = 0, serieCuatro = 0, serieCinco = 0, serieSeis = 0, serieSiete = 0, serieOcho = 0, serieNueve = 0, serieDiez = 0;
            // Puntaje
            int puntajeSerieI = 0;
            int puntajeSerieII = 0;
            int puntajeSerieIII = 0;
            double puntajeSerieIV = 0;
            int puntajeSerieV = 0;
            int puntajeSerieVI = 0;
            int puntajeSerieVII = 0;
            int puntajeSerieVIII = 0;
            int puntajeSerieIX = 0;
            int puntajeSerieX = 0;

            double resCorrectas = 18.00;
            // Configuraciones por secciones
            int correctasSerieII = 0;
            int correctasSerieIII = 0;
            double correctasSerieIV = 0;
            int correctasSerieV = 0;
            int correctasSerieVI = 0;
            int correctasSerieVIII = 0;
            int correctasSerieX = 0;

            // Incorrectas 
            int incorrectasIII = 0;
            int incorrectasIV = 0;
            int incorrectasVI = 0;
            int incorrectasVIII = 0;
            #endregion

            AModelo LastObject;

            DataTable dtCompose = new DataTable();
            dtCompose.Columns.Add("ClasificadorID");
            dtCompose.Columns.Add("Resultado");

            DataSet dsCompose = new DataSet();
            #endregion

            #region Se obtiene la Respuesta Prueba completa
            AResultadoPrueba buscarResultado = new ResultadoPruebaDinamica();
            if (resultado == null)
                throw new Exception("No se encontró el resultado de la prueba");
            if (!(resultado is ResultadoPruebaDinamica))
                throw new Exception("El resultado no corresponde a una prueba dinámica");
            #endregion

            #region Cálculo del total de preguntas y el total de preguntas correctas
            DataSet respuestasTerman = RetrieveRespuestasPruebaTerman(dctx, alumno);
            #endregion

            #region Optimizacion
            LastObject = new ModeloDinamico { ModeloID = resultado.Prueba.Modelo.ModeloID };
            List<Clasificador> clasificadores = modeloPruebaCtrl.RetrieveClasificadoresModeloDinamico(dctx, LastObject as ModeloDinamico);


            if (respuestasTerman.Tables[0].Rows.Count > 0)
            {
                DataSet DsClasificadores = new DataSet();
                DsClasificadores = modeloPruebaCtrl.RetrieveClasificador(dctx, new Clasificador { Activo = true }, LastObject as ModeloDinamico);

                DsClasificadores.Tables[0].DefaultView.Sort = "ClasificadorID ASC";
                DataTable dtb = DsClasificadores.Tables[0].DefaultView.ToTable();
                DsClasificadores.Tables[0].Rows.Clear();
                foreach (DataRow row in dtb.Rows)
                    DsClasificadores.Tables[0].Rows.Add(row.ItemArray);

                // Series
                serieUno = Convert.ToInt32(DsClasificadores.Tables[0].Rows[0].ItemArray[0].ToString());
                serieDos = Convert.ToInt32(DsClasificadores.Tables[0].Rows[1].ItemArray[0].ToString());
                serieTres = Convert.ToInt32(DsClasificadores.Tables[0].Rows[2].ItemArray[0].ToString());
                serieCuatro = Convert.ToInt32(DsClasificadores.Tables[0].Rows[3].ItemArray[0].ToString());
                serieCinco = Convert.ToInt32(DsClasificadores.Tables[0].Rows[4].ItemArray[0].ToString());
                serieSeis = Convert.ToInt32(DsClasificadores.Tables[0].Rows[5].ItemArray[0].ToString());
                serieSiete = Convert.ToInt32(DsClasificadores.Tables[0].Rows[6].ItemArray[0].ToString());
                serieOcho = Convert.ToInt32(DsClasificadores.Tables[0].Rows[7].ItemArray[0].ToString());
                serieNueve = Convert.ToInt32(DsClasificadores.Tables[0].Rows[8].ItemArray[0].ToString());
                serieDiez = Convert.ToInt32(DsClasificadores.Tables[0].Rows[9].ItemArray[0].ToString());
                // Cantidad
                int r58 = 0, r59 = 0, r60 = 0, r61 = 0, r62 = 0, r63 = 0, r64 = 0, r65 = 0, r66 = 0, r67 = 0, r68 = 0, r69 = 0, r70 = 0, r71 = 0, r72 = 0, r73 = 0, r74 = 0, r75 = 0;
                #region Calificaciones
                foreach (DataRow row in respuestasTerman.Tables[0].Rows)
                {
                    // Un punto por respuesta correcta
                    serieUno = Convert.ToInt32(DsClasificadores.Tables[0].Rows[0].ItemArray[0].ToString());
                    if (serieUno == (int)Convert.ChangeType(row["ClasificadorID"], typeof(int)))
                    {
                        {
                            if (row["Calificacion"] != DBNull.Value && (int)Convert.ChangeType(row["Calificacion"], typeof(int)) == 1)
                                puntajeSerieI++;

                        }
                    }


                    // Numero de aciertos multiplicados por dos                       
                    if (serieDos == (int)Convert.ChangeType(row["ClasificadorID"], typeof(int)))
                    {
                        {
                            {
                                if (row["Calificacion"] != DBNull.Value && (int)Convert.ChangeType(row["Calificacion"], typeof(int)) == 1)
                                {
                                    correctasSerieII++;
                                    puntajeSerieII = correctasSerieII * 2;

                                }
                            }
                        }
                    }

                    // Numero de aciertos menos numero de incorrectas                        
                    if (serieTres == (int)Convert.ChangeType(row["ClasificadorID"], typeof(int)))
                    {
                        {
                            if (row["Calificacion"] != DBNull.Value && (int)Convert.ChangeType(row["Calificacion"], typeof(int)) == 1)
                            {
                                correctasSerieIII++;
                                incorrectasIII = 30 - correctasSerieIII;

                                puntajeSerieIII = correctasSerieIII - incorrectasIII;
                            }
                        }
                    }

                    // Necesita dos respuetas
                    // Numero de aciertos menos numero de incorrectas                           
                    if (serieCuatro == (int)Convert.ChangeType(row["ClasificadorID"], typeof(int)))
                    {
                        {
                            if (row["Calificacion"] != DBNull.Value)
                            {
                                #region Serie IV
                                if ((string)Convert.ChangeType(row["NombreReactivo"], typeof(string)) == "TERMAN.058")
                                {
                                    if ((int)Convert.ChangeType(row["Calificacion"], typeof(int)) == 1)
                                        r58++;
                                }
                                if ((string)Convert.ChangeType(row["NombreReactivo"], typeof(string)) == "TERMAN.059")
                                {
                                    if ((int)Convert.ChangeType(row["Calificacion"], typeof(int)) == 1)
                                        r59++;
                                }
                                if ((string)Convert.ChangeType(row["NombreReactivo"], typeof(string)) == "TERMAN.060")
                                {
                                    if ((int)Convert.ChangeType(row["Calificacion"], typeof(int)) == 1)
                                        r60++;
                                }
                                if ((string)Convert.ChangeType(row["NombreReactivo"], typeof(string)) == "TERMAN.061")
                                {
                                    if ((int)Convert.ChangeType(row["Calificacion"], typeof(int)) == 1)
                                        r61++;
                                }
                                if ((string)Convert.ChangeType(row["NombreReactivo"], typeof(string)) == "TERMAN.062")
                                {
                                    if ((int)Convert.ChangeType(row["Calificacion"], typeof(int)) == 1)
                                        r62++;
                                }
                                if ((string)Convert.ChangeType(row["NombreReactivo"], typeof(string)) == "TERMAN.063")
                                {
                                    if ((int)Convert.ChangeType(row["Calificacion"], typeof(int)) == 1)
                                        r63++;
                                }
                                if ((string)Convert.ChangeType(row["NombreReactivo"], typeof(string)) == "TERMAN.064")
                                {
                                    if ((int)Convert.ChangeType(row["Calificacion"], typeof(int)) == 1)
                                        r64++;
                                }
                                if ((string)Convert.ChangeType(row["NombreReactivo"], typeof(string)) == "TERMAN.065")
                                {
                                    if ((int)Convert.ChangeType(row["Calificacion"], typeof(int)) == 1)
                                        r65++;
                                }
                                if ((string)Convert.ChangeType(row["NombreReactivo"], typeof(string)) == "TERMAN.066")
                                {
                                    if ((int)Convert.ChangeType(row["Calificacion"], typeof(int)) == 1)
                                        r66++;
                                }
                                if ((string)Convert.ChangeType(row["NombreReactivo"], typeof(string)) == "TERMAN.067")
                                {
                                    if ((int)Convert.ChangeType(row["Calificacion"], typeof(int)) == 1)
                                        r67++;
                                }
                                if ((string)Convert.ChangeType(row["NombreReactivo"], typeof(string)) == "TERMAN.068")
                                {
                                    if ((int)Convert.ChangeType(row["Calificacion"], typeof(int)) == 1)
                                        r68++;
                                }
                                if ((string)Convert.ChangeType(row["NombreReactivo"], typeof(string)) == "TERMAN.069")
                                {
                                    if ((int)Convert.ChangeType(row["Calificacion"], typeof(int)) == 1)
                                        r69++;
                                }
                                if ((string)Convert.ChangeType(row["NombreReactivo"], typeof(string)) == "TERMAN.070")
                                {
                                    if ((int)Convert.ChangeType(row["Calificacion"], typeof(int)) == 1)
                                        r70++;
                                }
                                if ((string)Convert.ChangeType(row["NombreReactivo"], typeof(string)) == "TERMAN.071")
                                {
                                    if ((int)Convert.ChangeType(row["Calificacion"], typeof(int)) == 1)
                                        r71++;
                                }
                                if ((string)Convert.ChangeType(row["NombreReactivo"], typeof(string)) == "TERMAN.072")
                                {
                                    if ((int)Convert.ChangeType(row["Calificacion"], typeof(int)) == 1)
                                        r72++;
                                }
                                if ((string)Convert.ChangeType(row["NombreReactivo"], typeof(string)) == "TERMAN.073")
                                {
                                    if ((int)Convert.ChangeType(row["Calificacion"], typeof(int)) == 1)
                                        r73++;
                                }
                                if ((string)Convert.ChangeType(row["NombreReactivo"], typeof(string)) == "TERMAN.074")
                                {
                                    if ((int)Convert.ChangeType(row["Calificacion"], typeof(int)) == 1)
                                        r74++;
                                }
                                if ((string)Convert.ChangeType(row["NombreReactivo"], typeof(string)) == "TERMAN.075")
                                {
                                    if ((int)Convert.ChangeType(row["Calificacion"], typeof(int)) == 1)
                                        r75++;
                                }
                                #endregion
                            }
                        }
                    }

                    // Numero de aciertos multiplicados por dos

                    if (serieCinco == (int)Convert.ChangeType(row["ClasificadorID"], typeof(int)))
                    {
                        if (row["Calificacion"] != DBNull.Value && (int)Convert.ChangeType(row["Calificacion"], typeof(int)) == 1)
                        {
                            correctasSerieV++;

                            puntajeSerieV = correctasSerieV * 2;
                        }
                    }

                    // Un punto por respuesta correcta

                    if (serieSeis == (int)Convert.ChangeType(row["ClasificadorID"], typeof(int)))
                    {
                        if (row["Calificacion"] != DBNull.Value && (int)Convert.ChangeType(row["Calificacion"], typeof(int)) == 1)
                        {
                            correctasSerieVI++;

                            incorrectasVI = 20 - correctasSerieVI;

                            puntajeSerieVI = correctasSerieVI - incorrectasVI;
                        }
                    }

                    // Un punto por respuesta correcta

                    if (serieSiete == (int)Convert.ChangeType(row["ClasificadorID"], typeof(int)))
                    {
                        if (row["Calificacion"] != DBNull.Value && (int)Convert.ChangeType(row["Calificacion"], typeof(int)) == 1)
                        {
                            puntajeSerieVII++;
                        }
                    }

                    // Numero de aciertos menos numero de incorrectas

                    if (serieOcho == (int)Convert.ChangeType(row["ClasificadorID"], typeof(int)))
                    {
                        if (row["Calificacion"] != DBNull.Value && (int)Convert.ChangeType(row["Calificacion"], typeof(int)) == 1)
                        {
                            correctasSerieVIII++;

                            incorrectasVIII = 17 - correctasSerieVIII;

                            puntajeSerieVIII = correctasSerieVIII - incorrectasVIII;
                        }
                    }

                    // Un punto por respuesta correcta                        

                    if (serieNueve == (int)Convert.ChangeType(row["ClasificadorID"], typeof(int)))
                    {
                        if (row["Calificacion"] != DBNull.Value && (int)Convert.ChangeType(row["Calificacion"], typeof(int)) == 1)
                        {
                            puntajeSerieIX++;
                        }
                    }

                    // Numero de aciertos multiplicados por dos                        

                    if (serieDiez == (int)Convert.ChangeType(row["ClasificadorID"], typeof(int)))
                    {
                        if (row["Calificacion"] != DBNull.Value && (int)Convert.ChangeType(row["Calificacion"], typeof(int)) == 1)
                        {
                            correctasSerieX++;

                            puntajeSerieX = correctasSerieX * 2;
                        }
                    }


                }

                #region Respuesta Serie IV
                if (r58 == 2)
                    correctasSerieIV = correctasSerieIV + 1;
                else if (r58 == 1)
                    correctasSerieIV = correctasSerieIV + 0.5;
                else if (r58 == 0)
                    incorrectasIV++;
                if (r59 == 2)
                    correctasSerieIV = correctasSerieIV + 1;
                else if (r59 == 1)
                    correctasSerieIV = correctasSerieIV + 0.5;
                else if (r59 <= 0)
                    incorrectasIV++;

                if (r60 == 2)
                    correctasSerieIV = correctasSerieIV + 1;
                else if (r60 == 1)
                    correctasSerieIV = correctasSerieIV + 0.5;
                else if (r60 <= 0)
                    incorrectasIV++;

                if (r61 == 2)
                    correctasSerieIV = correctasSerieIV + 1;
                else if (r61 == 1)
                    correctasSerieIV = correctasSerieIV + 0.5;
                else if (r61 <= 0)
                    incorrectasIV++;

                if (r62 == 2)
                    correctasSerieIV = correctasSerieIV + 1;
                else if (r62 == 1)
                    correctasSerieIV = correctasSerieIV + 0.5;
                else if (r62 <= 0)
                    incorrectasIV++;

                if (r63 == 2)
                    correctasSerieIV = correctasSerieIV + 1;
                else if (r63 == 1)
                    correctasSerieIV = correctasSerieIV + 0.5;
                else if (r63 <= 0)
                    incorrectasIV++;

                if (r64 == 2)
                    correctasSerieIV = correctasSerieIV + 1;
                else if (r64 == 1)
                    correctasSerieIV = correctasSerieIV + 0.5;
                else if (r64 <= 0)
                    incorrectasIV++;

                if (r65 == 2)
                    correctasSerieIV = correctasSerieIV + 1;
                else if (r65 == 1)
                    correctasSerieIV = correctasSerieIV + 0.5;
                else if (r65 <= 0)
                    incorrectasIV++;

                if (r66 == 2)
                    correctasSerieIV = correctasSerieIV + 1;
                else if (r66 == 1)
                    correctasSerieIV = correctasSerieIV + 0.5;
                else if (r66 <= 0)
                    incorrectasIV++;

                if (r67 == 2)
                    correctasSerieIV = correctasSerieIV + 1;
                else if (r67 == 1)
                    correctasSerieIV = correctasSerieIV + 0.5;
                else if (r67 <= 0)
                    incorrectasIV++;

                if (r68 == 2)
                    correctasSerieIV = correctasSerieIV + 1;
                else if (r68 == 1)
                    correctasSerieIV = correctasSerieIV + 0.5;
                else if (r68 <= 0)
                    incorrectasIV++;

                if (r69 == 2)
                    correctasSerieIV = correctasSerieIV + 1;
                else if (r69 == 1)
                    correctasSerieIV = correctasSerieIV + 0.5;
                else if (r69 <= 0)
                    incorrectasIV++;

                if (r70 == 2)
                    correctasSerieIV = correctasSerieIV + 1;
                else if (r70 == 1)
                    correctasSerieIV = correctasSerieIV + 0.5;
                else if (r70 <= 0)
                    incorrectasIV++;

                if (r71 == 2)
                    correctasSerieIV = correctasSerieIV + 1;
                else if (r71 == 1)
                    correctasSerieIV = correctasSerieIV + 0.5;
                else if (r71 <= 0)
                    incorrectasIV++;

                if (r72 == 2)
                    correctasSerieIV = correctasSerieIV + 1;
                else if (r72 == 1)
                    correctasSerieIV = correctasSerieIV + 0.5;
                else if (r72 <= 0)
                    incorrectasIV++;

                if (r73 == 2)
                    correctasSerieIV = correctasSerieIV + 1;
                else if (r73 == 1)
                    correctasSerieIV = correctasSerieIV + 0.5;
                else if (r73 <= 0)
                    incorrectasIV++;

                if (r74 == 2)
                    correctasSerieIV = correctasSerieIV + 1;
                else if (r74 == 1)
                    correctasSerieIV = correctasSerieIV + 0.5;
                else if (r74 <= 0)
                    incorrectasIV++;

                if (r75 == 2)
                    correctasSerieIV = correctasSerieIV + 1;
                else if (r75 == 1)
                    correctasSerieIV = correctasSerieIV + 0.5;
                else if (r75 <= 0)
                    incorrectasIV++;
                resCorrectas = correctasSerieIV - incorrectasIV;
                puntajeSerieIV = resCorrectas / 2;
                #endregion


                if (serieTres == 0) puntajeSerieIII = -30;
                if (serieCuatro == 0) puntajeSerieIV = -18 / 2;
                if (serieSeis == 0) puntajeSerieVI = -20;
                if (serieSeis != 0)
                    if (correctasSerieVI == 0)
                        puntajeSerieVI = -20;
                if (serieOcho == 0) puntajeSerieVIII = -17;
                if (serieOcho != 0)
                    if (correctasSerieVIII == 0)
                        puntajeSerieVIII = -17;

                #endregion
                #region Elimininacion negativo
                if (puntajeSerieI < 0)
                    puntajeSerieI = 0;
                if (puntajeSerieII < 0)
                    puntajeSerieII = 0;
                if (puntajeSerieIII < 0)
                    puntajeSerieIII = 0;
                if (puntajeSerieIV < 0)
                    puntajeSerieIV = 0;
                if (puntajeSerieV < 0)
                    puntajeSerieV = 0;
                if (puntajeSerieVI < 0)
                    puntajeSerieVI = 0;
                if (puntajeSerieVII < 0)
                    puntajeSerieVII = 0;
                if (puntajeSerieVIII < 0)
                    puntajeSerieVIII = 0;
                if (puntajeSerieIX < 0)
                    puntajeSerieIX = 0;
                if (puntajeSerieX < 0)
                    puntajeSerieX = 0;
                #endregion
                #region Llenado DataSet
                DataRow drCompose;
                drCompose = dtCompose.NewRow();
                drCompose["ClasificadorID"] = serieUno;
                drCompose["Resultado"] = puntajeSerieI;
                dtCompose.Rows.Add(drCompose);

                drCompose = dtCompose.NewRow();
                drCompose["ClasificadorID"] = serieDos;
                drCompose["Resultado"] = puntajeSerieII;
                dtCompose.Rows.Add(drCompose);

                drCompose = dtCompose.NewRow();
                drCompose["ClasificadorID"] = serieTres;
                drCompose["Resultado"] = puntajeSerieIII;
                dtCompose.Rows.Add(drCompose);

                drCompose = dtCompose.NewRow();
                drCompose["ClasificadorID"] = serieCuatro;
                drCompose["Resultado"] = puntajeSerieIV;
                dtCompose.Rows.Add(drCompose);

                drCompose = dtCompose.NewRow();
                drCompose["ClasificadorID"] = serieCinco;
                drCompose["Resultado"] = puntajeSerieV;
                dtCompose.Rows.Add(drCompose);

                drCompose = dtCompose.NewRow();
                drCompose["ClasificadorID"] = serieSeis;
                drCompose["Resultado"] = puntajeSerieVI;
                dtCompose.Rows.Add(drCompose); ;

                drCompose = dtCompose.NewRow();
                drCompose["ClasificadorID"] = serieSiete;
                drCompose["Resultado"] = puntajeSerieVII;
                dtCompose.Rows.Add(drCompose);

                drCompose = dtCompose.NewRow();
                drCompose["ClasificadorID"] = serieOcho;
                drCompose["Resultado"] = puntajeSerieVIII;
                dtCompose.Rows.Add(drCompose);

                drCompose = dtCompose.NewRow();
                drCompose["ClasificadorID"] = serieNueve;
                drCompose["Resultado"] = puntajeSerieIX;
                dtCompose.Rows.Add(drCompose);

                drCompose = dtCompose.NewRow();
                drCompose["ClasificadorID"] = serieDiez;
                drCompose["Resultado"] = puntajeSerieX;
                dtCompose.Rows.Add(drCompose);


                dsCompose.Tables.Add(dtCompose);
                #endregion

            }
            #endregion

            return dsCompose;
        }

        public DataSet RetrieveRespuestasPruebaTerman(IDataContext dctx, Alumno alumno)
        {
            ViewResultadoPruebaTermanRetHlp da = new ViewResultadoPruebaTermanRetHlp();
            DataSet ds = da.Action(dctx, alumno);
            return ds;
        }

        /// <summary>
        /// Calcula el resultado de cada seccion de la prueba Kuder
        /// </summary>
        /// <param name="dctx">Provee el acceso a la base de datos</param>
        /// <param name="resultadoPruebaID">ID del resultado de la prueba</param>
        /// <param name="resultadoPruebaID">ID de la prueba</param>
        /// <returns>Puntajes obtenidos en cada percetil</returns>
        public Dictionary<string, string> RetrieveResultadoPruebaKuder(IDataContext dctx, int? resultadoPruebaID, int? pruebaID, Alumno alumno)
        {
            #region Se obtiene la Respuesta Prueba completa
            APrueba prueba = new PruebaDinamica() { PruebaID = pruebaID };

            AResultadoPrueba resultado = new ResultadoPruebaDinamica();
            resultado.ResultadoPruebaID = resultadoPruebaID;
            resultado.Prueba = prueba;

            resultado = new PruebaDiagnosticoCtrl().RetrieveCompleteResultadoPrueba(dctx, resultado);

            if (resultado == null)
                throw new Exception("No se encontró el resultado de la prueba");
            if (!(resultado is ResultadoPruebaDinamica))
                throw new Exception("El resultado no corresponde a una prueba dinámica");

            ResultadoPruebaDinamica resultadoDinamica = resultado as ResultadoPruebaDinamica;
            #endregion

            #region Variables
            int totalPreguntas = 0;
            int totalPreguntasCorrectas = 0;
            int porgrupo = 0;
            int cont = 0;
            #endregion

            PruebaDinamica pruebaDinamica = new PruebaDinamica { PruebaID = pruebaID };

            AModelo LastObject;
            LastObject = new ModeloDinamico { ModeloID = resultado.Prueba.Modelo.ModeloID };

            DataSet dsClasificadores = new DataSet();
            dsClasificadores = modeloPruebaCtrl.RetrieveClasificador(dctx, new Clasificador { Activo = true }, LastObject as ModeloDinamico);

            DataSet dsPlantillasKuder = new DataSet();
            dsPlantillasKuder = plantillaKuderCtrl.Retrieve(dctx, new Clasificador());

            #region Cálculo del total de preguntas y el total de preguntas correctas
            DataSet respuestasKuder = RetrieveRespuestasPruebaKuder(dctx, alumno);
            #endregion

            #region Llenado DataSet
            DataSet dsRespuestaPlantillasKuderCopy = dsPlantillasKuder.Copy();
            // Se obtiene las plantillas
            DataTable dsPlantillas = dsRespuestaPlantillasKuderCopy.Tables[0].DefaultView.ToTable(true, new string[] { "Plantilla" });
            // Se obtienen los grupos
            DataTable dsGrupos = dsRespuestaPlantillasKuderCopy.Tables[0].DefaultView.ToTable(true, new string[] { "Grupo" });
            DataSet dsPlantillasUnicas = new DataSet();
            dsPlantillasUnicas.Tables.Add(dsPlantillas);
            DataSet dsGrupoUnicas = new DataSet();
            dsGrupoUnicas.Tables.Add(dsGrupos);
            // Se ordenan los grupos ASC
            dsGrupoUnicas.Tables[0].DefaultView.Sort = "Grupo ASC";
            DataTable dtbgrupo = dsGrupoUnicas.Tables[0].DefaultView.ToTable();
            dsGrupoUnicas.Tables[0].Rows.Clear();
            foreach (DataRow row in dtbgrupo.Rows)
                dsGrupoUnicas.Tables[0].Rows.Add(row.ItemArray);
            #endregion

            // Se crean la lista de Respuesta correctas Kuder a partir de DataSet
            List<PlantillaKuder> listaRespuestasKuder = new List<PlantillaKuder>();
            if (dsPlantillasKuder.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in dsPlantillasKuder.Tables[0].Rows)
                {
                    listaRespuestasKuder.Add(DataRowToPlantillaKuder(row));
                }
            }

            // Se crean la lista de Respuesta Kuder del alumno a partir de DataSet
            List<RespuestaAlumnoKuder> listaRespuestasAlumnoKuder = new List<RespuestaAlumnoKuder>();
            if (respuestasKuder.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in respuestasKuder.Tables[0].Rows)
                {
                    listaRespuestasAlumnoKuder.Add(DataRowToRespuestaAlumnoKuder(row));
                }
            }

            List<RespuestaAlumnoKuder> listRespuestaAlumnotmp = new List<RespuestaAlumnoKuder>();
            List<PlantillaKuder> listRespuestaKudertmp = new List<PlantillaKuder>();
            List<RespuestaAlumnoKuder> listResultado = new List<RespuestaAlumnoKuder>();
            Dictionary<string, string> resultadoPlantilla = new Dictionary<string, string>();

            if (dsGrupoUnicas.Tables[0].Rows.Count > 0)
            {
                if (dsPlantillasUnicas.Tables[0].Rows.Count > 0)
                {
                    for (int p = 0; p < dsPlantillasUnicas.Tables[0].Rows.Count; p++)
                    {
                        for (int c = 0; c < dsClasificadores.Tables[0].Rows.Count; c++)
                        {
                            for (int i = 0; i < dsGrupoUnicas.Tables[0].Rows.Count; i++)
                            {
                                int grupo = Convert.ToInt32(dsGrupoUnicas.Tables[0].Rows[i].ItemArray[0]);
                                int clasificadorID = Convert.ToInt32(dsClasificadores.Tables[0].Rows[c].ItemArray[0]);
                                string plantilla = dsPlantillasUnicas.Tables[0].Rows[p].ItemArray[0].ToString();
                                listRespuestaAlumnotmp = listaRespuestasAlumnoKuder.Where(x => x.Grupo == grupo && x.ClasificadorID == clasificadorID).ToList();
                                listRespuestaKudertmp = listaRespuestasKuder.Where(x => x.Grupo == grupo && x.ClasificadorID == clasificadorID && x.Plantilla == plantilla).ToList();

                                if (listRespuestaAlumnotmp.Count > 0 && listRespuestaKudertmp.Count > 0)
                                {
                                    listResultado = (from item in listRespuestaAlumnotmp
                                                     from item2 in listRespuestaKudertmp
                                                     where item.GradoInteres == item2.GradoInteres && item.RespuestaOpcion == item2.RespuestaOpcion
                                                     select item).ToList();
                                }
                                else
                                {
                                    continue;
                                }

                                if (listResultado.Count > 0)
                                {
                                    if (listResultado.Count == 3)
                                    {
                                        cont = 2;
                                        porgrupo += cont;
                                    }
                                    if (listResultado.Count <= 2)
                                    {
                                        cont = listResultado.Count;
                                        porgrupo += cont;
                                    }
                                }
                            }
                        }

                        if (porgrupo > 0)
                        {
                            resultadoPlantilla.Add(dsPlantillasUnicas.Tables[0].Rows[p].ItemArray[0].ToString(), porgrupo.ToString());
                            porgrupo = 0;
                        }
                    }
                }
            }

            Dictionary<string, string> areas = new Dictionary<string, string>();
            if (resultadoPlantilla.Count > 0)
            {
                var sorted = from item in resultadoPlantilla orderby item.Value descending select item;
                int tresprimero = 0;
                if (sorted != null)
                {
                    foreach (var item in sorted)
                    {
                        tresprimero++;
                        if (tresprimero <= 3)
                        {
                            areas.Add(item.Key, item.Value);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }

            return areas;
        }

        public Dictionary<string, string> RetrieveResultadoPruebaKuder(IDataContext dctx, ResultadoPruebaDinamica resultado, Alumno alumno)
        {
            #region Se obtiene la Respuesta Prueba completa
            if (resultado == null)
                throw new Exception("No se encontró el resultado de la prueba");
            if (!(resultado is ResultadoPruebaDinamica))
                throw new Exception("El resultado no corresponde a una prueba dinámica");

            ResultadoPruebaDinamica resultadoDinamica = resultado as ResultadoPruebaDinamica;
            #endregion

            #region Variables
            int totalPreguntas = 0;
            int totalPreguntasCorrectas = 0;
            int porgrupo = 0;
            int cont = 0;
            #endregion

            AModelo LastObject;
            LastObject = new ModeloDinamico { ModeloID = resultado.Prueba.Modelo.ModeloID };

            DataSet dsClasificadores = new DataSet();
            dsClasificadores = modeloPruebaCtrl.RetrieveClasificador(dctx, new Clasificador { Activo = true }, LastObject as ModeloDinamico);

            DataSet dsPlantillasKuder = new DataSet();
            dsPlantillasKuder = plantillaKuderCtrl.Retrieve(dctx, new Clasificador());

            #region Cálculo del total de preguntas y el total de preguntas correctas
            DataSet respuestasKuder = RetrieveRespuestasPruebaKuder(dctx, alumno);
            #endregion

            #region Llenado DataSet
            DataSet dsRespuestaPlantillasKuderCopy = dsPlantillasKuder.Copy();
            // Se obtiene las plantillas
            DataTable dsPlantillas = dsRespuestaPlantillasKuderCopy.Tables[0].DefaultView.ToTable(true, new string[] { "Plantilla" });
            // Se obtienen los grupos
            DataTable dsGrupos = dsRespuestaPlantillasKuderCopy.Tables[0].DefaultView.ToTable(true, new string[] { "Grupo" });
            DataSet dsPlantillasUnicas = new DataSet();
            dsPlantillasUnicas.Tables.Add(dsPlantillas);
            DataSet dsGrupoUnicas = new DataSet();
            dsGrupoUnicas.Tables.Add(dsGrupos);
            // Se ordenan los grupos ASC
            dsGrupoUnicas.Tables[0].DefaultView.Sort = "Grupo ASC";
            DataTable dtbgrupo = dsGrupoUnicas.Tables[0].DefaultView.ToTable();
            dsGrupoUnicas.Tables[0].Rows.Clear();
            foreach (DataRow row in dtbgrupo.Rows)
                dsGrupoUnicas.Tables[0].Rows.Add(row.ItemArray);
            #endregion

            // Se crean la lista de Respuesta correctas Kuder a partir de DataSet
            List<PlantillaKuder> listaRespuestasKuder = new List<PlantillaKuder>();
            if (dsPlantillasKuder.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in dsPlantillasKuder.Tables[0].Rows)
                {
                    listaRespuestasKuder.Add(DataRowToPlantillaKuder(row));
                }
            }

            // Se crean la lista de Respuesta Kuder del alumno a partir de DataSet
            List<RespuestaAlumnoKuder> listaRespuestasAlumnoKuder = new List<RespuestaAlumnoKuder>();
            if (respuestasKuder.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in respuestasKuder.Tables[0].Rows)
                {
                    listaRespuestasAlumnoKuder.Add(DataRowToRespuestaAlumnoKuder(row));
                }
            }

            List<RespuestaAlumnoKuder> listRespuestaAlumnotmp = new List<RespuestaAlumnoKuder>();
            List<PlantillaKuder> listRespuestaKudertmp = new List<PlantillaKuder>();
            List<RespuestaAlumnoKuder> listResultado = new List<RespuestaAlumnoKuder>();
            Dictionary<string, string> resultadoPlantilla = new Dictionary<string, string>();

            if (dsGrupoUnicas.Tables[0].Rows.Count > 0)
            {
                if (dsPlantillasUnicas.Tables[0].Rows.Count > 0)
                {
                    for (int p = 0; p < dsPlantillasUnicas.Tables[0].Rows.Count; p++)
                    {
                        for (int c = 0; c < dsClasificadores.Tables[0].Rows.Count; c++)
                        {
                            for (int i = 0; i < dsGrupoUnicas.Tables[0].Rows.Count; i++)
                            {
                                int grupo = Convert.ToInt32(dsGrupoUnicas.Tables[0].Rows[i].ItemArray[0]);
                                int clasificadorID = Convert.ToInt32(dsClasificadores.Tables[0].Rows[c].ItemArray[0]);
                                string plantilla = dsPlantillasUnicas.Tables[0].Rows[p].ItemArray[0].ToString();
                                listRespuestaAlumnotmp = listaRespuestasAlumnoKuder.Where(x => x.Grupo == grupo && x.ClasificadorID == clasificadorID).ToList();
                                listRespuestaKudertmp = listaRespuestasKuder.Where(x => x.Grupo == grupo && x.ClasificadorID == clasificadorID && x.Plantilla == plantilla).ToList();

                                if (listRespuestaAlumnotmp.Count > 0 && listRespuestaKudertmp.Count > 0)
                                {
                                    listResultado = (from item in listRespuestaAlumnotmp
                                                     from item2 in listRespuestaKudertmp
                                                     where item.GradoInteres == item2.GradoInteres && item.RespuestaOpcion == item2.RespuestaOpcion
                                                     select item).ToList();
                                }
                                else
                                {
                                    continue;
                                }

                                if (listResultado.Count > 0)
                                {
                                    if (listResultado.Count == 3)
                                    {
                                        cont = 2;
                                        porgrupo += cont;
                                    }
                                    if (listResultado.Count <= 2)
                                    {
                                        cont = listResultado.Count;
                                        porgrupo += cont;
                                    }
                                }
                            }
                        }

                        if (porgrupo > 0)
                        {
                            resultadoPlantilla.Add(dsPlantillasUnicas.Tables[0].Rows[p].ItemArray[0].ToString(), porgrupo.ToString());
                            porgrupo = 0;
                        }
                    }
                }
            }

            Dictionary<string, string> areas = new Dictionary<string, string>();
            if (resultadoPlantilla.Count > 0)
            {
                var sorted = from item in resultadoPlantilla orderby item.Value descending select item;
                int tresprimero = 0;
                if (sorted != null)
                {
                    foreach (var item in sorted)
                    {
                        tresprimero++;
                        if (tresprimero <= 3)
                        {
                            areas.Add(item.Key, item.Value);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }

            return areas;
        }

        public DataSet RetrieveRespuestasPruebaKuder(IDataContext dctx, Alumno alumno)
        {
            ViewResultadoPruebaKuderRetHlp da = new ViewResultadoPruebaKuderRetHlp();
            DataSet ds = da.Action(dctx, alumno);
            return ds;
        }


        /// <summary>
        /// Calcula el resultado de cada seccion de la prueba Allport
        /// </summary>
        /// <param name="dctx">Provee el acceso a la base de datos</param>
        /// <param name="resultadoPruebaID">ID del resultado de la prueba</param>
        /// <param name="resultadoPruebaID">ID de la prueba</param>
        /// <returns>Puntajes obtenidos en cada percetil</returns>
        public Dictionary<string, string> RetrieveResultadoPruebaAllport(IDataContext dctx, int? resultadoPruebaID, int? pruebaID, Alumno alumno)
        {
            #region Se obtiene la Respuesta Prueba completa
            APrueba prueba = new PruebaDinamica() { PruebaID = pruebaID };

            AResultadoPrueba resultado = new ResultadoPruebaDinamica();
            resultado.ResultadoPruebaID = resultadoPruebaID;
            resultado.Prueba = prueba;

            resultado = new PruebaDiagnosticoCtrl().RetrieveCompleteResultadoPrueba(dctx, resultado);

            if (resultado == null)
                throw new Exception("No se encontró el resultado de la prueba");
            if (!(resultado is ResultadoPruebaDinamica))
                throw new Exception("El resultado no corresponde a una prueba dinámica");
            #endregion

            #region Variables
            int totalPreguntas = 0;
            int totalPreguntasCorrectas = 0;
            int porgrupo = 0;
            int cont = 0;
            #endregion
            //List<OpcionRespuestaPlantilla> respuestasCorrectas = new List<OpcionRespuestaPlantilla>();

            PruebaDinamica pruebaDinamica = new PruebaDinamica { PruebaID = pruebaID };

            AModelo LastObject;
            LastObject = new ModeloDinamico { ModeloID = resultado.Prueba.Modelo.ModeloID };

            DataSet dsPlantillasAllport = new DataSet();
            dsPlantillasAllport = plantillaAllportCtrl.Retrieve(dctx, new Clasificador());

            #region Cálculo del total de preguntas y el total de preguntas correctas
            DataSet respuestaAllport = RetrieveRespuestasPruebaAllport(dctx, alumno);
            #endregion

            #region Llenado DataSet
            DataSet dsRespuestaPlantillasAllportCopy = dsPlantillasAllport.Copy();
            // Se obtiene las plantillas
            DataTable dsPlantillas = dsRespuestaPlantillasAllportCopy.Tables[0].DefaultView.ToTable(true, new string[] { "Plantilla" });
            // Se obtienen los grupos
            DataTable dsGrupos = dsRespuestaPlantillasAllportCopy.Tables[0].DefaultView.ToTable(true, new string[] { "Grupo" });
            DataSet dsPlantillasUnicas = new DataSet();
            dsPlantillasUnicas.Tables.Add(dsPlantillas);
            DataSet dsGrupoUnicas = new DataSet();
            dsGrupoUnicas.Tables.Add(dsGrupos);
            // Se ordenan los grupos ASC
            dsGrupoUnicas.Tables[0].DefaultView.Sort = "Grupo ASC";
            DataTable dtbgrupo = dsGrupoUnicas.Tables[0].DefaultView.ToTable();
            dsGrupoUnicas.Tables[0].Rows.Clear();
            foreach (DataRow row in dtbgrupo.Rows)
                dsGrupoUnicas.Tables[0].Rows.Add(row.ItemArray);
            #endregion

            // Se crean la lista de Respuesta Kuder del alumno a partir de DataSet
            List<RespuestaAlumnoAllport> listaRespuestasAlumnoAllport = new List<RespuestaAlumnoAllport>();
            if (respuestaAllport.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in respuestaAllport.Tables[0].Rows)
                {
                    listaRespuestasAlumnoAllport.Add(DataRowToRespuestaAlumnoAllport(row));
                }
            }

            List<RespuestaAlumnoAllport> listRespuestaAlumnotmp = new List<RespuestaAlumnoAllport>();
            Dictionary<string, string> resultadoPlantilla = new Dictionary<string, string>();
            DataSet tmp = new DataSet();

            if (dsPlantillasUnicas.Tables[0].Rows.Count > 0)
            {
                for (int p = 0; p < dsPlantillasUnicas.Tables[0].Rows.Count; p++)
                {
                    string plantillaUnica = dsPlantillasUnicas.Tables[0].Rows[p].ItemArray[0].ToString();
                    if (plantillaUnica != null)
                    {
                        string strFilter = string.Empty;
                        if (!string.IsNullOrEmpty(plantillaUnica))
                        {
                            strFilter += string.Format("AND Plantilla LIKE '%{0}%' ", EscapeLike(plantillaUnica));
                        }

                        if (strFilter.StartsWith("AND"))
                            strFilter = strFilter.Substring(4);

                        if (strFilter.Trim().Length > 0)
                        {
                            tmp = dsRespuestaPlantillasAllportCopy.Copy();
                            DataView dataView = new DataView(tmp.Tables[0]);
                            dataView.RowFilter = strFilter;
                            strFilter = string.Empty;
                            tmp.Tables.Clear();
                            tmp.Tables.Add(dataView.ToTable());
                        }
                        if (tmp.Tables[0].Rows.Count > 0)
                        {
                            for (int c = 0; c < tmp.Tables[0].Rows.Count; c++)
                            {
                                int grupo = Convert.ToInt32(tmp.Tables[0].Rows[c].ItemArray[3]);
                                int clasificadorID = Convert.ToInt32(tmp.Tables[0].Rows[c].ItemArray[2]);
                                listRespuestaAlumnotmp = listaRespuestasAlumnoAllport.Where(x => x.Grupo == grupo && x.ClasificadorID == clasificadorID).ToList();
                                if (listRespuestaAlumnotmp.Count > 0)
                                {
                                    foreach (var item in listRespuestaAlumnotmp)
                                    {
                                        porgrupo += int.Parse(item.Valor.ToString());
                                    }
                                }
                            }
                        }
                    }

                    if (porgrupo > 0)
                    {
                        resultadoPlantilla.Add(dsPlantillasUnicas.Tables[0].Rows[p].ItemArray[0].ToString(), porgrupo.ToString());
                        porgrupo = 0;
                    }
                }
            }

            // Ajuste de totales
            Dictionary<string, string> correcion = new Dictionary<string, string>();
            if (resultadoPlantilla.Count > 0)
            {
                foreach (var item in resultadoPlantilla)
                {
                    string plantilla = item.Key;
                    int valor = int.Parse(item.Value);
                    switch (plantilla)
                    {
                        case "1 Teorica":
                            valor = valor - 4;
                            break;
                        case "2 Economico":
                            valor = valor - 5;
                            break;
                        case "3 Estetico":
                            valor = valor + 6;
                            break;
                        case "4 Social":
                            valor = valor - 1;
                            break;
                        case "5 Politico":
                            valor = valor + 3;
                            break;
                        case "6 Religioso":
                            valor = valor + 1;
                            break;
                    }
                    correcion.Add(plantilla, valor.ToString());
                }
            }

            // Se obtienen los primeros tres resultados mas altos
            Dictionary<string, string> resultadoFinal = new Dictionary<string, string>();
            Dictionary<string, string> resultadoFinalTemp = new Dictionary<string, string>();
            if (correcion.Count > 0)
            {
                var sorted = from item in correcion orderby item.Value descending select item;
                int tresprimero = 0;
                if (sorted != null)
                {
                    foreach (var item in sorted)
                    {
                        tresprimero++;
                        if (tresprimero <= 3)
                        {
                            if (item.Key == "3 Estetico" || item.Key == "5 Politico")
                            {
                                switch (item.Key)
                                {
                                    case "3 Estetico":
                                        resultadoFinalTemp.Add("5 Estetico", item.Value);
                                        break;
                                    case "5 Politico":
                                        resultadoFinalTemp.Add("3 Politico", item.Value);
                                        break;
                                }
                            }
                            else
                                resultadoFinalTemp.Add(item.Key, item.Value);
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                var sortedFinal = from item2 in resultadoFinalTemp orderby item2.Key ascending select item2;
                tresprimero = 0;
                if (sortedFinal != null)
                {
                    foreach (var item in sortedFinal)
                    {
                        tresprimero++;
                        if (tresprimero <= 3)
                            resultadoFinal.Add(item.Key, item.Value);

                        else
                            break;
                    }
                }
            }

            return resultadoFinal;
        }

        public Dictionary<string, string> RetrieveResultadoPruebaAllport(IDataContext dctx, ResultadoPruebaDinamica resultado, Alumno alumno)
        {
            #region Se obtiene la Respuesta Prueba completa
            if (resultado == null)
                throw new Exception("No se encontró el resultado de la prueba");
            if (!(resultado is ResultadoPruebaDinamica))
                throw new Exception("El resultado no corresponde a una prueba dinámica");

            ResultadoPruebaDinamica resultadoDinamica = resultado as ResultadoPruebaDinamica;
            #endregion

            #region Variables
            int totalPreguntas = 0;
            int totalPreguntasCorrectas = 0;
            int porgrupo = 0;
            int cont = 0;
            #endregion

            AModelo LastObject;
            LastObject = new ModeloDinamico { ModeloID = resultado.Prueba.Modelo.ModeloID };

            DataSet dsPlantillasAllport = new DataSet();
            dsPlantillasAllport = plantillaAllportCtrl.Retrieve(dctx, new Clasificador());

            #region Cálculo del total de preguntas y el total de preguntas correctas
            DataSet respuestaAllport = RetrieveRespuestasPruebaAllport(dctx, alumno);
            #endregion


            #region Llenado DataSet
            DataSet dsRespuestaPlantillasAllportCopy = dsPlantillasAllport.Copy();
            // Se obtiene las plantillas
            DataTable dsPlantillas = dsRespuestaPlantillasAllportCopy.Tables[0].DefaultView.ToTable(true, new string[] { "Plantilla" });
            // Se obtienen los grupos
            DataTable dsGrupos = dsRespuestaPlantillasAllportCopy.Tables[0].DefaultView.ToTable(true, new string[] { "Grupo" });
            DataSet dsPlantillasUnicas = new DataSet();
            dsPlantillasUnicas.Tables.Add(dsPlantillas);
            DataSet dsGrupoUnicas = new DataSet();
            dsGrupoUnicas.Tables.Add(dsGrupos);
            // Se ordenan los grupos ASC
            dsGrupoUnicas.Tables[0].DefaultView.Sort = "Grupo ASC";
            DataTable dtbgrupo = dsGrupoUnicas.Tables[0].DefaultView.ToTable();
            dsGrupoUnicas.Tables[0].Rows.Clear();
            foreach (DataRow row in dtbgrupo.Rows)
                dsGrupoUnicas.Tables[0].Rows.Add(row.ItemArray);
            #endregion

            // Se crean la lista de Respuesta Kuder del alumno a partir de DataSet
            List<RespuestaAlumnoAllport> listaRespuestasAlumnoAllport = new List<RespuestaAlumnoAllport>();
            if (respuestaAllport.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in respuestaAllport.Tables[0].Rows)
                {
                    listaRespuestasAlumnoAllport.Add(DataRowToRespuestaAlumnoAllport(row));
                }
            }

            List<RespuestaAlumnoAllport> listRespuestaAlumnotmp = new List<RespuestaAlumnoAllport>();
            Dictionary<string, string> resultadoPlantilla = new Dictionary<string, string>();
            DataSet tmp = new DataSet();

            if (dsPlantillasUnicas.Tables[0].Rows.Count > 0)
            {
                for (int p = 0; p < dsPlantillasUnicas.Tables[0].Rows.Count; p++)
                {
                    string plantillaUnica = dsPlantillasUnicas.Tables[0].Rows[p].ItemArray[0].ToString();
                    if (plantillaUnica != null)
                    {
                        string strFilter = string.Empty;
                        if (!string.IsNullOrEmpty(plantillaUnica))
                        {
                            strFilter += string.Format("AND Plantilla LIKE '%{0}%' ", EscapeLike(plantillaUnica));
                        }

                        if (strFilter.StartsWith("AND"))
                            strFilter = strFilter.Substring(4);

                        if (strFilter.Trim().Length > 0)
                        {
                            tmp = dsRespuestaPlantillasAllportCopy.Copy();
                            DataView dataView = new DataView(tmp.Tables[0]);
                            dataView.RowFilter = strFilter;
                            strFilter = string.Empty;
                            tmp.Tables.Clear();
                            tmp.Tables.Add(dataView.ToTable());
                        }
                        if (tmp.Tables[0].Rows.Count > 0)
                        {
                            for (int c = 0; c < tmp.Tables[0].Rows.Count; c++)
                            {
                                int grupo = Convert.ToInt32(tmp.Tables[0].Rows[c].ItemArray[3]);
                                int clasificadorID = Convert.ToInt32(tmp.Tables[0].Rows[c].ItemArray[2]);
                                listRespuestaAlumnotmp = listaRespuestasAlumnoAllport.Where(x => x.Grupo == grupo && x.ClasificadorID == clasificadorID).ToList();
                                if (listRespuestaAlumnotmp.Count > 0)
                                {
                                    foreach (var item in listRespuestaAlumnotmp)
                                    {
                                        porgrupo += int.Parse(item.Valor.ToString());
                                    }
                                }
                            }
                        }
                    }

                    if (porgrupo > 0)
                    {
                        resultadoPlantilla.Add(dsPlantillasUnicas.Tables[0].Rows[p].ItemArray[0].ToString(), porgrupo.ToString());
                        porgrupo = 0;
                    }
                }
            }

            // Ajuste de totales
            Dictionary<string, string> correcion = new Dictionary<string, string>();
            if (resultadoPlantilla.Count > 0)
            {
                foreach (var item in resultadoPlantilla)
                {
                    string plantilla = item.Key;
                    int valor = int.Parse(item.Value);
                    switch (plantilla)
                    {
                        case "1 Teorica":
                            valor = valor - 4;
                            break;
                        case "2 Economico":
                            valor = valor - 5;
                            break;
                        case "3 Estetico":
                            valor = valor + 6;
                            break;
                        case "4 Social":
                            valor = valor - 1;
                            break;
                        case "5 Politico":
                            valor = valor + 3;
                            break;
                        case "6 Religioso":
                            valor = valor + 1;
                            break;
                    }
                    correcion.Add(plantilla, valor.ToString());
                }
            }

            // Se obtienen los primeros tres resultados mas altos
            Dictionary<string, string> resultadoFinal = new Dictionary<string, string>();
            Dictionary<string, string> resultadoFinalTemp = new Dictionary<string, string>();
            if (correcion.Count > 0)
            {
                var sorted = from item in correcion orderby item.Value descending select item;
                int tresprimero = 0;
                if (sorted != null)
                {
                    foreach (var item in sorted)
                    {
                        tresprimero++;
                        if (tresprimero <= 3)
                        {
                            if (item.Key == "3 Estetico" || item.Key == "5 Politico")
                            {
                                switch (item.Key)
                                {
                                    case "3 Estetico":
                                        resultadoFinalTemp.Add("5 Estetico", item.Value);
                                        break;
                                    case "5 Politico":
                                        resultadoFinalTemp.Add("3 Politico", item.Value);
                                        break;
                                }
                            }
                            else
                                resultadoFinalTemp.Add(item.Key, item.Value);
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                var sortedFinal = from item2 in resultadoFinalTemp orderby item2.Key ascending select item2;
                tresprimero = 0;
                if (sortedFinal != null)
                {
                    foreach (var item in sortedFinal)
                    {
                        tresprimero++;
                        if (tresprimero <= 3)
                            resultadoFinal.Add(item.Key, item.Value);

                        else
                            break;
                    }
                }
            }

            return resultadoFinal;
        }

        public DataSet RetrieveRespuestasPruebaAllport(IDataContext dctx, Alumno alumno)
        {
            ViewResultadoPruebaAllportRetHlp da = new ViewResultadoPruebaAllportRetHlp();
            DataSet ds = da.Action(dctx, alumno);
            return ds;
        }


        /// <summary>
        /// Calcula el resultado de cada seccion de la prueba Raven
        /// </summary>
        /// <param name="dctx">Provee el acceso a la base de datos</param>
        /// <param name="resultadoPruebaID">ID del resultado de la prueba</param>
        /// <param name="resultadoPruebaID">ID de la prueba</param>
        /// <returns>Diagnostico de la prueba Raben</returns>
        public DiagnosticoRaven RetrieveResultadoPruebaRaven(IDataContext dctx, ResultadoPruebaDinamica resultado, Alumno alumno)
        {
            #region Se obtiene la Respuesta Prueba completa
            if (resultado == null)
                throw new Exception("No se encontró el resultado de la prueba");
            if (!(resultado is ResultadoPruebaDinamica))
                throw new Exception("El resultado no corresponde a una prueba dinámica");

            ResultadoPruebaDinamica resultadoDinamica = resultado as ResultadoPruebaDinamica;
            #endregion

            #region Variables
            int totalPreguntas = 0;
            int totalPreguntasCorrectas = 0;
            string edadEstudiante = string.Empty; // String de edad estudiante
            // Porcentaje parcial de cada serie
            int puntajeParcialserieA = 0;
            int puntajeParcialserieB = 0;
            int puntajeParcialserieC = 0;
            int puntajeParcialserieD = 0;
            int puntajeParcialserieE = 0;
            // Porcentaje Total (Suma de las series)
            int puntajeTotal = 0;
            // Filtro de serie
            string serieFilter = string.Empty;
            int valorRespuesta = 0;

            // Porcentaje total normalizado solo pra calculo de valores de la tabla de varemo y para la discrepancia
            int puntajeTotalNormalizado = 0;

            // Variables discrepancia
            int discrepanciaSerieA = 0;
            int discrepanciaSerieB = 0;
            int discrepanciaSerieC = 0;
            int discrepanciaSerieD = 0;
            int discrepanciaSerieE = 0;
            int valorEsperado = 0;

            // Clase que contiene el resultado de la Prueba Raven
            DiagnosticoRaven diagnostico = new DiagnosticoRaven();
            #endregion

            //List<OpcionRespuestaPlantilla> respuestasCorrectas = new List<OpcionRespuestaPlantilla>();

            //PruebaDinamica pruebaDinamica = new PruebaDinamica { PruebaID = pruebaID };

            AModelo LastObject;
            LastObject = new ModeloDinamico { ModeloID = resultado.Prueba.Modelo.ModeloID };

            List<Clasificador> listaClasificadoresRaven = modeloPruebaCtrl.RetrieveClasificadoresModeloDinamico(dctx, LastObject as ModeloDinamico);

            DataSet dsDiscrepanciaRaven = new DataSet();

            #region Cálculo del total de preguntas y el total de preguntas correctas
            DataSet respuestasRaven = RetrieveRespuestasPruebaRaven(dctx, alumno);
            #endregion

            #region Calculo de puntajes parciales
            if (respuestasRaven.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in respuestasRaven.Tables[0].Rows)
                {
                    serieFilter = string.Empty;
                    valorRespuesta = 0;
                    if (row.IsNull("ClasificadorID"))
                    {
                        edadEstudiante = (string)Convert.ChangeType(row["Respuesta"], typeof(string));
                        string[] separador = edadEstudiante.Split(')');
                        edadEstudiante = separador[1].ToString().Trim();
                    }

                    if (!row.IsNull("NombreClasificador"))
                        serieFilter = (string)Convert.ChangeType(row["NombreClasificador"], typeof(string));
                    if (!row.IsNull("Valor"))
                        valorRespuesta = (int)Convert.ChangeType(row["Valor"], typeof(int));

                    if (!string.IsNullOrEmpty(serieFilter) && valorRespuesta > 0)
                    {
                        switch (serieFilter)
                        {
                            case "Serie A":
                                puntajeParcialserieA++;
                                break;
                            case "Serie B":
                                puntajeParcialserieB++;
                                break;
                            case "Serie C":
                                puntajeParcialserieC++;
                                break;
                            case "Serie D":
                                puntajeParcialserieD++;
                                break;
                            case "Serie E":
                                puntajeParcialserieE++;
                                break;
                        }
                    }
                }
            }
            puntajeTotal = puntajeParcialserieA + puntajeParcialserieB + puntajeParcialserieC + puntajeParcialserieD + puntajeParcialserieE;
            #endregion

            CalculoResultadoRavenCtrl calculoResultadoRavenCtrl = new CalculoResultadoRavenCtrl();
            // Clase para la obtencion del resultado de la tabla de Baremo
            ResultadoBaremoRaven resultadoBaremoRaven = new ResultadoBaremoRaven();
            #region Normalizacion para TablaBaremo y Discrepancia
            // Se adecua el puntaje total de acuerdo a los valores de la tabla de Baremo
            puntajeTotalNormalizado = NormalizarPuntajePrevioBaremo(edadEstudiante, puntajeTotal);
            resultadoBaremoRaven = calculoResultadoRavenCtrl.LastDatRowResultadoBaremo(calculoResultadoRavenCtrl.RetrieveTablaBaremo(dctx, edadEstudiante, puntajeTotalNormalizado));

            if (resultadoBaremoRaven != null)
            {
                diagnostico = DiagnosticoCapacidadIntelectualRaven(resultadoBaremoRaven.Percentil);
                diagnostico.Edad = edadEstudiante;
                diagnostico.Puntaje = puntajeTotal;
            }

            // Normalizacion de puntajeTotal para discrepancia
            puntajeTotalNormalizado = NormalizarPuntajePrevioDiscrepancia(puntajeTotalNormalizado);
            #endregion

            #region Otención de discrepancia para validez de la prueba
            Dictionary<string, int> puntuacionEsperada = new Dictionary<string, int>();
            Dictionary<string, int> discrepancias = new Dictionary<string, int>();
            // Obtencion de Discrepancia
            dsDiscrepanciaRaven = calculoResultadoRavenCtrl.RetrieveDiscrepanciaRaven(dctx, puntajeTotalNormalizado);
            // Parea calcular la discrepancia se resta el puntajeparcial - puntajeesperado
            // Para que la prueba sea válida debe obtenerse un valor discrepancia entre -2 a +2 en cada serie
            if (dsDiscrepanciaRaven.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in dsDiscrepanciaRaven.Tables[0].Rows)
                {
                    valorEsperado = 0;
                    serieFilter = string.Empty;
                    if (!row.IsNull("Serie"))
                        serieFilter = (string)Convert.ChangeType(row["Serie"], typeof(string));

                    if (!string.IsNullOrEmpty(serieFilter))
                    {
                        switch (serieFilter)
                        {
                            case "SerieA":
                                if (!row.IsNull("PuntajeEquivalente"))
                                    valorEsperado = (int)Convert.ChangeType(row["PuntajeEquivalente"], typeof(int));
                                discrepanciaSerieA = puntajeParcialserieA - valorEsperado;
                                puntuacionEsperada.Add("A", valorEsperado);
                                discrepancias.Add("A", discrepanciaSerieA);
                                break;
                            case "SerieB":
                                if (!row.IsNull("PuntajeEquivalente"))
                                    valorEsperado = (int)Convert.ChangeType(row["PuntajeEquivalente"], typeof(int));
                                discrepanciaSerieB = puntajeParcialserieB - valorEsperado;
                                puntuacionEsperada.Add("B", valorEsperado);
                                discrepancias.Add("B", discrepanciaSerieB);
                                break;
                            case "SerieC":
                                if (!row.IsNull("PuntajeEquivalente"))
                                    valorEsperado = (int)Convert.ChangeType(row["PuntajeEquivalente"], typeof(int));
                                discrepanciaSerieC = puntajeParcialserieC - valorEsperado;
                                puntuacionEsperada.Add("C", valorEsperado);
                                discrepancias.Add("C", discrepanciaSerieC);
                                break;
                            case "SerieD":
                                if (!row.IsNull("PuntajeEquivalente"))
                                    valorEsperado = (int)Convert.ChangeType(row["PuntajeEquivalente"], typeof(int));
                                discrepanciaSerieD = puntajeParcialserieD - valorEsperado;
                                puntuacionEsperada.Add("D", valorEsperado);
                                discrepancias.Add("D", discrepanciaSerieD);
                                break;
                            case "SerieE":
                                if (!row.IsNull("PuntajeEquivalente"))
                                    valorEsperado = (int)Convert.ChangeType(row["PuntajeEquivalente"], typeof(int));
                                discrepanciaSerieE = puntajeParcialserieE - valorEsperado;
                                puntuacionEsperada.Add("E", valorEsperado);
                                discrepancias.Add("E", discrepanciaSerieE);
                                break;
                        }
                    }
                }
            }
            diagnostico.PuntuacionEsperada = puntuacionEsperada;
            diagnostico.Discrepancias = discrepancias;
            #endregion

            #region Puntuacion Directa
            Dictionary<string, int> puntuacionDirecta = new Dictionary<string, int>();
            puntuacionDirecta.Add("A", puntajeParcialserieA);
            puntuacionDirecta.Add("B", puntajeParcialserieB);
            puntuacionDirecta.Add("C", puntajeParcialserieC);
            puntuacionDirecta.Add("D", puntajeParcialserieD);
            puntuacionDirecta.Add("E", puntajeParcialserieE);
            puntuacionDirecta.Add("Total", puntajeTotal);

            diagnostico.PuntuacionDirecta = puntuacionDirecta;
            #endregion

            // Los valores para validez de la prueba estan en un rango de -2 a +2 por serie
            // Si alguna serie esta fuera de este rango, la prueba no es valida
            if (discrepanciaSerieA >= -2 && discrepanciaSerieA <= 2 && discrepanciaSerieB >= -2 && discrepanciaSerieB <= 2 && discrepanciaSerieC >= -2 && discrepanciaSerieC <= 2
                && discrepanciaSerieD >= -2 && discrepanciaSerieD <= 2 && discrepanciaSerieE >= -2 && discrepanciaSerieE <= 2)
                diagnostico.Validez = true;
            else
                diagnostico.Validez = false;

            return diagnostico;
        }

        public DataSet RetrieveRespuestasPruebaRaven(IDataContext dctx, Alumno alumno)
        {
            ViewResultadoPruebaRavenRetHlp da = new ViewResultadoPruebaRavenRetHlp();
            DataSet ds = da.Action(dctx, alumno);
            return ds;
        }

        /// <summary>
        /// Metodo que devuelve el resultado de la capacidad intelectual del evaluado
        /// </summary>
        /// <param name="percentil"> Percentil porporcionado para la busqueda </param>
        /// <returns></returns>
        private DiagnosticoRaven DiagnosticoCapacidadIntelectualRaven(long percentil)
        {
            DiagnosticoRaven diagnostico = new DiagnosticoRaven();
            #region Igual o Superior a
            #region Norma P95
            if (percentil >= 95)
            {
                diagnostico.Percentil = 95;
                diagnostico.Rango = "I";
                diagnostico.Diagnostico = "Superior";
            }
            #endregion
            #region Norma P90
            if (percentil >= 90 && percentil <= 94)
            {
                diagnostico.Percentil = 90;
                diagnostico.Rango = "II +";
                diagnostico.Diagnostico = "Superior al término medio";
            }
            #endregion
            #region Norma P75
            if (percentil >= 75 && percentil <= 89)
            {
                diagnostico.Percentil = 75;
                diagnostico.Rango = "II";
                diagnostico.Diagnostico = "Superior al término medio";
            }
            #endregion
            #endregion
            #region Superior a
            #region Norma P50
            if (percentil > 50 && percentil <= 74)
            {
                diagnostico.Percentil = 50;
                diagnostico.Rango = "III +";
                diagnostico.Diagnostico = "Término medio";
            }
            #endregion
            #region Norma P50
            if (percentil == 50)
            {
                diagnostico.Percentil = 50;
                diagnostico.Rango = "III";
                diagnostico.Diagnostico = "Término medio";
            }
            #endregion
            #region Norma P50
            if (percentil >= 26 && percentil <= 49)
            {
                diagnostico.Percentil = 50;
                diagnostico.Rango = "III -";
                diagnostico.Diagnostico = "Término medio";
            }
            #endregion
            #endregion
            #region Igual o menor a
            #region Norma P25
            if (percentil >= 11 && percentil <= 25)
            {
                diagnostico.Percentil = 25;
                diagnostico.Rango = "IV +";
                diagnostico.Diagnostico = "Inferior al término medio";
            }
            #endregion
            #region Norma P10
            if (percentil >= 6 && percentil <= 10)
            {
                diagnostico.Percentil = 10;
                diagnostico.Rango = "IV";
                diagnostico.Diagnostico = "Inferior al término medio";
            }
            #endregion
            #region Norma P5
            if (percentil <= 5)
            {
                diagnostico.Percentil = 5;
                diagnostico.Rango = "V";
                diagnostico.Diagnostico = "Deficiente";
            }
            #endregion
            #endregion
            return diagnostico;
        }

        /// <summary>
        /// Método que devuelve el puntaje normalizado segun la Tabla de Baremo
        /// </summary>
        /// <param name="edad"> Edad porporcionada para la busqueda</param>
        /// <param name="puntajeTotal"> PuntajeTotal porporcionado para la busqueda</param>
        /// <returns> Retorna el porcentajeTotal normalizado segun la Tabla de Baremo </returns>
        private int NormalizarPuntajePrevioBaremo(string edad, int puntajeTotal)
        {
            int puntajeNormal = 0;
            switch (edad)
            {
                #region 12
                case "12":

                    if (puntajeTotal >= 53)
                        puntajeNormal = 53;
                    else if (puntajeTotal >= 47 && puntajeTotal < 53)
                        puntajeNormal = 47;
                    else if (puntajeTotal >= 43 && puntajeTotal < 47)
                        puntajeNormal = 43;
                    else if (puntajeTotal >= 39 && puntajeTotal < 43)
                        puntajeNormal = 39;
                    else if (puntajeTotal >= 33 && puntajeTotal < 39)
                        puntajeNormal = 33;
                    else if (puntajeTotal >= 24 && puntajeTotal < 33)
                        puntajeNormal = 24;
                    else if (puntajeTotal < 24)
                        puntajeNormal = 14;
                    break;
                #endregion
                #region 13-14
                case "13-14":
                    if (puntajeTotal >= 54)
                        puntajeNormal = 54;
                    else if (puntajeTotal >= 49 && puntajeTotal < 54)
                        puntajeNormal = 49;
                    else if (puntajeTotal >= 45 && puntajeTotal < 49)
                        puntajeNormal = 45;
                    else if (puntajeTotal >= 40 && puntajeTotal < 45)
                        puntajeNormal = 40;
                    else if (puntajeTotal >= 34 && puntajeTotal < 40)
                        puntajeNormal = 34;
                    else if (puntajeTotal >= 27 && puntajeTotal < 34)
                        puntajeNormal = 27;
                    else if (puntajeTotal < 27)
                        puntajeNormal = 17;
                    break;
                #endregion
                #region 15-16
                case "15-16":
                    if (puntajeTotal >= 55)
                        puntajeNormal = 55;
                    else if (puntajeTotal >= 50 && puntajeTotal < 55)
                        puntajeNormal = 50;
                    else if (puntajeTotal >= 46 && puntajeTotal < 50)
                        puntajeNormal = 46;
                    else if (puntajeTotal >= 41 && puntajeTotal < 46)
                        puntajeNormal = 41;
                    else if (puntajeTotal >= 35 && puntajeTotal < 41)
                        puntajeNormal = 35;
                    else if (puntajeTotal >= 29 && puntajeTotal < 35)
                        puntajeNormal = 29;
                    else if (puntajeTotal < 29)
                        puntajeNormal = 19;
                    break;
                #endregion
                #region 17
                case "17":
                    if (puntajeTotal >= 56)
                        puntajeNormal = 56;
                    else if (puntajeTotal >= 52 && puntajeTotal < 56)
                        puntajeNormal = 52;
                    else if (puntajeTotal >= 49 && puntajeTotal < 52)
                        puntajeNormal = 49;
                    else if (puntajeTotal >= 45 && puntajeTotal < 49)
                        puntajeNormal = 45;
                    else if (puntajeTotal >= 39 && puntajeTotal < 45)
                        puntajeNormal = 39;
                    else if (puntajeTotal >= 35 && puntajeTotal < 39)
                        puntajeNormal = 35;
                    else if (puntajeTotal < 35)
                        puntajeNormal = 28;
                    break;
                #endregion
                #region 18
                case "18":
                    if (puntajeTotal >= 57)
                        puntajeNormal = 57;
                    else if (puntajeTotal >= 53 && puntajeTotal < 57)
                        puntajeNormal = 53;
                    else if (puntajeTotal >= 50 && puntajeTotal < 53)
                        puntajeNormal = 50;
                    else if (puntajeTotal >= 46 && puntajeTotal < 50)
                        puntajeNormal = 46;
                    else if (puntajeTotal >= 42 && puntajeTotal < 46)
                        puntajeNormal = 42;
                    else if (puntajeTotal >= 36 && puntajeTotal < 42)
                        puntajeNormal = 36;
                    else if (puntajeTotal < 36)
                        puntajeNormal = 29;
                    break;
                #endregion
                #region 19
                case "19":
                    if (puntajeTotal >= 57)
                        puntajeNormal = 57;
                    else if (puntajeTotal >= 54 && puntajeTotal < 57)
                        puntajeNormal = 54;
                    else if (puntajeTotal >= 51 && puntajeTotal < 54)
                        puntajeNormal = 51;
                    else if (puntajeTotal >= 47 && puntajeTotal < 51)
                        puntajeNormal = 47;
                    else if (puntajeTotal >= 42 && puntajeTotal < 47)
                        puntajeNormal = 42;
                    else if (puntajeTotal >= 37 && puntajeTotal < 42)
                        puntajeNormal = 37;
                    else if (puntajeTotal < 37)
                        puntajeNormal = 30;
                    break;
                #endregion
                #region 20-21
                case "20-21":
                    if (puntajeTotal >= 58)
                        puntajeNormal = 58;
                    else if (puntajeTotal >= 54 && puntajeTotal < 58)
                        puntajeNormal = 54;
                    else if (puntajeTotal >= 51 && puntajeTotal < 54)
                        puntajeNormal = 51;
                    else if (puntajeTotal >= 47 && puntajeTotal < 51)
                        puntajeNormal = 47;
                    else if (puntajeTotal >= 43 && puntajeTotal < 47)
                        puntajeNormal = 43;
                    else if (puntajeTotal >= 37 && puntajeTotal < 43)
                        puntajeNormal = 37;
                    else if (puntajeTotal < 37)
                        puntajeNormal = 30;
                    break;
                #endregion
                #region 22-65
                case "22-65":
                    if (puntajeTotal >= 59)
                        puntajeNormal = 59;
                    else if (puntajeTotal >= 55 && puntajeTotal < 59)
                        puntajeNormal = 55;
                    else if (puntajeTotal >= 52 && puntajeTotal < 55)
                        puntajeNormal = 52;
                    else if (puntajeTotal >= 48 && puntajeTotal < 52)
                        puntajeNormal = 48;
                    else if (puntajeTotal >= 44 && puntajeTotal < 48)
                        puntajeNormal = 44;
                    else if (puntajeTotal >= 38 && puntajeTotal < 44)
                        puntajeNormal = 38;
                    else if (puntajeTotal < 38)
                        puntajeNormal = 31;
                    break;
                #endregion
            }
            return puntajeNormal;
        }

        /// <summary>
        /// Método que devuelve el puntaje normalizado segun la Tabla de Discrepancia
        /// </summary>
        /// <param name="puntajeTotal"> PuntajeTotal porporcionado para la busqueda</param>
        /// <returns> Retorna el porcentajeTotal normalizado segun la Tabla de Discrepancia </returns>
        private int NormalizarPuntajePrevioDiscrepancia(int puntajeTotal)
        {
            int puntajeNormalizado = 0;
            if (puntajeTotal >= 55)
                puntajeNormalizado = 55;
            else if (puntajeTotal >= 50 && puntajeTotal < 55)
                puntajeNormalizado = 50;
            else if (puntajeTotal >= 45 && puntajeTotal < 50)
                puntajeNormalizado = 45;
            else if (puntajeTotal >= 40 && puntajeTotal < 45)
                puntajeNormalizado = 40;
            else if (puntajeTotal >= 35 && puntajeTotal < 40)
                puntajeNormalizado = 35;
            else if (puntajeTotal >= 30 && puntajeTotal < 35)
                puntajeNormalizado = 30;
            else if (puntajeTotal >= 25 && puntajeTotal < 30)
                puntajeNormalizado = 25;
            else if (puntajeTotal >= 20 && puntajeTotal < 25)
                puntajeNormalizado = 20;
            else if (puntajeTotal >= 15 && puntajeTotal < 20)
                puntajeNormalizado = 15;
            else if (puntajeTotal < 15)
                puntajeNormalizado = 10;

            return puntajeNormalizado;
        }





        public Dictionary<string, string> RetriveResultadoPruebaEstilos(IDataContext dctx, int? resultadoPruebaID, int? pruebaID, Alumno alumno)
        { 
            APrueba prueba = new PruebaDinamica() { PruebaID = pruebaID };

            AResultadoPrueba resultado = new ResultadoPruebaDinamica();
            resultado.ResultadoPruebaID = resultadoPruebaID;
            resultado.Prueba = prueba;

            resultado = new PruebaDiagnosticoCtrl().RetrieveCompleteResultadoPrueba(dctx, resultado);

            if (resultado == null)
                throw new Exception("No se encontró el resultado de la prueba");
            if (!(resultado is ResultadoPruebaDinamica))
                throw new Exception("El resultado no corresponde a una prueba dinámica");

            ResultadoPruebaDinamica resultadoDinamica = resultado as ResultadoPruebaDinamica;
          
           
            #region Variables
            int totalPreguntas = 0;
            int totalPreguntasCorrectas = 0;
            int porgrupo = 0;
            int cont = 0;
            #endregion

            AModelo LastObject;
            LastObject = new ModeloDinamico { ModeloID = resultado.Prueba.Modelo.ModeloID };

            List<Clasificador> listaClasificadoresEstilos = modeloPruebaCtrl.RetrieveClasificadoresModeloDinamico(dctx, LastObject as ModeloDinamico);
            OpcionRespuestaModeloGenerico clasificadorRespuesta = new OpcionRespuestaModeloGenerico();
            #region Cálculo del total de preguntas y el total de preguntas correctas
            DataSet respuestasEstilos = RetrieveRespuestasEstilosdeAprendizaje(dctx, alumno);
            #endregion
            #region LLenado Dataset
            string strFilter = string.Empty;
            #endregion
            #region Cálculo de puntuación por Clasificador
            Dictionary<string, string> resultadoEstilosdeAprendizaje = new Dictionary<string, string>();
            foreach (var item in listaClasificadoresEstilos)
            {
                strFilter = "NombreClasificador LIKE('" + item.Nombre + "')";
                if (!String.IsNullOrEmpty(strFilter))
                {
                    DataSet dsTmp = new DataSet();
                    DataView dataView = new DataView(respuestasEstilos.Tables[0]);
                    dataView.RowFilter = strFilter;
                    strFilter = string.Empty;
                    dsTmp.Tables.Clear();
                    dsTmp.Tables.Add(dataView.ToTable());

                    if (dsTmp.Tables[0].Rows.Count > 0)
                    {
                        for (int c = 0; c < dsTmp.Tables[0].Rows.Count; c++)
                        {
                            porgrupo += Convert.ToInt32(dsTmp.Tables[0].Rows[c].ItemArray[9]);
                        }
                    }
                }

                if (porgrupo > 0)
                {
                    resultadoEstilosdeAprendizaje.Add(item.Nombre, porgrupo.ToString());
                    porgrupo = 0;
                }
            }


            #endregion
            return resultadoEstilosdeAprendizaje;

        }


        /// <summary>
        /// Calcula el resultado de cada seccion de la prueba Zavic
        /// </summary>
        /// <param name="dctx">Provee el acceso a la base de datos</param>
        /// <param name="resultadoPruebaID">ID del resultado de la prueba</param>
        /// <param name="resultadoPruebaID">ID de la prueba</param>
        /// <returns>Puntajes obtenidos en cada percetil</returns>
        public Dictionary<string, string> RetrieveResultadoPruebaZavic(IDataContext dctx, int? resultadoPruebaID, int? pruebaID, Alumno alumno)
        {
            #region Se obtiene la Respuesta Prueba completa
            APrueba prueba = new PruebaDinamica() { PruebaID = pruebaID };

            AResultadoPrueba resultado = new ResultadoPruebaDinamica();
            resultado.ResultadoPruebaID = resultadoPruebaID;
            resultado.Prueba = prueba;

            resultado = new PruebaDiagnosticoCtrl().RetrieveCompleteResultadoPrueba(dctx, resultado);

            if (resultado == null)
                throw new Exception("No se encontró el resultado de la prueba");
            if (!(resultado is ResultadoPruebaDinamica))
                throw new Exception("El resultado no corresponde a una prueba dinámica");

            ResultadoPruebaDinamica resultadoDinamica = resultado as ResultadoPruebaDinamica;
            #endregion

            #region Variables
            int totalPreguntas = 0;
            int totalPreguntasCorrectas = 0;
            int porgrupo = 0;
            int cont = 0;
            #endregion
            List<OpcionRespuestaPlantilla> respuestasCorrectas = new List<OpcionRespuestaPlantilla>();

            PruebaDinamica pruebaDinamica = new PruebaDinamica { PruebaID = pruebaID };

            AModelo LastObject;
            LastObject = new ModeloDinamico { ModeloID = resultado.Prueba.Modelo.ModeloID };

            List<Clasificador> listaClasificadoresZavic = modeloPruebaCtrl.RetrieveClasificadoresModeloDinamico(dctx, LastObject as ModeloDinamico);

            #region Cálculo del total de preguntas y el total de preguntas correctas
            DataSet respuestasZavic = RetrieveRespuestasPruebaZavic(dctx, alumno);
            #endregion

            #region Llenado DataSet
            string strFilter = string.Empty;
            #endregion

            #region Cálculo de puntuacion por Clasificador
            Dictionary<string, string> resultadoZavic = new Dictionary<string, string>();
            foreach (var item in listaClasificadoresZavic)
            {
                strFilter = "NombreClasificador LIKE('" + item.Nombre + "')";
                if (!String.IsNullOrEmpty(strFilter))
                {
                    DataSet dsTmp = new DataSet();
                    DataView dataView = new DataView(respuestasZavic.Tables[0]);
                    dataView.RowFilter = strFilter;
                    strFilter = string.Empty;
                    dsTmp.Tables.Clear();
                    dsTmp.Tables.Add(dataView.ToTable());

                    if (dsTmp.Tables[0].Rows.Count > 0)
                    {
                        for (int c = 0; c < dsTmp.Tables[0].Rows.Count; c++)
                        {
                            porgrupo += Convert.ToInt32(dsTmp.Tables[0].Rows[c].ItemArray[9]);
                        }
                    }
                }

                if (porgrupo > 0)
                {
                    resultadoZavic.Add(item.Nombre, porgrupo.ToString());
                    porgrupo = 0;
                }
            }
            #endregion

            return resultadoZavic;
        }
        public Dictionary<string, string>RetrieveResultadoPruebaZavic(IDataContext dctx, ResultadoPruebaDinamica resultado, Alumno alumno)
        {  
            #region provisiona

            #region Se obtiene la Respuesta Prueba completa
            if (resultado == null)
                throw new Exception("No se encontró el resultado de la prueba");
            if (!(resultado is ResultadoPruebaDinamica))
                throw new Exception("El resultado no corresponde a una prueba dinámica");
            #endregion

            #region Variables
            int totalPreguntas = 0;
            int totalPreguntasCorrectas = 0;
            int porgrupo = 0;
            int cont = 0;
            #endregion

            AModelo LastObject;
            LastObject = new ModeloDinamico { ModeloID = resultado.Prueba.Modelo.ModeloID };

            List<Clasificador> listaClasificadoresZavic = modeloPruebaCtrl.RetrieveClasificadoresModeloDinamico(dctx, LastObject as ModeloDinamico);

            OpcionRespuestaModeloGenerico clasificadorRespuesta = new OpcionRespuestaModeloGenerico();

            #region Cálculo del total de preguntas y el total de preguntas correctas
            DataSet respuestasZavic = RetrieveRespuestasPruebaZavic(dctx, alumno);
            #endregion

            #region Llenado DataSet
            string strFilter = string.Empty;




            #endregion
#endregion

            #region Cálculo de puntuacion por Clasificador
            Dictionary<string, string> resultadoZavic = new Dictionary<string, string>();
            foreach (var item in listaClasificadoresZavic)
            {
                strFilter = "NombreClasificador LIKE('" + item.Nombre + "')";
                if (!String.IsNullOrEmpty(strFilter))
                {
                    DataSet dsTmp = new DataSet();
                    DataView dataView = new DataView(respuestasZavic.Tables[0]);
                    dataView.RowFilter = strFilter;
                    strFilter = string.Empty;
                    dsTmp.Tables.Clear();
                    dsTmp.Tables.Add(dataView.ToTable());

                    if (dsTmp.Tables[0].Rows.Count > 0)
                    {
                        for (int c = 0; c < dsTmp.Tables[0].Rows.Count; c++)
                        {
                            porgrupo += Convert.ToInt32(dsTmp.Tables[0].Rows[c].ItemArray[9]);
                        }
                    }
                }

                if (porgrupo > 0)
                {
                    resultadoZavic.Add(item.Nombre, porgrupo.ToString());
                    porgrupo = 0;
                }
            }
            #endregion

            return resultadoZavic;
        }

        public Dictionary<string, string>RetrieveResultadoPruebaEstilos(IDataContext dctx, ResultadoPruebaDinamica resultado, Alumno alumno)
        {
            #region Se obtiene la Respuesta Prueba Competa
            if (resultado == null)
                throw new Exception("No se encontró el resultado de la prueba");
            if(!(resultado is ResultadoPruebaDinamica))
                throw new Exception("El resultado no corresponde a una prueba dinámica");
#endregion
            #region Variables
            int totalPreguntas = 0;
            int totalPreguntasCorrectas = 0;
            int porgrupo = 0;
            int cont = 0;
#endregion

            AModelo LastObject;
            LastObject = new ModeloDinamico { ModeloID = resultado.Prueba.Modelo.ModeloID };

            List<Clasificador> listaClasificadoresEstilos = modeloPruebaCtrl.RetrieveClasificadoresModeloDinamico(dctx, LastObject as ModeloDinamico);
            OpcionRespuestaModeloGenerico clasificadorRespuesta = new OpcionRespuestaModeloGenerico();
            #region Cálculo del total de preguntas y el total de preguntas correctas
            DataSet respuestasEstilos = RetrieveRespuestasEstilosdeAprendizaje(dctx, alumno);
            #endregion
            #region LLenado Dataset
            string strFilter = string.Empty;
            #endregion
            #region Cálculo de puntuación por Clasificador
            Dictionary<string, string> resultadoEstilosdeAprendizaje = new Dictionary<string, string>();
            foreach (var item in listaClasificadoresEstilos)
            {
                strFilter="NombreClasificador LIKE('"+item.Nombre+"')";
                if (!String.IsNullOrEmpty(strFilter))
                {
                     DataSet dsTmp = new DataSet();
                    DataView dataView = new DataView(respuestasEstilos.Tables[0]);
                    dataView.RowFilter = strFilter;
                    strFilter = string.Empty;
                    dsTmp.Tables.Clear();
                    dsTmp.Tables.Add(dataView.ToTable());

                     if (dsTmp.Tables[0].Rows.Count > 0)
                    {
                        for (int c = 0; c < dsTmp.Tables[0].Rows.Count; c++)
                        {
                            porgrupo ++;
                        }
                    }
                }

                if (porgrupo > 0)
                {
                    resultadoEstilosdeAprendizaje.Add(item.Nombre, porgrupo.ToString());
                    porgrupo = 0;
                }
                }
            

#endregion 
            return resultadoEstilosdeAprendizaje;

        }

        public Dictionary<string, string> RetrieveResultadoPruebaInteligenciasMultiples(IDataContext dctx, ResultadoPruebaDinamica resultado, Alumno alumno) {

            #region Se obtiene la Respuesta Prueba Competa
            if (resultado == null)
                throw new Exception("No se encontró el resultado de la prueba");
            if (!(resultado is ResultadoPruebaDinamica))
                throw new Exception("El resultado no corresponde a una prueba dinámica");
            #endregion
            #region Variables
            int totalPreguntas = 0;
            int totalPreguntasCorrectas = 0;
            int porgrupo = 0;
            int cont = 0;
            #endregion

            AModelo LastObject;
            LastObject = new ModeloDinamico { ModeloID = resultado.Prueba.Modelo.ModeloID };

            List<Clasificador> listaClasificadoresEstilos = modeloPruebaCtrl.RetrieveClasificadoresModeloDinamico(dctx, LastObject as ModeloDinamico);
            OpcionRespuestaModeloGenerico clasificadorRespuesta = new OpcionRespuestaModeloGenerico();
            #region Cálculo del total de preguntas y el total de preguntas correctas
            DataSet respuestasInteligencias = RetrieveRespuestasInteligenciasMultiples(dctx, alumno);
            #endregion
            #region LLenado Dataset
            string strFilter = string.Empty;
            #endregion
            #region Cálculo de puntuación por Clasificador
            Dictionary<string, string> resultadoInteligenciasMutliples = new Dictionary<string, string>();
            foreach (var item in listaClasificadoresEstilos)
            {
                strFilter = "NombreClasificador LIKE('" + item.Nombre + "')";
                if (!String.IsNullOrEmpty(strFilter))
                {
                    DataSet dsTmp = new DataSet();
                    DataView dataView = new DataView(respuestasInteligencias.Tables[0]);
                    dataView.RowFilter = strFilter;
                    strFilter = string.Empty;
                    dsTmp.Tables.Clear();
                    dsTmp.Tables.Add(dataView.ToTable());

                    if (dsTmp.Tables[0].Rows.Count > 0)
                    {
                        for (int c = 0; c < dsTmp.Tables[0].Rows.Count; c++)
                        {
                            porgrupo += Convert.ToInt32(dsTmp.Tables[0].Rows[c].ItemArray[9]);
                        }
                    }
                }

                resultadoInteligenciasMutliples.Add(item.Nombre, porgrupo.ToString());
                porgrupo = 0;
               
            }


            #endregion
            return resultadoInteligenciasMutliples;

        }

        public Dictionary<string, string> RetrieveResultadoPruebaInventariodeIntereses(IDataContext dctx, ResultadoPruebaDinamica resultado, Alumno alumno)
        {
                    #region Se obtiene la Respuesta Prueba Competa
            if (resultado == null)
                throw new Exception("No se encontró el resultado de la prueba");
            if (!(resultado is ResultadoPruebaDinamica))
                throw new Exception("El resultado no corresponde a una prueba dinámica");
            #endregion
            #region Variables
            int totalPreguntas = 0;
            int totalPreguntasCorrectas = 0;
            int porgrupo = 0;
            int cont = 0;
            #endregion

            AModelo LastObject;
            LastObject = new ModeloDinamico { ModeloID = resultado.Prueba.Modelo.ModeloID };

            List<Clasificador> listaClasificadoresEstilos = modeloPruebaCtrl.RetrieveClasificadoresModeloDinamico(dctx, LastObject as ModeloDinamico);
            OpcionRespuestaModeloGenerico clasificadorRespuesta = new OpcionRespuestaModeloGenerico();
            #region Cálculo del total de preguntas y el total de preguntas correctas
            DataSet respuestasInventario = RetrieveRepuestaInventariodeIntereses(dctx, alumno);
            #endregion
            #region LLenado Dataset
            string strFilter = string.Empty;
            #endregion
            #region Cálculo de puntuación por Clasificador
            Dictionary<string, string> resultadoInventariodeIntereses = new Dictionary<string, string>();
            foreach (var item in listaClasificadoresEstilos)
            {
                strFilter = "NombreClasificador LIKE('" + item.Nombre + "')";
                if (!String.IsNullOrEmpty(strFilter))
                {
                    DataSet dsTmp = new DataSet();
                    DataView dataView = new DataView(respuestasInventario.Tables[0]);
                    dataView.RowFilter = strFilter;
                    strFilter = string.Empty;
                    dsTmp.Tables.Clear();
                    dsTmp.Tables.Add(dataView.ToTable());

                    if (dsTmp.Tables[0].Rows.Count > 0)
                    {
                        for (int c = 0; c < dsTmp.Tables[0].Rows.Count; c++)
                        {
                            porgrupo += Convert.ToInt32(dsTmp.Tables[0].Rows[c].ItemArray[9]);
                        }
                    }
                }

                if (porgrupo > 0)
                {
                    resultadoInventariodeIntereses.Add(item.Nombre, porgrupo.ToString());
                    porgrupo = 0;
                }
            }


            #endregion
            return resultadoInventariodeIntereses;

        
        }

        public Dictionary<string,string> RetrieveResultadoPruebaInventarioHerrrera(IDataContext dctx, ResultadoPruebaDinamica resultado, Alumno alumno){
            #region Se obtiene la Respuesta Prueba Competa
            if (resultado == null)
                throw new Exception("No se encontró el resultado de la prueba");
            if (!(resultado is ResultadoPruebaDinamica))
                throw new Exception("El resultado no corresponde a una prueba dinámica");
            #endregion
            #region Variables
            int totalPreguntas = 0;
            int totalPreguntasCorrectas = 0;
            int porgrupo = 0;
            int cont = 0;
            #endregion

            AModelo LastObject;
            LastObject = new ModeloDinamico { ModeloID = resultado.Prueba.Modelo.ModeloID };

            List<Clasificador> listaClasificadoresEstilos = modeloPruebaCtrl.RetrieveClasificadoresModeloDinamico(dctx, LastObject as ModeloDinamico);
            OpcionRespuestaModeloGenerico clasificadorRespuesta = new OpcionRespuestaModeloGenerico();
            #region Cálculo del total de preguntas y el total de preguntas correctas
            DataSet respuestasInventarioHerrera = RetrieveRespuestasInventarioHerrera(dctx, alumno);
            #endregion
            #region LLenado Dataset
            string strFilter = string.Empty;
            #endregion
            #region Cálculo de puntuación por Clasificador
            Dictionary<string, string> resultadoInventarioHerrera = new Dictionary<string, string>();
            foreach (var item in listaClasificadoresEstilos)
            {
                strFilter = "NombreClasificador LIKE('" + item.Nombre + "')";
                if (!String.IsNullOrEmpty(strFilter))
                {
                    DataSet dsTmp = new DataSet();
                    DataView dataView = new DataView(respuestasInventarioHerrera.Tables[0]);
                    dataView.RowFilter = strFilter;
                    strFilter = string.Empty;
                    dsTmp.Tables.Clear();
                    dsTmp.Tables.Add(dataView.ToTable());

                    if (dsTmp.Tables[0].Rows.Count > 0)
                    {
                        for (int c = 0; c < dsTmp.Tables[0].Rows.Count; c++)
                        {
                            porgrupo += Convert.ToInt32(dsTmp.Tables[0].Rows[c].ItemArray[9]);
                        }
                    }
                }

                if (porgrupo > 0)
                {
                    resultadoInventarioHerrera.Add(item.Nombre, porgrupo.ToString());
                    porgrupo = 0;
                }
            }
            #endregion 
            return resultadoInventarioHerrera;

        }

        public DataSet RetrieveRespuestasEstilosdeAprendizaje(IDataContext dctx, Alumno alumno)
        {
            ViewResultadoPruebaEstilosdeAprendizajeRetHlp da = new ViewResultadoPruebaEstilosdeAprendizajeRetHlp();
            DataSet ds=da.Action(dctx,alumno);
            return ds;
        }

        public DataSet RetrieveRespuestasInventarioHerrera(IDataContext dctx, Alumno alumno)
        {
            ViewResultadoPruebaInventarioHerreraRetHlp da = new ViewResultadoPruebaInventarioHerreraRetHlp();
            DataSet ds = da.Action(dctx, alumno);
            return ds;
        }
        
        public DataSet RetrieveRespuestasInteligenciasMultiples(IDataContext dctx, Alumno alumno)
        {
            ViewResultadoPruebaInteligenciasMultiplesRetHlp da = new ViewResultadoPruebaInteligenciasMultiplesRetHlp();
            DataSet ds = da.Action(dctx, alumno);
            return ds;
        }
        
        public DataSet RetrieveRepuestaInventariodeIntereses(IDataContext dctx, Alumno alumno)
        {
            ViewResultadoPruebaInventariodeIntereseRetHlp da = new ViewResultadoPruebaInventariodeIntereseRetHlp();
            DataSet ds = da.Action(dctx, alumno);
            return ds;

        }
        
        public DataSet RetrieveRespuestasPruebaZavic(IDataContext dctx, Alumno alumno)
        {
            ViewResultadoPruebaZavicRetHlp da = new ViewResultadoPruebaZavicRetHlp();
            DataSet ds = da.Action(dctx, alumno);
            return ds;
        }

        /// <summary>
        /// Calcula el resultado de cada seccion de la prueba BullyingAutoconcepto
        /// </summary>
        /// <param name="dctx">Provee el acceso a la base de datos</param>
        /// <param name="resultadoPruebaID">ID del resultado de la prueba</param>
        /// <param name="resultadoPruebaID">ID de la prueba</param>
        /// <returns>Puntajes obtenidos en cada percetil</returns>
        public Dictionary<string, int> RetrieveResultadoPruebaBullyingAutoconcepto(IDataContext dctx, ResultadoPruebaDinamica resultado, Alumno alumno)
        {
            #region Variables
            int porfactor = 0;
            AModelo LastObject;
            // DataSet que almacenará la respuesta del alumno
            DataSet dsRespuestasAlumno = new DataSet();
            string filter = string.Empty;
            Dictionary<string, int> resultadoBullyingAutoconcepto = new Dictionary<string, int>();
            #endregion

            #region Se obtiene la Respuesta Prueba completa
            if (resultado == null)
                throw new Exception("No se encontró el resultado de la prueba");
            if (!(resultado is ResultadoPruebaDinamica))
                throw new Exception("El resultado no corresponde a una prueba dinámica");

            BateriaBullyingCtrl bateriaBullyingCtrl = new BateriaBullyingCtrl();
            dsRespuestasAlumno = bateriaBullyingCtrl.RetrieveResultadoBullyingAutoconcepto(dctx, alumno);
            #endregion

            LastObject = new ModeloDinamico { ModeloID = resultado.Prueba.Modelo.ModeloID };
            List<Clasificador> clasificadores = modeloPruebaCtrl.RetrieveClasificadoresModeloDinamico(dctx, LastObject as ModeloDinamico);

            #region Calculo factores
            foreach (var item in clasificadores)
            {
                filter = "Clasificador LIKE('" + item.Nombre + "')";
                if (!String.IsNullOrEmpty(filter))
                {
                    DataSet tmp = new DataSet();
                    DataView dataVierw = new DataView(dsRespuestasAlumno.Tables[0]);
                    dataVierw.RowFilter = filter;
                    filter = string.Empty;
                    tmp.Tables.Clear();
                    tmp.Tables.Add(dataVierw.ToTable());

                    if (tmp.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < tmp.Tables[0].Rows.Count; i++)
                        {
                            porfactor += Convert.ToInt32(tmp.Tables[0].Rows[i].ItemArray[5]);
                        }
                    }
                }

                if (porfactor >= 0)
                {
                    resultadoBullyingAutoconcepto.Add(item.Nombre, porfactor);
                    porfactor = 0;
                }
            }
            #endregion

            return resultadoBullyingAutoconcepto;
        }

        /// <summary>
        /// Calcula el resultado de cada seccion de la prueba BullyingActitudes
        /// </summary>
        /// <param name="dctx">Provee el acceso a la base de datos</param>
        /// <param name="resultadoPruebaID">ID del resultado de la prueba</param>
        /// <param name="resultadoPruebaID">ID de la prueba</param>
        /// <returns>Puntajes obtenidos en cada percetil</returns>
        public Dictionary<string, int> RetrieveResultadoPruebaBullyingActitudes(IDataContext dctx, ResultadoPruebaDinamica resultado, Alumno alumno)
        {
            #region Variables
            int porfactor = 0;
            AModelo LastObject;
            // DataSet que almacenará la respuesta del alumno
            DataSet dsRespuestasAlumno = new DataSet();
            string filter = string.Empty;
            Dictionary<string, int> resultadoBullyingActitudes = new Dictionary<string, int>();
            #endregion

            #region Se obtiene la Respuesta Prueba completa
            if (resultado == null)
                throw new Exception("No se encontró el resultado de la prueba");
            if (!(resultado is ResultadoPruebaDinamica))
                throw new Exception("El resultado no corresponde a una prueba dinámica");

            BateriaBullyingCtrl bateriaBullyingCtrl = new BateriaBullyingCtrl();
            dsRespuestasAlumno = bateriaBullyingCtrl.RetrieveResultadoBullyingActitudes(dctx, alumno);
            #endregion

            LastObject = new ModeloDinamico { ModeloID = resultado.Prueba.Modelo.ModeloID };
            List<Clasificador> clasificadores = modeloPruebaCtrl.RetrieveClasificadoresModeloDinamico(dctx, LastObject as ModeloDinamico);

            #region Calculo factores
            foreach (var item in clasificadores)
            {
                filter = "Clasificador LIKE('" + item.Nombre + "')";
                if (!String.IsNullOrEmpty(filter))
                {
                    DataSet tmp = new DataSet();
                    DataView dataVierw = new DataView(dsRespuestasAlumno.Tables[0]);
                    dataVierw.RowFilter = filter;
                    filter = string.Empty;
                    tmp.Tables.Clear();
                    tmp.Tables.Add(dataVierw.ToTable());

                    if (tmp.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < tmp.Tables[0].Rows.Count; i++)
                        {
                            porfactor += Convert.ToInt32(tmp.Tables[0].Rows[i].ItemArray[5]);
                        }
                    }
                }

                if (porfactor >= 0)
                {
                    resultadoBullyingActitudes.Add(item.Nombre, porfactor);
                    porfactor = 0;
                }
            }
            #endregion

            return resultadoBullyingActitudes;
        }

        /// <summary>
        /// Calcula el resultado de cada seccion de la prueba BullyingEmpatia
        /// </summary>
        /// <param name="dctx">Provee el acceso a la base de datos</param>
        /// <param name="resultadoPruebaID">ID del resultado de la prueba</param>
        /// <param name="resultadoPruebaID">ID de la prueba</param>
        /// <returns>Puntajes obtenidos en cada percetil</returns>
        public Dictionary<string, int> RetrieveResultadoPruebaBullyingEmpatia(IDataContext dctx, ResultadoPruebaDinamica resultado, Alumno alumno)
        {
            #region Variables
            int porfactor = 0;
            AModelo LastObject;
            // DataSet que almacenará la respuesta del alumno
            DataSet dsRespuestasAlumno = new DataSet();
            string filter = string.Empty;
            Dictionary<string, int> resultadoBullyingEmpatia = new Dictionary<string, int>();
            #endregion

            #region Se obtiene la Respuesta Prueba completa
            if (resultado == null)
                throw new Exception("No se encontró el resultado de la prueba");
            if (!(resultado is ResultadoPruebaDinamica))
                throw new Exception("El resultado no corresponde a una prueba dinámica");

            BateriaBullyingCtrl bateriaBullyingCtrl = new BateriaBullyingCtrl();
            dsRespuestasAlumno = bateriaBullyingCtrl.RetrieveResultadoBullyingEmpatia(dctx, alumno);
            #endregion

            LastObject = new ModeloDinamico { ModeloID = resultado.Prueba.Modelo.ModeloID };
            List<Clasificador> clasificadores = modeloPruebaCtrl.RetrieveClasificadoresModeloDinamico(dctx, LastObject as ModeloDinamico);

            #region Calculo factores
            foreach (var item in clasificadores)
            {
                filter = "Clasificador LIKE('" + item.Nombre + "')";
                if (!String.IsNullOrEmpty(filter))
                {
                    DataSet tmp = new DataSet();
                    DataView dataVierw = new DataView(dsRespuestasAlumno.Tables[0]);
                    dataVierw.RowFilter = filter;
                    filter = string.Empty;
                    tmp.Tables.Clear();
                    tmp.Tables.Add(dataVierw.ToTable());

                    if (tmp.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < tmp.Tables[0].Rows.Count; i++)
                        {
                            porfactor += Convert.ToInt32(tmp.Tables[0].Rows[i].ItemArray[5]);
                        }
                    }
                }

                if (porfactor >= 0)
                {
                    resultadoBullyingEmpatia.Add(item.Nombre, porfactor);
                    porfactor = 0;
                }
            }
            #endregion

            return resultadoBullyingEmpatia;
        }
        /// <summary>
        /// Calcula el resultado de cada seccion de la prueba BullyingHumor
        /// </summary>
        /// <param name="dctx">Provee el acceso a la base de datos</param>
        /// <param name="resultadoPruebaID">ID del resultado de la prueba</param>
        /// <param name="resultadoPruebaID">ID de la prueba</param>
        /// <returns>Puntajes obtenidos en cada percetil</returns>
        public Dictionary<string, int> RetrieveResultadoPruebaBullyingHumor(IDataContext dctx, ResultadoPruebaDinamica resultado, Alumno alumno)
        {
            #region Variables
            int porfactor = 0;
            AModelo LastObject;
            // DataSet que almacenará la respuesta del alumno
            DataSet dsRespuestasAlumno = new DataSet();
            string filter = string.Empty;
            Dictionary<string, int> resultadoBullyingHumor = new Dictionary<string, int>();
            #endregion

            #region Se obtiene la Respuesta Prueba completa
            if (resultado == null)
                throw new Exception("No se encontró el resultado de la prueba");
            if (!(resultado is ResultadoPruebaDinamica))
                throw new Exception("El resultado no corresponde a una prueba dinámica");

            BateriaBullyingCtrl bateriaBullyingCtrl = new BateriaBullyingCtrl();
            dsRespuestasAlumno = bateriaBullyingCtrl.RetrieveResultadoBullyingHumor(dctx, alumno);
            #endregion

            LastObject = new ModeloDinamico { ModeloID = resultado.Prueba.Modelo.ModeloID };
            List<Clasificador> clasificadores = modeloPruebaCtrl.RetrieveClasificadoresModeloDinamico(dctx, LastObject as ModeloDinamico);

            #region Calculo factores
            foreach (var item in clasificadores)
            {
                filter = "Clasificador LIKE('" + item.Nombre + "')";
                if (!String.IsNullOrEmpty(filter))
                {
                    DataSet tmp = new DataSet();
                    DataView dataVierw = new DataView(dsRespuestasAlumno.Tables[0]);
                    dataVierw.RowFilter = filter;
                    filter = string.Empty;
                    tmp.Tables.Clear();
                    tmp.Tables.Add(dataVierw.ToTable());

                    if (tmp.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < tmp.Tables[0].Rows.Count; i++)
                        {
                            porfactor += Convert.ToInt32(tmp.Tables[0].Rows[i].ItemArray[5]);
                        }
                    }
                }

                if (porfactor >= 0)
                {
                    resultadoBullyingHumor.Add(item.Nombre, porfactor);
                    porfactor = 0;
                }
            }
            #endregion

            return resultadoBullyingHumor;
        }
        /// <summary>
        /// Calcula el resultado de cada seccion de la prueba BullyingVictimizacion
        /// </summary>
        /// <param name="dctx">Provee el acceso a la base de datos</param>
        /// <param name="resultadoPruebaID">ID del resultado de la prueba</param>
        /// <param name="resultadoPruebaID">ID de la prueba</param>
        /// <returns>Puntajes obtenidos en cada percetil</returns>
        public Dictionary<string, int> RetrieveResultadoPruebaBullyingVictimizacion(IDataContext dctx, ResultadoPruebaDinamica resultado, Alumno alumno)
        {
            #region Variables
            int porfactor = 0;
            AModelo LastObject;
            // DataSet que almacenará la respuesta del alumno
            DataSet dsRespuestasAlumno = new DataSet();
            string filter = string.Empty;
            Dictionary<string, int> resultadoBullyingVictimizacion = new Dictionary<string, int>();
            #endregion

            #region Se obtiene la Respuesta Prueba completa
            if (resultado == null)
                throw new Exception("No se encontró el resultado de la prueba");
            if (!(resultado is ResultadoPruebaDinamica))
                throw new Exception("El resultado no corresponde a una prueba dinámica");

            BateriaBullyingCtrl bateriaBullyingCtrl = new BateriaBullyingCtrl();
            dsRespuestasAlumno = bateriaBullyingCtrl.RetrieveResultadoBullyingVictimizacion(dctx, alumno);
            #endregion

            LastObject = new ModeloDinamico { ModeloID = resultado.Prueba.Modelo.ModeloID };
            List<Clasificador> clasificadores = modeloPruebaCtrl.RetrieveClasificadoresModeloDinamico(dctx, LastObject as ModeloDinamico);

            #region Calculo factores
            foreach (var item in clasificadores)
            {
                filter = "Clasificador LIKE('" + item.Nombre + "')";
                if (!String.IsNullOrEmpty(filter))
                {
                    DataSet tmp = new DataSet();
                    DataView dataVierw = new DataView(dsRespuestasAlumno.Tables[0]);
                    dataVierw.RowFilter = filter;
                    filter = string.Empty;
                    tmp.Tables.Clear();
                    tmp.Tables.Add(dataVierw.ToTable());

                    if (tmp.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < tmp.Tables[0].Rows.Count; i++)
                        {
                            porfactor += Convert.ToInt32(tmp.Tables[0].Rows[i].ItemArray[5]);
                        }
                    }
                }

                if (porfactor >= 0)
                {
                    resultadoBullyingVictimizacion.Add(item.Nombre, porfactor);
                    porfactor = 0;
                }
            }
            #endregion

            return resultadoBullyingVictimizacion;
        }
        /// <summary>
        /// Calcula el resultado de cada seccion de la prueba BullyingCiberbullying
        /// </summary>
        /// <param name="dctx">Provee el acceso a la base de datos</param>
        /// <param name="resultadoPruebaID">ID del resultado de la prueba</param>
        /// <param name="resultadoPruebaID">ID de la prueba</param>
        /// <returns>Puntajes obtenidos en cada percetil</returns>
        public Dictionary<string, int> RetrieveResultadoPruebaBullyingCiberbullying(IDataContext dctx, ResultadoPruebaDinamica resultado, Alumno alumno)
        {
            #region Variables
            int porfactor = 0;
            AModelo LastObject;
            // DataSet que almacenará la respuesta del alumno
            DataSet dsRespuestasAlumno = new DataSet();
            string filter = string.Empty;
            Dictionary<string, int> resultadoBullyingCiberbullying = new Dictionary<string, int>();
            #endregion

            #region Se obtiene la Respuesta Prueba completa
            if (resultado == null)
                throw new Exception("No se encontró el resultado de la prueba");
            if (!(resultado is ResultadoPruebaDinamica))
                throw new Exception("El resultado no corresponde a una prueba dinámica");

            BateriaBullyingCtrl bateriaBullyingCtrl = new BateriaBullyingCtrl();
            dsRespuestasAlumno = bateriaBullyingCtrl.RetrieveResultadoBullyingCiberbullying(dctx, alumno);
            #endregion

            LastObject = new ModeloDinamico { ModeloID = resultado.Prueba.Modelo.ModeloID };
            List<Clasificador> clasificadores = modeloPruebaCtrl.RetrieveClasificadoresModeloDinamico(dctx, LastObject as ModeloDinamico);

            #region Calculo factores
            foreach (var item in clasificadores)
            {
                filter = "Clasificador LIKE('" + item.Nombre + "')";
                if (!String.IsNullOrEmpty(filter))
                {
                    DataSet tmp = new DataSet();
                    DataView dataVierw = new DataView(dsRespuestasAlumno.Tables[0]);
                    dataVierw.RowFilter = filter;
                    filter = string.Empty;
                    tmp.Tables.Clear();
                    tmp.Tables.Add(dataVierw.ToTable());

                    if (tmp.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < tmp.Tables[0].Rows.Count; i++)
                        {
                            porfactor += Convert.ToInt32(tmp.Tables[0].Rows[i].ItemArray[5]);
                        }
                    }
                }

                if (porfactor >= 0)
                {
                    resultadoBullyingCiberbullying.Add(item.Nombre, porfactor);
                    porfactor = 0;
                }
            }
            #endregion

            return resultadoBullyingCiberbullying;
        }
        /// <summary>
        /// Calcula el resultado de cada seccion de la prueba BullyingBullying
        /// </summary>
        /// <param name="dctx">Provee el acceso a la base de datos</param>
        /// <param name="resultadoPruebaID">ID del resultado de la prueba</param>
        /// <param name="resultadoPruebaID">ID de la prueba</param>
        /// <returns>Puntajes obtenidos en cada percetil</returns>
        public Dictionary<string, int> RetrieveResultadoPruebaBullyingBullying(IDataContext dctx, ResultadoPruebaDinamica resultado, Alumno alumno)
        {
            #region Variables
            int porfactor = 0;
            AModelo LastObject;
            // DataSet que almacenará la respuesta del alumno
            DataSet dsRespuestasAlumno = new DataSet();
            string filter = string.Empty;
            Dictionary<string, int> resultadoBullyingBullying = new Dictionary<string, int>();
            #endregion

            #region Se obtiene la Respuesta Prueba completa
            if (resultado == null)
                throw new Exception("No se encontró el resultado de la prueba");
            if (!(resultado is ResultadoPruebaDinamica))
                throw new Exception("El resultado no corresponde a una prueba dinámica");

            BateriaBullyingCtrl bateriaBullyingCtrl = new BateriaBullyingCtrl();
            dsRespuestasAlumno = bateriaBullyingCtrl.RetrieveResultadoBullyingBullying(dctx, alumno);
            #endregion

            LastObject = new ModeloDinamico { ModeloID = resultado.Prueba.Modelo.ModeloID };
            List<Clasificador> clasificadores = modeloPruebaCtrl.RetrieveClasificadoresModeloDinamico(dctx, LastObject as ModeloDinamico);

            #region Calculo factores
            foreach (var item in clasificadores)
            {
                filter = "Clasificador LIKE('" + item.Nombre + "')";
                if (!String.IsNullOrEmpty(filter))
                {
                    DataSet tmp = new DataSet();
                    DataView dataVierw = new DataView(dsRespuestasAlumno.Tables[0]);
                    dataVierw.RowFilter = filter;
                    filter = string.Empty;
                    tmp.Tables.Clear();
                    tmp.Tables.Add(dataVierw.ToTable());

                    if (tmp.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < tmp.Tables[0].Rows.Count; i++)
                        {
                            porfactor += Convert.ToInt32(tmp.Tables[0].Rows[i].ItemArray[5]);
                        }
                    }
                }

                if (porfactor >= 0)
                {
                    resultadoBullyingBullying.Add(item.Nombre, porfactor);
                    porfactor = 0;
                }
            }
            #endregion

            return resultadoBullyingBullying;
        }
        /// <summary>
        /// Calcula el resultado de cada seccion de la prueba BullyingViolencia
        /// </summary>
        /// <param name="dctx">Provee el acceso a la base de datos</param>
        /// <param name="resultadoPruebaID">ID del resultado de la prueba</param>
        /// <param name="resultadoPruebaID">ID de la prueba</param>
        /// <returns>Puntajes obtenidos en cada percetil</returns>
        public Dictionary<string, int> RetrieveResultadoPruebaBullyingViolencia(IDataContext dctx, ResultadoPruebaDinamica resultado, Alumno alumno)
        {
            #region Variables
            int porfactor = 0;
            AModelo LastObject;
            // DataSet que almacenará la respuesta del alumno
            DataSet dsRespuestasAlumno = new DataSet();
            string filter = string.Empty;
            Dictionary<string, int> resultadoBullyingViolencia = new Dictionary<string, int>();
            #endregion

            #region Se obtiene la Respuesta Prueba completa
            if (resultado == null)
                throw new Exception("No se encontró el resultado de la prueba");
            if (!(resultado is ResultadoPruebaDinamica))
                throw new Exception("El resultado no corresponde a una prueba dinámica");

            BateriaBullyingCtrl bateriaBullyingCtrl = new BateriaBullyingCtrl();
            dsRespuestasAlumno = bateriaBullyingCtrl.RetrieveResultadoBullyingViolencia(dctx, alumno);
            #endregion

            LastObject = new ModeloDinamico { ModeloID = resultado.Prueba.Modelo.ModeloID };
            List<Clasificador> clasificadores = modeloPruebaCtrl.RetrieveClasificadoresModeloDinamico(dctx, LastObject as ModeloDinamico);

            #region Calculo factores
            foreach (var item in clasificadores)
            {
                filter = "Clasificador LIKE('" + item.Nombre + "')";
                if (!String.IsNullOrEmpty(filter))
                {
                    DataSet tmp = new DataSet();
                    DataView dataVierw = new DataView(dsRespuestasAlumno.Tables[0]);
                    dataVierw.RowFilter = filter;
                    filter = string.Empty;
                    tmp.Tables.Clear();
                    tmp.Tables.Add(dataVierw.ToTable());

                    if (tmp.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < tmp.Tables[0].Rows.Count; i++)
                        {
                            porfactor += Convert.ToInt32(tmp.Tables[0].Rows[i].ItemArray[5]);
                        }
                    }
                }

                if (porfactor >= 0)
                {
                    resultadoBullyingViolencia.Add(item.Nombre, porfactor);
                    porfactor = 0;
                }
            }
            #endregion
            return resultadoBullyingViolencia;
        }
        /// <summary>
        /// Calcula el resultado de cada seccion de la prueba BullyingComunicacion
        /// </summary>
        /// <param name="dctx">Provee el acceso a la base de datos</param>
        /// <param name="resultadoPruebaID">ID del resultado de la prueba</param>
        /// <param name="resultadoPruebaID">ID de la prueba</param>
        /// <returns>Puntajes obtenidos en cada percetil</returns>
        public Dictionary<string, int> RetrieveResultadoPruebaBullyingComunicacion(IDataContext dctx, ResultadoPruebaDinamica resultado, Alumno alumno)
        {
            #region Variables
            int porfactor = 0;
            AModelo LastObject;
            // DataSet que almacenará la respuesta del alumno
            DataSet dsRespuestasAlumno = new DataSet();
            string filter = string.Empty;
            Dictionary<string, int> resultadoBullyingComunicacion = new Dictionary<string, int>();
            #endregion

            #region Se obtiene la Respuesta Prueba completa

            if (resultado == null)
                throw new Exception("No se encontró el resultado de la prueba");
            if (!(resultado is ResultadoPruebaDinamica))
                throw new Exception("El resultado no corresponde a una prueba dinámica");


            BateriaBullyingCtrl bateriaBullyingCtrl = new BateriaBullyingCtrl();
            dsRespuestasAlumno = bateriaBullyingCtrl.RetrieveResultadoBullyingComunicacion(dctx, alumno);
            #endregion

            LastObject = new ModeloDinamico { ModeloID = resultado.Prueba.Modelo.ModeloID };
            List<Clasificador> clasificadores = modeloPruebaCtrl.RetrieveClasificadoresModeloDinamico(dctx, LastObject as ModeloDinamico);

            #region Calculo factores
            foreach (var item in clasificadores)
            {
                filter = "Clasificador LIKE('" + item.Nombre + "')";
                if (!String.IsNullOrEmpty(filter))
                {
                    DataSet tmp = new DataSet();
                    DataView dataVierw = new DataView(dsRespuestasAlumno.Tables[0]);
                    dataVierw.RowFilter = filter;
                    filter = string.Empty;
                    tmp.Tables.Clear();
                    tmp.Tables.Add(dataVierw.ToTable());

                    if (tmp.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < tmp.Tables[0].Rows.Count; i++)
                        {
                            porfactor += Convert.ToInt32(tmp.Tables[0].Rows[i].ItemArray[5]);
                        }
                    }
                }

                if (porfactor >= 0)
                {
                    resultadoBullyingComunicacion.Add(item.Nombre, porfactor);
                    porfactor = 0;
                }
            }
            #endregion

            return resultadoBullyingComunicacion;
        }
        /// <summary>
        /// Calcula el resultado de cada seccion de la prueba ImagenCorporal
        /// </summary>
        /// <param name="dctx">Provee el acceso a la base de datos</param>
        /// <param name="resultadoPruebaID">ID del resultado de la prueba</param>
        /// <param name="resultadoPruebaID">ID de la prueba</param>
        /// <returns>Puntajes obtenidos en cada percetil</returns>
        public Dictionary<string, int> RetrieveResultadoPruebaBullyingImagenCorporal(IDataContext dctx, ResultadoPruebaDinamica resultado, Alumno alumno)
        {
            #region Variables
            int porfactor = 0;
            AModelo LastObject;
            // DataSet que almacenará la respuesta del alumno
            DataSet dsRespuestasAlumno = new DataSet();
            string filter = string.Empty;
            Dictionary<string, int> resultadoBullyingImagenCorporal = new Dictionary<string, int>();
            #endregion

            #region Se obtiene la Respuesta Prueba completa
            if (resultado == null)
                throw new Exception("No se encontró el resultado de la prueba");
            if (!(resultado is ResultadoPruebaDinamica))
                throw new Exception("El resultado no corresponde a una prueba dinámica");

            BateriaBullyingCtrl bateriaBullyingCtrl = new BateriaBullyingCtrl();
            dsRespuestasAlumno = bateriaBullyingCtrl.RetrieveResultadoBullyingImagen(dctx, alumno);
            #endregion

            LastObject = new ModeloDinamico { ModeloID = resultado.Prueba.Modelo.ModeloID };
            List<Clasificador> clasificadores = modeloPruebaCtrl.RetrieveClasificadoresModeloDinamico(dctx, LastObject as ModeloDinamico);

            #region Calculo factores
            foreach (var item in clasificadores)
            {
                filter = "Clasificador LIKE('" + item.Nombre + "')";
                if (!String.IsNullOrEmpty(filter))
                {
                    DataSet tmp = new DataSet();
                    DataView dataVierw = new DataView(dsRespuestasAlumno.Tables[0]);
                    dataVierw.RowFilter = filter;
                    filter = string.Empty;
                    tmp.Tables.Clear();
                    tmp.Tables.Add(dataVierw.ToTable());

                    if (tmp.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < tmp.Tables[0].Rows.Count; i++)
                        {
                            porfactor += Convert.ToInt32(tmp.Tables[0].Rows[i].ItemArray[5]);
                        }
                    }
                }

                if (porfactor >= 0)
                {
                    resultadoBullyingImagenCorporal.Add(item.Nombre, porfactor);
                    porfactor = 0;
                }
            }
            #endregion

            return resultadoBullyingImagenCorporal;
        }
        /// <summary>
        /// Calcula el resultado de cada seccion de la prueba BullyingAnsiedad
        /// </summary>
        /// <param name="dctx">Provee el acceso a la base de datos</param>
        /// <param name="resultadoPruebaID">ID del resultado de la prueba</param>
        /// <param name="resultadoPruebaID">ID de la prueba</param>
        /// <returns>Puntajes obtenidos en cada percetil</returns>
        public Dictionary<string, int> RetrieveResultadoPruebaBullyingAnsiedad(IDataContext dctx, ResultadoPruebaDinamica resultado, Alumno alumno)
        {
            #region Variables
            int porfactor = 0;
            AModelo LastObject;
            // DataSet que almacenará la respuesta del alumno
            DataSet dsRespuestasAlumno = new DataSet();
            string filter = string.Empty;
            Dictionary<string, int> resultadoBullyingAnsiedad = new Dictionary<string, int>();
            #endregion

            #region Se obtiene la Respuesta Prueba completa
            if (resultado == null)
                throw new Exception("No se encontró el resultado de la prueba");
            if (!(resultado is ResultadoPruebaDinamica))
                throw new Exception("El resultado no corresponde a una prueba dinámica");

            BateriaBullyingCtrl bateriaBullyingCtrl = new BateriaBullyingCtrl();
            dsRespuestasAlumno = bateriaBullyingCtrl.RetrieveResultadoBullyingAnsiedad(dctx, alumno);
            #endregion

            LastObject = new ModeloDinamico { ModeloID = resultado.Prueba.Modelo.ModeloID };
            List<Clasificador> clasificadores = modeloPruebaCtrl.RetrieveClasificadoresModeloDinamico(dctx, LastObject as ModeloDinamico);

            #region Calculo factores
            foreach (var item in clasificadores)
            {
                if (item.Nombre == "Ansiedad total")
                    filter = "Clave NOT IN('ANSIEDAD.04','ANSIEDAD.08','ANSIEDAD.12','ANSIEDAD.16','ANSIEDAD.20','ANSIEDAD.24','ANSIEDAD.28','ANSIEDAD.32','ANSIEDAD.36')";
                if (item.Nombre == "Ansiedad fisiológica")
                    filter = "Clave IN('ANSIEDAD.01','ANSIEDAD.05','ANSIEDAD.09','ANSIEDAD.13','ANSIEDAD.17','ANSIEDAD.19','ANSIEDAD.21','ANSIEDAD.25','ANSIEDAD.29','ANSIEDAD.33')";
                if (item.Nombre == "Hipersensibilidad")
                    filter = "Clave IN('ANSIEDAD.02','ANSIEDAD.06','ANSIEDAD.07','ANSIEDAD.10','ANSIEDAD.14','ANSIEDAD.18','ANSIEDAD.22','ANSIEDAD.26','ANSIEDAD.30','ANSIEDAD.34','ANSIEDAD.37')";
                if (item.Nombre == "Preocupaciones sociales")
                    filter = "Clave IN('ANSIEDAD.03','ANSIEDAD.11','ANSIEDAD.15','ANSIEDAD.23','ANSIEDAD.27','ANSIEDAD.31','ANSIEDAD.35')";
                if (item.Nombre == "Mentira")
                    filter = "Clave IN('ANSIEDAD.04','ANSIEDAD.08','ANSIEDAD.12','ANSIEDAD.16','ANSIEDAD.20','ANSIEDAD.24','ANSIEDAD.28','ANSIEDAD.32','ANSIEDAD.36')";

                if (!String.IsNullOrEmpty(filter))
                {
                    DataSet tmp = new DataSet();
                    DataView dataVierw = new DataView(dsRespuestasAlumno.Tables[0]);
                    dataVierw.RowFilter = filter;
                    filter = string.Empty;
                    tmp.Tables.Clear();
                    tmp.Tables.Add(dataVierw.ToTable());

                    if (tmp.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < tmp.Tables[0].Rows.Count; i++)
                        {
                            porfactor += Convert.ToInt32(tmp.Tables[0].Rows[i].ItemArray[5]);
                        }
                    }
                }

                if (porfactor >= 0)
                {
                    resultadoBullyingAnsiedad.Add(item.Nombre, porfactor);
                    porfactor = 0;
                }
            }
            #endregion

            return resultadoBullyingAnsiedad;
        }
        /// <summary>
        /// Calcula el resultado de cada seccion de la prueba BullyingDepresion
        /// </summary>
        /// <param name="dctx">Provee el acceso a la base de datos</param>
        /// <param name="resultadoPruebaID">ID del resultado de la prueba</param>
        /// <param name="resultadoPruebaID">ID de la prueba</param>
        /// <returns>Puntajes obtenidos en cada percetil</returns>
        public Dictionary<string, int> RetrieveResultadoPruebaBullyingDepresion(IDataContext dctx, ResultadoPruebaDinamica resultado, Alumno alumno)
        {
            #region Variables
            int porfactor = 0;
            AModelo LastObject;
            // DataSet que almacenará la respuesta del alumno
            DataSet dsRespuestasAlumno = new DataSet();
            string filter = string.Empty;
            Dictionary<string, int> resultadoBullyingDepresion = new Dictionary<string, int>();
            #endregion

            #region Se obtiene la Respuesta Prueba completa
            if (resultado == null)
                throw new Exception("No se encontró el resultado de la prueba");
            if (!(resultado is ResultadoPruebaDinamica))
                throw new Exception("El resultado no corresponde a una prueba dinámica");

            BateriaBullyingCtrl bateriaBullyingCtrl = new BateriaBullyingCtrl();
            dsRespuestasAlumno = bateriaBullyingCtrl.RetrieveResultadoBullyingDepresion(dctx, alumno);
            #endregion

            LastObject = new ModeloDinamico { ModeloID = resultado.Prueba.Modelo.ModeloID };
            List<Clasificador> clasificadores = modeloPruebaCtrl.RetrieveClasificadoresModeloDinamico(dctx, LastObject as ModeloDinamico);

            #region Calculo factores
            foreach (var item in clasificadores)
            {
                filter = "Clasificador LIKE('" + item.Nombre + "')";
                if (!String.IsNullOrEmpty(filter))
                {
                    DataSet tmp = new DataSet();
                    DataView dataVierw = new DataView(dsRespuestasAlumno.Tables[0]);
                    dataVierw.RowFilter = filter;
                    filter = string.Empty;
                    tmp.Tables.Clear();
                    tmp.Tables.Add(dataVierw.ToTable());

                    if (tmp.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < tmp.Tables[0].Rows.Count; i++)
                        {
                            porfactor += Convert.ToInt32(tmp.Tables[0].Rows[i].ItemArray[5]);
                        }
                    }
                }

                if (porfactor >= 0)
                {
                    resultadoBullyingDepresion.Add(item.Nombre, porfactor);
                    porfactor = 0;
                }
            }
            #endregion

            return resultadoBullyingDepresion;
        }

        private DataSet CriterioBusqueda(string filter, DataSet ds)
        {
            DataSet dsCriterio = new DataSet();
            DataView dataView = new DataView(ds.Tables[0]);
            dataView.RowFilter = filter;
            filter = string.Empty;
            dsCriterio.Tables.Clear();
            dsCriterio.Tables.Add(dataView.ToTable());
            return dsCriterio;
        }

        private string EscapeLike(string valueWithoutWildcards)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < valueWithoutWildcards.Length; i++)
            {
                char c = valueWithoutWildcards[i];
                if (c == '*' || c == '%' || c == '[' || c == ']')
                    sb.Append("[").Append(c).Append("]");
                else if (c == '\'')
                    sb.Append("''");
                else
                    sb.Append(c);
            }
            return sb.ToString();
        }

        public PlantillaKuder DataRowToPlantillaKuder(DataRow row)
        {
            PlantillaKuder obj = new PlantillaKuder();

            if (row.IsNull("PlantillaKuderID"))
                obj.PlantillaKuderID = null;
            else
                obj.PlantillaKuderID = (Int32)Convert.ChangeType(row["PlantillaKuderID"], typeof(Int32));

            if (row.IsNull("Plantilla"))
                obj.Plantilla = null;
            else
                obj.Plantilla = (string)Convert.ChangeType(row["Plantilla"], typeof(string));

            if (row.IsNull("ClasificadorID"))
                obj.ClasificadorID = null;
            else
                obj.ClasificadorID = (Int32)Convert.ChangeType(row["ClasificadorID"], typeof(Int32));

            if (row.IsNull("RespuestaOpcion"))
                obj.RespuestaOpcion = null;
            else
                obj.RespuestaOpcion = (string)Convert.ChangeType(row["RespuestaOpcion"], typeof(string));

            if (row.IsNull("GradoInteres"))
                obj.GradoInteres = null;
            else
                obj.GradoInteres = (string)Convert.ChangeType(row["GradoInteres"], typeof(string));

            if (row.IsNull("Grupo"))
                obj.Grupo = null;
            else
                obj.Grupo = (Int32)Convert.ChangeType(row["Grupo"], typeof(Int32));

            return obj;
        }

        public RespuestaAlumnoKuder DataRowToRespuestaAlumnoKuder(DataRow row)
        {
            RespuestaAlumnoKuder obj = new RespuestaAlumnoKuder();

            if (row.IsNull("ClasificadorID"))
                obj.ClasificadorID = null;
            else
                obj.ClasificadorID = (Int32)Convert.ChangeType(row["ClasificadorID"], typeof(Int32));

            if (row.IsNull("RespuestaOpcion"))
                obj.RespuestaOpcion = null;
            else
                obj.RespuestaOpcion = (string)Convert.ChangeType(row["RespuestaOpcion"], typeof(string));

            if (row.IsNull("GradoInteres"))
                obj.GradoInteres = null;
            else
                obj.GradoInteres = (string)Convert.ChangeType(row["GradoInteres"], typeof(string));

            if (row.IsNull("Grupo"))
                obj.Grupo = null;
            else
                obj.Grupo = (Int32)Convert.ChangeType(row["Grupo"], typeof(Int32));

            return obj;
        }

        public RespuestaAlumnoAllport DataRowToRespuestaAlumnoAllport(DataRow row)
        {
            RespuestaAlumnoAllport obj = new RespuestaAlumnoAllport();

            if (row.IsNull("ClasificadorID"))
                obj.ClasificadorID = null;
            else
                obj.ClasificadorID = (Int32)Convert.ChangeType(row["ClasificadorID"], typeof(Int32));

            if (row.IsNull("Opcion"))
                obj.Opcion = null;
            else
                obj.Opcion = (string)Convert.ChangeType(row["Opcion"], typeof(string));

            if (row.IsNull("Respuesta"))
                obj.Respuesta = null;
            else
                obj.Respuesta = (string)Convert.ChangeType(row["Respuesta"], typeof(string));

            if (row.IsNull("Valor"))
                obj.Valor = null;
            else
                obj.Valor = (Int32)Convert.ChangeType(row["Valor"], typeof(Int32));

            if (row.IsNull("Grupo"))
                obj.Grupo = null;
            else
                obj.Grupo = (Int32)Convert.ChangeType(row["Grupo"], typeof(Int32));


            return obj;
        }

      
    }
}
