using Elements.Core;
using ResoniteModLoader;
using System.Linq;
using System.Reflection;
using FrooxEngine;
using System.Collections.Generic;
using System;
using MoreCreateNew.Actions;

#if DEBUG
using ResoniteHotReloadLib;
#endif

namespace MoreCreateNew;

public partial class MoreCreateNewMod : ResoniteMod
{
    private static Assembly ModAssembly => typeof(MoreCreateNewMod).Assembly;

    public override string Name => ModAssembly.GetCustomAttribute<AssemblyTitleAttribute>().Title;
    public override string Author => ModAssembly.GetCustomAttribute<AssemblyCompanyAttribute>().Company;
    public override string Version => ModAssembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
    public override string Link => ModAssembly.GetCustomAttributes<AssemblyMetadataAttribute>().First(meta => meta.Key == "RepositoryUrl").Value;

    internal static string HarmonyId => $"com.nekometer.esnya.{ModAssembly.GetName()}";


    private static readonly List<KeyValuePair<string, string>> menuItems = new(SmallMesh.Actions.Length + ExtraMesh.Actions.Length + RadiantUIElement.Actions.Length);


    public override void OnEngineInit()
    {
        Init(this);

#if DEBUG
        HotReloader.RegisterForHotReload(this);
#endif
    }

    private static void AddAction(string path, string name, Action<Slot> action)
    {
        DevCreateNewForm.AddAction(path, name, action);
        menuItems.Add(new KeyValuePair<string, string>(path, name));
    }

    private static void Init(ResoniteMod modInstance)
    {
        foreach (var action in SmallMesh.Actions.Concat(ExtraMesh.Actions).Concat(RadiantUIElement.Actions))
        {
            AddAction(action.Category, action.Label, action.Spawn);
        }
    }

#if DEBUG
    public static void BeforeHotReload()
    {
        try
        {
            foreach (var pair in menuItems)
            {
                HotReloader.RemoveMenuOption(pair.Key, pair.Value);
            }
        }
        catch (Exception e)
        {
            Error(e);
        }
    }

    public static void OnHotReload(ResoniteMod modInstance)
    {
        Init(modInstance);
    }
#endif
}
