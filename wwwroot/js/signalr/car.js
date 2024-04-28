"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/carsHub").build();

connection.start().then(function () {
    console.log("SignalR connected");
}).catch(function (err) {
    console.error("SignalR connection error: ", err.toString());
});

function createCar(car) {
    connection.invoke("CreateCar", car).catch(function (err) {
        console.error("Error invoking CreateCar: ", err.toString());
    });
}

function updateCar(car) {
    connection.invoke("UpdateCar", car).catch(function (err) {
        console.error("Error invoking UpdateCar: ", err.toString());
    });
}

connection.on("CarCreated", function (car) {
    console.log("New car created:", car);
    updateCarUI(car, "Car created successfully!");
});

connection.on("CarUpdated", function (car) {
    console.log("Car updated:", car);
    updateCarUI(car, "Car updated successfully!");
});
function updateCarUI(car, message) {
    // Logic to update the UI with the new or updated car
    console.log("UI updated:", car);
    console.log("Message:", message);

    // Reload the list of cars after a successful creation or update
    reloadCarList();
}

function reloadCarList() {
    // Send an AJAX request to get the updated list of cars
    $.get("/Cars/Index", function (response) {
        // Replace the current HTML content of the container with the updated list of cars
        $("#car-list-container").html(response);
    }).fail(function (xhr, status, error) {
        console.error("Error reloading car list:", error);
        // Optionally, handle the error here
    });
}

