using Microsoft.EntityFrameworkCore;
using SpyStore.Dal.EfStructures;
using SpyStore.Dal.Exceptions;
using SpyStore.Dal.Repos.Base;
using SpyStore.Dal.Repos.Interfaces;
using SpyStore.Models.Entities;
using SpyStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpyStore.Dal.Repos
{
    public class ShoppingCartRepo : RepoBase<ShoppingCartRecord>, IShoppingCartRepo
    {
        private readonly IProductRepo _productRepo;
        private readonly ICustomerRepo _customerRepo;

        public ShoppingCartRepo(StoreContext context, IProductRepo productRepo, ICustomerRepo customerRepo)
            : base(context)
        {
            _productRepo = productRepo;
            _customerRepo = customerRepo;
        }

        internal ShoppingCartRepo(DbContextOptions<StoreContext> options) : base(options)
        {
            _productRepo = new ProductRepo(Context);
            _customerRepo = new CustomerRepo(Context);
        }

        public override void Dispose()
        {
            _productRepo.Dispose();
            _customerRepo.Dispose();
            base.Dispose();
        }

        public override IEnumerable<ShoppingCartRecord> GetAll()
        {
            return base.GetAll(x => x.DateCreated).ToList();
        }

        public int Add(ShoppingCartRecord entity, Product product, bool persist = true)
        {
            throw new NotImplementedException();
        }

        public ShoppingCartRecord GetBy(int productId)
        {
            return Table.FirstOrDefault(x => x.ProductId == productId);
        }

        public CartRecordWithProductInfo GetShoppingCartRecord(int id)
        {
            return Context.CartRecordWithProductInfos
                .FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<CartRecordWithProductInfo> GetShoppingCartRecords(int customerId)
        {
            return Context.CartRecordWithProductInfos
                .Where(x => x.CustomerId == customerId)
                .OrderBy(x => x.ModelName);
        }

        public CartWithCustomerInfo GetShoppingCartRecordsWithCustomer(int customerId)
        {
            return new CartWithCustomerInfo
            {
                Customer = _customerRepo.Find(customerId),
                CartRecords = GetShoppingCartRecords(customerId).ToList()
            };
        }

        public int Purchase(int customerId)
        {
            throw new NotImplementedException();
        }

        public override int Update(ShoppingCartRecord entity, bool persist = true)
        {
            var product = _productRepo.FindAsNoTracking(entity.ProductId);

            return Update(entity, product, persist);
        }

        public int Update(ShoppingCartRecord entity, Product product, bool persist = true)
        {
            if (product == null)
            {
                throw new SpyStoreInvalidProductException("Unable to locate product");
            }
            if (entity.Quantity <= 0)
            {
                Delete(entity, persist);
            }
            if (entity.Quantity > product.UnitsInStock)
            {
                throw new SpyStoreInvalidQuantityException("Can't add more than available in stock");
            }
            var dbRecord = Find(entity.Id);
            if (dbRecord == null)
            {
                throw new Exception("Unable to locate record");
            }
            if (entity.TimeStamp != null && dbRecord.TimeStamp.SequenceEqual(entity.TimeStamp))
            {
                dbRecord.Quantity = entity.Quantity;
                dbRecord.LineItemTotal = entity.Quantity * product.CurrentPrice;
                return base.Update(dbRecord, persist);
            }
            throw new SpyStoreConcurrencyException("record was changed since it was loaded");
        }

        public override int UpdateRange(IEnumerable<ShoppingCartRecord> entities, bool persist = true)
        {
            int counter = 0;
            foreach (var item in entities)
            {
                counter += Update(item, false);
            }
            
            return persist ? SaveChanges() : counter;
        }
    }
}
