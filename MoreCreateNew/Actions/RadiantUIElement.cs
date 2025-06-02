using System;
using System.Linq.Expressions;
using Elements.Core;
using FrooxEngine;
using FrooxEngine.UIX;

namespace MoreCreateNew.Actions;

internal sealed class RadiantUIElement : ISpawn
{
    public string Category => "Radiant UI";
    public string Label { get; private set; }

    private readonly float2 Size;
    private readonly Delegate Builder;
    private readonly bool SetupPanel;

    public RadiantUIElement(float2 size, LambdaExpression expression, bool setupPanel = false)
    {
        var callExpression = expression.Body as MethodCallExpression;
        if (callExpression == null)
        {
            throw new ArgumentException("Expression must be a method call");
        }

        Builder = expression.Compile();
        Label = callExpression.Method.Name;
        Size = size;
        SetupPanel = setupPanel;
    }

    public RadiantUIElement(string label, float2 size, Delegate builder, bool setupPanel = false)
    {
        Label = label;
        Size = size;
        Builder = builder;
        SetupPanel = setupPanel;
    }

    public void Spawn(Slot slot)
    {
        slot.LocalScale *= float3.One * 0.001f;

        if (SetupPanel)
        {
            Spawn(RadiantUI_Panel.SetupPanel(slot, Label, Size));
        }
        else
        {
            var canvas = slot.AttachComponent<Canvas>();
            canvas.Size.Value = Size;
            slot.AttachComponent<Grabbable>().Scalable.Value = true;

            var uiBuilder = new UIBuilder(canvas);

            RadiantUI_Constants.SetupDefaultStyle(uiBuilder);

            SpriteProvider spriteProvider = slot.AttachSprite(
                OfficialAssets.Graphics.UI.Circle.Light_Border.Circle_Phi2
            );
            spriteProvider.Borders.Value = float4.One * 0.5f;
            spriteProvider.FixedSize.Value = RadiantUI_Panel.BAR_SIZE;

            var panel = uiBuilder.Panel(
                RadiantUI_Constants.BG_COLOR,
                spriteProvider,
                uiBuilder.Style.NineSliceSizing,
                true
            );
            panel.Slot.AttachComponent<Mask>().ShowMaskGraphic.Value = true;

            uiBuilder.OverlappingLayout(RadiantUI_Constants.GRID_PADDING);

            Spawn(uiBuilder);
        }
    }

    private void Spawn(UIBuilder uiBuilder)
    {
        uiBuilder.PushStyle();
        Builder.DynamicInvoke(uiBuilder, Label);
        uiBuilder.PopStyle();
    }

    private static readonly float2 sq512 = float2.One * 512;
    private static readonly float2 hori512 = new float2(512, 64);

    private static void HeaderFooter(
        UIBuilder uiBuilder,
        string label,
        Func<(RectTransform, RectTransform)> action
    )
    {
        var (header, content) = action();

        uiBuilder.NestInto(header);
        uiBuilder.Text(label);

        uiBuilder.NestInto(content);
        uiBuilder.Text(label);
    }

    private static void HorizontalHeader(UIBuilder uiBuilder, string label, float size)
    {
        HeaderFooter(
            uiBuilder,
            label,
            () =>
            {
                uiBuilder.HorizontalHeader(size, out var header, out var content);
                return (header, content);
            }
        );
    }

    private static void HorizontalFooter(UIBuilder uiBuilder, string label, float size)
    {
        HeaderFooter(
            uiBuilder,
            label,
            () =>
            {
                uiBuilder.HorizontalFooter(size, out var header, out var content);
                return (header, content);
            }
        );
    }

    private static void FloatSlider(UIBuilder uiBuilder, float height)
    {
        uiBuilder.Slider<float>(height);
    }

    private static void VerticalHeader(UIBuilder uiBuilder, string label, float size)
    {
        HeaderFooter(
            uiBuilder,
            label,
            () =>
            {
                uiBuilder.VerticalHeader(size, out var header, out var content);
                return (header, content);
            }
        );
    }

    private static void VerticalFooter(UIBuilder uiBuilder, string label, float size)
    {
        HeaderFooter(
            uiBuilder,
            label,
            () =>
            {
                uiBuilder.VerticalFooter(size, out var header, out var content);
                return (header, content);
            }
        );
    }

    internal static readonly RadiantUIElement[] Actions = new RadiantUIElement[]
    {
        new(sq512, (UIBuilder uiBuilder, string label) => uiBuilder.Arc(label, true)),
        new(hori512, (UIBuilder uiBuilder, string label) => uiBuilder.Button(label)),
        new(
            "Checkbox R",
            hori512,
            (UIBuilder uiBuilder, string label) => uiBuilder.Checkbox(label, false, true, 4)
        ),
        new(
            "Checkbox L",
            hori512,
            (UIBuilder uiBuilder, string label) => uiBuilder.Checkbox(label, false, false, 4)
        ),
        new(sq512, (UIBuilder uiBuilder, string label) => uiBuilder.FitContent()),
        new(
            hori512,
            (UIBuilder uiBuilder, string label) =>
                uiBuilder.FloatField(float.MinValue, float.MaxValue, 2, null!, true)
        ),
        new(
            sq512,
            (UIBuilder uiBuilder, string label) =>
                uiBuilder.Gradient(
                    RadiantUI_Constants.Hero.RED,
                    RadiantUI_Constants.Hero.GREEN,
                    RadiantUI_Constants.Hero.CYAN,
                    RadiantUI_Constants.Hero.PURPLE
                )
        ),
        new(sq512, (UIBuilder uiBuilder, string label) => uiBuilder.GridLayout()),
        new(
            sq512,
            (UIBuilder uiBuilder, string label) =>
                HorizontalHeader(uiBuilder, label, RadiantUI_Constants.GRID_CELL_SIZE)
        ),
        new(
            sq512,
            (UIBuilder uiBuilder, string label) =>
                HorizontalFooter(uiBuilder, label, RadiantUI_Constants.GRID_CELL_SIZE)
        ),
        new(
            hori512,
            (UIBuilder uiBuilder, string label) =>
                uiBuilder.HorizontalElementWithLabel(
                    label,
                    RadiantUI_Constants.GRID_PADDING,
                    () => uiBuilder.Text(label, true, null, true, null!),
                    0.01f
                )
        ),
        new(
            sq512,
            (UIBuilder uiBuilder, string label) =>
                uiBuilder.HorizontalGradient(
                    RadiantUI_Constants.Hero.RED,
                    RadiantUI_Constants.Hero.GREEN
                )
        ),
        new(sq512, (UIBuilder uiBuilder, string label) => uiBuilder.HorizontalLayout(0, 0, null)),
        new(sq512, (UIBuilder uiBuilder, string label) => uiBuilder.IgnoreLayout()),
        new(
            sq512,
            (UIBuilder uiBuilder, string label) =>
                uiBuilder.Image(
                    RadiantUI_Constants.GetLineGridCellTransparentSprite(uiBuilder.World),
                    true
                )
        ),
        new(
            hori512,
            (UIBuilder uiBuilder, string label) =>
                uiBuilder.IntegerField(int.MinValue, int.MaxValue, 1, true)
        ),
        new(sq512, (UIBuilder uiBuilder, string label) => uiBuilder.Mask()),
        new(sq512, (UIBuilder uiBuilder, string label) => uiBuilder.OverlappingLayout(0, null)),
        new(sq512, (UIBuilder uiBuilder, string label) => uiBuilder.Panel(), true),
        new(sq512, (UIBuilder uiBuilder, string label) => uiBuilder.RawGraphic(null!, null!)),
        new(
            sq512,
            (UIBuilder uiBuilder, string label) =>
                uiBuilder.RawImage(
                    RadiantUI_Constants.GetLineGridCellTransparentTexture(uiBuilder.World),
                    true
                )
        ),
        new(
            sq512,
            (UIBuilder uiBuilder, string label) =>
                uiBuilder.RectMesh<AudioSourceWaveformMesh>(null!)
        ),
        new(
            sq512,
            (UIBuilder uiBuilder, string label) => uiBuilder.RectMesh<AudioSourceXYMesh>(null!)
        ),
        new(sq512, (UIBuilder uiBuilder, string label) => uiBuilder.RectMesh<LineGraphMesh>(null!)),
        new(sq512, (UIBuilder uiBuilder, string label) => uiBuilder.ScrollArea(null)),
        new(
            "Slider<int>",
            hori512,
            (UIBuilder uiBuilder, string label) =>
                uiBuilder.Slider(RadiantUI_Constants.GRID_CELL_SIZE)
        ),
        new(
            "Slider<float>",
            hori512,
            (UIBuilder uiBuilder, string label) =>
                uiBuilder.Slider<float>(RadiantUI_Constants.GRID_CELL_SIZE)
        ),
        new(sq512, (UIBuilder uiBuilder, string label) => uiBuilder.Spacer(1)),
        new(
            sq512,
            (UIBuilder uiBuilder, string label) =>
                uiBuilder.SpriteMask(RadiantUI_Constants.GetOutlinedSprite(uiBuilder.World), true)
        ),
        new(
            hori512,
            (UIBuilder uiBuilder, string label) => uiBuilder.Text(label, true, null, true, null!)
        ),
        new(
            hori512,
            (UIBuilder uiBuilder, string label) =>
                uiBuilder.TextField("", false, null!, true, default)
        ),
        new(
            sq512,
            (UIBuilder uiBuilder, string label) =>
                VerticalHeader(uiBuilder, label, RadiantUI_Constants.GRID_CELL_SIZE)
        ),
        new(
            sq512,
            (UIBuilder uiBuilder, string label) =>
                VerticalFooter(uiBuilder, label, RadiantUI_Constants.GRID_CELL_SIZE)
        ),
        new(
            sq512,
            (UIBuilder uiBuilder, string label) =>
                uiBuilder.VerticalGradient(
                    RadiantUI_Constants.Hero.RED,
                    RadiantUI_Constants.Hero.GREEN
                )
        ),
        new(
            sq512,
            (UIBuilder uiBuilder, string label) => uiBuilder.VerticalLayout(0, 0, null, null, null)
        ),
    };
}
