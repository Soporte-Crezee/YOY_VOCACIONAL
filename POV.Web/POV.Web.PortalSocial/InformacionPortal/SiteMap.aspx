<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SiteMap.aspx.cs"
    Inherits="POV.Web.PortalSocial.InformacionPortal.SiteMap" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            Cargarhijos();
            Cargardescripcion();
            EstilosInicio();
        });

        function EstilosInicio() {
            $('.contenedorhijo a').addClass("colorLinksInicio");
            $(".contenedorpadre a").addClass("colorLinksInicio");
        }

        function Cargarhijos() {
            $("li[id*='liPadre'] a").click(function (event) {
                event.preventDefault();
                var lista = $(this).parent().find('ul').clone();
                $('.contenedorhijo').empty().append(lista);
                var descripcion = $(this).parent().find($("[id*='Pdescripcion']")).clone();
                $('.parrafo').empty().append(descripcion.val());
                var link = $(this).parent().find($("[id*='Plink']")).clone();
                if (link.val() == "") {
                    $("#botonLink").css("display", "none");
                }
                else {
                    $("#botonLink").css("display", "block");
                    $('#botonLink').attr("href", link.val());
                }
                var imagen = $(this).parent().find($("[id*='PImagen']")).clone();
                if (imagen.val() == "") {
                    $("#ImagenDescripcion").css("display", "none");
                }
                else {
                    $("#ImagenDescripcion").css("display", "block");
                    $('#ImagenDescripcion').attr("src", imagen.val());
                }
                Cargardescripcion();
                EstilosInicio();
                $(".lista").find("a").removeClass("aClick");
                $(".lista").find("li").removeClass("listaClik");
                $(this).removeClass("colorLinksInicio");
                $(this).parent().addClass("listaClik");
                $(this).addClass("aClick");
            });
        }

        function Cargardescripcion() {
            $("li[id*='liAnidada'] a").click(function (event) {
                event.preventDefault();
                var descripcion = $(this).parent().find("[id*='Cdescripcion']").clone();
                $('.parrafo').empty().append(descripcion.val());
                var link = $(this).parent().find($("[id*='Clink']")).clone();
                if (link.val() == "") {
                    $("#botonLink").css("display", "none");
                }
                else {
                    $("#botonLink").css("display", "block");
                    $('#botonLink').attr("href", link.val());
                }
                var imagen = $(this).parent().find($("[id*='CImagen']")).clone();
                if (imagen.val() == "") {
                    $("#ImagenDescripcion").css("display", "none");
                }
                else {
                    $("#ImagenDescripcion").css("display", "block");
                    $('#ImagenDescripcion').attr("src", imagen.val());
                }
                $('.contenedorhijo a').addClass("colorLinksInicio");
                $(".contenedorhijo ul li").find("a").removeClass("aClick");
                $(this).removeClass("colorLinksInicio");
                $(this).parent().addClass("listaHijosclick");
                $(this).addClass("aClick");
            });
        }
    </script>
    <style type="text/css">
        .title
        {
            border-bottom: 5px solid #BB3BAB;
            margin-bottom: 10px;
            background-color: #CCCCCC;
        }
        .contenedorpadre
        {
            width: 25%;
            height: 500px;
            float: left;
            border-right: 5px solid #55269B;
            overflow: scroll;
        }
        .contenedorhijo
        {
            float: left;
            height: 500px;
            width: 25%;
            border-right: 5px solid #55269B;
            overflow: scroll;
        }
        .divDescripcion
        {
            float: right;
            width: 48%;
            height: 500px;
            overflow: scroll;
        }
        .lista li:hover
        {
            list-style-image: url("../Images/icons/viñetaMapaSitio.png");
            list-style-position: inside;
            color: #BB3BAB;
        }
        .lista a:hover
        {
            font: bold 1.5em Arial, Helvetica, sans-serif;
            color: #BB3BAB;
        }
        .listaClik
        {
            list-style-image: url("../Images/icons/viñetaMapaSitio.png");
            list-style-position: inside;
        }
        .aClick
        {
            font: bold 1.5em Arial, Helvetica, sans-serif;
            color: #BB3BAB;
        }
        
        .lista li
        {
            font-size: 1em;
            padding: 0.5em;
        }
        .colorLinksInicio
        {
            font-size: 1.5em;
            color: #a9a9a9;
        }
        .lista li ul
        {
            display: none;
        }
        .contenedorhijo ul li
        {
            text-decoration: underline;
            list-style-position: inside;
            font-size: 1em;
            padding: 0.5em;
            color: #a9a9a9;
        }
        .contenedorhijo ul li a:hover
        {
            font: bold 1.5em Arial, Helvetica, sans-serif;
            color: #BB3BAB;
        }
        .listaHijosclick
        {
            text-decoration: underline;
            list-style-position: inside;
            font-size: 1em;
            padding: 0.5em;
            color: #a9a9a9;
        }
        .parrafo
        {
            font-size: 1em;
            line-height: 1.25em;
            margin: 0;
            text-align: justify;
        }
        .imagen
        {
            border: 3px solid #a9a9a9;
            display: none;
            margin: 0 auto;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="page_content" runat="server">
    <div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div class="title">
                    <h1 class="title_2" style="padding-left: 30px">
                        Mapa del sitio
                    </h1>
                </div>
                <div id="ContenedorPadre" class="contenedorpadre subline_title" runat="server">
                    <ul id="linkList" class="lista" runat="server">
                    </ul>
                </div>
                <div id="ContenedorHijo" class="contenedorhijo">
                    <ul id="linkListHijos" runat="server">
                    </ul>
                </div>
                <div id="divDescripcion" class="divDescripcion">
                    <div id="title" style="height: 25px; display: block; border-bottom: 3px solid #55269B;">
                        <h1 class="title_6" style="padding-left: 10px; float: left;">
                            Descripci&oacute;n:
                        </h1>
                        <a href="#" id="botonLink" style="float: right; display: none; padding-right: 10px;">
                            <img src="../Images/botonIrASeccion.png" alt="Ir a esta sección"></a>
                    </div>
                    <div id="ContenidoDescripcion">
                        <br />
                        <img id="ImagenDescripcion" src="" alt="Imagen Descripci&oacute;n" height="80%" width="90%"
                            class="imagen">
                        <p id="descripcion" class="parrafo" style="padding: 10px">
                        </p>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
