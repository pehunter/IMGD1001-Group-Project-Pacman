using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Important objects
    public Ghost[] ghosts;
    public Pacman pacman;
    public Transform pellets;

    //UI
    public TextMeshProUGUI scoreDisplay;
    public TextMeshProUGUI highScoreDisplay;
    public GameObject lifeContainer;
    public GameObject bombContainer;
    public GameObject lifeIcon;
    public GameObject bombIcon;
    public GameObject incomingScore;

    private GameObject incScore;
    public int score { get; private set; }
    public int highScore { get; private set; }
    public int lives { get; private set; }

    List<PelletBomb> bombs;
    List<GameObject> explosions;
    private bool gameNotEnding = true;

    //Multiplier that increases for every ghost ate
    public int ghostMultiplier { get; private set; } = 1;

    //Runs when the game first loads
    public void Start()
    {
        bombs = new List<PelletBomb>();
        explosions = new List<GameObject>();

        SetHighScore(0);

        //Start a new game on load
        NewGame();
    }

    public void Update()
    {
        //If out of lives and input is received, start a new game.
        if(this.lives <= 0 && gameNotEnding && Input.anyKeyDown)
        {
            NewGame();
        }
    }

    //Initializes a new game
    private void NewGame()
    {
        //Set score and lives to their default values
        SetScore(0);
        SetLives(3);

        //Start the first round
        NewRound();
    }

    //Reset pellets, as well as pacman and ghosts
    private void NewRound()
    {
        //Reactivate all pellets
        foreach(Transform pellet in this.pellets) {
            pellet.gameObject.SetActive(true);
        }

        //Reset state of pacman and ghosts
        ResetState();
    }

    //Reset ghosts & pacman to how they are at round start
    private void ResetState()
    {
        pacman.ResetState();

        for (int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i].ResetState();
        }

        //Remove all bombs and explosions
        foreach (PelletBomb bomb in bombs)
            if (bomb != null && bomb.gameObject != null)
                Destroy(bomb.gameObject);

        foreach (GameObject explo in explosions)
            if (explo != null)
                Destroy(explo.gameObject);

        UnfreezeAll();

    }

    public void AddBomb(PelletBomb bomb)
    {
        bombs.Add(bomb);
    }

    public void AddExplosion(GameObject explo)
    {
        explosions.Add(explo);
    }

    private void GameOver()
    {
        //gameOverText.enabled = true;

        for (int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i].gameObject.SetActive(false);
        }

        pacman.gameObject.SetActive(false);

        pacman.AddBomb(pacman.bombs * -1);

        if (score > highScore)
            SetHighScore(score);
        gameNotEnding = true;
    }

    //Set score
    private void SetScore(int newScore)
    {
        this.score = newScore;
        this.scoreDisplay.text = newScore.ToString();
    }

    //Set score
    private void SetHighScore(int newScore)
    {
        this.highScore = newScore;
        this.highScoreDisplay.text = newScore.ToString();
    }

    //Set lives
    private void SetLives(int newLives)
    {
        if (!pacman.frozen)
        {
            this.lives = newLives;
            Debug.Log(newLives);
            //Add/remove lives from UI until it maches
            int lifeElapsed = lifeContainer.transform.childCount;

            //Add
            while (lifeElapsed < newLives)
            {
                var newLife = Instantiate(lifeIcon);
                newLife.transform.SetParent(lifeContainer.transform, false);
                newLife.GetComponent<RectTransform>().anchoredPosition = new Vector2(newLife.GetComponent<RectTransform>().rect.width * lifeElapsed, 0);
                //newLife.GetComponent<RectTransform>().localScale = new Vector3(-1, 1, 1);
                lifeElapsed++;
            }

            //Remove
            while (lifeElapsed > newLives)
            {
                lifeElapsed--;
                Destroy(lifeContainer.transform.GetChild(lifeElapsed).gameObject);
            }

        }
    }

    //Set bomb count
    public void SetBombs(int newBombs)
    {
        //Add/remove bombs from UI until it maches
        int bombsDrawn = bombContainer.transform.childCount;

        //Add
        while (bombsDrawn < newBombs)
        {
            var newBomb = Instantiate(bombIcon);
            newBomb.transform.SetParent(bombContainer.transform, false);
            newBomb.GetComponent<RectTransform>().anchoredPosition = new Vector2(newBomb.GetComponent<RectTransform>().rect.width * bombsDrawn, 0);
            //newLife.GetComponent<RectTransform>().localScale = new Vector3(-1, 1, 1);
            bombsDrawn++;
        }

        //Remove
        while (bombsDrawn > newBombs)
        {
            Destroy(bombContainer.transform.GetChild(bombsDrawn - 1).gameObject);
            bombsDrawn--;
            }
    }


    //Remove active incoming score
    public void RemoveIncScore()
    {
        if (incScore != null)
            Destroy(incScore);
    }

    //Handle Pacman eating a ghost
    public void GhostEaten(Ghost ghost)
    {
        //Calculate points based on ghost multiplier
        int points = ghost.points * ghostMultiplier;

        //Insert an incoming score at ghost location
        if (incScore != null)
            Destroy(incScore);
        incScore = Instantiate(incomingScore);
        incScore.GetComponent<TextMeshPro>().text = points.ToString();
        incScore.transform.position = new Vector3(ghost.transform.position.x, ghost.transform.position.y, -6);
        Invoke(nameof(RemoveIncScore), 1.0f);

        SetScore(score + points);

        ghostMultiplier++;

        //Eat ghost
        ghost.swapBehavior(typeof(GhostDead), ghost.respawnTime);
    }

    //Freeze everything
    public void FreezeAll()
    {
        for (int i = 0; i < ghosts.Length; i++)
            ghosts[i].Freeze();

        foreach (var bomb in bombs)
            bomb.Freeze();

        pacman.Freeze();
    }

    //Unfreeze everything
    public void UnfreezeAll()
    {
        for (int i = 0; i < ghosts.Length; i++)
            ghosts[i].Unfreeze();

        foreach (var bomb in bombs)
            bomb.Unfreeze();

        pacman.Unfreeze();
    }

    public void PacmanEaten() {
        //pacman.gameObject.SetActive(false);
        if (pacman.frozen)
            return;

        SetLives(lives - 1);

        foreach(Ghost g in ghosts)
            if (g.gameObject.GetComponent<GhostBomb>() != null)
                g.gameObject.GetComponent<GhostBomb>().PreventExplosion();

        FreezeAll();
        pacman.Die();

        if(this.lives > 0)
        {
            Invoke(nameof(ResetState), 4f);
        } else
        {
            gameNotEnding = false;
            Invoke(nameof(GameOver), 4f);
        }
    }

    //Handle score & game ending when pellet is increased
    public void PelletEaten(Pellet pellet)
    {
        //Remove pellet from game and add to score
        pellet.gameObject.SetActive(false);
        SetScore(score + pellet.points);

        //If the gameboard is empty, disable pacman and start new round.
        if (!HasRemainingPellets())
        {
            //Freeze everything
            FreezeAll();

            //Start new round
            Invoke(nameof(NewRound), 4f);
        }
    }

    public void GMPInvoke(float duration)
    {
        //Start or reset frightened state timer.
        CancelInvoke(nameof(ResetGhostMultiplier));
        Invoke(nameof(ResetGhostMultiplier), duration);
    }

    //Handle consuming a power pellet
    public void PowerPelletEaten(PowerPellet pellet)
    {
        for (int i = 0; i < ghosts.Length; i++)
        {
            //Frightenen ghost if out of home state
            if (ghosts[i].GetComponent<GhostBehavior>().frightenable)
                ghosts[i].swapBehavior(typeof(GhostFrightened), pellet.duration);
        }

        //Eat power pellet as if it were a normal pellet
        PelletEaten(pellet);

        GMPInvoke(pellet.duration);
    }

    //Handle consuming a bomb pellet
    public void BombPelletEaten(PelletBombGiver pellet)
    {
        //Add bombs to Pacman
        pacman.AddBomb(pellet.bombs);

        //Eat bomb pellet as if it were a normal pellet
        PelletEaten(pellet);
    }

    //Return true if there are still some active pellets.
    private bool HasRemainingPellets()
    {
        int idx = -1;
        bool pelletActive = false;
        
        //When an active pellet is reached, loop will stop.
        while (++idx < pellets.childCount && !pelletActive)
            pelletActive |= pellets.GetChild(idx).gameObject.activeSelf;

        return pelletActive;
    }

    //Reset the ghost multiplier back to its default value.
    private void ResetGhostMultiplier()
    {
        ghostMultiplier = 1;
    }
}
