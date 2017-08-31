using UnityEngine;
using System.Collections;

public class CVMPortal : JWMonoBehaviour {
    GameObject Caveman;
    int cavemenLimit = 3;

    // Use this for initialization
    void Start () {
        Caveman = Resources.Load<GameObject>("Prefab/Map/Caveman");
        StartCoroutine(InstantiateCaveman());
    }

    // Update is called once per frame
    void Update () {
	
	}

    IEnumerator InstantiateCaveman()
    {
        while (true)
        {
            yield return new WaitForSeconds(3);
            bool existingFreeCaveman = false;
            CVMCaveman[] cavemen = FindObjectsOfType<CVMCaveman>();
            if (cavemen.Length >= cavemenLimit)
                continue;
            foreach (CVMCaveman caveman in cavemen){
                if (caveman.onBuilding == false)
                    existingFreeCaveman = true;
            }
            if (!existingFreeCaveman)
            {
                JWInstantiate(Caveman, gameObject, Vector3.zero);
                transform.DetachChildren();
            }
        }
    }
}
