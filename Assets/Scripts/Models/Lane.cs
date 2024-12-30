using System.Collections.Generic;
using Interfaces;
using Services;
using UnityEngine;
using UniRx;
using Zenject;

namespace Model
{
    /// <summary>
    /// ノーツの制御
    /// </summary>
    public class Lane
    {
        private List<List<int>> _notesList;
        
        [Inject]
        public Lane(INotesSpawner spawner)
        {
            var _status = JsonReader.Read();
            _notesList = TrackReader.Read(_status);
        }
        
        public void Push(LaneName lane)
        {
            
        }

        public void DownPush()
        {
            
        }
    }
}