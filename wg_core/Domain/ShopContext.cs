using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace wg_core.Domain
{
    public partial class ShopContext : DbContext
    {
        public ShopContext()
        {
        }

        public ShopContext(DbContextOptions<ShopContext> options)
            : base(options)
        {
        }

        public virtual DbSet<t0_system> t0_system { get; set; }
        public virtual DbSet<t1_user> t1_user { get; set; }
        public virtual DbSet<t1_user_attr> t1_user_attr { get; set; }
        public virtual DbSet<t1_user_moneykey> t1_user_moneykey { get; set; }
        public virtual DbSet<t2_product> t2_product { get; set; }
        public virtual DbSet<t2_product_attr> t2_product_attr { get; set; }
        public virtual DbSet<t2_product_detail_Img> t2_product_detail_Img { get; set; }
        public virtual DbSet<t2_product_spec> t2_product_spec { get; set; }
        public virtual DbSet<t2_product_type> t2_product_type { get; set; }
        public virtual DbSet<t3_user_product_invitation> t3_user_product_invitation { get; set; }
        public virtual DbSet<t3_user_product_invitation_ip> t3_user_product_invitation_ip { get; set; }
        public virtual DbSet<t4_order> t4_order { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql("Data Source=cdb-6b5bkzin.gz.tencentcdb.com;port=10033;Initial Catalog=wg_shop;User ID=wbadmin;Password=wbadmin123");
            }

            //            if (!optionsBuilder.IsConfigured)
            //            {
            //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
            //                optionsBuilder.UseMySql("Data Source=cdb-6b5bkzin.gz.tencentcdb.com;port=10033;Initial Catalog=wg_shop;User ID=wbadmin;Password=wbadmin123");
            //            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<t0_system>(entity =>
            {
                entity.HasKey(e => e.config_id)
                    .HasName("PRIMARY");

                entity.Property(e => e.config_id).HasColumnType("int(11)");

                entity.Property(e => e.desc).HasColumnType("varchar(200)");

                entity.Property(e => e.key)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.value)
                    .IsRequired()
                    .HasColumnType("varchar(500)");
            });

            modelBuilder.Entity<t1_user>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PRIMARY");

                entity.Property(e => e.UserId).HasColumnType("int(11)");

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.EncryptionType)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Mobile).HasColumnType("varchar(20)");

                entity.Property(e => e.Mobile_Valid).HasColumnType("varchar(2)");

                entity.Property(e => e.PassWord)
                    .IsRequired()
                    .HasColumnType("varchar(200)");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<t1_user_attr>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PRIMARY");

                entity.Property(e => e.UserId).HasColumnType("int(11)");

                entity.Property(e => e.Amount).HasColumnType("decimal(10,2)");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.t1_user_attr)
                    .HasForeignKey<t1_user_attr>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_t1_user_attr_UserId");
            });

            modelBuilder.Entity<t1_user_moneykey>(entity =>
            {
                entity.HasKey(e => e.mk_id)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.used_user_id)
                    .HasName("Fk_t1_user_moneykey");

                entity.Property(e => e.mk_id).HasColumnType("int(11)");

                entity.Property(e => e.created_time).HasColumnType("datetime");

                entity.Property(e => e.mony_key)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.price).HasColumnType("decimal(10,2)");

                entity.Property(e => e.used)
                    .IsRequired()
                    .HasColumnType("varchar(2)");

                entity.Property(e => e.used_ip).HasColumnType("varchar(50)");

                entity.Property(e => e.used_time).HasColumnType("datetime");

                entity.Property(e => e.used_user_id).HasColumnType("int(11)");

                entity.HasOne(d => d.used_user_)
                    .WithMany(p => p.t1_user_moneykey)
                    .HasForeignKey(d => d.used_user_id)
                    .HasConstraintName("Fk_t1_user_moneykey");

                entity.Property(e => e.plat)
                   .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<t2_product>(entity =>
            {
                entity.HasKey(e => e.ProductId)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.CreateUserId)
                    .HasName("Fk_t2_product_CreateUserId");

                entity.HasIndex(e => e.Pt_Id)
                    .HasName("Fk_t2_product_Pt_Id");

                entity.Property(e => e.ProductId).HasColumnType("int(11)");

                entity.Property(e => e.CreateUserId).HasColumnType("int(11)");

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.Deleted)
                    .IsRequired()
                    .HasColumnType("varchar(2)");

                entity.Property(e => e.Enabled)
                    .IsRequired()
                    .HasColumnType("varchar(2)");

                entity.Property(e => e.Invs)
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.LogImg).HasColumnType("varchar(200)");

                entity.Property(e => e.Price).HasColumnType("decimal(10,2)");

                entity.Property(e => e.ProductDesc).HasColumnType("varchar(200)");

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Pt_Id).HasColumnType("int(11)");

                entity.Property(e => e.Stock)
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.HasOne(d => d.CreateUser)
                    .WithMany(p => p.t2_product)
                    .HasForeignKey(d => d.CreateUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_t2_product_CreateUserId");

                entity.HasOne(d => d.Pt_)
                    .WithMany(p => p.t2_product)
                    .HasForeignKey(d => d.Pt_Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_t2_product_Pt_Id");
            });

            modelBuilder.Entity<t2_product_attr>(entity =>
            {
                entity.HasKey(e => e.ProductId)
                    .HasName("PRIMARY");

                entity.Property(e => e.ProductId).HasColumnType("int(11)");

                entity.Property(e => e.Click)
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Collection)
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.HasOne(d => d.Product)
                    .WithOne(p => p.t2_product_attr)
                    .HasForeignKey<t2_product_attr>(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_t2_product_attr_ProductId");
            });

            modelBuilder.Entity<t2_product_detail_Img>(entity =>
            {
                entity.HasKey(e => e.img_id)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.ProductId)
                    .HasName("FK_t2_product_detail_Img_ProductId");

                entity.Property(e => e.img_id).HasColumnType("int(11)");

                entity.Property(e => e.Enabled)
                    .IsRequired()
                    .HasColumnType("varchar(2)")
                    .HasDefaultValueSql("'Y'");

                entity.Property(e => e.ProductId).HasColumnType("int(11)");

                entity.Property(e => e.Sort)
                    .HasColumnType("int(10)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.img_alt).HasColumnType("varchar(50)");

                entity.Property(e => e.img_desc).HasColumnType("varchar(2000)");

                entity.Property(e => e.img_url)
                    .IsRequired()
                    .HasColumnType("varchar(200)");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.t2_product_detail_Img)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_t2_product_detail_Img_ProductId");
            });

            modelBuilder.Entity<t2_product_spec>(entity =>
            {
                entity.HasKey(e => e.config_id)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.ProductId)
                    .HasName("FK_t2_product_spec_ProductId");

                entity.Property(e => e.config_id).HasColumnType("int(11)");

                entity.Property(e => e.Desc).HasColumnType("varchar(200)");

                entity.Property(e => e.Key).HasColumnType("varchar(50)");

                entity.Property(e => e.ProductId).HasColumnType("int(11)");

                entity.Property(e => e.Value).HasColumnType("varchar(200)");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.t2_product_spec)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_t2_product_spec_ProductId");
            });

            modelBuilder.Entity<t2_product_type>(entity =>
            {
                entity.HasKey(e => e.Pt_Id)
                    .HasName("PRIMARY");

                entity.Property(e => e.Pt_Id).HasColumnType("int(11)");

                entity.Property(e => e.Deleted)
                    .IsRequired()
                    .HasColumnType("varchar(2)");

                entity.Property(e => e.Enabled)
                    .IsRequired()
                    .HasColumnType("varchar(2)");

                entity.Property(e => e.Pt_Name)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Sort).HasColumnType("int(11)");
            });

            modelBuilder.Entity<t3_user_product_invitation>(entity =>
            {
                entity.HasKey(e => e.inv_code)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.ProductId)
                    .HasName("Fk_t3_user_product_invitation_ProductId");

                entity.HasIndex(e => e.UserId)
                    .HasName("FK_t3_user_product_invitation");

                entity.Property(e => e.inv_code).HasColumnType("varchar(50)");

                entity.Property(e => e.ProductId).HasColumnType("int(11)");

                entity.Property(e => e.UserId).HasColumnType("int(11)");

                entity.Property(e => e.inv_count).HasColumnType("int(11)");

                entity.Property(e => e.need_inv_count).HasColumnType("int(11)");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.t3_user_product_invitation)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_t3_user_product_invitation_ProductId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.t3_user_product_invitation)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_t3_user_product_invitation");
            });

            modelBuilder.Entity<t3_user_product_invitation_ip>(entity =>
            {
                entity.HasIndex(e => e.inv_code)
                    .HasName("K1");

                entity.HasIndex(e => e.inv_ip)
                    .HasName("K2");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.count).HasColumnType("int(11)");

                entity.Property(e => e.inv_code)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.inv_ip)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.HasOne(d => d.inv_codeNavigation)
                    .WithMany(p => p.t3_user_product_invitation_ip)
                    .HasForeignKey(d => d.inv_code)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_t3_user_product_invitation_ip_inv_code");
            });

            modelBuilder.Entity<t4_order>(entity =>
            {
                entity.HasKey(e => e.order_id)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.ProductId)
                    .HasName("FK_t4_order_ProductId");

                entity.HasIndex(e => e.UserId)
                    .HasName("FK_t4_order_UserId");

                entity.HasIndex(e => e.order_guid)
                    .HasName("UQ_OrderGuid")
                    .IsUnique();

                entity.Property(e => e.order_id).HasColumnType("int(11)");

                entity.Property(e => e.Amt).HasColumnType("decimal(10,2)");

                entity.Property(e => e.ProductId).HasColumnType("int(11)");

                entity.Property(e => e.UserId).HasColumnType("int(11)");

                entity.Property(e => e.created_time).HasColumnType("datetime");

                entity.Property(e => e.order_guid)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.pay_way)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.t4_order)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_t4_order_ProductId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.t4_order)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_t4_order_UserId");
            });
        }
    }
}
