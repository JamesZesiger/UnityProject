using UnityEngine;

public class SpriteAnimator : MonoBehaviour
{
    public Sprite[] frames;
    public float fps = 8f;

    int index;
    float timer;
    SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if(frames.Length == 0) return;

        timer += Time.deltaTime;

        if(timer >= 1f / fps)
        {
            timer = 0f;

            index = (index + 1) % frames.Length; // loops animation
            sr.sprite = frames[index];
        }
    }
    public void SetFrames(Sprite[] newFrames)
    {
        if(frames == newFrames) return;

        frames = newFrames;
        index = 0;
    }
}