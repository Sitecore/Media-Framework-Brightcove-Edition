;
var brightcoveListener = new PlayerEventsListener();

brightcoveListener.playerScripts = [];
brightcoveListener.playerScriptsLoaded = [];

brightcoveListener.getEventType = function (event) {
    switch (event.type) {
        case 'timeupdate': //brightcove.api.events.MediaEvent.PROGRESS:
            return this.eventTypes.PlaybackChanged;
        case 'play': //brightcove.api.events.MediaEvent.BEGIN:
            return this.eventTypes.PlaybackStarted;
        case 'ended': //brightcove.api.events.MediaEvent.COMPLETE:
            return this.eventTypes.PlaybackCompleted;
        case 'error': //brightcove.api.events.MediaEvent.ERROR:
            return this.eventTypes.PlaybackError;
        default:
            return undefined;
    }
};

brightcoveListener.getMediaId = function (event) {
    return event.target.player.catalog.data !== undefined
        ? event.target.player.catalog.data.id
        : event.target.player.mediainfo.id; //event.media.lineupId ? event.media.lineupId : event.media.id;
};

brightcoveListener.getPosition = function (event) {
    ////if (event.target.player.catalog.data !== undefined) {
    ////    return brightcoveListener.getTotalTime(event.target.player);
    ////}
    return event.target.player.currentTime();
};

brightcoveListener.getDuration = function (event) {
    ////if (event.target.player.catalog.data !== undefined) {
    ////    return brightcoveListener.getTotalDuration(event.target.player);
    ////}
    return event.target.player.duration();
};

brightcoveListener.getContainer = function (args) {
    return this.jQuerySMF(args.target).closest('.mf-player-container');
    //return this.jQuerySMF('#' + args.target.experience.id).closest('.mf-player-container');
};

brightcoveListener.getAdditionalParameters = function (event) {
    ////if (event.target.player.catalog.data !== undefined) {
    ////    return {
    ////        mediaId: event.target.player.catalog.data.id, //event.media.id,
    ////        mediaName: event.target.player.catalog.data.name, //event.media.displayName,
    ////        mediaLength: Math.round(brightcoveListener.getTotalDuration(event.target.player)) //Math.round(event.media.length / 1000)
    ////    };
    ////}
    return {
        mediaId: event.target.player.mediainfo.id, //event.media.id,
        mediaName: event.target.player.mediainfo.name, //event.media.displayName,
        mediaLength: Math.round(event.target.player.mediainfo.duration) //Math.round(event.media.length / 1000)
    };
};

//brightcoveListener.attachEventsCalled = false;

brightcoveListener.loadPlayerScripts = function () {

    window.onload = function() {
        var keys = Object.keys(brightcoveListener.playerScripts);
        //console.log(brightcoveListener.playerScripts);
        //console.log(brightcoveListener.playerScripts.length);

        this.jQuerySMF.each(keys, function(index1, value) {
            //console.log('2222');
            //console.log(index1);
            //console.log(value);
            //console.log(brightcoveListener.playerScripts[value]);
            brightcoveListener.loadScript(brightcoveListener.playerScripts[value], function() {
                //console.log('script callback ' + value);
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

        //console.log('3333');
        //console.log(val);
        brightcoveListener.onTemplateReady(val.id);
    });
}


brightcoveListener.log = function (eventType, event) {

    ////console.log(eventType + ' event');

    ////console.log('getMediaId');
    ////console.log(brightcoveListener.getMediaId(event));

    ////console.log('getPosition');
    ////console.log(brightcoveListener.getPosition(event));

    ////console.log('getDuration');
    ////console.log(brightcoveListener.getDuration(event));

    ////var a = brightcoveListener.getAdditionalParameters(event);
    ////console.log("additional parameters");
    ////console.log(a);

    ////console.log('getContainer parameters');
    ////console.log(brightcoveListener.getContainerParameters(event));
}

////brightcoveListener.getTotalTime = function (player) {
////    var playlist = player.playlist();
////    var index = player.playlist.currentItem();
////    var total = player.currentTime();

////    if (playlist !== undefined && index <= playlist.length) {
////        for (var i = 0; i < index; i++) {
////            total = total + playlist[i].duration;
////        }
////    }

////    return total;
////}

////brightcoveListener.getTotalDuration = function (player) {
////    var playlist = player.playlist();
////    var total = 0;

////    if (playlist !== undefined) {
////        for (var i = 0; i < playlist.length; i++) {
////            total = total + playlist[i].duration;
////        }
////    }

////    return total;
////}

brightcoveListener.onTemplateReady = function (playerId) {
    //console.log(playerId);
    videojs(playerId).ready(function () {
        var player = this;
        //console.log('player');
        //console.log(player);
        //////console.log(player.playlist());
        player.on('play', function (event) {
            
            if (player.catalog.data !== undefined) {
                var currentItem = event.target.player.playlist.currentItem();
                var playlist = event.target.player.playlist();

                if (currentItem >= 0 && playlist !== undefined && playlist.length > currentItem) {
                    var currentMediaInfo = playlist[currentItem];
                    if (currentMediaInfo === undefined) {
                        return;
                    }

                    if (event.target.player.mediainfo.id !== currentMediaInfo.id) {
                        brightcoveListener.log('change event', event);
                        brightcoveListener.onMediaChanged(event);
                    }
            
                    event.target.player.mediainfo = playlist[currentItem];
                }
            }

            brightcoveListener.log('play', event);
            brightcoveListener.onMediaEvent(event);
        });

        player.on('ended', function (event) {

            ////if (event.target.player.catalog.data !== undefined && event.target.player.playlist !== undefined) {
            ////    if (event.target.player.playlist.currentItem() == (event.target.player.playlist().length - 1)) {
            ////        brightcoveListener.onMediaEvent(event);
            ////    }
            //// return;
            ////}

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
    });
};

/*brightcoveListener.onTemplateReady = function (experienceModule) {
    var videoPlayer = experienceModule.target.experience.getModule(brightcove.api.modules.APIModules.VIDEO_PLAYER);

    videoPlayer.addEventListener(brightcove.api.events.MediaEvent.BEGIN, function (event) { brightcoveListener.onMediaEvent(event); });
    videoPlayer.addEventListener(brightcove.api.events.MediaEvent.COMPLETE, function (event) { brightcoveListener.onMediaEvent(event); });
    videoPlayer.addEventListener(brightcove.api.events.MediaEvent.ERROR, function (event) { brightcoveListener.onMediaEvent(event); });
    videoPlayer.addEventListener(brightcove.api.events.MediaEvent.PROGRESS, function (event) { brightcoveListener.onMediaEvent(event); });

    videoPlayer.addEventListener(brightcove.api.events.MediaEvent.CHANGE, function (event) { brightcoveListener.onMediaChanged(event); });
};*/

brightcoveListener.loadPlayerScripts();