using Ecom.DataAccess.Data;
using Ecom.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public ICategoryRepository Category { get; private set; }
        public IProductRepository Product { get; private set; }
        public IShoppingCartRepository ShoppingCart { get; private set; }
        public IOrderDetailRepository OrderDetail { get; private set; }
        public IOrderHeaderRepository OrderHeader { get; private set; }
        public IApplicationUserRepository ApplicationUser { get; private set; }
        public IBrandRepository Brand { get; private set; }
        public IFeedbackRepository FeedBack { get; private set; }

        public ISizesRepository Sizes { get; private set; }
        public IProductDetailsRepository ProductDetails { get; private set; }   
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
            Product = new ProductRepository(_db);
            ShoppingCart = new ShoppingCartRepository(_db);
            ApplicationUser = new ApplicationUserRepository(_db);
            OrderDetail = new OrderDetailRepository(_db);
            OrderHeader = new OrderHeaderRepository(_db);
            Brand = new BrandRepository(_db);
            FeedBack = new FeedbackRepository(_db);
            Sizes=new SizesRepository(_db); 
            ProductDetails=new ProductDetailsRepository(_db);   
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
