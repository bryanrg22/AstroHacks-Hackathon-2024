using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Reflection.Emit;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] float velX = 0.0f;
    [SerializeField] float velY = 0.0f;
    [SerializeField] float velCap;
    [SerializeField] float velMult = 1f;
    private Rigidbody2D rb;
    private ChatGPTManager chatGPTManager;
    [SerializeField] GameObject textBoxPrefab;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        float axisX = Input.GetAxis("Horizontal");
        float axisY = Input.GetAxis("Vertical");

        velX += axisX * velMult;
        velY += axisY * velMult;

        velX = Mathf.Max(-velCap, Mathf.Min(velX, velCap));
        velY = Mathf.Max(-velCap, Mathf.Min(velY, velCap));

        rb.velocity = new Vector2(velX,velY);

        //Debug.Log(chatGPTManager.storedResponse);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Planet")
        {
            if (other.isTrigger) return;
            PlanetScript planetScript = other.gameObject.GetComponent<PlanetScript>();

            if (planetScript.description.Equals(string.Empty))
            {
                StartCoroutine(GenerateDescription(planetScript));
            }else
            {
                GameObject currPrefab = Instantiate(textBoxPrefab, Vector3.zero, textBoxPrefab.transform.rotation, planetScript.gameObject.transform);
                currPrefab.GetComponent<RectTransform>().SetLocalPositionAndRotation(new Vector3(100, 100, 0), textBoxPrefab.transform.rotation);
                currPrefab.transform.Find("DescriptionBox").gameObject.GetComponent<TMP_Text>().text = planetScript.description;
                currPrefab.transform.localScale = (new Vector3(1 / planetScript.gameObject.transform.localScale.x, 1 / planetScript.gameObject.transform.localScale.y, 1 / planetScript.gameObject.transform.localScale.z));

            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Planet")
        {
            if (other.isTrigger) return;

            Destroy(other.gameObject.transform.Find("TextBoxBackground(Clone)").gameObject);
        }
    }

    private IEnumerator GenerateDescription(PlanetScript planetScript)
    {
        GPTManagerManager.instance.AskChatGPT("Give me a random, unique planet description based off the name" + planetScript.gameObject.name + ", just the string of the description, nothing else, starting with the name in the first sentence");
        while (GPTManagerManager.instance.storedResponse.Equals(""))
        {
            yield return null;
        }
        planetScript.description = GPTManagerManager.instance.storedResponse;
        GPTManagerManager.instance.storedResponse = "";

        GameObject currPrefab = Instantiate(textBoxPrefab, Vector3.zero, textBoxPrefab.transform.rotation, planetScript.gameObject.transform);
        currPrefab.GetComponent<RectTransform>().SetLocalPositionAndRotation(new Vector3(100, 100, 0), textBoxPrefab.transform.rotation);
        currPrefab.transform.Find("DescriptionBox").gameObject.GetComponent<TMP_Text>().text = planetScript.description;
        currPrefab.transform.localScale = (new Vector3(1 / planetScript.gameObject.transform.localScale.x, 1 / planetScript.gameObject.transform.localScale.y, 1 / planetScript.gameObject.transform.localScale.z));
        //.GetComponent<TextMeshPro>());
    }
}
