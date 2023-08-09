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
    public class FeedbackRepository : Repository<Feedback>, IFeedbackRepository
    {
        private ApplicationDbContext _db;
        public FeedbackRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Feedback obj)
        {
            _db.Feedbacks.Update(obj);

        }
    }
}
