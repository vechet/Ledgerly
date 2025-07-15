using Microsoft.AspNetCore.Identity;

namespace Ledgerly.API.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }

}
