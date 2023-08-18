using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Utility
{
    public static class SD
    {
        public const string Role_customer = "Customer";
        public const string Role_Admin = "Admin";

		//initial status when the order is created
		public const string StatusPending = "Pending";
		//Will be updated by admin when they are processing the order
		public const string StatusInProcess = "Processing";
		//After the process is done the order will be shipped(Final status)
		public const string StatusShipped = "Shipped";
		public const string StatusCancelled = "Cancelled";

        public const string SessionCart = "SessionShoppingCart";


    }
}
