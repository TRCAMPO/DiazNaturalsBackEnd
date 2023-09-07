using System;
using System.Collections.Generic;

namespace BACK_END_DIAZNATURALS.Model;

public partial class Cart
{
    public int IdOrder { get; set; }

    public int IdProduct { get; set; }

    public int QuantityProductCart { get; set; }

    public virtual Order IdOrderNavigation { get; set; } = null!;

    public virtual Product IdProductNavigation { get; set; } = null!;
}
