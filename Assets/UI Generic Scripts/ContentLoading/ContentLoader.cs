using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using rlmg.logging;
using UnityEngine.Serialization;
using System.IO;
using System;
using System.Text.RegularExpressions;
using UnityEngine.Networking;
using UnityEngine.Events;
using JoshKery.GenericUI.Events;

namespace JoshKery.GenericUI.ContentLoading
{
	public abstract class ContentLoader : MonoBehaviour
	{
		/// <summary>
        /// If true, LoadContent is called on Awake
        /// </summary>
		public bool loadOnAwake = true;

		/// <summary>
        /// If false, LoadContentCoroutine completes without making a WebRequest
        /// </summary>
		public bool doLoadContent = true;

		/// <summary>
        /// If Editor and true, LoadContentCoroutine completes
        /// </summary>
		public bool canUseInEditor = true;

		/// <summary>
        /// If true, LoadContentCoroutine loads local content only
        /// </summary>
		public bool doDefaultLocalLoadContent = false;

		


        /// <summary>
        /// Filename where local json will be written, and read from by default
        /// </summary>
        public string ContentFilename = "config.json";

		//public string cmsUrl;
		//public bool tryDownloadFromCMS = false;
		//public float cmsTimeOut = 60f;
		//protected float cmsTimer = 0f;
		//protected bool didSuccessfullyDownloadFromCMS = false;
		//public bool cacheFromCMS = false;
		//public string cachedFileExtensions = ".jpeg|.png|.jpg|.ogg";

		protected bool _hasLoadedContent = false;
		public bool HasLoadedContent
		{
			get
			{
				return _hasLoadedContent;
			}
		}

        #region Events

        #region Load Content Coroutine Events
        private UnityEvent _onLoadContentCoroutineStart;
		/// <summary>
        /// Fires at start of LoadContentCoroutine
        /// </summary>
		public UnityEvent onLoadContentCoroutineStart
        {
			get
            {
				if (_onLoadContentCoroutineStart == null)
					_onLoadContentCoroutineStart = new UnityEvent();

				return _onLoadContentCoroutineStart;
            }
        }

		private UnityEvent _onLoadContentCoroutineFinish;
		/// <summary>
		/// Fires at end of LoadContentCouroutine
		/// </summary>
		public UnityEvent onLoadContentCoroutineFinish
        {
			get
            {
				if (_onLoadContentCoroutineFinish == null)
					_onLoadContentCoroutineFinish = new UnityEvent();

				return _onLoadContentCoroutineFinish;
            }
        }
		#endregion

		private UnityEvent _onPopulateContentFinish;
		/// <summary>
        /// Fires when caching and setting up content is complete
        /// </summary>
		public UnityEvent onPopulateContentFinish
        {
			get
            {
				if (_onPopulateContentFinish == null)
					_onPopulateContentFinish = new UnityEvent();

				return _onPopulateContentFinish;
            }
        }

		public delegate void OnLoadingProgressEvent(string mainMessage, string detailMessage = null, float totalWork = 0f, float loadedWork = 0f);
		public OnLoadingProgressEvent onLoadingProgress;

		public delegate void OnLoadingDetailsEvent(string message);
		public OnLoadingDetailsEvent onLoadingDetails;

		public delegate void OnLoadingErrorEvent(string message);
		public OnLoadingErrorEvent onLoadingError;
		#endregion

		/*		public static string fileProtocolPrefix
				{
					get
					{
		#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
					return "file:///";
		#else
						return "file://";
		#endif
					}
				}*/

		public enum CONTENT_LOCATION
		{
			StreamingAssets,
			Desktop,
			Application
		}

		public CONTENT_LOCATION contentLocation = CONTENT_LOCATION.Application;

		protected string LocalContentDirectory
		{
			get
			{
				string path = "";

				if (contentLocation == CONTENT_LOCATION.StreamingAssets)
				{
					path = Application.streamingAssetsPath;
				}
				if (contentLocation == CONTENT_LOCATION.Desktop)
				{
					path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
				}
				else if (contentLocation == CONTENT_LOCATION.Application)
				{
					path = Path.Combine(Application.dataPath, "..");
				}

				if (!Directory.Exists(path))
					Directory.CreateDirectory(path);

				return path;
			}
		}

		protected string LocalContentPath
        {
			get
            {
				return Path.Combine(LocalContentDirectory, ContentFilename);
			}
        }

		[SerializeField]
		protected string LocalMediaCacheDirectoryName = "mediaCache";

		private string LocalMediaCacheDirectory
        {
			get
            {
				string directoryName = string.IsNullOrEmpty(LocalMediaCacheDirectoryName) ? "mediaCache" : LocalMediaCacheDirectoryName;

				string path = Path.Combine(LocalContentDirectory, directoryName);

				if (!Directory.Exists(path))
					Directory.CreateDirectory(path);

				return path;
            }
        }

		#region Refresh Content Fields
		public enum RefreshType
		{
			NoRefresh = 0,
			HardRefresh = 1,
			SoftRefresh = 2
		}

		public enum RefreshLimit
		{
			AllContent = 0,
			TextOnly = 1,
			ImagesOnly = 2,
			VideoOnly = 3
		}

		public RefreshType refreshType;
		public RefreshLimit refreshLimit;
		#endregion

		protected virtual void Awake()
		{
#if UNITY_EDITOR
			contentLocation = CONTENT_LOCATION.StreamingAssets;
#endif
			if (loadOnAwake)
				LoadContent();
		}

        #region Load Content Primary Methods
        public virtual void LoadContent()
		{
			StopAllCoroutines();
			StartCoroutine(LoadContentCoroutine());
		}

		public virtual IEnumerator LoadContentCoroutine()
		{
			_hasLoadedContent = false;

			onLoadContentCoroutineStart?.Invoke();
			onLoadingProgress?.Invoke("Starting to Load Content");

			if (doLoadContent && (!Application.isEditor || canUseInEditor))
			{
				if (doDefaultLocalLoadContent)
					yield return StartCoroutine(LoadLocalContent());
				else
					yield return StartCoroutine(LoadTargetContent());
			}

			_hasLoadedContent = true;

			onLoadContentCoroutineFinish?.Invoke();
			onLoadingProgress?.Invoke("Finished Loading Content");
		}
		#endregion

		#region Load Target Content
		protected virtual IEnumerator LoadTargetContent()
        {
			onLoadingProgress?.Invoke("Started Loading Target Content");

			//override to do something other than load local content
			yield return StartCoroutine(LoadLocalContent());
        }
		#endregion

		#region Load Local Content
		protected virtual IEnumerator LoadLocalContent()
        {
			onLoadingProgress?.Invoke("Started Loading Local Content");

			using (UnityWebRequest webRequest = UnityWebRequest.Get(LocalContentPath))
			{
				yield return webRequest.SendWebRequest();

				switch (webRequest.result)
				{
					case UnityWebRequest.Result.ConnectionError:
					case UnityWebRequest.Result.DataProcessingError:
					case UnityWebRequest.Result.ProtocolError:
						yield return LoadLocalContentFailure(webRequest.error, webRequest.url);
						break;
					case UnityWebRequest.Result.Success:
						SaveContentFileToDisk(webRequest.downloadHandler.text);
						yield return LoadLocalContentSuccess(webRequest.downloadHandler.text);
						break;
				}

				yield return LoadLocalContentFinish(webRequest.result);
			}
		}

		protected abstract IEnumerator LoadLocalContentSuccess(string text);

		protected virtual IEnumerator LoadLocalContentFailure(string error, string url)
        {
			RLMGLogger.Instance.Log("Load Local Content Error: " + error + " at " + url);
			onLoadingError?.Invoke("Load Local Content Error: " + error + " at " + url);
			yield return null;
        }

		protected abstract IEnumerator LoadLocalContentFinish(UnityWebRequest.Result result);
        #endregion

        //protected virtual void CopyOverIfNecessary()
        //{
        //	string desiredFilePath;
        //	desiredFilePath = Path.Combine(ContentDirectory, contentFilename);

        //	//if not there, and not already checking streaming assets, check for a backup in streaming assets
        //	if (!File.Exists(desiredFilePath) && contentLocation != CONTENT_LOCATION.StreamingAssets)
        //	{
        //		string backupFilePath = Path.Combine(Application.streamingAssetsPath, contentFilename);

        //		//if backup in streaming assets does exist, copy it to the desired external location
        //		if (File.Exists(backupFilePath))
        //		{
        //			File.Copy(backupFilePath, desiredFilePath, true);
        //		}
        //	}
        //}

        #region Save Content File to Disk
		public virtual void SaveContentFileToDisk(string text)
		{
			Debug.Log(LocalContentPath);
			Debug.Log(text);
			File.WriteAllText(LocalContentPath, text);
			RLMGLogger.Instance.Log(string.Format("Saved data to local directory: {0}", LocalContentPath), MESSAGETYPE.INFO);
		}
		#endregion

		#region Save Media to Disk
		public string GetLocalMediaPath(string filename)
		{
			string path = Path.Combine(LocalMediaCacheDirectory, filename);
			return path;
		}
		protected virtual IEnumerator SaveMediaToDisk(string onlinePath, string localPath)
        {
			onLoadingDetails?.Invoke("Downloading media from " + onlinePath + " to " + localPath);

			using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(onlinePath))
            {
				yield return webRequest.SendWebRequest();

				switch (webRequest.result)
				{
					case UnityWebRequest.Result.ConnectionError:
					case UnityWebRequest.Result.DataProcessingError:
					case UnityWebRequest.Result.ProtocolError:
						yield return SaveMediaToDiskFailure(webRequest);
						break;
					case UnityWebRequest.Result.Success:
						yield return SaveMediaToDiskSuccess(webRequest.result);
						File.WriteAllBytes(localPath, webRequest.downloadHandler.data);
						break;
				}
			}			
		}

		protected virtual IEnumerator SaveMediaToDiskSuccess(UnityWebRequest.Result result)
		{
			onLoadingDetails?.Invoke("Successfully saved media to disk.");
			yield return null;
		}

		protected virtual IEnumerator SaveMediaToDiskFailure(UnityWebRequest request)
		{
			onLoadingError?.Invoke("Save Media to Disk Failure: " + request.error);
			RLMGLogger.Instance.Log("Save Media to Disk Failure: " + request.error);
			yield return null;
		}
		#endregion

		//protected IEnumerator CacheContent(string serverResponse, string cachePath, string extensions)
		//{
		//	// use RegEx to find all image paths
		//	//Regex regex = new Regex("(http|ftp|https)://([\\w_-]+(?:(?:\\.[\\w_-]+)+))([\\w.,@?^=%&:/~+#-]*[\\w@?^=%&/~+#-])?(.jpeg|.png|.jpg)");
		//	Regex regex = new Regex("(http|ftp|https)://([\\w_-]+(?:(?:\\.[\\w_-]+)+))([\\w.,@?^=%&:/~+#-]*[\\w@?^=%&/~+#-])?(" + extensions + ")");

		//	MatchCollection matches = regex.Matches(serverResponse);
		//	if (matches.Count > 0)
		//	{
		//		foreach (Match match in matches)
		//		{
		//			string remotePath = match.Value;

		//			// parse out only filename
		//			string filename = remotePath.Substring(remotePath.LastIndexOf("/") + 1);
		//			string localPath = Path.Combine(cachePath, filename);

		//			// check if it exists
		//			if (!File.Exists(localPath))
		//			{
		//				UnityWebRequest www = UnityWebRequestTexture.GetTexture(remotePath);
		//				yield return www.SendWebRequest();

		//				if (www.isNetworkError)
		//				{
		//					Debug.Log(www.error);
		//				}
		//				else
		//				{
		//					// cache it if necessary
		//					CacheFile(localPath, www.downloadHandler.data);
		//				}
		//			}
		//		}

		//		// now clear out any local images that are NOT in json data
		//		var info = new DirectoryInfo(cachePath);
		//		var fileInfo = info.GetFiles();

		//		foreach (FileInfo file in fileInfo)
		//		{
		//			if (serverResponse.IndexOf(file.Name) == -1 && regex.Match(file.Name).Success)
		//			{
		//				// doesn't exist in data anymore, so delete
		//				file.Delete();
		//			}
		//		}
		//	}
		//}



		//public string GetCachedFilePath(string remotePath, string cachePath)
		//{
		//	if (remotePath == "" || remotePath == null)
		//	{
		//		return null;
		//	}

		//	string filename = remotePath.Substring(remotePath.LastIndexOf("/") + 1);
		//	string localPath = Path.Combine(cachePath, filename);

		//	bool isCached = File.Exists(localPath);

		//	// set URL based on if local cached copy exists
		//	string url = isCached ? fileProtocolPrefix + localPath : remotePath;

		//	//Debug.Log("referencing local file at " + cachePath + " using remote url: " + remotePath);

		//	return url;
		//}

		//protected virtual IEnumerator PopulateContent(string contentData)
		//{
		//	//override to do things!

		//	yield break;
		//}

		////https://forum.unity.com/threads/passing-ref-variable-to-coroutine.379640/
		////call via the following syntax: StartCoroutine(LoadSpriteFromFilepath(imgFilePath, result => spriteFileReference = result));
		//public static IEnumerator LoadSpriteFromFilepath(string imgFilePath, Action<Sprite> spriteRef)
		//{
		//	if (!string.IsNullOrEmpty(imgFilePath))
		//	{
		//		WWW externalImgFile = new WWW(imgFilePath);
		//		yield return externalImgFile;

		//		if (externalImgFile.error != null)
		//		{
		//			Debug.LogWarning(imgFilePath + " error = " + externalImgFile.error);
		//		}
		//		else
		//		{
		//			spriteRef(Utilities.TextureToMipMappedSprite(externalImgFile.texture));
		//		}
		//	}
		//}

		//public IEnumerator LoadAudioFileFromFilepath(string audioFilePath, Action<AudioClip> audioClipRef)
		//{
		//	if (!string.IsNullOrEmpty(audioFilePath))
		//	{
		//		WWW externalAudioFile = new WWW(audioFilePath);
		//		yield return externalAudioFile;

		//		if (externalAudioFile.error != null)
		//		{
		//			Debug.LogWarning(audioFilePath + " error = " + externalAudioFile.error);
		//		}
		//		else
		//		{
		//			audioClipRef(externalAudioFile.GetAudioClip(false, false, GetAudioType(audioFilePath)));
		//		}
		//	}
		//}

		//protected AudioType GetAudioType(string audioFilename)
		//{
		//	AudioType audioType = AudioType.UNKNOWN;

		//	if (Path.GetExtension(audioFilename).ToLower() == ".aiff")
		//		audioType = AudioType.AIFF;
		//	else if (Path.GetExtension(audioFilename).ToLower() == ".wav")
		//		audioType = AudioType.WAV;

		//	return audioType;
		//}
	}
}


