using System.Linq;
using System.Reflection;
using FluentAssertions;

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
    public void Name_Property_ShouldReturnExpectedValue()
    {
        // Arrange
        var mod = new MoreCreateNewMod();

        // Act
        var name = mod.Name;

        // Assert
        name.Should().NotBeNull().And.Be("MoreCreateNew");
    }

    [Fact]
    public void Author_Property_ShouldReturnExpectedValue()
    {
        // Arrange
        var mod = new MoreCreateNewMod();

        // Act
        var author = mod.Author;

        // Assert
        author.Should().NotBeNull().And.Be("esnya");
    }

    [Fact]
    public void Version_Property_ShouldStartWithExpectedValue()
    {
        // Arrange
        var mod = new MoreCreateNewMod();

        // Act
        var version = mod.Version;

        // Assert
        version.Should().NotBeNull().And.NotContain("+");
    }

    [Fact]
    public void Link_Property_ShouldReturnExpectedValue()
    {
        // Arrange
        var mod = new MoreCreateNewMod();

        // Act
        var link = mod.Link;

        // Assert
        link.Should().NotBeNull().And.Be("https://github.com/esnya/ResoniteMoreCreateNew");
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
}
