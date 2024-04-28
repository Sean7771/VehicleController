"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/vehicleHub").build();

connection.start().then(function () {
    console.log("SignalR connected");
}).catch(function (err) {
    console.error("SignalR connection error: ", err.toString());
});

function updateVehicleStatus(vehicleId, status) {
    connection.invoke("UpdateVehicleStatus", vehicleId, status).catch(function (err) {
        console.error("Error invoking UpdateVehicleStatus: ", err.toString());
    });
}

connection.on("VehicleStatusUpdated", function (vehicleId, status) {
    updateVehicleStatusAndMessage(vehicleId, status, "Vehicle status updated successfully!");
    var messageElement = document.getElementById("message-" + vehicleId);
    if (messageElement) {
        messageElement.innerText = "Vehicle status updated successfully!";
    }
});

function updateVehicleStatusAndMessage(vehicleId, status, message) {
    $(`#status-id-${vehicleId}`).html(status);
    $(`#message-${vehicleId}`).html(message);
}