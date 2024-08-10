using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billy.Utility
{
    public static class SD
    {
        public const string role_customer = "Customer";
        public const string role_company = "Compnay";
        public const string role_admin = "Admin";
        public const string role_employee = "Employee";

		public const string StatusPending = "Pending";
		public const string StatusApproved = "Approved";
		public const string StatusInProcess = "Inprocess";
		public const string StatusShipped = "Shipped";
		public const string StatusCancelled = "Cancelled";
		public const string StatusRefunded = "Pending";

		public const string PaymentPending = "Pending";
		public const string PaymentDone = "Paid";
		public const string PaymentDelayedDone = "Approved for Delayed Payment";
		public const string PaymentRejected = "Rejected";
	}
}
