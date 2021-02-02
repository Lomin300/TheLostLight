using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveMent : Bolt.EntityBehaviour<ICustomUnitState>
{

    bool isSecend;
    int ClientID;
    Vector3 tr;
    public Animator playerAnimator;
    public NavMeshAgent2D agent;
    private Transform goal;
    public override void Attached()
    {
        state.SetTransforms(state.UnitTransform, gameObject.transform);

        if (BoltNetwork.IsServer && !entity.IsOwner)
        {
            Vector3 scale = transform.localScale;

            scale.x *= -1;

            transform.localScale = scale;
        }
        state.SetAnimator(playerAnimator);
    }

    public void Start()
    {
        if (BoltNetwork.IsServer && entity.IsOwner)
            agent.SetDestination(new Vector2(7f, 0f));
        else if (BoltNetwork.IsClient && entity.IsOwner)
            agent.SetDestination(new Vector2(-7f, 0));
    }

    public override void SimulateOwner()
    {

        if (agent.remainingDistance <= 0.01f)
        {
            state.IsMove = false;
            
        }
        else
        {
            state.IsMove = true;
            
        }
      
    }

    public void Update()
    {
        if (state.IsMove)
        {
            state.Animator.Play("Burserker_Walk");
            agent.isStopped = false;
        }
        else
        {
            state.Animator.Play("Burserker_Idle");
            agent.isStopped = true;
        }
    }

}
