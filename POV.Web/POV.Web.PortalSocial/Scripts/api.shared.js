(function ($) {
    $.apiCall = function (options) {
        var config = $.extend({
            type: 'GET',
            data: {},
            contentType: 'application/x-www-form-urlencoded',
            success: function () { },
            error: function () { }
        }, options);

        $.ajax({
            type: config.type,
            url: config.url,
            contentType: config.contentType,
            data: config.data,
            success: function (result) {
                config.success(result);
            },
            error: function (result) {
                config.error(result);
                if (myApiBlockUI != undefined && myApiBlockUI != null) {
                    myApiBlockUI.unblockContainer();
                }
            }
        });
    }
})(jQuery);