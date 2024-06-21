using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NotesApiNext.Migrations
{
    /// <inheritdoc />
    public partial class DbUserPassToBytea : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"ALTER TABLE public.""Users"" ALTER COLUMN ""Password"" TYPE bytea USING ""Password""::bytea;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"ALTER TABLE public.""Users"" ALTER COLUMN ""Password"" TYPE text USING ""Password""::text;");
        }
    }
}
