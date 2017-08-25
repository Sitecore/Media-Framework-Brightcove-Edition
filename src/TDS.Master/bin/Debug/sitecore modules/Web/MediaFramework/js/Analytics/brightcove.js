;
var brightcoveListener = new PlayerEventsListener();

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
    console.log('getMediaId');
    console.log(event.target.player.mediainfo.id);
    return event.target.player.mediainfo.id; //event.media.lineupId ? event.media.lineupId : event.media.id;
};

brightcoveListener.getPosition = function (event) {
    //console.log('current time');
    //console.log(event.target.player.currentTime());
    return event.target.player.currentTime();
};

brightcoveListener.getDuration = function (event) {
    //console.log('duration');
    //console.log(event.target.player.duration());
    return event.target.player.duration();
};

brightcoveListener.getContainer = function (args) {
    //console.log('args');
    //console.log(args);
    //console.log(args.target.player.id);
    console.log(this.jQuerySMF(args.target));
    return this.jQuerySMF(args.target).closest('.mf-player-container');
    //return this.jQuerySMF('#' + args.target.experience.id).closest('.mf-player-container');
};

brightcoveListener.getAdditionalParameters = function (event) {
    return {
        mediaId: event.target.player.mediainfo.id, //event.media.id,
        mediaName: event.target.player.mediainfo.name, //event.media.displayName,
        mediaLength: Math.round(event.target.player.mediainfo.duration) //Math.round(event.media.length / 1000)
    };
};

brightcoveListener.attachEvents = function () {

    this.jQuerySMF.each(this.jQuerySMF(".video-js"), function (i, val) {

        //console.log('22222');
        //console.log(val);

        brightcoveListener.onTemplateReady(val.id);
    });
    //return this.jQuerySMF('#' + args.target.experience.id).closest('.mf-player-container');
}

brightcoveListener.onTemplateReady = function (playerId) {
    videojs(playerId).ready(function () {
        var player = this;
        console.log(player);
        console.log(player.playlist());
        player.on('play', function (event) {
            //console.log('play event');
            //console.log(event);
            console.log(event.target.player.playlist());

            brightcoveListener.onMediaEvent(event);
        });

        /*player.on('ended', function (event) {
            //console.log('ended event');
            //console.log(event);
            brightcoveListener.onMediaEvent(event);
        });

        player.on('error', function (event) {
            //console.log('error event');
            //console.log(event);
            brightcoveListener.onMediaEvent(event);
        });

        player.on('timeupdate', function (event) {
            //console.log('timeupdate event');
            //console.log(event);
            brightcoveListener.onMediaEvent(event);
        });*/
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