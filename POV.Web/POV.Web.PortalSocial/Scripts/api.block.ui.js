
var BlockUIApi = function (options) {
    this.config = $.extend({
        loadingImg: '',
        processImg: '',
        searchImg: '',
        sendImg: '',
        imageSource: ''
    }, options || {});


    this.loading = function () {
        var messageBlock = '<h1><img src="' + this.config.imageSource + this.config.loadingImg + '" class="mensaje_bloqueo" /></h1>'

        $.blockUI({ message: messageBlock });
    }

    this.send = function () {
        var messageBlock = '<h1><img src="' + this.config.imageSource + this.config.sendImg + '" class="mensaje_bloqueo" /></h1>'

        $.blockUI({ message: messageBlock });
    }

    this.process = function () {
        var messageBlock = '<h1><img src="' + this.config.imageSource + this.config.processImg + '" class="mensaje_bloqueo" /></h1>'

        $.blockUI({ message: messageBlock });
    }

    this.search = function () {
        var messageBlock = '<h1><img src="' + this.config.imageSource + this.config.searchImg + '" class="mensaje_bloqueo" /></h1>';

        $.blockUI({ message: messageBlock });
    }

    this.unblockContainer = function () {
        $.unblockUI();
    }

};


