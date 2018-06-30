$(document).ready(function () {
    $("div.container").addClass("container-fluid");
    $("div.container").removeClass("container");

    google.charts.load('current', { 'packages': ['corechart'] });
    google.charts.setOnLoadCallback(startLoad);
});

function startLoad() {
    loadPeriods();
    loadMonthly();
}

function loadPeriods() {
    $.ajax({
        url: "/api/Statistics/SummariesPeriods", success: function (data, textStatus) {
            if (textStatus === 204) {
                hidePeriodsChart();
            } else {
                drawPeriodsChart(getPeriodsDataTable(data));
            }
        }, error: function () {
            hidePeriodsChart();
        }
    });
}

function loadMonthly() {
    $.ajax({
        url: "/api/Statistics/Summaries", success: function (data, textStatus) {
            if (textStatus === 204) {
                hideMonthlyCharts();
            } else {
                drawLineChart(getMonthlyDataTable(data, "Tickets"), "tickets-monthly", "Темпи зростання кількості квитків", "blue");
                drawLineChart(getMonthlyDataTable(data, "HappyTickets"), "happy-tickets-monthly", "Темпи зростання кількості щасливих квитків", "green");
                drawLineChart(getMonthlyDataTable(data, "Packages"), "packages-monthly", "Темпи зростання кількості пачок", "red");
            }
        }, error: function () {
            hideMonthlyCharts();
        }
    });
}

function getPeriodsDataTable(dataArray) {
    var table = [["Період", "Квитків"]];

    for (var i in dataArray) {
        table.push([dataArray[i].Period, dataArray[i].Tickets]);
    }

    return google.visualization.arrayToDataTable(table);
}

function getMonthlyDataTable(dataArray, valueKey) {
    var table = [["Дата", "Квитків"]];

    for (var i in dataArray) {
        var date = new Date(dataArray[i].Date).toLocaleDateString();

        table.push([date, dataArray[i][valueKey]]);
    }

    return google.visualization.arrayToDataTable(table);
}

function drawPeriodsChart(dataTable) {
    var chart = new google.visualization.ColumnChart(document.getElementById("tickets-periods"));
    var opts = {
        title: "Кількість зібраних квитків за місяцями",
        legend: { position: 'none' }
    };

    chart.draw(dataTable, opts);
}

function drawLineChart(dataTable, elementId, title, color) {
    var chart = new google.visualization.LineChart(document.getElementById(elementId));
    var opts = {
        title: title,
        legend: { position: 'none' },
        colors: [color]
    };

    chart.draw(dataTable, opts);
}

function hidePeriodsChart() {
    $('#tickets-periods').hide();
}

function hideMonthlyCharts() {
    $('#tickets-monthly').hide();
    $('#happy-tickets-monthly').hide();
    $('#packages-monthly').hide();
}
