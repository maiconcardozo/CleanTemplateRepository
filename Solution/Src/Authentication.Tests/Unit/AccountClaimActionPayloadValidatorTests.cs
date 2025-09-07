[Fact]
public void Model_WhenBothFieldsValid_ShouldHaveNoValidationErrors()
{
    // Arrange
    var model = new AccountClaimActionPayLoadDTO { IdAccount = 1, IdClaimAction = 1, CreatedBy = "testuser" };

    // Act
    var result = _validator.TestValidate(model);

    // Assert
    result.ShouldNotHaveAnyValidationErrors();
}

[Fact]
public void Model_WithLargeValidValues_ShouldValidateCorrectly()
{
    // Arrange
    var model = new AccountClaimActionPayLoadDTO { IdAccount = 999999, IdClaimAction = 999999, CreatedBy = "testuser" };

    // Act
    var result = _validator.TestValidate(model);

    // Assert
    result.ShouldNotHaveAnyValidationErrors();
}
