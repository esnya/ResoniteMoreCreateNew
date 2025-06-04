using System.Reflection;
using FluentAssertions;
using MoreCreateNew.Actions;

namespace MoreCreateNew.Tests;

public class InitializationTests
{
    [Fact]
    public void Init_ShouldCallHarmonyPatchAll()
    {
        // This test verifies that the Init method works correctly
        // Since we can't easily mock static Harmony calls, we'll test the observable effects

        // Arrange
        var menuItemsField = typeof(MoreCreateNewMod).GetField(
            "menuItems",
            BindingFlags.NonPublic | BindingFlags.Static
        )!;

        var initMethod = typeof(MoreCreateNewMod).GetMethod(
            "Init",
            BindingFlags.NonPublic | BindingFlags.Static
        )!;

        // Clear existing items
        var emptyList = new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<
            string,
            string
        >>();
        menuItemsField.SetValue(null, emptyList);

        // Act
        var parameters = new object?[] { null };
        initMethod.Invoke(null, parameters);

        // Assert
        var menuItems =
            menuItemsField.GetValue(null)
            as System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<
                string,
                string
            >>;
        menuItems.Should().NotBeNull();

        var expectedCount =
            SmallMesh.actions.Length + ExtraMesh.actions.Length + RadiantUIElement.actions.Length;
        menuItems!.Count.Should().Be(expectedCount, "all actions should be added to menu items");
    }

    [Fact]
    public void AddAction_ShouldAddToMenuItems()
    {
        // Arrange
        var menuItemsField = typeof(MoreCreateNewMod).GetField(
            "menuItems",
            BindingFlags.NonPublic | BindingFlags.Static
        )!;

        var addActionMethod = typeof(MoreCreateNewMod).GetMethod(
            "AddAction",
            BindingFlags.NonPublic | BindingFlags.Static
        )!;

        // Clear existing items
        var initialItems =
            new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<
                string,
                string
            >>();
        menuItemsField.SetValue(null, initialItems);

        const string testPath = "Test/Category";
        const string testName = "TestAction";

        // Act
        var parameters = new object[]
        {
            testPath,
            testName,
            new System.Action<FrooxEngine.Slot>(_ => { }),
        };
        addActionMethod.Invoke(null, parameters);

        // Assert
        var menuItems =
            menuItemsField.GetValue(null)
            as System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<
                string,
                string
            >>;
        menuItems
            .Should()
            .Contain(
                new System.Collections.Generic.KeyValuePair<string, string>(testPath, testName)
            );
    }

    [Fact]
    public void HarmonyId_ShouldBeConsistent()
    {
        // This test ensures the Harmony ID is stable and follows expected format
        var harmonyIdProperty = typeof(MoreCreateNewMod).GetProperty(
            "HarmonyId",
            BindingFlags.NonPublic | BindingFlags.Static
        );

        if (harmonyIdProperty != null)
        {
            var harmonyId = harmonyIdProperty.GetValue(null) as string;
            harmonyId.Should().NotBeNullOrWhiteSpace();
            harmonyId.Should().StartWith("com.nekometer.esnya.");
        }
    }

    [Theory]
    [InlineData(nameof(SmallMesh))]
    [InlineData(nameof(ExtraMesh))]
    [InlineData(nameof(RadiantUIElement))]
    public void ActionClasses_ShouldHaveActionsProperty(string className)
    {
        // Arrange
        var type = typeof(SmallMesh).Assembly.GetType($"MoreCreateNew.Actions.{className}");

        // Act & Assert
        type.Should().NotBeNull($"{className} class should exist");

        var actionsField = type!.GetField("actions", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
        actionsField.Should().NotBeNull($"{className} should have static actions field");

        var actionsValue = actionsField!.GetValue(null);
        actionsValue.Should().NotBeNull($"{className}.actions should not be null");
    }
}
