using Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Concrete
{
    internal class FoodInfo : BaseEntity
    {
        public string Title { get; set; }
        public string Info { get; set; }
    }
}
