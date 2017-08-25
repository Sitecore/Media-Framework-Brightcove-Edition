;
var brightcoveListener = new PlayerEventsListener();

brightcoveListener.playerScripts = [];
brightcoveListener.playerScriptsLoaded = [];

brightcoveListener.getEventType = function (event) {
    switch (event.type) {
        case 'timeupdate':
            return this.eventTypes.PlaybackChanged;
        case 'play':
            return this.eventTypes.PlaybackStarted;
        case 'ended':
            return this.eventTypes.PlaybackCompleted;
        case 'error':
            return this.eventTypes.PlaybackError;
        default:
            return undefined;
    }
};

brightcoveListener.getMediaId = function (event) {
    return event.target.player.catalog.data !== undefined
        ? event.target.player.catalog.data.id
        : event.target.player.mediainfo.id;
};

brightcoveListener.getPosition = function (event) {
    return event.target.player.currentTime();
};

brightcoveListener.getDuration = function (event) {
    return event.target.player.duration();
};

brightcoveListener.getContainer = function (args) {
    return this.jQuerySMF(args.target).closest('.mf-player-container');
};

brightcoveListener.getAdditionalParameters = function (event) {
    return {
        mediaId: event.target.player.mediainfo.id,
        mediaName: event.target.player.mediainfo.name,
        mediaLength: Math.round(event.target.player.mediainfo.duration)
    };
};

brightcoveListener.loadPlayerScripts = function () {

    window.onload = function() {
        var keys = Object.keys(brightcoveListener.playerScripts);
        this.jQuerySMF.each(keys, function(index1, value) {
            brightcoveListener.loadScript(brightcoveListener.playerScripts[value], function() {
                brightcoveListener.playerScriptsLoaded[value] = true;
                if (Object.keys(brightcoveListener.playerScripts).length == Object.keys(brightcoveListener.playerScriptsLoaded).length) {
                    brightcoveListener.attachEvents();
                }
            });
        });
    };
}

brightcoveListener.loadScript = function (file, callback) {
    var head = document.getElementsByTagName("head")[0];
    var script = document.createElement('script');
    script.src = file;
    script.type = 'text/javascript';
    //real browsers
    script.onload = callback;
    //Internet explorer
    script.onreadystatechange = function () {
        if (this.readyState == 'complete') {
            callback();
        }
    }
    head.appendChild(script);
}

brightcoveListener.attachEvents = function () {
    this.jQuerySMF.each(this.jQuerySMF("div.video-js"), function (i, val) {
        brightcoveListener.onTemplateReady(val.id);
    });
}


brightcoveListener.log = function (eventType, event) {
    
}

brightcoveListener.onTemplateReady = function (playerId) {
    videojs(playerId).ready(function () {
        var player = this;
        player.on('play', function (event) {
            
            if (player.catalog.data !== undefined) {
                var currentItem = event.target.player.playlist.currentItem();
                var playlist = event.target.player.playlist();

                if (currentItem >= 0 && playlist !== undefined && playlist.length > currentItem) {
                    var currentMediaInfo = playlist[currentItem];
                    if (currentMediaInfo === undefined) {
                        return;
                    }

                    event.target.player.mediainfo = playlist[currentItem];
                }
            }

            brightcoveListener.log('play', event);
            brightcoveListener.onMediaEvent(event);
        });

        player.on('ended', function (event) {
            brightcoveListener.log('ended', event);
            brightcoveListener.onMediaEvent(event);
        });

        player.on('error', function (event) {
            brightcoveListener.log('error', event);
            brightcoveListener.onMediaEvent(event);
        });

        player.on('timeupdate', function (event) {
            brightcoveListener.log('timeupdate', event);
            brightcoveListener.onMediaEvent(event);
        });
        player.on('loadstart', function (event) {
            brightcoveListener.log('change event', event);
            brightcoveListener.onMediaChanged(event);
        });
    });
};

brightcoveListener.loadPlayerScripts();