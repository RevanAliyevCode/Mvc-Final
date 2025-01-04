using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class SelfReference : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_comments_news_news_ıd",
                table: "comments");

            migrationBuilder.AlterColumn<int>(
                name: "news_ıd",
                table: "comments",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "parent_comment_ıd",
                table: "comments",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ıx_comments_parent_comment_ıd",
                table: "comments",
                column: "parent_comment_ıd");

            migrationBuilder.AddForeignKey(
                name: "fk_comments_comments_parent_comment_ıd",
                table: "comments",
                column: "parent_comment_ıd",
                principalTable: "comments",
                principalColumn: "ıd",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_comments_news_news_ıd",
                table: "comments",
                column: "news_ıd",
                principalTable: "news",
                principalColumn: "ıd",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_comments_comments_parent_comment_ıd",
                table: "comments");

            migrationBuilder.DropForeignKey(
                name: "fk_comments_news_news_ıd",
                table: "comments");

            migrationBuilder.DropIndex(
                name: "ıx_comments_parent_comment_ıd",
                table: "comments");

            migrationBuilder.DropColumn(
                name: "parent_comment_ıd",
                table: "comments");

            migrationBuilder.AlterColumn<int>(
                name: "news_ıd",
                table: "comments",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "fk_comments_news_news_ıd",
                table: "comments",
                column: "news_ıd",
                principalTable: "news",
                principalColumn: "ıd");
        }
    }
}
