using System;                                   // System contains a lot of default C# libraries 
using GXPEngine;                                // GXPEngine contains the engine
using System.Drawing;
using System.Media;                           // System.Drawing contains drawing tools such as Color definitions

public class MyGame : Game {

	private Player player;
	public MyGame() : base(800, 600, false)     // Create a window that's 800x600 and NOT fullscreen
	{
		//Canvas canvas = new Canvas(800,600);

		
		player = new Player("triangle.png", 1, 1); 
		AddChild(player);

	}

	// For every game object, Update is called every frame, by the engine:
	void Update() {
		player.Update();
	}

	static void Main()                          // Main() is the first method that's called when the program is run
	{
		new MyGame().Start();                   // Create a "MyGame" and start it
	}
}