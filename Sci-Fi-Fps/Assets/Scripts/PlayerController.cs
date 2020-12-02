using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float _movementSpeed = 20.0f;
    [SerializeField][Range(0, 1)] private float _aimingWeightWhileMovement = 0.5f;
    [SerializeField] private int _healthPoints;
    [SerializeField] private int _shootLatency;
    [SerializeField] private int _shootDamage;
    [SerializeField] private int _shootRange;
    [SerializeField] private float _aimSensibility;
    [SerializeField] GameObject _aim;
    [SerializeField] GameObject _camera;
    private float _lastShootCounter;
    private RobotController _robotController;
    private Vector3 _movementInputState;
    private float _currentAimWeight;
    private LevelManager _levelManager;
    private bool _isShooting;
    private bool _isAiming;

    // Start is called before the first frame update
    void Start()
    {
        this._isShooting = false;
        this._isAiming = false;
        this._lastShootCounter = this._shootLatency;
        this._currentAimWeight = 1.0f;
        this._levelManager = GameObject.FindGameObjectWithTag(Constant.TAG_LEVEL_MANAGER).GetComponent<LevelManager>();
        this._robotController = this.GetComponentInChildren<RobotController>();
        this._movementInputState = new Vector3();
    }

    // Update is called once per frame
    void Update()
    {
        this.updateShootCounter();
        // Efetua o disparo se for requisitado respeitando o tempo minimo para o proximo tiro
        if (this._isShooting && this._lastShootCounter >= this._shootLatency)
        {
            this.shoot();
        }
        // Aplica a movimentação ao jogador
        this.transform.Translate(this._movementInputState * ((this._movementSpeed * this._currentAimWeight) * Time.deltaTime));
    }

    // Se o tempo minimo para poder efetuar um tiro, não foi alcançado, adiciona o Time.deltatime ao contador
    private void updateShootCounter()
    {
        if (this._lastShootCounter < this._shootLatency)
        {
            this._lastShootCounter += Time.deltaTime;
        }
    }

    // Método chamado pelo InputSystem para movimentar o jogador
    public void setMovementInputStateXZ(InputAction.CallbackContext context)
    {
        Vector2 newInputStateXZ = context.ReadValue<Vector2>();
        this._movementInputState.x = newInputStateXZ.x;
        this._movementInputState.z = newInputStateXZ.y;
        this._robotController.isMoving((newInputStateXZ.x != 0.0f || newInputStateXZ.y != 0.0f));
        
    }

    // Método chamado pelo InputSystem para identificar se esta sendo solicitado do uso da "mira"
    public void setAim(InputAction.CallbackContext context)
    {
        bool aimNewState = Convert.ToBoolean(context.ReadValue<float>());
        this._currentAimWeight = (aimNewState) ? this._aimingWeightWhileMovement : 1.0f;
        this._robotController.isAiming(aimNewState);
        if (this._isAiming != aimNewState && this._isShooting == false)
        {
            this._camera.GetComponent<CameraController>().changeCameraPosition(aimNewState);
            this._levelManager.showAim(aimNewState);
        }
        this._isAiming = aimNewState;
    }

    // Método chamado pelo InputSystem para identificar se esta sendo solicitado a criação de um "tiro"
    public void setShootState(InputAction.CallbackContext context)
    {
        bool shootNewState = Convert.ToBoolean(context.ReadValue<float>());
        this._currentAimWeight = (shootNewState) ? this._aimingWeightWhileMovement : 1.0f;
        this._robotController.isShooting(shootNewState);
        if (this._isShooting != shootNewState && this._isAiming == false)
        {
            this._camera.GetComponent<CameraController>().changeCameraPosition(shootNewState);
            this._levelManager.showAim(shootNewState);
        }
        this._isShooting = shootNewState;
        
    }

    // Método chamado pelo InputSystem para atualizar a rotação no eixo Y do personagem, atraves do movimento do mouse
    public void setPlayerRotationY(InputAction.CallbackContext context)
    {
        this.transform.Rotate(0.0f, context.ReadValue<Vector2>().x * this._aimSensibility, 0.0f);
    }

    // aplica dano ao jogador.
    public void takeDamage(int damage)
    {
        this._healthPoints -= damage;
        if (this._healthPoints <= 0 )
        {
            this._healthPoints = 0;
        }
    }

    // Efetua um "tiro", caso o inimigo seja acertado, solicita a aplicação de dano a ele. Zera o contador de segundos do ultimo tiro efetuado
    private void shoot()
    {
        RaycastHit raycastHit;
        Debug.DrawRay(this._aim.transform.position, this._aim.transform.forward * this._shootRange, Color.white, 0.5f);
        if (Physics.Raycast(this._aim.transform.position, this._aim.transform.forward, out raycastHit, this._shootRange))
        {
            if (raycastHit.collider.gameObject.tag == Constant.TAG_ENEMY)
            {
                raycastHit.collider.gameObject.GetComponentInParent<EnemyController>().takeDamage(this._shootDamage);
            }
            
        }
        this._lastShootCounter = 0.0f;
    }

    // Captura o vida atual do jogador
    public int getHealthPoints()
    {
        return this._healthPoints;
    }
}
