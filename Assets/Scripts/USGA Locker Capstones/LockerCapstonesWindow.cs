using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using JoshKery.GenericUI.DOTweenHelpers;

namespace JoshKery.USGA.LockerCapstones
{
   public abstract class LockerCapstonesWindow : BaseWindow
    {
        private static UnityEvent _onSetContent;
        public static UnityEvent onSetContent
        {
            get
            {
                if (_onSetContent == null)
                    _onSetContent = new UnityEvent();

                return _onSetContent;
            }
        }

        [SerializeField]
        protected AppState appState;

        protected override void OnEnable()
        {
            base.OnEnable();

            onSetContent.AddListener(SetContent);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            onSetContent.RemoveListener(SetContent);
        }

        public virtual void SetContent() { }
    }
}


