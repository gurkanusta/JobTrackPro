using System;

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobTrackPro.Infrastructure.Migrations
{
    public partial class AddJobUrlCompanyUrlInterviewDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "JobUrl",
                table: "JobApplicationsSet",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyUrl",
                table: "JobApplicationsSet",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "InterviewDate",
                table: "JobApplicationsSet",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "JobUrl", table: "JobApplicationsSet");
            migrationBuilder.DropColumn(name: "CompanyUrl", table: "JobApplicationsSet");
            migrationBuilder.DropColumn(name: "InterviewDate", table: "JobApplicationsSet");
        }
    }
}