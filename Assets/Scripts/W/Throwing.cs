using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwing : Bolt.EntityBehaviour<ICustomThrowingState>
{

    private GameObject target;
    private GameObject owner;
    private float speed;
    private int index=0;
   
    public override void Attached()
    {
        state.SetTransforms(state.Transform, gameObject.transform);
    }


    public override void SimulateOwner()
    {
        if (index == 1)
        {
            if(target==null || owner == null)
            {
                BoltNetwork.Destroy(this.gameObject);
            }

            Vector2 vec=target.transform.position - owner.transform.position;
            vec = Vector3.Normalize(vec);
            transform.Translate(vec*0.05f);
        }
            
    }

    public void TakeThrowing(float _speed, GameObject _target,GameObject _owner)
    {
        index = 1;
        target = _target;
        owner = _owner;
        speed = _speed;
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (entity.IsOwner)
        {
            if (coll.transform == target.transform)
            {
                owner.GetComponent<UnitSpecialSkill>().Dryad(coll.gameObject);
                BoltNetwork.Destroy(this.gameObject);
            }
        }
    }
}
