using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Core.Entities
{
    public class Book:BaseEntity
    {
        int AuthorId { get; set; }
        public string Name { get; set; }
        public double CostPrice { get; set; }
        public double SalePrice { get; set; }
        public string Desc { get; set; }
        public Author Author { get; set; }  
    }
}
