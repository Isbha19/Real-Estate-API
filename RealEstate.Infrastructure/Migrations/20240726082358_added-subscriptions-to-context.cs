using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstate.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedsubscriptionstocontext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscription_Plan_PlanId",
                table: "Subscription");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscription_companies_CompanyId",
                table: "Subscription");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Subscription",
                table: "Subscription");

            migrationBuilder.RenameTable(
                name: "Subscription",
                newName: "Subscriptions");

            migrationBuilder.RenameIndex(
                name: "IX_Subscription_PlanId",
                table: "Subscriptions",
                newName: "IX_Subscriptions_PlanId");

            migrationBuilder.RenameIndex(
                name: "IX_Subscription_CompanyId",
                table: "Subscriptions",
                newName: "IX_Subscriptions_CompanyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subscriptions",
                table: "Subscriptions",
                column: "SubscriptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_Plan_PlanId",
                table: "Subscriptions",
                column: "PlanId",
                principalTable: "Plan",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_companies_CompanyId",
                table: "Subscriptions",
                column: "CompanyId",
                principalTable: "companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Plan_PlanId",
                table: "Subscriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_companies_CompanyId",
                table: "Subscriptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Subscriptions",
                table: "Subscriptions");

            migrationBuilder.RenameTable(
                name: "Subscriptions",
                newName: "Subscription");

            migrationBuilder.RenameIndex(
                name: "IX_Subscriptions_PlanId",
                table: "Subscription",
                newName: "IX_Subscription_PlanId");

            migrationBuilder.RenameIndex(
                name: "IX_Subscriptions_CompanyId",
                table: "Subscription",
                newName: "IX_Subscription_CompanyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subscription",
                table: "Subscription",
                column: "SubscriptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscription_Plan_PlanId",
                table: "Subscription",
                column: "PlanId",
                principalTable: "Plan",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscription_companies_CompanyId",
                table: "Subscription",
                column: "CompanyId",
                principalTable: "companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
