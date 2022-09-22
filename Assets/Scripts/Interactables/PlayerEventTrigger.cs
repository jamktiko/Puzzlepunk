using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class PlayerEventTrigger : MonoBehaviour
{
    public string CheckTag = "";
    public bool TriggerOnce = false;
        public UnityEvent onStay;
        public UnityEvent onEnter;
        public UnityEvent onExit;
        public virtual void OnEnable()
        {
            has_triggered = false;
        }

        protected bool has_triggered = false;
        protected virtual bool OnEnter()
        {
            if (TriggerOnce && has_triggered)
                return false;
            Debug.Log("Player triggered " + name);
            onEnter.Invoke();
            has_triggered = true;
            return true;
        }
        protected virtual bool OnExit()
        {
            if (TriggerOnce && has_triggered)
                return false;
            Debug.Log("Player triggered " + name);
            onExit.Invoke();
            has_triggered = true;
            return true;
        }
        protected virtual bool OnStay()
        {
            if (TriggerOnce && has_triggered)
                return false;
            Debug.Log("Player triggered " + name);
            onStay.Invoke();
            has_triggered = true;
            return true;
        }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (CheckTag == "" || other.gameObject.CompareTag( CheckTag))
        {
            OnEnter();
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (CheckTag == "" || other.gameObject.CompareTag(CheckTag))
        {
            OnExit();
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (CheckTag == "" || other.gameObject.CompareTag(CheckTag))
        {
            OnStay();
        }
    }
    
}
