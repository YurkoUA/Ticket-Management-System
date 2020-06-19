class StatisticsPage {
    public CurrentPageId: number = 0;
    public Name: string = null;
    public Charts: ChartInfo[] = [];
    public Pages: Page[] = [];

    public OpenedChart: ChartFilter = new ChartFilter();
}