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
    public class ProductDetailsRepository : Repository<ProductDetails>, IProductDetailsRepository
    {
        private ApplicationDbContext _db;
        public ProductDetailsRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(ProductDetails obj)
        {
            _db.ProductDetails.Update(obj);
        }
    }
}
