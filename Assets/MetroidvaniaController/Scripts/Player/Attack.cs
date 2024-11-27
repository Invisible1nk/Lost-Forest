using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
	[SerializeField] CharacterController2D controller;
	public float dmgValue = 4;
	public GameObject throwableObject;
	public Transform attackCheck;
	private Rigidbody2D m_Rigidbody2D;
	public Animator animator;
	public bool canAttack = true;
	public bool canThrow = true;	
	public bool isTimeToCheck = false;

	public GameObject cam;

	private void Awake()
	{
		controller = this.gameObject.GetComponent<CharacterController2D>();
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.Mouse0) && canAttack)
		{
			canAttack = false;
			animator.SetBool("IsAttacking", true);
			StartCoroutine(AttackCooldown());
		}
		if (Input.GetKeyDown(KeyCode.Mouse1) && canThrow)
		{
			canThrow = false;
			ThrowWeapon();
			StartCoroutine(ThrowCooldown());
		}

		//if (Input.GetKeyDown(KeyCode.Mouse1))
		//{
		//	GameObject throwableWeapon = Instantiate(throwableObject, transform.position + new Vector3(transform.localScale.x * 0.5f,-0.2f), Quaternion.identity) as GameObject; 
		//	Vector2 direction = new Vector2(transform.localScale.x, 0);
		//	throwableWeapon.GetComponent<ThrowableWeapon>().direction = direction; 
		//	throwableWeapon.name = "ThrowableWeapon";
		//}
	}

	IEnumerator AttackCooldown()
	{
		yield return new WaitForSeconds(0.25f);
		canAttack = true;
	}

	IEnumerator ThrowCooldown()
	{
		yield return new WaitForSeconds(1f);
		canThrow = true;
	}

	public void DoDashDamage()
	{
		dmgValue = Mathf.Abs(dmgValue);
		Collider2D[] collidersEnemies = Physics2D.OverlapCircleAll(attackCheck.position, 0.9f);
		for (int i = 0; i < collidersEnemies.Length; i++)
		{
			if (collidersEnemies[i].gameObject.tag == "Enemy")
			{
				if (collidersEnemies[i].transform.position.x - transform.position.x < 0)
				{
					dmgValue = -dmgValue;
				}
				collidersEnemies[i].gameObject.SendMessage("ApplyDamage", dmgValue);
				cam.GetComponent<CameraFollow>().ShakeCamera();
			}
		}
	}

	public void ThrowWeapon()
    {
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePosition.z = 0f;
		Vector2 direction = (mousePosition - transform.position).normalized;
		if (controller.m_FacingRight && direction.x < 0)
        {
			controller.Flip();
        }
		else if (!controller.m_FacingRight && direction.x > 0)
        {
			controller.Flip();
        }
		GameObject throwableWeapon = Instantiate(throwableObject, transform.position + new Vector3(transform.localScale.x * 0.5f, -0.2f), Quaternion.identity) as GameObject;
		
		throwableWeapon.GetComponent<ThrowableWeapon>().direction = direction;
		throwableWeapon.name = "ThrowableWeapon";
	}
	
}
