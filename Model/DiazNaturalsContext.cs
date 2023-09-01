using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BACK_END_DIAZNATURALS.Model;

public partial class DiazNaturalsContext : DbContext
{
    public DiazNaturalsContext()
    {
    }

    public DiazNaturalsContext(DbContextOptions<DiazNaturalsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Administrator> Administrators { get; set; }

    public virtual DbSet<Credential> Credentials { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=JOSE\\SQLEXPRESS; Database=DiazNaturals; Trusted_Connection=True; TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Administrator>(entity =>
        {
            entity.HasKey(e => e.IdAdministrator).HasName("PK_Administrator");

            entity.ToTable("ADMINISTRATOR");

            entity.HasIndex(e => e.EmailAdministrator, "UQ_Administrator_Email").IsUnique();

            entity.HasIndex(e => e.IdAdministrator, "UQ_Administrator_ID").IsUnique();

            entity.HasIndex(e => e.NameAdministrator, "UQ_Administrator_name").IsUnique();

            entity.Property(e => e.IdAdministrator)
                .ValueGeneratedNever()
                .HasColumnName("id_administrator");
            entity.Property(e => e.EmailAdministrator)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email_administrator");
            entity.Property(e => e.NameAdministrator)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name_administrator");
        });

        modelBuilder.Entity<Credential>(entity =>
        {
            entity.HasKey(e => e.Password).HasName("PK_Credential");

            entity.ToTable("CREDENTIALS");

            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.IdAdministrator).HasColumnName("id_administrator");
            entity.Property(e => e.SaltCredential)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("salt_credential");

            entity.HasOne(d => d.IdAdministratorNavigation).WithMany(p => p.Credentials)
                .HasForeignKey(d => d.IdAdministrator)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Administrator");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
