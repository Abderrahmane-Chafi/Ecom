using Ecom.DataAccess.Data;
using Ecom.DataAccess.Repository.IRepository;
using Ecom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Product obj)
        {
            var objFromDb = _db.Products.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = obj.Name;
                objFromDb.Price = obj.Price;
                objFromDb.Gender = obj.Gender;
                objFromDb.CategoryId = obj.CategoryId;
                objFromDb.BrandId = obj.BrandId;
                objFromDb.Quantity = obj.Quantity;
                objFromDb.status = obj.status;


            }
            if (obj.ImageUrl != null)
            {
                objFromDb.ImageUrl = obj.ImageUrl;
            }
            if (obj.ImageUrl1 != null)
            {
                objFromDb.ImageUrl1 = obj.ImageUrl1;
            }
        }
    }
}
