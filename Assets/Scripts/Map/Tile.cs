using System.Collections.Generic;
using UnityEngine;

public class Tile
{
	private TileScriptable _data;

	private readonly SpriteRenderer _renderer2D;
	private readonly AudioSource _source;
	
	private int _currentIndex;
	
	public bool IsWalkable { get; private set; }
	public bool IsBreakable { get; private set; }

	public Tile(SpriteRenderer renderer2D, AudioSource source)
	{
		_renderer2D = renderer2D;
		_source = source;
	}

	public void Hit()
	{
		if (IsWalkable)
			return;

		int lastIndex = _data.sprites.Length - 1;
		_currentIndex = _currentIndex < lastIndex ? _currentIndex + 1 : lastIndex;

		_renderer2D.sprite = _data.sprites[_currentIndex];
		//_source.Play();

		//Wheater arrived the last index on the sprite array, set not already breakable
		IsBreakable = _currentIndex == lastIndex ? false : true;

		//Walkable is always inverse from brekable
		IsWalkable = !IsBreakable;
	}
	public void SetTile(TileScriptable data)
	{
		_data = data;

		_renderer2D.sprite = _data.sprites[0];
		_renderer2D.sortingLayerName = _data.sortingLayerName;
		_renderer2D.sortingOrder = _data.sortingOrder;

		IsWalkable = _data.isWalkable;
		IsBreakable = _data.isBreakable;
		
		_currentIndex = 0;
	}	

}
