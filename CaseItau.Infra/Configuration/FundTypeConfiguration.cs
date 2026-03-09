using CaseItau.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseItau.Infrastructure.Configuration;

public class FundTypeMap : IEntityTypeConfiguration<FundType>
{
    public void Configure(EntityTypeBuilder<FundType> builder)
    {
        builder.ToTable("TIPO_FUNDO");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("CODIGO")
            .IsRequired();

        builder.Property(x => x.Name)
            .HasColumnName("NOME")
            .IsRequired()
            .HasMaxLength(20);

        builder.HasMany(x => x.Funds)
            .WithOne(x => x.FundType)
            .HasForeignKey(x => x.FundTypeId);
    }
}
