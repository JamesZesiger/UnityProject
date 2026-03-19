using UnityEngine;
using System.Collections;

public class EnemyAnimator : MonoBehaviour
{
    public Transform root;
    public SpriteAnimator spriteAnimator;
    public SpriteRenderer sr;

    [Header("Animations")]
    public DirectionalAnimation idle;
    public DirectionalAnimation walk;
    public DirectionalAnimation attack;
    public DirectionalAnimation hit;
    public DirectionalAnimation death;
    public DirectionalAnimation explode;

    private EnemyState currentState;
    private bool isLocked = false; // prevents override
    private Coroutine currentCoroutine = null; // track running coroutine

    void Update()
    {
        if (!isLocked)
        {
            UpdateDirectionalAnimation(GetAnimationSet(currentState));
        }
    }

    public void SetState(EnemyState newState)
    {
        if (isLocked) return;
        currentState = newState;
    }

    public void PlayHit(Vector3 localHitDir, float duration = 0.2f)
    {
        if (isLocked) return;
        StartLockedCoroutine(PlayTemporary(hit, duration));
    }


    public void PlayDeath()
    {
        Debug.Log("Death");
        // Death overrides everything
        StopCurrentCoroutine();
        StartLockedCoroutine(PlayLocked(death));
    }

    public void PlayExplosion()
    {
        StopCurrentCoroutine();
        StartLockedCoroutine(PlayLocked(explode));
    }



    private void StartLockedCoroutine(IEnumerator routine)
    {
        StopCurrentCoroutine();
        currentCoroutine = StartCoroutine(routine);
    }

    private void StopCurrentCoroutine()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }
        // Optionally lock sprite to current frame immediately
    }

    private IEnumerator PlayTemporary(DirectionalAnimation anim, float duration)
    {
        isLocked = true;

        UpdateDirectionalAnimation(anim);

        yield return new WaitForSeconds(duration);

        isLocked = false;
        currentCoroutine = null;
    }

    private IEnumerator PlayLocked(DirectionalAnimation anim)
    {
        isLocked = true;
        
        // Get directional frames for current angle
        Sprite[] frames = GetDirectionalFrames(anim);
        if (frames == null || frames.Length == 0)
            yield break;

        // Set all frames to spriteAnimator so it animates naturally
        spriteAnimator.SetFrames(frames);

        // Wait for full duration
        float duration = frames.Length / spriteAnimator.fps;
        yield return new WaitForSeconds(duration-0.1f);

        // Lock to last frame
        Sprite[]dead = new Sprite[] {frames[frames.Length - 1]};
        Debug.Log(dead.Length);
        spriteAnimator.SetFrames(dead);

        // Keep locked
        isLocked = true;
        currentCoroutine = null;
    }

     

    void UpdateDirectionalAnimation(DirectionalAnimation anim)
    {
        if (anim == null) return;

        Vector3 toPlayer = (Camera.main.transform.position - root.position).normalized;
        toPlayer.y = 0;

        float angle = Vector3.SignedAngle(root.forward, toPlayer, Vector3.up);
        angle = (angle + 360f) % 360f;

        sr.flipX = false;
        Sprite[] selected = null;

        if (angle < 22.5f || angle >= 337.5f)
            selected = anim.front;
        else if (angle < 67.5f)
        {
            sr.flipX = true;
            selected = anim.frontRight;
        }
        else if (angle < 112.5f)
        {
            sr.flipX = true;
            selected = anim.right;
        }
        else if (angle < 157.5f)
        {
            sr.flipX = true;
            selected = anim.backRight;
        }
        else if (angle < 202.5f)
            selected = anim.back;
        else if (angle < 247.5f)
            selected = anim.backRight;
        else if (angle < 292.5f)
            selected = anim.right;
        else
            selected = anim.frontRight;

        if (selected != null)
            spriteAnimator.SetFrames(selected);
    }

    Sprite[] GetDirectionalFrames(DirectionalAnimation anim)
    {
        Vector3 toPlayer = (Camera.main.transform.position - root.position).normalized;
        toPlayer.y = 0;

        float angle = Vector3.SignedAngle(root.forward, toPlayer, Vector3.up);
        angle = (angle + 360f) % 360f;

        sr.flipX = false;
        Sprite[] selected = null;

        if (angle < 22.5f || angle >= 337.5f)
            selected = anim.front;
        else if (angle < 67.5f)
        {
            sr.flipX = true;
            selected = anim.frontRight;
        }
        else if (angle < 112.5f)
        {
            sr.flipX = true;
            selected = anim.right;
        }
        else if (angle < 157.5f)
        {
            sr.flipX = true;
            selected = anim.backRight;
        }
        else if (angle < 202.5f)
            selected = anim.back;
        else if (angle < 247.5f)
            selected = anim.backRight;
        else if (angle < 292.5f)
            selected = anim.right;
        else
            selected = anim.frontRight;

        return selected;
    }

    DirectionalAnimation GetAnimationSet(EnemyState state)
    {
        switch (state)
        {
            case EnemyState.Idle: return idle;
            case EnemyState.Walk: return walk;
            case EnemyState.Attack: return attack;
            case EnemyState.Hit: return hit;
            case EnemyState.Death: return death;
            case EnemyState.Explode: return explode;
        }
        return idle;
    }
}