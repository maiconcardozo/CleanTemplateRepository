using Authentication.Login.Domain.Implementation;
using FluentAssertions;
using Xunit;

namespace Authentication.Tests.Unit
{
    public class EntityTemplateExampleEntityTests
    {
        [Fact]
        public void EntityTemplateExample_WhenCreated_ShouldHaveDefaultValues()
        {
            // Act
            var entity = new EntityTemplateExample();

            // Assert
            entity.Pro1.Should().Be(string.Empty);
            entity.Pro2.Should().Be(0);
            entity.Pro3.Should().Be(0);
            entity.Pro4.Should().Be(default(DateTime));
            entity.Pro5.Should().BeFalse();
            entity.Id.Should().Be(0);
        }

        [Fact]
        public void EntityTemplateExample_SetPro1_ShouldUpdatePro1Property()
        {
            // Arrange
            var entity = new EntityTemplateExample();
            var expectedValue = "test value";

            // Act
            entity.Pro1 = expectedValue;

            // Assert
            entity.Pro1.Should().Be(expectedValue);
        }

        [Fact]
        public void EntityTemplateExample_SetPro2_ShouldUpdatePro2Property()
        {
            // Arrange
            var entity = new EntityTemplateExample();
            var expectedValue = 42;

            // Act
            entity.Pro2 = expectedValue;

            // Assert
            entity.Pro2.Should().Be(expectedValue);
        }

        [Fact]
        public void EntityTemplateExample_SetPro3_ShouldUpdatePro3Property()
        {
            // Arrange
            var entity = new EntityTemplateExample();
            var expectedValue = 123.45m;

            // Act
            entity.Pro3 = expectedValue;

            // Assert
            entity.Pro3.Should().Be(expectedValue);
        }

        [Fact]
        public void EntityTemplateExample_SetPro4_ShouldUpdatePro4Property()
        {
            // Arrange
            var entity = new EntityTemplateExample();
            var expectedValue = new DateTime(2023, 10, 15);

            // Act
            entity.Pro4 = expectedValue;

            // Assert
            entity.Pro4.Should().Be(expectedValue);
        }

        [Fact]
        public void EntityTemplateExample_SetPro5_ShouldUpdatePro5Property()
        {
            // Arrange
            var entity = new EntityTemplateExample();
            var expectedValue = true;

            // Act
            entity.Pro5 = expectedValue;

            // Assert
            entity.Pro5.Should().Be(expectedValue);
        }

        [Fact]
        public void EntityTemplateExample_WithAllProperties_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            var expectedPro1 = "example";
            var expectedPro2 = 100;
            var expectedPro3 = 99.99m;
            var expectedPro4 = new DateTime(2024, 1, 1);
            var expectedPro5 = true;

            // Act
            var entity = new EntityTemplateExample
            {
                Pro1 = expectedPro1,
                Pro2 = expectedPro2,
                Pro3 = expectedPro3,
                Pro4 = expectedPro4,
                Pro5 = expectedPro5,
            };

            // Assert
            entity.Pro1.Should().Be(expectedPro1);
            entity.Pro2.Should().Be(expectedPro2);
            entity.Pro3.Should().Be(expectedPro3);
            entity.Pro4.Should().Be(expectedPro4);
            entity.Pro5.Should().Be(expectedPro5);
        }

        [Fact]
        public void EntityTemplateExample_Implementation_ShouldImplementIEntityTemplateExampleInterface()
        {
            // Arrange & Act
            var entity = new EntityTemplateExample();

            // Assert
            entity.Should().BeAssignableTo<Authentication.Login.Domain.Interface.IEntityTemplateExample>();
        }

        [Fact]
        public void EntityTemplateExample_TwoInstancesWithSameData_ShouldHaveEqualPropertyValues()
        {
            // Arrange
            var entity1 = new EntityTemplateExample
            {
                Pro1 = "test",
                Pro2 = 50,
                Pro3 = 25.5m,
                Pro4 = new DateTime(2023, 5, 10),
                Pro5 = true,
            };

            var entity2 = new EntityTemplateExample
            {
                Pro1 = "test",
                Pro2 = 50,
                Pro3 = 25.5m,
                Pro4 = new DateTime(2023, 5, 10),
                Pro5 = true,
            };

            // Act & Assert
            entity1.Pro1.Should().Be(entity2.Pro1);
            entity1.Pro2.Should().Be(entity2.Pro2);
            entity1.Pro3.Should().Be(entity2.Pro3);
            entity1.Pro4.Should().Be(entity2.Pro4);
            entity1.Pro5.Should().Be(entity2.Pro5);
        }

        [Fact]
        public void EntityTemplateExample_UpdatePropertiesAfterCreation_ShouldReflectChanges()
        {
            // Arrange
            var entity = new EntityTemplateExample
            {
                Pro1 = "initial",
                Pro2 = 10,
            };
            var newPro1 = "updated";
            var newPro2 = 20;

            // Act
            entity.Pro1 = newPro1;
            entity.Pro2 = newPro2;

            // Assert
            entity.Pro1.Should().Be(newPro1);
            entity.Pro2.Should().Be(newPro2);
        }
    }
}
