using Godot;
using System;
using System.Collections.Generic;

public class DealScene : Spatial
{
	ADeal _deal;

	public ADeal Deal {
		get { return _deal; }
		set {
			_deal = value;
			BindDeal (_deal);
		}
	}

	public IList<CardScene> CardScenes { get; set; } 


	[Export]
	public float CardSpacing { get; set; } = 2.5f;

	[Export]
	public float CardRotation { get; set; } = 1;

	void BindDeal (ADeal deal)
	{
		if (_deal == null) {
			return;
		}

		Reset();

		var cardSceneRes = ResourceLoader.Load<PackedScene> ("res://CardScene.tscn");
		var idx = 0;
		foreach (var slot in deal.CardSlots) {
			var cardScene = (CardScene) cardSceneRes.Instance ();
			cardScene.Card = slot.Card;
			cardScene.Card.OnIsSelectInDealChanged += OnCardIsSelectedInDealChanged;
			cardScene.Name = "Card" + idx;
			cardScene.Translation = new Vector3 (0f, (deal.CardSlots.Count - 1) * CardSpacing / 2f - (idx * CardSpacing), 0f);

			if (!deal.IsOpen) {
				cardScene.Rotate (new Vector3 (0, 1, 0), Mathf.Pi);
			}

			cardScene.Rotate (new Vector3 (1f, 0f, 0f), Mathf.Deg2Rad (CardRotation));
			AddCardScene (cardScene);
			idx++;
		}
	}

	void OnCardIsSelectedInDealChanged(object sender, EventArgs e)
	{
		var card = (ACard) sender;
		var cardSc = GetCardSceneByCard(card);
		if (cardSc != null) {
			if (card.IsSelectedInDeal) {
				cardSc.Translation -= new Vector3 (0.5f, 0f, 0f);
			}
			else {
				cardSc.Translation += new Vector3 (0.5f, 0f, 0f);
			}
		}
	}

	CardScene GetCardSceneByCard (ACard card)
	{
		var cardIdx = Deal.GetSlotIndex(card);
		if (cardIdx >= 0) {
			return CardScenes[cardIdx];
		}
		return null;
	}

	void Reset ()
	{
		if (CardScenes != null) {
			foreach (var cardScene in CardScenes) {
				if (cardScene != null) {
					RemoveChild (cardScene);
				}
			}
		}
		CardScenes = new List<CardScene> ();
	}

	void AddCardScene (CardScene cardScene)
	{
		AddChild(cardScene);
		CardScenes.Add (cardScene);
	}

	public void RemoveCardScene (CardScene cardScene)
	{
		RemoveChild (cardScene);
		var idx = CardScenes.IndexOf (cardScene);
		CardScenes [idx] = null;
		var card = Deal.CardSlots[idx].Card;
		if (card != null) {
			card.IsSelectedInDeal = false;
			card.OnIsSelectInDealChanged -= OnCardIsSelectedInDealChanged;
			Deal.CardSlots[idx] = null;
		}
	}
}
