namespace PlaceBot;

public readonly struct Pixel
{
    public ushort X { get; }
    public ushort Y { get; }
    public byte Color { get; }

    public Pixel(ushort x, ushort y, byte color)
    {
        X = x;
        Y = y;
        Color = color;
    }
}