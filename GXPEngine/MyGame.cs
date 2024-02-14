using System;                                   // System contains a lot of default C# libraries 
using GXPEngine;                                // GXPEngine contains the engine
using System.Drawing;
using System.Media;
using System.Drawing.Text;
using System.Collections.Generic;
using System.Collections;
using System.Threading;
// System.Drawing contains drawing tools such as Color definitions

public class MyGame : Game {

	private Sprite background;
	private float backgroundSpeed = 1f;

	private Player player;
	private List<Enemy> enemies = new List<Enemy>();
	private Random rnd = new Random();
	private bool spawnEnemy = false;
	private float enemyCooldown = 1.5f;
	private bool timerStarted = false;
    private List<int> toDestroy = new List<int>();
	public float difficulty = 1;
	public int score { get; set; } = 0;
	private int finalScore;
	public EasyDraw UI;

	public bool gameOver = true;
	private bool playerDestroyed = true;

    public MyGame() : base(1366, 768, false)
	{

		//TODO: implement Second background, immplement second fade.	

        //TODO: implement Start menu (discuss menu design)

        //TODO: implement sprites and animations (requires Sprites)

        //TODO: implement powerups (Maybe? discuss powerups)

        //TODO: implement jump move (maybe??)




    }

    // For every game object, Update is called every frame, by the engine:
    void Update() 
	{
		
		if (!gameOver)
		{
			MoveBackground();
			player.Update();
			foreach (Enemy enemy in enemies)
			{
				enemy.Update();

				if (enemy.y > height || enemy.flagged)
				{
					if (enemy.flagged)
					{
						changeScore(50);
					}
					toDestroy.Add(enemies.IndexOf(enemy));

				}
			}
			foreach (int index in toDestroy)
			{
				enemies[index].LateDestroy();
				enemies.RemoveAt(index);

			}
			toDestroy.Clear();
			UI.ClearTransparent();
			UI.Text("Score: " + score, 25, 40);
			UI.Text("Lives: " + Math.Floor(player.lives), width - 150, 40);

			if (player.lives < 1) gameOver = true;
		}
		else if (!playerDestroyed)
		GameOver();
		else
		{
			if (Input.GetKeyDown(Key.SPACE))
			{
				StartGame();
			}
		}
    }

	private void StartGame()
	{

		background = new Sprite("background.png", false, false);
		background.SetXY(0, -height);

		AddChild(background);
		
        player = new Player("triangle.png", 1, 1);

        UI = new EasyDraw(width, 200, false);
        AddChild(player);

        AddChild(new Coroutine(enemyLoop()));
		AddChild(new Coroutine(difficultyLoop()));

        player.SetColor(0.5f, 0.1f, 0.1f);

        AddChild(UI);
        UI.TextFont(Utils.LoadFont("minecraft.ttf", 24));
        UI.Fill(255, 255, 255);
		
		gameOver = false;
        playerDestroyed = false;
    }

	private void GameOver()
	{

			player.LateDestroy();
			playerDestroyed = true;
			RemoveChild(player);
            foreach (GameObject obj in GetChildren())
            {
                RemoveChild(obj);
            }
		finalScore = score;
		score = 0;
		difficulty = 1;
    }

	public void changeScore(int change)
	{
		score += change;
	}

	static void Main()                          // Main() is the first method that's called when the program is run
	{
		new MyGame().Start();                   // Create a "MyGame" and start it
	}
	private void MoveBackground()
	{
		background.Translate(0, backgroundSpeed);
		if (background.y == 0) background.SetXY(0, -height);

		background.alpha = Math.Max(1 - (difficulty - 1) / 2, 0);
	}

	IEnumerator enemyLoop()
	{
		while (!gameOver)
		{

				Enemy newEnemy = new Enemy("square.png", 1, 1, rnd.Next(width));
				Console.WriteLine("enemy created");

				enemies.Add(newEnemy);
				AddChild(newEnemy);

			yield return new WaitForSeconds(enemyCooldown / difficulty);

			
		}
    }

	IEnumerator difficultyLoop() 
	{
		while (difficulty < 3)
        {
			yield return new WaitForSeconds(0.1f);
            difficulty += 0.01f;
        }
    }
}