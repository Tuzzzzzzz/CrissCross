namespace Logic;

public class Word
{
    private string _rWord;

    private int _x;

    private int _y;

    private Orientation _orient;


    public Word(string word, Orientation orient, int x = 0, int y = 0)
    {
        _rWord = word;

        _orient = orient;

        _x = x;

        _y = y;
    }


    public int Count => _rWord.Length;

    public string RawWord => _rWord;

    public char this[int index] => _rWord[index];


    public Orientation NegativeOrient
        => _orient == Orientation.Horizontal ? Orientation.Vertical : Orientation.Horizontal;

    public int XFrom(int index)
        => _orient == Orientation.Horizontal ? _x + index : _x;

    public int YFrom(int index)
        => _orient == Orientation.Horizontal ? _y : _y + index;

    public (int, int) XYFrom(int index) => (XFrom(index), YFrom(index));

    public int MinX => _x;

    public int MinY => _y;

    public int MaxX => XFrom(_rWord.Length - 1);

    public int MaxY => YFrom(_rWord.Length - 1);

    public (int, int) XYOver((int, int) xy)
    {
        var (x, y) = xy;
        return _orient == Orientation.Horizontal ? (x, y - 1) : (x - 1, y);
    }

    public (int, int) XYUnder((int, int) xy)
    {
        var (x, y) = xy;
        return _orient == Orientation.Horizontal ? (x, y + 1) : (x + 1, y);
    }

    public (int, int) LeftXY => XYFrom(-1);

    public (int, int) RightXY => XYFrom(_rWord.Length);

    public char CharAt((int, int) xy) => _rWord[IndexFrom(xy)];

    public int IndexFrom((int, int) xy)
    {
        var(x, y) = xy;
        return _orient == Orientation.Horizontal ? x - MinX : y - MinY;
    }

    public char CharAt(int index) => _rWord[index];

    public bool TryCharAt((int, int) xy, out char letter)
    {
        var (x, y) = xy;
        if (x < MinX || x > MaxX || y < MinY || y > MaxY)
        {
            letter = default(char);
            return false;
        }
        letter = CharAt(xy);
        return true;
    }

    public void NormalizeXY(int minXInArea, int minYInArea)
    {
        _x -= minXInArea;
        _y -= minYInArea;
    }

    public void Draw(ICellDrawer drawer)
    {
        for (int i = 0; i < Count; i++)
            drawer.Draw(_rWord[i], XFrom(i), YFrom(i));
    }
}