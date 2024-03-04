<%@ Page Title="" Language="C#" MasterPageFile="~/Content/MasterPages/Site.Master" AutoEventWireup="true"
    CodeBehind="RegistrarSumarioGeneralFrasesVocacionalesUI.aspx.cs" Inherits="POV.Web.HerramientasDocente.Pages.Pruebas.RegistrarSumarioGeneralFrasesVocacionalesUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(function () {
            $("#accordion").accordion({
                collapsible: true,
                active: false,
                heightStyle: "content"
            });

            $("#accordionInstruccion").accordion({
                collapsible: true,
                active: false,
                heightStyle: "content"
            });
        });
    </script>
    <style type="text/css">
        .list {
            text-align: left;
            margin-left: 25px;
        }

        #contenedorfinal {
            height: auto !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="bodyadaptable">
        <div class="col-xs-12">
            <br />
            <div class="titulo_principal_superior">
                <h1 style="margin-top: -13px;">
                    <asp:HyperLink ID="lnkBack" CssClass="first" runat="server">Volver</asp:HyperLink>/Registrar Sumario General
                </h1>
            </div>

            <div class="titulo_marco">
                <asp:Label runat="server" ID="Label1" Text="Sumario general "></asp:Label>
            </div>

            <div id="accordionInstruccion" class="col-xs-12">
                <h3>Consideraciones generales</h3>
                <ul>
                    <li>Los siguientes pasos se realizan para completar el reporte de resultados del estudiante, correspondiente a la prueba proyectiva de Frases 
                                    Incompletas Vocacionales.
                    </li>
                    <li>Al llenar cada apartado, el orientador debe considerar que esta información servirá en el proceso vocacional y que será visible tanto 
                                        para él, como para los estudiantes, padres de familia y directivos; por lo que las redacciones que realice deben contribuir a nutrir la 
                                        orientación vocacional exclusivamente.
                    </li>
                    <li>Tradicionalmente la prueba se califica mediante una escala de 4 puntos, que van de: 2-Disturbio serio, 1-Disturbo medio, 0-No se encuentra 
                                        disturbio significativo, X-Evidencias insuficientes. En el caso de nuestra versión, se realizará directamente una redacción sumatoria de 
                                        los que les sirva a todos los interesados del proceso vocacional del estudiante.
                    </li>
                    <li>Las redacciones deben permitir la relación de la descripción que realiza el orientador con alguna de las áreas del conocimiento, de modo que 
                                        muestre una pauta de personalidad y aptitud que oriente al estudiante hacia alguna profesión.
                    </li>
                    <li>Las redacciones se deberán limitar a describir la interrelación de las actitudes que manifiesta el estudiante y la estructura de su 
                                        personalidad, con la identificación de las áreas de interés vocacional y los indicadores de éxito que el estudiante puede tener en 
                                        determinado campo profesional.
                    </li>
                    <li>En caso de identificar algún área de conflicto clínico, deberá de ser notificado a la autoridad educativa pertinente y emitir una 
                                        recomendación para que sea canalizado de inmediato a terapia psicológica con algún profesional del área clínica.
                    </li>
                    <li>Otras consideraciones:
                                    <ul type="circle" class="list">
                                        <li>
                                            <p>
                                                Evite describir la ausencia de rasgos, en vez de eso escribe acerca de la presencia de ciertos rasgos. En caso de que no hayan evidencias
                                                 suficientes para afirmar algo, exprésalo de ese modo.
                                            </p>
                                        </li>
                                        <li>
                                            <p>
                                                Evite presentar términos que no entienda la mayoría, abstracciones, ambigüedades y tecnicismos.
                                            </p>
                                        </li>
                                        <li>
                                            <p>
                                                Evite información que no aporte directamente al proceso vocacional.
                                            </p>
                                        </li>
                                        <li>
                                            <p>
                                                Evite en sus descripciones las fuentes de información, es decir referir personas: “su madre, padre, hermano, profesor, etc.”
                                            </p>
                                        </li>
                                        <li>
                                            <p>
                                                Evite incluir información que sea nociva para el examinado; de ser necesario de comunicarla, deberá hacerse mediante el mecanismo mencionado
                                                anteriormente.
                                            </p>
                                        </li>
                                    </ul>
                    </li>
                </ul>
            </div>
            <div id="accordion" class="col-xs-12" runat="server">
                <h3>
                    <asp:Label runat="server" ID="lblSubtitulo1" Text="Respuestas del estudiante clasificadas"></asp:Label>
                </h3>
                <div>
                    <div class="row">
                        <h1>
                            I. Organizaci&oacute;n de personalidad y conducta de elecci&oacute;n.
                        </h1>
                        <asp:Repeater ID="rptrClasificadorOrganizacion" runat="server" OnItemDataBound="rptrClasificadorOrganizacion_ItemDataBound" OnDataBinding="rptrClasificadorOrganizacion_DataBinding">
                        <HeaderTemplate><%#Eval("Titulo") %></HeaderTemplate>
                        <ItemTemplate>
                            <h2 class="titulo_marco">
                                <%#Eval("Descripcion") %>
                            </h2>
                            <div>
                                <asp:Repeater ID="rptrReactivosOrganizacion" runat="server">
                                    <ItemTemplate>
                                        <div style="padding-left: 2em">
                                            <asp:Label ID="lblTextoPregunta" runat="server">
                                        <%#Eval("TextoPregunta") %> : 
                                            </asp:Label>
                                            <asp:Label ID="lblTextoRespuesta" runat="server" Font-Bold="true" Font-Underline="true">
                                        <%#Eval("TextoRespuesta") %>
                                            </asp:Label>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                    </div>
                    <div class="row">
                        <h1>
                            2. Perspectiva de las opciones profesionales y ocupacionales.
                        </h1>
                        <asp:Repeater ID="rptrClasificadorPerspectiva" runat="server" OnItemDataBound="rptrClasificadorPerspectiva_ItemDataBound" OnDataBinding="rptrClasificadorPerspectiva_DataBinding">
                        <HeaderTemplate><%#Eval("Titulo") %></HeaderTemplate>
                        <ItemTemplate>
                            <h2 class="titulo_marco">
                                <%#Eval("Descripcion") %>
                            </h2>
                            <div>
                                <asp:Repeater ID="rptrReactivosPerspectiva" runat="server">
                                    <ItemTemplate>
                                        <div style="padding-left: 2em">
                                            <asp:Label ID="lblTextoPregunta" runat="server">
                                        <%#Eval("TextoPregunta") %> : 
                                            </asp:Label>
                                            <asp:Label ID="lblTextoRespuesta" runat="server" Font-Bold="true" Font-Underline="true">
                                        <%#Eval("TextoRespuesta") %>
                                            </asp:Label>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                    </div>
                    <div class="row">
                        <h1>
                           3. Fuentes de conflicto para la elección.
                        </h1>
                        <asp:Repeater ID="rptrClasificadorFuentes" runat="server" OnItemDataBound="rptrClasificadorFuentes_ItemDataBound" OnDataBinding="rptrClasificadorFuentes_DataBinding">
                        <HeaderTemplate><%#Eval("Titulo") %></HeaderTemplate>
                        <ItemTemplate>
                            <h2 class="titulo_marco">
                                <%#Eval("Descripcion") %>
                            </h2>
                            <div>
                                <asp:Repeater ID="rptrReactivosFuentes" runat="server">
                                    <ItemTemplate>
                                        <div style="padding-left: 2em">
                                            <asp:Label ID="lblTextoPregunta" runat="server">
                                        <%#Eval("TextoPregunta") %> : 
                                            </asp:Label>
                                            <asp:Label ID="lblTextoRespuesta" runat="server" Font-Bold="true" Font-Underline="true">
                                        <%#Eval("TextoRespuesta") %>
                                            </asp:Label>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                    </div>
                </div>
            </div>
            <div class="col-xs-12 ui-widget-content">
                <div class="col-xs-12 col-lg-6">
                    <div class="form-group">
                        <asp:Label runat="server" ID="Label4" Text="Organización de personalidad y conducta de elección" Style="font-weight: bold;" CssClass="control-label col-xs-12 col-sm-3 col-md-3 col-lg-3"></asp:Label>
                        <div class="col-xs-12 col-sm-9 col-md-9 col-lg-9">
                            <asp:TextBox ID="txtOrganizacionPersonalidad" runat="server" TextMode="MultiLine" Columns="40"
                                Rows="5" CssClass="required form-control" MaxLength="1000" required></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" ID="Label3" Text="Perspectiva de las opciones profesionales y ocupacionales" Style="font-weight: bold;" CssClass="control-label col-xs-12 col-sm-3 col-md-3 col-lg-3"></asp:Label>
                        <div class="col-xs-12 col-sm-9 col-md-9 col-lg-9">
                            <asp:TextBox ID="txtPerspectivaOpciones" runat="server" TextMode="MultiLine" Columns="40"
                                Rows="5" CssClass="required form-control" MaxLength="1000" required></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" ID="lbl2" Text="Fuentes de conflicto para la elección" Style="font-weight: bold;" CssClass="control-label col-xs-12 col-sm-3 col-md-3 col-lg-3"></asp:Label>
                        <div class="col-xs-12 col-sm-9 col-md-9 col-lg-9">
                            <asp:TextBox ID="txtFuentesConflicto" runat="server" TextMode="MultiLine" Columns="40"
                                Rows="5" CssClass="required form-control" MaxLength="1000" required></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-xs-12 col-lg-6">
                    <h2>Instrucciones para completar el reporte de Frases Incompletas Vocacionales:</h2>
                    <ol>
                        <li>Revise de manera cualitativa cada uno de los grupos de 4 frases, referentes a cada área de las actitudes: hacia la familia, las 
                                relaciones interpersonales, las relaciones intrapersonales, y hacia la parte afectiva de la persona.
                        </li>
                        <li>Diríjase a cada uno de los apartados de Madurez, Nivel de realidad y Manera en que los conflictos son expresados, y complete según 
                                sea el caso:                                    
                                        <ol type="a" class="list">
                                            <li>Organización de personalidad y conducta de elección (Descripcion en que consiste).
                                            </li>
                                            <li>Perspectiva de las opciones profesionales y ocupacionales (Descripcion en que consiste).
                                            </li>
                                            <li>Fuentes de conflicto para la elección (Descripcion en que consiste).
                                            </li>
                                        </ol>
                        </li>
                        <li>Al completar los tres apartados guarde sus anotaciones antes de pasar a otro reporte o cambiar de tarea.
                        </li>
                    </ol>
                </div>
            </div>
            <%-- END formulario  principal--%>
            <div class="line"></div>
            <div class="form-group">
                <asp:Button ID="BtnGuardar" Text="Guardar" runat="server"
                    CssClass="button_clip_39215E" OnClick="BtnGuardar_Click" />
                <asp:Button ID="BtnCancelar" Text="Cancelar" runat="server"
                    CssClass="btn-cancel" OnClick="BtnCancelar_Click" UseSubmitBehavior="false" />
            </div>
            <div class="line"></div>
        </div>
    </div>
</asp:Content>
