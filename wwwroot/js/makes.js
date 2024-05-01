"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/makesHub").build();

connection.start().then(function () {
    console.log("SignalR Connected");
}).catch(function (err) {
    return console.error(err.toString());
});
connection.on("MakeAdded", function (makeName, makeId) {
    
    console.log("Make added: " + makeName + " with ID: " + makeId);
    var tableBody = document.getElementById("makeTableBody");
    var newRow = document.createElement("tr");
    newRow.id = "make-" + makeId;
    newRow.innerHTML = `<td>${makeName}</td>
                        <td>
                            <a href="/Makes/Edit/${makeId}">Edit</a> |
                            <a href="/Makes/Details/${makeId}">Details</a> |
                            <a class="delete-link" data-id="${makeId}" href="#">Delete</a>
                        </td>`;
    tableBody.appendChild(newRow);
});

connection.on("MakeEdited", function (makeName, makeId) {
    console.log("Make edited: " + makeName + " with ID: " + makeId);
    
    var makeRow = document.getElementById("make-" + makeId);
    if (makeRow) {
        makeRow.getElementsByTagName('td')[0].textContent = makeName;
    }
});

connection.on("MakeDeleted", function (makeId) {
    
    console.log("Make deleted with ID: " + makeId);
    
    var makeRow = document.getElementById("make-" + makeId);
    if (makeRow) {
        makeRow.remove();
    }
});

document.addEventListener('click', function (event) {
    if (event.target.classList.contains('delete-link')) {
        event.preventDefault();
        var makeId = event.target.getAttribute('data-id');
        if (confirm('Are you sure you want to delete this make?')) {
            deleteMake(makeId);
        }
    }
});

function deleteMake(makeId) {
    fetch('/Makes/Delete/' + makeId, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
        }
    }).then(function (response) {
        if (response.ok) {
            console.log("Make deleted successfully.");
        } else {
            console.error("Failed to delete make.");
        }
    }).catch(function (error) {
        console.error("Error occurred while deleting make:", error);
    });
}

