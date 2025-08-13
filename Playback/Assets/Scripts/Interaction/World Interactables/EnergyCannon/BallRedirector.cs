using UnityEngine;

public class BallRedirector : MonoBehaviour
{
    private GameObject prefab;

    [SerializeField] private Transform spawnLocation;

    public void SetPrefab(GameObject newPrefab)
    {
        prefab = newPrefab;
    }

    public void InstantiatePrefab(EnergyCannon owner, int redirects)
    {
        GameObject ballGO = Instantiate(prefab, spawnLocation.position, spawnLocation.rotation);
        ballGO.GetComponent<EnergyBall>().owner = owner;
        ballGO.GetComponent<EnergyBall>().redirects = redirects + 1;
    }
}
