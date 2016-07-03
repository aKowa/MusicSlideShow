using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MusicShowManager : MonoBehaviour
{
	public Image image;
	public Image imageFade;
	public Text text;
	public AudioSource audioSource;
	public int[] years;
	public float minDuration = 5f;
	public float maxDuration = 10f;
	private int yearID = -1;
	private MusicShow currentMusicShow;
	private float spriteDuration;


	private void Start ()
	{
		yearID = -1;
		SetNextMusicShow();
	}


	private IEnumerator ShowText()
	{
		image.enabled = false;
		text.enabled = true;
		text.text = currentMusicShow.year.ToString();
		yield return new WaitForSeconds( spriteDuration );
		image.enabled = true;
		text.enabled = false;
		StartCoroutine (ShowSprites());
	}


	private IEnumerator ShowSprites()
	{
		var sprite = currentMusicShow.GetNextSprite();
		if ( sprite == null ) yield break;
		image.sprite = sprite;
		yield return new WaitForSeconds( spriteDuration );
		StartCoroutine( ShowSprites() );
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
			spriteDuration = currentMusicShow.DurationPerSprite ();

			StopAllCoroutines ();
			StartCoroutine ( Play () );
			StartCoroutine ( ShowText() );

			Debug.Log ( "Year: " + years[yearID] + "  Duration: " + currentMusicShow.Duration + "s" + "  Duration per sprite: " + spriteDuration + "s" );
			if (spriteDuration < minDuration)
			{
				Debug.LogError("Duration per sprite too low. Use less sprites or longer music!");
			}
			else if (spriteDuration > maxDuration )
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
		Debug.Log("Reload");
		SceneManager.LoadScene(0);
	}
}
