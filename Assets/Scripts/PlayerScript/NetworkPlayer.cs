using Fusion;
using UnityEngine;

public class Player : NetworkBehaviour
{
    private NetworkCharacterController _cc;
    private Camera _mainCam;

    public float moveSpeed = 5f;


    private void Awake()
    {
        _cc = GetComponent<NetworkCharacterController>();
    }
    public override void Spawned()
    {
        if (Object.HasInputAuthority)
            _mainCam = Camera.main;
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out PlayerInput input))
        {
            Vector3 moveDir = input.MoveDirection;
            moveDir.y = 0; 

            _cc.Move(moveSpeed * moveDir.normalized * Runner.DeltaTime);

        }

        if (Object.HasInputAuthority)
        {
            RotateToMouse();
        }
    }

    private void RotateToMouse()
    {
        if (_mainCam == null) return;

        Ray ray = _mainCam.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, transform.position);

        if (groundPlane.Raycast(ray, out float hitDist))
        {
            Vector3 hitPoint = ray.GetPoint(hitDist);
            Vector3 lookDir = hitPoint - transform.position;
            lookDir.y = 0;

            if (lookDir.sqrMagnitude > 0.001f)
            {
                transform.rotation = Quaternion.LookRotation(lookDir);
            }
        }
    }
}