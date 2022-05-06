using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Collections;
using SimpleJSON;

public class WeatherData : MonoBehaviour
{
	private TextMeshProUGUI textWeatherCurrent;

	private FindLocation findLocation;
	private JSONNode weatherInfo;

	private string apiKey = "c8f99e79f0bb84c2be80658b48c16be1";
	private string uri;
	private float timer = 0;
	private float updateIntervalMinutes = 10;

	private string city;
	private float latitude;
	private float longitude;
	private bool locationFound = false;
	private bool useCity = false;

	private string weatherMain;

    private void Start()
    {
		textWeatherCurrent = gameObject.GetComponent<TextMeshProUGUI>();
		findLocation = gameObject.transform.root.GetComponent<FindLocation>();
		uri = "https://api.openweathermap.org/data/2.5/weather?";
    }

    private void FixedUpdate()
	{
		if (locationFound == true)
		{
			if (timer <= 0)
			{
				timer = updateIntervalMinutes * 60;
				StartCoroutine(GetWeatherInfo());
			}
			else
			{
				timer -= Time.fixedDeltaTime;
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

		var webRequest = new UnityWebRequest("https://api.openweathermap.org/data/2.5/weather?q=Timisoara,Ro&appid=c8f99e79f0bb84c2be80658b48c16be1")
		{
			downloadHandler = new DownloadHandlerBuffer()
		};

		yield return webRequest.SendWebRequest();

		if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
        {
			Debug.Log("Web Request Error:" + webRequest.result);
			yield break;
		}

		weatherInfo = JSON.Parse(webRequest.downloadHandler.text);

		weatherMain = weatherInfo["weather"][0]["main"];

		//textWeatherCurrent.text = "Current Weather: " + weatherInfo["weather"].ToString() + " | " + weatherInfo["name"].ToString();
		textWeatherCurrent.text = "Current Weather: " + weatherMain.ToString();

		//Debug.Log("Weather Info: " + weatherInfo["weather"].ToString());
		//Debug.Log("Main Weather Info: " + weatherMain);
		//Debug.Log("City: " + weatherInfo["name"]);

		yield break;
	}
}
