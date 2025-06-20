using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MySocialMedia.Models.Users;

namespace MySocialMedia.Configs;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.ToTable("Messages").HasKey(m => m.Id);
        builder.Property(m => m.Id).UseIdentityColumn();
    }
}
