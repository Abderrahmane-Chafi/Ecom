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
    public class SizesRepository : Repository<Sizes>, ISizesRepository
    {
        private ApplicationDbContext _db;
        public SizesRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Sizes obj)
        {
            _db.Sizes.Update(obj);
        }
    }
}
