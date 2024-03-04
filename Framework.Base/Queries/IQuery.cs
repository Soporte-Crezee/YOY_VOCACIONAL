using System;
using System.Linq.Expressions;

namespace Framework.Base.Entity.Queries
{
    public interface IQuery<T> where T : class
    {
        Expression<Func<T, bool>> Action(params Expression<Func<T,bool>>[] filters);
    }
}
