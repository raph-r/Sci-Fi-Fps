using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelManager : MonoBehaviour
{
    private int _killCounter;
    private int _amountOfEnemies;
    private GameState _gameState;
    [SerializeField] private GameObject _playerController;
    [SerializeField] private GameObject _canvas;
    // Start is called before the first frame update
    void Start()
    {
        this._gameState = GameState.Play;
        this._amountOfEnemies = GameObject.FindGameObjectsWithTag(Constant.TAG_ENEMY).Length;
        this._killCounter = 0;
        GUIController gameUserInterface = this._canvas.GetComponent<GUIController>();
        gameUserInterface.setKills(this._killCounter);
        gameUserInterface.setHealthPoints(this._playerController.GetComponent<PlayerController>().getHealthPoints());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Verifica se o jogador ganhou
    private void tryUpdateGameStateToWin()
    {
        if (this._killCounter == this._amountOfEnemies)
        {
            this._gameState = GameState.Win;
            this.updateGameState();
        }
    }
    // Verifica se o jogador perdeu
    private void tryUpdateGameStateToGameOver(int playerCurrentHealthPoints)
    {
        if (playerCurrentHealthPoints <= 0)
        {
            this._gameState = GameState.GameOver;
            this.updateGameState();
        }
    }

    // Aplica dano ao jogador e dispara a verificação de condição de derrota. Também atualiza o GameUserInterface
    public void applyDamageOnPlayer(int damage)
    {
        PlayerController playerController = this._playerController.GetComponent<PlayerController>();
        playerController.takeDamage(damage);
        int playerCurrentHealthPoints = playerController.getHealthPoints();
        this._canvas.GetComponent<GUIController>().setHealthPoints(playerCurrentHealthPoints);
        this.tryUpdateGameStateToGameOver(playerCurrentHealthPoints);
    }

    // Adiciona um Kill ao jogador e dispara a verificação da condição de vitoria. Também atualiza o GameUserInterface
    public void addKill()
    {
        this._killCounter++;
        this._canvas.GetComponent<GUIController>().setKills(this._killCounter);
        this.tryUpdateGameStateToWin();
    }

    // Atualiza o GameStatus do jogo. Também atualiza o GameUserInterface com o resultado do jogo
    private void updateGameState()
    {
        switch (this._gameState)
        {
            case GameState.Play:
                break;
            case GameState.Win:
                InputSystem.DisableAllEnabledActions();
                this._canvas.GetComponent<GUIController>().setEndGameMsg(Constant.MSG_WIN);
                break;
            case GameState.GameOver:
                InputSystem.DisableAllEnabledActions();
                Destroy(GameObject.FindGameObjectWithTag(Constant.TAG_PLAYER_GAME_OBJ_COLLIDER));
                this._canvas.GetComponent<GUIController>().setEndGameMsg(Constant.MSG_GAME_OVER);
                break;
            default:
                break;
        }
    }

    // Atualiza o status do componente reponsavel por mostrar a mira
    public void showAim(bool status)
    {
        this._canvas.GetComponent<GUIController>().showAim(status);
    }
}
