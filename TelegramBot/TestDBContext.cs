using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace TelegramBot
{
    public partial class TestDBContext : DbContext
    {
        public TestDBContext()
        {
        }

        public TestDBContext(DbContextOptions<TestDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TblCheckBonusesUser> TblCheckBonusesUsers { get; set; }
        public virtual DbSet<TblTransaction> TblTransactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=MiniComp\\MSSQLSWRVER;Database=TestDB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<TblCheckBonusesUser>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tblCheckBonusesUser");

                entity.Property(e => e.AddBonusesDate).HasColumnType("datetime");

                entity.Property(e => e.CountProduct).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.NameInCheck)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.PriceSku).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.UserBonuses).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.UserPhoneNumber)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblTransaction>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tblTransactions");

                entity.Property(e => e.CheckCountSku).HasColumnName("check_count_sku");

                entity.Property(e => e.CheckDate)
                    .HasColumnType("datetime")
                    .HasColumnName("check_date");

                entity.Property(e => e.CheckFn)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("check_fn");

                entity.Property(e => e.CheckNum)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("check_num");

                entity.Property(e => e.CheckSumTotal)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("check_sum_total");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.Discount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.IdCheck).ValueGeneratedOnAdd();

                entity.Property(e => e.Messenger)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserChatId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserPhoneNumber)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
