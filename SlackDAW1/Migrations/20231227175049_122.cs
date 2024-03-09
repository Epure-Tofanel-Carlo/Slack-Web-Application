using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SlackDAW1.Migrations
{
    public partial class _122 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Channels_ChannelID",
                table: "Messages");

            migrationBuilder.AlterColumn<int>(
                name: "ChannelID",
                table: "Messages",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Channels_ChannelID",
                table: "Messages",
                column: "ChannelID",
                principalTable: "Channels",
                principalColumn: "ChannelID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Channels_ChannelID",
                table: "Messages");

            migrationBuilder.AlterColumn<int>(
                name: "ChannelID",
                table: "Messages",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Channels_ChannelID",
                table: "Messages",
                column: "ChannelID",
                principalTable: "Channels",
                principalColumn: "ChannelID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
