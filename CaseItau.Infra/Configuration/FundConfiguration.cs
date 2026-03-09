using CaseItau.Domain.Entities;
using CaseItau.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseItau.Infrastructure.Configuration;

public class FundConfiguration : IEntityTypeConfiguration<Fund>
{
    public void Configure(EntityTypeBuilder<Fund> builder)
    {
        builder.ToTable("FUNDO");

        builder.HasKey(x => x.Code);

        builder.Property(x => x.Code)
            .HasColumnName("CODIGO")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.Name)
            .HasColumnName("NOME")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.FundTypeId)
            .HasColumnName("CODIGO_TIPO")
            .IsRequired();

        builder.Property(x => x.GrossValue)
            .HasColumnName("PATRIMONIO");

        builder.OwnsOne(x => x.Document, doc =>
        {
            doc.Property(d => d.Value)
                .HasColumnName("CNPJ")
                .HasMaxLength(14)
                .IsRequired();
        });

        builder.HasOne(x => x.FundType)
            .WithMany(x => x.Funds)
            .HasForeignKey(x => x.FundTypeId);
    }
}