var App = (function() {
    "use strict";

    var ResultType = Object.freeze({
        Unknown: 0,
        Address: 1,
        Postcode: 2,
        Locality: 3
    });

    var _resultHtml = '<a href="#" class="mab-pca-predict-capture-plus-suggestion" data-id="{{id}}" data-type="{{type}}">{{text}}, {{description}}</a>';

    var _ui = {
        console: null,
        suggestions: null,
        inputs: {}
    };

    var _options = {
        test: 0
    };

    function _isEmpty(str) {
        return !/[^\s]/.test(str);
    }

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

    function _renderSuggestion(id, text, description, type) {
        return _resultHtml.replace(/{{id}}/g, id)
                          .replace(/{{text}}/g, text)
                          .replace(/{{description}}/g, description)
                          .replace(/{{type}}/g, type);
    }

    function _renderSuggestions(data) {
        var suggestions = [];
        for(var i = 0; i < data.length; i++) {
            var result = data[i];
            suggestions.push(_renderSuggestion(result.Id, result.Text, result.Description, result.Type));
        }
        return suggestions.join('');
    }

    function _positionSuggestionDropdown(input, container) {
        var pos = input.getBoundingClientRect();
        container.style.display = 'block';
        container.style.top = (pos.top + input.offsetHeight) + 'px';
        // TODO: figure out how to get left padding of suggestions reliably
        container.style.left = (pos.left - 10) + 'px';
    }

    function _setSuggestionInput(container, id) {
        container.setAttribute('data-input', id);
    }

    function _hideSuggestionDropdown(container) {
        container.style.top = -500;
        container.style.display = 'none';
    }

    function _handleSuggestionClick(container, a) {
        var input = container.getAttribute('data-input');
        var id = a.getAttribute('data-id');
        var type = parseInt(a.getAttribute('data-type'), 10);

        if(type !== ResultType.Address) {
            _search('/Home/FindByContainer', id, function (data) {
                _ui.suggestions.innerHTML = _renderSuggestions(data);
            });
        } else {
            _ajaxPost('/Home/Retrieve', { id: id }, function(data) {
                _forEachPropertyOf(_options.fields, function(k, v) {
                    _ui.inputs[k].value = data[k];
                    // console.log(_options.fields[k].input + ': ' + data[k]);
                });
                _hideSuggestionDropdown(container);
            }, function(err) {
                console.log(err);
            });
        }
    }

    function _search(url, text, resultCallback, noresultCallback, errorCallback) {
        _ajaxPost(url, { text: text }, function (data) {
            if(data.length) {
                resultCallback(data);
            } else {
                noresultCallback();
            }
        }, errorCallback);
    }

    function _init(options) {
        _options = _extend(_options, options);

        _ui.console = document.getElementById('console');
        _ui.suggestions = document.getElementsByClassName('mab-pca-predict-capture-plus-suggestions')[0];

        _forEachPropertyOf(_options.fields, function(k, v) {
            var input = document.getElementById(v.input);
            // Cache the input reference
            _ui.inputs[v.input] = input;
            if(v.search) {
                input.addEventListener('keyup', _debounce(function(e) { 
                    var term = input.value;
                    if(_isEmpty(term)) {
                        _hideSuggestionDropdown(_ui.suggestions);
                    } else {
                        _search('/Home/Find', term, function(data) {
                            _ui.suggestions.innerHTML = _renderSuggestions(data);
                            _positionSuggestionDropdown(input, _ui.suggestions);
                            _setSuggestionInput(_ui.suggestions, input.id);
                        }, function() {
                            _hideSuggestionDropdown(_ui.suggestions);
                        }, function(err) {
                            console.log(err);
                        });
                    }
                }, 250));
            }
        });

        _ui.suggestions.addEventListener('click', function(e) {
            var suggestions = this;
            e.preventDefault();
            if(e.target.className === 'mab-pca-predict-capture-plus-suggestion') {
                _handleSuggestionClick(suggestions, e.target);
            }
        });
    }

    return {
        init: _init
    };
}());