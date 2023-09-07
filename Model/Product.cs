using System;
using System.Collections.Generic;

namespace BACK_END_DIAZNATURALS.Model;

public partial class Product
{
    public int IdProduct { get; set; }

    public int IdSupplier { get; set; }

    public int IdPresentation { get; set; }

    public int IdCategory { get; set; }

    public string NameProduct { get; set; } = null!;

    public int PriceProduct { get; set; }

    public int QuantityProduct { get; set; }

    public string DescriptionProduct { get; set; } = null!;

    public string ImageProduct { get; set; } = null!;

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual Category IdCategoryNavigation { get; set; } = null!;

    public virtual Presentation IdPresentationNavigation { get; set; } = null!;

    public virtual Supplier IdSupplierNavigation { get; set; } = null!;
}
