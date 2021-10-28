namespace Net
{
    public class RequestPageDeliveryParams
    {
        public RequestPageDeliveryParams(int tenderId)
        {
            Page = 1;
            ItemsPerPage = 10;
            Id = tenderId;
        }

        public int Page { get; set; }
        public int ItemsPerPage { get; set; }
        public int Id { get; set; }
    }
}
