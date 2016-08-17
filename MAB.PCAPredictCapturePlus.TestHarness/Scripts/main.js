var App = (function($) {
    "use strict";

    function _ajaxPost(url, data, successCallback, errorCallback, contentType) {
        var options = {
            url: url,
            data: data,
            dataType: 'json',
            type: 'POST',
            success: successCallback,
            error: errorCallback
        };

        if (contentType) {
            options.contentType = contentType;
        }

        $.ajax(options);
    };

    function _init() {
        console.log('test');
    };

    return {
        init: _init
    };
}(jQuery));