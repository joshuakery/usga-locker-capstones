using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.Networking;
using JoshKery.GenericUI.ContentLoading.Airtable;

namespace JoshKery.GenericUI.Example
{
    public class ExampleAirtableLocalConfigLoader : AirtableLocalConfigLoader
    {
        [SerializeField]
        protected ExampleAirtableLoader exampleAirtableLoader;

        [System.Serializable]
        public class ExampleAirtableConfigJSON : AirtableLocalConfigJSON
        {
            public string TableName;
        }

        protected override IEnumerator LoadLocalContentSuccess(string text)
        {
            ExampleAirtableConfigJSON configData = JsonConvert.DeserializeObject<ExampleAirtableConfigJSON>(text);

            if (configData == null)
                yield break;

            if (!string.IsNullOrEmpty(configData.ApiVersion) &&
                !string.IsNullOrEmpty(configData.AppKey) &&
                !string.IsNullOrEmpty(configData.ApiKey)
                )
            {
                AirtableUnity.PX.Proxy.SetEnvironment(
                    configData.ApiVersion,
                    configData.AppKey,
                    configData.ApiKey
                );
            }
            else
            {
                AirtableUnity.PX.Proxy.SetEnvironment(
                    DefaultApiVersion,
                    DefaultAppKey,
                    DefaultApiKey
                );
            }

            if (exampleAirtableLoader != null)
            {
                exampleAirtableLoader.doDefaultLocalLoadContent = configData.doDefaultLocalLoadContent;
                exampleAirtableLoader.DefaultTableName = configData.TableName;
            }

            onPopulateContentFinish.Invoke();
        }
    }
}


