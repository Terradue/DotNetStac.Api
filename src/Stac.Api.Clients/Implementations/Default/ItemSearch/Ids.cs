using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stac.Api.Clients.ItemSearch
{
    public partial class Ids : System.Collections.ObjectModel.Collection<string>
    {
        public Ids(IEnumerable<string> ids)
        {
        }
    }
}