$(document).ready(getSummary);

function getSummary() {
    var showButton = $("#show-summary-button");
    showButton.popover({});

    $.ajax({
        url: showButton.attr("data-summary-url"),
        success: function (data, textStatus) {
            if (textStatus === 204) {
                $("#show-summary-button").attr("data-content", "Дані відсутні!");
            }
            else {
                if (data != undefined && data.length > 0) {
                    console.log(data);

                    var markup = createMarkup(data);
                    console.log(markup);

                    $("#show-summary-button").attr("data-content", markup);
                }
            }
        },
        error: function () {
            $("#show-summary-button").attr("data-content", "Помилка :(");
        }
    });
}

function createMarkup(data) {
    var ul = "<ul>";

    for (var i in data) {
        ul += "<li><b>" + data[i].Name + ":</b> " + data[i].Count;

        if (data[i].HappyCount > 0) {
            ul += " <i>(Щ: " + data[i].HappyCount + ")</i>";
        }

        ul += "</li>";
    }

    return ul += "</ul>"
}