namespace BACK_END_DIAZNATURALS.Model;

public partial class OrderHistory
{
    public int IdOrder { get; set; }

    public int IdStatus { get; set; }

    public DateTime DateOrderHistory { get; set; }

    public virtual Order IdOrderNavigation { get; set; } = null!;

    public virtual Status IdStatusNavigation { get; set; } = null!;
}
