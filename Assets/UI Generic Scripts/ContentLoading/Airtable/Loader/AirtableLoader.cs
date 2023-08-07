using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using AirtableUnity.PX.Model;
using Newtonsoft.Json;

namespace JoshKery.GenericUI.ContentLoading.Airtable
{
    public abstract class AirtableLoader<T> : ContentLoader
    {
        public delegate void OnAirtableResponseFinish(List<BaseRecord<T>> records);
        public static OnAirtableResponseFinish onAirtableResponseFinish;

        public delegate void OnAirtableResponseFail(AirtableUnity.PX.Response response, UnityWebRequest request);
        public static OnAirtableResponseFail onAirtableResponseFail;

        public string DefaultTableName;
        public string DefaultFilterByFormula;

        [SerializeField]
        private AirtableLocalConfigLoader airtableLocalConfigLoader;

        protected override void Awake()
        {
            base.Awake();

            if (airtableLocalConfigLoader == null)
                airtableLocalConfigLoader = FindObjectOfType<AirtableLocalConfigLoader>();
        }

        protected virtual void OnEnable()
        {
            if (airtableLocalConfigLoader != null)
                airtableLocalConfigLoader.onPopulateContentFinish.AddListener(LoadContent);
        }

        protected virtual void OnDisable()
        {
            if (airtableLocalConfigLoader != null)
                airtableLocalConfigLoader.onPopulateContentFinish.RemoveListener(LoadContent);
        }

        //--------------------------------------Load Airtable Content--------------------------------------
        protected override IEnumerator LoadTargetContent()
        {
            yield return LoadAirtableContent();
        }

        //--------------------------------------Load Airtable Content Helpers--------------------------------------
        protected virtual IEnumerator LoadAirtableContent()
        {
            yield return StartCoroutine(
                AirtableUnity.PX.Proxy.ListRecordsCo<T>(
                    DefaultTableName,
                    AirtableResponseFinish,
                    AirtableResponseFail,
                    DefaultFilterByFormula
                )
            );
        }

        protected virtual void AirtableResponseFinish(List<BaseRecord<T>> records)
        {
            if (onAirtableResponseFinish != null)
                onAirtableResponseFinish.Invoke(records);

            StartCoroutine(AirtableResponseFinishCo(records));
        }

        protected virtual IEnumerator AirtableResponseFinishCo(List<BaseRecord<T>> records)
        {
            string text = JsonConvert.SerializeObject(records, Formatting.Indented);
            SaveContentFileToDisk(text);

            if (onPopulateContentFinish != null)
                onPopulateContentFinish.Invoke();

            yield return null;
        }

        protected virtual void AirtableResponseFail(AirtableUnity.PX.Response response, UnityWebRequest request)
        {
            if (onAirtableResponseFail != null)
                onAirtableResponseFail.Invoke(response, request);

            StartCoroutine(LoadLocalContent());
        }

        //--------------------------------------Updating Airtable Fields--------------------------------------
        public IEnumerator UpdateRecordCo(
            string tableName,
            string recordId,
            string newData,
            Action<BaseRecord<T>> callback,
            bool useHardUpdate = false
        )
        {
            yield return StartCoroutine(AirtableUnity.PX.Proxy.UpdateRecordCo<T>(tableName, recordId, newData,
                (baseRecordUpdated) =>
                {
                    callback?.Invoke(baseRecordUpdated);
                }, useHardUpdate));
        }


    }
}


