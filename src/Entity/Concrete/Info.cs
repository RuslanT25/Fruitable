using Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Concrete
{
    internal class Info : BaseEntity
    {
        public string Title { get; set; }
        public int Number { get; set; }

    }
}
