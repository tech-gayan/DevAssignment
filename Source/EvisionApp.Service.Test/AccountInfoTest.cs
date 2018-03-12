using System;
using System.Threading;
using System.Threading.Tasks;
using EvisionApp.Domain.Model;
using EvisionApp.Service.Contracts;
using EvisionApp.Service.Implementation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EvisionApp.Service.Test
{
    [TestClass]
    public class AccountInfoTest
    {
        private int _accountId;
        private IAccountService _accountService;
        private double _amount;

        [TestInitialize]
        public void Initialize()
        {
            _accountId = 42;
            _amount = 524.52;

            Mock<IAccountService> accountServiceStub = new Mock<IAccountService>();

            accountServiceStub.Setup(m => m.GetAccountAmount(It.Is<int>(account => account <= 0)))
                .Callback(() => Thread.Sleep(10))
                .ThrowsAsync(new ArgumentOutOfRangeException());

            accountServiceStub.Setup(m => m.GetAccountAmount(It.IsInRange(1, int.MaxValue, Range.Inclusive)))
               .Callback(() => Thread.Sleep(10))
                .Returns<double>(t => Task.FromResult(_amount));

            _accountService = accountServiceStub.Object;
        }

        [TestMethod]
        [Description("Test ckecks if Amount property will read before RefreshAmount() method call. Expected to return 0.")]
        public void Amount_RefreshAmountNotCalled_ReturnsZero()
        {
            AccountInfo account = new AccountInfo(_accountId, _accountService);
            Assert.AreEqual(0, account.Amount);
        }

        [TestMethod]
        [Description("Test checks if IAccountService is not initialized, but class will not faults while RefreshAmount() method not called.")]
        public void Amount_IAccountServiceNotDefined_ReturnsZero()
        {
            AccountInfo account = new AccountInfo(_accountId, null);
            Assert.AreEqual(0, account.Amount);
        }

        [TestMethod]
        [Description("Test regular scenario when RefreshAmount called and Amount property requested.")]
        public async Task RefreshAmount_PositiveAmountExists_ReturnsAccountValue()
        {
            AccountInfo account = new AccountInfo(_accountId, _accountService);
            await account.RefreshAmount();
            Assert.AreEqual(_amount, account.Amount);
        }

        [TestMethod]
        [Description("Test scenario when RefreshAmount called few times, and then Amount property requested.")]
        public async Task RefreshAmount_RefreshAmountDoubleCallWithSameData_ReturnsAccountValue()
        {
            AccountInfo account = new AccountInfo(_accountId, _accountService);
            await account.RefreshAmount();
            Assert.AreEqual(_amount, account.Amount);

            await account.RefreshAmount();
            Assert.AreEqual(_amount, account.Amount);
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(double.MaxValue)]
        [DataRow(double.MinValue)]
        [DataRow(double.NaN)]
        [DataRow(double.PositiveInfinity)]
        [DataRow(double.NegativeInfinity)]
        public async Task RefreshAmount_RefreshAmountReturnsExtremalValues_ReturnsAccountValue(double amountValue)
        {
            _amount = amountValue;

            AccountInfo account = new AccountInfo(_accountId, _accountService);
            await account.RefreshAmount();

            Assert.AreEqual(_amount, account.Amount);
        }

        [TestMethod]
        [Description("Test scenario when requested account number is invalid, and data was not found.")]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public async Task RefreshAmount_RefreshAmountForNotExistedAccountNumber_Exception()
        {
            _accountId = 0;

            AccountInfo account = new AccountInfo(_accountId, _accountService);
            await account.RefreshAmount();
        }

        [TestMethod]
        [Description("Test scenario when RefreshAmount called few times with data changes between calls. Amount property checked few times.")]
        public async Task RefreshAmount_RefreshAmountDoubleCallWithDifferentData_ReturnsAccountValue()
        {
            AccountInfo account = new AccountInfo(_accountId, _accountService);

            await account.RefreshAmount();
            Assert.AreEqual(_amount, account.Amount);

            // change amount expectation between the calls.
            _amount = _amount / _accountId;

            await account.RefreshAmount();
            Assert.AreEqual(_amount, account.Amount);
        }

        [TestMethod]
        [Description("This is the only method invoked the real implementation to check whether the correct value is returning")]
        public async Task TestAccountObj_RefreshAmountAsync()
        {
            const int Id = 438;
            IAccountService accountService = new AccountService();
            var testObject = new AccountInfo(Id, accountService);

            await testObject.RefreshAmount();
            double amount = testObject.Amount;
            double expectedAmount = await accountService.GetAccountAmount(Id);
            Assert.AreEqual(expectedAmount, amount);
        }

        [TestCleanup]
        public void TearDown()
        {
            _accountId = 0;
            _amount = 0.0;
            _accountService = null;
        }
    }
}
