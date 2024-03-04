using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using DevExpress.XtraCharts;
using System.Collections.Generic;
using POV.CentroEducativo.BO;
using POV.Seguridad.BO;

namespace POV.Expediente.Reports.Reports
{
    public partial class ResultadoPruebaInteligenciasMultiplesRpt : DevExpress.XtraReports.UI.XtraReport
    {
        private CentroEducativo.BO.Alumno alumno;
        private Seguridad.BO.Usuario usuario;
        private System.Collections.Generic.Dictionary<string, string> SS_RespuestaIntMul;
        private string p;

        public ResultadoPruebaInteligenciasMultiplesRpt()
        {
            InitializeComponent();
        }

        public ResultadoPruebaInteligenciasMultiplesRpt(CentroEducativo.BO.Alumno alumno, Seguridad.BO.Usuario usuario, System.Collections.Generic.Dictionary<string, string> SS_RespuestaIntMul, string p)
        {
            // TODO: Complete member initialization
            InitializeComponent();

            SetData(alumno, usuario, SS_RespuestaIntMul, p);
        }
        public void SetData(Alumno alumno, Usuario usuario, IDictionary<string, string> respuestasInteligencias, string fechaFin)
        {

            this.cNombre.Text = alumno.Nombre + " " + alumno.PrimerApellido + " " + alumno.SegundoApellido;
            this.cCorreo.Text = usuario.Email;
            this.cFinalPrueba.Text = fechaFin;

            Series IntVerb = new Series("Verbal", ViewType.Bar);
            Series IntMat = new Series("Lógico-matemática", ViewType.Bar);
            Series Intvis = new Series("Visual espacial", ViewType.Bar);
            Series IntKines = new Series("kinestesica-corporal", ViewType.Bar);
            Series IntMus = new Series("Musical-rítmica", ViewType.Bar);
            Series IntIntra = new Series("Intrapersonal", ViewType.Bar);
            Series IntInter = new Series("InInterpersonal", ViewType.Bar);
            Dictionary<string, string> Descripcion = new Dictionary<string, string>();
            Descripcion.Add("Inteligencia Verbal", "Comprende la capacidad de emplear efectivamente las" +
            "palabras ya sea en forma oral y escrita. La utilizamos cuando hablamos en una conversación formal o" +
            "informal, cuando ponemos pensamientos por escrito, escribimos poemas, o escribimos una carta a un" +
            "amigo. Es la capacidad de traducir en palabras adecuadas, pertinentes y exactas lo que piensa. Según" +
            "Gardner este tipo de capacidad está en su forma más completa en los poetas.");

            Descripcion.Add("Inteligencia Lógico - Matemática", "Consiste en la capacidad para utilizar los números en forma" +
            "efectiva y para razonar en forma lógica. Está a menudo asociada con lo que llamamos el pensamiento" +
            "científico. Utilizamos esta Inteligencia cuando podemos realizar patrones abstractos, como contar de 2 en" +
            "2 o saber si hemos recibido el vuelto correcto en el supermercado, también lo usamos para encontrar" +
            "conexiones o ver relaciones entre trozos de información.");

            Descripcion.Add("Inteligencia Visual - Espacial", "Consiste en la capacidad de percibir el mundo visual espacial" +
            "adecuadamente. Puede verse expresada claramente en la imaginación los niños. Utilizamos esta" +
            "inteligencia cuando hacemos un dibujo para expresar nuestros pensamientos o nuestras emociones, o" +
            "cuando decoramos una pieza para crear cierta atmósfera, o cuando jugamos al ajedrez. Nos permite" +
            "visualizar las cosas que queremos en nuestras vidas. Es la capacidad para formarse un modelo mental de" +
            "un espacio y para maniobrar y operar usando ese modelo. Requieren de esta clase de inteligencia, de" +
            "modo especial, los marinos, ingenieros, cirujanos, escultores, pintores.");

            Descripcion.Add("Inteligencia Kinestésica", "Se encuentra en la capacidad para utilizar el cuerpo entero en" +
            "expresar ideas y sentimientos. Esta inteligencia se vería cuando en el teclado se escribe una carta, si ando" +
            "en bicicleta, si se está en un auto o mantener el equilibrio al caminar. Es la capacidad para resolver" +
            "problemas o para elaborar productos empleando el cuerpo o parte del mismo. Muestran esta clase de" +
            "inteligencia en un nivel superior, los bailarines, los atletas, los cirujanos y artesanos");

            Descripcion.Add("Inteligencia Musical", "Es la capacidad que algunos poseen, a través de formas" +
            "musicales, percibir, discriminar y juzgar, transformar y expresar. Utilizamos esta inteligencia cuando" +
            "tocamos música, para calmarnos o estimularnos. Está muy presente cuando al escuchar alguna música la" +
            "repetimos en la mente todo el día. Implica el aprecio por la música, el canto, el tocar un instrumento" +
            "musical, etc. Entre ellos están los buenos cantantes, los canta-autores.");

            Descripcion.Add("Inteligencia Intrapersonal", "Es la capacidad para comprenderse a uno mismo y para actuar en" +
            "forma autorreflexiva y de acostumbrarse a ello. También se llama Inteligencia ''Introspectiva''. Nos permite" +
            "reflexionar acerca de nosotros mismos. Involucra el conocimiento y el darnos cuenta de los aspectos" +
            "internos de la persona, tales como los sentimientos, el proceso pensante y la intuición acerca de" +
            "realidades espirituales. Es la capacidad de auto-comprenderse, de conocerse bien, de saber cuales son" +
                "los lados brillantes de uno y cuales son los lados opacos de la propia personalidad.");

            Descripcion.Add("Inteligencia Interpersonal", "Es la capacidad de captar y evaluar en forma rápida los estados de" +
            "ánimo, intenciones, motivaciones, sentimientos de los demás. La experimentamos en forma más directa" +
            "cuando formamos parte de un trabajo en equipo ya sea deportivo, en la iglesia o tarea comunitaria. Nos" +
            "permite desarrollar un sentido de empatía y de preocupación por el tema. También nos permite mantener" +
            "nuestra identidad individual. Capacidad de entender a las otras personas. Entre ellos están los ministros," +
            "los religiosos, los orientadores, los psicólogos, los buenos vendedores, las mamás (para poder" +
            "comprender y dialogar con sus hijos");

            #region Resultados
            double porcentaje = 0;
            auxiliar[] top3 = new auxiliar[7];
            top3[0] = new auxiliar("Inteligencia Verbal", Int32.Parse(respuestasInteligencias["Inteligencia Verbal"]));
            top3[1] = new auxiliar("Inteligencia Lógico - Matemática", Int32.Parse(respuestasInteligencias["Inteligencia Lógico - Matemática"]));
            top3[2] = new auxiliar("Inteligencia Visual - Espacial", Int32.Parse(respuestasInteligencias["Inteligencia Visual - Espacial"]));
            top3[3] = new auxiliar("Inteligencia Kinestésica", Int32.Parse(respuestasInteligencias["Inteligencia Kinestésica"]));
            top3[4] = new auxiliar("Inteligencia Musical", Int32.Parse(respuestasInteligencias["Inteligencia Musical"]));
            top3[5] = new auxiliar("Inteligencia Intrapersonal", Int32.Parse(respuestasInteligencias["Inteligencia Intrapersonal"]));
            top3[6] = new auxiliar("Inteligencia Interpersonal", Int32.Parse(respuestasInteligencias["Inteligencia Interpersonal"]));


            auxiliar t;
            for (int a = 1; a < top3.Length; a++)
                for (int b = top3.Length - 1; b >= a; b--)
                {
                    if (top3[b - 1].getValor() > top3[b].getValor())
                    {
                        t = top3[b - 1];
                        top3[b - 1] = top3[b];
                        top3[b] = t;
                    }
                }



            for (int i = 4; i < top3.Length; i++)
            {
                switch (top3[i].getClasificador())
                {
                    case "Inteligencia Verbal":
                        porcentaje = top3[i].getValor() * 10;
                        IntVerb.Points.Add(new SeriesPoint(top3[i].getClasificador(), porcentaje.ToString()));
                        break;
                    case "Inteligencia Lógico - Matemática":
                        porcentaje = top3[i].getValor() * 10;
                        IntMat.Points.Add(new SeriesPoint(top3[i].getClasificador(), porcentaje.ToString()));
                        break;
                    case "Inteligencia Visual - Espacial":
                        porcentaje = top3[i].getValor() * 10;
                        Intvis.Points.Add(new SeriesPoint(top3[i].getClasificador(), porcentaje.ToString()));
                        break;
                    case "Inteligencia Kinestésica":
                        porcentaje = top3[i].getValor() * 10;
                        IntKines.Points.Add(new SeriesPoint(top3[i].getClasificador(), porcentaje.ToString()));
                        break;
                    case "Inteligencia Musical":
                        porcentaje = top3[i].getValor() * 10;
                        IntMus.Points.Add(new SeriesPoint(top3[i].getClasificador(), porcentaje.ToString()));
                        break;
                    case "Inteligencia Intrapersonal":
                        porcentaje = top3[i].getValor() * 10;
                        IntIntra.Points.Add(new SeriesPoint(top3[i].getClasificador(), porcentaje.ToString()));
                        break;
                    case "Inteligencia Interpersonal":
                        porcentaje = top3[i].getValor() * 10;
                        IntInter.Points.Add(new SeriesPoint(top3[i].getClasificador(), porcentaje.ToString()));
                        break;
                }
            }


            #endregion
            #region Agregar al reporte
            xrChart1.Series.Add(IntVerb);
            xrChart1.Series.Add(IntMat);
            xrChart1.Series.Add(Intvis);
            xrChart1.Series.Add(IntKines);
            xrChart1.Series.Add(IntMus);
            xrChart1.Series.Add(IntIntra);
            xrChart1.Series.Add(IntInter);

            xrIntAlta.Text = top3[6].getClasificador();
            xrInt2Alta.Text = top3[5].getClasificador();
            xrInt3Alta.Text = top3[4].getClasificador();
            IntAlntaDesc.Text = Descripcion[top3[6].getClasificador()];
            Int2AltaDesc.Text = Descripcion[top3[5].getClasificador()];
            Int3AltaDesc.Text = Descripcion[top3[4].getClasificador()];
            this.lblAnio.Text = "©" + DateTime.Now.ToString("yyyy") + ", Todos los derechos reservados YOY Vocacional";
            #endregion
        }
    }
    }
    

    

