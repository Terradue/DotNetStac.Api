using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Stac.Api.Interfaces;

namespace Stac.Api.Services.Queryable
{
    public class StacQueryable<TSource> : IQueryable<TSource>
    {

        public StacQueryable(IStacQueryProvider provider, Expression expression)
        {
            this.StacQueryProvider = provider;
            this.Expression = expression;
        }

        #region IQueryable Members

        public Type ElementType => typeof(TSource);

        public Expression Expression { get; private set; }

        public IQueryProvider Provider => this.StacQueryProvider;
        
        public IStacQueryProvider StacQueryProvider { get; internal set; }

        #endregion

        #region IEnumerable<TSource> Members

        public IEnumerator<TSource> GetEnumerator()
        {
            return this.Provider.Execute<IEnumerable<TSource>>(this.Expression).GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        public override string ToString()
        {
            return this.Expression.ToString();
        }
    }
}