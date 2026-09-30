using UnityEngine;

public interface IPoolable <T>{
    public T GetObject ();
    public void ReturnObject (T obj);
}
