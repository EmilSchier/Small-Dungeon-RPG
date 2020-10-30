using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovementTest : MonoBehaviour
{
  public float Speed = 5f;
  Rigidbody2D body;
  // Start is called before the first frame update
  void Start()
  {
    body = GetComponent<Rigidbody2D>();
  }

  // Update is called once per frame
  private void Update()
  {
    var input = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
    Vector3 velocity = input.normalized * Speed;
    transform.position += velocity * Time.deltaTime;
  }
}
