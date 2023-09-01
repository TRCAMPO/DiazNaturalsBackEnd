using System;
using System.Collections.Generic;

namespace BACK_END_DIAZNATURALS.Model;

public partial class Administrator
{
    public int IdAdministrator { get; set; }

    public string NameAdministrator { get; set; } = null!;

    public string EmailAdministrator { get; set; } = null!;

    public virtual ICollection<Credential> Credentials { get; set; } = new List<Credential>();
}
