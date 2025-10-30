        [Fact]
        public void UpdateAccount_WithUniqueUserName_ShouldUpdateSuccessfully()
        {
            // Arrange
            var accountToUpdate = new Account { Id = 1, UserName = "updateduser", Password = "newpassword" };
            _mockAccountRepository.Setup(x => x.GetByUserName(accountToUpdate.UserName)).Returns((Account?)null);
            _mockUnitOfWork.Setup(x => x.ExecuteInTransaction(It.IsAny<System.Action>()))
                .Callback<System.Action>(action => action());

            // Act
            _accountService.UpdateAccount(accountToUpdate);

            // Assert
            _mockAccountRepository.Verify(x => x.GetByUserName(accountToUpdate.UserName), Times.Once);
            _mockAccountRepository.Verify(x => x.Update(accountToUpdate), Times.Once);
        }

        [Fact]
        public void UpdateAccount_WithSameAccountUserName_ShouldAllowUpdate()
        {
            // Arrange
            var accountToUpdate = new Account { Id = 1, UserName = "sameuser", Password = "newpassword" };
            var existingAccount = new Account { Id = 1, UserName = "sameuser", Password = "oldpassword" };
            
            _mockAccountRepository.Setup(x => x.GetByUserName(accountToUpdate.UserName)).Returns(existingAccount);
            _mockUnitOfWork.Setup(x => x.ExecuteInTransaction(It.IsAny<System.Action>()))
                .Callback<System.Action>(action => action());

            // Act
            _accountService.UpdateAccount(accountToUpdate);

            // Assert
            _mockAccountRepository.Verify(x => x.GetByUserName(accountToUpdate.UserName), Times.Once);
            _mockAccountRepository.Verify(x => x.Update(accountToUpdate), Times.Once);
        }

        [Fact]
        public void AddAccount_WithNewUserName_ShouldAddAccountSuccessfully()
        {
            // Arrange
            var newAccount = new Account { UserName = "newuser", Password = "plainpassword" };
            _mockAccountRepository.Setup(x => x.GetByUserName(newAccount.UserName)).Returns((Account?)null);
            _mockUnitOfWork.Setup(x => x.ExecuteInTransaction(It.IsAny<System.Action>()))
                .Callback<System.Action>(action => action());

            // Act
            _accountService.AddAccount(newAccount);

            // Assert
            _mockAccountRepository.Verify(x => x.GetByUserName(newAccount.UserName), Times.Once);
            _mockAccountRepository.Verify(x => x.Add(newAccount), Times.Once);
        }