using System;

namespace Youpay.API.Utils
{
    public interface ITransactionsUtil
    {
        string GenerateRandomValues(int length);

        string GenerateTransactionCode(String previousCode);
    }
}