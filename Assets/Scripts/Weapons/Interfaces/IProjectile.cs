using UnityEngine;

public interface IProjectile {
    public void Launch (float speed);
    public void DestroyProjectile();
    public void Reset ();
}
