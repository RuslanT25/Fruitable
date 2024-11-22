using Core.DataAccess.Abstract;
using Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess.UnitOfWorks
{
    internal interface IUnitOfWork : IAsyncDisposable
    {
        IBaseRepository<T> GetBaseRepository<T>() where T : class, IEntity, new();
        Task<int> SaveChangesAsync();
        int SaveChanges();
    }
}
