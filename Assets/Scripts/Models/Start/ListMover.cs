using System;
using System.Collections.Generic;

namespace Model
{
    /// <summary>
    /// Listの切り替え
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ListMover<T>
    {
        public List<T> List;
        private int _index = 0;
        private bool _loop;
        
        /// <summary>
        /// Listの切り替え
        /// </summary>
        /// <param name="list">切り替えるリスト</param>
        /// <param name="loop">終端と始端をつなげるか(Default -> true)</param>
        public ListMover(List<T> list, bool loop = true)
        {
            _loop = loop;
            if (list != null)
            {
                List = list;
            }
        }

        /// <summary>
        /// 選択中の要素
        /// </summary>
        /// <returns></returns>
        public T Selecting()
        {
            return List[_index];
        }

        /// <summary>
        /// 選択中の要素番号
        /// </summary>
        /// <returns></returns>
        public int SelectingNumber()
        {
            return _index;
        }

        /// <summary>
        /// 選択中の要素に代入
        /// </summary>
        /// <param name="value"></param>
        public void SetValue(T value)
        {
            List[_index] = value;
        }

        /// <summary>
        /// 要素移動
        /// </summary>
        /// <param name="direction">正の数で終端方向に移動</param>
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