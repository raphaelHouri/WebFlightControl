let flagExist = 0;
let markers = [];
let markerList = [];
let map;
let options;
let flightPlanCoordinates = [];
let flightPath;
let ExsitLine = false;

function initMap() {

    // Map options
    options = {
        zoom: 2,
        center: { lat: 42.3601, lng: -71.0589 },
        mapTypeId: google.maps.MapTypeId.TERRAIN
    };

    // New map
    map = new google.maps.Map(document.getElementById('map'), options);

}
//clean markers that not supposed to appear
function setMarkerWithoutData() {
    if (markers.length == 0) {
        return;
    }
    removeMarkers();
    removeDetailsMarkers();
    markers = [];
}
//clean the list of markers
function removeMarkers() {
    for (let i = 0; i < markerList.length; i++) {
        //Remove previous Marker.
        if (markerList[i] != null) {
            markerList[i].setMap(null);
        }
    }
    markerList = []
}
//remove the display element of each flight
function removeDetailsMarkers() {
    markers.forEach(function (item) {
        let myobj = document.getElementById(item.flight_id);
        myobj.remove();
    });
}

//set the details off each marker we get
function setMarker() {

    removeMarkers();


    for (let i = 0; i < markers.length; i++) {

        //Set Marker on Map.
        let data = markers[i];
        //let myLatlng = new google.maps.LatLng(data.latitude, data.longitude);
        let marker = new google.maps.Marker({
            position: new Coordinate(markers[i].latitude, markers[i].longitude),
            //position: myLatlng,
            map: map,
            title: data.title
        });
        // Set icon image
        marker.setIcon('/src/image/plane.png');
        markerList.push(marker);

        //Create and open InfoWindow.
        let infoWindow = new google.maps.InfoWindow({
            content: `${data.flight_id} ${data.company_name}`,
            object: markers[i]
        });

        marker.addListener('mouseover', () => infoWindow.open(map, marker))
        marker.addListener('mouseout', () => infoWindow.close())

        //when we click on marker display the table flight
        marker.addListener('click', function () {
            flagExist = 1;
            showTable(infoWindow.object.flight_id)
                .catch(alert);
            if (ExsitLine) {
                flightPath.setMap(null);
                ExsitLine = false;
            }
        });
        //if append click on the map display clean marker elements 
        google.maps.event.addListener(map, 'click', function () {
            if (flagExist == 1) {
                let elmnt = document.getElementById("flightDetail1");
                elmnt.remove();
                flagExist = 0;
                if (ExsitLine) {
                    flightPath.setMap(null);
                    ExsitLine = false;
                }

            }
        });
    }
}



async function showTable(id) {
    // GET: api/FlightPlan/
    let response = await fetch('api/FlightPlan/' + id);
    //good response 
    if (response.status == 200) {
        let data = await response.json();
        await addFlightDetail(data, id)
    } else {
        //bad response 
        throw new Error(response.status);

    }

}


//add flight detail for the table details
function addFlightDetail(user, id) {
    let dateLanding = getEndTime(user.segments, user.initial_location.date_time);
    let segLength = user.segments.length;
    let output = '<div>Flights:</div>';
    output +=
    `<tr id="flightDetail1" style="font-size: small;">
        <th>${id}</th>
        <th>${user.initial_location.latitude} , ${user.initial_location.longitude}</th>
        <th>${user.segments[segLength - 1].longitude} ,
        ${user.segments[segLength - 1].latitude}</th>
        <th>${user.initial_location.date_time}</th>
        <th>${dateLanding}</th>
        <th>${user.company_Name}</th>
        <th>${user.passengers}</th>

        </tr>`
    ;
    document.getElementById('output').innerHTML = output;
    //list of coord for the path of the plain
    flightPlanCoordinates = createListPathCoord(user);
    flightPath = new google.maps.Polyline({
        path: flightPlanCoordinates,
        strokeColor: "#FF0000",
        strokeOpacity: 1.0,
        strokeWeight: 2
    });

    flightPath.setMap(map);
    ExsitLine = true;
}



async function getAllFlight() {
    //get current utc time
    let time = getUTCTime()
    //ask for all the planes that flight now
    let response = await fetch("/api/Flights?relative_to=" + time + "&sync_all");
    if (response.status == 200) {
        //create list flight from json file
        let data = await response.json();
        addDetailsFlights(data);
        setMarker();
    } else {
        setMarkerWithoutData()
    }
}


//updat the left side screen with the table of extrnal flight and internal flight
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
    removeDetailsMarkers();
    markers = data;

    data.forEach(function (user) {

        if (!user.is_external) {
            outputMyFlight +=
            `<div class="item clearfix" id="${user.flight_id}">
                 <div class="item__description" >${user.flight_id}</div>

                <div class="item__description" style="text-indent :1em">${user.company_name}</div>
                <div class="right clearfix">
                <div class="item__delete" style="text-indent :1em">
                    <button class="item__delete--btn" onClick="reply_click('${user.flight_id}')">
                    <i class="ion-ios-close-outline"></i></button>
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

//the main loop always running
setInterval(function () {
    getAllFlight()
}, 3000);

//delete when we click on  delete button
async function reply_click(id) {

    let response = await fetch('/api/Flights/' + id, {
        method: 'DELETE',
        body: id
    });
    //good respons the flight delete
    if (response.status == 200) {
        alert(id + ' delete from DB');
        getAllFlight();
        if (ExsitLine) {
            flightPath.setMap(null);
            ExsitLine = false;
        }
    } else {
        try {
            throw new Error(response.status);
        } catch (error) {
            alert(error)
        }
    }
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

function getUTCTime() {
    let date1 = new Date().toISOString().slice(0, -5) + 'Z';
    date1 = date1.replace(/T/g, " ");
    return date1;
}

//create the list of path flight 
function createListPathCoord(user) {
    let listCoord = [];
    listCoord.push(new google.maps.LatLng(user.initial_location.latitude,
        user.initial_location.longitude));

    user.segments.forEach(function (item) {

        listCoord.push(new google.maps.LatLng(item.latitude, item.longitude));
    })
    return listCoord;
}

class Coordinate {
    constructor(lat, lng) {
        this.lat = lat;
        this.lng = lng;
    }
}
