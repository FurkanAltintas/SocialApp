using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerApp.Migrations
{
    public partial class AddingEntityMessage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserToUser_AspNetUsers_FollowerId",
                table: "UserToUser");

            migrationBuilder.DropForeignKey(
                name: "FK_UserToUser_AspNetUsers_UserId",
                table: "UserToUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserToUser",
                table: "UserToUser");

            migrationBuilder.RenameTable(
                name: "UserToUser",
                newName: "UserToUsers");

            migrationBuilder.RenameIndex(
                name: "IX_UserToUser_FollowerId",
                table: "UserToUsers",
                newName: "IX_UserToUsers_FollowerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserToUsers",
                table: "UserToUsers",
                columns: new[] { "UserId", "FollowerId" });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SenderId = table.Column<int>(type: "INTEGER", nullable: false),
                    RecipientId = table.Column<int>(type: "INTEGER", nullable: false),
                    Text = table.Column<string>(type: "TEXT", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateRead = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsRead = table.Column<bool>(type: "INTEGER", nullable: false),
                    SenderDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    RecipientDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_AspNetUsers_RecipientId",
                        column: x => x.RecipientId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Messages_AspNetUsers_SenderId",
                        column: x => x.SenderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_RecipientId",
                table: "Messages",
                column: "RecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderId",
                table: "Messages",
                column: "SenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserToUsers_AspNetUsers_FollowerId",
                table: "UserToUsers",
                column: "FollowerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserToUsers_AspNetUsers_UserId",
                table: "UserToUsers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserToUsers_AspNetUsers_FollowerId",
                table: "UserToUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_UserToUsers_AspNetUsers_UserId",
                table: "UserToUsers");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserToUsers",
                table: "UserToUsers");

            migrationBuilder.RenameTable(
                name: "UserToUsers",
                newName: "UserToUser");

            migrationBuilder.RenameIndex(
                name: "IX_UserToUsers_FollowerId",
                table: "UserToUser",
                newName: "IX_UserToUser_FollowerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserToUser",
                table: "UserToUser",
                columns: new[] { "UserId", "FollowerId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserToUser_AspNetUsers_FollowerId",
                table: "UserToUser",
                column: "FollowerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserToUser_AspNetUsers_UserId",
                table: "UserToUser",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
