namespace IO;

using Logic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;

public class CanvasDrawer : ICellDrawer
{
    public const int CellSize = 40;

    private Canvas _canvas;

    public CanvasDrawer(Canvas canvas)
    {
        _canvas = canvas;
    }

    //public void SetSizes(int width, int height)
    //{
    //    _canvas.Width = width;
    //    _canvas.Height = height;
    //}

    public void Draw(char letter, int x, int y)
    {
        // Создание текстового блока с буквой
        var textBlock = new TextBlock
        {
            Text = letter.ToString(),
            FontSize = 30,
            Foreground = Brushes.Black,
            TextAlignment = TextAlignment.Left // Устанавливаем выравнивание текста
        };

        // Создание прямоугольника для клетки
        var square = new Rectangle
        {
            Width = CellSize,
            Height = CellSize,
            Fill = Brushes.White
        };

        // Создание рамки вокруг клетки
        var border = new Border
        {
            Width = CellSize,
            Height = CellSize,
            BorderBrush = Brushes.Black,
            BorderThickness = new Thickness(1)
        };

        // Создание Grid для размещения текста
        var grid = new Grid();
        grid.Children.Add(square);
        grid.Children.Add(textBlock);

        // Устанавливаем Grid в качестве дочернего элемента рамки
        border.Child = grid;

        // Добавление рамки на холст
        _canvas.Children.Add(border);

        // Установка позиции рамки на холсте
        Canvas.SetLeft(border, x * CellSize);
        Canvas.SetTop(border, y * CellSize);

        // Установка позиции текста внутри клетки
        Canvas.SetLeft(textBlock, 5); // Отступ от левого края
        Canvas.SetTop(textBlock, 5); // Отступ от верхнего края
    }

    public void Clear() => _canvas.Children.Clear();
}
