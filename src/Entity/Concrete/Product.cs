using Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Concrete
{
    internal class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Kg { get; set; }
        public string Origin { get; set; }
        public double Price { get; set; }
        public bool IsFeatured { get; set; }
      }
}
