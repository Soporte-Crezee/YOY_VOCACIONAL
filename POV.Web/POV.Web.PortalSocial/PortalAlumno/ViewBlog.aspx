<%@ Page Title="YOY - ESTUDIANTE" Language="C#" MasterPageFile="~/PortalAlumno/PortalAlumno.master" AutoEventWireup="true" CodeBehind="ViewBlog.aspx.cs" Inherits="POV.Web.PortalSocial.PortalAlumno.ViewBlog" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head_selected" runat="server">
    <script src="<% =Page.ResolveClientUrl("~/Scripts/jquery.validate.min.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/jquery.ui.datepicker.validation.min.js")%>" type="text/javascript"></script>
    <%-- Adecuacion Crezee --%>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>api.favoritos.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>core.favoritos.js" type="text/javascript"></script>

    <style type="text/css">
        .star {
            cursor: pointer;
        }

        .ui-datepicker-year {
            color: #242525;
        }

        .ui-datepicker-month {
            color: #242525;
        }

        .ui-datepicker .ui-datepicker-header {
            background-color: #05aed9;
            border-color: gray;
        }
    </style>

    <script src="<% =Page.ResolveClientUrl("~/Scripts/rating/")%>star-rating.js" type="text/javascript"></script>
    <script type="text/javascript">

        $(document).ready(initPage);

        function showError(message) {
            $("#txtFechaInicio").addClass("error");
            setTimeout(function () {
                alert(message);
            }, 300);
        }

        function initPage() {
            $(hdnmessageinputid).val("La imagen debe ser tuya y no debe ser ofensiva para los usuarios.");
            $.datepicker.setDefaults($.datepicker.regional['es']);
            $('#txtFechaInicio').datepicker({
                yearRange: '-100:+0',
                changeYear: true,
                changeMonth: true,
                dateFormat: "dd/mm/yy",
                error: "Error"
            });

            $('#<%=txtFechaFin.ClientID %>').datepicker({
			    yearRange: '-100:+0',
			    changeYear: true,
			    changeMonth: true,
			    dateFormat: "dd/mm/yy"
			});

			rules = {
			    '<%=txtFechaInicio.UniqueID %>': { dpDate: true },
                '<%=txtFechaFin.UniqueID %>': { dpDate: true }
            };

            $('#form1').validate(
            {
                errorPlacement: $.datepicker.errorPlacement,
                rules: rules,
                submitHandler: function (form) {
                    $('.page_container_login').block();
                    form.submit();
                },
                messages: {
                    dpDate: 'Formato invalido (dd/mm/yyyy)',
                    '<%=txtFechaInicio.UniqueID %>': { dpCompareDate: 'La fecha inicio no puede ser mayor a la fecha fin' },
            		'<%=txtFechaFin.UniqueID %>': { dpCompareDate: 'La fecha Fin no puede ser menor a la fecha inicio' }
            	}
            });
        }

        $(function () {
            var totalCardsFavorites = 0;
            var splitUrl = function () {
                var vars = [], hash;
                var url = document.URL.split('?')[0];
                var p = document.URL.split('?')[1];
                if (p != undefined) {
                    p = p.split('&');
                    for (var i = 0; i < p.length; i++) {
                        hash = p[i].split('=');
                        vars.push(hash[1]);
                        vars[hash[0]] = hash[1];
                    }
                }
                vars['url'] = url;
                return vars;
            };
            var urlAreaConocimientoBlog = splitUrl()['AreaConocimientoBlog'];
            var urlRedirect = splitUrl()['Redirect'];

            $('span.filled-stars').attr({
                'title': 'Eliminar como favorito',
            });

            $('span.empty-stars').attr({
                'title': 'Guardar como favorito',
            });

            $("span.filled-stars").each(function (i) {
                var input = $('input.rating').eq(i);
                var star = $(this);
                var clear = $('div.clear-rating').eq(i);
                clear.hide();
                if (input.val() == 0) {
                    star.hide();
                }
                totalCardsFavorites++;
            });

            if (urlAreaConocimientoBlog == 'postfavoritos' && urlRedirect == undefined) {
                getTotalPostsFavoritos(totalCardsFavorites);
            }

            $('span.filled-stars').on('click',
                function () {
                    var i = $('span.filled-stars').index($(this));
                    var star = $(this);
                    var postCard = $('div.post-card').eq(i);
                    var input = $('input.rating').eq(i);
                    var clear = $('div.clear-rating').eq(i);

                    var BlogId = input.attr('id').split('_')[1];
                    var PostId = input.attr('id').split('_')[2];

                    if ((star.hasClass('active')) || (input.val() == 1)) {
                        star.removeClass("active");
                        star.hide();
                        input.val(0);
                        setTimeout(function () {
                            clear.click();
                            eliminarPostFavorito(BlogId, PostId);
                            if (urlAreaConocimientoBlog == 'postfavoritos' && urlRedirect == undefined) {
                                postCard.hide();
                                totalCardsFavorites--;
                                getTotalPostsFavoritos(totalCardsFavorites);
                            }
                        }, 300);
                    }
                    else {
                        star.addClass("active");
                        star.show();
                        input.val(1);
                    }
                });

            $('span.empty-stars').on('click',
                function () {
                    var i = $('span.empty-stars').index($(this));
                    var star = $('span.filled-stars').eq(i);
                    var input = $('input.rating').eq(i);

                    var BlogId = input.attr('id').split('_')[1];
                    var PostId = input.attr('id').split('_')[2];
                    var Categorias = input.attr('id').split('_')[3];

                    star.addClass("active");
                    star.show();
                    input.val(1);

                    guardarPostFavorito(BlogId, PostId, Categorias);
                });

            function guardarPostFavorito(_blogId, _postId, _categorias) {
                var PostFavorito = {};
                var dto = {};
                dto.PostFavoritoAspiranteId = null;
                dto.BlogId = _blogId;
                dto.PostId = _postId;
                dto.Categorias = _categorias;
                dto.Success = "";
                dto.Error = "";

                PostFavorito.dto = dto;
                insertarFavoritos(PostFavorito);
            }

            function eliminarPostFavorito(_blogId, _postId) {
                var PostFavorito = {};
                var dto = {};
                dto.PostFavoritoAspiranteId = null;
                dto.BlogId = _blogId;
                dto.PostId = _postId;
                dto.Success = "";
                dto.Error = "";

                PostFavorito.dto = dto;
                eliminarFavoritos(PostFavorito);
            }

            function getTotalPostsFavoritos(_totalCardsFavorites) {
                if (_totalCardsFavorites < 1) {
                    $("#emptyPostsFavoritos").show();
                }
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content_selected" runat="server">
    <div id="info_perfil">
        <div style="height: 700px" runat="server" id="blogIframe">
            <iframe src="http://testpov.grupoplenum.com/Blog/?rule=loginHidden" frameborder="1" style="width: 102%; height: 100%"></iframe>
        </div>
        <div runat="server" id="postsList" class="col-xs-12 col-md-12">
            <div runat="server" id="filterPosts" style="padding: 0px 0px 15px 12px" class="col-xs-12">
                <div class="col-xs-12 form-group">
                    <asp:Label class="col-sm-1 control-label" runat="server" ID="lblFecha">Fecha:</asp:Label>
                    <div class="col-sm-3">
                        <asp:TextBox runat="server" ID="txtFechaInicio" ClientIDMode="Static" CssClass="form-control input-sm"></asp:TextBox>
                    </div>
                    <asp:Label class="col-sm-1 control-label" runat="server" ID="lblAl">Al</asp:Label>
                    <div class="col-sm-3">
                        <asp:TextBox runat="server" ID="txtFechaFin" ClientIDMode="Static" CssClass="form-control input-sm"></asp:TextBox>
                    </div>
                    <div class="col-sm-4">
                        <asp:Button runat="server" ID="btnfiltrar" Text="Buscar" OnClick="btnfiltrar_Click" CssClass="button_clip_39215E btn-green" />
                    </div>
                </div>
            </div>
            <div runat="server" id="emptyPosts" class="col-xs-12">
                <div id="more">
                    <span>No existen posts en el &aacute;rea de conocimiento seleccionada</span>
                </div>
            </div>
            <div id="emptyPostsFavoritos" style="display: none" class="col-xs-12">
                <div id="more">
                    <span>No existen posts favoritos</span>
                </div>
            </div>
            <div runat="server" id="listPosts">
            </div>
        </div>

        <div style="height: 700px" runat="server" id="postIframe">
            <div id="more" class='pull-left'>
                <span><a runat="server" id="backPosts" href="?AreaConocimientoBlog=">Regresar a la lista</a></span>
            </div>
            <iframe runat="server" id="postView" src="http://testpov.grupoplenum.com/Blog/?rule=loginHidden&redirect=" frameborder="1" style="width: 102%; height: 100%;"></iframe>
        </div>
    </div>
    <asp:HiddenField ID="hdnSocialHubID" runat="server" />
    <asp:HiddenField ID="hdnUsuarioSocialID" runat="server" />
    <asp:HiddenField ID="hdnSessionSocialHubID" runat="server" />
    <asp:HiddenField ID="hdnSessionUsuarioSocialID" runat="server" />
    <asp:HiddenField ID="hdnFuente" runat="server" Value="A" />
</asp:Content>
