namespace BACK_END_DIAZNATURALS.Model
{
    public class GetCartDTO
    {
        public string name { get; set; } = null!;
        public string supplier { get; set; } = null!;
        public string presentation { get; set; } = null!;
        public int  quantity { get; set; }
        public string image { get; set; } = null!;
        public int price { get; set; }
    }
}
