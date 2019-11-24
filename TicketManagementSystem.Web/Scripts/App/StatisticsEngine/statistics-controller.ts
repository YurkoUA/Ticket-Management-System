declare var google;

class StatisticsController {
    constructor(private business: StatisticsBusiness) {
    }

    public InitializePage(pageId?: number): void {
        this.ApplyBindings();
        this.BindEvents();

        google.charts.load('current', { 'packages': ['corechart'] });
        google.charts.setOnLoadCallback(() => this.business.OnPageOpen(pageId));
    }

    private LoadData(charts: ChartInfo[]): void {
        for (let c of charts) {
            this.CreateChartElement(c);
            this.business.GetChartData(c.Id);
        }
    }

    private CreateChartElement(chart: ChartInfo): void {
        const container = $("#charts-container");
        const chartElement = $(`<div id="chart-${chart.Id}" class="${chart.StyleClass}"></div>`)
        container.append(chartElement);
    }

    private DrawChart(chart: ChartInfo, data: any): void {
        const elementId = `chart-${chart.Id}`;
        const dataTable = google.visualization.arrayToDataTable(data);
        const chartType = ChartType[chart.Type];
        const googleChart = new google.visualization[chartType](document.getElementById(elementId));

        if (googleChart) {
            googleChart.draw(dataTable, {
                title: chart.Title,
                is3D: chart.Is3D
            });
        }
    }

    private ApplyBindings(): void {

    }

    private BindEvents(): void {
        this.business.OnChartListReceived = (c) => this.LoadData(c);
        this.business.OnChartDataReceived = (c, d) => this.DrawChart(c, d);
    }
}