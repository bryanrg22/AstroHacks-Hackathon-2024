using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Assertions;

public class PlanetPathFinder : MonoBehaviour
{
    private class Ant
    {
        public double lengthTraveled = 0.0;
        public static PlanetScript endPlanet = null;
        public List<PlanetScript> pathTaken;
        public HashSet<int> planetsTouched;

        public Ant()
        {
            pathTaken = new List<PlanetScript>();
            planetsTouched = new HashSet<int>();
        }

        private void _register(PlanetScript planet)
        {
            if (planetsTouched.Contains(planet.uuid))
            {
                throw new System.Exception("YOUR BAD");
            } 
            planetsTouched.Add(planet.uuid);
            pathTaken.Add(planet);
        }
        private PlanetScript getCurrentPlanet()
        {
            return pathTaken[pathTaken.Count - 1];
        }

        public List<PlanetScript> getClosestPlanets()
        {
            PlanetScript currentPlanet = pathTaken[pathTaken.Count - 1];
            return currentPlanet.closePlanets;
        }
        public void travel(PlanetScript startPlanet)
        {
            planetsTouched.Clear();
            pathTaken.Clear();
            _register(startPlanet);
            while (getCurrentPlanet().name != endPlanet.name)
            {
                List<PlanetScript> closestPlanets = getClosestPlanets();
                for (int i = closestPlanets.Count-1; i >-1; i--) 
                { 
                    if (planetsTouched.Contains(closestPlanets[i].uuid))
                    {
                        closestPlanets.RemoveAt(i);
                    }
                }
                if (closestPlanets.Count == 0)
                {
                    lengthTraveled = double.MaxValue;
                    print("Returning");
                    return;
                }
                int index = Random.Range(0, closestPlanets.Count);

                PlanetScript nextPlanet = closestPlanets[index];
                double dist = getPlanetDistance(nextPlanet, getCurrentPlanet());
                lengthTraveled += dist;
                print("Going to " + nextPlanet.name);
                _register(nextPlanet);
            }
        }

    }
    [SerializeField] private List<PlanetScript> planets;
    private List<Ant> ants = new List<Ant>();
    public const int NUMBER_OF_ANTS = 5000;

    public PlanetScript getPlanetByName(string name)
    {
        foreach (PlanetScript planet in planets)
        {
            if (planet.name == name)
            {
                return planet;
            }
        }
        return null;
    }

    public PlanetScript getClosestPlanetToLocation(Transform transform)
    {
        PlanetScript closest = planets[0];
        double x = transformDistanceSquared(closest.transform, transform);
        for (int i = 1; i < planets.Count;i++)
        {
            double n = transformDistanceSquared(planets[i].transform, transform);
            if (n < x)
            {
                x = n;
                closest = planets[i];
            }
        }
        return closest;
    }


    // Start is called before the first frame update
    void Start()
    {
        planets = new List<PlanetScript>(FindObjectsOfType<PlanetScript>());
        print(planets.Count);
        for (int i = 0; i< NUMBER_OF_ANTS; i++)
        {
            ants.Add(new Ant());
        }
        int j = 0;
        foreach (PlanetScript p in planets )
        {
            p.uuid = j;
            j++;
        }
    }

    public static double getPlanetDistance(PlanetScript p1,PlanetScript p2)
    {
        return (p1.transform.position - p2.transform.position).magnitude;
    }
    public static double transformDistanceSquared(Transform t, Transform p)
    {
        return (t.position - p.position).sqrMagnitude;
    }

    public List<PlanetScript> pathFindPlanet(Transform start, PlanetScript end)
    {
        List<PlanetScript> startingPlanets = new List<PlanetScript>
        {
            getClosestPlanetToLocation(start)
        };
        Vector3 dist = startingPlanets[0].transform.position - start.position;
        double sPDist = transformDistanceSquared(startingPlanets[0].transform, start);
        if (sPDist > 1)
        {
            dist.Normalize();
            foreach (PlanetScript p in startingPlanets[0].closePlanets)
            {
                Vector3 sub = (p.transform.position - start.position).normalized;
                if (sub.x*dist.x + sub.y*dist.y > 0.8)
                {
                    continue;
                }
                startingPlanets.Add(p);
            }

        }
        Ant.endPlanet = end;
        foreach (Ant ant in ants)
        {
            int index = Random.Range(0, startingPlanets.Count);
            ant.travel(startingPlanets[0]);
        }
        Ant best = ants[0];
        foreach (Ant ant in ants)
        {
            if (ant.lengthTraveled < best.lengthTraveled) {
                best = ant;
            }
        }
        print("Starintplanets" + startingPlanets.Count);
        print("sdfg " + best.pathTaken.Count);
        return best.pathTaken;
    }
    
}
