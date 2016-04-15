using UnityEngine;
using System.Collections;

public enum EColor { DEFAULT = 0, BLUE, RED, GREEN, YELLOW }

public class Ball : MonoBehaviour
{
	public Transform t;
	public EColor c;
	[SerializeField]
	public MeshRenderer renderer;

	public Material blue;
	public Material red;
	public Material green;
	public Material yellow;
	void Start()
	{
		switch(c)
		{
		case EColor.BLUE:
			renderer.material = blue;
            break;
		case EColor.RED:
			renderer.material = red;
            break;
		case EColor.GREEN:
			renderer.material = green;
			break;
		case EColor.YELLOW:
			renderer.material = yellow;
			break;
		default:
			break;
		}
		
	}

	public Material GetColor( EColor color)
	{
		switch(color)
		{
		case EColor.BLUE:
			return blue;
		case EColor.RED:
			return red;
		case EColor.GREEN:
			return green;
		case EColor.YELLOW:
			return yellow;
		default:
			Debug.LogError("PAS NORMAL !");
			break;
		}
		return null;
	}
	public int GetPosByColor(EColor color)
	{
		switch( color )
		{
		case EColor.BLUE:
			return -1;
		case EColor.RED:
			return 1;
		case EColor.GREEN:
			return 2;
		case EColor.YELLOW:
			return 3;
		default:
			Debug.LogError("PAS NORMAL !");
			break;
		}
		return 0;
	}
}
