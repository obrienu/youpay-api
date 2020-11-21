namespace Youpay.API.Helpers
{
    public class UserTransactionsParams
    {
        public string Completed { get; set; } = "true";
        public string OrderDirection { get; set; }
        private const int MaxPageSize = 30;
        public int PageNumber { get; set; } = 1;
        private int pageSize = 10;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > MaxPageSize ? MaxPageSize : value; }
        }
        
    }
}