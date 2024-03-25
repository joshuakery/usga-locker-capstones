using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JoshKery.GenericUI.DOTweenHelpers;

namespace JoshKery.USGA.LockerCapstones
{
    public class LockerCapstonesSetupWindow : BaseWindow
    {
        private LockerCapstonesConfigLoader configLoader;

        [SerializeField]
        private TMP_Text timeDisplay;

        [SerializeField]
        private Button proceedButton;

        [SerializeField]
        private TMP_Dropdown dropdown;

        protected override void Awake()
        {
            base.Awake();

            configLoader = FindObjectOfType<LockerCapstonesConfigLoader>();
        }
        protected override void OnEnable()
        {
            base.OnEnable();

            if (configLoader != null)
            {
                configLoader.onLoadContentCoroutineStart.AddListener(DoOpen);
                configLoader.onEraOptionsLoaded += OnEraOptionsLoaded;
                configLoader.onTimeUpdated += OnTimeUpdated;
                configLoader.onPopulateContentFinish.AddListener(DoClose);

                if (proceedButton != null)
                    proceedButton.onClick.AddListener(configLoader.ProceedToLoadContent);

                if (dropdown != null)
                    dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
            }

        }

        protected override void OnDisable()
        {
            base.OnDisable();

            if (configLoader != null)
            {
                configLoader.onLoadContentCoroutineStart.RemoveListener(DoOpen);
                configLoader.onEraOptionsLoaded -= OnEraOptionsLoaded;
                configLoader.onTimeUpdated -= OnTimeUpdated;
                configLoader.onPopulateContentFinish.RemoveListener(DoClose);

                if (proceedButton != null)
                    proceedButton.onClick.RemoveListener(configLoader.ProceedToLoadContent);

                if (dropdown != null)
                    dropdown.onValueChanged.RemoveListener(OnDropdownValueChanged);
            }
                
        }

        private void OnEraOptionsLoaded(LockerCapstonesConfigLoader.EraOption[] eraOptions, int selectedIndex)
        {
            if (dropdown != null && configLoader != null)
            {
                dropdown.options.Clear();

                for (int i=0; i<eraOptions.Length; i++)
                {
                    LockerCapstonesConfigLoader.EraOption eraOption = eraOptions[i];

                    dropdown.options.Add( new TMP_Dropdown.OptionData() { text=eraOption.name } );
                }

                dropdown.value = selectedIndex;
            }
        }

        private void OnDropdownValueChanged(int index)
        {
            if (configLoader != null)
                configLoader.SelectEra(index);
        }

        private void OnTimeUpdated()
        {
            if (configLoader != null)
            {
                if (configLoader.timeRemainingToProceed > System.TimeSpan.Zero)
                {
                    if (!isOpen)
                        _Open(SequenceType.UnSequenced);
                }
                else
                {
                    if (isOpen)
                        _Close(SequenceType.UnSequenced);
                }

                if (timeDisplay != null)
                    timeDisplay.text = System.Convert.ToInt16(configLoader.timeRemainingToProceed.TotalSeconds).ToString();
            }
        }
    }
}


