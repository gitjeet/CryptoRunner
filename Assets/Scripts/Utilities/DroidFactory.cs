using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Networking;
public class DroidFactory : Singleton<DroidFactory>
{
    [SerializeField] private Droid[] availableDroids;
    [SerializeField] private float waitTime = 180.0f;
    [SerializeField] private int startingDroids = 5;
    [SerializeField] private float minRange = 0.50f;
    [SerializeField] private float maxRange = 15.0f;
    [SerializeField] private string apiUrl = "https://aptos-api-8fm1.onrender.com/genrandloc";

    private List<Droid> liveDroids = new List<Droid>();
    private Droid selectedDroid;
    private Player player;

    public List<Droid> LiveDroids
    {
        get { return liveDroids; }
    }
    public Droid SelectedDroid
    {
        get { return selectedDroid; }
    }
    private void Awake()
    {
        Assert.IsNotNull(availableDroids);
    }
    void Start()
    {
        player = GameManager.Instance.CurrentPlayer;
        Assert.IsNotNull(player);
        for (int i = 0; i < startingDroids; i++)
        {
            InstantiateDroid();
        }

        StartCoroutine(GenerateDroids());
    }
    private IEnumerator GenerateDroids()
    {
        while (true)
        {
            InstantiateDroid();
            yield return new WaitForSeconds(waitTime);
        }
    }
    private void InstantiateDroid()
    {
        int index = Random.Range(0, availableDroids.Length);

       
        if (liveDroids.Count < 2)
        {
            StartCoroutine(FetchRandomNumber((randomNumber) =>
            {
                float x = player.transform.position.x + randomNumber + Random.Range(0.0f, 30.0f);
                float y = player.transform.position.y + 0.25f;
                float z = player.transform.position.z + GenerateRange();
                liveDroids.Add(Instantiate(availableDroids[index], new Vector3(x, y, z), Quaternion.identity));
            }));
        }
        else
        {
            float x = player.transform.position.x + Random.Range(0.0f, 30.0f);
            float y = player.transform.position.y + 0.25f;
            float z = player.transform.position.z + GenerateRange();
            liveDroids.Add(Instantiate(availableDroids[index], new Vector3(x, y, z), Quaternion.identity));
        }
    }
    private IEnumerator FetchRandomNumber(System.Action<float> callback)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(apiUrl))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                string response = webRequest.downloadHandler.text;
                Debug.Log("API Response: " + response);

                // Remove the first occurrence of "
                response = response.Replace("\"", "");

                float randomNumber;
                if (float.TryParse(response, out randomNumber))
                {
                    Debug.Log("Parsed Random Number: " + randomNumber);
                    callback?.Invoke(randomNumber);
                }
                else
                {
                    Debug.LogError("Failed to parse API response as float.");
                }
            }
        }
    }



    public void DroidWasSelected(Droid droid)
    {
        Debug.Log("This droid was selected" + droid);

        selectedDroid = droid;
    }
    private float GenerateRange()
    {
        float randomNum = Random.Range(minRange, maxRange);
        bool isPositive = Random.Range(0, 2) <4;
        return randomNum ;
    }
}
