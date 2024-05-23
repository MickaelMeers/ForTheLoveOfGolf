using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public GameObject CollectibleManager;

    public int index;

    /// <summary>
    /// Quand le player est d�t�cter, il appelle le manager et ajoute la pi�ce
    /// </summary>
    /// <param name="other">le collider qui entre en contact</param>
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            CollectibleManager.GetComponent<CollectibleManager>().AddCollectible(index);
        }
    }
}
