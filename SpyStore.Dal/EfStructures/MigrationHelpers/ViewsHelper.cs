using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SpyStore.Dal.EfStructures.MigrationHelpers
{
    public static class ViewsHelper
    {
        public static void CreateOrderDetailWithProductInfoView(MigrationBuilder migrationBuilder)
        {
            var sql = @"
                CREATE VIEW [Store].[OrderDetailWithProductInfo] AS
                SELECT od.Id, od.TimeStamp, od.OrderId, od.ProductId,
                    od.Quantity, od.UnitCost,
                    od.Quantity * od.UnitCost AS LineItemTotal,
                    p.Details_Description as Description,
                    p.Details_ModelName as ModelName,
                    p.Details_ModelNumber as ModelNumber,
                    p.Details_ProductImage as ProductImage,
                    p.Details_ProductImageLarge as ProductImageLarge,
                    p.Details_ProductImageThumb as ProductImageThumb,
                    p.CategoryId, p.UnitsInStock, p.CurrentPrice, c.CategoryName
                FROM Store.OrderDetails od
                INNER JOIN Store.Orders o ON o.Id = od.OrderId
                INNER JOIN Store.Products AS p ON od.ProductId = p.Id
                INNER JOIN Store.Categories AS c ON p.CategoryId = c.id
            ";
            migrationBuilder.Sql(sql);
        }
        public static void DropOrderDetailWithProductInfoView(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("drop view [Store].[OrderDetailWithProductInfo]");
        }

        public static void CreateCartRecordWithProductInfoView(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE VIEW [Store].[CartRecordWithProductInfo] AS
                SELECT scr.Id, scr.TimeStamp, scr.DateCreated, scr.CustomerId,
                    scr.Quantity, scr.LineItemTotal, scr.ProductId,
                    p.Details_Description as Description,
                    p.Details_ModelName as ModelName,
                    p.Details_ModelNumber as ModelNumber,
                    p.Details_ProductImage as ProductImage,
                    p.Details_ProductImageLarge as ProductImageLarge,
                    p.Details_ProductImageThumb as ProductImageThumb,
                    p.CategoryId, p.UnitsInStock, p.CurrentPrice, c.CategoryName
                FROM Store.ShoppingCartRecords scr
                INNER JOIN Store.Products p ON p.Id = scr.ProductId
                INNER JOIN Store.Categories c ON c.Id = p.CategoryId
            ");
        }
        
        public static void DropCartRecordWithProductInfoView(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("drop view [Store].[CartRecordWithProductInfo]");
        }
    }
}
