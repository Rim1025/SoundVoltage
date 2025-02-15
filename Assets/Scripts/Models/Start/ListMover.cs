using System;
using System.Collections.Generic;

namespace Model
{
    public class ListMover<T>
    {
        public List<T> List;
        private int _index = 0;
        private bool _loop;
        public ListMover(List<T> list, bool loop = true)
        {
            _loop = loop;
            if (list != null)
            {
                List = list;
            }
        }

        public T Selecting()
        {
            return List[_index];
        }

        public int SelectingNumber()
        {
            return _index;
        }

        public void SetValue(T value)
        {
            List[_index] = value;
        }

        public void Move(int direction)
        {
            _index += direction;
            while (_loop)
            {
                if (_index < 0 || _index >= List.Count)
                {
                    _index = List.Count - (int)MathF.Abs(_index);
                }
                else
                {
                    break;
                }
            }

            if (!_loop)
            {
                if (_index < 0)
                {
                    _index = 0;
                }

                if (_index >= List.Count)
                {
                    _index = List.Count - 1;
                }
            }
        }
    }
}