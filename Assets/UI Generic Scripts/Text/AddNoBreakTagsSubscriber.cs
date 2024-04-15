using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JoshKery.GenericUI.ContentLoading;
using TMPro;

namespace JoshKery.GenericUI.Text
{
    [RequireComponent(typeof(TMP_Text))]
    public class AddNoBreakTagsSubscriber : MonoBehaviour
    {
        private TMP_Text target;

        private void Awake()
        {
            target = GetComponent<TMP_Text>();
            AddNoBreakTags.AddNoBreakTagsToText(target);
        }

        private void OnEnable()
        {
            ContentLoader.onSetContentDone.AddListener(OnSetContentDone);
        }

        private void OnDisable()
        {
            ContentLoader.onSetContentDone.RemoveListener(OnSetContentDone);
        }

        private void OnSetContentDone()
        {
            AddNoBreakTags.AddNoBreakTagsToText(target);
        }
    }
}


