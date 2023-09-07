using System;
using System.Collections.Generic;

namespace BACK_END_DIAZNATURALS.Model;

public partial class Supplier
{
    public int IdSupplier { get; set; }

    public string NitSupplier { get; set; } = null!;

    public string NameSupplier { get; set; } = null!;

    public string AddressSupplier { get; set; } = null!;

    public string PhoneSupplier { get; set; } = null!;

    public string EmailSupplier { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
