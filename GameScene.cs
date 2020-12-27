using Godot;
using System;

public class GameScene : Spatial
{
	protected Camera Camera => GetNode<Camera> (nameof (Camera));

	protected Spatial TestBoard => GetNode<Spatial> (nameof (TestBoard));

	protected BoardScene Board => GetNode<BoardScene> (nameof (Board));

	protected CardScene Card1 => TestBoard.GetNode<CardScene> (nameof (Card1));

	protected CardScene Card2 => TestBoard.GetNode<CardScene> (nameof (Card2));
	
	protected CardScene Card3 => TestBoard.GetNode<CardScene> (nameof (Card3));
	
	protected CardScene Card4 => TestBoard.GetNode<CardScene> (nameof (Card4));
	
	protected DealScene LeftDeal => GetNode<DealScene> (nameof (LeftDeal));

	protected DealScene RightDeal => GetNode<DealScene> (nameof (RightDeal));

	IGame _game;
	IGame Game {
		get { return _game; }
		set {
			_game = value;
			Bind (_game);
		}
	}

	void Bind (IGame game)
	{
		if (game == null) {
			return;
		}

		LeftDeal.Deal = game.Player1.Deal;
		RightDeal.Deal = game.Player2.Deal;
		Board.Board = game.Board;

		game.Player1.OnPlayCard += Player1_PlayCard;
		game.Player2.OnPlayCard += Player2_PlayCard;
	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Game = new SampleGame ();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
	}

	public override void _Input(InputEvent inputEvent)
	{
		if (inputEvent.IsActionPressed("ui_left")) {
			TestBoard.Rotate (new Vector3 (0, 1, 0), (float) Math.PI / 16);
		}
		else if (inputEvent.IsActionPressed("ui_right")) {
			TestBoard.Rotate (new Vector3 (0, 1, 0), -(float) Math.PI / 16);
		}
		else if (inputEvent.IsActionPressed("card1")) {
			((SampleGame) Game).Player2Turn (cardIdx: 0);
		}
		else if (inputEvent.IsActionPressed("card2")) {
			((SampleGame) Game).Player2Turn (cardIdx: 1);
		}
		else if (inputEvent.IsActionPressed("card3")) {
			((SampleGame) Game).Player2Turn (cardIdx: 2);
		}
		else if (inputEvent.IsActionPressed("card4")) {
			((SampleGame) Game).Player2Turn (cardIdx: 3);
		}
		else if (inputEvent.IsActionPressed("card5")) {
			((SampleGame) Game).Player2Turn (cardIdx: 4);
		}
		/*
		else if (inputEvent.IsActionPressed("test_rotate1")) {
			Card1.Rotate_H ();
		}
		else if (inputEvent.IsActionPressed("test_rotate2")) {
			Card2.Rotate_V ();
		}
		else if (inputEvent.IsActionPressed("test_rotate3")) {
			Card3.Rotate_D1 ();
		}
		else if (inputEvent.IsActionPressed("test_rotate4")) {
			Card4.Rotate_D2 ();
		}*/
	}

	void Player2_PlayCard (object sender, PlayCardEventArgs args)
	{
		var cardScene = RightDeal.CardScenes [args.PlayCardThinkResult.CardIndex];
		RightDeal.RemoveCardScene (cardScene);
		Board.AddCardScene (cardScene, args.PlayCardThinkResult.BoardCoords.X, args.PlayCardThinkResult.BoardCoords.Y);
	}

	void Player1_PlayCard (object sender, PlayCardEventArgs args)
	{
		var cardScene = LeftDeal.CardScenes [args.PlayCardThinkResult.CardIndex];
		LeftDeal.RemoveCardScene (cardScene);
		Board.AddCardScene (cardScene, args.PlayCardThinkResult.BoardCoords.X, args.PlayCardThinkResult.BoardCoords.Y);
	}
}
