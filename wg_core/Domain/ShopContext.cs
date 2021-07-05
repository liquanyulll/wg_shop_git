using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

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

        public virtual DbSet<t0_system> t0_systems { get; set; }
        public virtual DbSet<t1_user> t1_users { get; set; }
        public virtual DbSet<t1_user_attr> t1_user_attrs { get; set; }
        public virtual DbSet<t1_user_login_history> t1_user_login_histories { get; set; }
        public virtual DbSet<t1_user_moneykey> t1_user_moneykeys { get; set; }
        public virtual DbSet<t2_product> t2_products { get; set; }
        public virtual DbSet<t2_product_detail_Img> t2_product_detail_Imgs { get; set; }
        public virtual DbSet<t2_product_spec> t2_product_specs { get; set; }
        public virtual DbSet<t2_product_type> t2_product_types { get; set; }
        public virtual DbSet<t3_user_product_invitation> t3_user_product_invitations { get; set; }
        public virtual DbSet<t3_user_product_invitation_ip> t3_user_product_invitation_ips { get; set; }
        public virtual DbSet<t4_order> t4_orders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("data source=sh-cynosdbmysql-grp-09q1dvti.sql.tencentcdb.com;port=25611;initial catalog=xnshop;user id=root;password=admin@123", Microsoft.EntityFrameworkCore.ServerVersion.Parse("5.7.18-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            modelBuilder.Entity<t0_system>(entity =>
            {
                entity.HasKey(e => e.config_id)
                    .HasName("PRIMARY");

                entity.ToTable("t0_system");

                entity.Property(e => e.config_id).HasColumnType("int(11)");

                entity.Property(e => e.desc).HasMaxLength(200);

                entity.Property(e => e.key).HasMaxLength(50);

                entity.Property(e => e.value).HasMaxLength(1000);
            });

            modelBuilder.Entity<t1_user>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PRIMARY");

                entity.ToTable("t1_user");

                entity.Property(e => e.UserId).HasColumnType("int(11)");

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.EncryptionType)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Mobile).HasMaxLength(50);

                entity.Property(e => e.Mobile_Valid).HasMaxLength(2);

                entity.Property(e => e.Money)
                    .HasPrecision(10)
                    .HasComment("余额");

                entity.Property(e => e.PassWord)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<t1_user_attr>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PRIMARY");

                entity.ToTable("t1_user_attr");

                entity.Property(e => e.UserId)
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.Amount).HasPrecision(11, 2);
            });

            modelBuilder.Entity<t1_user_login_history>(entity =>
            {
                entity.HasKey(e => e.his_id)
                    .HasName("PRIMARY");

                entity.ToTable("t1_user_login_history");

                entity.HasIndex(e => e.user_id, "FK_t1_user_login_history_userId");

                entity.Property(e => e.his_id).HasColumnType("int(11)");

                entity.Property(e => e.ipaddress).HasMaxLength(50);

                entity.Property(e => e.login_time).HasColumnType("datetime");

                entity.Property(e => e.user_id).HasColumnType("int(11)");

                entity.HasOne(d => d.user)
                    .WithMany(p => p.t1_user_login_histories)
                    .HasForeignKey(d => d.user_id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_t1_user_login_history_userId");
            });

            modelBuilder.Entity<t1_user_moneykey>(entity =>
            {
                entity.HasKey(e => e.mk_id)
                    .HasName("PRIMARY");

                entity.ToTable("t1_user_moneykey");

                entity.HasIndex(e => e.used_user_id, "FK_t1_user_moneykey_used_user_id");

                entity.Property(e => e.mk_id).HasColumnType("int(11)");

                entity.Property(e => e.created_time).HasColumnType("datetime");

                entity.Property(e => e.mony_key)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.plat).HasMaxLength(20);

                entity.Property(e => e.price).HasPrecision(8, 2);

                entity.Property(e => e.used)
                    .IsRequired()
                    .HasMaxLength(2);

                entity.Property(e => e.used_ip).HasMaxLength(20);

                entity.Property(e => e.used_time).HasColumnType("datetime");

                entity.Property(e => e.used_user_id).HasColumnType("int(11)");

                entity.HasOne(d => d.used_user)
                    .WithMany(p => p.t1_user_moneykeys)
                    .HasForeignKey(d => d.used_user_id)
                    .HasConstraintName("FK_t1_user_moneykey_used_user_id");
            });

            modelBuilder.Entity<t2_product>(entity =>
            {
                entity.HasKey(e => e.ProductId)
                    .HasName("PRIMARY");

                entity.ToTable("t2_product");

                entity.HasIndex(e => e.CreateUserId, "FK_t2_product_CreateUserId");

                entity.HasIndex(e => e.PtIds, "PtIDS");

                entity.Property(e => e.ProductId).HasColumnType("int(11)");

                entity.Property(e => e.Click)
                    .HasColumnType("int(11)")
                    .HasComment("点赞数");

                entity.Property(e => e.Collection)
                    .HasColumnType("int(11)")
                    .HasComment("收藏数");

                entity.Property(e => e.CreateUserId).HasColumnType("int(11)");

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.Deleted).HasMaxLength(2);

                entity.Property(e => e.Enabled).HasMaxLength(2);

                entity.Property(e => e.Invs)
                    .HasColumnType("int(11)")
                    .HasComment("邀请数");

                entity.Property(e => e.LogImg).HasMaxLength(100);

                entity.Property(e => e.Price)
                    .HasPrecision(8, 2)
                    .HasComment("售价");

                entity.Property(e => e.ProductDesc).HasMaxLength(200);

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.PtIds).HasMaxLength(50);

                entity.Property(e => e.Stock)
                    .HasColumnType("int(11)")
                    .HasComment("库存");

                entity.Property(e => e.UpdateTime).HasColumnType("datetime");

                entity.HasOne(d => d.CreateUser)
                    .WithMany(p => p.t2_products)
                    .HasForeignKey(d => d.CreateUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_t2_product_CreateUserId");
            });

            modelBuilder.Entity<t2_product_detail_Img>(entity =>
            {
                entity.HasKey(e => e.img_id)
                    .HasName("PRIMARY");

                entity.ToTable("t2_product_detail_Img");

                entity.HasIndex(e => e.ProductId, "FK_t2_product_detail_Img_ProductId");

                entity.Property(e => e.img_id).HasColumnType("int(11)");

                entity.Property(e => e.Enabled).HasMaxLength(2);

                entity.Property(e => e.ProductId).HasColumnType("int(11)");

                entity.Property(e => e.Sort).HasColumnType("int(11)");

                entity.Property(e => e.img_alt).HasMaxLength(100);

                entity.Property(e => e.img_desc).HasMaxLength(300);

                entity.Property(e => e.img_url).HasMaxLength(100);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.t2_product_detail_Imgs)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_t2_product_detail_Img_ProductId");
            });

            modelBuilder.Entity<t2_product_spec>(entity =>
            {
                entity.HasKey(e => e.config_id)
                    .HasName("PRIMARY");

                entity.ToTable("t2_product_spec");

                entity.HasIndex(e => e.ProductId, "FK_t2_product_spec_ProductId");

                entity.Property(e => e.config_id).HasColumnType("int(11)");

                entity.Property(e => e.Desc).HasMaxLength(100);

                entity.Property(e => e.Key)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ProductId).HasColumnType("int(11)");

                entity.Property(e => e.Value).HasMaxLength(500);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.t2_product_specs)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_t2_product_spec_ProductId");
            });

            modelBuilder.Entity<t2_product_type>(entity =>
            {
                entity.HasKey(e => e.Pt_Id)
                    .HasName("PRIMARY");

                entity.ToTable("t2_product_type");

                entity.HasIndex(e => e.Pt_ParentId, "FK_t2_product_type_Pt_ParentId");

                entity.Property(e => e.Pt_Id).HasColumnType("int(11)");

                entity.Property(e => e.Enabled).HasMaxLength(2);

                entity.Property(e => e.Pt_Name).HasMaxLength(50);

                entity.Property(e => e.Pt_ParentId).HasColumnType("int(11)");

                entity.Property(e => e.Sort).HasColumnType("int(11)");

                entity.HasOne(d => d.Pt_Parent)
                    .WithMany(p => p.InversePt_Parent)
                    .HasForeignKey(d => d.Pt_ParentId)
                    .HasConstraintName("FK_t2_product_type_Pt_ParentId");
            });

            modelBuilder.Entity<t3_user_product_invitation>(entity =>
            {
                entity.HasKey(e => e.inv_code)
                    .HasName("PRIMARY");

                entity.ToTable("t3_user_product_invitation");

                entity.HasIndex(e => e.ProductId, "FK_t3_user_product_invitation_ProductId");

                entity.HasIndex(e => e.UserId, "FK_t3_user_product_invitation_UserId");

                entity.Property(e => e.inv_code).HasMaxLength(20);

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.ProductId).HasColumnType("int(11)");

                entity.Property(e => e.UserId).HasColumnType("int(11)");

                entity.Property(e => e.inv_count).HasColumnType("int(11)");

                entity.Property(e => e.need_inv_count).HasColumnType("int(11)");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.t3_user_product_invitations)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_t3_user_product_invitation_ProductId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.t3_user_product_invitations)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_t3_user_product_invitation_UserId");
            });

            modelBuilder.Entity<t3_user_product_invitation_ip>(entity =>
            {
                entity.ToTable("t3_user_product_invitation_ip");

                entity.HasIndex(e => e.inv_code, "FK_inv_code");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.count).HasColumnType("int(11)");

                entity.Property(e => e.inv_code)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.inv_ip)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.inv_codeNavigation)
                    .WithMany(p => p.t3_user_product_invitation_ips)
                    .HasForeignKey(d => d.inv_code)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_inv_code");
            });

            modelBuilder.Entity<t4_order>(entity =>
            {
                entity.HasKey(e => e.order_id)
                    .HasName("PRIMARY");

                entity.ToTable("t4_order");

                entity.HasIndex(e => e.ProductId, "FK_t4_order_ProductId");

                entity.HasIndex(e => e.UserId, "FK_t4_order_UserId");

                entity.Property(e => e.order_id).HasColumnType("int(11)");

                entity.Property(e => e.Amt).HasPrecision(8, 2);

                entity.Property(e => e.ProductId).HasColumnType("int(11)");

                entity.Property(e => e.UserId).HasColumnType("int(11)");

                entity.Property(e => e.created_time).HasColumnType("datetime");

                entity.Property(e => e.order_guid)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.pay_way)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.t4_orders)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_t4_order_ProductId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.t4_orders)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_t4_order_UserId");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
