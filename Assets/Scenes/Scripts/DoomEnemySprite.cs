using UnityEngine;

public class DoomEnemySprite : MonoBehaviour
{
    public SpriteAnimator animator;
    public Transform root; // the Enemy root (not visual)

    public Sprite[] front;
    public Sprite[] frontRight;
    public Sprite[] right;
    public Sprite[] backRight;
    public Sprite[] back;

    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Vector3 toPlayer = (Camera.main.transform.position - root.position).normalized;
        toPlayer.y = 0;

        float angle = Vector3.SignedAngle(root.forward, toPlayer, Vector3.up);
        angle = (angle + 360f) % 360f;

        // Reset flip
        sr.flipX = false;

        if (angle < 22.5f || angle >= 337.5f)
        {
            animator.SetFrames(front);
        }
        else if (angle < 67.5f)
        {
            sr.flipX = true;
            animator.SetFrames(frontRight);
        }
        else if (angle < 112.5f)
        {
            sr.flipX = true;
            animator.SetFrames(right);
        }
        else if (angle < 157.5f)
        {
            sr.flipX = true; // flip horizontally
            animator.SetFrames(backRight);
        }
        else if (angle < 202.5f)
        {
            animator.SetFrames(back);
        }
        else if (angle < 247.5f) // LEFT SIDE → mirror right sprites
        {
            
            animator.SetFrames(backRight);
        }
        else if (angle < 292.5f)
        {
            
            animator.SetFrames(right);
        }
        else // front-left
        {
            
            animator.SetFrames(frontRight);
        }
    }
}