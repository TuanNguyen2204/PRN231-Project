using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessObjects.Migrations
{
    public partial class InitDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CATEGORY",
                columns: table => new
                {
                    CATEGORY_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CATEGORY_NAME = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CATEGORY", x => x.CATEGORY_ID);
                });

            migrationBuilder.CreateTable(
                name: "COLORS",
                columns: table => new
                {
                    COLOR_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    COLOR_NAME = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COLORS", x => x.COLOR_ID);
                });

            migrationBuilder.CreateTable(
                name: "SIZES",
                columns: table => new
                {
                    SIZE_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SIZE_NAME = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SIZES", x => x.SIZE_ID);
                });

            migrationBuilder.CreateTable(
                name: "USERS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    USERNAME = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    PASSWORD = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    NAME = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    EMAIL = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    ADDRESS = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    IS_SELLER = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USERS", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PRODUCTS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NAME = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    DESCRIPTION = table.Column<string>(type: "text", nullable: false),
                    PRICE = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    IMAGE = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    CATEGORY_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUCTS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PRODUCTS_CATEGORY",
                        column: x => x.CATEGORY_ID,
                        principalTable: "CATEGORY",
                        principalColumn: "CATEGORY_ID");
                });

            migrationBuilder.CreateTable(
                name: "CHAT_MESSAGES",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SENDER_ID = table.Column<int>(type: "int", nullable: false),
                    RECEIVER_ID = table.Column<int>(type: "int", nullable: false),
                    MESSAGE = table.Column<string>(type: "text", nullable: false),
                    DATE_SENT = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CHAT_MESSAGES", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CHAT_MESSAGES_USERS2",
                        column: x => x.SENDER_ID,
                        principalTable: "USERS",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_CHAT_MESSAGES_USERS3",
                        column: x => x.RECEIVER_ID,
                        principalTable: "USERS",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "ORDERS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    USER_ID = table.Column<int>(type: "int", nullable: false),
                    QUANTITY = table.Column<int>(type: "int", nullable: false),
                    DATE_ORDERED = table.Column<DateTime>(type: "date", nullable: false),
                    PAYMENT_METHOD = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    DELIVERY_LOCATION = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    TOTAL_PRICE = table.Column<decimal>(type: "decimal(18,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ORDERS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ORDERS_USERS",
                        column: x => x.USER_ID,
                        principalTable: "USERS",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "CARTS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    USER_ID = table.Column<int>(type: "int", nullable: false),
                    PRODUCT_ID = table.Column<int>(type: "int", nullable: false),
                    SIZE_ID = table.Column<int>(type: "int", nullable: false),
                    COLOR_ID = table.Column<int>(type: "int", nullable: false),
                    QUANTITY = table.Column<int>(type: "int", nullable: false),
                    DATE_ADDED = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CARTS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CARTS_COLORS",
                        column: x => x.COLOR_ID,
                        principalTable: "COLORS",
                        principalColumn: "COLOR_ID");
                    table.ForeignKey(
                        name: "FK_CARTS_PRODUCTS",
                        column: x => x.PRODUCT_ID,
                        principalTable: "PRODUCTS",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_CARTS_SIZES",
                        column: x => x.SIZE_ID,
                        principalTable: "SIZES",
                        principalColumn: "SIZE_ID");
                    table.ForeignKey(
                        name: "FK_CARTS_USERS",
                        column: x => x.USER_ID,
                        principalTable: "USERS",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "INVENTORY",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PRODUCT_ID = table.Column<int>(type: "int", nullable: false),
                    SIZE_ID = table.Column<int>(type: "int", nullable: false),
                    COLOR_ID = table.Column<int>(type: "int", nullable: false),
                    QUANTITY = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_INVENTORY", x => x.ID);
                    table.ForeignKey(
                        name: "FK_INVENTORY_COLORS",
                        column: x => x.COLOR_ID,
                        principalTable: "COLORS",
                        principalColumn: "COLOR_ID");
                    table.ForeignKey(
                        name: "FK_INVENTORY_PRODUCTS",
                        column: x => x.PRODUCT_ID,
                        principalTable: "PRODUCTS",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_INVENTORY_SIZES",
                        column: x => x.SIZE_ID,
                        principalTable: "SIZES",
                        principalColumn: "SIZE_ID");
                });

            migrationBuilder.CreateTable(
                name: "REVIEWS",
                columns: table => new
                {
                    REVIEW_ID = table.Column<int>(type: "int", nullable: false),
                    USER_ID = table.Column<int>(type: "int", nullable: false),
                    PRODUCT_ID = table.Column<int>(type: "int", nullable: false),
                    RATING = table.Column<int>(type: "int", nullable: false),
                    REVIEW_TEXT = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REVIEWS", x => x.REVIEW_ID);
                    table.ForeignKey(
                        name: "FK_REVIEWS_PRODUCTS",
                        column: x => x.PRODUCT_ID,
                        principalTable: "PRODUCTS",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_REVIEWS_USERS",
                        column: x => x.USER_ID,
                        principalTable: "USERS",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "SALES",
                columns: table => new
                {
                    SALE_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PRODUCT_ID = table.Column<int>(type: "int", nullable: false),
                    SALE_PRICE = table.Column<int>(type: "int", nullable: false),
                    START_DATE = table.Column<DateTime>(type: "date", nullable: false),
                    END_DATE = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SALES", x => x.SALE_ID);
                    table.ForeignKey(
                        name: "FK_SALES_PRODUCTS",
                        column: x => x.PRODUCT_ID,
                        principalTable: "PRODUCTS",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "ORDER_DETAILS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ORDER_ID = table.Column<int>(type: "int", nullable: false),
                    PRODUCT_ID = table.Column<int>(type: "int", nullable: false),
                    SIZE_ID = table.Column<int>(type: "int", nullable: false),
                    COLOR_ID = table.Column<int>(type: "int", nullable: false),
                    QUANTITY = table.Column<int>(type: "int", nullable: false),
                    PRICE = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ORDER_DETAILS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ORDER_DETAILS_COLORS",
                        column: x => x.COLOR_ID,
                        principalTable: "COLORS",
                        principalColumn: "COLOR_ID");
                    table.ForeignKey(
                        name: "FK_ORDER_DETAILS_ORDERS",
                        column: x => x.ORDER_ID,
                        principalTable: "ORDERS",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_ORDER_DETAILS_PRODUCTS",
                        column: x => x.PRODUCT_ID,
                        principalTable: "PRODUCTS",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_ORDER_DETAILS_SIZES",
                        column: x => x.SIZE_ID,
                        principalTable: "SIZES",
                        principalColumn: "SIZE_ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CARTS_COLOR_ID",
                table: "CARTS",
                column: "COLOR_ID");

            migrationBuilder.CreateIndex(
                name: "IX_CARTS_PRODUCT_ID",
                table: "CARTS",
                column: "PRODUCT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_CARTS_SIZE_ID",
                table: "CARTS",
                column: "SIZE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_CARTS_USER_ID",
                table: "CARTS",
                column: "USER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_CHAT_MESSAGES_RECEIVER_ID",
                table: "CHAT_MESSAGES",
                column: "RECEIVER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_CHAT_MESSAGES_SENDER_ID",
                table: "CHAT_MESSAGES",
                column: "SENDER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_INVENTORY_COLOR_ID",
                table: "INVENTORY",
                column: "COLOR_ID");

            migrationBuilder.CreateIndex(
                name: "IX_INVENTORY_PRODUCT_ID",
                table: "INVENTORY",
                column: "PRODUCT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_INVENTORY_SIZE_ID",
                table: "INVENTORY",
                column: "SIZE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ORDER_DETAILS_COLOR_ID",
                table: "ORDER_DETAILS",
                column: "COLOR_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ORDER_DETAILS_ORDER_ID",
                table: "ORDER_DETAILS",
                column: "ORDER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ORDER_DETAILS_PRODUCT_ID",
                table: "ORDER_DETAILS",
                column: "PRODUCT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ORDER_DETAILS_SIZE_ID",
                table: "ORDER_DETAILS",
                column: "SIZE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ORDERS_USER_ID",
                table: "ORDERS",
                column: "USER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_PRODUCTS_CATEGORY_ID",
                table: "PRODUCTS",
                column: "CATEGORY_ID");

            migrationBuilder.CreateIndex(
                name: "IX_REVIEWS_PRODUCT_ID",
                table: "REVIEWS",
                column: "PRODUCT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_REVIEWS_USER_ID",
                table: "REVIEWS",
                column: "USER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_SALES_PRODUCT_ID",
                table: "SALES",
                column: "PRODUCT_ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CARTS");

            migrationBuilder.DropTable(
                name: "CHAT_MESSAGES");

            migrationBuilder.DropTable(
                name: "INVENTORY");

            migrationBuilder.DropTable(
                name: "ORDER_DETAILS");

            migrationBuilder.DropTable(
                name: "REVIEWS");

            migrationBuilder.DropTable(
                name: "SALES");

            migrationBuilder.DropTable(
                name: "COLORS");

            migrationBuilder.DropTable(
                name: "ORDERS");

            migrationBuilder.DropTable(
                name: "SIZES");

            migrationBuilder.DropTable(
                name: "PRODUCTS");

            migrationBuilder.DropTable(
                name: "USERS");

            migrationBuilder.DropTable(
                name: "CATEGORY");
        }
    }
}
