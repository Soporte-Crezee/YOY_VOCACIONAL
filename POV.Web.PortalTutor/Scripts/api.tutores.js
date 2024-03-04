var TutoresApi = function () { };

TutoresApi.prototype.ValidateTutor = function (options, tutor) {
    var config = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    $.apiCall({
        url: encodeURI("../wcf/TutorService.svc/ValidateTutor"),
        type: 'POST',
        data: tutor,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        async: false,
        success: function (result) { config.success(result); },
        error: function (result) { config.error(result); }
    });
};