"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/vehicleHub").build();

connection.start().then(function () {
    console.log("SignalR connected");
}).catch(function (err) {
    console.error("SignalR connection error: ", err.toString());
});

function driveVehicle(vehicleId) {
    connection.invoke("DriveVehicle", vehicleId).catch(function (err) {
        console.error("Error invoking DriveVehicle: ", err.toString());
    });
}

function stopVehicle(vehicleId) {
    connection.invoke("StopVehicle", vehicleId).catch(function (err) {
        console.error("Error invoking StopVehicle: ", err.toString());
    });
}

function reverseVehicle(vehicleId) {
    connection.invoke("ReverseVehicle", vehicleId).catch(function (err) {
        console.error("Error invoking ReverseVehicle: ", err.toString());
    });
}

connection.on("VehicleDriven", function (vehicleId) {
    
    var messageElement = document.getElementById("message-" + vehicleId);
    if (messageElement) {
        messageElement.innerText = "Vehicle driven successfully!";
    }
});

connection.on("VehicleStopped", function (vehicleId) {
    
    var messageElement = document.getElementById("message-" + vehicleId);
    if (messageElement) {
        messageElement.innerText = "Vehicle stopped successfully!";
    }
});

connection.on("VehicleReversed", function (vehicleId) {
    
    var messageElement = document.getElementById("message-" + vehicleId);
    if (messageElement) {
        messageElement.innerText = "Vehicle reversed successfully!";
    }
    
});