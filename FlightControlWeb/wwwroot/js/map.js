
let firstTime = true;
let flagExist = 0;
let markers = [];
let map;
let options;
let flightPlanCoordinates = [];
let flightPath;

function initMap() {

    if (firstTime) {
        // Map options
        options = {
            zoom: 2,
            center: { lat: 42.3601, lng: -71.0589 },
            mapTypeId: google.maps.MapTypeId.TERRAIN
        };
        firstTime = false;

    }


    // New map
    map = new google.maps.Map(document.getElementById('map'), options);

    //click event after press the map

    google.maps.event.addListener(map, 'click', function (e) {
        if (flagExist == 1) {
            let elmnt = document.getElementById("flightDetail1");
            elmnt.remove();
            flagExist = 0;
            flightPath.setMap(null);
        }
    });
    function refeshMap() {

        // Loop through markers
        for (let i = 0; i < markers.length; i++) {
            // Add marker
            addMarker(markers[i]);
        }
    }

    refeshMap();

    // Add Marker Function
    function addMarker(props) {
        let marker = new google.maps.Marker({
            position: new coordinate(props.longitude, props.latitude),
            map: map,

        });


        // Set icon image
        marker.setIcon('/src/image/plane.png');


        // Check content
        let infoWindow = new google.maps.InfoWindow({
            content: `${props.flight_id} ${ props.company_name }`,
            object: props
        });
        //click event after press the map

        marker.addListener('click', function () {
            flagExist = 1;
            showTable(infoWindow.object.flight_id);
            flightPath.setMap(null);


        });

        marker.addListener('mouseover', () => infoWindow.open(map, marker))
        marker.addListener('mouseout', () => infoWindow.close())

    }
    function showTable(id) {
       // GET: api/FlightPlan/
        fetch('api/FlightPlan/' + id)
            .then(result => {

                return result.json();
            })
            .then(data => {
                addFlightDetail(data,id);
            })
            .catch(error => {
                alert(error);

            });



    }
    function addFlightDetail(user,id) {
        let dateLanding = getEndTime(user.segments, user.initial_Location.date_time);
        let segLength = user.segments.length;
        let output = '<div>Flights:</div>';
        output +=
            `<tr id="flightDetail1" style="font-size: small;">
                <th>${id}</th>
                <th>${user.initial_Location.longitude} , ${user.initial_Location.latitude}</th>
                <th>${user.segments[segLength - 1].longitude} , ${user.segments[segLength - 1].latitude}</th>
                <th>${user.initial_Location.date_time}</th>
                <th>${dateLanding}</th>
                <th>${user.company_Name}</th>
                <th>${user.passengers}</th>

              </tr>`
            ;


        document.getElementById('output').innerHTML = output;
        flightPlanCoordinates = [];
        flightPath = null;
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


function getAllFlight() {
    markers = [];
    fetch("https://localhost:44300/api/Flights?relative_to=2020-12-27 01:56:22Z")
        .then(result => {
            
            return result.json();
        })
        .then(data => {
            console.log(data);
            addDetailsFlights(data);
        })
        .catch(error => console.log(error));
}


function addDetailsFlights(data) {
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

                if (!user.is_external) {
                    outputMyFlight +=
                        `<div class="item clearfix" id="${user.flight_id}">
                            <div class="item__description" >${user.flight_id}</div>
                            
                            <div class="item__description" style="text-indent :1em">${user.company_name}</div>
                            <div class="right clearfix">
                                <div class="item__delete" style="text-indent :1em">
                                    <button class="item__delete--btn" onClick="reply_click('${user.flight_id}')"><i class="ion-ios-close-outline"></i></button>
                                </div>
                            </div>
                    </div>
                   </div>`;


                } else {
                    outputExFlight +=
                        ` <div class="item clearfix" id="${user.flight_id}">
                            <div class="item__description">${user.flight_id}</div>
                            <div class="item__description" style="text-indent :1em">${user.company_name}</div>
                        </div>
                       </div>`;
                }
            });
            document.getElementById('outputMyFlight').innerHTML = outputMyFlight;
            document.getElementById('outputExFlight').innerHTML = outputExFlight;
}
getAllFlight()

setInterval(function () {
    getAllFlight();
}, 3000);




function reply_click(id) {


    fetch('https://localhost:44300/api/Flights/'+ id, {
        method: 'DELETE',
        body: id
    }).then(result => {
            alert(id + ' delete from DB');
            let myobj = document.getElementById(id);
            myobj.remove();
           getAllFlight();
        
        })
        .catch(error => {
            alert(error);

        });


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


