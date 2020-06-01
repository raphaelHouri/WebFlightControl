let firstTime = true;
let flagData ;
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
    //getAllFlight();
    //SetMarker();
}
function SetMarkerWithoutData() {
    if (markers.length == 0) {
        return;
    }
    removeMarkers();
    removeDetailsMarkers();
    markers = [];
}
function removeMarkers() {
    for (let i = 0; i < markerList.length; i++) {
        //Remove previous Marker.
        if (markerList[i] != null) {
            markerList[i].setMap(null);
        }
    }
    markerList = []
}
function removeDetailsMarkers() {
    markers.forEach(function (item) {
        let myobj = document.getElementById(item.flight_id);
        myobj.remove();
    });
}


function SetMarker() {

    removeMarkers();


    for (let i = 0; i < markers.length; i++) {

        //Set Marker on Map.
        let data = markers[i];
        //let myLatlng = new google.maps.LatLng(data.latitude, data.longitude);
        let marker = new google.maps.Marker({
            position: new coordinate(markers[i].latitude, markers[i].longitude),
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


        marker.addListener('click', function () {
            flagExist = 1;
            showTable(infoWindow.object.flight_id)
                .catch(alert);
            if (ExsitLine) {
                flightPath.setMap(null);
                ExsitLine = false;
            }
        });
        google.maps.event.addListener(map, 'click', function (e) {
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



async function showTable(id) { // (1)
        // GET: api/FlightPlan/
    let response = await fetch('api/FlightPlan/' + id); 

    if (response.status == 200) {
        let data = await response.json(); 
        await addFlightDetail(data, id)
    } else {
        throw new Error(response.status);

    }

}



//async function showTable(id) {
//    // GET: api/FlightPlan/
//    await fetch('api/FlightPlan/' + id)
//        .then(result => {

//            let object =  result.json();
//            return object;
//        })
//        .then(data => {
//            addFlightDetail(data, id);
//        })
//        .catch(error => {
//            alert(error);

//        });
//}

function addFlightDetail(user, id) {
    let dateLanding = getEndTime(user.segments, user.initial_location.date_time );
    let segLength = user.segments.length;
    let output = '<div>Flights:</div>';
    output +=
        `<tr id="flightDetail1" style="font-size: small;">
                    <th>${id}</th>
                    <th>${user.initial_location.latitude} , ${user.initial_location.longitude}</th>
                    <th>${user.segments[segLength - 1].longitude} , ${user.segments[segLength - 1].latitude}</th>
                    <th>${user.initial_location.date_time}</th>
                    <th>${dateLanding}</th>
                    <th>${user.company_Name}</th>
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
    ExsitLine = true;
}





async function getAllFlight() { 
    flagData = false
    let time = getUTCTime()

    let response = await fetch("https://localhost:44300/api/Flights?relative_to=" + time); 

    if (response.status == 200) {
        let data = await response.json(); 
        addDetailsFlights(data);
        SetMarker();
    } else {

        SetMarkerWithoutData()

    }

}

//function getAllFlight() {
//    flagData = false
//    let time = getUTCTime()
//    let flightsUrl = "api/Flights";
//    $.ajax({
//        type: "GET",
//        url: flightsUrl,
//        dataType: 'json',
//        data: {
//            relative_to: time
//        }, success: function (data) {
//            flagData = true
//            addDetailsFlights(data);
//            SetMarker();
//        },
//        error: function (error) {
//            console.log(error)
//            SetMarkerWithoutData()
//        }
//    });

//}


//function getAllFlight() {
//    let time = getUTCTime()
//    flagData = false

//    fetch("https://localhost:44300/api/Flights?relative_to=" + time)
//        .then(result => {

//            return result.json();
//        })
//        .then(data => {
//                flagData = true
//                console.log(data);
//                addDetailsFlights(data);
         

//        })
//        .catch(error => {
//            console.log(error)
//        });

//}


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


setInterval(function () {
    getAllFlight()

    //SetMarker();

}, 9000);




async function reply_click(id) { 

    let response = await  fetch('https://localhost:44300/api/Flights/' + id, {
        method: 'DELETE',
        body: id
    }); 

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
        } catch(error){
            alert(error)
        }
    }

}

//async function reply_click(id) {


//    await fetch('https://localhost:44300/api/Flights/' + id, {
//        method: 'DELETE',
//        body: id
//    }).then(result => {
//        alert(id + ' delete from DB');
//        //let myobj = document.getElementById(id);
//        //myobj.remove();
//        getAllFlight();

//        if (ExsitLine) {
//            flightPath.setMap(null);
//            ExsitLine = false;
//        }
//    })
//        .catch(error => {
//            alert(error);

//        });


//}
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
    var date1 = new Date().toISOString().slice(0, -5) + 'Z';
    date1 = date1.replace(/T/g, " ");


    return date1;
}

function createListPathCoord(user) {
    let listCoord = [];
    listCoord.push(new google.maps.LatLng(user.initial_location.latitude, user.initial_location.longitude));

    user.segments.forEach(function (item) {

        listCoord.push(new google.maps.LatLng(item.latitude, item.longitude));
    })
    return listCoord;
}


class coordinate {
    constructor(lat, lng) {
        this.lat = lat;
        this.lng = lng;
    }
}
