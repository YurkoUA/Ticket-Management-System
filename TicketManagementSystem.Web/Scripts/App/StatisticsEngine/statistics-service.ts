class StatisticsService extends ServiceBase {
    public GetCharts(pageId?: number): JQueryXHR {
        return this.Get(`/api/Statistics/Charts/${pageId}`);
    }

    public GetChartData(chartId: number): JQueryXHR {
        return this.Get(`/api/Statistics/Data/`, { ChartId: chartId });
    }

    public GetPages(): JQueryXHR {
        return this.Get('/api/Statistics/Pages/');
    }
}