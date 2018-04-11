// ReSharper disable All
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Foods.Service.Api.Migrations
{
    public partial class AddedPreviousEmail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PreviousEmail",
                table: "AspNetUsers",
                type: "longtext",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreviousEmail",
                table: "AspNetUsers");
        }
    }
}
// ReSharper restore All