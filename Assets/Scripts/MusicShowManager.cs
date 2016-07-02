using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;

public class MusicShowManager : MonoBehaviour
{
	public Image image;
	public AudioSource audioSource;
	public int[] years;
	public float minDuration = 5f;
	public float maxDuration = 10f;
	private int yearID = -1;
	private MusicShow currentMusicShow;


	private void Start ()
	{
		yearID = -1;
		SetNextMusicShow();
	}


	private IEnumerator Show()
	{
		var sprite = currentMusicShow.GetNextSprite();
		if ( sprite == null ) yield break;
		image.sprite = sprite;
		yield return new WaitForSeconds( currentMusicShow.DurationPerSprite() );
		StartCoroutine( Show() );
	}


	private IEnumerator Play()
	{
		var clip = currentMusicShow.GetNextClip ();

		if (clip == null)
		{
			SetNextMusicShow();
			yield break;
		}

		audioSource.clip = clip;
		audioSource.Play();
		yield return new WaitForSeconds ( audioSource.clip.length );
		StartCoroutine( Play() );
	}


	public void SetNextMusicShow()
	{
		++yearID;
		if (yearID < years.Length)
		{
			var sprites = (Sprite[])Resources.LoadAll<Sprite> ( years[yearID].ToString () );
			var clips = (AudioClip[])Resources.LoadAll<AudioClip> ( years[yearID].ToString () );
			currentMusicShow = new MusicShow ( clips, sprites, years[yearID] );

			StopAllCoroutines ();
			StartCoroutine ( Play () );
			StartCoroutine ( Show () );

			Debug.Log ( "Year: " + years[yearID] + "  Duration: " + currentMusicShow.Duration + "s" + "  Duration per sprite: " + currentMusicShow.DurationPerSprite() + "s" );
			if (currentMusicShow.DurationPerSprite() < minDuration)
			{
				Debug.LogError("Duration per sprite too low. Use less sprites or longer music!");
			}
			else if ( currentMusicShow.DurationPerSprite() > maxDuration )
			{
				Debug.LogError ( "Duration per sprite too high. Use more sprites or shorten music!" );
			}
		}
		else
		{
			OnEnd();
		}
	}

	private void OnEnd()
	{
		Debug.Log("All Done");
	}
}
