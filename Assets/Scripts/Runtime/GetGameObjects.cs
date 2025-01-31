using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetGameObjects : MonoBehaviour
{
    public static GetGameObjects instance;

    public List<Coin> collectibles = new List<Coin>();
    public List<Hole> holes = new List<Hole>();
    public List<Door> doors = new List<Door>();
    public List<PannelCollectible> pannels = new List<PannelCollectible>();
    public List<TimeChallenge> challenges = new List<TimeChallenge>();

    private void Start()
    {
        instance = this;
    }

    private void Awake()
    {
        GameObject[] tmp = FindObjectsByType<GameObject>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        for (int i = 0; i < tmp.Length; i++)
        {
            if (tmp[i].TryGetComponent(out Coin currentCollectible))
            {
                collectibles.Add(currentCollectible);
                SaveManager.coinsObject.Add(false);
            }
            if (tmp[i].TryGetComponent(out Hole currentHole))
            {
                holes.Add(currentHole);
                SaveManager.holesObject.Add(false);
            }
            if (tmp[i].TryGetComponent(out Door currentDoor))
            {
                doors.Add(currentDoor);
                SaveManager.doors.Add(false);
            }
            if (tmp[i].TryGetComponent(out PannelCollectible currentPannel))
            {
                pannels.Add(currentPannel);
                SaveManager.pannels.Add(false);
            }
            if (tmp[i].TryGetComponent(out TimeChallenge currentChallenge))
            {
                challenges.Add(currentChallenge);
                SaveManager.challenges.Add(false);
            }
        }
    }
}
