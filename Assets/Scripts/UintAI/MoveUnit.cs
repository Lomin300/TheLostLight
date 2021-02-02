using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUnit : MonoBehaviour
{
    UnitState state;
    Rigidbody2D rigid;


    // Start is called before the first frame update
    void Start()
    {
        state = GetComponent<UnitState>();
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 position = transform.position;

        position.x = position.x + state.MoveSpeed * Time.deltaTime;
        //position.y = position.y + state.MoveSpeed * Time.deltaTime;

        rigid.MovePosition(position);
    }
}
