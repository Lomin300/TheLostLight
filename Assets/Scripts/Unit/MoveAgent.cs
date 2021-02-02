using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NavMeshAgent2D))]
public class MoveAgent : Bolt.EntityBehaviour<ICustomUnitState>
{
    public NavMeshAgent2D agent;

    public Transform wayPoint;

    private float traceSpeed = 2.0f;

    private bool _patrolling;

    private UnitAI m_unitAI;

    
    public bool patrolling
    {
        get { return _patrolling; }
        set
        {
            _patrolling = value;
            if (_patrolling)
            {
                agent.speed = traceSpeed;
                MoveWayPoint();
            }
        }
    }

    private Vector2 _traceTarget;

    public Vector2 traceTarget
    {
        get { return _traceTarget; }
        set
        {
            
            _traceTarget = value;
            agent.speed = traceSpeed;
            TraceTarget(traceTarget);
        }
    }

    private void Start()
    {
       
    }

    public override void Attached()
    {
        if (!entity.IsOwner)
        {
            this.enabled = false;
            return;
        }
        else
        {
            if (BoltNetwork.IsServer)
            {
                if (gameObject.tag == "Hero")
                    wayPoint = GameObject.Find("WarPoint").transform;
                else
                    wayPoint = GameObject.Find("ClientHero").transform;
            }
            else
            {
                if (gameObject.tag=="Hero")
                    wayPoint = GameObject.Find("WarPoint").transform;
                else
                    wayPoint = GameObject.Find("ServerHero").transform;
            }

            agent.autoBraking = false;
            agent.speed = traceSpeed;
            patrolling = true;
        }
    }
  

    void TraceTarget(Vector2 pos)
    {
        if (agent.isPathStale) return;

        agent.destination = pos;
        agent.isStopped = false;
    }

    void MoveWayPoint()
    {
        if (agent.isPathStale) return;
        agent.destination = wayPoint.position;
        agent.isStopped = false;
    }

    public void Stop()
    {
        agent.isStopped = true;
        agent.velocity = Vector2.zero;
        
        patrolling = false;
    }

}
