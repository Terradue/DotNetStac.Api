using System.Net.Http;

namespace Stac.Api.Clients.Extensions
{
    public partial class ExtensionsClient
    {
        private TransactionClient _transaction;

        public TransactionClient Transaction
        {
            get
            {
                if (_transaction == null)
                {
                    _transaction = new TransactionClient(_client);
                }
                return _transaction;
            }
        }

    }
}