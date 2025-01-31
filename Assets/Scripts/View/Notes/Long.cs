using System.Collections.Generic;
using Abstracts;
using Defaults;
using Model;
using UnityEngine;
using Interfaces;

namespace View.Notes
{
    public class Long: NotesCore,ILongNotes
    {
        public override NotesType Type => NotesType.Long;
        
        [SerializeField] private Material _normalMaterial;
        [SerializeField] private Material _bigMaterial;

        private List<float> _growLength = new();
        protected override void OnPush()
        {
            if (_growLength.Count > 1)
            {
                var _old = _growLength[^1];
                _growLength.Remove(_growLength[^1]);
                var _length = _growLength[^1];
                var _tr = transform;
                transform.localScale = new Vector3(_tr.localScale.x, _tr.localScale.y, _length*1 / 10 + GameData.NormalNotesScale.z);
                Position = new Vector3(Position.x, Position.y, Position.z + _old - _length);
                transform.position = new Vector3(Position.x, Position.y, Position.z + _length / 2);
            }
            else
            {
                DeActivate();
            }
        }

        public void Grow()
        {
            var _length = GameData.LanePositions[MyLane].z - Position.z;
            var _tr = transform;
            transform.localScale = new Vector3(_tr.localScale.x, _tr.localScale.y, _length*1 / 10 + GameData.NormalNotesScale.z);
            transform.position = new Vector3(Position.x, Position.y, Position.z + _length / 2);
            _growLength.Add(_length);
            //Debug.Log(_growLength.Count);
        }

        public override void OnActivate(LaneName laneName,float speed)
        {
            _growLength = new();
            _growLength.Add(0);
            if (laneName is LaneName.BigRight or LaneName.BigLeft)
            {
                Material = GetComponent<Renderer>().material = _bigMaterial;
                transform.localScale = GameData.BigNotesScale;
            }
            else
            {
                Material = GetComponent<Renderer>().material = _normalMaterial;
                transform.localScale = GameData.NormalNotesScale;
            }
        }
    }
}