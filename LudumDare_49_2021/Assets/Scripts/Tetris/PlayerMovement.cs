using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D playerRB;

    private Vector2 movementInput;

    private float currentSpeed;

    public GameObject boundL;
    public GameObject boundR;

    public bool canMove = true;

    public float defaultSpeed;
    public float sprintSpeed;

    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        CheckSprint();

        CheckCast();
    }

	private void FixedUpdate()
	{
        MoveHorizontal();
	}

	private void CheckInput()
    {
        movementInput.x = Input.GetAxisRaw("Horizontal");
    }

    private void CheckSprint()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = sprintSpeed;
        }
        else
        {
            currentSpeed = defaultSpeed;
        }
    }

    private void MoveHorizontal()
    {
        Vector2 newPosition = new Vector2(Mathf.Clamp(playerRB.position.x + movementInput.x * currentSpeed, boundL.transform.position.x, boundR.transform.position.x), 0f);
        playerRB.MovePosition(newPosition);
    }

    private void CheckCast()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), 10f);

            Debug.DrawRay(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));

            if (hit.collider != null)
            {
                Debug.Log(hit.collider.gameObject.name);
            }
        }
    }
}
