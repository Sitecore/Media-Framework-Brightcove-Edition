;

var jQuerySMF = jQuerySMF || jQuery.noConflict(true);

//Old browsers support.
//https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Object/getOwnPropertyNames
if (typeof Object.getOwnPropertyNames !== "function") {
    Object.getOwnPropertyNames = function(obj) {
        var properties = [];
        if (typeof obj === "object" && obj !== null) {
            for (var x in obj) {
                if (obj.hasOwnProperty(x)) {
                    properties.push(x);
                }
            }
        }

        return properties;
    };
}

function PlayerEventsListener() {
}

PlayerEventsListener.prototype = {
    jQuerySMF: window.jQuerySMF,

    getEventType: function (args) {
        return undefined;
    },

    getMediaId: function (args) {
        return undefined;
    },

    getPosition: function (args) {
        return undefined;
    },

    getDuration: function (args) {
        return undefined;
    },
    
    getContainer: function (args) {
        return undefined;
    },
    
    getContainerParameters: function (args) {
        var container = this.getContainer(args);
        if (container != undefined) {
            return this.jQuerySMF(container).data("mf-params");
        }
        return '';
    },

    getAdditionalParameters: function (args) {
        return {};
    },

    eventTypes: {
        PlaybackStarted: "PlaybackStarted",
        PlaybackCompleted: "PlaybackCompleted",
        PlaybackChanged: "PlaybackChanged",
        PlaybackError: "PlaybackError"
    },

    playbackEvents: {},
    raisedEvents: [],
    
    getEventKey: function(mediaId, eventType, parameter) {
        return mediaId + '|' + eventType + '|' + (parameter || '');
    },

    addRaisedEvent: function (mediaId, eventType, parameter) {
        if (!mediaId || !eventType) {
            return;
        }
        
        this.raisedEvents.push(this.getEventKey(mediaId, eventType, parameter));
    },
    
    removeRaisedEventsById: function (mediaId) {
        if (!mediaId) {
            return;
        }
        var i = this.raisedEvents.length;
        while (i--) {
            if (this.raisedEvents[i].indexOf(mediaId) == 0) {
                this.raisedEvents.splice(i, 1);
            }
        }
    },
    
    isEventRaised: function (mediaId, eventType, parameter) {
        if (!mediaId || !eventType) {
            return false;
        }

        var eventKey = this.getEventKey(mediaId, eventType, parameter);

        var i = this.raisedEvents.length;
        while (i--) {
            if (this.raisedEvents[i] === eventKey) {
                return true;
            }
        }
        return false;
    },
    
    getPlaybackParameters: function (mediaId, eventType) {
        var result = [];

        if (!mediaId || !eventType) {
            return result;
        }

        //console.log('this.playbackEvents-1');
        //console.log(this.playbackEvents);
        var events = this.playbackEvents[mediaId];
        //console.log('this.playbackEvents-2');
        //console.log(events);
        if (!events) {
            return result;
        }

        var params = events[eventType];
        if (!params) {
            return result;
        }

        for (var i = 0; i < params.length; i++) {
            var tmp = params[i];
            
            if (!this.isEventRaised(mediaId, eventType, tmp)) {
                result.push(tmp);
            }
        }

        return result;
    },
    
    getPlaybackChangedParameter : function(eventParams, position, duration) {
        for (var i = 0; i < eventParams.length; i++) {
            var parameter = eventParams[i];
            
            if (parameter[parameter.length - 1] === '%') {
                var percent = parseInt(parameter.substr(0, parameter.length - 1));
                
                if (percent <= (position / duration) * 100) {
                    return parameter;
                }
            } else {
                var time = parseInt(parameter);
                
                if (time <= position) {
                    return parameter;
                }
            }
        }
        
        return undefined;
    },
        
    onMediaEvent: function (args, eventType) {
        ////console.log(args);
        eventType = eventType || this.getEventType(args);

        //console.log('eventType');
        //console.log(eventType);
        var mediaId = this.getMediaId(args);
        //console.log('eventType-mediaid');
        //console.log(mediaId);
        if (eventType == undefined || mediaId == undefined) {
            return;
        }
        
        var eventParams = this.getPlaybackParameters(mediaId, eventType);
        //console.log('eventParams');
        //console.log(eventParams);
        if (!eventParams.length) {
            //console.log('dead');
            return;
        }

        var parameter = '';
        
        if (eventType === this.eventTypes.PlaybackChanged) {
            parameter = this.getPlaybackChangedParameter(eventParams, this.getPosition(args), this.getDuration(args));

            if (!parameter) {
                return;
            }
        }

        //console.log('send event');
        this.sendEvent(eventType, args, { eventParameter: parameter });
        this.addRaisedEvent(mediaId, eventType, parameter);
    },
    
    onMediaChanged : function (args) {
        this.removeRaisedEventsById(this.getMediaId(args));
    },

    sendEvent: function (eventType,eventArgs, parameters) {
        if (!eventType || !eventArgs) {
            return;
        }

        //console.log('pbefore');
        //console.log(parameters);

        parameters = parameters || {};

        parameters = this.jQuerySMF.extend(parameters || {}, this.getAdditionalParameters(eventArgs));

        //console.log('app');
        //console.log(parameters);

        var containerParams = this.parseParameters(this.getContainerParameters(eventArgs));
        if (Object.getOwnPropertyNames(containerParams).length) {
            parameters = this.jQuerySMF.extend(parameters, containerParams);
        }

        if (!parameters.contextItemId) {
            parameters.contextItemId = this.getItemId() || '00000000000000000000000000000000';
        }

        //console.log('pafter');
        //console.log(parameters);

        var webMethod = window.location.origin + "/sitecore modules/Web/MediaFramework/Analytics/PlayerEventsRecorder.asmx/RecordEvent";

        this.jQuerySMF.ajax({
            type: "POST",
            url: webMethod,
            data: "{ 'eventName': '" + eventType + "', 'parameters': '" + JSON.stringify(parameters) + "' }",
            contentType: "application/json; charset=utf-8",
            dataType: "json"
        });
    },
    
    parseParameters: function (query) {
        if (!query) {
            return {};
        }
        
        var obj = {};
        
        var arr = query.split('&');
        for (var i = 0; i < arr.length; i++) {
            var bits = arr[i].split('=');
            obj[bits[0]] = bits[1];
        }

        return obj;
    },

    getItemId : function () {
        var hfItemId;
        var url = '';
        var w = window;

        do {
            if (url == w.document.URL) {
                break;
            }
            url = w.document.URL;
            hfItemId = this.jQuerySMF('#MediaFramework_ItemId', w.document);
            w = window.parent;
        } while (w && !hfItemId.length);

        if (hfItemId != undefined && hfItemId.length) {
            return hfItemId.val();
        }
        
        return undefined;
    }
};