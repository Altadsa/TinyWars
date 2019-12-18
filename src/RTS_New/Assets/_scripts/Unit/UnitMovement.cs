using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
    private NavMeshAgent _agent;
    private Animator _animator;
    
    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MoveToMouse();
        }

        if (Input.GetMouseButtonDown(1))
        {
            _agent.ResetPath();
            _animator.SetTrigger("action");
        }
        _animator.SetBool("moving", _agent.hasPath);
    }

    private void MoveToMouse()
    {
        var mousepos = Input.mousePosition;
        var ray = Camera.main.ScreenPointToRay(mousepos);
        RaycastHit hit;
        bool hashit = Physics.Raycast(ray, out hit);
        if (hashit)
        {
            _agent.SetDestination(hit.point);
        }
    }
}
