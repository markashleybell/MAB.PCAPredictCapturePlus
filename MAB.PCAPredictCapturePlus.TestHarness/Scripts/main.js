var App = (function() {
    "use strict";

    var _ui = {
        console: null,
        suggestions: null,
        inputs: {}
    };

    var _options = {
        test: 0
    };

    function _forEachPropertyOf(obj, action) {
        for(var p in obj) {
            if (obj.hasOwnProperty(p)) {
                action(p, obj[p]);
            }
        }
    }

    function _extend(target, source) {
        target = target || {};
        _forEachPropertyOf(source, function(k, v) {
            target[k] = (typeof v === 'object') ? _extend(target[k], v) : target[k] = v;
        });
        return target;
    }

    function _queryString(obj) {
        var output = [];
        _forEachPropertyOf(obj, function(k, v) {
            output.push(encodeURIComponent(k) + '=' + encodeURIComponent(v));
        });
        return output.join('&');
    }

    function _debounce(func, wait, immediate) {
        var timeout;
        return function() {
            var context = this, 
                args = arguments;
            var later = function() {
                timeout = null;
                if (!immediate) func.apply(context, args);
            }
            var callNow = immediate && !timeout;
            clearTimeout(timeout);
            timeout = setTimeout(later, wait);
            if (callNow) {
                func.apply(context, args);
            }
        }
    }

    function _ajaxPost(url, data, successCallback, errorCallback) {
        var request = new XMLHttpRequest();

        request.open('POST', url, true);
        request.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded; charset=UTF-8');

        request.onload = function() {
            if (request.status >= 200 && request.status < 400) {
                var json = JSON.parse(request.responseText);
                successCallback(json);
            } else {
                errorCallback(request);
            }
        };

        request.onerror = function(e) {
            errorCallback(request, e);
        };

        request.send(_queryString(data));
    }

    function _init(options) {
        _options = _extend(_options, options);

        _ui.console = document.getElementById('console');
        _ui.suggestions = document.getElementsByClassName('mab-pca-predict-capture-plus-suggestions')[0];

        _forEachPropertyOf(_options.fields, function(k, v) {
            // _ui.inputs[k] = document.getElementById(v.input);
            var input = document.getElementById(v.input);
            if(v.search) {
                input.addEventListener('keyup', _debounce(function(e) { 
                    _ajaxPost('/Home/Find', { term: input.value }, function(data) { 
                        _ui.suggestions.innerHTML = data;
                        _ui.suggestions.style.display = 'block';
                        var pos = input.getBoundingClientRect();
                        _ui.suggestions.style.top = (pos.top + input.offsetHeight) + 'px';
                        _ui.suggestions.style.left = pos.left + 'px';
                    }, function(err) {
                        console.log(err);
                    });
                }, 500));
            }
        });
    }

    return {
        init: _init
    };
}());