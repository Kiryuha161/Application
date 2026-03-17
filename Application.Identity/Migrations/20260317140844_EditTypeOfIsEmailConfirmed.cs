using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Application.Identity.Migrations
{
    /// <inheritdoc />
    public partial class EditTypeOfIsEmailConfirmed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"
                ALTER TABLE identity.users 
                ALTER COLUMN ""IsEmailConfirmed"" TYPE boolean 
                USING (false);
                "
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"
                ALTER TABLE identity.users 
                ALTER COLUMN ""IsEmailConfirmed"" TYPE text 
                USING (CASE WHEN ""IsEmailConfirmed"" = true THEN 'true' ELSE 'false' END);
                "
            );
        }
    }
}