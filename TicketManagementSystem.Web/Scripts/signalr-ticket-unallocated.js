﻿$(function () {
    var ticketsHub = $.connection.ticketsHub;

    ticketsHub.client.removeTickets = function (idArray) {
        if (idArray != undefined && idArray.length > 0) {
            var removedCount = 0;

            for (var i in idArray) {
                var selector = "tr#ticket-" + idArray[i];

                if ($(selector).length) {
                    removedCount++;

                    $(selector).remove();
                    console.log("The ticket ID: " + idArray[i] + " has been removed from the list.");
                }
            }

            changeCount(removedCount);
        }
    };

    $.connection.hub.start();

    var changeCount = function (removedCount) {
        var count = parseInt($("#unallocated-count").html());

        if (count != undefined && !isNaN(count)) {
            count -= removedCount;
            console.log("Count: " + count);

            if (count > 0) {
                $("#unallocated-count").html(count);
                getSummary();
            }
            else {
                location.reload();
            }
        }
    };
});