using System.Collections;
using UnityEngine;

public class DestroyDelay : MonoBehaviour
{
    public float delay;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Destroy());
    }

    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
