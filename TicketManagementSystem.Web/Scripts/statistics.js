$(document).ready(function () {
    google.charts.load('current', { 'packages': ['corechart'] });
    google.charts.setOnLoadCallback(startLoad);
});

function startLoad() {
    var charts = [
        new Chart("by-serial-chart", "BySerial", "Serial", "Tickets", "Квитки за серіями", "Серія", "Квитків"),
        new Chart("by-color-chart", "ByColor", "Color", "Tickets", "Квитки за кольорами", "Колір", "Квитків"),
        new Chart("tickets-chart", "Tickets", "Type", "Count", "Звичайні/щасливі квитки", "Тип", "Квитків"),
        new Chart("by-number-chart", "ByFirstNumber", "Number", "Count", "Квитки за першою цифрою", "Цифра", "Квитків"),
        new Chart("happy-by-serial-chart", "HappyBySerial", "Serial", "Tickets", "Щасливі квитки за серіями", "Серія", "Квитків"),
        new Chart("happy-by-number-chart", "HappyByFirstNumber", "Number", "Count", "Щасливі квитки за першою цифрою", "Цифра", "Квитків"),
    ];

    for (var i in charts) {
        charts[i].Draw();
    }
}

function Chart(elementId, methodName, key, value, chartTitle, keyTitle, valueTitle) {
    this.ElementId = elementId;
    this.Url = "/api/Statistics/" + methodName;
    this.ChartTitle = chartTitle;
    this.Key = key;
    this.Value = value;
    this.KeyTitle = keyTitle;
    this.ValueTitle = valueTitle;

    this.ChartOptions = {
        title: this.ChartTitle,
        is3D: true
    };

    this.Draw = function () {
        var element = document.getElementById(this.ElementId);
        var opts = this.ChartOptions;

        this.GetDataArray(function (dataArray) {
            var dataTable = google.visualization.arrayToDataTable(dataArray);
            var chart = new google.visualization.PieChart(element);

            chart.draw(dataTable, opts);
        });
    };

    this.GetDataArray = function (callback) {
        var array = this.InitializeArray();
        var key = this.Key;
        var value = this.Value;
        var elem = this.ElementId

        $.ajax({
            url: this.Url, success: function (data) {
                for (var i in data) {
                    array.push([data[i][key], data[i][value]]);
                }

                callback(array);
            }, error: function () {
                $('#' + elem).hide();
            }
        });
    };

    this.InitializeArray = function () {
        return [[this.KeyTitle, this.ValueTitle]];
    };
}
