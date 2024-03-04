using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using System.Linq;
using POV.CentroEducativo.BO;
using POV.Seguridad.BO;
using POV.Expediente.BO;
using System.Collections.Generic;
using System.Drawing.Printing;
using POV.Modelo.BO;
using System.IO;
using System.Text;
using DevExpress.XtraCharts;
using DevExpress.Office;
using DevExpress.XtraPrinting.Drawing;
using POV.Prueba.Diagnostico.Dinamica.BO;
using DevExpress.Skins;
using DevExpress.LookAndFeel;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.Utils;

namespace POV.Expediente.Reports.Reports
{
    public partial class ResultadoPruebaBullyingRpt : DevExpress.XtraReports.UI.XtraReport
    {
        private Alumno _alumno;
        private Usuario _usuario;
        private List<ResultadoBullying> _resBullying;
        private Dictionary<string, string> _imagesreporte;

        public ResultadoPruebaBullyingRpt()
        {
            InitializeComponent();
        }

        public ResultadoPruebaBullyingRpt(Alumno alumno, Usuario usuario, List<ResultadoBullying> resBullying, Dictionary<string, string> imagesreporte)
        {
            InitializeComponent();
            this.SetData(alumno, usuario, resBullying, imagesreporte);

        }

        public void SetData(Alumno alumno, Usuario usuario, List<ResultadoBullying> resBullying, Dictionary<string, string> imagesreporte)
        {
            this._alumno = alumno;
            this._usuario = usuario;
            this._resBullying = resBullying;
            this._imagesreporte = imagesreporte;

            this.cNombre.Text = _alumno.Nombre + " " + " " + _alumno.PrimerApellido + " " + _alumno.SegundoApellido;
            this.cCorreo.Text = _usuario.Email;

            #region Colores
            this.Header.ForeColor = System.Drawing.ColorTranslator.FromHtml("#05aed9");

            this.Titulo.ForeColor = System.Drawing.ColorTranslator.FromHtml("#05aed9");

            this.NamePrueba.ForeColor = System.Drawing.ColorTranslator.FromHtml("#05aed9");

            #region Titulo de tablas
            this.TableInfoEstudiante.BackColor = System.Drawing.ColorTranslator.FromHtml("#05aed9");
            this.TableInfoEstudiante.BorderColor = System.Drawing.ColorTranslator.FromHtml("#ff9e19");

            this.TableInfoPrueba.BackColor = System.Drawing.ColorTranslator.FromHtml("#05aed9");
            this.TableInfoPrueba.BorderColor = System.Drawing.ColorTranslator.FromHtml("#ff9e19");

            this.TableResultGenerales.BackColor = System.Drawing.ColorTranslator.FromHtml("#05aed9");
            this.TableResultGenerales.BorderColor = System.Drawing.ColorTranslator.FromHtml("#ff9e19");


            this.TableResultadoPrueba1.BackColor = System.Drawing.ColorTranslator.FromHtml("#05aed9");
            this.TableResultadoPrueba1.BorderColor = System.Drawing.ColorTranslator.FromHtml("#ff9e19");

            this.TableResultadoPrueba2.BackColor = System.Drawing.ColorTranslator.FromHtml("#05aed9");
            this.TableResultadoPrueba2.BorderColor = System.Drawing.ColorTranslator.FromHtml("#ff9e19");

            this.TableResultadoPrueba3.BackColor = System.Drawing.ColorTranslator.FromHtml("#05aed9");
            this.TableResultadoPrueba3.BorderColor = System.Drawing.ColorTranslator.FromHtml("#ff9e19");

            this.TableResultadoPrueba4.BackColor = System.Drawing.ColorTranslator.FromHtml("#05aed9");
            this.TableResultadoPrueba4.BorderColor = System.Drawing.ColorTranslator.FromHtml("#ff9e19");
            #endregion

            #region Resultados generales
            this.titlegralBloqueI.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ff9e19");
            this.titlegralBloqueII.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ff9e19");
            this.titlegralBloqueIII.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ff9e19");

            this.resultgralBloqueI.ForeColor = System.Drawing.ColorTranslator.FromHtml("#05aed9");
            this.resultgralBloqueII.ForeColor = System.Drawing.ColorTranslator.FromHtml("#05aed9");
            this.resultgralBloqueIII.ForeColor = System.Drawing.ColorTranslator.FromHtml("#05aed9");
            #endregion
            #region Bloque I
            this.titleBloqueI.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ff9e19");
            this.resultBloque1.ForeColor = System.Drawing.ColorTranslator.FromHtml("#05aed9");
            #endregion
            #region Bloque II
            this.titleBloqueII.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ff9e19");
            this.resultBloque2.ForeColor = System.Drawing.ColorTranslator.FromHtml("#05aed9");
            #endregion
            #region Bloque III
            this.titleBloqueIII.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ff9e19");
            this.resultBloque3.ForeColor = System.Drawing.ColorTranslator.FromHtml("#05aed9");
            #endregion
            #region Imagenes Auxiliares
            foreach (var item in imagesreporte)
            {
                switch (item.Key)
                {
                    #region Auxiliares
                    case "Bloque I Auxiliar":
                        this.auxiliarbloquei.ImageUrl = item.Value;
                        break;
                    case "Bloque II Auxiliar":
                        this.auxiliarbloqueii.ImageUrl = item.Value;
                        break;
                    case "Bloque III Auxiliar":
                        this.auxiliarbloqueiii.ImageUrl = item.Value;
                        break;
                    #endregion
                    #region Fondos
                    case "Escala 5 Fondo":
                        this.escala5fondo.ImageUrl = item.Value;
                        break;
                    case "Escala 9":
                        this.fondoescala9.ImageUrl = item.Value;
                        break;
                    #endregion
                }
            }
            #endregion
            this.xrLine1.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ff9e19");
            this.xrLine2.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ff9e19");
            #endregion

            #region Bullying
            ResaltarResultadoBateriaBullying(_resBullying, _imagesreporte);
            #endregion
            this.lblAnio.Text = "©" + DateTime.Now.ToString("yyyy") + ", Todos los derechos reservados YOY Vocacional";
        }

        private void ResaltarResultadoBateriaBullying(List<ResultadoBullying> resBullying, Dictionary<string, string> images)
        {
            Series series7 = new Series("Factores", ViewType.Line);
            #region Imagenes
            if (images.Count > 0)
            {
                foreach (var item in images)
                {
                    switch (item.Key)
                    {
                        #region General
                        case "General Bloque I":
                            generalbloqueI.ImageUrl = item.Value;
                            resultadobloqueI.ImageUrl = item.Value;
                            break;
                        case "General Bloque II":
                            generalbloqueII.ImageUrl = item.Value;
                            resultadobloqueii.ImageUrl = item.Value;
                            break;
                        case "General Bloque III":
                            generalbloqueIII.ImageUrl = item.Value;
                            resultadobloqueiii.ImageUrl = item.Value;
                            break;
                        case "Texto Bloque I":
                            resultgralBloqueI.Text = item.Value;
                            resultBloque1.Text = item.Value;
                            break;
                        case "Texto Bloque II":
                            resultgralBloqueII.Text = item.Value;
                            resultBloque2.Text = item.Value;
                            break;
                        case "Texto Bloque III":
                            resultgralBloqueIII.Text = item.Value;
                            resultBloque3.Text = item.Value;
                            break;
                        #endregion
                        #region Bloque I
                        case "Bloque I Auxiliar":
                            auxiliarbloquei.ImageUrl = item.Value;
                            break;
                        case "Escala 2 Autoridad":
                            escala2autoridad.ImageUrl = item.Value;
                            break;
                        case "Escala 2 Trangresion":
                            escala2transgresion.ImageUrl = item.Value;
                            break;
                        case "Escala 3 Empatia":
                            escala3empatia.ImageUrl = item.Value;
                            break;
                        #endregion
                        #region Bloque II
                        case "Bloque II Auxiliar":
                            auxiliarbloqueii.ImageUrl = item.Value;
                            break;
                        #region Escala 5 imagenes
                        case "Escala 5 Fondo":
                            escala5fondo.ImageUrl = item.Value;
                            break;
                        case "Escala 5 Relacional":
                            escala5relacional.ImageUrl = item.Value;
                            break;
                        case "Escala 5 Fisica":
                            escala5fisica.ImageUrl = item.Value;
                            break;
                        case "Escala 5 Verbal":
                            escala5verbal.ImageUrl = item.Value;
                            break;
                        case "Escala 5 Indirecta":
                            escala5indirecta.ImageUrl = item.Value;
                            break;
                        #endregion
                        #region Escala 5 texto
                        case "Escala 5 Relacional Text":
                            lbl5relacional.Text = item.Value;
                            if (item.Value == "Sin Riesgo")
                                lbl5relacional.ForeColor = System.Drawing.ColorTranslator.FromHtml("#00a651");
                            else if (item.Value == "Riesgo Medio")
                                lbl5relacional.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffcb08");
                            else if (item.Value == "Riesgo Alto")
                                lbl5relacional.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ed1c24");
                            break;
                        case "Escala 5 Fisica Text":
                            lbl5fisica.Text = item.Value;
                            if (item.Value == "Sin Riesgo")
                                lbl5fisica.ForeColor = System.Drawing.ColorTranslator.FromHtml("#00a651");
                            else if (item.Value == "Riesgo Medio")
                                lbl5fisica.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffcb08");
                            else if (item.Value == "Riesgo Alto")
                                lbl5fisica.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ed1c24");
                            break;
                        case "Escala 5 Verbal Text":
                            lbl5verbal.Text = item.Value;
                            if (item.Value == "Sin Riesgo")
                                lbl5verbal.ForeColor = System.Drawing.ColorTranslator.FromHtml("#00a651");
                            else if (item.Value == "Riesgo Medio")
                                lbl5verbal.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffcb08");
                            else if (item.Value == "Riesgo Alto")
                                lbl5verbal.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ed1c24");
                            break;
                        case "Escala 5 Indirecta Text":
                            lbl5indirecta.Text = item.Value;
                            if (item.Value == "Sin Riesgo")
                                lbl5indirecta.ForeColor = System.Drawing.ColorTranslator.FromHtml("#00a651");
                            else if (item.Value == "Riesgo Medio")
                                lbl5indirecta.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffcb08");
                            else if (item.Value == "Riesgo Alto")
                                lbl5indirecta.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ed1c24");

                            break;
                        #endregion
                        case "Escala 6 Ciberbullying":
                            escala6ciberbullying.ImageUrl = item.Value;
                            break;
                        #endregion
                        #region Bloque III
                        case "Bloque III Auxiliar":
                            auxiliarbloqueiii.ImageUrl = item.Value;
                            break;
                        case "Escala 8 Violencia":
                            escala8violencia.ImageUrl = item.Value;
                            break;
                        case "Escala 9":
                            fondoescala9.ImageUrl = item.Value;
                            break;
                        case "Escala 10 Imagen":
                            escala10imagen.ImageUrl = item.Value;
                            break;
                        case "Escala 12 Depresion":
                            escala12depresion.ImageUrl = item.Value;
                            break;
                        #endregion
                    }
                }
            }
            #endregion
            if (resBullying.Count > 0)
            {
                foreach (var resultado in resBullying)
                {
                    foreach (var factor in resultado.ResultadoFactores)
                    {
                        switch (factor.Key)
                        {
                            #region Escala 1
                            #region Académico
                            case "Autoconcepto Académico":
                                this.pBar1Academico.Properties.Minimum = 0;
                                this.pBar1Academico.Properties.Maximum = 30;
                                // Deshabilidta Skin por default
                                this.pBar1Academico.LookAndFeel.UseDefaultLookAndFeel = false;
                                // Tipo de estilo
                                this.pBar1Academico.Properties.LookAndFeel.Style = LookAndFeelStyle.Flat;
                                // Colo de inicio y de fin
                                this.pBar1Academico.Properties.StartColor = System.Drawing.ColorTranslator.FromHtml("#7fd2e6");
                                this.pBar1Academico.Properties.EndColor = System.Drawing.ColorTranslator.FromHtml("#7fd2e6");
                                // Tipo de estilo de vista de la barra de progreso
                                this.pBar1Academico.Properties.ProgressViewStyle = ProgressViewStyle.Solid;
                                this.pBar1Academico.Properties.BorderStyle = BorderStyles.NoBorder;

                                if (factor.Value <= 9)
                                {
                                    if (factor.Value == 0)
                                        this.pBar1Academico.Position = 1;
                                    else
                                        this.pBar1Academico.Position = factor.Value;
                                    this.lbl1academico.Text = "Muy pobre";
                                    this.lbl1academico.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ed1c24");
                                }
                                else if (factor.Value >= 10 && factor.Value <= 24)
                                {
                                    this.pBar1Academico.Position = factor.Value;
                                    this.lbl1academico.Text = "Medio";
                                    this.lbl1academico.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffcb08");
                                }
                                else if (factor.Value >= 25)
                                {
                                    this.pBar1Academico.Position = factor.Value;
                                    this.lbl1academico.Text = "Elevado";
                                    this.lbl1academico.ForeColor = System.Drawing.ColorTranslator.FromHtml("#00a651");
                                }
                                break;
                            #endregion
                            #region Social
                            case "Autoconcepto Social":
                                this.pBar1Social.Properties.Minimum = 0;
                                this.pBar1Social.Properties.Maximum = 30;
                                // Deshabilidta Skin por default
                                this.pBar1Social.LookAndFeel.UseDefaultLookAndFeel = false;
                                // Tipo de estilo
                                this.pBar1Social.Properties.LookAndFeel.Style = LookAndFeelStyle.Flat;
                                // Colo de inicio y de fin
                                this.pBar1Social.Properties.StartColor = System.Drawing.ColorTranslator.FromHtml("#37c1dd");
                                this.pBar1Social.Properties.EndColor = System.Drawing.ColorTranslator.FromHtml("#37c1dd");
                                // Tipo de estilo de vista de la barra de progreso
                                this.pBar1Social.Properties.ProgressViewStyle = ProgressViewStyle.Solid;
                                this.pBar1Social.Properties.BorderStyle = BorderStyles.NoBorder;

                                if (factor.Value <= 9)
                                {
                                    if (factor.Value == 0)
                                        this.pBar1Social.Position = 1;
                                    else
                                        this.pBar1Social.Position = factor.Value;
                                    this.lbl1social.Text = "Muy pobre";
                                    this.lbl1social.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ed1c24");
                                }
                                else if (factor.Value >= 10 && factor.Value <= 24)
                                {
                                    this.pBar1Social.Position = factor.Value;
                                    this.lbl1social.Text = "Medio";
                                    this.lbl1social.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffcb08");
                                }
                                else if (factor.Value >= 25)
                                {
                                    this.pBar1Social.Position = factor.Value;
                                    this.lbl1social.Text = "Elevado";
                                    this.lbl1social.ForeColor = System.Drawing.ColorTranslator.FromHtml("#00a651");
                                }
                                break;
                            #endregion
                            #region Emocional
                            case "Autoconcepto Emocional":
                                this.pBar1Emocional.Properties.Minimum = 0;
                                this.pBar1Emocional.Properties.Maximum = 30;
                                // Deshabilidta Skin por default
                                this.pBar1Emocional.LookAndFeel.UseDefaultLookAndFeel = false;
                                // Tipo de estilo
                                this.pBar1Emocional.LookAndFeel.Style = LookAndFeelStyle.Flat;
                                // Colo de inicio y de fin
                                this.pBar1Emocional.Properties.StartColor = System.Drawing.ColorTranslator.FromHtml("#1b9abe");
                                this.pBar1Emocional.Properties.EndColor = System.Drawing.ColorTranslator.FromHtml("#1b9abe");
                                // Tipo de estilo de vista de la barra de progreso
                                this.pBar1Emocional.Properties.ProgressViewStyle = ProgressViewStyle.Solid;
                                this.pBar1Emocional.Properties.BorderStyle = BorderStyles.NoBorder;

                                if (factor.Value <= 9)
                                {
                                    if (factor.Value == 0)
                                        this.pBar1Emocional.Position = 1;
                                    else
                                        this.pBar1Emocional.Position = factor.Value;
                                    this.lbl1emocional.Text = "Muy pobre";
                                    this.lbl1emocional.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ed1c24");
                                }
                                else if (factor.Value >= 10 && factor.Value <= 24)
                                {
                                    this.pBar1Emocional.Position = factor.Value;
                                    this.lbl1emocional.Text = "Medio";
                                    this.lbl1emocional.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffcb08");
                                }
                                else if (factor.Value >= 25)
                                {
                                    this.pBar1Emocional.Position = factor.Value;
                                    this.lbl1emocional.Text = "Elevado";
                                    this.lbl1emocional.ForeColor = System.Drawing.ColorTranslator.FromHtml("#00a651");
                                }
                                break;
                            #endregion
                            #region Familiar
                            case "Autoconcepto Familiar":
                                this.pBar1Familiar.Properties.Minimum = 0;
                                this.pBar1Familiar.Properties.Maximum = 30;
                                // Deshabilidta Skin por default
                                this.pBar1Familiar.LookAndFeel.UseDefaultLookAndFeel = false;
                                // Tipo de estilo
                                this.pBar1Familiar.LookAndFeel.Style = LookAndFeelStyle.Flat;
                                // Colo de inicio y de fin
                                this.pBar1Familiar.Properties.StartColor = System.Drawing.ColorTranslator.FromHtml("#7fd2e6");
                                this.pBar1Familiar.Properties.EndColor = System.Drawing.ColorTranslator.FromHtml("#7fd2e6");
                                // Tipo de estilo de vista de la barra de progreso
                                this.pBar1Familiar.Properties.ProgressViewStyle = ProgressViewStyle.Solid;
                                this.pBar1Familiar.Properties.BorderStyle = BorderStyles.NoBorder;

                                if (factor.Value <= 9)
                                {
                                    if (factor.Value == 0)
                                        this.pBar1Familiar.Position = 1;
                                    else
                                        this.pBar1Familiar.Position = factor.Value;
                                    this.lbl1familiar.Text = "Muy pobre";
                                    this.lbl1familiar.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ed1c24");
                                }
                                else if (factor.Value >= 10 && factor.Value <= 24)
                                {
                                    this.pBar1Familiar.Position = factor.Value;
                                    this.lbl1familiar.Text = "Medio";
                                    this.lbl1familiar.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffcb08");
                                }
                                else if (factor.Value >= 25)
                                {
                                    this.pBar1Familiar.Position = factor.Value;
                                    this.lbl1familiar.Text = "Elevado";
                                    this.lbl1familiar.ForeColor = System.Drawing.ColorTranslator.FromHtml("#00a651");
                                }
                                break;
                            #endregion
                            #region Físico
                            case "Autoconcepto Físico":
                                this.pBar1Fisico.Properties.Minimum = 0;
                                this.pBar1Fisico.Properties.Maximum = 30;
                                // Deshabilidta Skin por default
                                this.pBar1Fisico.LookAndFeel.UseDefaultLookAndFeel = false;
                                // Tipo de estilo
                                this.pBar1Fisico.LookAndFeel.Style = LookAndFeelStyle.Flat;
                                // Colo de inicio y de fin
                                this.pBar1Fisico.Properties.StartColor = System.Drawing.ColorTranslator.FromHtml("#37c1dd");
                                this.pBar1Fisico.Properties.EndColor = System.Drawing.ColorTranslator.FromHtml("#37c1dd");
                                // Tipo de estilo de vista de la barra de progreso
                                this.pBar1Fisico.Properties.ProgressViewStyle = ProgressViewStyle.Solid;
                                this.pBar1Fisico.Properties.BorderStyle = BorderStyles.NoBorder;

                                if (factor.Value <= 9)
                                {
                                    if (factor.Value == 0)
                                        this.pBar1Fisico.Position = 1;
                                    else
                                        this.pBar1Fisico.Position = factor.Value;
                                    this.lbl1fisico.Text = "Muy pobre";
                                    this.lbl1fisico.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ed1c24");
                                }
                                else if (factor.Value >= 10 && factor.Value <= 24)
                                {
                                    this.pBar1Fisico.Position = factor.Value;
                                    this.lbl1fisico.Text = "Medio";
                                    this.lbl1fisico.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffcb08");
                                }
                                else if (factor.Value >= 25)
                                {
                                    this.pBar1Fisico.Position = factor.Value;
                                    this.lbl1fisico.Text = "Elevado";
                                    this.lbl1fisico.ForeColor = System.Drawing.ColorTranslator.FromHtml("#00a651");
                                }
                                break;
                            #endregion
                            #endregion
                            #region Escala 4
                            #region Afiliación
                            case "Afiliación":
                                this.pBar4Afilicacion.Properties.Minimum = 0;
                                this.pBar4Afilicacion.Properties.Maximum = 40;
                                // Deshabilidta Skin por default
                                this.pBar4Afilicacion.LookAndFeel.UseDefaultLookAndFeel = false;
                                // Tipo de estilo
                                this.pBar4Afilicacion.LookAndFeel.Style = LookAndFeelStyle.Flat;
                                // Colo de inicio y de fin
                                this.pBar4Afilicacion.Properties.StartColor = System.Drawing.ColorTranslator.FromHtml("#8ac33f");
                                this.pBar4Afilicacion.Properties.EndColor = System.Drawing.ColorTranslator.FromHtml("#8ac33f");
                                // Tipo de estilo de vista de la barra de progreso
                                this.pBar4Afilicacion.Properties.ProgressViewStyle = ProgressViewStyle.Broken;
                                this.pBar4Afilicacion.Properties.BorderStyle = BorderStyles.NoBorder;

                                if (factor.Value <= 12)
                                {
                                    if (factor.Value == 0)
                                        this.pBar4Afilicacion.Position = 1;
                                    else
                                        this.pBar4Afilicacion.Position = factor.Value;
                                    this.lbl4afiliacion.Text = "Conflicto Marcado";
                                    this.lbl4afiliacion.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ed1c24");
                                }
                                else if (factor.Value >= 13 && factor.Value <= 32)
                                {
                                    this.pBar4Afilicacion.Position = factor.Value;
                                    this.lbl4afiliacion.Text = "Riesgo medio";
                                    this.lbl4afiliacion.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffcb08");
                                }
                                else if (factor.Value >= 33)
                                {
                                    this.pBar4Afilicacion.Position = factor.Value;
                                    this.lbl4afiliacion.Text = "Sin Riesgo";
                                    this.lbl4afiliacion.ForeColor = System.Drawing.ColorTranslator.FromHtml("#00a651");
                                }
                                break;
                            #endregion
                            #region Mejoramiento
                            case "Mejoramiento personal":
                                this.pBar4Mejora.Properties.Minimum = 0;
                                this.pBar4Mejora.Properties.Maximum = 40;
                                // Deshabilidta Skin por default
                                this.pBar4Mejora.LookAndFeel.UseDefaultLookAndFeel = false;
                                // Tipo de estilo
                                this.pBar4Mejora.LookAndFeel.Style = LookAndFeelStyle.Flat;
                                // Colo de inicio y de fin
                                this.pBar4Mejora.Properties.StartColor = System.Drawing.ColorTranslator.FromHtml("#20a59a");
                                this.pBar4Mejora.Properties.EndColor = System.Drawing.ColorTranslator.FromHtml("#20a59a");
                                // Tipo de estilo de vista de la barra de progreso
                                this.pBar4Mejora.Properties.ProgressViewStyle = ProgressViewStyle.Broken;
                                this.pBar4Mejora.Properties.BorderStyle = BorderStyles.NoBorder;

                                if (factor.Value <= 12)
                                {
                                    if (factor.Value == 0)
                                        this.pBar4Mejora.Position = 1;
                                    else
                                        this.pBar4Mejora.Position = factor.Value;
                                    this.lbl4mejorap.Text = "Conflicto Marcado";
                                    this.lbl4mejorap.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ed1c24");
                                }
                                else if (factor.Value >= 13 && factor.Value <= 32)
                                {
                                    this.pBar4Mejora.Position = factor.Value;
                                    this.lbl4mejorap.Text = "Riesgo medio";
                                    this.lbl4mejorap.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffcb08");
                                }
                                else if (factor.Value >= 33)
                                {
                                    this.pBar4Mejora.Position = factor.Value;
                                    this.lbl4mejorap.Text = "Sin Riesgo";
                                    this.lbl4mejorap.ForeColor = System.Drawing.ColorTranslator.FromHtml("#00a651");
                                }
                                break;
                            #endregion
                            #region Agresividad
                            case "Agresividad":
                                this.pBar4Agresividad.Properties.Minimum = 0;
                                this.pBar4Agresividad.Properties.Maximum = 40;
                                // Deshabilidta Skin por default
                                this.pBar4Agresividad.LookAndFeel.UseDefaultLookAndFeel = false;
                                // Tipo de estilo
                                this.pBar4Agresividad.LookAndFeel.Style = LookAndFeelStyle.Flat;
                                // Colo de inicio y de fin
                                this.pBar4Agresividad.Properties.StartColor = System.Drawing.ColorTranslator.FromHtml("#ef592b");
                                this.pBar4Agresividad.Properties.EndColor = System.Drawing.ColorTranslator.FromHtml("#ef592b");
                                // Tipo de estilo de vista de la barra de progreso
                                this.pBar4Agresividad.Properties.ProgressViewStyle = ProgressViewStyle.Broken;
                                this.pBar4Agresividad.Properties.BorderStyle = BorderStyles.NoBorder;

                                if (factor.Value <= 12)
                                {
                                    if (factor.Value == 0)
                                        this.pBar4Agresividad.Position = 1;
                                    else
                                        this.pBar4Agresividad.Position = factor.Value;
                                    this.lbl4agresividad.Text = "Conflicto Marcado";
                                    this.lbl4agresividad.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ed1c24");
                                }
                                else if (factor.Value >= 13 && factor.Value <= 32)
                                {
                                    this.pBar4Agresividad.Position = factor.Value;
                                    this.lbl4agresividad.Text = "Riesgo medio";
                                    this.lbl4agresividad.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffcb08");
                                }
                                else if (factor.Value >= 33)
                                {
                                    this.pBar4Agresividad.Position = factor.Value;
                                    this.lbl4agresividad.Text = "Sin Riesgo";
                                    this.lbl4agresividad.ForeColor = System.Drawing.ColorTranslator.FromHtml("#00a651");
                                }
                                break;
                            #endregion
                            #region Descalificación
                            case "Descalificación personal":
                                this.pBar4Descalificacion.Properties.Minimum = 0;
                                this.pBar4Descalificacion.Properties.Maximum = 40;
                                // Deshabilidta Skin por default
                                this.pBar4Descalificacion.LookAndFeel.UseDefaultLookAndFeel = false;
                                // Tipo de estilo
                                this.pBar4Descalificacion.LookAndFeel.Style = LookAndFeelStyle.Flat;
                                // Colo de inicio y de fin
                                this.pBar4Descalificacion.Properties.StartColor = System.Drawing.ColorTranslator.FromHtml("#f59321");
                                this.pBar4Descalificacion.Properties.EndColor = System.Drawing.ColorTranslator.FromHtml("#f59321");
                                // Tipo de estilo de vista de la barra de progreso
                                this.pBar4Descalificacion.Properties.ProgressViewStyle = ProgressViewStyle.Broken;
                                this.pBar4Descalificacion.Properties.BorderStyle = BorderStyles.NoBorder;

                                if (factor.Value <= 12)
                                {
                                    if (factor.Value == 0)
                                        this.pBar4Descalificacion.Position = 1;
                                    else
                                        this.pBar4Descalificacion.Position = factor.Value;
                                    this.lbl4descalificacionp.Text = "Conflicto Marcado";
                                    this.lbl4descalificacionp.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ed1c24");
                                }
                                else if (factor.Value >= 13 && factor.Value <= 32)
                                {
                                    this.pBar4Descalificacion.Position = factor.Value;
                                    this.lbl4descalificacionp.Text = "Riesgo medio";
                                    this.lbl4descalificacionp.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffcb08");
                                }
                                else if (factor.Value >= 33)
                                {
                                    this.pBar4Descalificacion.Position = factor.Value;
                                    this.lbl4descalificacionp.Text = "Sin Riesgo";
                                    this.lbl4descalificacionp.ForeColor = System.Drawing.ColorTranslator.FromHtml("#00a651");
                                }
                                break;
                            #endregion
                            #endregion
                            #region Escala 7
                            #region Emocional
                            case "Afectación emocional":
                                series7.Points.Add(new SeriesPoint("AE", factor.Value));
                                if (factor.Value <= 24)
                                {
                                    lbl7afectacion.Text = "Sin Riesgo";
                                    lbl7afectacion.ForeColor = System.Drawing.ColorTranslator.FromHtml("#00a651");
                                }
                                else if (factor.Value >= 25 && factor.Value <= 91)
                                {
                                    lbl7afectacion.Text = "Riesgo medio";
                                    lbl7afectacion.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffcb08");
                                }
                                else if (factor.Value >= 92)
                                {
                                    lbl7afectacion.Text = "Muy alto riesgo";
                                    lbl7afectacion.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ed1c24");
                                }
                                break;
                            #endregion
                            #region Desequilibrio
                            case "Desequilibrio de poder":
                                series7.Points.Add(new SeriesPoint("DP", factor.Value));
                                if (factor.Value <= 14)
                                {
                                    lbl7desequilibrio.Text = "Sin Riesgo";
                                    lbl7desequilibrio.ForeColor = System.Drawing.ColorTranslator.FromHtml("#00a651");
                                }
                                else if (factor.Value >= 15 && factor.Value <= 45)
                                {
                                    lbl7desequilibrio.Text = "Riesgo medio";
                                    lbl7desequilibrio.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffcb08");
                                }
                                else if (factor.Value >= 46)
                                {
                                    lbl7desequilibrio.Text = "Muy alto riesgo";
                                    lbl7desequilibrio.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ed1c24");
                                }
                                break;
                            #endregion
                            #region Evasión
                            case "Evasión sistemática":
                                series7.Points.Add(new SeriesPoint("ES", factor.Value));
                                if (factor.Value <= 15)
                                {
                                    lbl7evasion.Text = "Sin Riesgo";
                                    lbl7evasion.ForeColor = System.Drawing.ColorTranslator.FromHtml("#00a651");
                                }
                                else if (factor.Value >= 16 && factor.Value <= 54)
                                {
                                    lbl7evasion.Text = "Riesgo medio";
                                    lbl7evasion.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffcb08");
                                }
                                else if (factor.Value >= 55)
                                {
                                    lbl7evasion.Text = "Muy alto riesgo";
                                    lbl7evasion.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ed1c24");
                                }
                                break;
                            #endregion
                            #region Observador
                            case "Observador de bullying":
                                series7.Points.Add(new SeriesPoint("OB", factor.Value));
                                if (factor.Value <= 17)
                                {
                                    lbl7observador.Text = "Sin Riesgo";
                                    lbl7observador.ForeColor = System.Drawing.ColorTranslator.FromHtml("#00a651");
                                }
                                else if (factor.Value >= 18 && factor.Value <= 59)
                                {
                                    lbl7observador.Text = "Riesgo medio";
                                    lbl7observador.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffcb08");
                                }
                                else if (factor.Value >= 60)
                                {
                                    lbl7observador.Text = "Muy alto riesgo";
                                    lbl7observador.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ed1c24");
                                }
                                break;
                            #endregion
                            #region Violencia física
                            case "Violencia física":
                                series7.Points.Add(new SeriesPoint("VF", factor.Value));
                                if (factor.Value <= 14)
                                {
                                    lbl7vfisica.Text = "Sin Riesgo";
                                    lbl7vfisica.ForeColor = System.Drawing.ColorTranslator.FromHtml("#00a651");
                                }
                                else if (factor.Value >= 15 && factor.Value <= 45)
                                {
                                    lbl7vfisica.Text = "Riesgo medio";
                                    lbl7vfisica.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffcb08");
                                }
                                else if (factor.Value >= 46)
                                {
                                    lbl7vfisica.Text = "Muy alto riesgo";
                                    lbl7vfisica.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ed1c24");
                                }
                                break;
                            #endregion
                            #region Violencia verbal
                            case "Violencia verbal":
                                series7.Points.Add(new SeriesPoint("VV", factor.Value));
                                if (factor.Value <= 13)
                                {
                                    lbl7vverbal.Text = "Sin Riesgo";
                                    lbl7vverbal.ForeColor = System.Drawing.ColorTranslator.FromHtml("#00a651");
                                }
                                else if (factor.Value >= 14 && factor.Value <= 43)
                                {
                                    lbl7vverbal.Text = "Riesgo medio";
                                    lbl7vverbal.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffcb08");
                                }
                                else if (factor.Value >= 44)
                                {
                                    lbl7vverbal.Text = "Muy alto riesgo";
                                    lbl7vverbal.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ed1c24");
                                }
                                break;
                            #endregion
                            #endregion
                            #region Escala 9
                            case "Comunicación con mi madre":
                                if (factor.Value <= 13)
                                {
                                    lbl9madre.Text = "Riesgo alto";
                                    lbl9madre.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ed1c24");
                                }
                                else if (factor.Value > 13 && factor.Value <= 42)
                                {
                                    lbl9madre.Text = "Riesgo medio";
                                    lbl9madre.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffcb08");
                                }
                                else if (factor.Value > 42)
                                {
                                    lbl9madre.Text = "Sin riesgo";
                                    lbl9madre.ForeColor = System.Drawing.ColorTranslator.FromHtml("#00a651");
                                }
                                break;
                            case "Comunicación con mi padre":
                                if (factor.Value <= 13)
                                {
                                    lbl9padre.Text = "Riesgo alto";
                                    lbl9padre.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ed1c24");
                                }
                                else if (factor.Value > 13 && factor.Value <= 42)
                                {
                                    lbl9padre.Text = "Riesgo medio";
                                    lbl9padre.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffcb08");
                                }
                                else if (factor.Value > 42)
                                {
                                    lbl9padre.Text = "Sin riesgo";
                                    lbl9padre.ForeColor = System.Drawing.ColorTranslator.FromHtml("#00a651");
                                }
                                break;
                            case "Soporte de mis padres":
                                if (factor.Value <= 10)
                                {
                                    lbl9padres.Text = "Riesgo alto";
                                    lbl9padres.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ed1c24");
                                }
                                else if (factor.Value > 11 && factor.Value <= 28)
                                {
                                    lbl9padres.Text = "Riesgo medio";
                                    lbl9padres.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffcb08");
                                }
                                else if (factor.Value > 29)
                                {
                                    lbl9padres.Text = "Sin riesgo";
                                    lbl9padres.ForeColor = System.Drawing.ColorTranslator.FromHtml("#00a651");
                                }
                                break;
                            #endregion
                            #region Escala 11
                            #region Total
                            case "Ansiedad total":
                                this.pBar11total.Properties.Minimum = 0;
                                this.pBar11total.Properties.Maximum = 28;
                                // Deshabilidta Skin por default
                                this.pBar11total.LookAndFeel.UseDefaultLookAndFeel = false;
                                // Tipo de estilo
                                this.pBar11total.LookAndFeel.Style = LookAndFeelStyle.Flat;
                                // Colo de inicio y de fin
                                this.pBar11total.Properties.StartColor = System.Drawing.ColorTranslator.FromHtml("#3d464f");
                                this.pBar11total.Properties.EndColor = System.Drawing.ColorTranslator.FromHtml("#3d464f");
                                // Tipo de estilo de vista de la barra de progreso
                                this.pBar11total.Properties.ProgressViewStyle = ProgressViewStyle.Solid;
                                this.pBar11total.Properties.BorderStyle = BorderStyles.NoBorder;

                                if (factor.Value <= 7)
                                {
                                    if (factor.Value == 0)
                                        pBar11total.Position = 1;
                                    else
                                        pBar11total.Position = factor.Value;
                                    lbl11total.Text = "Sin Riesgo";
                                    lbl11total.ForeColor = System.Drawing.ColorTranslator.FromHtml("#00a651");
                                }
                                else if (factor.Value >= 8 && factor.Value <= 21)
                                {
                                    pBar11total.Position = factor.Value;
                                    lbl11total.Text = "Riesgo Medio";
                                    lbl11total.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffcb08");
                                }
                                else if (factor.Value >= 22)
                                {
                                    pBar11total.Position = factor.Value;
                                    lbl11total.Text = "Riesgo Alto";
                                    lbl11total.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ed1c24");
                                }
                                break;
                            #endregion
                            #region Fisiológica
                            case "Ansiedad fisiológica":
                                this.pBar11fisiologica.Properties.Minimum = 0;
                                this.pBar11fisiologica.Properties.Maximum = 28;
                                // Deshabilidta Skin por default
                                this.pBar11fisiologica.LookAndFeel.UseDefaultLookAndFeel = false;
                                // Tipo de estilo
                                this.pBar11fisiologica.LookAndFeel.Style = LookAndFeelStyle.Flat;
                                // Colo de inicio y de fin
                                this.pBar11fisiologica.Properties.StartColor = System.Drawing.ColorTranslator.FromHtml("#ffcd00");
                                this.pBar11fisiologica.Properties.EndColor = System.Drawing.ColorTranslator.FromHtml("#ffcd00");
                                // Tipo de estilo de vista de la barra de progreso
                                this.pBar11fisiologica.Properties.ProgressViewStyle = ProgressViewStyle.Solid;
                                this.pBar11fisiologica.Properties.BorderStyle = BorderStyles.NoBorder;

                                if (factor.Value <= 2)
                                {
                                    if (factor.Value == 0)
                                        pBar11fisiologica.Position = 1;
                                    else
                                        pBar11fisiologica.Position = factor.Value;
                                    lbl11fisiologica.Text = "Sin Riesgo";
                                    lbl11fisiologica.ForeColor = System.Drawing.ColorTranslator.FromHtml("#00a651");
                                }
                                else if (factor.Value >= 3 && factor.Value <= 8)
                                {
                                    pBar11fisiologica.Position = factor.Value;
                                    lbl11fisiologica.Text = "Riesgo Medio";
                                    lbl11fisiologica.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffcb08");
                                }
                                else if (factor.Value >= 9)
                                {
                                    pBar11fisiologica.Position = factor.Value;
                                    lbl11fisiologica.Text = "Riesgo Alto";
                                    lbl11fisiologica.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ed1c24");
                                }
                                break;
                            #endregion
                            #region Hipersensibilidad
                            case "Hipersensibilidad":
                                this.pBar11hipersensibilidad.Properties.Minimum = 0;
                                this.pBar11hipersensibilidad.Properties.Maximum = 28;
                                // Deshabilidta Skin por default
                                this.pBar11hipersensibilidad.LookAndFeel.UseDefaultLookAndFeel = false;
                                // Tipo de estilo
                                this.pBar11hipersensibilidad.LookAndFeel.Style = LookAndFeelStyle.Flat;
                                // Colo de inicio y de fin
                                this.pBar11hipersensibilidad.Properties.StartColor = System.Drawing.ColorTranslator.FromHtml("#08bfb3");
                                this.pBar11hipersensibilidad.Properties.EndColor = System.Drawing.ColorTranslator.FromHtml("#08bfb3");
                                // Tipo de estilo de vista de la barra de progreso
                                this.pBar11hipersensibilidad.Properties.ProgressViewStyle = ProgressViewStyle.Solid;
                                this.pBar11hipersensibilidad.Properties.BorderStyle = BorderStyles.NoBorder;

                                if (factor.Value <= 3)
                                {
                                    if (factor.Value == 0)
                                        pBar11hipersensibilidad.Position = 1;
                                    else
                                        pBar11hipersensibilidad.Position = factor.Value;
                                    lbl11hipersensibilidad.Text = "Sin Riesgo";
                                    lbl11hipersensibilidad.ForeColor = System.Drawing.ColorTranslator.FromHtml("#00a651");
                                }
                                else if (factor.Value >= 4 && factor.Value <= 9)
                                {
                                    pBar11hipersensibilidad.Position = factor.Value;
                                    lbl11hipersensibilidad.Text = "Riesgo Medio";
                                    lbl11hipersensibilidad.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffcb08");
                                }
                                else if (factor.Value >= 10)
                                {
                                    pBar11hipersensibilidad.Position = factor.Value;
                                    lbl11hipersensibilidad.Text = "Riesgo Alto";
                                    lbl11hipersensibilidad.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ed1c24");
                                }
                                break;
                            #endregion
                            #region Preocupaciones
                            case "Preocupaciones sociales":
                                this.pBar11preocupaciones.Properties.Minimum = 0;
                                this.pBar11preocupaciones.Properties.Maximum = 28;
                                // Deshabilidta Skin por default
                                this.pBar11preocupaciones.LookAndFeel.UseDefaultLookAndFeel = false;
                                // Tipo de estilo
                                this.pBar11preocupaciones.LookAndFeel.Style = LookAndFeelStyle.Flat;
                                // Colo de inicio y de fin
                                this.pBar11preocupaciones.Properties.StartColor = System.Drawing.ColorTranslator.FromHtml("#1c7590");
                                this.pBar11preocupaciones.Properties.EndColor = System.Drawing.ColorTranslator.FromHtml("#1c7590");
                                // Tipo de estilo de vista de la barra de progreso
                                this.pBar11preocupaciones.Properties.ProgressViewStyle = ProgressViewStyle.Solid;
                                this.pBar11preocupaciones.Properties.BorderStyle = BorderStyles.NoBorder;

                                if (factor.Value <= 2)
                                {
                                    if (factor.Value == 0)
                                        pBar11preocupaciones.Position = 1;
                                    else
                                        pBar11preocupaciones.Position = factor.Value;
                                    lbl11preocupaciones.Text = "Sin Riesgo";
                                    lbl11preocupaciones.ForeColor = System.Drawing.ColorTranslator.FromHtml("#00a651");
                                }
                                else if (factor.Value >= 3 && factor.Value <= 6)
                                {
                                    pBar11preocupaciones.Position = factor.Value;
                                    lbl11preocupaciones.Text = "Riesgo Medio";
                                    lbl11preocupaciones.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffcb08");
                                }
                                else if (factor.Value >= 7)
                                {
                                    pBar11preocupaciones.Position = factor.Value;
                                    lbl11preocupaciones.Text = "Riesgo Alto";
                                    lbl11preocupaciones.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ed1c24");
                                }
                                break;
                            #endregion
                            #region Mentira
                            case "Mentira":
                                this.pBar11mentira.Properties.Minimum = 0;
                                this.pBar11mentira.Properties.Maximum = 28;
                                // Deshabilidta Skin por default
                                this.pBar11mentira.LookAndFeel.UseDefaultLookAndFeel = false;
                                // Tipo de estilo
                                this.pBar11mentira.LookAndFeel.Style = LookAndFeelStyle.Flat;
                                // Colo de inicio y de fin
                                this.pBar11mentira.Properties.StartColor = System.Drawing.ColorTranslator.FromHtml("#72cfc8");
                                this.pBar11mentira.Properties.EndColor = System.Drawing.ColorTranslator.FromHtml("#72cfc8");
                                // Tipo de estilo de vista de la barra de progreso
                                this.pBar11mentira.Properties.ProgressViewStyle = ProgressViewStyle.Solid;
                                this.pBar11mentira.Properties.BorderStyle = BorderStyles.NoBorder;

                                if (factor.Value <= 2)
                                {
                                    if (factor.Value == 0)
                                        pBar11mentira.Position = 1;
                                    else
                                        pBar11mentira.Position = factor.Value;
                                    lbl11mentira.Text = "Sin Riesgo";
                                    lbl11mentira.ForeColor = System.Drawing.ColorTranslator.FromHtml("#00a651");
                                }
                                else if (factor.Value >= 3 && factor.Value <= 8)
                                {
                                    pBar11mentira.Position = factor.Value;
                                    lbl11mentira.Text = "Riesgo Medio";
                                    lbl11mentira.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffcb08");
                                }
                                else if (factor.Value >= 9)
                                {
                                    pBar11mentira.Position = factor.Value;
                                    lbl11mentira.Text = "Riesgo Alto";
                                    lbl11mentira.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ed1c24");
                                }
                                break;
                            #endregion
                            #endregion
                        }

                    }

                }
                series7.LabelsVisibility = DefaultBoolean.False;
                series7.View.Color = System.Drawing.ColorTranslator.FromHtml("#ffcb08");
                xrChart7Bullying.Series.Add(series7);
            }
        }
    }
}
