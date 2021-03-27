; //vital   keep this

(function (self, $, _window, _document, _gifffer) {

    "use strict";

    let _config = {
        rootContainerSelector: ""
    };

    let _$els = {
        $window: null,
        $document: null,
        $rootContainer: null
    };

    self.Init = function (config) {
        try {
            initConfig(config);
            hookElements();
            initImplementation();
        } catch (err) {
            console.error("[WDC.INIT010] [BUG] Initialization failed", err);
        }
    };

    function hookElements() {
        _$els.$window = $(_window);
        _$els.$document = $(_document);
        _$els.$rootContainer = _$els.$document.find(_config.rootContainerSelector);
    }

    function initConfig(config) {
        _config = config;
    }

    function initImplementation() {
        const gifs = _gifffer({ //todo  fork gifffer and sanitize it to make its api more appropriate
            playButtonStyles: {'display': 'none'},
            playButtonIconStyles: {'display': 'none'}
        });

        const handler = function() {
            if (gifs.length === 0) {
                _window.setTimeout( handler, 5);
                return;
            }

            $(gifs).click(); //start animation
        };

        _window.setTimeout( handler, 50);
    }

})((window.WelldoneController = window.WelldoneController || {}), window.jQuery, window, document, window.Gifffer);
