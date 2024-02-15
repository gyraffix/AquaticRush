using System;                                   // System contains a lot of default C# libraries 
using GXPEngine;                                // GXPEngine contains the engine
using System.Drawing;							// System.Drawing contains drawing tools such as Color definitions
using System.Media;
using System.Drawing.Text;
using System.Collections.Generic;
using System.Collections;
using System.Threading;


public class MyGame : Game {

    private Random rnd = new Random();

    private Sprite background;
	private Sprite background1;
	private float backgroundSpeed = 1f;

	private AnimationSprite beach;
	private Player player;

	private Sprite enemyPlace;
	private List<Enemy> enemies = new List<Enemy>();
	private bool spawnEnemy = false;
	private float enemyCooldown = 1.5f;

    private List<int> toDestroy = new List<int>();

	private EasyDraw startScreen;
	private EasyDraw gameOverScreen;

	public float difficulty = 1;
	public int score { get; set; } = 0;
	private int finalScore;

	public EasyDraw UI;

    private bool restart;
    public bool gameOver = true;
	private bool playerDestroyed = true;

    public MyGame() : base(1366, 768, false)
	{
		targetFps = 60;
 
        //TODO: Better Start menu.

        //TODO: implement sprites and animations (requires Sprites)

        //TODO: implement powerups (Maybe? discuss powerups)

        //TODO: Collectibles? (Something like coins maybe)

        background1 = new Sprite("background1.png", false, false);
        background1.SetXY(0, -height);
        background = new Sprite("background.png", false, false);
        background.SetXY(0, -height);
		AddChild(background1);
        AddChild(background);

        startScreen = new EasyDraw(width, height);
		AddChild(startScreen);
		startScreen.TextFont("minecraft.ttf", 40);
		startScreen.Text("Press Space to Start", width/3.3f, height/2);

        gameOverScreen = new EasyDraw(width, height);
        gameOverScreen.TextFont("minecraft.ttf", 40);
		gameOverScreen.TextAlign(CenterMode.Center, CenterMode.Center);


        beach = new AnimationSprite("beach.png", 4, 2, -1, false, false);
		beach.SetCycle(0,8,12);
		AddChild(beach);

        player = new Player("triangle.png", 1, 1);
        AddChild(player);
		
    }

    // For every game object, Update is called every frame, by the engine:
    void Update() 
	{
		beach.Animate();
        MoveBackground();

        if (!gameOver)
		{
            
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
        background1.SetXY(background1.x, ((int)background1.y));
        background.SetXY(background.x, ((int)background.y));
        if (!restart) startScreen.Destroy();

		else
		{
			RemoveChild(gameOverScreen);
			
			background.SetXY(0, -height);
            background1.SetXY(0, -height);

			AddChild(background1);
            AddChild(background);
			AddChild(beach);
			beach.SetXY(0, 0);
            player = new Player("triangle.png", 1, 1);
        }
		
		enemyPlace = new Sprite("square.png", false, false);
		enemyPlace.alpha = 0;
		AddChild(enemyPlace);


        UI = new EasyDraw(width, 200, false);
        if(restart) AddChild(player);

        AddChild(new Coroutine(enemyLoop()));
		AddChild(new Coroutine(difficultyLoop()));

        AddChild(UI);
        UI.TextFont(Utils.LoadFont("minecraft.ttf", 24));
        UI.Fill(255, 255, 255);
		
		gameOver = false;
        playerDestroyed = false;
		player.start = true;
    }

	private void GameOver()
	{
		gameOverScreen.ClearTransparent();
		restart = true;
		player.LateDestroy();
		playerDestroyed = true;
		RemoveChild(player);
        foreach (GameObject obj in GetChildren())
        {
			RemoveChild(obj);
        }
		finalScore = score;
        Console.WriteLine(finalScore);
        score = 0;
		difficulty = 1;
		AddChild(gameOverScreen);
        gameOverScreen.Text("You Lost" + "\n" + "Score:" + finalScore + "\nPress Space to try again", width / 2f, height / 2);
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
		if (!gameOver)
		{
			beach.Translate(0, backgroundSpeed);
			background.Translate(0, backgroundSpeed);
			background1.Translate(0, backgroundSpeed);
		}
		else
		{
			background1.Translate(0, backgroundSpeed / 5);
            background.Translate(0, backgroundSpeed / 5);
        }

		if (background.y == 0)
		{
			background.SetXY(0, -height);
            background1.SetXY(0, -height);

        }
        if (beach.y > height) RemoveChild(beach);
		background.alpha = Math.Max(1 - (difficulty - 1) / 2, 0);
        background1.alpha = Math.Min(0 + (difficulty - 1) / 2, 1);
    }

	IEnumerator enemyLoop()
	{
		while (!gameOver)
		{

				Enemy newEnemy = new Enemy("square.png", 1, 1, rnd.Next(width));
				Console.WriteLine("enemy created");

				enemies.Add(newEnemy);
				enemyPlace.AddChild(newEnemy);

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