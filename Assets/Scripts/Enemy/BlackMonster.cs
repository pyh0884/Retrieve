using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackMonster : MonoBehaviour
{
	public Vector2 initialVelocity;
	public Transform groundCheck;
	public LayerMask groundLayer;
	public float distance;
	public bool grounded;

	private bool landed;
	private Animator anim;
	private Rigidbody2D wormRigidbody;
	private SpriteRenderer sr;

	// Start is called before the first frame update
	void Awake()
	{
		anim = GetComponent<Animator>();
		wormRigidbody = GetComponent<Rigidbody2D>();
		sr = GetComponent<SpriteRenderer>();
	}

	void Start()
    {
		landed = false;
		if (transform.rotation.y != 0) { initialVelocity.x *= -1; sr.flipX = true; }
		wormRigidbody.velocity = initialVelocity;
    }

    // Update is called once per frame
    void Update()
    {
		if (!landed)
		{
			if (isGround)
			{
				landed = true;
				anim.SetBool("landed", true);
				wormRigidbody.gravityScale = 0;
				wormRigidbody.velocity = new Vector2(initialVelocity.x / 2, 0);
				sr.flipX = (!sr.flipX);
			}
		}
		else {
			if (!isGround) {
				wormRigidbody.velocity *= -1;
				sr.flipX = (!sr.flipX);
			}
		}
    }
	bool isGround
	{
		get
		{
			Vector2 start = groundCheck.position;
			Vector2 end = new Vector2(start.x, start.y - distance);
			Debug.DrawLine(start, end, Color.blue);
			grounded = Physics2D.Linecast(start, end, groundLayer);
			return grounded;
		}
	}
}
