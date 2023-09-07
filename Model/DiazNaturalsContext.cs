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

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Credential> Credentials { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderHistory> OrderHistories { get; set; }

    public virtual DbSet<Presentation> Presentations { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Administrator>(entity =>
        {
            entity.HasKey(e => e.IdAdministrator).HasName("PK_Id_Administrator");

            entity.ToTable("ADMINISTRATORS");

            entity.HasIndex(e => e.EmailAdministrator, "UQ_Email_Administrator").IsUnique();

            entity.HasIndex(e => e.NameAdministrator, "UQ_Name_Administrator").IsUnique();

            entity.Property(e => e.IdAdministrator).HasColumnName("id_administrator");
            entity.Property(e => e.EmailAdministrator)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email_administrator");
            entity.Property(e => e.IdCredential).HasColumnName("id_credential");
            entity.Property(e => e.NameAdministrator)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name_administrator");

            entity.HasOne(d => d.IdCredentialNavigation).WithMany(p => p.Administrators)
                .HasForeignKey(d => d.IdCredential)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_IdCredential_Administrator");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => new { e.IdOrder, e.IdProduct }).HasName("PK_Id_Carts");

            entity.ToTable("CARTS");

            entity.Property(e => e.IdOrder).HasColumnName("id_order");
            entity.Property(e => e.IdProduct).HasColumnName("id_product");
            entity.Property(e => e.QuantityProductCart).HasColumnName("quantity_product_cart");

            entity.HasOne(d => d.IdOrderNavigation).WithMany(p => p.Carts)
                .HasForeignKey(d => d.IdOrder)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_IdOrder_Carts");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.Carts)
                .HasForeignKey(d => d.IdProduct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_IdProduct_Carts");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.IdCategory).HasName("PK_Id_Category");

            entity.ToTable("CATEGORIES");

            entity.HasIndex(e => e.NameCategory, "UQ_Name_Category").IsUnique();

            entity.Property(e => e.IdCategory).HasColumnName("id_category");
            entity.Property(e => e.NameCategory)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name_category");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.IdClient).HasName("PK_Id_Client");

            entity.ToTable("CLIENTS");

            entity.HasIndex(e => e.EmailClient, "UQ_Email_Client").IsUnique();

            entity.HasIndex(e => e.NameClient, "UQ_Name_Client").IsUnique();

            entity.HasIndex(e => e.NitClient, "UQ_Nit_Client").IsUnique();

            entity.Property(e => e.IdClient).HasColumnName("id_client");
            entity.Property(e => e.AddressClient)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("address_client");
            entity.Property(e => e.CityClient)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("city_client");
            entity.Property(e => e.EmailClient)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email_client");
            entity.Property(e => e.IdCredential).HasColumnName("id_credential");
            entity.Property(e => e.IsActiveClient).HasColumnName("is_active_client");
            entity.Property(e => e.NameClient)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("name_client");
            entity.Property(e => e.NameContactClient)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name_contact_client");
            entity.Property(e => e.NitClient)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nit_client");
            entity.Property(e => e.PhoneClient)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("phone_client");
            entity.Property(e => e.StateClient)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("state_client");

            entity.HasOne(d => d.IdCredentialNavigation).WithMany(p => p.Clients)
                .HasForeignKey(d => d.IdCredential)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_IdCredential_Client");
        });

        modelBuilder.Entity<Credential>(entity =>
        {
            entity.HasKey(e => e.IdCredential).HasName("PK_Credential");

            entity.ToTable("CREDENTIALS");

            entity.Property(e => e.IdCredential).HasColumnName("id_credential");
            entity.Property(e => e.PasswordCredential)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("password_credential");
            entity.Property(e => e.SaltCredential)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("salt_credential");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.IdOrder).HasName("PK_Id_Order");

            entity.ToTable("ORDERS");

            entity.Property(e => e.IdOrder).HasColumnName("id_order");
            entity.Property(e => e.IdClient).HasColumnName("id_client");
            entity.Property(e => e.StartDateOrder)
                .HasColumnType("datetime")
                .HasColumnName("start_date_order");

            entity.HasOne(d => d.IdClientNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.IdClient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_IdClient_Order");
        });

        modelBuilder.Entity<OrderHistory>(entity =>
        {
            entity.HasKey(e => new { e.IdOrder, e.IdStatus }).HasName("PK_Id_Order_History");

            entity.ToTable("ORDER_HISTORY");

            entity.Property(e => e.IdOrder).HasColumnName("id_order");
            entity.Property(e => e.IdStatus).HasColumnName("id_status");
            entity.Property(e => e.DateOrderHistory)
                .HasColumnType("datetime")
                .HasColumnName("date_order_history");

            entity.HasOne(d => d.IdOrderNavigation).WithMany(p => p.OrderHistories)
                .HasForeignKey(d => d.IdOrder)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_IdOrder_OrdHis");

            entity.HasOne(d => d.IdStatusNavigation).WithMany(p => p.OrderHistories)
                .HasForeignKey(d => d.IdStatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_IdStatus_OrdHis");
        });

        modelBuilder.Entity<Presentation>(entity =>
        {
            entity.HasKey(e => e.IdPresentation).HasName("PK_Id_Presentation");

            entity.ToTable("PRESENTATIONS");

            entity.HasIndex(e => e.NamePresentation, "UQ_Name_Presentation").IsUnique();

            entity.Property(e => e.IdPresentation).HasColumnName("id_presentation");
            entity.Property(e => e.NamePresentation)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name_presentation");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.IdProduct).HasName("PK_Id_Product");

            entity.ToTable("PRODUCTS");

            entity.HasIndex(e => e.ImageProduct, "UQ_Image_Product").IsUnique();

            entity.Property(e => e.IdProduct).HasColumnName("id_product");
            entity.Property(e => e.DescriptionProduct)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("description_product");
            entity.Property(e => e.IdCategory).HasColumnName("id_category");
            entity.Property(e => e.IdPresentation).HasColumnName("id_presentation");
            entity.Property(e => e.IdSupplier).HasColumnName("id_supplier");
            entity.Property(e => e.ImageProduct)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("image_product");
            entity.Property(e => e.NameProduct)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("name_product");
            entity.Property(e => e.PriceProduct).HasColumnName("price_product");
            entity.Property(e => e.QuantityProduct).HasColumnName("quantity_product");

            entity.HasOne(d => d.IdCategoryNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.IdCategory)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_IdCategory_Carts");

            entity.HasOne(d => d.IdPresentationNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.IdPresentation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_IdPresentation_Carts");

            entity.HasOne(d => d.IdSupplierNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.IdSupplier)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_IdSupplier_Carts");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.IdStatus).HasName("PK_Id_Status");

            entity.ToTable("STATUSES");

            entity.HasIndex(e => e.NameStatus, "UQ_Name_Status").IsUnique();

            entity.Property(e => e.IdStatus)
                .ValueGeneratedNever()
                .HasColumnName("id_status");
            entity.Property(e => e.NameStatus)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name_status");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.IdSupplier).HasName("PK_Id_Supplier");

            entity.ToTable("SUPPLIERS");

            entity.HasIndex(e => e.NameSupplier, "UQ_Name_Supplier").IsUnique();

            entity.HasIndex(e => e.NitSupplier, "UQ_Nit_Supplier").IsUnique();

            entity.Property(e => e.IdSupplier).HasColumnName("id_supplier");
            entity.Property(e => e.AddressSupplier)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("address_supplier");
            entity.Property(e => e.EmailSupplier)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email_supplier");
            entity.Property(e => e.NameSupplier)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("name_supplier");
            entity.Property(e => e.NitSupplier)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nit_supplier");
            entity.Property(e => e.PhoneSupplier)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("phone_supplier");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
