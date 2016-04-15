using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class DigitImage
{
	public byte[][] pixels;
	public byte label;

	public DigitImage( byte[][] pixels,
	  byte label )
	{
		this.pixels = new byte[28][];
		for( int i = 0 ; i < this.pixels.Length ; ++i )
			this.pixels[i] = new byte[28];

		for( int i = 0 ; i < 28 ; ++i )
			for( int j = 0 ; j < 28 ; ++j )
				this.pixels[i][j] = pixels[i][j];

		this.label = label;
	}

	public override string ToString()
	{
		string s = "";
		for( int i = 0 ; i < 28 ; ++i )
		{
			for( int j = 0 ; j < 28 ; ++j )
			{
				if( this.pixels[i][j] == 0 )
					s += " "; // white
				else if( this.pixels[i][j] == 255 )
					s += "O"; // black
				else
					s += "."; // gray
			}
			s += "\n";
		}
		s += this.label.ToString();
		return s;
	} // ToString

}
public class ReadMNIST : MonoBehaviour
{
	public GameObject ball;
	public Material white;
	public Material blue;
	public Material black;


	List<byte[][]> images;
	List<byte> labels;
	// Use this for initialization
	void Start()
	{
		images = new List<byte[][]>();
		labels = new List<byte>();
		byte[][] pixels = new byte[28][];
		FileStream ifsLabels = new FileStream("Assets/MNIST/t10k-labels.idx1-ubyte", FileMode.Open); // test labels
		FileStream ifsImages = new FileStream("Assets/MNIST/t10k-images.idx3-ubyte", FileMode.Open); // test images
		BinaryReader brLabels = new BinaryReader(ifsLabels);
		BinaryReader brImages = new BinaryReader(ifsImages);

		int magic1 = brImages.ReadInt32(); // discard
		int numImages = brImages.ReadInt32();
		int numRows = brImages.ReadInt32();
		int numCols = brImages.ReadInt32();

		int magic2 = brLabels.ReadInt32();
		int numLabels = brLabels.ReadInt32();

		
		for( int i = 0 ; i < pixels.Length ; ++i )
			pixels[i] = new byte[28];

		// each test image
		for( int di = 0 ; di < 10000 ; ++di )
		{
			for( int i = 0 ; i < 28 ; ++i )
			{
				for( int j = 0 ; j < 28 ; ++j )
				{
					byte b = brImages.ReadByte();
					pixels[i][j] = b;
				}
			}
			images.Add(pixels);
			labels.Add(brLabels.ReadByte());
			

			//DigitImage dImage = new DigitImage(pixels, lbl);
			//DrawPixels(pixels, lbl, di);
			//DrawImage(pixels, di);
			//dImage.ToString();
			//Debug.Log(dImage.ToString());
		} // each image

		ifsImages.Close();
		brImages.Close();
		ifsLabels.Close();
		brLabels.Close();
	}

	public void DrawPixels( byte[][] pixels, byte label, int indice )
	{
		Vector3 pos = Vector3.zero;
		indice *= 20;
		int x = indice, z = 0;
		int k = 0;
		for( int i = 0 ; i < 28 ; ++i )
		{
			for( int j = 0 ; j < 28 ; ++j )
			{
				var go = GameObject.Instantiate(ball) as GameObject;
				if( pixels[i][j] == 0 )
				{
					go.GetComponent<MeshRenderer>().material = blue;
				}
				else if( pixels[i][j] == 255 )
				{
					go.GetComponent<MeshRenderer>().material = white;
				}
				else
				{
					go.GetComponent<MeshRenderer>().material = black;
				}
				pos = new Vector3(i, 0, j);
				go.transform.position = pos;
				k++;
			}
		}
		Debug.Log(label + " has been drawned");
	}


	void DrawImage( string s, int i )
	{
		Vector3 pos = Vector3.zero;
		i *= 20;
		int x = i, z = 0;
		int j = i;
		foreach( var c in s )
		{
			if( c == s[s.Length - 1] )
				break;
			//Debug.Log(c);
			if( c == '\n' )
			{
				z--;
				j = i;
			}
			else
			{
				var go = GameObject.Instantiate(ball) as GameObject;
				if( c == ' ' ) // white
				{
					go.GetComponent<MeshRenderer>().material = white;
				}
				else if( c == 'O' ) // black)
				{
					//return;
					go.GetComponent<MeshRenderer>().material = black;
				}
				else if( c == '.' ) // gray)
				{
					go.GetComponent<MeshRenderer>().material = blue;
				}
				pos = new Vector3(i + j, 0, z);
				go.transform.position = pos;
				j++;
			}
		}
		Debug.Log("Generate : " + s[s.Length - 1]);
	}



	









}
