using System.Collections;
using UnityEngine;

public class SoundPool : Pool<AudioSource> {
    private static SoundPool _instance;
    public static SoundPool Instance { get { return _instance; } }

    public void Awake () {
        if (_instance == null) {
            _instance = this;
        } else {
            Destroy(gameObject);
            _instance = null;
        }
    }

    private void Start () {
        InitiatePool();
    }

    override protected void InitiatePool () {
        if (_pool.Count > 0) return;

        for (int i = 0; i < _capacity; i++) {
            AudioSource sound = Instantiate(_prefab, transform.position, transform.rotation);
            sound.gameObject.SetActive(false);
            _pool.Enqueue(sound);
        }
    }

    public override AudioSource GetObject () {
        AudioSource sound;
        if (_pool.Count == 0) {
            sound = Instantiate(_prefab, transform.position, transform.rotation);
            _capacity++;
        } else {
            sound = _pool.Dequeue();
        }

        sound.gameObject.SetActive(true);
        return sound;
    }

    public override void ReturnObject (AudioSource sound) {
        sound.gameObject.SetActive(false);
        _pool.Enqueue(sound);
    }

    public override void ReturnObject (AudioSource sound, float time) {
        StartCoroutine(CountdownCoroutine(sound, time));
    }

    private IEnumerator CountdownCoroutine (AudioSource sound, float time) {
        yield return new WaitForSeconds(time);
        ReturnObject(sound);
    }
}
