using EvisionApp.Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvisionApp.Service.Implementation
{
    public class AccountService : IAccountService
    {
        public async Task<double> GetAccountAmount(int accountId)
        {
            var x = 137 * accountId - 472;
            return await Task.FromResult<double>(x);
        }
    }
}
