using System;
using System.Collections.Generic;

namespace BACK_END_DIAZNATURALS.Model;

public partial class Presentation
{
    public int IdPresentation { get; set; }

    public string NamePresentation { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
