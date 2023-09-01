using System;
using System.Collections.Generic;

namespace BACK_END_DIAZNATURALS.Model;

public partial class Credential
{
    public string Password { get; set; } = null!;

    public string SaltCredential { get; set; } = null!;

    public int IdAdministrator { get; set; }
    
    public virtual Administrator IdAdministratorNavigation { get; set; } = null!;
}
