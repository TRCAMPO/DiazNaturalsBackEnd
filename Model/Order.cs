namespace BACK_END_DIAZNATURALS.Model;

public partial class Order
{
    public int IdOrder { get; set; }

    public int IdClient { get; set; }

    public DateTime StartDateOrder { get; set; }

    public string ImageOrder { get; set; } = null!;

    public int TotalPriceOrder { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();
    
    public virtual Client IdClientNavigation { get; set; } = null!;

    public virtual ICollection<OrderHistory> OrderHistories { get; set; } = new List<OrderHistory>();
}
