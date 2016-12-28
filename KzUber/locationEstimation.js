var timer;
var siteURL = "http://localhost:63685/KzUber/CommonWebMethods.aspx/";

$(document).ready(function(){
    var productName = window.location.search.split('?')[1];
    var carData = {"rideProduct": productName};
    localStorage.setItem('.json/rideName.json', JSON.stringify(carData));

    $("#requestRide").click(function(){
        if($("#destinationLatitude").val() === "" || $("#destinationLongitude").val() === "")
        {
            alert("Select Destination");
        }else
        {                   
            var obj = {"destLat": +$("#destinationLatitude").val(), "destLong": +$("#destinationLongitude").val()};
            localStorage.setItem('.json/userDestination.json', JSON.stringify(obj));
            window.location.href = "https://login.uber.com/oauth/v2/authorize?client_id=3ihOozZlsQwhat85XL3TH_MPaA5prGFu&response_type=code&scope=request+profile+request_receipt";
        }
    });

    $("#fareEstimate").click(function(){
        if($("#destinationLatitude").val() === "" || $("#destinationLongitude").val() === "")
        {
            alert("Select Destination");
        }else
        {           
            var destLat = +$("#destinationLatitude").val();
            var destLong = +$("#destinationLongitude").val();     
            getUserLocation(destLat,destLong,productName);        
        }
    });    
});

function getUserLocation(destLat,destLong,productName)
{
    navigator.geolocation.watchPosition(function (position) {


    var userLatitude = position.coords.latitude;
    var userLongitude = position.coords.longitude;    

    if (typeof timer === typeof undefined) {
        timer = setInterval(function () {                    
            getEstimatesForUserLocation(userLatitude, userLongitude, +$("#destinationLatitude").val(), +$("#destinationLongitude").val(), productName); 
        }, 10000);            
        
        getEstimatesForUserLocation(userLatitude, userLongitude, +$("#destinationLatitude").val(), +$("#destinationLongitude").val(), productName);         
    }
});

}

function getEstimatesForUserLocation(latitude, longitude, destinationLatitude, destinationLongitude, productName) {

    console.log("Requesting updated time estimate...");
    var dataString = "{'latitude':'" + latitude + "','longitude':'" + longitude + "','destinationLatitude':'" + destinationLatitude + "','destinationLongitude':'" + destinationLongitude + "'}";
    var name = productName;
    $.ajax({
        url: siteURL + "getEstimatesForUserLocation",
        type: "POST",
        data: dataString,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var product = result.d.prices.filter(function(val){                
                 return val.localized_display_name == name;
            });
            console.log(product);
            $("#fareEstimatePanel").removeClass("hiddenEstimate");
            $("#fareEstimatePanel").addClass("visibleEstimate");
            $("#showFareEstimate").html(product[0].estimate);
        },
        error: function (response) {
            alert("Sorry, Some techincal error occured");
        },
        failure: function (response) {
            alert("Sorry, Some techincal error occured");
        }
    });
}

