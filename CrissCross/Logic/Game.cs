using System.Diagnostics;
using System.Reflection;

namespace Logic;

public class Game
{
    IStringListReader _reader;

    ICellDrawer _drawer;

    //текущее решение
    private WordArea? _currentWordArea = null;

    private List<string> _rawWords = new List<string>();

    private int _currentSpace = 1000;

    private double _currentDensity = -1;

    private double _alpha;

    public int cnt = 0;


    public Game(IStringListReader reader, ICellDrawer drawer)
    {
        _reader = reader;
        _drawer = drawer;
    }


    public void Play()
    {
        _drawer.Clear();

        ReadWords();

        if (_rawWords.Count != 0)
        {
            _alpha = 1.0 / Math.Pow(1.4, _rawWords.Count);
            var copy = new List<string>(_rawWords);
            string rWord = copy[0];
            copy.RemoveAt(0);
            BackTraсking(new WordArea(new Word(rWord, Orientation.Horizontal)), copy);
        }

        Draw();

        Debug.WriteLine("count: " + cnt);
    }

    private void ReadWords()
    {
        _rawWords = _reader.Read();
        _rawWords.Sort((x, y) => -1 * (x.Length - y.Length));
    }

    public bool IsSolution(WordArea wordArea)
    {
        if (wordArea.Count < _rawWords.Count) return false;

        int space = wordArea.Space();
        //if (space > _currentSpace * 2) return false;

        double density = wordArea.IntersectionCount * 1.0 / space;
        //if (_currentWordArea == null || density > _currentDensity)
        //if (_currentWordArea == null || wordArea.IntersectionCount >= _currentWordArea.IntersectionCount)
        //{
            _currentWordArea = new WordArea(wordArea);
            _currentSpace = space;
            _currentDensity = density;
            //return true;
        //}
        return false;
    }

    private bool IsBadParticalSolution(WordArea wordArea, List<string> rawWords) 
    {
        //не с чем сравнивать
        if (_currentWordArea == null) return false;

        int space = wordArea.Space();
        if (space * 1.4 >= _currentSpace) return true;

        //double density = wordArea.IntersectionCount * 1.0 / space;
        //if (density + _alpha < _currentDensity) return true;

        double mediunSumWordArea = wordArea.Sum(word => word.Count) * 1.0 / wordArea.Count;
        double mediunSumRawWords = rawWords.Sum(str => str.Length) * 1.0 / rawWords.Count;
        if (mediunSumWordArea <= mediunSumRawWords) return true;

        return false;
    }

    private void BackTraсking(WordArea wordArea, List<string> rawWords)
    {

        if (IsSolution(wordArea)) return;

        if (IsBadParticalSolution(wordArea, rawWords)) return;

        

        for (int i = 0; i < rawWords.Count; i++)
        {
            var copy = new List<string>(rawWords);
            string rWord = copy[i];
            copy.RemoveAt(i);
            TryAddWord(wordArea, rWord, copy);
        }
    }

    private void TryAddWord(WordArea wordArea, string rWord, List<string> otherRawWords)
    {
        for (int k = 0; k < wordArea.Count; k++)
        {
            Word word = wordArea[k];
            for (int i = 0; i < word.Count; i++)
            {
                for (int j = 0; j < rWord.Length; j++)
                {
                    if (rWord[j] == word[i])
                    {
                        //координаты точки пересечения (intersection)
                        var(iX, iY)  = word.XYFrom(i);

                        //ориентация в пространстве добавляемого слова 
                        Orientation orient = word.NegativeOrient;

                        //координаты добавляемого слова
                        int x, y;
                        if (orient == Orientation.Horizontal)
                        {
                            x = iX - j;
                            y = iY;
                        }
                        else
                        {
                            x = iX;
                            y = iY - j;
                        }

                        Word newWord = new Word(rWord, orient, x, y);

                        if (wordArea.TryAdd(newWord))
                        {
                            cnt++;
                            BackTraсking(wordArea, otherRawWords);
                            wordArea.Remove(newWord);
                        }
                    }
                }
            }
        }
    }
    //private (int, int) Sizes()
    //{
    //    var (x, y) = _currentWordArea!.MaxCoord();
    //    return (x + 1, y + 1);
    //}

    public void Draw()
    {
        //int width, height;
        if (_currentWordArea == null)
        {
            _currentWordArea = new WordArea(
                new Word("введите", Orientation.Horizontal),
                new Word("другие", Orientation.Horizontal, 0, 1),
                new Word("слова:)", Orientation.Horizontal, 0, 2)
            );
            //width = 7;
            //height = 3;
        }
        else
        {
            _currentWordArea.NomalizeXY();
            //(width, height) = Sizes();
        }

        //_drawer.SetSizes(width, height);
        _currentWordArea.Draw(_drawer);
    }
}