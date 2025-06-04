using System;
using System.Linq;
using FluentAssertions;
using FrooxEngine;
using MoreCreateNew.Actions;

namespace MoreCreateNew.Tests.Actions;

public class SpawnBehaviorTests
{
    [Fact]
    public void SmallMesh_Constructor_ShouldSetProperties()
    {
        // Arrange
        static void testScaler(BoxMesh mesh) { }

        // Act
        var smallMesh = new SmallMesh<BoxMesh>(testScaler);

        // Assert
        smallMesh.Category.Should().Be("3DModel/Small");
        smallMesh.Label.Should().Be("Box");
    }

    [Fact]
    public void SmallMesh_Label_ShouldRemoveMeshSuffix()
    {
        // Arrange
        static void testScaler(SphereMesh mesh) { }

        // Act
        var smallMesh = new SmallMesh<SphereMesh>(testScaler);

        // Assert
        smallMesh.Label.Should().Be("Sphere");
        smallMesh.Label.Should().NotContain("Mesh");
    }

    [Fact]
    public void ExtraMesh_Constructor_ShouldSetProperties()
    {
        // Act
        var extraMesh = new ExtraMesh<ArrowMesh>();

        // Assert
        extraMesh.Category.Should().Be("3DModel/Others");
        extraMesh.Label.Should().Be("Arrow");
    }

    [Fact]
    public void ExtraMesh_Label_ShouldRemoveMeshSuffix()
    {
        // Act
        var extraMesh = new ExtraMesh<ConeMesh>();

        // Assert
        extraMesh.Label.Should().Be("Cone");
        extraMesh.Label.Should().NotContain("Mesh");
    }

    [Theory]
    [InlineData(typeof(BoxMesh), "Box")]
    [InlineData(typeof(SphereMesh), "Sphere")]
    [InlineData(typeof(CylinderMesh), "Cylinder")]
    [InlineData(typeof(CapsuleMesh), "Capsule")]
    public void SmallMesh_Generic_ShouldHaveCorrectLabel(Type meshType, string expectedLabel)
    {
        // Arrange
        var scalerType = typeof(Action<>).MakeGenericType(meshType);
        var scaler = CreateDummyScaler(scalerType);
        var smallMeshType = typeof(SmallMesh<>).MakeGenericType(meshType);

        // Act
        var instance = Activator.CreateInstance(smallMeshType, scaler) as ISpawn;

        // Assert
        instance.Should().NotBeNull();
        instance!.Label.Should().Be(expectedLabel);
        instance.Category.Should().Be("3DModel/Small");
    }

    private static Delegate CreateDummyScaler(Type scalerType)
    {
        var method = typeof(SpawnBehaviorTests).GetMethod(
            nameof(DummyScalerGeneric),
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static
        );
        var genericMethod = method!.MakeGenericMethod(scalerType.GetGenericArguments()[0]);
        return Delegate.CreateDelegate(scalerType, genericMethod);
    }

    private static void DummyScalerGeneric<T>(T _)
        where T : ProceduralMesh
    {
        // This is just a dummy scaler for testing
    }

    [Theory]
    [InlineData(typeof(ArrowMesh), "Arrow")]
    [InlineData(typeof(ConeMesh), "Cone")]
    [InlineData(typeof(TorusMesh), "Torus")]
    [InlineData(typeof(QuadMesh), "Quad")]
    public void ExtraMesh_Generic_ShouldHaveCorrectLabel(Type meshType, string expectedLabel)
    {
        // Arrange
        var extraMeshType = typeof(ExtraMesh<>).MakeGenericType(meshType);

        // Act
        var instance = Activator.CreateInstance(extraMeshType) as ISpawn;

        // Assert
        instance.Should().NotBeNull();
        instance!.Label.Should().Be(expectedLabel);
        instance.Category.Should().Be("3DModel/Others");
    }

    [Fact]
    public void AllSpawnActions_ShouldHaveValidSpawnMethod()
    {
        // Test that all spawn actions have valid Spawn method
        var allActions = Array
            .Empty<ISpawn>()
            .Concat(SmallMesh.actions)
            .Concat(ExtraMesh.actions)
            .Concat(RadiantUIElement.actions);

        foreach (var action in allActions)
        {
            // Act & Assert - Should not throw ArgumentNullException when checking method exists
            var spawnMethod = action.GetType().GetMethod("Spawn");
            spawnMethod.Should().NotBeNull("All spawn actions should have a Spawn method");

            // Note: We can't actually call Spawn due to Resonite dependencies,
            // but we can verify the method signature exists
            var parameters = spawnMethod!.GetParameters();
            parameters.Should().HaveCount(1, "Spawn method should take one parameter");
            parameters[0]
                .ParameterType.Name.Should()
                .Be("Slot", "Spawn method should take a Slot parameter");
        }
    }

    [Fact]
    public void ActionCounts_ShouldMatchExpectedValues()
    {
        // Test specific counts to ensure all actions are present
        SmallMesh
            .actions.Should()
            .HaveCount(9, "SmallMesh should have 9 actions based on actual implementation");
        ExtraMesh
            .actions.Should()
            .HaveCountGreaterThan(10, "ExtraMesh should have many different mesh types");
        RadiantUIElement
            .actions.Should()
            .HaveCountGreaterThan(5, "RadiantUIElement should have several UI elements");
    }
}
