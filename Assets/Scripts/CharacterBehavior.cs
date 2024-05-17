using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehavior : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float rotationSpeed = 10f;

    private Animator _animator;
    private Coroutine _movementCoroutine;
    
    private void Start()
    {
        _animator = GetComponent<Animator>();
        gameObject.SetActive(false);
    }

    public void FollowPath(List<Vector3> path)
    {
        if (_movementCoroutine != null) return;
        
        gameObject.SetActive(true);
        _animator.SetFloat("Speed", speed);
        _animator.SetFloat("MotionSpeed", speed / 4);
        _movementCoroutine = StartCoroutine(ProcessMovement(path));
    }

    public void ForceStop()
    {
        if (_movementCoroutine != null) StopCoroutine(_movementCoroutine);
        _movementCoroutine = null;
        gameObject.SetActive(false);
    }
    
    private IEnumerator ProcessMovement(List<Vector3> path)
    {
        transform.position = path[0];
        transform.LookAt(path[1]);

        for (int i = 1; i < path.Count; i++)
        {
            var startPosition = transform.position;
            var endPosition = path[i];
            var journeyLength = Vector3.Distance(startPosition, endPosition);
            var startTime = Time.time;

            var direction = (endPosition - startPosition).normalized;
            if (direction == Vector3.zero) continue;
            
            var targetRotation = Quaternion.LookRotation(direction);
            while (Vector3.Distance(transform.position, endPosition) > 0.01f)
            {
                var distCovered = (Time.time - startTime) * speed;
                var fractionOfJourney = distCovered / journeyLength;
                transform.position = Vector3.Lerp(startPosition, endPosition, fractionOfJourney);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

                yield return null;
            }

            transform.position = endPosition;
            transform.rotation = targetRotation;
        }
        
        gameObject.SetActive(false);
        _movementCoroutine = null;
    }
}
