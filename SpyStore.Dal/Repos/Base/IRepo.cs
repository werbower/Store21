using SpyStore.Models.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpyStore.Dal.Repos.Base
{
    public interface IRepo<T>: IDisposable where T: EntityBase
    {
    }
}
