namespace BACK_END_DIAZNATURALS.DTO
{
    public class ProductDTO
    {
        public int IdProduct { get; set; }

        public string name { get; set; } = null!;

        public string supplier { get; set; } = null!;

        public int price { get; set; }

        public int amount { get; set; }

        public string presentation { get; set; }

        public string category { get; set; }

        public string description { get; set; }

        public string image { get; set; }
    }
}
