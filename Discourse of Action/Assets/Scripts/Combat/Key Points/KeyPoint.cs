using UnityEngine;

[RequireComponent(typeof(KeyPointDisplay))]
public class KeyPoint : MonoBehaviour
{
    public KeyPointData keyPointData;
    public GameObject target;

    public int displayIndex;

    private KeyPointManager _keyPointManager;

    private void Start()
    {
        _keyPointManager = FindObjectOfType<KeyPointManager>();
    }

    public void SetKeyPoint()
    {
        AudioManager.instance.PlayClip(AudioManager.instance.SFXSource, AudioManager.instance.buttonSFX);

        _keyPointManager.selectedKeyPoint = this;
        _keyPointManager.SetTarget(this);
    }
}
