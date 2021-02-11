using System;

namespace dotnetpostgres.Dto.Customer
{
    public class Customer : DtoBase
    {
        public string Title { get; set; }

        public string PhoneNumber { get; set; }

        public string AuthorizedPersonName { get; set; }

        //public double DebtBalance { get; set; }

        //public double ReceivableBalance { get; set; }

        public DateTime CreatedAt { get; set; }

       // public virtual ICollection<Transaction> Transactions { get; set; }// 1=>n relation
    }
}
