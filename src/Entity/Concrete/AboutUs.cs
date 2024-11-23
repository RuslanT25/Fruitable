using Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Concrete
{
    internal class AboutUs : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }

    }
}
