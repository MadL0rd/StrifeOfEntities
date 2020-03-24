using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdScript : MonoBehaviour {

    public GameObject bird;
    public GameObject mover;

    private Vector3 target;
    enum birdState 
    {
        wait, move2Target, circle
    }
    private birdState state;

    private Animator animatorBird;
    private Animator animatorMover;

    List<Vector3> circlePoints;
    private int circleIndex;

    private void Start()
    {
        state = birdState.wait;

        animatorBird = bird.GetComponent<Animator>();
        animatorMover = mover.GetComponent<Animator>();
    }
    private void StopBird()
    {
        state = birdState.wait;
        RefrashAnimation();
    }
    public void FlyHome()
    {
        if (Vector3.Distance(transform.position, transform.parent.position) >= 1)
        {
            SetNewTarget(transform.parent);
        }
    }
    public void SetNewTarget(GameObject buff)
    {
        target = buff.transform.position;
        state = birdState.move2Target;
        RefrashAnimation();
    }
    public void SetNewTarget(Transform buff)
    {
        target = buff.position;
        state = birdState.move2Target;
        RefrashAnimation();
    }
    public void SetNewTarget(Vector3 buff)
    {
        target = buff;
        state = birdState.move2Target;
        RefrashAnimation();
    }
    public void FlyCircle(GameObject targetCircle, float length, float height, bool reverce)
    {
        circleIndex = 0;
        state = birdState.circle;
        circlePoints = new List<Vector3>();

        Vector3 buffPosition = targetCircle.transform.position;
        buffPosition.y += height;
        buffPosition.x -= length / 2;
        buffPosition.z -= length / 2;

        circlePoints.Add(buffPosition);

        buffPosition.z += length;
        circlePoints.Add(buffPosition);

        buffPosition.x += length;
        circlePoints.Add(buffPosition);

        buffPosition.z -= length;
        circlePoints.Add(buffPosition);

        if (reverce)
        {
            circlePoints.Reverse();
        }

        RefrashAnimation();
    }

    [Space]
    [Range(1, 20)]
    public float findingRadius;
    public void NextStep()
    {
        transform.Translate(new Vector3(0, 0, 1));

        //если долетел, то остановить
        float distance = Vector3.Distance(transform.position, target);
        if (distance < findingRadius)
        {
            if (state != birdState.circle)
            {
                StopBird();
            }
            else
            {
                RefrashAnimation();
            }
        }
    }

    private void RefrashAnimation()
    {
        switch (state)
        {
            case birdState.wait:
                animatorMover.SetBool("move2Target", false);
                animatorBird.SetBool("flying", false);
                break;

            case birdState.move2Target:
                animatorMover.SetBool("move2Target", true);
                animatorBird.SetBool("flying", true);
                break;

            case birdState.circle:
                circleIndex = (circleIndex + 1) % circlePoints.Count;
                target = circlePoints[circleIndex];
                animatorMover.SetBool("move2Target", true);
                animatorBird.SetBool("flying", true);
                break;

            default:
                break;
        }
    }

    [Space]
    [Range (1, 20)]
    public float rotateSpeed;
    [Space]
    public bool DisableRotation;
    private void Update()
    {
        if (!DisableRotation)
        {
            Vector3 direction = target - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotateSpeed * Time.deltaTime);
        }
    }
}