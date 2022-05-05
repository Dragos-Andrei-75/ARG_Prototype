using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class FindLocation : MonoBehaviour
{
	public LocationInfo locationInfo;
	public WeatherData weatherData;

	public string city;
	public float latitude;
	public float longitude;

	private string IPAddress;

	private void Start()
	{
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
	}

	private IEnumerator GetCoordinates()
	{
		var www = new UnityWebRequest("http://ip-api.com/json/" + IPAddress)
		{
			downloadHandler = new DownloadHandlerBuffer()
		};

		yield return www.SendWebRequest();

		if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
		{
			Debug.Log("Web Request Error");
			yield break;
		}

		locationInfo = JsonUtility.FromJson<LocationInfo>(www.downloadHandler.text);

		city = locationInfo.city;
		latitude = locationInfo.lat;
		longitude = locationInfo.lon;

		weatherData.Begin();
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
