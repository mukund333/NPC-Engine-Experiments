using System.Collections;
using UnityEngine;

public class ThrustMechanics : MonoBehaviour, IThrustControl
{
    //public float MAX_SPEED = 10f;

    private Rigidbody2D rb;
     [SerializeField]  private IGearSystem gearSystem;
    [SerializeField] private float currentSpeed;
    private Coroutine accelerationCoroutine;

    [Header("Movement Curve")]
    public AnimationCurve movementCurve;
    private float lerpValue;
    private float startSpeed;
    public float targetSpeed;
    private float duration = 1f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gearSystem = GetComponent<IGearSystem>();
        currentSpeed = 0f;
    }

    public void Accelerate()
    {
        StopCoroutines();
        accelerationCoroutine = StartCoroutine(AccelerateOverTime());
    }

    public void Decelerate()
    {
        StopCoroutines();
        accelerationCoroutine = StartCoroutine(DecelerateOverTime());
    }

    private IEnumerator AccelerateOverTime()
    {
        lerpValue = 0f;
        float timeElapsed = 0f;
        startSpeed = currentSpeed;
        targetSpeed = gearSystem.GetMaxSpeed();

        while (timeElapsed < duration)
        {
            float t = timeElapsed / duration;
            lerpValue = Mathf.Lerp(startSpeed, targetSpeed, movementCurve.Evaluate(t));
            currentSpeed = lerpValue;
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        currentSpeed = targetSpeed;
    }

    private IEnumerator DecelerateOverTime()
    {
        lerpValue = 0f;
        float timeElapsed = 0f;
        startSpeed = currentSpeed;
        targetSpeed = gearSystem.GetMaxSpeed();

        while (timeElapsed < duration)
        {
            float t = timeElapsed / duration;
            lerpValue = Mathf.Lerp(startSpeed, targetSpeed, movementCurve.Evaluate(t));
            currentSpeed = lerpValue;
            timeElapsed += Time.deltaTime;

            if (currentSpeed <= targetSpeed + 0.1f)
            {
                currentSpeed = targetSpeed;
                break;
            }
            yield return null;
        }
    }

    private void FixedUpdate()
    {
        // Apply the current speed continuously
        rb.velocity = transform.up * currentSpeed;
    }

    private void StopCoroutines()
    {
        if (accelerationCoroutine != null) StopCoroutine(accelerationCoroutine);
    }
}
