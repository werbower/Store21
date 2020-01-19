using Microsoft.EntityFrameworkCore;
using SpyStore.Dal.EfStructures;
using SpyStore.Dal.Repos.Base;
using SpyStore.Dal.Repos.Interfaces;
using SpyStore.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpyStore.Dal.Repos
{
    public class ProductRepo : RepoBase<Product>, IProductRepo
    {
        public ProductRepo(StoreContext context) : base(context)
        {
        }

        public ProductRepo(DbContextOptions<StoreContext> options) : base(options)
        {
        }

        public override IEnumerable<Product> GetAll()
        {
            return base.GetAll(x => x.Details.ModelName);
        }

        public IList<Product> GetFeaturedWithCategoryName()
        {
            return Table.Where(x => x.IsFeatured)
                .Include(x => x.CategoryNavigation)
                .OrderBy(x => x.Details.ModelName)
                .ToList();
        }

        public Product GetOneWithCategoryName(int id)
        {
            return Table.Where(x => x.Id == id)
                .Include(x => x.CategoryNavigation)
                .FirstOrDefault();
        }

        public IList<Product> GetProductsForCategory(int id)
        {
            return Table.Where(x => x.CategoryId == id)
                .Include(x => x.CategoryNavigation)
                .OrderBy(x => x.Details.ModelName)
                .ToList();
        }

        public IList<Product> Search(string searchString)
        {
            return Table.Where(x => EF.Functions.Like(x.Details.Description, $"%{searchString}%")
                || EF.Functions.Like(x.Details.ModelName, $"%{searchString}%"))
                .Include(x => x.CategoryNavigation)
                .OrderBy(x => x.Details.ModelName)
                .ToList();
        }
    }
}
