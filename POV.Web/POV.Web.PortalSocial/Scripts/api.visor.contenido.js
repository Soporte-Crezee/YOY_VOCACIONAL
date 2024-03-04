//Consultar.Informacion.Contenido.Digital.Red.Social
var VisorApi = function (opciones) {
    this.myConfigs = $.extend({
        srcPlantillas: '',
        srcScripts: '',
        elementVisorId: '',
        elementOpcionesId: '',
        urlContenido: '',
        visorType: '',
        esDescargable: false,
        esRedireccion: false,
        esReproducible: false,
        plantillaVisorVideo: "VisorVideo.htm",
        plantillaVisorAudio: "VisorAudio.htm"
    }, opciones || {});

    this.initVisor = function () {
        this.initOpcionesVisor(this.myConfigs);

        if (this.myConfigs.esReproducible) {
            switch (this.myConfigs.visorType) {
                case 'VISORPDF':
                    this.initVisorPDF(this.myConfigs);
                    break;
                case 'VISORMP4':
                    this.initVisorVideo(this.myConfigs);
                    break;
                case 'VISORMP3':
                    this.initVisorAudio(this.myConfigs);
                    break;
                case 'VISORYOUTUBE':
                    this.initVisorYoutube(this.myConfigs);
                    break;
                case 'VISORIMG':
                    this.initVisorIMG(this.myConfigs);
                    break;
                case 'VISORURL':
                    this.initVisorURL(this.myConfigs);
                    break
                default:
                    this.initDefaultVisor();
            }
        }

    };

    this.initOpcionesVisor = function (oConfigs) {
        var htmlOpciones = '';
        var inicia = (/^(https?):\/\//).test(oConfigs.urlContenido);
        var direccion = "#";
        if (!inicia)
            direccion = "http://" + oConfigs.urlContenido;
        else
            direccion = oConfigs.urlContenido;

        if (oConfigs.esDescargable) {
            htmlOpciones += '<a href="' + direccion + '" class="btn-green" target="_blank">Descargar</a> ';

        }

        if (oConfigs.esRedireccion) {
            htmlOpciones += '<a href="' + direccion + '" class="btn-green" target="_blank">Ir a recurso didáctico</a>';
        }

        $("#" + oConfigs.elementOpcionesId).html(htmlOpciones);
    }

    /** funciones para iniciar el contenido**/
    this.initVisorPDF = function (oConfigs) { //VISORPDF
        var variablename = new PDFObject({ url: oConfigs.urlContenido }).embed(oConfigs.elementVisorId);
        $("#" + oConfigs.elementVisorId).css({ height: '500px' });
    };
    this.initVisorYoutube = function (oConfigs) { //VISORYOUTUBE
        var widthVisor = 640;

        var videoId = getYouTubeIdFromUrl(oConfigs.urlContenido);

        var urlVideo = "http://www.youtube.com/embed/" + videoId;
        var htmlVisor = getYouTubePlayer(urlVideo, widthVisor, 360);
        var widthContainer = $("#" + oConfigs.elementVisorId).width();
        var width = ((widthContainer - widthVisor) / 2) - 10;
        $("#" + oConfigs.elementVisorId).css({ margin: '0px' });

        $("#" + oConfigs.elementVisorId).html(htmlVisor);
    }

    function getYouTubePlayer(URL, width, height) {
        var YouTubePlayer = '<iframe title="YouTube video player" class="embed-responsive-item" style="margin:0; padding:0;" width="100%" ';
        YouTubePlayer += 'height="'+height+'" src="' + URL + '" frameborder="0" allowfullscreen></iframe>';
        return YouTubePlayer;
    }

    function getYouTubeIdFromUrl(youtubeUrl) {
        var regExp = /^.*(youtu.be\/|v\/|u\/\w\/|embed\/|watch\?v=)([^#\&\?]*).*/;
        var match = youtubeUrl.match(regExp);
        if (match && match[2].length == 11) {
            return match[2];
        } else {
            return false;
        }
    }

    this.initVisorVideo = function (oConfigs) { //VISORVIDEO
        $("#contenido_digital").show();
        $.get(oConfigs.srcPlantillas + oConfigs.plantillaVisorVideo, function (template) {
            $("#" + oConfigs.elementVisorId).html(template);
            var widthContainer = $("#" + oConfigs.elementVisorId).width();
            //if (widthContainer >= 959) {
                var width = ((widthContainer - 640) / 2) - 10;
                $("#" + oConfigs.elementVisorId).css({ margin: '0px' });

                $("#jquery_jplayert_video").jPlayer({
                    solution: "flash",
                    ready: function () {
                        $("#jquery_jplayert_video").jPlayer("setMedia", {
                            m4v: oConfigs.urlContenido
                        }).jPlayer("play");
                    },
                    cssSelectorAncestor: "#jp_container_video",
                    swfPath: oConfigs.srcScripts + "jQuery.jPlayer.2.4.0/Jplayer.swf",
                    supplied: "m4v, flv",
                    loop: false,
                    size: { width: "100%", height: "360px", cssClass: "jp-video-360p" },
                    error: onErrorInitPlayer
                });
        });
    }

    this.initVisorAudio = function (oConfigs) { //VISORAUDIO
        $.get(oConfigs.srcPlantillas + oConfigs.plantillaVisorAudio, function (template) {
            $("#" + oConfigs.elementVisorId).html(template);
            var widthContainer = $("#" + oConfigs.elementVisorId).width();
            var width = ((widthContainer - 250) / 2) - 10;
            $("#" + oConfigs.elementVisorId).css({ margin: '0px' });
            $("#jquery_jplayert_audio").jPlayer({
                solution: "flash",
                ready: function () {
                    $(this).jPlayer("setMedia", {
                        mp3: oConfigs.urlContenido
                    }).jPlayer("play");
                },
                cssSelectorAncestor: "#jp_container_audio",
                swfPath: oConfigs.srcScripts + "jQuery.jPlayer.2.4.0/Jplayer.swf",
                supplied: "mp3",
                loop: false,
                error: onErrorInitPlayer
            });
        });

    }
    function onErrorInitPlayer(event) {
        var apiMessages = new MessageApi();
        var message = "Error de archivo " + event.jPlayer.error.type;
        apiMessages.CreateMessage(message,"ERROR");

        apiMessages.Show();       
    }

    this.initVisorIMG = function (oConfigs) {
        var htmlImg = '<img src="' + oConfigs.urlContenido + '" alt="Sistema_img" class="img img-responsive"></img>';
        var widthContainer = $("#" + oConfigs.elementVisorId).width();
        var width = ((widthContainer - 850) / 2);
        $("#" + oConfigs.elementVisorId).css({ margin: '0px' });
        $("#" + oConfigs.elementVisorId).html(htmlImg);

    }

    this.initVisorURL = function (oConfigs) {
        var widthVisor = 900;


        var htmlVisor = getYouTubePlayer(oConfigs.urlContenido, widthVisor, 600);
        var widthContainer = $("#" + oConfigs.elementVisorId).width();
        var width = ((widthContainer - widthVisor) / 2) - 10;
        $("#" + oConfigs.elementVisorId).css({ margin: '0 ' + width + 'px' });

        $("#" + oConfigs.elementVisorId).html(htmlVisor);

        $("#" + oConfigs.elementOpcionesId).html("");
    }
};




