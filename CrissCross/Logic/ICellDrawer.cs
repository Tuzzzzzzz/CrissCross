namespace Logic;

public interface ICellDrawer
{
    public void Draw(char letter, int x, int y);

    //public void SetSizes(int width, int height)

    public void Clear();
}