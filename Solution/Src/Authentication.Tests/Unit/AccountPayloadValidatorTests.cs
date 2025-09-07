        [Fact]
        public void Model_WithBoundaryLengthValues_ShouldValidateCorrectly()
        {
            // Arrange - Both at minimum length (6 characters)
            var model = new AccountPayLoadDTO { UserName = "user12", Password = "pass12", CreatedBy = "testuser" };

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Model_WithMaximumLengthValues_ShouldValidateCorrectly()
        {
            // Arrange - Both at maximum length (50 characters)
            var userName = new string('u', 50);
            var password = new string('p', 50);
            var model = new AccountPayLoadDTO { UserName = userName, Password = password, CreatedBy = "testuser" };

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
