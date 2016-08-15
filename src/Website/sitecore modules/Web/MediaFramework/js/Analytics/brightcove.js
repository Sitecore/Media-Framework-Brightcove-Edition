;
var brightcoveListener = new PlayerEventsListener();

brightcoveListener.getEventType = function (event) {
    switch (event.type) {
        case brightcove.api.events.MediaEvent.PROGRESS:
            return this.eventTypes.PlaybackChanged;
        case brightcove.api.events.MediaEvent.BEGIN:
            return this.eventTypes.PlaybackStarted;
        case brightcove.api.events.MediaEvent.COMPLETE:
            return this.eventTypes.PlaybackCompleted;
        case brightcove.api.events.MediaEvent.ERROR:
            return this.eventTypes.PlaybackError;
        default:
            return undefined;
    }
};

brightcoveListener.getMediaId = function (event) {
    return event.media.lineupId ? event.media.lineupId : event.media.id;
};

brightcoveListener.getPosition = function (event) {
    return event.position;
};

brightcoveListener.getDuration = function (event) {
    return event.duration;
};

brightcoveListener.getContainer = function (args) {
    return this.jQuerySMF('#' + args.target.experience.id).closest('.mf-player-container');
},

brightcoveListener.getAdditionalParameters = function (event) {
    return {
        mediaId: event.media.id,
        mediaName: event.media.displayName,
        mediaLength: Math.round(event.media.length / 1000)
    };
};

brightcoveListener.onTemplateReady = function (experienceModule) {
    var videoPlayer = experienceModule.target.experience.getModule(brightcove.api.modules.APIModules.VIDEO_PLAYER);
    
    videoPlayer.addEventListener(brightcove.api.events.MediaEvent.BEGIN, function (event) { brightcoveListener.onMediaEvent(event); });
    videoPlayer.addEventListener(brightcove.api.events.MediaEvent.COMPLETE, function (event) { brightcoveListener.onMediaEvent(event); });
    videoPlayer.addEventListener(brightcove.api.events.MediaEvent.ERROR, function (event) { brightcoveListener.onMediaEvent(event); });
    videoPlayer.addEventListener(brightcove.api.events.MediaEvent.PROGRESS, function (event) { brightcoveListener.onMediaEvent(event); });
    
    videoPlayer.addEventListener(brightcove.api.events.MediaEvent.CHANGE, function (event) { brightcoveListener.onMediaChanged(event); });
};