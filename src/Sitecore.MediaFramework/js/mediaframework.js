;
var jQuerySMF = jQuerySMF || jQuery.noConflict(true);

(function ($) {
    $(document).ready(function () {
        $('head').append(' <link rel="Stylesheet" href="/sitecore modules/Web/MediaFramework/js/fancybox/jquery.fancybox-1.3.4.css">');
        $("a.mf_mediaLink").bind('click', function () {
            var width = $(this).attr('frwidth');
            var height = $(this).attr('frheight');
            $.fancybox({
                'width': parseInt(width),
                'height': parseInt(height),
                'href': $(this).attr('href'),
                'type': 'iframe',
                'autoScale': 'false',
                'scrolling': 'no'
            });
            return false;
        });

        //allows dragging fancybox window
        if ($.fn.draggable) {
            $('#fancybox-outer').draggable({ cursor: 'move' });
        }
    });
})(jQuerySMF);
;