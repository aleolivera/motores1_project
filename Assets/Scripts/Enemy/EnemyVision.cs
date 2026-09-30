using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    [SerializeField] Transform _eye;
    [SerializeField] float _range = 20f;
    [SerializeField, Range(1, 360)] float _fov = 90f;
    [SerializeField] LayerMask _playerMask;
    [SerializeField] LayerMask _obstacleMask;
    
    readonly Collider[] buffer = new Collider[4];
    public float Range { get { return _fov; } }

    private void Start () {
        if(_eye == null) {
            Debug.Log("EnemyVision: Eye is null");
        }
    }

    public float GetVisibility (out Transform target) {
        target      = null;
        int count   = Physics.OverlapSphereNonAlloc(_eye.position, _range, buffer, _playerMask);
        if (count == 0) return 0f;

        Collider collider       = buffer[0];
        Bounds colliderBounds   = collider.bounds;
        Vector3 toCenter        = colliderBounds.center - _eye.position;

        if (Vector3.Angle(_eye.forward, toCenter) > _fov * 0.5f) return 0f;

        Vector3[] points = {
            new Vector3(
                colliderBounds.center.x, 
                colliderBounds.max.y - 0.1f, 
                colliderBounds.center.z),   // cabeza
            colliderBounds.center,          // torso
            new Vector3(
                colliderBounds.center.x, 
                colliderBounds.min.y + 0.1f, 
                colliderBounds.center.z)    // pies
        };

        int visible = 0;
        foreach (var p in points) {
            Vector3 dir = p - _eye.position;
            if (!Physics.Raycast(_eye.position, dir.normalized, dir.magnitude, _obstacleMask))
                visible++;
        }

        if (visible > 0) target = collider.transform;
        return visible / (float)points.Length;
    }

    void OnDrawGizmosSelected () {
        if (_eye == null) return;
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_eye.position, _range);

        Vector3 left    = Quaternion.AngleAxis(-_fov * 0.5f, _eye.up) * _eye.forward;
        Vector3 right   = Quaternion.AngleAxis(_fov * 0.5f, _eye.up) * _eye.forward;
        
        Gizmos.DrawRay(_eye.position, left * _range);
        Gizmos.DrawRay(_eye.position, right * _range);
    }
}
