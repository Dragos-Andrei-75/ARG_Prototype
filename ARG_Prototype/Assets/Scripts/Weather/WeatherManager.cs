using UnityEngine;
using System.Collections;

public class WeatherManager : MonoBehaviour
{
	[SerializeField] private GameObject objectClear;
	[SerializeField] private GameObject objectClouds;
	[SerializeField] private GameObject objectRain;
	[SerializeField] private GameObject objectSnow;

	[SerializeField] private Material materialClouds;
	[SerializeField] private ParticleSystem particleSystemWeaher;

	[SerializeField] private Transform playerCar;

	public WeatherData weatherData;

	[SerializeField] private string weatherCurrent;
	private float weatherEnableTime;
	private float weatherDisableTime;
	private bool clear = false;
	private bool clouds = false;
	private bool rain = false;
	private bool snow = false;
	private bool none = false;

	private void Start()
	{
		//		objectClear = gameObject.transform.GetChild(0).gameObject;
		//		objectClouds = gameObject.transform.GetChild(1).gameObject;
		//		objectRain = gameObject.transform.GetChild(2).gameObject;
		//		objectSnow = gameObject.transform.GetChild(3).gameObject;

		//		objectClear.SetActive(false);
		//		objectClouds.SetActive(false);
		//		objectRain.SetActive(false);
		//		objectSnow.SetActive(false);

		weatherCurrent = weatherData.weatherMain;
		weatherEnableTime = 1.25f;
		weatherDisableTime = 2.5f;
	}

	private void Update()
	{
		if (weatherCurrent == "Clear" && clear == false)
		{
			StartCoroutine(SpawnClear());
			none = false;
		}
		else if (weatherCurrent == "Clouds" && clouds == false)
		{
			StartCoroutine(SpawnClouds());
			none = false;
		}
		else if (weatherCurrent == "Rain" && rain == false)
		{
			StartCoroutine(SpawnRain());
			none = false;
		}
		else if (weatherCurrent == "Snow" && snow == false)
		{
			StartCoroutine(SpawnSnow());
			none = false;
		}
		else if((weatherCurrent != "Clear" && weatherCurrent != "Clouds" && weatherCurrent != "Rain" && weatherCurrent != "Snow") && none == false)
		{
			StartCoroutine(None());
		}

		if ((objectRain.activeSelf == true || objectSnow.activeSelf == true) && particleSystemWeaher != null)
		{
			ParticleSystem.ShapeModule shapeModule = particleSystemWeaher.shape;
			shapeModule.position = new Vector3(playerCar.position.x, 50.0f, playerCar.position.z);
		}
	}

	private IEnumerator SpawnClear()
	{
		yield return new WaitForSeconds(weatherEnableTime);

		clear = true;
		objectClear.SetActive(true);

		particleSystemWeaher = objectClear.GetComponent<ParticleSystem>();

		if (clouds == true) StartCoroutine(DisableClouds());
		else if (rain == true) StartCoroutine(DisableRain());
		else if (snow == true) StartCoroutine(DisableSnow());

		yield break;
	}

	private IEnumerator SpawnClouds()
	{
		yield return new WaitForSeconds(weatherEnableTime);

		clouds = true;
		objectClouds.SetActive(true);

		particleSystemWeaher = objectClouds.GetComponent<ParticleSystem>();

		RenderSettings.skybox = materialClouds;

		if (clear == true) StartCoroutine(DisableClear());
		else if (rain == true) StartCoroutine(DisableRain());
		else if (snow == true) StartCoroutine(DisableSnow());

		yield break;
	}

	private IEnumerator SpawnRain()
	{
		yield return new WaitForSeconds(weatherEnableTime);

		rain = true;
		objectRain.SetActive(true);

		particleSystemWeaher = objectRain.GetComponent<ParticleSystem>();

		if (clear == true) StartCoroutine(DisableClear());
		else if (clouds == true) StartCoroutine(DisableClouds());
		else if (snow == true) StartCoroutine(DisableSnow());

		yield break;
	}

	private IEnumerator SpawnSnow()
	{
		yield return new WaitForSeconds(weatherEnableTime);

		snow = true;
		objectSnow.SetActive(true);

		particleSystemWeaher = objectSnow.GetComponent<ParticleSystem>();

		if (clear == true) StartCoroutine(DisableClear());
		else if (clouds == true) StartCoroutine(DisableClouds());
		else if (rain == true) StartCoroutine(DisableRain());

		yield break;
	}

	private IEnumerator None()
	{
		yield return new WaitForSeconds(weatherEnableTime);

		none = true;

		particleSystemWeaher = null;

		if (clear == true) StartCoroutine(DisableClear());
		else if (clouds == true) StartCoroutine(DisableClouds());
		else if (rain == true) StartCoroutine(DisableRain());
		else if (snow == true) StartCoroutine(DisableSnow());

		yield break;
	}

	private IEnumerator DisableClear()
	{
		clear = false;

		yield return new WaitForSeconds(weatherDisableTime);

		objectClear.SetActive(false);

		yield break;
	}

	private IEnumerator DisableClouds()
	{
		clouds = false;

		yield return new WaitForSeconds(weatherDisableTime);

		objectClouds.SetActive(false);

		yield break;
	}

	private IEnumerator DisableRain()
	{
		rain = false;

		yield return new WaitForSeconds(weatherDisableTime);

		objectRain.SetActive(false);

		yield break;
	}

	private IEnumerator DisableSnow()
	{
		snow = false;

		yield return new WaitForSeconds(weatherDisableTime);

		objectSnow.SetActive(false);

		yield break;
	}
}
