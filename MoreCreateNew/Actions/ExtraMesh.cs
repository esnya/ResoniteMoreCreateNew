using FrooxEngine;

namespace MoreCreateNew.Actions;

internal sealed class ExtraMesh<T> : ISpawn
    where T : ProceduralMesh
{
    public string Category => "3DModel/Others";
    public string Label { get; private set; } = typeof(T).Name.Replace("Mesh", "");

    public void Spawn(Slot slot)
    {
        DevCreateNewForm.SpawnMesh(slot, typeof(T));
    }
}

internal static class ExtraMesh
{
    public static readonly ISpawn[] Actions = new ISpawn[]
    {
        new ExtraMesh<ArrowMesh>(),
        new ExtraMesh<BallisticPathMesh>(),
        new ExtraMesh<BentTubeMesh>(),
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
    };
}
