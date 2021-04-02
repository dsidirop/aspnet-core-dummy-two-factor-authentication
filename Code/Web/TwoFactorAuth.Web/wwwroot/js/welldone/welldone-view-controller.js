; //vital   keep this

(function (self, $, _window, _document) {

    "use strict";

    let _config = {
        rootContainerSelector: ""
    };

    const _$els = {
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
        _$els.$imgGreenTick = _$els.$rootContainer.find(".app-green-tick");
    };

    function initConfig(config) {
        _config = config;
    };

    function initImplementation() {
        console.log("** initImplementation2");
        
        _$els
            .$imgGreenTick
            .gifplayer();

        _$els
            .$imgGreenTick
            .gifplayer("play");

        _window.setTimeout(
            function() {
                _$els
                    .$imgGreenTick
                    .gifplayer("stop");
            },
            4500
        );
    };

})((window.WelldoneController = window.WelldoneController || {}), window.jQuery, window, document);
