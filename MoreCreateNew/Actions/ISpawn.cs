using FrooxEngine;

namespace MoreCreateNew.Actions;

internal interface ISpawn
{
    string Category { get; }
    string Label { get; }
    void Spawn(Slot slot);
}
