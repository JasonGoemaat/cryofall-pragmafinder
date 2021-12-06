# World Map Display

## ClientWorldMapEventVisualizer

This looks like it might be great for what we want...

```cs
public class ClientWorldMapEventVisualizer : BaseWorldMapVisualizer

private static readonly IWorldClientService ClientWorld = Api.Client.World;


public ClientWorldMapEventVisualizer(WorldMapController worldMapController)
    : base(worldMapController)
{
    ClientWorld.LogicObjectInitialized += this.LogicObjectInitializedHandler;
    ClientWorld.LogicObjectDeinitialized += this.LogicObjectDeinitializedHandler;

    foreach (var worldEvent in ClientWorld.GetGameObjectsOfProto<ILogicObject, IProtoEvent>())
    {
        this.OnWorldEventAdded(worldEvent);
    }
}

protected override void DisposeInternal()
{
    ClientWorld.LogicObjectInitialized -= this.LogicObjectInitializedHandler;
    ClientWorld.LogicObjectDeinitialized -= this.LogicObjectDeinitializedHandler;

    foreach (var worldEvent in ClientWorld.GetGameObjectsOfProto<ILogicObject, IProtoEvent>())
    {
        this.OnWorldEventRemoved(worldEvent);
    }
}

// add a circle for the search area
var publicState = worldEvent.GetPublicState<EventWithAreaPublicState>();
var circleRadius = publicState.AreaCircleRadius;
var circleCanvasPosition = this.WorldToCanvasPosition(publicState.AreaCirclePosition.ToVector2D());

var control = new WorldMapMarkEvent
{
    Width = 2 * circleRadius * WorldMapSectorProvider.WorldTileTextureSize,
    Height = 2 * circleRadius * WorldMapSectorProvider.WorldTileTextureSize,
    EllipseColorStroke = Api.Client.UI.GetApplicationResource<Color>("Color6"),
    EllipseColorStart = Api.Client.UI.GetApplicationResource<Color>("Color4").WithAlpha(0),
    EllipseColorEnd = Api.Client.UI.GetApplicationResource<Color>("Color4").WithAlpha(0x99)
};

Canvas.SetLeft(control, circleCanvasPosition.X - control.Width / 2);
Canvas.SetTop(control, circleCanvasPosition.Y - control.Height / 2);
Panel.SetZIndex(control, 1);
this.AddControl(control, false);
this.visualizedSearchAreas.Add((worldEvent, control));
ToolTipServiceExtend.SetToolTip(
    control,
    new WorldMapMarkEventTooltip()
    {
        Text = GetTooltipText(worldEvent),
        Icon = Api.Client.UI.GetTextureBrush(
            ((IProtoEvent)worldEvent.ProtoGameObject).Icon)
    });

```