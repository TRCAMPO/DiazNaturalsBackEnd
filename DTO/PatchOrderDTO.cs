namespace BACK_END_DIAZNATURALS.DTO
{
    public class PatchOrderDTO
    {
        public int IdOrder { get; set; }
        public string NameStatus { get; set; } = null!;
        public DateTime DateOrderHistory { get; set; }
    }
}
