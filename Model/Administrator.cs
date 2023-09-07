using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BACK_END_DIAZNATURALS.Model;

public partial class Administrator
{
    
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdAdministrator { get; set; }

    public int IdCredential { get; set; }

    public string NameAdministrator { get; set; } = null!;

    public string EmailAdministrator { get; set; } = null!;

    public virtual Credential IdCredentialNavigation { get; set; } = null!;
}
