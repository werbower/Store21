using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpyStore.Dal.EfStructures.MigrationHelpers
{
    public static class FunctionsHelper
    {
        public static void CreateOrderTotalFunction(MigrationBuilder migrationBuilder)
        {
            string sql = @"
                create function Store.GetOrderTotal(@OrderId int)
                returns money with schemabinding
                begin
	                declare @Result money;
	                select @Result = sum(Quantity*UnitCost)
	                from Store.OrderDetails
	                where OrderId = @OrderId
	                return coalesce(@Result, 0)
                end
            ";
            migrationBuilder.Sql(sql);
        }

        public static void DropOrderTotalFunction(MigrationBuilder migrationBuilder)
        {
            string sql = @"drop function [Store].[GetOrderTotal]";
            migrationBuilder.Sql(sql);

        }
    }
}
