using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FicArchive.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkMetadata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdditionalTags",
                table: "Works",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Works",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Characters",
                table: "Works",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Fandoms",
                table: "Works",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Rating",
                table: "Works",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Relationships",
                table: "Works",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalTags",
                table: "Works");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Works");

            migrationBuilder.DropColumn(
                name: "Characters",
                table: "Works");

            migrationBuilder.DropColumn(
                name: "Fandoms",
                table: "Works");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Works");

            migrationBuilder.DropColumn(
                name: "Relationships",
                table: "Works");
        }
    }
}
