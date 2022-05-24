using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Rubble : MonoBehaviour
{
    public void StartDeactivation(float wait)
    {
        StartCoroutine(DeactivateRoutine(wait));
    }

    private IEnumerator DeactivateRoutine(float wait)
    {
        yield return new WaitForSeconds(wait);
        Transform[] children = GetComponentsInChildren<Transform>();
        GameObject[] gos = children.Select(transform => transform.gameObject).ToArray();
        foreach (Transform child in children)
        {
            Component[] components = child.GetComponents<Component>();
            foreach(Component component in components)
            {
                if(!(component is Transform)
                && !(component is MeshFilter)
                && !(component is MeshRenderer))
                {
                    Destroy(component);
                }
            }
            child.gameObject.isStatic = true;
        }
        gameObject.isStatic = true;
        StaticBatchingUtility.Combine(gos, gameObject);
    }
}

