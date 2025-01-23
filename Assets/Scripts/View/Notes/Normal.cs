using Abstracts;
using Defaults;
using Model;
using UnityEngine;
using Zenject;

namespace View.Notes
{
    public class Normal: NotesCore
    {
        [SerializeField] private Material _normalMaterial;
        [SerializeField] private Material _bigMaterial;
        public override NotesType Type => NotesType.Normal;
        
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

        protected override void OnDeActivate()
        {
            
        }
    }
}