using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int shootDamage = 10;
    public int healthPoints = 50;
    public float shootLatency = 2.0f;
    public int shootRange = 30;
    public int detectionRange = 10;
    public float movementSpeedPerSecond = 1.0f;
    private float _lastShootCounter = 0.0f;
    [SerializeField]private GameObject _aim;
    private LevelManager _levelManager;
    private GameObject _robot;

    // Start is called before the first frame update
    void Start()
    {
        // Corrige a altura do inimigo
        if (this.transform.position.y != Constant.positionYForEnemy)
        {
            Vector3 newPosition = this.transform.position;
            newPosition.y = Constant.positionYForEnemy;
            this.transform.position = newPosition;
        }
        this._levelManager = GameObject.FindGameObjectWithTag(Constant.TAG_LEVEL_MANAGER).GetComponent<LevelManager>();
        this._robot = GameObject.FindGameObjectWithTag(Constant.TAG_PLAYER_GAME_OBJ_COLLIDER);
        this._lastShootCounter = this.shootLatency;
    }

    // Update is called once per frame
    void Update()
    {
        if (this._robot != null)
        {
            this.updateShootCounter();
            if (this.isPlayerDetectionRange())
            {
                this.transform.LookAt(this._robot.transform.position);
                if (this.isPlayerShootRange())
                {
                    if (this._lastShootCounter >= this.shootLatency)
                    {
                        this.shoot();
                    }
                }
                else
                {
                    this.moveTowardPlayer();        
                }
            }
        }
    }

    // move o inimigo em direção ao jogador
    private void moveTowardPlayer()
    {
        Vector3 newPosition = Vector3.Lerp(this.transform.position, this._robot.transform.position, (this.movementSpeedPerSecond * Time.deltaTime));
        newPosition.y = this.transform.position.y;
        this.transform.position = newPosition;
    }

    // Se o tempo minimo para poder efetuar um tiro, não foi alcançado, adiciona o Time.deltatime ao contador
    private void updateShootCounter()
    {
        if (this._lastShootCounter < this.shootLatency)
        {
            this._lastShootCounter += Time.deltaTime;
        }
    }
    // verifica se o jogador esta no alcance de detecção
    private bool isPlayerDetectionRange()
    {
        return Vector3.Distance(this.transform.position, this._robot.transform.position) <= this.detectionRange;
    }

    // Verifica se o jogador esta no alcançe do tiro
    private bool isPlayerShootRange()
    {
        return Vector3.Distance(this.transform.position, this._robot.transform.position) <= this.shootRange;
    }

    // Efetua um "tiro" no jogador, caso o jogador seja acertado, solicita a aplicação de dano a ele. Zera o contador de segundos do ultimo tiro efetuado
    private void shoot()
    {
        RaycastHit raycastHit;
        Debug.DrawRay(this._aim.transform.position, this._aim.transform.forward * this.shootRange, Color.red, 0.5f);
        if (Physics.Raycast(this._aim.transform.position, this._aim.transform.forward, out raycastHit, this.shootRange))
        {
            if (raycastHit.collider.gameObject.name == Constant.TAG_PLAYER_GAME_OBJ_COLLIDER)
            {
                this._levelManager.applyDamageOnPlayer(this.shootDamage);
            }
        }
        this._lastShootCounter = 0.0f;
    }

    // aplica dano ao inimigo. Caso ele morra, adiciona uma morte ao contador no LevelManager e se destri
    public void takeDamage(int damage)
    {
        this.healthPoints -= damage;
        if (this.healthPoints <= 0)
        {
            this._levelManager.addKill();
            Destroy(this.gameObject);
        }
    }   
}
