using System.Collections;

namespace Logic;

public class WordArea : IEnumerable<Word>
{
    private List<Word> _words = new List<Word>();

    private HashSet<(int, int)> _intersectionSet = new HashSet<(int, int)>();

    private HashSet<(int, int)> _tempIntersectionSet = new HashSet<(int, int)>();



    public WordArea(params Word[] words)
    {
        foreach (var word in words) NotSafeAdd(word);
    }

    public WordArea(WordArea wordArea)
    {
        foreach (Word word in wordArea) NotSafeAdd(word);
        _intersectionSet = new HashSet<(int, int)>(wordArea.IntersectionSet);
    }


    public int Count => _words.Count;

    public Word this[int index] => _words[index];

    public HashSet<(int, int)> IntersectionSet => _intersectionSet;

    public int IntersectionCount => _intersectionSet.Count;


    public void NotSafeAdd(Word word) => _words.Add(word);

    public IEnumerator<Word> GetEnumerator()
    {
        foreach (Word word in _words) yield return word;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void Remove(Word word)
    {
        //удаляем все пересечения со словом и само слово
        for (int i = 0; i < word.Count; i++)
        {
            _intersectionSet.Remove(word.XYFrom(i));
        }
        _words.Remove(word);
    }

    public bool TryAdd(Word newWord)
    {
        //множество для временного хранения новых пересечений
        _tempIntersectionSet.Clear();

        //проверяем являются ли допустимыми пересечения со словами
        for (int i = 0; i < newWord.Count; i++)
        {
            var xy = newWord.XYFrom(i);
            if (!_intersectionSet.Contains(xy))
            {
                foreach (Word word in this)
                {
                    if (word.TryCharAt(xy, out char letter))
                    {
                        if (letter != newWord[i])
                        {
                            return false;
                        }
                        else
                        {
                            _tempIntersectionSet.Add(xy);
                        }
                    }
                }
            }
            //два слова уже пересеклись в данной позиции
            else
            {
                return false;
            }
        }

        //проверяем, чтобы над или под буквой без пересечения не были заняты ячейки
        for (int i = 0; i < newWord.Count; i++)
        {
            var xy = newWord.XYFrom(i);
            if (!_tempIntersectionSet.Contains(xy))
            {
                foreach (Word word in this)
                {
                    if (word.TryCharAt(newWord.XYOver(xy), out _) || word.TryCharAt(newWord.XYUnder(xy), out _))
                    {
                        return false;
                    }
                }
            }
        }

        //смотрим, чтобы слева была пустая клетка
        var leftXY = newWord.LeftXY;
        if (!_intersectionSet.Contains(leftXY))
        {
            foreach (Word word in this)
            {
                if (word.TryCharAt(leftXY, out _))
                {
                    return false;
                }
            }
        }
        else
        {
            return false;
        }

        //смотрим, чтобы справа была пустая клетка
        var rightXY = newWord.RightXY;
        if (!_intersectionSet.Contains(rightXY))
        {
            foreach (Word word in this)
            {
                if (word.TryCharAt(rightXY, out _))
                {
                    return false;
                }
            }
        }
        else
        {
            return false;
        }

        foreach (var xy in _tempIntersectionSet) _intersectionSet.Add(xy);
        NotSafeAdd(newWord);
        return true;
    }

    public int Space()
    {
        int minX = _words[0].MinX;
        int minY = _words[0].MinY;
        int maxX = _words[0].MaxX;
        int maxY = _words[0].MaxY;
        for (int i = 1; i < _words.Count; i++)
        {
            if (_words[i].MinX < minX) minX = _words[i].MinX;
            if (_words[i].MinY < minY) minY = _words[i].MinY;
            if (_words[i].MaxX > maxX) maxX = _words[i].MaxX;
            if (_words[i].MaxY > maxY) maxY = _words[i].MaxY;
        }
        return (maxX - minX + 1) * (maxY - minY + 1);
    }

    public void NomalizeXY()
    {
        int minX = _words[0].MinX;
        int minY = _words[0].MinY;
        for (int i = 1; i < _words.Count; i++)
        {
            if (_words[i].MinX < minX) minX = _words[i].MinX;
            if (_words[i].MinY < minY) minY = _words[i].MinY;
        }
        foreach (var word in _words)
        {
            word.NormalizeXY(minX, minY);
        }
    }

    //public (int, int) MaxCoord()
    //{
    //    int maxX = _words[0].MaxX;
    //    int maxY = _words[0].MaxY;
    //    for (int i = 1; i < _words.Count; i++)
    //    {
    //        if (maxX < _words[i].MaxX) maxX = _words[i].MaxX;
    //        if (maxY < _words[i].MaxY) maxY = _words[i].MaxX;
    //    }
    //    return (maxX, maxY);
    //}

    public void Draw(ICellDrawer drawer)
    {
        foreach (Word word in this) word.Draw(drawer);
    }
}