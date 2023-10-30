using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stac.Api.Models;

namespace Stac.Api.Models.Extensions.Transactions
{
    public static class TransactionsExtensions
    {
        public static bool ValidateInputForTransaction(this PostStacItemOrCollection postStacItemOrCollection)
        {
            if (postStacItemOrCollection.IsCollection)
            {
                return postStacItemOrCollection.StacFeatureCollection.ValidateInputForTransaction();
            }
            else
            {
                return postStacItemOrCollection.StacItem.ValidateInputForTransaction();
            }
        }

        public static bool ValidateInputForTransaction(this StacFeatureCollection stacFeatureCollection)
        {
            // Must have an id field.
            if ( stacFeatureCollection.Items.Any(x => x.Id == null) )
            {
                throw new ArgumentException("All items in a collection must have an id field.");
            }

            return true;
        }

        public static bool ValidateInputForTransaction(this StacItem stacItem)
        {
            // Must have an id field.
            if (stacItem.Id == null)
            {
                throw new ArgumentException("Item must have an id field.");
            }

            return true;
        }
    }
}