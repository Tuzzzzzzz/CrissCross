using Logic;
using System.Windows.Controls;

namespace IO;

public class TextBoxReader : IStringListReader
{
    private TextBox _textBox;

    public TextBoxReader(TextBox textBox)
    {
        _textBox = textBox;
    }

    public List<string> Read()
        => new List<string>(
            _textBox.Text
            .Split(new char[] { '\n', ' ', '\r' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.ToLower())
            );
}