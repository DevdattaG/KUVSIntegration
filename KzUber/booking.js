var userLat;
var userLong;
var siteURL = "http://localhost:63685/KzUber/CommonWebMethods.aspx/";
var siteBaseURL = "http://localhost:63685/KzUber/";

$(document).ready(function(){
    var objLocation = JSON.parse(localStorage.getItem('.json/userDestination.json'));
    if(localStorage.getItem('.json/userSource.json') !== null && localStorage.getItem('.json/rideName.json') !== null)
    {
        var sourceLocation = JSON.parse(localStorage.getItem('.json/userSource.json'));
        userLat = sourceLocation.sourceLat;
        userLong = sourceLocation.sourceLong;
        var productNameData = JSON.parse(localStorage.getItem('.json/rideName.json'));
        getProductInformation(productNameData.rideProduct.toUpperCase(), userLat, userLong);
    }
    if(window.location.hash == "#_")
    {
        getAuthToken(window.location.search.split("code=")[1], objLocation.destLat, objLocation.destLong);
    }        
});

function getAuthToken(authCode, destLat, destLong)
{
    console.log("Fetching Authentication Token...");
    var dataString = "{'authCode':'" + authCode + "'}";
    $.ajax({
        url: siteURL + "getAuthToken",
        type: "POST",
        data: dataString,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            console.log(JSON.stringify(result.d));
            if (result.d > 0) {
                var timerData = { "expiryTime": result.d };
                localStorage.setItem('.json/timer.json', JSON.stringify(timerData));
                window.location.replace(siteBaseURL+"bookUber.html");
            } else {
                window.location.replace(siteBaseURL + "UberIntegration.html");
            }

        },
        error: function (response) {
            alert("Sorry, Some techincal error occured");
            window.location.replace(siteBaseURL + "UberIntegration.html");
        },
        failure: function (response) {
            alert("Sorry, Some techincal error occured");
            window.location.replace(siteBaseURL + "UberIntegration.html");
        }
    });
}

function getProductInformation(productName, lat, long){
    console.log("Fetching product information...");
    var dataString = "{'latitude':'" + lat + "','longitude':'" + long + "'}";
    var name = productName;
    $.ajax({
        url: siteURL + "getProductInformation",
        type: "POST",
        data: dataString,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var productData = result.d.products.filter(function(val){
                return val.display_name.toUpperCase() === productName.toUpperCase();
            });
            console.log(productData);
            var productInfoData = {"productInfo": productData[0]};
            localStorage.setItem('.json/productDataInfo.json', JSON.stringify(productInfoData));
        },
        error: function (response) {
            alert("Sorry, Some techincal error occured");
        },
        failure: function (response) {
            alert("Sorry, Some techincal error occured");
        }
    });

}


