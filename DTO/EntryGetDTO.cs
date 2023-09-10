namespace BACK_END_DIAZNATURALS.DTO
{
    public class EntryGetDTO
    {
        public int IdEntry { get; set; }

        public string name { get; set; } = null!;

        public string supplier { get; set; } = null!;

        public string presentation { get; set; }

        public DateTime DateEntry { get; set; }

        public int QuantityProductEntry { get; set; }


    }
}
