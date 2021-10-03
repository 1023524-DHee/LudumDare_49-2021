using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDown_Movement : MonoBehaviour
{
    private Rigidbody2D playerRB;

    private Coroutine spinningCoroutine;

    private Vector2 movementInputDir;

    private bool canStopSpin = true;
    private float randomSpinDirection;
    private float randomSpinDuration;
    private float randomSpinSpeed;

    public Transform firePoint;
    public GameObject bulletGO;

    [Header("Player movement variables")]
    public float moveSpeed;

    [Header("Spin time variables")]
    public float stopSpinDuration;
    public float stopSpinCooldownDuration;

    [Header("Spin behaviour variables")]
    public float minSpinDuration;
    public float maxSpinDuration;
    public float minSpinSpeed;
    public float maxSpinSpeed;

    

    // Start is called before the first frame update
    private void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();

        spinningCoroutine = StartCoroutine(SpinCharacter());
    }

    // Update is called once per frame
    private void Update()
    {
        CheckMovementInput();

        CheckAttackInput();

        CheckStopSpinInput();

    }

    private void FixedUpdate()
    {
        CharacterMovement();
    }

    private void CheckMovementInput()
    {
        movementInputDir.x = Input.GetAxisRaw("Horizontal");
        movementInputDir.y = Input.GetAxisRaw("Vertical");
    }

    private void CheckAttackInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CharacterAttack();
        }
    }

    private void CheckStopSpinInput()
    {
        if (Input.GetMouseButtonDown(1) && canStopSpin)
        {
            StartCoroutine(StopSpinning());
        }
    }

    private IEnumerator StopSpinning()
    {
        canStopSpin = false;
        StopCoroutine(spinningCoroutine);
        yield return new WaitForSeconds(stopSpinDuration);
        StartCoroutine(SpinCooldown());
    }

    private IEnumerator SpinCooldown()
    {
        spinningCoroutine = StartCoroutine(SpinCharacter());
        yield return new WaitForSeconds(stopSpinCooldownDuration);
        canStopSpin = true;
    }

    private IEnumerator SpinCharacter()
    {
        float startTime = Time.time;

        randomSpinDirection = Random.Range(-1f, 1f);
        randomSpinDuration = Random.Range(minSpinDuration, maxSpinDuration);
        randomSpinSpeed = Random.Range(minSpinSpeed, maxSpinSpeed);

        while (Time.time < startTime + randomSpinDuration)
        {
            transform.rotation = Quaternion.Euler(0, 0, transform.eulerAngles.z + randomSpinDirection * randomSpinSpeed * Time.deltaTime);
            yield return null;
        }

        spinningCoroutine = StartCoroutine(SpinCharacter());
    }

    private void CharacterMovement()
    {
        playerRB.velocity = movementInputDir.normalized * moveSpeed;
    }

    private void CharacterAttack()
    {
        GameObject clone;
        clone = Instantiate(bulletGO, transform.position, transform.rotation);
        clone.GetComponent<Rigidbody2D>().AddForce(firePoint.up * 10f, ForceMode2D.Impulse);
    }
}
