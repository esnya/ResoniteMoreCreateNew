using System;
using System.Linq;
using System.Linq.Expressions;
using Elements.Core;
using FluentAssertions;
using FrooxEngine;
using FrooxEngine.UIX;
using MoreCreateNew.Actions;

namespace MoreCreateNew.Tests.Actions;

public class SpawnActionTests
{
    [Theory]
    [InlineData(typeof(ExtraMesh<ArrowMesh>), "3DModel/Others", "Arrow")]
    [InlineData(typeof(ExtraMesh<SphereMesh>), "3DModel/Others", "Sphere")]
    public void ExtraMesh_Generic_ShouldHaveCorrectProperties(
        Type meshType,
        string expectedCategory,
        string expectedLabel
    )
    {
        // Arrange & Act
        var meshInstance = Activator.CreateInstance(meshType) as ISpawn;

        // Assert
        meshInstance.Should().NotBeNull();
        meshInstance!.Category.Should().Be(expectedCategory);
        meshInstance.Label.Should().Be(expectedLabel);
    }

    [Fact]
    public void SmallMesh_Actions_ShouldAllHaveCorrectCategory()
    {
        // Arrange
        var expectedCategory = "3DModel/Small";

        // Act
        var actions = SmallMesh.actions;

        // Assert
        actions
            .Should()
            .NotBeNull()
            .And.NotBeEmpty()
            .And.AllSatisfy(action =>
            {
                action.Should().NotBeNull();
                action.Category.Should().Be(expectedCategory);
                action.Label.Should().NotBeNullOrWhiteSpace();
            });
    }

    [Fact]
    public void ExtraMesh_Actions_ShouldAllHaveCorrectCategory()
    {
        // Arrange
        var expectedCategory = "3DModel/Others";

        // Act
        var actions = ExtraMesh.actions;

        // Assert
        actions
            .Should()
            .NotBeNull()
            .And.NotBeEmpty()
            .And.AllSatisfy(action =>
            {
                action.Should().NotBeNull();
                action.Category.Should().Be(expectedCategory);
                action.Label.Should().NotBeNullOrWhiteSpace();
            });
    }

    [Fact]
    public void RadiantUIElement_Actions_ShouldAllHaveCorrectCategory()
    {
        // Arrange
        var expectedCategory = "Radiant UI";

        // Act
        var actions = RadiantUIElement.actions;

        // Assert
        actions
            .Should()
            .NotBeNull()
            .And.NotBeEmpty()
            .And.AllSatisfy(action =>
            {
                action.Should().NotBeNull();
                action.Category.Should().Be(expectedCategory);
                action.Label.Should().NotBeNullOrWhiteSpace();
            });
    }

    [Fact]
    public void AllActions_ShouldHaveUniqueLabelsWithinEachCategory()
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

    [Fact]
    public void AllActions_ShouldHaveExpectedTotalCount()
    {
        // Arrange
        var allActions = SmallMesh
            .actions.Concat(ExtraMesh.actions)
            .Concat(RadiantUIElement.actions);

        // Act & Assert
        allActions
            .Should()
            .HaveCount(83, "the total number of actions should match the expected count");
    }

    [Theory]
    [InlineData("3DModel/Small")]
    [InlineData("3DModel/Others")]
    [InlineData("Radiant UI")]
    public void AllActions_ShouldContainExpectedCategories(string expectedCategory)
    {
        // Arrange
        var allActions = SmallMesh
            .actions.Concat(ExtraMesh.actions)
            .Concat(RadiantUIElement.actions);

        // Act
        var categories = allActions.Select(a => a.Category).Distinct();

        // Assert
        categories.Should().Contain(expectedCategory);
    }

    [Fact]
    public void SmallMesh_Actions_ShouldOnlyContainSmallMeshInstances()
    {
        // Act
        var actions = SmallMesh.actions;

        // Assert
        actions
            .Should()
            .AllSatisfy(action =>
            {
                action
                    .GetType()
                    .Should()
                    .Match(type =>
                        type.IsGenericType
                        && type.GetGenericTypeDefinition().Name.StartsWith("SmallMesh")
                    );
            });
    }

    [Fact]
    public void ExtraMesh_Actions_ShouldOnlyContainExtraMeshInstances()
    {
        // Act
        var actions = ExtraMesh.actions;

        // Assert
        actions
            .Should()
            .AllSatisfy(action =>
            {
                action
                    .GetType()
                    .Should()
                    .Match(type =>
                        type.IsGenericType
                        && type.GetGenericTypeDefinition().Name.StartsWith("ExtraMesh")
                    );
            });
    }

    [Fact]
    public void ISpawn_Interface_ShouldBeImplementedCorrectly()
    {
        // Arrange - Test with a simple concrete implementation
        var spawn = new ExtraMesh<ArrowMesh>();

        // Act & Assert
        spawn.Should().BeAssignableTo<ISpawn>();
        spawn.Category.Should().NotBeNullOrWhiteSpace();
        spawn.Label.Should().NotBeNullOrWhiteSpace();

        // Verify that Spawn method exists and basic properties work
        spawn.Category.Should().Be("3DModel/Others");
        spawn.Label.Should().Be("Arrow");
    }

    [Fact]
    public void GenericMeshTypes_ShouldGenerateCorrectLabels()
    {
        // Arrange & Act
        var standaloneRectAction = ExtraMesh.actions.FirstOrDefault(a =>
            a.Label.Contains("LineGraph")
        );

        // Assert
        standaloneRectAction.Should().NotBeNull();
        standaloneRectAction!.Label.Should().Be("StandaloneRectMesh<LineGraph>");
    }

    [Fact]
    public void MeshLabel_ShouldRemoveMeshSuffix()
    {
        // Arrange & Act
        // Note: BoxMesh is commented out in ExtraMesh.actions, so we test with available meshes
        var arrowAction = ExtraMesh.actions.FirstOrDefault(a => a.Label == "Arrow");
        var sphereAction = ExtraMesh.actions.FirstOrDefault(a => a.Label == "Sphere");

        // Assert
        arrowAction.Should().NotBeNull("Arrow mesh should exist");
        sphereAction.Should().NotBeNull("Sphere mesh should exist");

        // Verify that the "Mesh" suffix was properly removed
        arrowAction!.Label.Should().NotEndWith("Mesh");
        sphereAction!.Label.Should().NotEndWith("Mesh");
    }

    [Theory]
    [InlineData("Box")]
    [InlineData("Capsule")]
    [InlineData("Sphere")]
    public void SmallMesh_Actions_ShouldContainExpectedLabels(string label)
    {
        // Act
        var action = SmallMesh.actions.FirstOrDefault(a => a.Label == label);

        // Assert
        action.Should().NotBeNull();
        action!.Category.Should().Be("3DModel/Small");
    }

    [Fact]
    public void ExtraMesh_NestedGenericLabel_ShouldFormatCorrectly()
    {
        // Arrange & Act
        var action = new ExtraMesh<StandaloneRectMesh<AudioSourceWaveformMesh>>();

        // Assert
        action.Label.Should().Be("StandaloneRectMesh<AudioSourceWaveform>");
        action.Category.Should().Be("3DModel/Others");
    }

    [Fact]
    public void RadiantUIElement_ExpressionConstructor_ShouldSetLabel()
    {
        // Arrange
        Expression<Action<UIBuilder>> expr = ui => ui.FitContent();
        var element = new RadiantUIElement(float2.One, expr);

        // Assert
        element.Label.Should().Be("FitContent");
        element.Category.Should().Be("Radiant UI");
    }
}
