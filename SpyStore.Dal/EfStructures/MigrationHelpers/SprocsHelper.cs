using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SpyStore.Dal.EfStructures.MigrationHelpers
{
    public static class SprocsHelper
    {
        public static void CreatePurchaseSproc(MigrationBuilder migrationBuilder)
        {
            var sql = @"
                create procedure Store.PurchaseItemsInCart (@customerId int=0, @orderId int output) as
                begin
	                set nocount on;
	                insert into Store.Orders (CustomerId, OrderDate, ShipDate)
	                values (@customerId, GETDATE(), GETDATE());
	                set @orderId = SCOPE_IDENTITY();
	                declare @tranName varchar(20);
	                set @tranName = 'commitOrder';
	                begin transaction @tranName;

	                begin try
		                insert into Store.OrderDetails(OrderId, ProductId, Quantity, UnitCost)
		                select @orderId, scr.ProductId, scr.Quantity, p.CurrentPrice
		                from Store.ShoppingCartRecords as scr
		                inner join Store.Products as p on p.Id = scr.ProductId
		                where scr.CustomerId = @customerId;

		                delete from Store.ShoppingCartRecords
			            where CustomerId = @customerId;

		                commit transaction @tranName;
	                end try
	                begin catch
		                rollback transaction @tranName;
		                set @orderId = -1;
	                end catch
                end
            ";
            migrationBuilder.Sql(sql);
        }
        public static void DropPurchaseSproc(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE [Store].[PurchaseItemsInCart]");
        }
    }
}
