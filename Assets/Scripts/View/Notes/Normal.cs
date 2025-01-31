using Abstracts;
using Defaults;
using Model;
using UnityEngine;
using Zenject;

namespace View.Notes
{
    public class Normal: NotesCore
    {
        public override NotesType Type => NotesType.Normal;

        [SerializeField] private Material _normalMaterial;
        [SerializeField] private Material _bigMaterial;
        
        protected override void OnPush()
        {
            DeActivate();
        }

        public override void OnActivate(LaneName laneName,float speed)
        {
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