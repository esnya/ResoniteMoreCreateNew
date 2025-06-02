using System.Linq;
using FluentAssertions;
using MoreCreateNew.Actions;

namespace MoreCreateNew.Tests;

public class ActionsTests
{
    [Fact]
    public void SmallMesh_Actions_ShouldNotBeEmpty()
    {
        // Act
        var actions = SmallMesh.actions;

        // Assert
        actions.Should().NotBeNull().And.NotBeEmpty();
    }

    [Fact]
    public void SmallMesh_Actions_ShouldHaveCorrectCategory()
    {
        // Act
        var actions = SmallMesh.actions;

        // Assert
        actions.Should().AllSatisfy(action => action.Category.Should().Be("3DModel/Small"));
    }

    [Fact]
    public void ExtraMesh_Actions_ShouldNotBeEmpty()
    {
        // Act
        var actions = ExtraMesh.actions;

        // Assert
        actions.Should().NotBeNull().And.NotBeEmpty();
    }

    [Fact]
    public void ExtraMesh_Actions_ShouldHaveCorrectCategory()
    {
        // Act
        var actions = ExtraMesh.actions;

        // Assert
        actions.Should().AllSatisfy(action => action.Category.Should().Be("3DModel/Others"));
    }

    [Fact]
    public void RadiantUIElement_Actions_ShouldNotBeEmpty()
    {
        // Act
        var actions = RadiantUIElement.actions;

        // Assert
        actions.Should().NotBeNull().And.NotBeEmpty();
    }

    [Fact]
    public void RadiantUIElement_Actions_ShouldHaveCorrectCategory()
    {
        // Act
        var actions = RadiantUIElement.actions;

        // Assert
        actions.Should().AllSatisfy(action => action.Category.Should().Be("Radiant UI"));
    }

    [Fact]
    public void AllActions_ShouldHaveExpectedCount()
    {
        // Arrange
        var allActions = SmallMesh
            .actions.Concat(ExtraMesh.actions)
            .Concat(RadiantUIElement.actions)
            .ToList();

        // Act & Assert
        allActions
            .Should()
            .HaveCount(83, "the total number of actions should match the expected count");
    }

    [Fact]
    public void AllActions_ShouldHaveUniqueLabelsWithinCategory()
    {
        // Arrange
        var allActions = SmallMesh
            .actions.Concat(ExtraMesh.actions)
            .Concat(RadiantUIElement.actions);

        // Act
        var groupedByCategory = allActions.GroupBy(a => a.Category);

        // Assert
        groupedByCategory
            .Should()
            .AllSatisfy(categoryGroup =>
            {
                var labels = categoryGroup.Select(a => a.Label).ToList();
                labels
                    .Should()
                    .OnlyHaveUniqueItems(
                        $"Category '{categoryGroup.Key}' should not have duplicate labels"
                    );
            });
    }
}
