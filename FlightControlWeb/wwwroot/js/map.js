

let flagExist = 0;
let markers = [];
let map;
let options;
let flightPlanCoordinates = [];
let flightPath;
function initMap() {
    let myLatLng = new google.maps.LatLng(0, -180);
    // Map options
    let options = {
        zoom: 2,
        center: { lat: 42.3601, lng: -71.0589 },
        mapTypeId: google.maps.MapTypeId.TERRAIN
    };

    // New map
    map = new google.maps.Map(document.getElementById('map'), options);

    //click event after press the map

    google.maps.event.addListener(map, 'click', function (e) {
        if (flagExist == 1) {
            let elmnt = document.getElementById("flightDetail1");
            elmnt.remove();
            flagExist = 0;
            initMap();
        }
    });


    // Loop through markers
    for (let i = 0; i < markers.length; i++) {
        // Add marker
        addMarker(markers[i]);
    }



    // Add Marker Function
    function addMarker(props) {
        let marker = new google.maps.Marker({
            position: new coordinate(props.initial_location.longitude, props.initial_location.latitude),
            map: map
        });


        // Set icon image
        marker.setIcon('/src/image/plane.png');


        // Check content
        let infoWindow = new google.maps.InfoWindow({
            content: `${props.id} ${props.company_name}`,
            object: props
        });
        //click event after press the map

        marker.addListener('click', function () {
            flagExist = 1;
            infoWindow.open(map, marker);
            showTable(infoWindow.object);
        });

    }
    function showTable(user) {
        let dateLanding = getEndTime(user.segments, user.initial_location.date_time);
        let segLength = user.segments.length;
        let output = '<div>Flights:</div>';
        output +=
            `<tr id="flightDetail1" style="font-size: small;">
                <th>${user.id}</th>
                <th>${user.initial_location.longitude} , ${user.initial_location.latitude}</th>
                <th>${user.segments[segLength - 1].longitude} , ${user.segments[segLength - 1].latitude}</th>
                <th>${user.initial_location.date_time}</th>
                <th>${dateLanding}</th>
                <th>${user.company_name}</th>
                <th>${user.passengers}</th>

              </tr>`
            ;

        document.getElementById('output').innerHTML = output;

        flightPlanCoordinates = createListPathCoord(user);

        flightPath = new google.maps.Polyline({
            path: flightPlanCoordinates,
            strokeColor: "#FF0000",
            strokeOpacity: 1.0,
            strokeWeight: 2
        });

        flightPath.setMap(map);
    }

}




document.getElementById('getUsers').addEventListener('click', getUsers);
function getUsers() {

    fetch('json.json')
        .then((res) => res.json())
        .then((data) => {
            //            let output = '<h2>User</h2>';
            let outputMyFlight =
                 `<div class="item clearfix">
                            <div class="item__description" ><h6>ID</h6></div>
                            
                            <div class="item__description" style="text-indent :1em"><h6>COMPANY</h6></div>
                            <div class="right clearfix">
                            </div>
                    </div>
                   </div>`;
            let outputExFlight =
                `<div class="item clearfix">
                            <div class="item__description" ><h6>ID</h6></div>
                            
                            <div class="item__description" style="text-indent :1em"><h6>COMPANY</h6></div>
                            <div class="right clearfix">
                            </div>
                    </div>
                   </div>`;
                 

            markers = data;
            initMap();
            data.forEach(function (user) {

                console.log(user);

                if (user.myFlight == 1) {
                    outputMyFlight +=
                        `<div class="item clearfix">
                            <div class="item__description" >${user.id}</div>
                            
                            <div class="item__description" style="text-indent :1em">${user.company_name}</div>
                            <div class="right clearfix">
                                <div class="item__delete" style="text-indent :1em">
                                    <button class="item__delete--btn" onClick="reply_click(${user.id})"><i class="ion-ios-close-outline"></i></button>
                                </div>
                            </div>
                    </div>
                   </div>`;


                } else {
                    outputExFlight +=
                        ` <div class="item clearfix" id="${user.id}">
                            <div class="item__description">${user.id}</div>
                            <div class="item__description" style="text-indent :1em">${user.company_name}</div>
                        </div>
                       </div>`;

                }


            });

            console.log(output);
            console.log(outputMyFlight);
            console.log(outputExFlight);

            document.getElementById('outputMyFlight').innerHTML = outputMyFlight;
            document.getElementById('outputExFlight').innerHTML = outputExFlight;
        })
}
function reply_click(clicked_id) {
    alert(clicked_id);

}
function getEndTime(seg, startTime) {
    let sumSeconds = 0;
    for (let i = 0; i < seg.length; i++) {
        sumSeconds += seg[i].timespan_seconds;
    }
    //server time
    let t = Date.parse(startTime);
    //millisecond
    t = t + sumSeconds * 1000;
    //object date
    let d = new Date(t);
    //utc date
    let now_utc = new Date(d.toUTCString().slice(0, -4));
    //convert to UTC time string
    let n = now_utc.toISOString();
    return n;
}
//fix that
function createListPathCoord(user) {
    let listCoord = [];
    //        // before
    //        for (let i = 0; i < user.segments.length; i++) {
    //          listCoord.push(new google.maps.LatLng(37.772323+i, -122.214897));
    //        }
    let i = 0;
    user.segments.forEach(function (item) {

        listCoord.push(new google.maps.LatLng(item.longitude, item.latitude));
    })
    return listCoord;
}


class coordinate {
    constructor(lat, lng) {
        this.lat = lat;
        this.lng = lng;
    }
}


