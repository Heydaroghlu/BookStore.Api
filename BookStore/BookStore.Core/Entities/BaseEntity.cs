using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Core.Entities
{
   public class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime ModifiedTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
