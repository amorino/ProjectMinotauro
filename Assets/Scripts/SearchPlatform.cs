using UnityEngine;
using System.Collections;

public class SearchPlatform : MonoBehaviour {

    bool searching = false;
    bool find = false;
    Vector3 nextPosition;
    GameObject otherPlatform;

	void Start () { }
	
	void Update ()
    {
        if(searching)
            Search();
    }

    void Search()
    {
        RaycastHit hit;
        Ray platformToVectorDown = new Ray(gameObject.transform.position, Vector3.down);
        int layerMask = 1 << LayerMask.NameToLayer("Platforms");
        if (Physics.Raycast(platformToVectorDown, out hit, (float)2, layerMask))
        {
            Debug.Log(this + "-Search, gameObject.name: " + hit.collider.gameObject.name);
            otherPlatform = hit.collider.gameObject;
            find = true;
            nextPosition = hit.collider.gameObject.transform.position;
            Debug.Log(this + "-Search, nextPosition: " + nextPosition);
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

    public bool GetFind()
    {
        return find;
    }

    public void SetFind(bool value)
    {
        find = value;
    }

    public Vector3 GetNextPosition()
    {
        return nextPosition;
    }

    public GameObject GetOtherPlatform()
    {
        return otherPlatform;
    }
}
