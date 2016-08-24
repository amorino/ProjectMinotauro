using UnityEngine;
using System.Collections;

public class MovePlatform : MonoBehaviour {

    bool drag = false;
    bool back = false;
    bool next = false;

    Camera gameplayCamera;
    Vector3 lastPosition;
    Vector3 nextPosition;

    void Start () {}

    void Update()
    {
        if (drag)
            DragPlatform();

        if (back)
            BackPlatform();

        if (next)
            MoveToNextPosition();

    }

    void MoveToNextPosition()
    {
        Debug.Log(this + "- MoveToNextPosition");
        next = false;
    }

    void DragPlatform()
    {
        Ray cameraToStage = new Ray(gameplayCamera.ScreenToWorldPoint(Input.mousePosition), gameplayCamera.transform.forward);
        RaycastHit hit;
        int layerMask = 1 << LayerMask.NameToLayer("DragCollider");

        if (Physics.Raycast(cameraToStage, out hit, (float)25, layerMask))
            transform.position = hit.point;
    }

    void BackPlatform()
    {
        Debug.Log(this + "- Back Platform");
        back = false;
    }

    public void StartDrag()
    {
        drag = true;
    }

    public void StopDrag()
    {
        drag = false;
    }

    public bool GetDrag()
    {
        return drag;
    }

    public void SetCamera(Camera camera)
    {
        if(gameplayCamera == null)
            gameplayCamera = camera;
    }

    public void StartBack()
    {
        back = true;
    }

    public void SetNextPosition(Vector3 position)
    {
        nextPosition = position;
        next = true;
        //Debug.Log(this + "-SetNextPosition, nextPosition: " + nextPosition);
    }

    public void SetLastPosition(Vector3 position)
    {
        lastPosition = position;
        //Debug.Log(this + "-SetLastPosition, lastPosition: " + lastPosition);
    }

    public Vector3 GetLastPosition()
    {
        //Debug.Log(this + "-GetLastPosition, lastPosition: " + lastPosition);
        return lastPosition;
    }

}
