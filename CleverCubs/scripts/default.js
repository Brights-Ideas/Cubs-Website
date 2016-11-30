$(document).ready(function () {

    initMaps();
});

function initMaps(){
 if ($('#map_canvas').length) {
            var latlng = new google.maps.LatLng(52.635126, 0.147150);
            var myOptions = {
                zoom: 10,
                center: latlng,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };
            var map = new google.maps.Map(document.getElementById("map_canvas"), myOptions);

            var contentString = '<div class="innerGmap"><strong>Elm Lodge Kennels</strong> <br/>' +
             '8 Belt Drove Begdale Elm,<br />' +
             'Wisbech<br /> PE14 0BA</div>';

            var infowindow = new google.maps.InfoWindow({
                content: contentString
            });

            var marker = new google.maps.Marker({
                position: latlng,
                title: "Elm Lodge"
            });

            // To add the marker to the map, call setMap();
            marker.setMap(map);
            google.maps.event.addListener(marker, 'click', function() {
                infowindow.open(map, marker);
            });
        }
	
		google.maps.event.addDomListener(window, 'resize', initMaps);
		google.maps.event.addDomListener(window, 'load', initMaps);
}

/* Rotating Panel */
var rotatingPanel = function () {

    var panels = [];

    $('.rotating-panel').each(function (i) {
        panels.push($(this));
    })

    if (panels.length > 0) {
        function StartTimer(position) {
            $(document).everyTime('4s', 'rotating-panel', function () { rotatingPanel.rotate(position); }, 0);
        }

        function StopTimer() {
            $(document).stopTime('rotating-panel');
        }

        StartTimer(2);

        $('#slidePager a').click(function (e) {
            $('#slidePager a').removeClass("active");
           $(this).addClass("active");

            var newPosition = $(this).attr('href').replace('?position=', '');
            rotatingPanel.rotate(newPosition);

            e.preventDefault();
        });
    }

    return {
        currentIndex: 0,
        rotate: function (direction) {
            StopTimer();
            direction = direction - 1;
            var oldIndex = this.currentIndex;

            if (direction < (panels.length) & direction >= 0) {
                this.currentIndex = direction;

                if (this.currentIndex < 0) {
                    this.currentIndex = (panels.length - 1);
                }
            }
            else {
                this.currentIndex++;

                if (this.currentIndex > (panels.length - 1)) {
                    this.currentIndex = 0;
                }
            }

            if (oldIndex != this.currentIndex) {
                $('#slidePager a').removeClass("active");
                $('#slidePager a').eq(this.currentIndex).addClass("active");    

                panels[oldIndex].fadeOut(1000);
                panels[this.currentIndex].fadeIn(1000, function () { StartTimer(this.currentIndex + 1); });
            }
        }
    }
} ();

