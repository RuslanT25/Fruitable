﻿using Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Concrete
{
    public abstract class BaseEntity : IEntity
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}