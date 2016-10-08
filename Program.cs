using System;

internal static class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine(AlignAndReverse("A  B C DEF G H"));
        Console.WriteLine(AlignAndReverse("A  BCD"));
        Console.WriteLine(AlignAndReverse("        A  BCD"));
        Console.WriteLine(AlignAndReverse("ABCDEFGH      "));
        Console.WriteLine(AlignAndReverse("ABCDEFGH I     "));
        Console.WriteLine(AlignAndReverse("Z "));
        Console.WriteLine(AlignAndReverse("A  "));
        Console.WriteLine(AlignAndReverse("B"));
        Console.WriteLine(AlignAndReverse("ABC"));
        Console.WriteLine(AlignAndReverse("ABC    "));

        Console.ReadKey();
    }

    static string AlignAndReverse(string source)
    {
        var ar = new AlignReverse(source);
        var src = ar.ToString();
        ar.Process();
        var dst = ar.ToString();
        return string.Format("{0} ---> {1}", src, dst);
    }

    private class AlignReverse
    {
        private char[] _list;
        private char[] _buffer;
        private int _bufferLeftIndex;

        private int _leftIndex;
        private int _rightIndex;

        public override string ToString()
        {
            return string.Format("'{0}'", new string(_list).Replace(' ', '\u00b7'));
        }

        public AlignReverse(string source) : this(source.ToCharArray())
        {
        }

        public AlignReverse(char[] source)
        {
            //todo: check that source is valid
            _list = source;

            _leftIndex = 0;
            _rightIndex = _list.Length - 1;
        }

        public void Process()
        {
            ProcessInternal();

            for (var i = 0; i <= _leftIndex; i++)
            {
                _list[i] = ' ';
            }

            for (var i = _rightIndex; i < _list.Length; i++)
            {
                _list[i] = ' ';
            }
        }

        private void ProcessInternal()
        {
            while (_list[_leftIndex] == ' ')
                _leftIndex++;

            while (_list[_rightIndex] == ' ')
                _rightIndex--;
            //todo: make sure that the string containing only the spaces will return valid result

            var groupLength = 0;
            while ((_list[_leftIndex + groupLength] != ' ') && (_list[_rightIndex - groupLength] != ' '))
            {
                if ((_leftIndex + groupLength) >= (_rightIndex - groupLength)) // string has been completely passed
                {
                    var ll = _leftIndex + groupLength - 1;
                    var rr = _rightIndex - groupLength + 1;
                    _buffer = CreateBuffer();

                    if ((_leftIndex + groupLength) == (_rightIndex - groupLength)) // this is odd string
                    {
                        var v = ReadWithBuffer(_leftIndex + groupLength);
                        _leftIndex = (_list.Length / 2) - 1;
                        _rightIndex = _leftIndex + 2; //because the midlle char should be only moved without reverse
                        _list[_leftIndex + 1] = v;
                    }
                    else
                    {
                        _leftIndex = (_list.Length / 2) - 1;
                        _rightIndex = _leftIndex + 1;
                    }

                    ReversePartial(ll, rr, groupLength);
                    return;
                }

                groupLength++;
            }

            _leftIndex += groupLength;
            _rightIndex -= groupLength;
            var l = _leftIndex;
            var r = _rightIndex;
            ProcessInternal();
            ReversePartial(l - 1, r + 1, groupLength);
        }

        private char[] CreateBuffer()
        {
            var bufferSize = (_list.Length / 2) - _leftIndex;
            if (bufferSize < 0)
                bufferSize = -bufferSize;

            var buffer = new char[bufferSize];
            _bufferLeftIndex = (_list.Length - bufferSize) / 2;
            Array.Copy(_list, _bufferLeftIndex, buffer, 0, bufferSize);
            return buffer;
        }

        private char ReadWithBuffer(int index)
        {
            var bIndex = index - _bufferLeftIndex;
            if (bIndex >= 0 && bIndex < _buffer.Length)
                return _buffer[bIndex];

            return _list[index];
        }

        private void ReversePartial(int leftReadIndex, int righthReadIndex, int groupLength)
        {
            for (var i = 0; i < groupLength; i++)
            {
                var l = ReadWithBuffer(leftReadIndex - i);
                var r = ReadWithBuffer(righthReadIndex + i);

                _list[_leftIndex - i] = r;
                _list[_rightIndex + i] = l;
            }

            _leftIndex -= groupLength;
            _rightIndex += groupLength;
        }
    }
}
