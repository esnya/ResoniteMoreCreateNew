# More Create New

A [ResoniteModLoader](https://github.com/resonite-modding-group/ResoniteModLoader) mod that adds useful items to Resonite's "Create New" menu.

## Features

This mod extends Resonite's "Create New" menu with the following categories:

### 3DModel/Small

Small-sized basic primitive meshes (0.1x scale of normal size):

- Box, Capsule, Cone, Cylinder, Grid, Quad, Sphere, Torus, Triangle

### 3DModel/Others

Various other mesh objects:

- Arrow, Ballistic Path, Bent Tube, Bezier Tube, Box Array, Camera Frustum
- Circle, Circle Segment Shader, Color Depth Grid, Cross, Curved Plane
- Frame, Hollow Cone, Ico Sphere, Image Color Distribution Graph
- Label Pointer, Lightning, Multi Line, Multi Segment, Point Cluster, Point
- Quad Array, Ramp, Ring, Segment, Slot Segment, Stripe
- Triangle Diagnostic, Tube Box, Tube, Tube Spiral, Tube Wire, Wrapper
- And many more procedural meshes

### Radiant UI

Various Radiant UI elements:

- Arc, Button, Checkbox (Left/Right), Float Field, Gradient
- Grid Layout, Headers/Footers, Horizontal/Vertical Layout
- Image, Integer Field, Mask, Panel, Scroll Area
- Slider (int/float), Spacer, Sprite Mask, Text, Text Field
- And other UI components

## Installation

1. Install the [ResoniteModLoader](https://github.com/resonite-modding-group/ResoniteModLoader).
2. Place the [MoreCreateNew.dll](https://github.com/esnya/ResoniteMoreCreateNew/releases/latest/download/MoreCreateNew.dll) into your `rml_mods` folder. This folder should be located at `C:\Program Files (x86)\Steam\steamapps\common\Resonite\rml_mods` for a standard installation. You can create it if it's missing, or if you start the game once with the ResoniteModLoader installed it will create this folder for you.
3. Launch the game. If you want to check that the mod is working you can check your Resonite logs.

## Usage

1. Open the developer tools in Resonite
2. Select "Create New"
3. Choose items from the newly added categories ("3DModel/Small", "3DModel/Others", "Radiant UI")

## Development Requirements

For development, you will need the [ResoniteHotReloadLib](https://github.com/Nytra/ResoniteHotReloadLib) to be able to hot reload your mod with DEBUG build.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
