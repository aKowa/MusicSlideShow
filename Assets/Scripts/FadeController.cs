using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
	public Image image;
	public Text text;
	public float speed = 1f;

	private AudioSource audioSource;

	public void Start ()
	{
		audioSource = this.GetComponent<AudioSource>();
		if ( audioSource.clip == null )
		{
			Debug.LogError( "No credit song assigned." );
		}

		StartCoroutine( FadeIn() );
		StartCoroutine( FadeOut() );
	}

	private IEnumerator FadeIn ()
	{
		for ( float t=0; t <= 1; t += Time.deltaTime * speed )
		{
			var c = Color.Lerp ( Color.clear, Color.white, t );
			text.color = c;
			image.color = c;
			yield return null;
		}
	}


	private IEnumerator FadeOut ()
	{
		yield return new WaitForSeconds( audioSource.clip.length );
		for (float t = 0; t <= 1; t += Time.deltaTime * speed)
		{
			var c = Color.Lerp ( Color.white, Color.clear, t );
			text.color = c;
			image.color = c;
			yield return null;
		}

		Application.Quit();
	}
}
