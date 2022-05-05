using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System;
using System.Collections;

public class WeatherData : MonoBehaviour
{
	private WeatherInfo weatherInfo;
	private FindLocation findLocation;
	private TextMeshProUGUI textWeatherCurrent;

	private string apiKey = "c8f99e79f0bb84c2be80658b48c16be1";
	private string uri;
	private float timer;
	private float updateIntervalMinutes;
	private string city;
	private float latitude;
	private float longitude;
	private bool locationFound;
	private bool useCity = false;

    private void Start()
    {
		findLocation = gameObject.transform.root.GetComponent<FindLocation>();
		uri = "https://api.openweathermap.org/data/2.5/weather?";
    }

    private void Update()
	{
		if (locationFound == true)
		{
			if (timer <= 0)
			{
				StartCoroutine(GetWeatherInfo());
				timer = updateIntervalMinutes * 60;
			}
			else
			{
				timer -= Time.deltaTime;
			}
		}
	}

	public void Begin()
	{
		city = findLocation.city;
		latitude = findLocation.latitude;
		longitude = findLocation.longitude;
		locationFound = true;
	}

	private IEnumerator GetWeatherInfo()
	{
		if (useCity == true)
        {
			uri += "q=" + city + "&appid=" + apiKey;
        }
		else
        {
			uri += "lat=" + latitude + "&lon=" + longitude + "&appid=" + apiKey;
		}

		var webRequest = new UnityWebRequest(uri)
		{
			downloadHandler = new DownloadHandlerBuffer()
		};

		yield return webRequest.SendWebRequest();

		if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
        {
			Debug.Log("Web Request Error:" + webRequest.result);
			yield break;
		}

		weatherInfo = JsonUtility.FromJson<WeatherInfo>(webRequest.downloadHandler.text);
		textWeatherCurrent.text = "Current Weather: " + weatherInfo;
	}
}

[Serializable]
public class WeatherInfo
{
	public float latitude;
	public float longitude;
	public string timezone;
	public Currently currently;
	public int offset;
}

[Serializable]
public class Currently
{
	public int time;
	public string summary;
	public string icon;
	public int nearestStormDistance;
	public int nearestStormBearing;
	public int precipIntensity;
	public int precipProbability;
	public double temperature;
	public double apparentTemperature;
	public double dewPoint;
	public double humidity;
	public double pressure;
	public double windSpeed;
	public double windGust;
	public int windBearing;
	public int cloudCover;
	public int uvIndex;
	public double visibility;
	public double ozone;
}
