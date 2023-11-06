using BACK_END_DIAZNATURALS.Model;

namespace BACK_END_DIAZNATURALS.DTO
{
    public class GetOrderDTO
    {
        public int IdOrder { get; set; }
        public int IdClient { get; set; }
        public string nitClient { get; set; } = null!;
        public string nameClient { get; set; } = null!;
        public string stateClient { get; set; } = null!;
        public string cityClient { get; set; } = null!;
        public string addressClient { get; set; } = null!;
        public string phoneClient { get; set; } = null!;
        public string nameContactClient { get; set; } = null!;
        public DateTime StartDateOrder { get; set; }
        public string ImageOrder { get; set; } = null!;
        public string StatusOrder { get; set; } = null!;
        public int TotalPriceOrder { get; set; }
        public DateTime DateOrderHistory { get; set; }


    }
}
