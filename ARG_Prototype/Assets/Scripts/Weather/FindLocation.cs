using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class FindLocation : MonoBehaviour
{
	private WeatherData weatherData;
	public LocationInfo locationInfo;

	public string city;
	public float latitude;
	public float longitude;

	private string IPAddress;

	private void Start()
	{
		weatherData = gameObject.transform.GetChild(0).GetComponent<WeatherData>();

		StartCoroutine(GetIP());
	}

	private IEnumerator GetIP()
	{
		var webRequest = new UnityWebRequest("https://icanhazip.com/")
		{
			downloadHandler = new DownloadHandlerBuffer()
		};

		yield return webRequest.SendWebRequest();

		if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
		{
			Debug.Log("Web Request Error: " + webRequest.result);
			yield break;
		}

		IPAddress = webRequest.downloadHandler.text;

		StartCoroutine(GetCoordinates());

		yield break;
	}

	private IEnumerator GetCoordinates()
	{
		var webRequest = new UnityWebRequest("http://ip-api.com/json/" + IPAddress)
		{
			downloadHandler = new DownloadHandlerBuffer()
		};

		yield return webRequest.SendWebRequest();

		if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
		{
			Debug.Log("Web Request Error: " + webRequest.result);
			yield break;
		}

		locationInfo = JsonUtility.FromJson<LocationInfo>(webRequest.downloadHandler.text);

		city = locationInfo.city;
		latitude = locationInfo.lat;
		longitude = locationInfo.lon;

		weatherData.Begin();

		yield break;
	}
}

[SerializeField]
public class LocationInfo
{
	public string status;
	public string country;
	public string countryCode;
	public string region;
	public string regionName;
	public string city;
	public string zip;
	public float lat;
	public float lon;
	public string timezone;
	public string isp;
	public string org;
	public string @as;
	public string query;
}
