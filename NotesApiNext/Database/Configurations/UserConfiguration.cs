using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NotesApiNext.Models.User;

namespace NotesApiNext.Database.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(user => user.UserId);
            builder.Property(user => user.UserName).HasMaxLength(100);
            builder.Property(user => user.Email).HasMaxLength(100);
            builder.Property(user => user.Password).HasMaxLength(100);
        }
    }
}
