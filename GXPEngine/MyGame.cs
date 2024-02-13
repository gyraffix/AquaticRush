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

	private Player player;
	private List<Enemy> enemies = new List<Enemy>();
	private Random rnd = new Random();
	private bool spawnEnemy = false;
	private float enemyCooldown = 1.5f;
	private bool timerStarted = false;
    private List<int> toDestroy = new List<int>();

    public MyGame() : base(800, 600, false)     // Create a window that's 800x600 and NOT fullscreen
	{
		//Canvas canvas = new Canvas(800,600);

		
		player = new Player("triangle.png", 1, 1); 
		AddChild(player);

		AddChild(new Coroutine(enemyLoop()));
		

	}

	// For every game object, Update is called every frame, by the engine:
	void Update() 
	{
		player.Update();
		foreach(Enemy enemy in enemies)
		{
			enemy.Update();

			if (enemy.y > 620 || enemy.flagged)
			{
				toDestroy.Add(enemies.IndexOf(enemy));
				
			}
		}
        foreach (int index in toDestroy)
        {
            enemies[index].LateDestroy();

        }
		toDestroy.Clear();

    }

	static void Main()                          // Main() is the first method that's called when the program is run
	{
		new MyGame().Start();                   // Create a "MyGame" and start it
	}


	IEnumerator enemyLoop()
	{
		while (true)
		{

				Enemy newEnemy = new Enemy("square.png", 1, 1, rnd.Next(800));
				Console.WriteLine("enemy created");

				enemies.Add(newEnemy);
				AddChild(newEnemy);

			yield return new WaitForSeconds(enemyCooldown);

			
		}
    }
}