abstract class ServiceBase {
    public Get(url: string, data?: any): JQueryXHR {
        return $.get(url, data);
    }
}