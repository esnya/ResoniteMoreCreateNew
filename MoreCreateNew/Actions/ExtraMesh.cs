using System;
using System.Linq;
using FrooxEngine;

namespace MoreCreateNew.Actions;

internal sealed class ExtraMesh<T> : ISpawn
    where T : ProceduralMesh
{
    public string Category => "3DModel/Others";
    public string Label { get; private set; } = GenerateLabel();

    private static string GenerateLabel()
    {
        var type = typeof(T);
        var name = type.Name;

        // Remove "Mesh" suffix
        if (name.EndsWith("Mesh", StringComparison.Ordinal))
        {
            name = name.Substring(0, name.Length - 4);
        }

        // Handle generic types
        if (type.IsGenericType)
        {
            // Remove generic type parameter count (e.g., "`1")
            var backtickIndex = name.IndexOf('`');
            if (backtickIndex >= 0)
            {
                name = name.Substring(0, backtickIndex);
            }

            // Add type arguments
            var args = type.GetGenericArguments();
            if (args.Length > 0)
            {
                var argNames = args.Select(arg =>
                {
                    var argName = arg.Name;
                    if (argName.EndsWith("Mesh", StringComparison.Ordinal))
                    {
                        argName = argName.Substring(0, argName.Length - 4);
                    }
                    return argName;
                });
                name += "<" + string.Join(",", argNames) + ">";
            }
        }

        return name;
    }

    public void Spawn(Slot slot)
    {
        DevCreateNewForm.SpawnMesh(slot, typeof(T));
    }
}

internal static class ExtraMesh
{
    public static readonly ISpawn[] actions =
    [
        new ExtraMesh<ArrowMesh>(),
        new ExtraMesh<BallisticPathMesh>(),
        new ExtraMesh<BentTubeMesh>(),
        new ExtraMesh<BevelBoxMesh>(),
        new ExtraMesh<BezierTubeMesh>(),
        new ExtraMesh<BoxArrayMesh>(),
        // new ExtraMesh<BoxMesh>(),
        new ExtraMesh<CameraFrustumMesh>(),
        // new ExtraMesh<CapsuleMesh>(),
        new ExtraMesh<CircleMesh>(),
        new ExtraMesh<CircleSegmentShaderMesh>(),
        new ExtraMesh<ColorDepthGrid>(),
        // new ExtraMesh<ConeMesh>(),
        new ExtraMesh<CrossMesh>(),
        new ExtraMesh<CurvedPlaneMesh>(),
        // new ExtraMesh<CylinderMesh>(),
        new ExtraMesh<FrameMesh>(),
        // new ExtraMesh<GridMesh>(),
        new ExtraMesh<HollowConeMesh>(),
        new ExtraMesh<IcoSphereMesh>(),
        new ExtraMesh<ImageColorDistributionGraph>(),
        new ExtraMesh<LabelPointerMesh>(),
        new ExtraMesh<LightningMesh>(),
        new ExtraMesh<MultiLineMesh>(),
        new ExtraMesh<MultiSegmentMesh>(),
        new ExtraMesh<PointClusterMesh>(),
        new ExtraMesh<PointMesh>(),
        new ExtraMesh<QuadArrayMesh>(),
        new ExtraMesh<QuadMesh>(),
        new ExtraMesh<RampMesh>(),
        new ExtraMesh<RingMesh>(),
        new ExtraMesh<SegmentMesh>(),
        new ExtraMesh<SlotSegmentMesh>(),
        new ExtraMesh<SphereMesh>(),
        new ExtraMesh<StandaloneRectMesh<AudioSourceWaveformMesh>>(),
        new ExtraMesh<StandaloneRectMesh<AudioSourceXYMesh>>(),
        new ExtraMesh<StandaloneRectMesh<LineGraphMesh>>(),
        new ExtraMesh<StripeMesh>(),
        // new ExtraMesh<TorusMesh>(),
        new ExtraMesh<TriangleDiagnosticMesh>(),
        //new ExtraMesh<TriangleMesh>(),
        new ExtraMesh<TubeBoxMesh>(),
        new ExtraMesh<TubeMesh>(),
        new ExtraMesh<TubeSpiralMesh>(),
        new ExtraMesh<TubeWireMesh>(),
        new ExtraMesh<WrapperMesh>(),
    ];
}
