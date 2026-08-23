namespace backend.Models
{
    public class Comments
    {
        public int id {get; set;}
        public string title {get; set;}= string.Empty;
        public string content {get; set;} =string.Empty;

        public int? StockId { get; set; }
        public Stock? Stock { get; set; }
    }
}