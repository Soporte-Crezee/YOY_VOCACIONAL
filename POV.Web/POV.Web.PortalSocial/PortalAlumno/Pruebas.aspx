
<%@ Page MasterPageFile="~/Site.Master" Language="C#" AutoEventWireup="true" CodeBehind="Pruebas.aspx.cs" Inherits="POV.Web.PortalSocial.PortalAlumno.Pruebas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .panel-heading h1 {
            margin: 5px 0px 0px 0px;
            font-size: 30px !important;
        }

        .tBienvenida {
            font-family: Roboto-Condensed !important;
            background: #ff9e19 !important;
        }

        .classdisabled {
            opacity: 0.4;
            filter: alpha(opacity=40);
        }

        .classenabled {
            opacity: 1;
            filter: alpha(opacity=100);
        }
    </style>
</asp:Content>
<asp:Content ID="Conten2" ContentPlaceHolderID="page_content" runat="server">
    <div class="divPrincipal">
        <div class="bodyadaptable">
            <div class="form-group center-block container_busqueda_general ui-widget-content">
                <%-- Cabecera --%>
                <div id="infoAlumno">
                    <h1 class="tBienvenida">Pruebas
                    </h1>
                </div>
                <%-- Bateria Bullying --%>
                <div class="row">
                    <div class="col-md-12">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <div class="row">
                                    <div class="col-xs-1">
                                        <img alt="PruebaBullying" src="../Images/YOY_imgIconPruebas_Personalidad.png" class="" />
                                    </div>
                                    <h1 class="col-xs-10">Bateria de Bullying</h1>
                                </div>
                            </div>
                            <div class="panel-body">
                                <div class="col-md-12">
                                    <div class="col-md-8">
                                        <p>
                                            La bater&iacute;a de pruebas <b>Bullying 12</b>, tiene el objetivo de mapear de forma integral las diversas variables del acoso escolar que se 
                                    presenta en el centro educativo, con la finalidad de contar con informaci&oacute;n para prevenir, disminuir y tratar de manera oportuna 
                                    la violencia en la escuela. La violencia escolar es un fen&oacute;meno de origen multifactorial, por lo tanto, se requiere una bater&iacute;a 
                                    de instrumentos que identifique cada uno de los elementos de la violencia escolar e identifique la presencia de acoso y violencia 
                                    de manera precisa. La bater&iacute;a cuenta con 12 escalas que se aplican, de forma sugerida en 3 bloques de trabajo de 50 min cada uno. 
                                    Al finalizar cada una de las pruebas se genera el reporte final.
                                        </p>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="panel-body">
                                            <div class="col-md-12 text-center" style="margin: 0; padding: 0;">
                                                <asp:ImageButton ID="imgBtnReporteBullying" runat="server" ImageUrl="~/Images/YOY_btnImg_Reporte.png" ToolTip="Ver Reporte" OnClick="imgBtnReporteBullying_Click" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-4">
                                    <div class="panel-heading text-center" style="background-color: #fff !important; border-bottom: #fff solid 5px !important; color: #000 !important;">
                                        <h3>Bloque 1</h3>
                                    </div>
                                    <div class="panel-body" style="text-align: center;">
                                        <div class="btn-group">
                                            <!-- Autoconcepto -->
                                            <asp:ImageButton ID="imgBtnTomarPruebaAutoconcepto" runat="server" ImageUrl="~/Images/bullying/btnautoconcepto-on.png" ToolTip="Tomar Prueba" CssClass="img-responsive-bloque" OnClick="imgBtnTomarPruebaAutoconcepto_Click" />
                                            <!-- Actitudes -->
                                            <asp:ImageButton ID="imgBtnTomarPruebaActitudes" runat="server" ImageUrl="~/Images/bullying/btnactitudes-on.PNG" ToolTip="Tomar Prueba" CssClass="img-responsive-bloque" OnClick="imgBtnTomarPruebaActitudes_Click" />
                                            <!-- Empatia -->
                                            <asp:ImageButton ID="imgBtnTomarPruebaEmpatia" runat="server" ImageUrl="~/Images/bullying/btnempatia-on.PNG" ToolTip="Tomar Prueba" CssClass="img-responsive-bloque" OnClick="imgBtnTomarPruebaEmpatia_Click" />
                                            <!-- Humor -->
                                            <asp:ImageButton ID="imgBtnTomarPruebaHumor" runat="server" ImageUrl="~/Images/bullying/btnhumor-on.PNG" ToolTip="Tomar Prueba" CssClass="img-responsive-bloque" OnClick="imgBtnTomarPruebaHumor_Click" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="panel-heading text-center" style="background-color: #fff !important; border-bottom: #fff solid 5px !important; color: #000 !important;">
                                        <h3>Bloque 2</h3>
                                    </div>
                                    <div class="panel-body" style="text-align: center;">
                                        <div class="panel-body" style="padding: 0px !important; margin: 0px !important;">
                                            <div class="btn-group">
                                                <!-- Victimizacion -->
                                                <asp:ImageButton ID="imgBtnTomarPruebaVictimizacion" runat="server" ImageUrl="~/Images/bullying/btnvictimizacion-on.PNG" ToolTip="Tomar Prueba" CssClass="img-responsive-bloque" OnClick="imgBtnTomarPruebaVictimizacion_Click" />

                                                <!-- Ciberbullying -->
                                                <asp:ImageButton ID="imgBtnTomarPruebaCiberbullying" runat="server" ImageUrl="~/Images/bullying/btnciberbullying-on.PNG" ToolTip="Tomar Prueba" CssClass="img-responsive-bloque" OnClick="imgBtnTomarPruebaCiberbullying_Click" />

                                                <!-- Bullying -->
                                                <asp:ImageButton ID="imgBtnTomarPruebaBullying" runat="server" ImageUrl="~/Images/bullying/btnbullying-on.png" ToolTip="Tomar Prueba" CssClass="img-responsive-bloque" OnClick="imgBtnTomarPruebaBullying_Click" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-5">
                                    <div class="panel-heading text-center" style="background-color: #fff !important; border-bottom: #fff solid 5px !important; color: #000 !important;">
                                        <h3>Bloque 3</h3>
                                    </div>
                                    <div class="panel-body" style="text-align: center;">
                                        <div class="col-md-1"></div>
                                        <div class="btn-group">
                                            <!-- Violencia -->
                                            <asp:ImageButton ID="imgBtnTomarPruebaViolencia" runat="server" ImageUrl="~/Images/bullying/btnviolencia-on.PNG" ToolTip="Tomar Prueba" CssClass="img-responsive-bloque" OnClick="imgBtnTomarPruebaViolencia_Click" />

                                            <!-- Comunicacion -->
                                            <asp:ImageButton ID="imgBtnTomarPruebaComunicacion" runat="server" ImageUrl="~/Images/bullying/btncomunicacion-on.PNG" ToolTip="Tomar Prueba" CssClass="img-responsive-bloque" OnClick="imgBtnTomarPruebaComunicacion_Click" />

                                            <!-- Imagen -->
                                            <asp:ImageButton ID="imgBtnTomarPruebaImagen" runat="server" ImageUrl="~/Images/bullying/btnimagen-on.PNG" ToolTip="Tomar Prueba" CssClass="img-responsive-bloque" OnClick="imgBtnTomarPruebaImagen_Click" />

                                            <!-- Ansiedad -->
                                            <asp:ImageButton ID="imgBtnTomarPruebaAnsiedad" runat="server" ImageUrl="~/Images/bullying/btnansiedad-on.PNG" ToolTip="Tomar Prueba" CssClass="img-responsive-bloque" OnClick="imgBtnTomarPruebaAnsiedad_Click" />

                                            <!-- Depresion -->
                                            <asp:ImageButton ID="imgBtnTomarPruebaDepresion" runat="server" ImageUrl="~/Images/bullying/btndepresion-on.PNG" ToolTip="Tomar Prueba" CssClass="img-responsive-bloque" OnClick="imgBtnTomarPruebaDepresion_Click" />
                                        </div>
                                    </div>
                                    <div class="col-md-1"></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <%-- Habitos --%>
                    <div class="col-md-6">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <div class="row">
                                    <div class="col-xs-2">
                                        <img alt="PruebaHabitos" src="../Images/YOY_imgIcon_HabitosEstudio.png" class="" />
                                    </div>
                                    <h1 class="col-xs-10">H&aacute;bitos de estudio</h1>
                                </div>
                            </div>
                            <div class="panel-body">
                                <p>
                                    El cuestionario de <b>Hábitos de estudio</b> pretende ser una herramienta diagnóstica de los posibles problemas en cuanto al rendimiento y las 
                                    actitudes frente al estudio (aprender a aprender). Tiene como objetivo identificar el nivel de hábitos de estudio que tienes, de tal forma 
                                    que se puedan identificar las áreas de oportunidad y mejora del estudiante. Contar con buenos hábitos de estudio posibilita que una vez que 
                                    el alumno ya no cuente con la guía permanente de sus docentes, pueda transformarse en alguien capaz de estudiar por sí solo. La prueba consta de 
                                    50 reactivos, no es una prueba de tiempo, aunque se pide que la realices de forma rápida, entre 20 y 45 minutos.
                                </p>
                                <div class="col-md-12">
                                    <div class="col-md-4"></div>
                                    <div class="col-md-4" style="text-align: center">
                                        <asp:ImageButton ID="imgBtnTomarPruebaHabitos" runat="server" ImageUrl="~/Images/YOY_btnImg_iniciarPrueba.png" ToolTip="Tomar Prueba" OnClick="imgBtnTomarPruebaHabitos_Click" />
                                        <asp:ImageButton ID="imgBtnContinuarHabitos" runat="server" ImageUrl="~/Images/YOY_btnImg_continuarPrueba.png" ToolTip="Continuar" Visible="false" OnClick="imgBtnContinuarHabitos_Click" />
                                        <asp:ImageButton ID="imgBtnReporteHabitos" runat="server" ImageUrl="~/Images/YOY_btnImg_Reporte.png" ToolTip="Ver Reporte" Visible="false" OnClick="imgBtnReporteHabitos_Click" />
                                    </div>
                                    <div class="col-md-4"></div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <%-- Inteligencia - Dominos --%>
                    <div class="col-md-6">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <div class="row">
                                    <div class="col-xs-2">
                                        <img alt="PruebaDominos" src="../Images/YOY_imgIconPruebas_Inteligencia.png" class="" />
                                    </div>
                                    <h1 class="col-xs-10">Inteligencia - Dominós</h1>
                                </div>
                            </div>
                            <div class="panel-body">
                                <p>
                                    <b>Dominós</b> es una prueba no verbal de inteligencia destinada a valorar la capacidad de una persona para conceptualizar y aplicar el 
                                    razonamiento lógico a nuevos problemas. Esta prueba mide el factor “G” de la inteligencia o factor general de inteligencia, que es el 
                                    fundamento esencial del comportamiento inteligente en cualquier situación. Esta prueba es también una herramienta diagnóstica de los posibles 
                                    problemas relacionados con el rendimiento académico y las actitudes frente al estudio (aprender a aprender). La prueba consta de 48 ejercicios 
                                    en orden de dificultad creciente, y no tiene límite de tiempo, aunque se estima una duración de entre 30 y 45 minutos para completarla.
                                </p>
                                <div class="col-md-12">
                                    <div class="col-md-4"></div>
                                    <div class="col-md-4" style="text-align: center">
                                        <asp:ImageButton ID="imgBtnTomarPruebaDominos" runat="server" ImageUrl="~/Images/YOY_btnImg_iniciarPrueba.png" ToolTip="Tomar Prueba" OnClick="imgBtnTomarPruebaDominos_Click" />
                                        <asp:ImageButton ID="imgBtnContinuarDominos" runat="server" ImageUrl="~/Images/YOY_btnImg_continuarPrueba.png" ToolTip="Continuar" Visible="false" OnClick="imgBtnContinuarDominos_Click" />
                                        <asp:ImageButton ID="imgBtnReporteDominos" runat="server" ImageUrl="~/Images/YOY_btnImg_Reporte.png" ToolTip="Ver Reporte" Visible="false" OnClick="imgBtnReporteDominos_Click" />
                                    </div>
                                    <div class="col-md-4"></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <%-- Terman --%>
                    <div class="col-md-6">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <div class="row">
                                    <div class="col-xs-2">
                                        <img alt="PruebaTerman" src="../Images/YOY_imgIconPruebas_Inteligencia.png" class="" />
                                    </div>
                                    <h1 class="col-xs-10">Inteligencia - Terman Merrill</h1>
                                </div>
                            </div>
                            <div class="panel-body">
                                <p>
                                    La prueba que se presenta a continuación se llama <b>Terman Merrill</b> y su objetivo es determinar el cociente intelectual, factor general,
                                     mediante la evaluación de la habilidad mental y conocimientos. La escala de Terman Merrill evalúa la inteligencia a través de seis áreas: 
                                    inteligencia general, conocimiento, razonamiento fluido, razonamiento cuantitativo, proceso visual-espacial y memoria de trabajo. Estas 
                                    áreas son cubiertas por 10 partes o subpruebas, con un promedio de 17 reactivos por cada subprueba. Las tareas de cada subprueba son de 
                                    habilidad matemática y lingüística, con un tiempo de duración total de 30 minutos; es decir, esta es una prueba de tiempo. El coeficiente 
                                    de confiabilidad para las edades entre 14 y 18 años es de .95.
                                </p>
                                <div class="col-md-12">
                                    <div class="col-md-4"></div>
                                    <div class="col-md-4" style="text-align: center">
                                        <asp:ImageButton ID="imgBtnTomarPruebaTerman" runat="server" ImageUrl="~/Images/YOY_btnImg_iniciarPrueba.png" ToolTip="Tomar Prueba" OnClick="imgBtnTomarPruebaTerman_Click" />
                                        <asp:ImageButton ID="imgBtnContinuarTerman" runat="server" ImageUrl="~/Images/YOY_btnImg_continuarPrueba.png" ToolTip="Ver Reporte" Visible="false" OnClick="imgBtnContinuarTerman_Click" />
                                        <asp:ImageButton ID="imgBtnReporteTerman" runat="server" ImageUrl="~/Images/YOY_btnImg_Reporte.png" ToolTip="Ver Reporte" Visible="false" OnClick="imgBtnReporteTerman_Click" />
                                    </div>
                                    <div class="col-md-4"></div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <%-- Raven --%>
                    <div class="col-md-6">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <div class="row">
                                    <div class="col-xs-2">
                                        <img alt="PruebaRaven" src="../Images/YOY_imgIconPruebas_Inteligencia.png" class="" />
                                    </div>
                                    <h1 class="col-xs-10">Inteligencia - Raven</h1>
                                </div>
                            </div>
                            <div class="panel-body">
                                <p>
                                    El test de <b>Raven</b> se utiliza para medir la capacidad intelectual, comparando formas y razonando por analogías, independientemente de los conocimientos adquiridos, 
                                    por lo que brinda información sobre la capacidad y claridad de pensamiento del examinado para la actividad intelectual. Esta prueba obliga a poner en marcha el razonamiento 
                                    analógico, la percepción y la capacidad de abstracción. Consiste en encontrar la pieza faltante en una serie de figuras que se irán mostrando.
                                    Se debe analizar la serie que se le presenta y siguiendo la secuencia horizontal y vertical, escoger uno de las piezas sugeridas  la que encaje perfectamente en ambos sentidos,
                                    tanto en el horizontal como en el vertical. No suele utilizarse límite de tiempo, pero se pide que la realices de forma rapidae entre 40 y 60 minutos
                                </p>
                                <div class="col-md-12">
                                    <div class="col-md-4"></div>
                                    <div class="col-md-4" style="text-align: center">
                                        <asp:ImageButton ID="imgBtnTomarPruebaRaven" runat="server" ImageUrl="~/Images/YOY_btnImg_iniciarPrueba.png" ToolTip="Tomar Prueba" OnClick="imgBtnTomarPruebaRaven_Click" />
                                        <asp:ImageButton ID="imgBtnContinuarRaven" runat="server" ImageUrl="~/Images/YOY_btnImg_continuarPrueba.png" ToolTip="Continuar" Visible="false" OnClick="imgBtnContinuarRaven_Click" />
                                        <asp:ImageButton ID="imgBtnReporteRaven" runat="server" ImageUrl="~/Images/YOY_btnImg_Reporte.png" ToolTip="Ver Reporte" Visible="false" OnClick="imgBtnReporteRaven_Click" />
                                    </div>
                                    <div class="col-md-4"></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <%-- Personalidad - Sacks --%>
                    <div class="col-md-6">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <div class="row">
                                    <div class="col-xs-2">
                                        <img alt="PruebaSacks" src="../Images/YOY_imgIconPruebas_Personalidad.png" class="" />
                                    </div>
                                    <h1 class="col-xs-10">Personalidad -Sacks</h1>
                                </div>
                            </div>
                            <div class="panel-body">
                                <p>
                                    La prueba de <b>Frases incompletas de Sacks</b> tiene como objetivo identificar aspectos y rasgos de personalidad mediante la asociación de 
                                    palabras. Los estímulos del instrumento son estandarizados, en tanto que a todos los sujetos se les presentan los mismos troncos 
                                    verbales para completar. En esta prueba, el entrevistado proyecta sus ideas, valores, creencias, anhelos, fantasías, temores, etc., 
                                    por lo cual se la considera una técnica proyectiva verbal, que debe ser analizada e interpretada sólo por un profesional de la orientación 
                                    y consejería psicológica. La prueba consta de 60 frases incompletas, y aunque no es una prueba de tiempo, se pide que la realices de forma 
                                    rápida, entre 20 y 45 minutos.
                                </p>
                                <div class="col-md-12">
                                    <div class="col-md-4"></div>
                                    <div class="col-md-4" style="text-align: center">
                                        <asp:ImageButton ID="imgBtnTomarPruebaSacks" runat="server" ImageUrl="~/Images/YOY_btnImg_iniciarPrueba.png" ToolTip="Tomar Prueba" OnClick="imgBtnTomarPruebaSacks_Click" />
                                        <asp:ImageButton ID="imgBtnContinuarSacks" runat="server" ImageUrl="~/Images/YOY_btnImg_continuarPrueba.png" ToolTip="Continuar" Visible="false" OnClick="imgBtnContinuarSacks_Click" />
                                        <asp:ImageButton ID="imgBtnReporteSacks" runat="server" ImageUrl="~/Images/YOY_btnImg_Reporte.png" ToolTip="Ver Reporte" Visible="false" OnClick="imgBtnReporteSacks_Click" />
                                    </div>
                                    <div class="col-md-4"></div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <%-- Cleaver --%>
                    <div class="col-md-6">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <div class="row">
                                    <div class="col-xs-2">
                                        <img alt="PruebaCleaver" src="../Images/YOY_imgIconPruebas_Personalidad.png" class="" />
                                    </div>
                                    <h1 class="col-xs-10">Personalidad - Cleaver</h1>
                                </div>
                            </div>
                            <div class="panel-body">
                                <p>
                                    La prueba <b>Cleaver</b> proporciona una descripción completa de la personalidad del individuo, haciendo énfasis en sus aptitudes para realizar 
                                    diferentes labores sociales, su capacidad para desenvolverse con otras personas y relacionarse con ellas. Realiza un pronóstico de la forma en 
                                    que este individuo reacciona ante determinadas circunstancias y también de sus reacciones y actitudes típicas bajo situaciones de presión. Para 
                                    realizar la valoración, se basa en cuatro escalas que se calculan partiendo de la autodescripción de la persona: Empuje, Influencia social, 
                                    Constancia y Valores. La prueba consta de 96 reactivos, no es una prueba de tiempo, aunque se pide que la realices de forma rápida, entre 
                                    20 y 45 minutos.
                                </p>
                                <div class="col-md-12">
                                    <div class="col-md-4"></div>
                                    <div class="col-md-4" style="text-align: center">
                                        <asp:ImageButton ID="imgBtnTomarPruebaCleaver" runat="server" ImageUrl="~/Images/YOY_btnImg_iniciarPrueba.png" ToolTip="Tomar Prueba" OnClick="imgBtnTomarPruebaCleaver_Click" />
                                        <asp:ImageButton ID="imgBtnContinuarCleaver" runat="server" ImageUrl="~/Images/YOY_btnImg_continuarPrueba.png" ToolTip="Continuar" Visible="false" OnClick="imgBtnContinuarCleaver_Click" />
                                        <asp:ImageButton ID="imgBtnReporteCleaver" runat="server" ImageUrl="~/Images/YOY_btnImg_Reporte.png" ToolTip="Ver Reporte" Visible="false" OnClick="imgBtnReporteCleaver_Click" />
                                    </div>
                                    <div class="col-md-4"></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <%-- Vocacional - Chaside --%>
                    <div class="col-md-6">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <div class="row">
                                    <div class="col-xs-2">
                                        <img alt="PruebaChaside" src="../Images/YOY_imgIconPruebas_Vocacionales.png" class="" />
                                    </div>
                                    <h1 class="col-xs-10">Vocacional - Chaside</h1>
                                </div>
                            </div>
                            <div class="panel-body">
                                <p>
                                    La prueba <b>CHASIDE</b> es una herramienta confiable para identificar las áreas de conocimiento que se encuentran más fortalecidas, 
                                    ya sea por tener mayor aptitud para desarrollarla o por ser de mayor interés para la persona. Los intereses deben entenderse como 
                                    manifestaciones de los gustos que tenemos hacia ciertas actividades o situaciones, por otra parte, las aptitudes son habilidades  
                                    naturales que una persona posee para efectuar actividades con un buen nivel de desempeño. Esta herramienta ayuda a 
                                    diferenciar cual sería la decisión vocacional más acertada, partiendo de sus intereses y preferencias. La prueba consta de 98 reactivos, 
                                    no es una prueba de tiempo, aunque se pide que la realices de forma rápida, entre 20 y 45 minutos.
                                </p>
                                <div class="col-md-12">
                                    <div class="col-md-4"></div>
                                    <div class="col-md-4" style="text-align: center">
                                        <asp:ImageButton ID="imgBtnTomarPruebaChaside" runat="server" ImageUrl="~/Images/YOY_btnImg_iniciarPrueba.png" ToolTip="Tomar Prueba" OnClick="imgBtnTomarPruebaChaside_Click" />
                                        <asp:ImageButton ID="imgBtnContinuarChaside" runat="server" ImageUrl="~/Images/YOY_btnImg_continuarPrueba.png" ToolTip="Continuar" Visible="false" OnClick="imgBtnContinuarChaside_Click" />
                                        <asp:ImageButton ID="imgBtnReporteChaside" runat="server" ImageUrl="~/Images/YOY_btnImg_Reporte.png" ToolTip="Ver Reporte" Visible="false" OnClick="imgBtnReporteChaside_Click" />
                                    </div>
                                    <div class="col-md-4"></div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <%-- Kuder --%>
                    <div class="col-md-6">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <div class="row">
                                    <div class="col-xs-2">
                                        <img alt="PruebaKuder" src="../Images/YOY_imgIconPruebas_Vocacionales.png" class="" />
                                    </div>
                                    <h1 class="col-xs-10">Vocacional - Kuder</h1>
                                </div>
                            </div>
                            <div class="panel-body">
                                <p>
                                    La prueba <b>Kuder</b> permite conocer en qué áreas ocupacionales se manifiestan los intereses y preferencias del examinado y los tipos de 
                                    actividades a los cuales, probablemente, le gustaría vincularse. El instrumento es útil para que los estudiantes puedan realizar un estudio 
                                    organizado de las ocupaciones, seleccionar una carrera y orientar las actividades formativas y vocacionales para encontrar mayores satisfacciones 
                                    actuales y futuras. Esta prueba no tiene tiempo límite, aunque la mayoría de las personas la contesta entre 30 y 55 minutos. La prueba consta de 
                                    504 reactivos y puede ser respondida en dos o más sesiones de trabajo; la recomendación son dos sesiones como máximo.
                                </p>
                                <div class="col-md-12">
                                    <div class="col-md-4"></div>
                                    <div class="col-md-4" style="text-align: center">
                                        <asp:ImageButton ID="imgBtnTomarPruebaKuder" runat="server" ImageUrl="~/Images/YOY_btnImg_iniciarPrueba.png" ToolTip="Tomar Prueba" OnClick="imgBtnTomarPruebaKuder_Click" />
                                        <asp:ImageButton ID="imgBtnContinuarKuder" runat="server" ImageUrl="~/Images/YOY_btnImg_continuarPrueba.png" ToolTip="Continuar" Visible="false" OnClick="imgBtnContinuarKuder_Click" />
                                        <asp:ImageButton ID="imgBtnReporteKuder" runat="server" ImageUrl="~/Images/YOY_btnImg_Reporte.png" ToolTip="Ver Reporte" Visible="false" OnClick="imgBtnReporteKuder_Click" />
                                    </div>
                                    <div class="col-md-4"></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <%-- Frases Incompletas Vocacionales --%>
                    <div class="col-md-6">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <div class="row">
                                    <div class="col-xs-2">
                                        <img alt="PruebaFrasesVocacionales" src="../Images/YOY_imgIconPruebas_Vocacionales.png" class="" />
                                    </div>
                                    <h1 class="col-xs-10">Vocacional - Frases Incompletas</h1>
                                </div>
                            </div>
                            <div class="panel-body">
                                <p>
                                    La prueba de <b>Frases incompletas de Sacks</b> tiene como objetivo identificar aspectos y rasgos de personalidad mediante la asociación de 
                                    palabras. Los estímulos del instrumento son estandarizados, en tanto que a todos los sujetos se les presentan los mismos troncos 
                                    verbales para completar. En esta prueba, el entrevistado proyecta sus ideas, valores, creencias, anhelos, fantasías, temores, etc., 
                                    por lo cual se la considera una técnica proyectiva verbal, que debe ser analizada e interpretada sólo por un profesional de la orientación 
                                    y consejería psicológica. La prueba consta de 60 frases incompletas, y aunque no es una prueba de tiempo, se pide que la realices de forma 
                                    rápida, entre 20 y 45 minutos.
                                </p>
                                <div class="col-md-12">
                                    <div class="col-md-4"></div>
                                    <div class="col-md-4" style="text-align: center">
                                        <asp:ImageButton ID="imgBtnTomarPruebaFrasesVocacionales" runat="server" ImageUrl="~/Images/YOY_btnImg_iniciarPrueba.png" ToolTip="Tomar Prueba" OnClick="imgBtnTomarPruebaFrasesVocacionales_Click" />
                                        <asp:ImageButton ID="imgBtnContinuarPruebaFrasesVocacionales" runat="server" ImageUrl="~/Images/YOY_btnImg_continuarPrueba.png" ToolTip="Continuar" Visible="false" OnClick="imgBtnContinuarPruebaFrasesVocacionales_Click" />
                                        <asp:ImageButton ID="imgBtnReportePruebaFrasesVocacionales" runat="server" ImageUrl="~/Images/YOY_btnImg_Reporte.png" ToolTip="Ver Reporte" Visible="false" OnClick="imgBtnReportePruebaFrasesVocacionales_Click" />
                                    </div>
                                    <div class="col-md-4"></div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <%-- Interes - Allport --%>
                    <div class="col-md-6">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <div class="row">
                                    <div class="col-xs-2">
                                        <img alt="PruebaAllport" src="../Images/YOY_imgIconPruebas_Valores.png" class="" />
                                    </div>
                                    <h1 class="col-xs-10">Intereses - Allport</h1>
                                </div>
                            </div>
                            <div class="panel-body">
                                <p>
                                    El Estudio de Valores de <b>Allport</b> tiene como objetivo medir la importancia relativa de seis intereses o motivos básicos en la personalidad 
                                    y la adaptación al medio social, mediante la identificación de los principios morales e ideológicos que condicionan el comportamiento humano, 
                                    llamados valores. Los valores son los siguientes: Teórico, Económico, Estético, Social, Político y Religioso. Aquellos motivos válidos para la 
                                    propia conciencia inciden en la vida diaria de los individuos y en sus diversos estilos de relacionarse con su entorno. La prueba consta de 45 
                                    reactivos en dos partes, y no es una prueba de tiempo, aunque se pide que la realices de forma rápida entre 20 y 45 minutos.
                                </p>
                                <div class="col-md-12">
                                    <div class="col-md-4"></div>
                                    <div class="col-md-4" style="text-align: center">
                                        <asp:ImageButton ID="imgBtnTomarPruebaAllport" runat="server" ImageUrl="~/Images/YOY_btnImg_iniciarPrueba.png" ToolTip="Tomar Prueba" OnClick="imgBtnTomarPruebaAllport_Click" />
                                        <asp:ImageButton ID="imgBtnContinuarAllport" runat="server" ImageUrl="~/Images/YOY_btnImg_continuarPrueba.png" ToolTip="Continuar" Visible="false" OnClick="imgBtnContinuarAllport_Click" />
                                        <asp:ImageButton ID="imgBtnReporteAllport" runat="server" ImageUrl="~/Images/YOY_btnImg_Reporte.png" ToolTip="Ver Reporte" Visible="false" OnClick="imgBtnReporteAllport_Click" />
                                    </div>
                                    <div class="col-md-4"></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <%-- Zavic --%>
                    <div class="col-md-6">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <div class="row">
                                    <div class="col-xs-2">
                                        <img alt="PruebaZavic" src="../Images/YOY_imgIconPruebas_Valores.png" class="" />
                                    </div>
                                    <h1 class="col-xs-10">Intereses - Zavic</h1>
                                </div>
                            </div>
                            <div class="panel-body">
                                <p>
                                    La conocida prueba <b>Zavic</b> es la mejor para poder de pronto sacar un perfil de candidatos para un puesto de trabajo, los empleados no 
                                    siempre est&aacute;n contentos de pasar por este test, ya que pone al descubierto no solamente sus valores, sino que tambi&eacute;n sus intereses, 
                                    entonces es muy intenso, sin embargo la duraci&oacute;n que tiene este test en particular es de 20 minutos solamente y solamente consta de 20 
                                    preguntas diferentes que tienen una modalidad de respuesta en donde aparecer 4 variantes y quien est&eacute; realizando la prueba elegir&aacute; 
                                    solamente una de ellas.
                                </p>
                                <div class="col-md-12">
                                    <div class="col-md-4"></div>
                                    <div class="col-md-4" style="text-align: center">
                                        <asp:ImageButton ID="imgBtnTomarPruebaZavic" runat="server" ImageUrl="~/Images/YOY_btnImg_iniciarPrueba.png" ToolTip="Tomar Prueba" OnClick="imgBtnTomarPruebaZavic_Click" />
                                        <asp:ImageButton ID="imgBtnContinuarZavic" runat="server" ImageUrl="~/Images/YOY_btnImg_continuarPrueba.png" ToolTip="Continuar" Visible="false" OnClick="imgBtnContinuarZavic_Click" />
                                        <asp:ImageButton ID="imgBtnReporteZavic" runat="server" ImageUrl="~/Images/YOY_btnImg_Reporte.png" ToolTip="Ver Reporte" Visible="false" OnClick="imgBtnReporteZavic_Click" />
                                    </div>
                                    <div class="col-md-4"></div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <%-- Motivacion - Rotter --%>
                    <div class="col-md-6">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <div class="row">
                                    <div class="col-xs-2">
                                        <img alt="PruebaRotter" src="../Images/YOY_imgIconPruebas_Personalidad.png" class="" />
                                    </div>
                                    <h1 class="col-xs-10">Motivaci&oacute;n - Rotter</h1>
                                </div>
                            </div>
                            <div class="panel-body">
                                <p>
                                    El test de Locus de control <b>Rotter</b> es ampliamente utilizado en la psicolog&iacute;a en el campo de la autorregulaci&oacute;n para 
                                    identificar de d&oacute;nde surge la motivaci&oacute;n de la persona para alcanzar sus objetivos y cubrir sus necesidades personales, es decir, 
                                    si la persona percibe que un acontecimiento es causado por su conducta o no lo es. Se trata de una escala bidimensional de internalidad-externalidad
                                    de locus de control, en la que cada &iacute;tem consta de dos enunciados generales sobre las causas gen&eacute;ricas del &eacute;xito y el fracaso
                                    en la vida en general. La prueba consta de 29 reactivos, y no es una prueba de tiempo, aunque se pide que la realices de manera &aacute;gil. El 
                                    tiempo de respuesta est&aacute; entre 15 y 30 minutos.
                                </p>
                                <div class="col-md-12">
                                    <div class="col-md-4"></div>
                                    <div class="col-md-4" style="text-align: center">
                                        <asp:ImageButton ID="imgBtnTomarPruebaRotter" runat="server" ImageUrl="~/Images/YOY_btnImg_iniciarPrueba.png" ToolTip="Tomar Prueba" OnClick="imgBtnTomarPruebaRotter_Click" />
                                        <asp:ImageButton ID="imgBtnContinuarRotter" runat="server" ImageUrl="~/Images/YOY_btnImg_continuarPrueba.png" ToolTip="Continuar" Visible="false" OnClick="imgBtnContinuarPruebaRotter_Click" />
                                        <asp:ImageButton ID="imgBtnReporteRotter" runat="server" ImageUrl="~/Images/YOY_btnImg_Reporte.png" ToolTip="Ver Reporte" Visible="false" OnClick="imgBtnReporteRotter_Click" />
                                    </div>
                                    <div class="col-md-4"></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                        <div class="row">
                    <%--Estilos de Aprendizaje --%>
                    <div class="col-md-6">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <div class="row">
                                    <div class="col-xs-2">
                                        <img alt="PruebaEstilosDeAprendizaje" src="../Images/YOY_imgIcon_HabitosEstudio.png" class="" />
                                    </div>
                                    <h1 class="col-xs-10">Estilos de Aprendizaje</h1>
                                </div>
                            </div>
                            <div class="panel-body">
                                <p>
                                   Estilos de Aprendizaje <br />
                                    Es una prueba para identificar el estilo de aprendizaje predominante de una                                   
                                    persona. Los estilo de aprendizaje son características personales con las que se nace 
                                    y que se desarrollan conforme la persona va creciendo. Tenemos tres grandes 
                                    estilos: visual, el auditivo y el kinestésico. Son los rasgos cognitivos, afectivos y
                                    fisiológicos que sirven como indicadores estables de cómo los alumnos perciben 
                                    interacciones y responden a sus ambientes de aprendizaje, cómo estructuran los 
                                    contenidos, forman conceptos y resuelven los problemas (visual, auditivo, 
                                    kinestésico). La prueba es de opción múltiple y consta de 40 preguntas, con un 
                                    tiempo de respuesta de 20 min aproximadamente.

                                </p>
                                <div class="col-md-12">
                                    <div class="col-md-4"></div>
                                    <div class="col-md-4" style="text-align: center">
                                        <asp:ImageButton ID="ImageButtonTomarPruebaEstilos" runat="server" ImageUrl="~/Images/YOY_btnImg_iniciarPrueba.png" ToolTip="Tomar Prueba" OnClick="ImageButtonTomarPruebaEstilos_Click"  />
                                        <asp:ImageButton ID="ImageButtoncontinuarPruebaEstilos" runat="server" ImageUrl="~/Images/YOY_btnImg_continuarPrueba.png" ToolTip="Continuar" Visible="false" OnClick="ImageButtoncontinuarPruebaEstilos_Click"  />
                                        <asp:ImageButton ID="ImageButtonVerReporteEstilos" runat="server" ImageUrl="~/Images/YOY_btnImg_Reporte.png" ToolTip="Ver Reporte" Visible="false" OnClick="ImageButtonVerReporteEstilos_Click"  />
                                    </div>
                                    <div class="col-md-4"></div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <%-- Inteligencias Multiples --%>
                    <div class="col-md-6">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <div class="row">
                                    <div class="col-xs-2">
                                        <img alt="PruebaInsteligenciasMultiples" src="../Images/YOY_imgIconPruebas_Inteligencia.png" class="" />
                                    </div>
                                    <h1 class="col-xs-10">Inteligencias Multiples</h1>
                                </div>
                            </div>
                            <div class="panel-body">
                                <p>
                                 Test de Inteligencias Múltiples <br />
                                    Este test ayuda a tener un conocimiento más profundo de las habilidades y competencias que tiene cada persona,
                                    a través de la teoría de Howard Gardner que plantea que la inteligencia no es una sola unidad que agrupa diferentes
                                    capacidades específicas, sino una red de factores biológicos, de la vida personal y culturales e históricos, conocidos como
                                     inteligencias múltiples. Estas inteligencias se clasifican de la siguiente manera:
                                     A) Int. Verbal B) Int. Lógico-matemática C) Int. Visual espacial D) Int. kinestésica-corporal E) Int. Musical-rítmica F) Int. Intrapersonal G) Int Interpersonal. 
                                    La prueba consta de 35 preguntas en un formato de respuesta de verdadero y falso, con un tiempo de respuesta de aproximado de 30 min.

                                <div class="col-md-12">
                                    <div class="col-md-4"></div>
                                    <div class="col-md-4" style="text-align: center">
                                        <asp:ImageButton ID="ImageButtonTomarPruebaIntMul" runat="server" ImageUrl="~/Images/YOY_btnImg_iniciarPrueba.png" ToolTip="Tomar Prueba" OnClick="ImageButtonTomarPruebaIntMul_Click"  />
                                        <asp:ImageButton ID="ImageButtonContinuarPruebaIntMul" runat="server" ImageUrl="~/Images/YOY_btnImg_continuarPrueba.png" ToolTip="Continuar" Visible="false" OnClick="ImageButtonContinuarPruebaIntMul_Click" />
                                        <asp:ImageButton ID="ImageButtonVerReporteIntMul" runat="server" ImageUrl="~/Images/YOY_btnImg_Reporte.png" ToolTip="Ver Reporte" Visible="false" OnClick="ImageButtonVerReporteIntMul_Click"  />
                                    </div>
                                    <div class="col-md-4"></div>
                                </div>
                            </div>
                        </div>
                    </div>


                </div>
                     <div class="row">
                    <%--Inventario de Intereses --%>
                    <div class="col-md-6">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <div class="row">
                                    <div class="col-xs-2">
                                        <img alt="PruebaInventariodeIntereses" src="../Images/YOY_imgIconPruebas_Valores.png" class="" />
                                    </div>
                                    <h1 class="col-xs-10">Inventario de Intereses</h1>
                                </div>
                            </div>
                            <div class="panel-body">
                                <p>
                   
                   El inventario de Intereses Profesionales de Hereford <br />
                   El inventario de Intereses Profesionales de Hereford (IIPH) es un test de tipo vocacional que pretende descubrir
                   áreas de interés en las cuales los jóvenes demuestran cierto tipo de gusto o motivación y que, con base en ésta información,
                   permite hacer ciertas predicciones acerca de la actividad futura para la cual el joven parece ser más afín. El IIPH ubica al 
                   joven en alguna de las 9 áreas siguientes: cálculo, científico-físico, científico biológico, mecánico, servicio social, literario, persuasivo, artístico y musical.
                   Cada ítem contiene una frase referente a alguna clase de actividad, y el joven debe calificar estas afirmaciones usando una escala de 1 a 5. 
                   La prueba consta de 90 preguntas, con duración aproximada de 50 min.


                                </p>
                                <div class="col-md-12">
                                    <div class="col-md-4"></div>
                                    <div class="col-md-4" style="text-align: center">
                                        <asp:ImageButton ID="imgBtnTomarPruebaInventariodeIntereses" runat="server" ImageUrl="~/Images/YOY_btnImg_iniciarPrueba.png" ToolTip="Tomar Prueba" OnClick="imgBtnTomarPruebaInventariodeIntereses_Click"  />
                                        <asp:ImageButton ID="imgBtnContinuarPruebaInventariodeIntereses" runat="server" ImageUrl="~/Images/YOY_btnImg_continuarPrueba.png" ToolTip="Continuar" Visible="false" OnClick="imgBtnContinuarPruebaInventariodeIntereses_Click"  />
                                        <asp:ImageButton ID="verRptPruebaInventariodeIntereses" runat="server" ImageUrl="~/Images/YOY_btnImg_Reporte.png" ToolTip="Ver Reporte" Visible="false" OnClick="verRptPruebaInventariodeIntereses_Click"  />
                                    </div>
                                    <div class="col-md-4"></div>
                                </div>
                            </div>
                        </div>
                    </div>

                  <%-- Inventario Herrera y Montes --%>
                         <div class="col-md-6">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <div class="row">
                                    <div class="col-xs-2">
                                        <img alt="PruebaInventarioHerreraMontes" src="../Images/YOY_imgIconPruebas_Vocacionales.png" class="" />
                                    </div>
                                    <h1 class="col-xs-10">Inventario de Herrera y Montes</h1>
                                </div>
                            </div>
                            <div class="panel-body">
                                <p>
                                    Inventario Herrera y Montes <br />
                                    El objetivos de esta prueba es identificar tus intereses y aptitudes del joven para luego elaborar un perfil vocacional.
                                    El presente cuestionario fue elaborado por el profesor Luis Herrera y Montes, y ayudará a definir e identificar tus intereses y aptitudes de manera más precisa. 
                                    Es necesario que contestes con sinceridad cada una de las preguntas que se hacen, con el fin de obtener mejores resultados. 
                                    Se recomienda que tus respuestas las marques en una sola sesión, es decir, desde el momento que inicies no deberás interrumpir 
                                    la actividad hasta finalizar, de lo contrario, el resultado tendrá variaciones y será menos preciso. La prueba consta de 60 preguntas, 
                                    con un tiempo de respuesta aproximado de 50 min.

                                </p>
                                
                       
                                <div class="col-md-12">
                                    <div class="col-md-4"></div>
                                    <div class="col-md-4" style="text-align: center">
                                        <asp:ImageButton ID="TomarPruebaInventrarioHerrera" runat="server" ImageUrl="~/Images/YOY_btnImg_iniciarPrueba.png" ToolTip="Tomar Prueba" OnClick="TomarPruebaInventarioHerrera_Click"  />
                                        <asp:ImageButton ID="ContinuarPruebaInventarioHerrera" runat="server" ImageUrl="~/Images/YOY_btnImg_continuarPrueba.png" ToolTip="Continuar" Visible="false" OnClick="ContinuarPruebaInventarioHerrera_Click"  />
                                        <asp:ImageButton ID="VerReporteInventarioHerrera" runat="server" ImageUrl="~/Images/YOY_btnImg_Reporte.png" ToolTip="Ver Reporte" Visible="false" OnClick="VerReportePruebaInventarioHerrera_Click"  />
                                    </div>
                                    <div class="col-md-4"></div>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>




                 <div class="row">
                    <%--Sucesos de Vida --%>
                    <div class="col-md-6">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <div class="row">
                                    <div class="col-xs-2">
                                        <img alt="PruebaInventariodeIntereses" src="../Images/YOY_imgIconPruebas_Valores.png" class="" />
                                    </div>
                                    <h1 class="col-xs-10">Sucesos de Vida</h1>
                                </div>
                            </div>
                            <div class="panel-body">
                    <p>Prueba Sucesos de Vida

                                </p>
                                <div class="col-md-12">
                                    <div class="col-md-4"></div>
                                    <div class="col-md-4" style="text-align: center">
                                        <asp:ImageButton ID="TomarPruebaSucesos" runat="server" ImageUrl="~/Images/YOY_btnImg_iniciarPrueba.png" ToolTip="Tomar Prueba" OnClick="TomarPruebaSucesos_Click"  />
                                        <asp:ImageButton ID="ContinuarPruebaSucesos" runat="server" ImageUrl="~/Images/YOY_btnImg_continuarPrueba.png" ToolTip="Continuar" Visible="false" OnClick="ContinuarPruebaSucesos_Click"  />
                                        <asp:ImageButton ID="VerReporteSucesos" runat="server" ImageUrl="~/Images/YOY_btnImg_Reporte.png" ToolTip="Ver Reporte" Visible="false" OnClick="VerReporteSucesos_Click"  />
                                    </div>
                                    <div class="col-md-4"></div>
                                </div>
                            </div>
                        </div>
                    </div>

                  <%-- Luscher --%>
                         <div class="col-md-6">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <div class="row">
                                    <div class="col-xs-2">
                                        <img alt="PruebaInventariodeIntereses" src="../Images/YOY_imgIconPruebas_Valores.png" class="" />
                                    </div>
                                    <h1 class="col-xs-10">Prueba de Colores de Luscher</h1>
                                </div>
                            </div>
                            <div class="panel-body">
                              Prueba de Colores de Luscher
                                <br/>
                                
                                <div class="col-md-12">
                                    <div class="col-md-4"></div>
                                    <div class="col-md-4" style="text-align: center">
                                        <asp:ImageButton ID="TomarPruebaLuscher" runat="server" ImageUrl="~/Images/YOY_btnImg_iniciarPrueba.png" ToolTip="Tomar Prueba" OnClick="TomarPruebaLuscher_Click"  />
                                        <asp:ImageButton ID="ContinuarPruebaLuscher" runat="server" ImageUrl="~/Images/YOY_btnImg_continuarPrueba.png" ToolTip="Continuar" Visible="false" OnClick="ContinuarPruebaLuscher_Click"  />
                                        <asp:ImageButton ID="VerReporteLuscher" runat="server" ImageUrl="~/Images/YOY_btnImg_Reporte.png" ToolTip="Ver Reporte" Visible="false" OnClick="VerReporteLuscher_Click"  />
                                    </div>
                                    <div class="col-md-4"></div>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>
</asp:Content>
