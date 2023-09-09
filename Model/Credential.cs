namespace BACK_END_DIAZNATURALS.Model;

public partial class Credential
{
    public int IdCredential { get; set; }

    public string PasswordCredential { get; set; } = null!;

    public string SaltCredential { get; set; } = null!;

    public virtual ICollection<Administrator> Administrators { get; set; } = new List<Administrator>();

    public virtual ICollection<Client> Clients { get; set; } = new List<Client>();
}
