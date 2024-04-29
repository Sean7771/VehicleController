"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/vehicleHub").build();

connection.start().then(function () {
    console.log("SignalR connected");
}).catch(function (err) {
    console.error("SignalR connection error: ", err.toString());
});

function updateVehicleStatus(vehicleId, status, distanceDriven, distanceReversed) {
    connection.invoke("UpdateVehicleStatus", vehicleId, status).catch(function (err) {
        console.error("Error invoking UpdateVehicleStatus: ", err.toString());
    });
}

connection.on("VehicleStatusUpdated", function (vehicleId, status, distanceDriven, distanceReversed) {
    updateVehicleStatusAndMessage(vehicleId, status, "Vehicle status updated successfully!");
    updateDistanceInfo(vehicleId, distanceDriven, distanceReversed);
});

function updateVehicleStatusAndMessage(vehicleId, status, message) {
    $(`#status-id-${vehicleId}`).html(status);
    $(`#message-${vehicleId}`).html(message);
}

function updateDistanceInfo(vehicleId, distanceDriven, distanceReversed) {
    $(`#distance-driven-${vehicleId}`).html(distanceDriven);
    $(`#distance-reversed-${vehicleId}`).html(distanceReversed);
}