using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ApnaAahar.Repository.Models
{
    public partial class Orchard1Context : DbContext
    {
        public Orchard1Context(DbContextOptions<Orchard1Context> options)
            : base(options)
        {
        }
      
        public virtual DbSet<CommunityDetails> CommunityDetails { get; set; }
        public virtual DbSet<ContactRequest> ContactRequest { get; set; }
        public virtual DbSet<FarmerDetails> FarmerDetails { get; set; }
        public virtual DbSet<Otp> Otp { get; set; }
        public virtual DbSet<ProductListingData> ProductListingData { get; set; }
        public virtual DbSet<ProductType> ProductType { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CommunityDetails>(entity =>
            {
                entity.HasKey(e => e.CommunityId);

                entity.HasIndex(e => e.CommunityName)
                    .HasName("UQ__Communit__F71270193358456C")
                    .IsUnique();

                entity.Property(e => e.CommunityId).HasColumnName("community_id");

                entity.Property(e => e.CommunityName)
                    .HasColumnName("community_name")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ContactRequest>(entity =>
            {
                entity.Property(e => e.ContactRequestId).HasColumnName("contactRequest_id");

                entity.Property(e => e.BuyerId).HasColumnName("buyer_id");

                entity.Property(e => e.ProductListingId).HasColumnName("productListing_id");

                entity.HasOne(d => d.Buyer)
                    .WithMany(p => p.ContactRequest)
                    .HasForeignKey(d => d.BuyerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ContactRe__buyer__36B12243");

                entity.HasOne(d => d.ProductListing)
                    .WithMany(p => p.ContactRequest)
                    .HasForeignKey(d => d.ProductListingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ContactRe__produ__37A5467C");
            });

            modelBuilder.Entity<FarmerDetails>(entity =>
            {
                entity.HasKey(e => e.FarmerId);

                entity.Property(e => e.FarmerId)
                    .HasColumnName("farmer_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.CommunityId).HasColumnName("community_id");

                entity.Property(e => e.IsAccountDisabled).HasColumnName("isAccountDisabled");

                entity.Property(e => e.IsApproved).HasColumnName("isApproved");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Community)
                    .WithMany(p => p.FarmerDetails)
                    .HasForeignKey(d => d.CommunityId)
                    .HasConstraintName("FK__FarmerDet__commu__2E1BDC42");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.FarmerDetails)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__FarmerDet__user___2D27B809");
            });

            modelBuilder.Entity<Otp>(entity =>
            {
                entity.ToTable("OTP");

                entity.HasIndex(e => e.Otp1)
                    .HasName("UQ__OTP__CB3903D9E8DE2216")
                    .IsUnique();

                entity.Property(e => e.OtpId).HasColumnName("OTP_id");

                entity.Property(e => e.Otp1).HasColumnName("OTP");

                entity.Property(e => e.Timestamp)
                    .HasColumnName("timestamp")
                    .HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Otp)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OTP__user_id__3B75D760");
            });

            modelBuilder.Entity<ProductListingData>(entity =>
            {
                entity.HasKey(e => e.ProductListingId);

                entity.Property(e => e.ProductListingId).HasColumnName("productListing_id");

                entity.Property(e => e.FarmerId).HasColumnName("farmer_id");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.ProductTypeId).HasColumnName("productType_id");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.HasOne(d => d.Farmer)
                    .WithMany(p => p.ProductListingData)
                    .HasForeignKey(d => d.FarmerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ProductLi__farme__32E0915F");

                entity.HasOne(d => d.ProductType)
                    .WithMany(p => p.ProductListingData)
                    .HasForeignKey(d => d.ProductTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ProductLi__produ__33D4B598");
            });

            modelBuilder.Entity<ProductType>(entity =>
            {
                entity.Property(e => e.ProductTypeId).HasColumnName("productType_id");

                entity.Property(e => e.Msp).HasColumnName("msp");

                entity.Property(e => e.ProductType1)
                    .IsRequired()
                    .HasColumnName("productType")
                    .HasMaxLength(25)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => e.TypeId);

                entity.Property(e => e.TypeId).HasColumnName("type_id");

                entity.Property(e => e.UserRole1)
                    .IsRequired()
                    .HasColumnName("userRole")
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.HasIndex(e => e.Email)
                    .HasName("UQ__Users__AB6E6164E0866E74")
                    .IsUnique();

                entity.HasIndex(e => e.PhoneNumber)
                    .HasName("UQ__Users__A1936A6B1960B3AE")
                    .IsUnique();

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");

                entity.Property(e => e.Location)
                    .IsRequired()
                    .HasColumnName("location")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasColumnName("phone_number")
                    .HasMaxLength(13)
                    .IsUnicode(false);

                entity.Property(e => e.UserFullName)
                    .IsRequired()
                    .HasColumnName("user_full_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserRole).HasColumnName("userRole");

                entity.HasOne(d => d.UserRoleNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.UserRole)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Users__userRole__276EDEB3");
            });
        }
    }
}
