namespace ECommerceMVC.Helper.Excel
{
    public interface IExcel<T>
    {
        byte[] Export(List<T> items);
        List<T> Import(Stream fileStream);
    }
}
