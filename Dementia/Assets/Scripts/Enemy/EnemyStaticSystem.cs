using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EnemyStaticSystem : MonoBehaviour
{
    [SerializeField] private GameObject prefab;

    public void MoveToPosition(Vector3 origin, Vector3 destination, float speed, GameObject prefab = null, string animationName = "Walk")
    {
        if (prefab == null)
        {
            prefab = this.prefab;
        }

        prefab = Instantiate(prefab, origin, Quaternion.identity);
        Animator prefabAnimator = prefab.GetComponent<Animator>();
        prefabAnimator.Play(animationName);
        prefab.transform.LookAt(destination);
        StartCoroutine(MoveTowards(prefab, destination, speed));
    }

    public IEnumerator MoveTowards(GameObject prefab, Vector3 destination, float speed)
    {
        while (Vector3.Distance(prefab.transform.position, destination) > .5f)
        {
            var step =  speed * Time.deltaTime / 2; 
            prefab.transform.position = Vector3.MoveTowards(prefab.transform.position, destination, step);
            yield return null;            
        }
        Destroy(prefab);

    }
    
}
