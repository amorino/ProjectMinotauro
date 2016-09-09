using UnityEngine;
using System.Collections;

public class BoxInteraction : MonoBehaviour {

    //Stage
    public Camera gameplayCamera;
    Vector3 firstMousePosition;
    bool firstClick = false;
    float distance;
    float minDistanceToMove = 5;

    //Platform
    GameObject gameObjectSelected;
    SearchPlatform searchPlatform;
    PlatformRotation platformRotation;
    MovePlatform movePlatform;
	ChangeColor changeColor;

    void Start () {}
	
	void Update () {
        if (Input.GetMouseButtonDown(0))
            OnMouseButtonDown();

        if (Input.GetMouseButtonUp(0))
            OnMouseButtonUp();  

        if (Input.GetMouseButton(0))
            OnMouseButtonHeldDown();
    }

    void OnMouseButtonDown()
    {
        Ray cameraToStage = new Ray(gameplayCamera.ScreenToWorldPoint(Input.mousePosition), gameplayCamera.transform.forward);
        RaycastHit hit;
        int layerMask = 1 << LayerMask.NameToLayer("Platforms");

        if (Physics.Raycast(cameraToStage, out hit, (float)25, layerMask))
        {
            gameObjectSelected = hit.collider.gameObject;
            SetScripts();
            firstMousePosition = Input.mousePosition;
            SetFirstClick(true);
        }
    }

    void SetScripts()
    {
        searchPlatform = gameObjectSelected.GetComponent<SearchPlatform>();
        platformRotation = gameObjectSelected.GetComponent<PlatformRotation>();
        movePlatform = gameObjectSelected.GetComponent<MovePlatform>();
		changeColor = gameObjectSelected.GetComponent<ChangeColor>();
        movePlatform.SetLastPosition(gameObjectSelected.transform.position);
        movePlatform.SetCamera(gameplayCamera);
    }

    void OnMouseButtonUp()
    {
        if (GetFirstClick() && !movePlatform.GetMove() && !platformRotation.GetRotate())
        {
            if (searchPlatform != null && searchPlatform.GetSearching())
                searchPlatform.StopSearch();

            if (platformRotation != null && distance < minDistanceToMove)
                platformRotation.StartRotation();

            if (movePlatform != null && movePlatform.GetDrag())
            { 
                movePlatform.StopDrag();
				changeColor.StopDragging ();
                if (searchPlatform != null && searchPlatform.GetFind())
                    ChagingPositions();
                else
                    BackToLastPosition();
            }
            ClearScripts();
        }
        SetFirstClick(false);
    }

    void BackToLastPosition()
    {
        searchPlatform.SetFind(false);
        MovePlatformToPosition(movePlatform, movePlatform.GetLastPosition(), gameObjectSelected.transform.position);
    }

    void ChagingPositions()
    {
        searchPlatform.SetFind(false);
        MovePlatformToPosition(movePlatform, searchPlatform.GetNextPosition(), gameObjectSelected.transform.position);

        GameObject otherPlatform = searchPlatform.GetOtherPlatform();
        MovePlatform otherMovePlatform = otherPlatform.GetComponent<MovePlatform>();
        MovePlatformToPosition(otherMovePlatform, movePlatform.GetLastPosition(), otherPlatform.transform.position);
    }

    void MovePlatformToPosition(MovePlatform moveTemp, Vector3 nextPosition, Vector3 currentPosition)
    {
        moveTemp.SetNextPosition(nextPosition);
        moveTemp.CalculateLinearTrajectory(currentPosition);
        moveTemp.StartMove();
    }

    void ClearScripts()
    {
        gameObjectSelected = null;
        platformRotation = null;
        searchPlatform = null;
        movePlatform = null;
    }

    void OnMouseButtonHeldDown()
    {
        if(GetFirstClick() && !movePlatform.GetMove() && !platformRotation.GetRotate())
        {
            distance = Vector3.Distance(firstMousePosition, Input.mousePosition);
            if(distance > minDistanceToMove)
                MoveGameObject();
        }
    }

    void MoveGameObject()
    {
		if (movePlatform != null && !movePlatform.GetDrag ()) {
			movePlatform.StartDrag();
			changeColor.IsDragging ();
		}
          

        if (searchPlatform != null && !searchPlatform.GetSearching())
            searchPlatform.StartSearch();
    }

    void SetFirstClick(bool value)
    {
        firstClick = value;
    }

    public bool GetFirstClick()
    {
        return firstClick;
    }
}
