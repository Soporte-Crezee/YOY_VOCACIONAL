using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using POV.Seguridad.BO;
using POV.CentroEducativo.BO;
using System.Collections.Generic;
using DevExpress.XtraCharts;

namespace POV.Expediente.Reports.Reports
{
    public partial class ResultadoPruebaEstilosdeAprendizaje : DevExpress.XtraReports.UI.XtraReport
    {
        Series Visual = new Series("Visual", ViewType.Bar);
        Series Auditivo = new Series("Auditivo", ViewType.Bar);
        Series Cinestesico = new Series("Kinestesico", ViewType.Bar);
      
       private Alumno _alumno;
        private Usuario _usuario;
        private IDictionary<string, string> _puntuacionesEstilos;
        private string _fechaFin;
       

        
          public ResultadoPruebaEstilosdeAprendizaje()
          {
              InitializeComponent();
          }




          public ResultadoPruebaEstilosdeAprendizaje(Alumno alumno, Usuario usuario, System.Collections.Generic.Dictionary<string, string> SS_RespuestaEstilo, string fecha_fin)
          {
              InitializeComponent();
              this._alumno = alumno;
              this._usuario = usuario;
              this._puntuacionesEstilos = SS_RespuestaEstilo;
              this._fechaFin = fecha_fin;

              this.SetData(alumno, usuario, SS_RespuestaEstilo, fecha_fin);


          }

         public void SetData(Alumno alumno, Usuario usuario, IDictionary<string, string> puntuacionesEstilos, string fechaFin)
        {

            this._alumno = alumno;
            this._usuario = usuario;
            this._puntuacionesEstilos = puntuacionesEstilos;
            this._fechaFin = fechaFin;

            this.cNombre.Text = _alumno.Nombre + " "  + _alumno.PrimerApellido + " " + _alumno.SegundoApellido;
            this.cCorreo.Text = _usuario.Email;
            this.cFinalPrueba.Text = _fechaFin;

            #region Resultados
             
             auxiliar[] mayor = new auxiliar[3];
           mayor[0] = new auxiliar("VISUAL",int.Parse(puntuacionesEstilos["VISUAL"]));
           mayor[1] = new auxiliar("AUDITIVO", int.Parse(puntuacionesEstilos["AUDITIVO"]));
           mayor[2] = new auxiliar("CINESTÉSICO", int.Parse(puntuacionesEstilos["CINESTÉSICO"]));

           auxiliar t;
           for (int a = 1; a < mayor.Length; a++)
               for (int b = mayor.Length - 1; b >= a; b--)
               {
                   if (mayor[b - 1].getValor() > mayor[b].getValor())
                   {
                       t = mayor[b - 1];
                       mayor[b - 1] = mayor[b];
                       mayor[b] = t;
                   }
               }
           double porcentaje = 0;
           for (int i = 0; i < mayor.Length; i++)
            {
                if (mayor[i].getClasificador() == "VISUAL")
                {
                    porcentaje = mayor[i].getValor() * 2.5;
                    Visual.Points.Add(new SeriesPoint(mayor[i].getClasificador(), porcentaje.ToString()));
                }
                if (mayor[i].getClasificador() == "AUDITIVO")
                {
                    porcentaje = mayor[i].getValor() * 2.5;
                    Auditivo.Points.Add(new SeriesPoint(mayor[i].getClasificador(), porcentaje.ToString()));
                }
                if (mayor[i].getClasificador() == "CINESTÉSICO")
                {
                    porcentaje = mayor[i].getValor() * 2.5;
                    Cinestesico.Points.Add(new SeriesPoint(mayor[i].getClasificador(), porcentaje.ToString()));
                }
            }
               
            xrChartEstilos.Series.Add(Visual);
            xrChartEstilos.Series.Add(Auditivo);
            xrChartEstilos.Series.Add(Cinestesico);
            #endregion

         
          switch (mayor[2].getClasificador()){
              case "VISUAL":
                  Desvis.Visible = true;
                  vis1.Visible = true;
                  vis2.Visible = true;
                  vis3.Visible = true;
                  vis4.Visible = true;
                  vis5.Visible = true;
                  vis6.Visible = true;
                  vis7.Visible = true;
                  vis8.Visible = true;
                  vis9.Visible = true;
                  vis10.Visible = true;
                  vis11.Visible = true;
                  break;
              case  "AUDITIVO":
                  DescAud.Visible = true;
                  aud1.Visible = true;
                  aud2.Visible = true;
                  aud3.Visible = true;
                  aud4.Visible = true;
                  aud5.Visible = true;
                  aud6.Visible = true;
                  aud7.Visible = true;
                  aud8.Visible = true;
                  aud9.Visible = true;
                  aud10.Visible = true;
                  aud11.Visible = true;

                  break;
              case "CINESTÉSICO":
                  deskin.Visible = true;
                  kin1.Visible = true;
                  kin2.Visible = true;
                  kin3.Visible = true;
                  kin4.Visible = true;
                  kin5.Visible = true;
                  kin6.Visible = true;
                  kin7.Visible = true;
                  kin8.Visible = true;
                  kin9.Visible = true;
                  kin10.Visible = true;
                  kin11.Visible = true;
                  break;

          }
             


            this.lblAnio.Text = "©" + DateTime.Now.ToString("yyyy") + ", Todos los derechos reservados YOY Vocacional";
        
    }

         private void ResultadoPruebaEstilosdeAprendizaje_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
         {

         }
    }
}

