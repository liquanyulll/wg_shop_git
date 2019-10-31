using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace wg_core.Domain
{
    public partial class ShopContext2 : DbContext
    {
        public ShopContext2()
        {
        }

        public ShopContext2(DbContextOptions<ShopContext2> options)
            : base(options)
        {
        }

        public virtual DbSet<t1_user_login_history> t1_user_login_history { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySql("Data Source=cdb-6b5bkzin.gz.tencentcdb.com;port=10033;Initial Catalog=wg_shop;User ID=wbadmin;Password=wbadmin123");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<t1_user_login_history>(entity =>
            {
                entity.HasKey(e => e.his_id)
                    .HasName("PRIMARY");

                entity.Property(e => e.his_id).HasColumnType("int(11)");

                entity.Property(e => e.ipaddress).HasColumnType("varchar(50)");

                entity.Property(e => e.login_time).HasColumnType("datetime");

                entity.Property(e => e.user_id).HasColumnType("int(11)");
            });
        }
    }
}
