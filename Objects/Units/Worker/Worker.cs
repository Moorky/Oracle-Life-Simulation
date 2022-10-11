using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : MonoBehaviour
{
    // Base
    public Base myBase;

    // State Handling
    public string state = "base";
    public bool newState;

    // Detection
    public float sightRange = 5f;
    public float interactionRange = 1f;
    public LayerMask woodLayer;
    
    // Movement
    public float movementSpeed = 2f;
    public Vector2 basePos;
    public Vector2 newStatePos;
    public Vector3 targetPos;

    // Start is called before the first frame update
    void Start()
    {
        this.newState = true;
    }

    void FixedUpdate()
    {
        this.handleState();
    }

    void handleState()
    {
        if (this.newState) {
            this.initNewState();
        }

        switch (this.state)
        {
            case "idle":
                this.idle();
                break;
            case "base":
                this.goBase();
                break;
            case "explore":
                this.explore();
                break;
            case "wood":
                this.getWood();
                break;
            default:
                break;
        }
    }

    void initNewState()
    {
        this.basePos = this.myBase.transform.position;
        this.newStatePos = transform.position;
        this.targetPos = transform.position;
        this.newState = false;
    }

    public void setState(string state)
    {
        this.state = state;
        this.newState = true;
    }

    void idle()
    {
        if (!this.goToTargetPos()) {
            this.targetPos = this.newStatePos + Random.insideUnitCircle * 10;
        }
    }

    void goBase()
    {
        if (!this.goToTargetPos()) {
            this.targetPos = this.basePos + Random.insideUnitCircle * 10;
        }
    }

    void explore()
    {
        if (!this.goToTargetPos()) {
            this.targetPos = Random.insideUnitCircle * 10;
            this.targetPos += this.transform.position;
        }
    }

    bool goToTargetPos()
    {
        if (this.transform.position != this.targetPos)
        {
            this.walkTo(this.targetPos);

            return true;
        }

        return false;
    }

    void walkTo(Vector3 pos)
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, pos, this.movementSpeed * Time.deltaTime);
    }

    bool findObj(LayerMask obj)
    {
        Collider2D[] objsInRange = Physics2D.OverlapCircleAll(this.transform.position, this.sightRange, obj);

        if (objsInRange.Length > 0) {
            this.walkTo(this.calculateClosestCollider(objsInRange).position);
            return true;
        }

        return false;
    }

    Transform calculateClosestCollider(Collider2D[] objsInRange)
    {
        Transform nearestObj = null;

        foreach (Collider2D obj in objsInRange) {
            if (nearestObj == null) {
                nearestObj = obj.transform;

            } else {
                if (Vector3.Distance(this.transform.position, obj.transform.position) 
                    < Vector3.Distance(this.transform.position, nearestObj.position)) {
                        nearestObj = obj.transform;
                }
            }
        }

        return nearestObj;
    }

    bool interactObj(LayerMask obj)
    {
        Collider2D[] objsInRange = Physics2D.OverlapCircleAll(this.transform.position, this.interactionRange, obj);

        if (objsInRange.Length > 0) {
            // Do something
            return true;
        }

        return false;
    }

    void getWood()
    {
        if (!this.interactObj(this.woodLayer)) {
            if (!this.findObj(this.woodLayer)) {
                this.explore();
            }
        }
    }

    void OnDrawGizmosSelected() 
    {
        Gizmos.DrawWireSphere(this.transform.position, this.sightRange);
        Gizmos.DrawWireSphere(this.transform.position, this.interactionRange);
    }
}
