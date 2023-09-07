using System;
using System.Collections.Generic;

namespace BACK_END_DIAZNATURALS.Model;

public partial class Client
{
    public int IdClient { get; set; }

    public int IdCredential { get; set; }

    public string NitClient { get; set; } = null!;

    public string NameClient { get; set; } = null!;

    public string EmailClient { get; set; } = null!;

    public bool IsActiveClient { get; set; }

    public string AddressClient { get; set; } = null!;

    public string PhoneClient { get; set; } = null!;

    public string CityClient { get; set; } = null!;

    public string StateClient { get; set; } = null!;

    public string NameContactClient { get; set; } = null!;

    public virtual Credential IdCredentialNavigation { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
