using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> _treePrefabs = new List<GameObject>();
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DelayTreeSpawn(Random.Range(0f, 2f)));
    }

    private void SpawnTree()
    {
        //random chance to spawn a tree
        if (Random.Range(0, 100) > 30)
        {
            return;
        }
        
        //instantiate a random tree prefab from the list
        int randomIndex = Random.Range(0, _treePrefabs.Count);
        Instantiate(_treePrefabs[randomIndex], transform.position, Quaternion.identity, transform);
        
        //set scale to 000
        transform.localScale = Vector3.zero;
        
        //tween scale up
        LeanTween.scale(gameObject, new Vector3(1.2f, 1.2f, 1.2f), 0.5f).setEase(LeanTweenType.easeOutBack);
        

    }
    
    //coroutine to delay tree spawn
    public IEnumerator DelayTreeSpawn(float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnTree();
    }
}
