using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using JoshKery.GenericUI.ContentLoading;
using JoshKery.GenericUI.DOTweenHelpers;
using GraphQlClient.Core;
using Newtonsoft.Json;
using rlmg.logging;

namespace JoshKery.USGA.LockerCapstones
{
    public class LockerCapstonesContentLoader : ContentLoader
    {
        #region FIELDS
        [SerializeField]
        private UISequenceManager sequenceManager;

        private QueryLoader queryLoader;

        [SerializeField]
        private string OnlineContentDirectory;

        [SerializeField]
        private AppState appState;
        #endregion

        #region Monobehaviour Methods

        protected override void Awake()
        {
            queryLoader = GetComponent<QueryLoader>();

            base.Awake();
        }
        private void OnEnable()
        {
            if (onPopulateContentFinish != null)
                onPopulateContentFinish.AddListener(InvokeDOTweenHelpersStartUp);
        }

        private void OnDisable()
        {
            if (onPopulateContentFinish != null)
                onPopulateContentFinish.RemoveListener(InvokeDOTweenHelpersStartUp);
        }
        #endregion

        #region Graph Loading Methods
        /// <summary>
        /// Override method that gets called via LoadContent()
        /// 
        /// </summary>
        /// <returns></returns>
        protected override IEnumerator LoadTargetContent()
        {
            yield return LoadGraphContent().AsIEnumerator();
        }

        private async Task LoadGraphContent()
        {

            await queryLoader.LoadContentCoroutine();

            await new WaitForEndOfFrame();

            UnityWebRequest request = await HttpHandler.PostAsync(queryLoader.url, queryLoader.query, queryLoader.authToken);

            if (request.result != UnityWebRequest.Result.Success)
            {
                GraphResponseFail(request);
            }
            else
            {
                await GraphResponseSuccess(request.downloadHandler.text);
            }
        }

        private async Task GraphResponseSuccess(string text)
        {
            SaveContentFileToDisk(text);

            await PopulateContent(text);

            onPopulateContentFinish.Invoke();

            await new WaitForEndOfFrame(); //TODO why do I do this?
        }

        private void GraphResponseFail(UnityWebRequest request)
        {
            RLMGLogger.Instance.Log(request.error, MESSAGETYPE.ERROR);
            //TODO UI display of error handling and option to try again
        }
        #endregion

        #region Load Local Content Fallback
        protected override IEnumerator LoadLocalContentSuccess(string text)
        {
            yield return StartCoroutine(PopulateContent(text));

            SaveContentFileToDisk(text);

            onPopulateContentFinish.Invoke();

            yield return new WaitForEndOfFrame();
        }

        protected override IEnumerator LoadLocalContentFinish(UnityWebRequest.Result result)
        {
            yield return null;
        }
        #endregion

        #region Save Local Content to Disk Override
        protected override void SaveContentFileToDisk(string text)
        {
            string aux = JsonConvert.SerializeObject(JsonConvert.DeserializeObject<LockerCapstonesDataWrapper>(text), Formatting.Indented);

            base.SaveContentFileToDisk(aux);
        }
        #endregion



        #region PopulateContent
        private IEnumerator PopulateContent(string text)
        {
            yield return null;
            //TODO manage displays for pgoress updates?

            LockerCapstonesDataWrapper wrapper = JsonConvert.DeserializeObject<LockerCapstonesDataWrapper>(text);
            appState.data = wrapper.data;

            /*yield return StartCoroutine(LoadNewInducteesMedia());*/

            yield return null;

            /*NewInducteesWindow.onSetContent.Invoke();*/
        }
        #endregion

        #region After Populate Content
        private void InvokeDOTweenHelpersStartUp()
        {
            if (sequenceManager != null)
                sequenceManager.CompleteCurrentSequence();

            BaseWindow.onAwakeWindows.Invoke();
            BaseWindow.onStartUpWindows.Invoke();
            BaseStateMachine.onStartUpStateMachines.Invoke();
        }
        #endregion
    }
}


