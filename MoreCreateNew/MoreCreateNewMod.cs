using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Elements.Core;
using FrooxEngine;
using HarmonyLib;
using MoreCreateNew.Actions;
using ResoniteModLoader;
#if DEBUG
using ResoniteHotReloadLib;
#endif

namespace MoreCreateNew;

/// <summary>
/// Represents the main mod class for MoreCreateNew.
/// Provides additional create new options for Resonite.
/// </summary>
public class MoreCreateNewMod : ResoniteMod
{
    private static readonly Assembly Assembly = typeof(MoreCreateNewMod).Assembly;

    /// <inheritdoc />
    public override string Name => Assembly.GetCustomAttribute<AssemblyTitleAttribute>().Title;

    /// <inheritdoc />
    public override string Author =>
        Assembly.GetCustomAttribute<AssemblyCompanyAttribute>().Company;

    /// <inheritdoc />
    public override string Version =>
        Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

    /// <inheritdoc />
    public override string Link =>
        Assembly
            .GetCustomAttributes<AssemblyMetadataAttribute>()
            .FirstOrDefault(meta => meta.Key == "RepositoryUrl")
            ?.Value ?? string.Empty;

    private static string HarmonyId => $"com.nekometer.esnya.{Assembly.GetName().Name}";

    private static readonly Harmony harmony = new(HarmonyId);
    private static readonly List<KeyValuePair<string, string>> menuItems = new(
        SmallMesh.Actions.Length + ExtraMesh.Actions.Length + RadiantUIElement.Actions.Length
    );

    /// <inheritdoc />
    public override void OnEngineInit()
    {
        Init(this);

#if DEBUG
        HotReloader.RegisterForHotReload(this);
#endif
    }

    /// <summary>
    /// Initializes the mod by applying Harmony patches and adding create new actions.
    /// </summary>
    /// <param name="mod">The mod instance to initialize.</param>
#pragma warning disable IDE0060 // Remove unused parameter
    private static void Init(ResoniteMod? mod)
#pragma warning restore IDE0060 // Remove unused parameter
    {
        harmony.PatchAll();

        foreach (
            var action in SmallMesh
                .Actions.Concat(ExtraMesh.Actions)
                .Concat(RadiantUIElement.Actions)
        )
        {
            AddAction(action.Category, action.Label, action.Spawn);
        }
    }

    private static void AddAction(string path, string name, Action<Slot> action)
    {
        DevCreateNewForm.AddAction(path, name, action);
        menuItems.Add(new KeyValuePair<string, string>(path, name));
    }

#if DEBUG
    /// <summary>
    /// Called before hot reload occurs. Removes all menu items and Harmony patches.
    /// </summary>
    public static void BeforeHotReload()
    {
        try
        {
            foreach (var pair in menuItems)
            {
                HotReloader.RemoveMenuOption(pair.Key, pair.Value);
            }
            harmony.UnpatchAll(HarmonyId);
        }
        catch (Exception e)
        {
            Error(e);
        }
    }

    /// <summary>
    /// Called after hot reload occurs. Re-initializes the mod.
    /// </summary>
    /// <param name="mod">The mod instance to re-initialize.</param>
    public static void OnHotReload(ResoniteMod mod)
    {
        Init(mod);
    }
#endif
}
