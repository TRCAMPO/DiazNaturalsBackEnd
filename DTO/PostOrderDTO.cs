namespace BACK_END_DIAZNATURALS.DTO
{
    public class PostOrderDTO
    {
        public int IdClient { get; set; }
        public DateTime StartDateOrder { get; set; }
        public List<AddCartDTO> AddCart { get; set; }
       // public string ImageOrder { get; set; } = null!;
    }
}
