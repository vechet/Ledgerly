using Ledgerly.API.Helpers;

namespace Ledgerly.API.Models.DTOs.TransactionType
{
    public class GetTransactionTypesResponse
    {
        public List<TransactionTypesResponse> TransactionTypes { get; set; }
        public PageInfo PageInfo { get; set; }

        public GetTransactionTypesResponse()
        { }

        public GetTransactionTypesResponse(List<TransactionTypesResponse> transactionTypes, PageInfo pageInfo)
        {
            TransactionTypes = transactionTypes;
            PageInfo = pageInfo;
        }
    }

    public class TransactionTypesResponse
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
    }
}
