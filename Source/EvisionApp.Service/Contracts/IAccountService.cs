using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvisionApp.Service.Contracts
{
    public interface IAccountService
    {
        Task<double> GetAccountAmount(int accountId);
    }
}
