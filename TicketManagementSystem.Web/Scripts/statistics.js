$(document).ready(function () {
    google.charts.load('current', { 'packages': ['corechart'] });
    google.charts.setOnLoadCallback(startLoad);
});

function startLoad() {
    var statistics = [
        {
            url: "/Statistics/Colors",
            elementId: "colors-chart",
            title: "Квитки за кольорами"
        },
        {
            url: "/Statistics/Series",
            elementId: "series-chart",
            title: "Квитки за серіями"
        },
        {
            url: "/Statistics/HappyTickets",
            elementId: "tickets-chart",
            title: "Щасливі квитки"
        },
        {
            url: "/Statistics/TicketsByFirstNumber",
            elementId: "number-chart",
            title: "Квитки за першою цифрою"
        }
    ];

    statistics.forEach(function (item, index) {
        drawChart(item);
    });
}

function drawChart(stat) {
    $.get(stat.url, function (data) {
        var chartOptions = {
            title: stat.title,
            is3D: true
        };
        data = google.visualization.arrayToDataTable(JSON.parse(data));
        
        var chart = new google.visualization.PieChart(document.getElementById(stat.elementId));
        chart.draw(data, chartOptions);
    });
}