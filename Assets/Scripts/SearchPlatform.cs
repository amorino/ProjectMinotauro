using UnityEngine;
using System.Collections;

public class SearchPlatform : MonoBehaviour {

    bool searching = false;

	void Start () { }
	
	void Update ()
    {
        if(searching)
            Search();
    }

    void Search()
    {
        Debug.Log(this + "-Search");
        RaycastHit hit;
        Ray platformToVectorDown = new Ray(gameObject.transform.position, Vector3.down);
        if (Physics.Raycast(platformToVectorDown, out hit, (float)2))
        {
            Debug.Log(this + "-Search, gameObject.name: " + hit.collider.gameObject.name);
        }
    }

    public void StartSearch ()
    {
        searching = true;  
    }

    public void StopSearch()
    {
        searching = false;
    }

    public bool GetSearching()
    {
        return searching;
    }
}
