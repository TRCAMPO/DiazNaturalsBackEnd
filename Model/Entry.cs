namespace BACK_END_DIAZNATURALS.Model;

public partial class Entry
{
    public int IdEntry { get; set; }

    public int IdProduct { get; set; }

    public DateTime DateEntry { get; set; }

    public int QuantityProductEntry { get; set; }

    public virtual Product IdProductNavigation { get; set; } = null!;
}
