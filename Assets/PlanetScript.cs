using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlanetScript : MonoBehaviour
{
    public int uuid;
    public string description = string.Empty;
    public List<PlanetScript> closePlanets;
    public List<Sprite> images;
    
    public int ComparePlanets(PlanetScript p1, PlanetScript p2)
    {
        double p1Dist = (p1.transform.position - transform.position).sqrMagnitude;
        double p2Dist = (p2.transform.position - transform.position).sqrMagnitude;
        if (p1Dist == p2Dist)
        {
            return 0;
        }
        if (p1Dist < p2Dist)
        {
            return -1;
        }
        return 1;
    }

    void Start()
    {
        GetComponent<CircleCollider2D>().radius /= transform.localScale.x;
        GetComponent<CircleCollider2D>().radius *= 2;
        closePlanets.Sort(ComparePlanets);
        foreach (Collider2D collision in Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), 10))
        {
            OnTriggerEnter2D(collision);
        }

        GetComponent<Image>().sprite = images[Random.Range(0,images.Count - 1)];
        GetComponent<Image>().color = new Color(Random.Range(0, 255), Random.Range(0, 255), Random.Range(0, 255));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.isTrigger) return;
        if (collision.gameObject.CompareTag("Planet"))
        {
            if (closePlanets.Contains(collision.gameObject.GetComponent<PlanetScript>())) return;
            closePlanets.Add(collision.gameObject.GetComponent<PlanetScript>());
            closePlanets.Sort(ComparePlanets);
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.isTrigger) return;
        if (collision.gameObject.CompareTag("Planet"))
        {
            closePlanets.Remove(collision.gameObject.GetComponent<PlanetScript>());
            closePlanets.Sort(ComparePlanets);

        }
    }
}
