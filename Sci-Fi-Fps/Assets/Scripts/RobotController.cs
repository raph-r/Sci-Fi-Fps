using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotController : MonoBehaviour
{
    private Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        this._animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Ativa|desativa a animação de mira
    public void isAiming(bool newState)
    {
        if (this._animator.GetBool("isAiming") != newState)
        {
            this._animator.SetBool("isAiming", newState);
        }
    }

    // Ativa|desativa a animação de movimento
    public void isMoving(bool newState)
    {
        if (this._animator.GetBool("isMoving") != newState)
        {
            this._animator.SetBool("isMoving", newState);
        }
    }
    
    // Ativa|desativa a animação de tiro
    public void isShooting(bool newState)
    {
        if (this._animator.GetBool("isShooting") != newState)
        {
            this._animator.SetBool("isShooting", newState);
        }
    }
}
