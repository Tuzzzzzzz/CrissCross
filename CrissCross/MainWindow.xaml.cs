using Logic;
using IO;
using System.Windows;

namespace CrissCross
{
    public partial class MainWindow : Window
    {
        private Game _crissCross;

        public MainWindow()
        {
            InitializeComponent();

            InputTextBox.Text =
                "мул" + '\n' +
                "сом" + '\n' +
                "крот" + '\n' +
                "енот" + '\n' +
                "койот" + '\n' +
                "панда" + '\n' +
                "тукан" + '\n' +
                "лемур" + '\n' +
                "мустанг" + '\n' +
                "гиппопотам";

            var textBoxReader = new TextBoxReader(InputTextBox);
            var canvasDrawer = new CanvasDrawer(OutputCanvas);
            _crissCross = new Game(textBoxReader, canvasDrawer);
        }

        public void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            _crissCross.Play();
        }
    }
}