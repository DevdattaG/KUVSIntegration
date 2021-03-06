var userLat;
var userLong;
var siteURL = "http://localhost:63685/KzUber/CommonWebMethods.aspx/";
var siteBaseURL = "http://localhost:63685/KzUber/";

$(document).ready(function(){
    var objLocation = JSON.parse(localStorage.getItem('.json/userDestination.json'));
    var objProductData = JSON.parse(localStorage.getItem('.json/productDataInfo.json'));    
    if(localStorage.getItem('.json/userSource.json') !== null)
    {
        var sourceLocation = JSON.parse(localStorage.getItem('.json/userSource.json'));
        userLat = sourceLocation.sourceLat;
        userLong = sourceLocation.sourceLong;
    }
    if(window.location.search.split("surge_confirmation_id=")[1] === undefined)
    {
        getRequestEstimate(objProductData.productInfo.product_id, objLocation.destLat, objLocation.destLong);        
    }else if(window.location.search.split("surge_confirmation_id=")[1] !== undefined)
    {
        console.log("Returned to page successfully after accepting higher surge rate ...");
        $('#loading').removeClass("showBooking");
        $('#loading').addClass("hideBooking");
        $('#bookingPage').removeClass("hideBooking");
        $('#bookingPage').addClass("showBooking");
        bookRideRequest(objProductData.productInfo.product_id, objLocation.destLat, objLocation.destLong, undefined, window.location.search.split("surge_confirmation_id=")[1]);
    }      
    $("#cancelRide").click(function(){
        cancelRideRequest();
    });
    $("#homeLink").click(function () {
        window.location.replace(siteBaseURL + "UberIntegration.html");        
    });
});


function getRefreshToken() {
    console.log("Getting refresh token...");    
    var requestURL = siteURL + "getRefreshToken";
    var timerLoop = JSON.parse(localStorage.getItem('.json/timer.json')).expiryTime;
    var requestStatus = setInterval(function () {
        var objTimer = JSON.parse(localStorage.getItem('.json/timer.json'));
        if (objTimer.expiryTime != null) {
            timerLoop = objTimer.expiryTime;
            $.ajax({
                url: requestURL,
                type: "POST",
                //                    data: dataString,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    console.log(result.d);
                    if (result.d > 0) {
                        var timerData = { "expiryTime": result.d };
                        localStorage.setItem('.json/timer.json', JSON.stringify(timerData));
                    }
                    // $("#showRideStatus").html(result.status);
                    // console.log(result.status);
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
        } else {
            clearInterval(requestStatus);
        }
    }, timerLoop);                  
}

function getRequestEstimate(productID, destLat, destLong)
{
        console.log("Fetching request estimate...");
        var productId = productID;
        var dataString = "{'productID':'" + productID + "','latitude':'" + userLat + "','longitude':'" + userLong + "','destLat':'" + destLat + "','destLong':'" + destLong + "'}";
        $.ajax({
            url: siteURL + "getRequestEstimate",
            type: "POST",
            data: dataString,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                // alert("Got the info");
                if (result.d !== null) {
                    console.log(JSON.stringify(result.d));
                    $('#loading').removeClass("showBooking");
                    $('#loading').addClass("hideBooking");
                    $('#bookingPage').removeClass("hideBooking");
                    $('#bookingPage').addClass("showBooking");
                    if (result.d.estimate !== null) {
                        console.log("Upfront fares not enabled... Checking for surge multiplier rate...");
                        console.log(result);
                        if (result.d.estimate.surge_confirmation_href === null) {
                            bookRideRequest(productId, destLat, destLong);
                        } else {
                            console.log("Redirecting for surge confirmation");
                            window.location.href = result.d.estimate.surge_confirmation_href;
                            console.log("Surge confirmed by user");
                        }
                    } else {
                        $("#fareTypeEstimate").html("Upfront Fare Estimate")
                        $(".modal-body #surgeMultiplierValue").html(result.d.fare.display);
                        $('#myModal')
                    .modal({ backdrop: 'static', keyboard: false })
                    .one('click', '[data-value]', function (e) {
                        if ($(this).data('value')) {
                            bookRideRequest(productId, destLat, destLong, result.d.fare.fare_id);
                        } else {
                            var timerData = { "expiryTime": null };
                            localStorage.setItem('.json/timer.json', JSON.stringify(timerData));
                            window.location.replace(siteBaseURL + "UberIntegration.html");
                        }
                    });

                    }
            } else {
                alert("Sorry, Some techincal error occured");
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

function bookRideRequest(productID, destLat, destLong, fareId, surgeConfirmationId)
{
    console.log("Requesting Ride...");
    var dataString;
    var notApplicable = "NULL";    
    if(fareId === undefined)
    {
        if(surgeConfirmationId === undefined) {
            dataString = "{'productID':'" + productID + "','latitude':'" + userLat + "','longitude':'" + userLong + "','destLat':'" + destLat + "','destLong':'" + destLong + "','fareId':'" + notApplicable + "','surgeConfirmationId':'" + notApplicable + "'}";                    
        }else {
            dataString = "{'productID':'" + productID + "','latitude':'" + userLat + "','longitude':'" + userLong + "','destLat':'" + destLat + "','destLong':'" + destLong + "','fareId':'" + notApplicable + "','surgeConfirmationId':'" + surgeConfirmationId + "'}";                                
        }

    }else {
        dataString = "{'productID':'" + productID + "','latitude':'" + userLat + "','longitude':'" + userLong + "','destLat':'" + destLat + "','destLong':'" + destLong + "','fareId':'" + fareId + "','surgeConfirmationId':'" + notApplicable + "'}";                
    }

    $.ajax({
        url: siteURL + "bookRideRequest",
        type: "POST",
        data: dataString,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            if (result.d !== null) {
                console.log(JSON.stringify(result));
                $("#showRideStatus").html(result.d.status);
                console.log(result.d.status);
                //  setTimeout(function() { autoAcceptRequest(result.d.request_id); },7000);    
                getRefreshToken();
                getRideRequestStatus(result.d.request_id);
            } else {
                alert("Sorry, Some techincal error occured");
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

function getRideRequestStatus(requestID)
{
    console.log("Getting ride request status...");
    var requestURL = siteURL + "getRideRequestStatus";
    var dataString = "{'requestID':'" + requestID + "'}";
    var requestStatus = setInterval(function () {
        $.ajax({
            url: requestURL,
            type: "POST",
            data: dataString,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.d !== null) {
                    console.log(result.d);
                    $("#showRideStatus").html(result.d.status);
                    if (result.d.status === "accepted") {
                        var obj = { "tripData": result.d };
                        localStorage.setItem('.json/tripData.json', JSON.stringify(obj));
                        clearInterval(requestStatus);
                        getUserRideMap(result.d.request_id);
                        showTripDetails(result.d);
                    }
                } else {
                    alert("Sorry, Some techincal error occured");
                    window.location.replace(siteBaseURL + "UberIntegration.html");
                }
                
                // $("#showRideStatus").html(result.status);
                // console.log(result.status);
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
    }, 7000);
}

function autoAcceptRequest(requestID)
{
    console.log("Accepting Request...");
    var dataString = "{'requestID':'" + requestID + "'}";      
    //var dataString = '{"status":"accepted"}';
    var requestURL = siteURL + "autoAcceptRequest";
        $.ajax({
        url: requestURL,
        type: "POST",
        data: dataString,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            console.log("Ride Accepted ...");          
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

function getRideReceiptData(requestID, trip)
{
    console.log("Getting trip status...");
    var requestURL = siteURL + "getRideReceiptData";
    var dataString = "{'requestID':'" + requestID + "'}";    
    var requestId = requestID;
    var tripInfo = trip;

    var receiptStatus = setInterval(function () {
        $.ajax({
            url: requestURL,
            type: "POST",
            data: dataString,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.d !== null) {
                    console.log(result.d);
                    if (result.d.status === "arriving") {
                        $("#showRideStatus").html("Your Uber is arriving now");
                    }
                    if (result.d.status === "in_progress") {
                        $("#showRideStatus").html("On Trip");
                    }
                    if (result.d.status === "completed") {
                        clearInterval(receiptStatus);
                        generateRideReceipt(requestId, tripInfo);
                    }
                } else {
                    alert("Sorry, Some techincal error occured");
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
    }, 10000);
}

function generateRideReceipt(requestID, tripInfo)
{
    console.log("Generating Ride Receipt...");  
    var trip = tripInfo;
    var requestURL = siteURL + "generateRideReceipt";
    var dataString = "{'requestID':'" + requestID + "'}";
    $.ajax({
        url: requestURL,
        type: "POST",
        data: dataString,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            if (result.d !== null) {
                console.log("Receipt Generated ...");
                console.log(result.d);
                var obj = { "receiptData": result.d };
                localStorage.setItem('.json/receiptData.json', JSON.stringify(obj));
                showUserReceipt(result.d, trip);
            } else {
                alert("Sorry, Some techincal error occured");
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

function showTripDetails(trip){    
    if(localStorage.getItem('.json/rideName.json') !== null)
    {
        var myRide = JSON.parse(localStorage.getItem('.json/rideName.json'));
        $("#showTripStatus").removeClass("hiddenEstimate");
        $("#showTripStatus").addClass("visibleEstimate");
        $("#cancelRideContainer").removeClass("hiddenEstimate");
        $("#cancelRideContainer").addClass("visibleEstimate");        
        $("#showRideStatus").html("Your Uber is arriving in "+ trip.eta +" min");
        $("#driverName").text(trip.driver.name.toUpperCase());
        $("#driverRating").text(trip.driver.rating);
        $("#driverPhone").text(trip.driver.phone_number);
        $("#rideType").text(myRide.rideProduct.toUpperCase());
        $("#driverImage").attr("src", trip.driver.picture_url);
        $("#rideMake").text(trip.vehicle.make.toUpperCase());
        $("#rideName").text(trip.vehicle.model.toUpperCase());
        $("#rideNumber").text(trip.vehicle.license_plate.toUpperCase());  
//        setTimeout(function() { autoUpdateRequestStatus(trip.request_id, "arriving"); },10000);
//        setTimeout(function() { autoUpdateRequestStatus(trip.request_id, "in_progress"); },13000); 
//        setTimeout(function() { autoUpdateRequestStatus(trip.request_id, "completed"); },20000); 
        getRideReceiptData(trip.request_id, trip);
    } 
}

function getUserRideMap(requestID)
{
    console.log("Getting User Map ...");    
    var dataString = "{'requestID':'" + requestID + "'}";
    $.ajax({
        url: siteURL + "getUserRideMap",
        type: "POST",
        data: dataString,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            if (result.d !== null) {
                console.log("User ride map received ...");
                console.log(result.d);
                showUserRideMap(result.d.href);
            } else {
                alert("Sorry, Some techincal error occured");
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

function showUserRideMap(mapLink)
{
    $("#showUberRideMap").removeClass("hiddenEstimate");
    $("#showUberRideMap").addClass("visibleEstimate");
    $("#showUberRideMap").attr("src", mapLink);
}

function autoUpdateRequestStatus(requestID, updateStatus)
{
    console.log("Changing Request Status to : " + updateStatus + " ...");
    var dataString = "{'requestID':'" + requestID + "','updateStatus':'" + updateStatus + "'}";    
    //var dataString = '{"status":"'+updateStatus+'"}';
    var requestURL = siteURL + "autoUpdateRequestStatus";
        $.ajax({
            url: requestURL,
            type: "POST",
            data: dataString,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
        success: function (result) {
            console.log("Status changed to : "+ updateStatus);          
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

function showUserReceipt(receipt, trip)
{
        var myRide = JSON.parse(localStorage.getItem('.json/rideName.json'));
        $("#titleRow").html("Trip Receipt");
        $("#descriptionRow").html("Thank you for riding with us");
        $("#rideStatusRow").html("Receipt");
        $("#showRideStatus").html("Total : "+receipt.total_charged);
        $("#showTripStatus").removeClass("visibleEstimate");
        $("#showTripStatus").addClass("hiddenEstimate");
        $("#cancelRideContainer").removeClass("visibleEstimate");
        $("#cancelRideContainer").addClass("hiddenEstimate");
        $("#receiptPage").removeClass("hiddenEstimate");
        $("#receiptPage").addClass("visibleEstimate");
        $("#homeContainer").removeClass("hiddenEstimate");
        $("#homeContainer").addClass("visibleEstimate");

        $("#receiptRodeWith").html("You rode with "+trip.driver.name);

        $("#receiptDistance").text(receipt.distance + " " +receipt.distance_label);
        $("#receiptTime").text(receipt.duration);
        $("#receiptProduct").html(myRide.rideProduct.toUpperCase());
        $("#receiptVehicle").html(trip.vehicle.make + " " + trip.vehicle.model);

        receipt.charges.forEach(function(val){
            $("#showFareBreakdown").append("<div><p style='text-align:center; color:#6e6e6e; float:right;'><b>"+val.amount+"</b></p><p style='color:#6e6e6e;'><b>"+val.name+"</b></p></div>");
        });        
        receipt.charge_adjustments.forEach(function(val){
            $("#showFareBreakdown").append("<div><p style='text-align:center; color:#6e6e6e; float:right;'><b>"+val.amount+"</b></p><p style='color:#6e6e6e;'><b>"+val.name+"</b></p></div>");
        });
        $("#showFareBreakdown").append("<hr style='height:1px; border:none; color:#333; background-color:#333;' />");
        $("#showFareBreakdown").append("<div><p style='text-align:center; color:black; float:right;'><b id='receiptSubtotal'>" + receipt.total_charged + "</b></p><p style='color:black;'><b>Subtotal</b></p></div>");
        var timerData = { "expiryTime": null };
        localStorage.setItem('.json/timer.json', JSON.stringify(timerData));
}

function cancelRideRequest()
{
    console.log("Cancelling Ride...");
    var objRide = JSON.parse(localStorage.getItem('.json/tripData.json'));
    var dataString = "{'requestID':'" + objRide.tripData.request_id + "'}";
    $.ajax({
        url: siteURL + "cancelRideRequest",
        type: "POST",
        data: dataString,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            if (result.d > 0) {
                console.log("Ride cancelled successfully : " + result);
                var timerData = { "expiryTime": null };
                localStorage.setItem('.json/timer.json', JSON.stringify(timerData));
                window.location.replace(siteBaseURL + "UberIntegration.html");
            }            
        },
        error: function (response) {
            alert("Sorry, Some techincal error occured");
        },
        failure: function (response) {
            alert("Sorry, Some techincal error occured");
        }
    });
}
