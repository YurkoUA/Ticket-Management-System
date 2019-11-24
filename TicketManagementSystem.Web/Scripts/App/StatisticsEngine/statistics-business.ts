class StatisticsBusiness {
    public Model: StatisticsPage = new StatisticsPage();
    private pageId?: number;

    public OnChartListReceived: (charts: ChartInfo[]) => void;
    public OnChartDataReceived: (chart: ChartInfo, data: any) => void;

    constructor(private service: StatisticsService) {
    }

    public OnPageOpen(pageId?: number): void {
        this.pageId = pageId;
        this.GetCharts();
    }

    public GetCharts(): void {
        this.service.GetCharts(this.pageId)
            .done((charts: ChartInfo[]) => {
                this.Model.Charts = charts;
                this.OnChartListReceived(charts);
            });
    }

    public GetChartData(chartId: number): void {
        this.service.GetChartData(chartId)
            .done((data: ChartData) => {
                // TODO: Array Extensions.
                let chart = this.Model.Charts.filter(c => c.Id == chartId)[0];
                this.OnChartDataReceived(chart, this.GetDataArray(chart, data));
            });
    }

    public GetDataArray(chart: ChartInfo, data: ChartData): any {
        let dataArray: any = [[chart.KeyTitle, chart.ValueTitle]];

        for (let item of data.Data) {
            dataArray.push([item.Name, item.Count])
        }

        return dataArray;
    }
}