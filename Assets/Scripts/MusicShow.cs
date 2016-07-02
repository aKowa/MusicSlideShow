using UnityEngine;
using System.Linq;

public class MusicShow
{
	public readonly int year;
	private readonly AudioClip[] musicClips;
	private int clipID;
	private readonly Sprite[] sprites;
	private int spriteID;


	public MusicShow( AudioClip[] newClips, Sprite[] newSprites, int newYear )
	{
		musicClips = newClips;
		sprites = newSprites;
		year = newYear;
		clipID = -1;
		spriteID = -1;
	}


	public float Duration
	{
		get
		{
			return musicClips.Sum(m => m.length);
		}
	}


	public float DurationPerSprite()
	{
		return Duration / sprites.Length;
	}


	public AudioClip GetNextClip()
	{
		++clipID;
		return clipID < musicClips.Length ? musicClips[clipID] : null;
	}


	public Sprite GetNextSprite()
	{
		++spriteID;
		return spriteID < sprites.Length ? sprites[spriteID] : null;
	}
}
