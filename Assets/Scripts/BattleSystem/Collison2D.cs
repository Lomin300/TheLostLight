using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collison2D : MonoBehaviour
{
    Vector2 pos;
    Vector2 Colliedpos;
    Rigidbody2D rigid;
    UnitState stat;

    void OnTriggerEnter2D(Collider2D other)
    {
        pos = transform.position;
        rigid = GetComponent<Rigidbody2D>();
        stat = GetComponent<UnitState>();
    }

    void OnTriggerStay2D(Collider2D other)
    //rigidBody가 무언가와 충돌할때 호출되는 함수 입니다.
    //Collider2D other로 부딪힌 객체를 받아옵니다.
    {
        Debug.Log("충돌");
        if (other.gameObject.tag.Equals("Enemy"))
        //부딪힌 객체의 태그를 비교해서 적인지 판단합니다.
        {
            Colliedpos = other.transform.position;
            Colliedpos = Colliedpos - pos;
            Colliedpos.Normalize();
            Colliedpos = Colliedpos * stat.MoveSpeed * Time.deltaTime;
            pos = pos + Colliedpos;
            rigid.MovePosition(pos);
        }
    }

}
