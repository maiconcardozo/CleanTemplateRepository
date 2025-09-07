using Authentication.Login.Domain.Implementation;
using FluentAssertions;
using Xunit;

namespace Authentication.Tests.Unit
{
    /// <summary>
    /// Unit tests for the CleanEntity domain model.
    /// Tests basic entity properties and behavior.
    /// </summary>
    public class CleanEntityTests
    {
        [Fact]
        public void CleanEntity_WhenCreated_ShouldHaveDefaultValues()
        {
            // Act
            var cleanEntity = new CleanEntity();

            // Assert
            cleanEntity.Name.Should().Be(string.Empty);
            cleanEntity.Description.Should().Be(string.Empty);
            cleanEntity.Id.Should().Be(0);
        }

        [Fact]
        public void CleanEntity_SetName_ShouldUpdateNameProperty()
        {
            // Arrange
            var cleanEntity = new CleanEntity();
            var expectedName = "TestCleanEntity";

            // Act
            cleanEntity.Name = expectedName;

            // Assert
            cleanEntity.Name.Should().Be(expectedName);
        }

        [Fact]
        public void CleanEntity_SetDescription_ShouldUpdateDescriptionProperty()
        {
            // Arrange
            var cleanEntity = new CleanEntity();
            var expectedDescription = "Test description for clean entity";

            // Act
            cleanEntity.Description = expectedDescription;

            // Assert
            cleanEntity.Description.Should().Be(expectedDescription);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void CleanEntity_SetNameToNullOrEmpty_ShouldAllowValue(string name)
        {
            // Arrange
            var cleanEntity = new CleanEntity();

            // Act
            cleanEntity.Name = name;

            // Assert
            cleanEntity.Name.Should().Be(name ?? string.Empty);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void CleanEntity_SetDescriptionToNullOrEmpty_ShouldAllowValue(string description)
        {
            // Arrange
            var cleanEntity = new CleanEntity();

            // Act
            cleanEntity.Description = description;

            // Assert
            cleanEntity.Description.Should().Be(description ?? string.Empty);
        }

        [Fact]
        public void CleanEntity_WithValidNameAndDescription_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            var expectedName = "ValidCleanEntity";
            var expectedDescription = "Valid description for testing";

            // Act
            var cleanEntity = new CleanEntity
            {
                Name = expectedName,
                Description = expectedDescription
            };

            // Assert
            cleanEntity.Name.Should().Be(expectedName);
            cleanEntity.Description.Should().Be(expectedDescription);
        }

        [Fact]
        public void CleanEntity_WithLongName_ShouldAllowValue()
        {
            // Arrange
            var longName = new string('a', 200);
            var cleanEntity = new CleanEntity();

            // Act
            cleanEntity.Name = longName;

            // Assert
            cleanEntity.Name.Should().Be(longName);
        }

        [Fact]
        public void CleanEntity_WithLongDescription_ShouldAllowValue()
        {
            // Arrange
            var longDescription = new string('b', 1000);
            var cleanEntity = new CleanEntity();

            // Act
            cleanEntity.Description = longDescription;

            // Assert
            cleanEntity.Description.Should().Be(longDescription);
        }

        [Fact]
        public void CleanEntity_WithSpecialCharactersInName_ShouldAllowValue()
        {
            // Arrange
            var specialName = "Test!@#$%^&*()_+-=[]{}|;':\",./<>?";
            var cleanEntity = new CleanEntity();

            // Act
            cleanEntity.Name = specialName;

            // Assert
            cleanEntity.Name.Should().Be(specialName);
        }

        [Fact]
        public void CleanEntity_WithSpecialCharactersInDescription_ShouldAllowValue()
        {
            // Arrange
            var specialDescription = "Description with symbols: !@#$%^&*()_+-=[]{}|;':\",./<>?";
            var cleanEntity = new CleanEntity();

            // Act
            cleanEntity.Description = specialDescription;

            // Assert
            cleanEntity.Description.Should().Be(specialDescription);
        }

        [Fact]
        public void CleanEntity_Implementation_ShouldImplementICleanEntityInterface()
        {
            // Act
            var cleanEntity = new CleanEntity();

            // Assert
            cleanEntity.Should().BeAssignableTo<Authentication.Login.Domain.Interface.ICleanEntity>();
        }

        [Fact]
        public void CleanEntity_TwoInstancesWithSameData_ShouldBeEqual()
        {
            // Arrange
            var name = "TestName";
            var description = "TestDescription";
            
            var cleanEntity1 = new CleanEntity { Name = name, Description = description };
            var cleanEntity2 = new CleanEntity { Name = name, Description = description };

            // Assert
            cleanEntity1.Name.Should().Be(cleanEntity2.Name);
            cleanEntity1.Description.Should().Be(cleanEntity2.Description);
        }

        [Fact]
        public void CleanEntity_UpdateNameAfterCreation_ShouldReflectChanges()
        {
            // Arrange
            var cleanEntity = new CleanEntity { Name = "OriginalName" };
            var newName = "UpdatedName";

            // Act
            cleanEntity.Name = newName;

            // Assert
            cleanEntity.Name.Should().Be(newName);
        }

        [Fact]
        public void CleanEntity_UpdateDescriptionAfterCreation_ShouldReflectChanges()
        {
            // Arrange
            var cleanEntity = new CleanEntity { Description = "Original description" };
            var newDescription = "Updated description";

            // Act
            cleanEntity.Description = newDescription;

            // Assert
            cleanEntity.Description.Should().Be(newDescription);
        }

        [Fact]
        public void CleanEntity_InheritsFromEntity_ShouldHaveBaseProperties()
        {
            // Act
            var cleanEntity = new CleanEntity();

            // Assert
            cleanEntity.Should().BeAssignableTo<Foundation.Base.Domain.Implementation.Entity>();
            // Base entity properties should be available
            cleanEntity.Id.Should().Be(0);
            cleanEntity.IsActive.Should().BeTrue(); // Default value from base class
        }

        [Fact]
        public void CleanEntity_SetId_ShouldUpdateIdProperty()
        {
            // Arrange
            var cleanEntity = new CleanEntity();
            var expectedId = 123;

            // Act
            cleanEntity.Id = expectedId;

            // Assert
            cleanEntity.Id.Should().Be(expectedId);
        }

        [Fact]
        public void CleanEntity_SetIsActive_ShouldUpdateIsActiveProperty()
        {
            // Arrange
            var cleanEntity = new CleanEntity();

            // Act
            cleanEntity.IsActive = false;

            // Assert
            cleanEntity.IsActive.Should().BeFalse();
        }
    }
}