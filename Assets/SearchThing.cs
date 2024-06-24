using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchThing : MonoBehaviour
{ 
      public PlanetPathFinder pathFinder;
    public List<PlanetScript> pathToFollow;
    public GameObject startScreen;

// Start is called before the first frame update
    public GameObject player;
    public void DoPlanet(string planetName)
    {
        print("aset");
        startScreen.SetActive(false);
        PlanetScript endPlanet = pathFinder.getPlanetByName(planetName);
        if ( endPlanet==null)        {
            return;
        }
        pathToFollow = pathFinder.pathFindPlanet(player.transform, endPlanet);
        print(pathToFollow.Count);  
        print("Droing tusfadjf");
    }

    private void Start()
    {
        
    }
    private void Update()
    {
        
    }
}
