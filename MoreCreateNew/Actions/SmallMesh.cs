using System;
using FrooxEngine;

namespace MoreCreateNew.Actions;

internal sealed class SmallMesh<T> : ISpawn
    where T : ProceduralMesh
{
    public string Category => "3DModel/Small";
    public string Label { get; private set; } = typeof(T).Name.Replace("Mesh", "");
    private readonly Action<T> scaler;

    public SmallMesh(Action<T> scaler)
    {
        this.scaler = scaler;
    }

    public void Spawn(Slot slot)
    {
        DevCreateNewForm.SpawnMesh(slot, typeof(T));
        scaler(slot.GetComponent<T>());
    }
}

internal static class SmallMesh
{
    public static readonly ISpawn[] actions =
    [
        new SmallMesh<BoxMesh>(m => m.Size.Value *= 0.1f),
        new SmallMesh<CapsuleMesh>(m =>
        {
            m.Radius.Value *= 0.1f;
            m.Height.Value *= 0.1f;
        }),
        new SmallMesh<ConeMesh>(m =>
        {
            m.RadiusBase.Value *= 0.1f;
            m.RadiusTop.Value *= 0.1f;
            m.Height.Value *= 0.1f;
        }),
        new SmallMesh<CylinderMesh>(m =>
        {
            m.Radius.Value *= 0.1f;
            m.Height.Value *= 0.1f;
        }),
        new SmallMesh<GridMesh>(m => m.Size.Value *= 0.1f),
        new SmallMesh<QuadMesh>(m => m.Size.Value *= 0.1f),
        new SmallMesh<SphereMesh>(m => m.Radius.Value *= 0.1f),
        new SmallMesh<TorusMesh>(m =>
        {
            m.MajorRadius.Value *= 0.1f;
            m.MinorRadius.Value *= 0.1f;
        }),
        new SmallMesh<TriangleMesh>(m =>
        {
            m.Vertex0.Position.Value *= 0.1f;
            m.Vertex1.Position.Value *= 0.1f;
            m.Vertex2.Position.Value *= 0.1f;
        }),
    ];
}
