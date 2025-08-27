using BankManagementSystem.Models;
using BankManagementSystem.Models.Enums;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BankManagementSystem.Tests.Models
{
    public class AccountTests
    {
        [Fact]
        public void Account_Deposit_Returns_TransactionStatus()
        {
            //Arrange
            decimal amountToBeDeposit = 40000;
            Customer customer = new Customer();
            Account account = new SavingsAccount(customer);
            account.Status = AccountStatus.ACTIVE;
            //Act
            TransactionStatus status= account.Deposit(amountToBeDeposit);
            //Assert
            status.Should().NotBe(TransactionStatus.FAILURE);
            account.Balance.Should().Be(amountToBeDeposit);
            account.Transactions.Should().HaveCount(1);

        }
        [Theory]
        [InlineData(1000, 200, 100, TransactionStatus.SUCCESS, 900, 300)] 
        [InlineData(500, 200, 200, TransactionStatus.FAILURE, 500, 200)] 
        [InlineData(800, 0, 300, TransactionStatus.SUCCESS, 500, 300)]
        public void TransferFunds_VariousScenarios_ReturnsExpectedResult(
    decimal fromInitialBalance,
    decimal toInitialBalance,
    decimal transferAmount,
    TransactionStatus expectedStatus,
    decimal expectedFromBalance,
    decimal expectedToBalance)
        {
            // Arrange - create accounts with specified balances
            Customer customer = new Customer();
            Account fromAccount = new SavingsAccount(customer);
            Account toAccount = new SavingsAccount(customer);
            fromAccount.Balance = fromInitialBalance;
            toAccount.Balance = toInitialBalance;
            decimal amountToTransfer = transferAmount;
            // Act - call TransferFunds
            TransactionStatus Status= toAccount.TransferFunds(amountToTransfer, fromAccount, toAccount);
            // Assert - check status and final balances
            Status.Should().Be(expectedStatus);
            toAccount.Balance.Should().Be(expectedToBalance);
            fromAccount.Balance.Should().Be(expectedFromBalance);
        }

    }
}
