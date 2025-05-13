using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping
{
    public class SaleItemConfiguration : IEntityTypeConfiguration<SaleItem>
    {
        public void Configure(EntityTypeBuilder<SaleItem> builder)
        {
            builder.ToTable("SaleItem");

            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");

            builder.Property(u => u.ProductId).IsRequired();
            builder.Property(u => u.ProductName).IsRequired().HasMaxLength(500);
            builder.Property(u => u.Quantity).IsRequired();
            builder.Property(u => u.UnitPrice).IsRequired();
            builder.Property(u => u.Discount).HasDefaultValue(0);
            builder.Property(u => u.IsCancelled).IsRequired().HasDefaultValue(false);

            builder.Property<Guid>("SaleId").IsRequired();
        }

    }
}
