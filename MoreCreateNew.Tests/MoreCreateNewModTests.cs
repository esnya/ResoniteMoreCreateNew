using System.Linq;
using System.Reflection;
using FluentAssertions;
using System.Collections.Generic;
using System;
using FrooxEngine;
using MoreCreateNew.Actions;

namespace MoreCreateNew.Tests;

public class MoreCreateNewModTests
{
    [Fact]
    public void MoreCreateNewMod_CanBeInstantiated()
    {
        // Act & Assert - Should not throw
        var modCreation = () => new MoreCreateNewMod();
        var mod = modCreation.Should().NotThrow().Subject;
        mod.Should().NotBeNull();
    }

    [Fact]
    public void Assembly_ShouldHaveExpectedAttributes()
    {
        // Arrange
        var assembly = typeof(MoreCreateNewMod).Assembly;

        // Act & Assert
        var titleAttribute = assembly.GetCustomAttribute<AssemblyTitleAttribute>();
        titleAttribute.Should().NotBeNull();
        titleAttribute!.Title.Should().NotBeNullOrWhiteSpace();

        var companyAttribute = assembly.GetCustomAttribute<AssemblyCompanyAttribute>();
        companyAttribute.Should().NotBeNull();
        companyAttribute!.Company.Should().NotBeNullOrWhiteSpace();

        var versionAttribute = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
        versionAttribute.Should().NotBeNull();
        versionAttribute!.InformationalVersion.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public void Assembly_ShouldHaveRepositoryUrlMetadata()
    {
        // Arrange
        var assembly = typeof(MoreCreateNewMod).Assembly;

        // Act
        var metadataAttributes = assembly.GetCustomAttributes<AssemblyMetadataAttribute>();
        var repositoryUrlAttribute = metadataAttributes.FirstOrDefault(attr =>
            attr.Key == "RepositoryUrl"
        );

        // Assert
        repositoryUrlAttribute.Should().NotBeNull("assembly should have RepositoryUrl metadata");
        repositoryUrlAttribute!.Value.Should().NotBeNullOrWhiteSpace();
    }

    private static FieldInfo MenuItemsField =>
        typeof(MoreCreateNewMod).GetField(
            "menuItems",
            BindingFlags.NonPublic | BindingFlags.Static
        )!;

    private static MethodInfo AddActionMethod =>
        typeof(MoreCreateNewMod).GetMethod(
            "AddAction",
            BindingFlags.NonPublic | BindingFlags.Static
        )!;

    private static MethodInfo InitMethod =>
        typeof(MoreCreateNewMod).GetMethod("Init", BindingFlags.NonPublic | BindingFlags.Static)!;

    private static void ResetMenuItems()
    {
        MenuItemsField.SetValue(null, new List<KeyValuePair<string, string>>());
    }

    [Fact]
    public void AddAction_ShouldStoreMenuItem()
    {
        // Arrange
        ResetMenuItems();

        const string path = "Test/Path";
        const string name = "Item";

        // Act
        AddActionMethod.Invoke(null, [path, name, (Action<Slot>)(_ => { })]);

        // Assert
        var menuItems = (List<KeyValuePair<string, string>>)MenuItemsField.GetValue(null)!;
        menuItems.Should().Contain(new KeyValuePair<string, string>(path, name));
    }

    [Fact]
    public void Init_ShouldRegisterAllActions()
    {
        // Arrange
        ResetMenuItems();

        var expectedCount =
            SmallMesh.actions.Length + ExtraMesh.actions.Length + RadiantUIElement.actions.Length;

        // Act
        InitMethod.Invoke(null, [null]);

        // Assert
        var menuItems = (List<KeyValuePair<string, string>>)MenuItemsField.GetValue(null)!;
        menuItems.Should().HaveCount(expectedCount);
    }
}
