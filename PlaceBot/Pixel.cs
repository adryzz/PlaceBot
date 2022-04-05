namespace PlaceBot;

public readonly struct Pixel
{
    public int X { get; }
    public int Y { get; }
    public byte Color { get; }

    public Pixel(int x, int y, byte color)
    {
        X = x;
        Y = y;
        Color = color;
    }
}