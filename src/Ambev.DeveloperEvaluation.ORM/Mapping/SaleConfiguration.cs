using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping
{
    public class SaleConfiguration : IEntityTypeConfiguration<Sale>
    {

        public void Configure(EntityTypeBuilder<Sale> builder)
        {
            builder.ToTable("Sale");

            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");

            builder.Property(u => u.SaleNumber).IsRequired().HasMaxLength(50);
            builder.Property(u => u.SaleDate).IsRequired();
            builder.Property(u => u.CustomerId).IsRequired();
            builder.Property(u => u.CustomerName).IsRequired().HasMaxLength(500);
            builder.Property(u => u.BranchName).IsRequired().HasMaxLength(500);
            builder.Property(u => u.BranchId).IsRequired();
            builder.Property(u => u.TotalAmount).IsRequired();
            builder.Property(u => u.IsCancelled).IsRequired().HasDefaultValue(false);
        }
    }
}
