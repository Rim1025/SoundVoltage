using System.Collections.Generic;
using UnityEngine;

namespace View
{
    /// <summary>
    /// キャンバス群所持
    /// </summary>
    public class Canvases: MonoBehaviour
    {
        /// <summary>
        /// キャンバス群
        /// </summary>
        [SerializeField] private List<GameObject> _canvas;

        public List<GameObject> GetCanvases()
        {
            return _canvas;
        }
    }
}