using Microsoft.Maui.Graphics;
namespace Camping.Core.Models;

public class ClickArea
{
    public required double XPosition { get; set; }
    public required double YPosition { get; set; }
    public required double Width { get; set; }
    public required double Height { get; set; }
    public Rect Bounds => new Rect(XPosition, YPosition, Width, Height);
}
