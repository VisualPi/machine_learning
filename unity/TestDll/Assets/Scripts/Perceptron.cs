using UnityEngine;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;

public class Perceptron : MonoBehaviour
{
	public GameObject boules;
	public GameObject examples;
	private List<Ball> balls;
	private List<Ball> exBalls;

	public Material defaultColor;
	[SerializeField]
	public List<int> neuronByLayer;

	#region DLL_DEFINITIONS
	[DllImport("DllForUnity")]
	public extern static System.IntPtr linear_create_model( int input_model );

	[DllImport("DllForUnity")]
	public extern static void linear_remove_model( System.IntPtr model );
	[DllImport("DllForUnity")]
	public extern static int perceptron_classify( System.IntPtr model, double[] input, int inputSize );
	[DllImport("DllForUnity")]
	public extern static void perceptron_fit( System.IntPtr model, double[] inputs, int modelSize, int inputsSize, double[] results );
	[DllImport("DllForUnity")]
	public extern static void classification_hebb( System.IntPtr model, double[] input, int inputSize, double result );
	[DllImport("DllForUnity")]
	public extern static void regression_fit( System.IntPtr model, double[] inputs, int modelSize, int inputsSize, double[] results );
	[DllImport("DllForUnity")]
	public extern static double perceptron_predict( System.IntPtr model, double[] input, int inputSize );
	[DllImport("DllForUnity")]
	public extern static void SetAlpha( double value );
	[DllImport("DllForUnity")]
	public extern static void SetIteration( int value );

	[DllImport("DllForUnity")]
	public extern static System.IntPtr multilayer_create_model_bis( int[] perceptronsByLayer, int nbLayer );
	[DllImport("DllForUnity")]
	public extern static System.IntPtr multilayer_classify_perceptron( System.IntPtr model, double[] inputs, int nbLayer, bool useClassify );
	[DllImport("DllForUnity")]
	public extern static void multilayer_classify_gradient_backpropagation( System.IntPtr model, double[] inputs, int inputSize, int exampleNumber, double[] output, int nbLayer, bool useClassify );
	[DllImport("DllForUnity")]
	public extern static double GetResultForIndex( System.IntPtr model, int nbLayer, int index );
	#endregion
	private System.IntPtr _model;

	public int iteration = 10000;
	public double alpha = 0.1;

	void Start()
	{
		SetIteration(iteration);
		SetAlpha(alpha);
	}
	#region SIMPLE
	public void StartProcessClassify()
	{
		SetIteration(iteration);
		SetAlpha(alpha);
		balls = new List<Ball>();
		exBalls = new List<Ball>();
		for( var i = 0 ; i < boules.transform.childCount ; ++i )
		{
			balls.Add(boules.transform.GetChild(i).GetComponent<Ball>());
		}
		for( var i = 0 ; i < examples.transform.childCount ; ++i )
		{
			for( var j = 0 ; j < examples.transform.GetChild(i).childCount ; ++j )
			{
				exBalls.Add(examples.transform.GetChild(i).GetChild(j).GetComponent<Ball>());
			}
		}
		_model = linear_create_model(2);

		List<double> inputs = new List<double>();
		List<double> results = new List<double>();
		foreach( var b in exBalls )
		{
			inputs.Add(b.transform.position.x);
			inputs.Add(b.transform.position.z);
			results.Add(b.c == EColor.BLUE ? -1 : 1);
		}
		perceptron_fit(_model, inputs.ToArray(), 2, inputs.Count, results.ToArray());
		foreach( var b in balls )
		{
			double[] wanted = new double[2] { b.transform.position.x, b.transform.position.z };
			if( perceptron_classify(_model, wanted, 2) == -1 )
			{
				b.renderer.material.color = b.blue.color;
				b.transform.position = new Vector3(b.transform.position.x, -1, b.transform.position.z);
			}
			else
			{
				b.renderer.material.color = b.red.color;
				b.transform.position = new Vector3(b.transform.position.x, +1, b.transform.position.z);
			}
		}
	}
	public void StartProcessRegression()
	{
		SetIteration(iteration);
		SetAlpha(alpha);
		balls = new List<Ball>();
		exBalls = new List<Ball>();
		for( var i = 0 ; i < boules.transform.childCount ; ++i )
			balls.Add(boules.transform.GetChild(i).GetComponent<Ball>());
		for( var i = 0 ; i < examples.transform.childCount ; ++i )
		{
			for( var j = 0 ; j < examples.transform.GetChild(i).childCount ; ++j )
				exBalls.Add(examples.transform.GetChild(i).GetChild(j).GetComponent<Ball>());
		}
		_model = linear_create_model(2);

		List<double> inputs = new List<double>();
		List<double> results = new List<double>();
		foreach( var b in exBalls )
		{
			inputs.Add(b.transform.position.x);
			inputs.Add(b.transform.position.z);
			results.Add(b.c == EColor.BLUE ? -1 : 1);
		}
		regression_fit(_model, inputs.ToArray(), 2, inputs.Count, results.ToArray());
		foreach( var b in balls )
		{
			double[] wanted = new double[2] { b.transform.position.x, b.transform.position.z };
			var coef = (float)perceptron_predict(_model, wanted, 2);
			b.renderer.material.color = new Color(Mathf.Max(0, coef * defaultColor.color.r), 0, Mathf.Max(0, defaultColor.color.b * -coef));
			//b.transform.position = new Vector3(b.transform.position.x, (float)perceptron_predict(_model, wanted, 2), b.transform.position.z);
		}
	}
	#endregion
	#region SQUARE
	public void StartProcessClassifySquare()
	{
		SetIteration(iteration);
		SetAlpha(alpha);
		balls = new List<Ball>();
		exBalls = new List<Ball>();
		for( var i = 0 ; i < boules.transform.childCount ; ++i )
			balls.Add(boules.transform.GetChild(i).GetComponent<Ball>());
		for( var i = 0 ; i < examples.transform.childCount ; ++i )
		{
			for( var j = 0 ; j < examples.transform.GetChild(i).childCount ; ++j )
				exBalls.Add(examples.transform.GetChild(i).GetChild(j).GetComponent<Ball>());
		}
		_model = linear_create_model(2);

		List<double> inputs = new List<double>();
		List<double> results = new List<double>();
		foreach( var b in exBalls )
		{
			inputs.Add(b.transform.position.x * b.transform.position.x);
			inputs.Add(b.transform.position.z * b.transform.position.z);
			results.Add(b.c == EColor.BLUE ? -1 : 1);
		}
		perceptron_fit(_model, inputs.ToArray(), 2, inputs.Count, results.ToArray());
		foreach( var b in balls )
		{
			double[] wanted = new double[2] { b.transform.position.x * b.transform.position.x, b.transform.position.z * b.transform.position.z };
			if( perceptron_classify(_model, wanted, 2) == -1 )
			{
				b.renderer.material.color = b.blue.color;
				b.transform.position = new Vector3(b.transform.position.x, -1, b.transform.position.z);
			}
			else
			{
				b.renderer.material.color = b.red.color;
				b.transform.position = new Vector3(b.transform.position.x, +1, b.transform.position.z);
			}
		}
	}
	public void StartProcessRegressionSquare()
	{
		SetIteration(iteration);
		SetAlpha(alpha);
		balls = new List<Ball>();
		exBalls = new List<Ball>();
		for( var i = 0 ; i < boules.transform.childCount ; ++i )
			balls.Add(boules.transform.GetChild(i).GetComponent<Ball>());
		for( var i = 0 ; i < examples.transform.childCount ; ++i )
		{
			for( var j = 0 ; j < examples.transform.GetChild(i).childCount ; ++j )
				exBalls.Add(examples.transform.GetChild(i).GetChild(j).GetComponent<Ball>());
		}
		_model = linear_create_model(2);

		List<double> inputs = new List<double>();
		List<double> results = new List<double>();
		foreach( var b in exBalls )
		{
			inputs.Add(b.transform.position.x * b.transform.position.x);
			inputs.Add(b.transform.position.z * b.transform.position.z);
			results.Add(b.c == EColor.BLUE ? -1 : 1);
		}
		regression_fit(_model, inputs.ToArray(), 2, inputs.Count, results.ToArray());
		foreach( var b in balls )
		{
			double[] wanted = new double[2] { b.transform.position.x * b.transform.position.x, b.transform.position.z * b.transform.position.z };
			var coef = (float)perceptron_predict(_model, wanted, 2);
			b.renderer.material.color = new Color(Mathf.Max(0, coef * defaultColor.color.r), 0, Mathf.Max(0, defaultColor.color.b * -coef));
			//b.transform.position = new Vector3(b.transform.position.x, (float)perceptron_predict(_model, wanted, 2), b.transform.position.z);
		}
	}
	#endregion
	#region CROSS
	public void StartProcessClassifyCross()
	{
		SetIteration(iteration);
		SetAlpha(alpha);
		balls = new List<Ball>();
		exBalls = new List<Ball>();
		for( var i = 0 ; i < boules.transform.childCount ; ++i )
		{
			balls.Add(boules.transform.GetChild(i).GetComponent<Ball>());
		}
		for( var i = 0 ; i < examples.transform.childCount ; ++i )
		{
			for( var j = 0 ; j < examples.transform.GetChild(i).childCount ; ++j )
			{
				exBalls.Add(examples.transform.GetChild(i).GetChild(j).GetComponent<Ball>());
			}
		}
		_model = linear_create_model(2);

		List<double> inputs = new List<double>();
		List<double> results = new List<double>();
		foreach( var b in exBalls )
		{
			inputs.Add(Mathf.Abs(b.transform.position.x) + Mathf.Abs(b.transform.position.z));
			inputs.Add(Mathf.Abs(b.transform.position.x * b.transform.position.z));
			results.Add(b.c == EColor.BLUE ? -1 : 1);
		}
		perceptron_fit(_model, inputs.ToArray(), 2, inputs.Count, results.ToArray());
		foreach( var b in balls )
		{
			double[] wanted = new double[2] { Mathf.Abs(b.transform.position.x) + Mathf.Abs(b.transform.position.z), Mathf.Abs(b.transform.position.x * b.transform.position.z) };
			if( perceptron_classify(_model, wanted, 2) == -1 )
			{
				b.renderer.material.color = b.blue.color;
				b.transform.position = new Vector3(b.transform.position.x, -1, b.transform.position.z);
			}
			else
			{
				b.renderer.material.color = b.red.color;
				b.transform.position = new Vector3(b.transform.position.x, +1, b.transform.position.z);
			}
		}
	}
	public void StartProcessRegressionCross()
	{
		SetIteration(iteration);
		SetAlpha(alpha);
		balls = new List<Ball>();
		exBalls = new List<Ball>();
		for( var i = 0 ; i < boules.transform.childCount ; ++i )
			balls.Add(boules.transform.GetChild(i).GetComponent<Ball>());
		for( var i = 0 ; i < examples.transform.childCount ; ++i )
		{
			for( var j = 0 ; j < examples.transform.GetChild(i).childCount ; ++j )
				exBalls.Add(examples.transform.GetChild(i).GetChild(j).GetComponent<Ball>());
		}
		_model = linear_create_model(2);

		List<double> inputs = new List<double>();
		List<double> results = new List<double>();
		foreach( var b in exBalls )
		{
			inputs.Add(Mathf.Abs(b.transform.position.x) + Mathf.Abs(b.transform.position.z));
			inputs.Add(Mathf.Abs(b.transform.position.x * b.transform.position.z));
			results.Add(b.c == EColor.BLUE ? -1 : 1);
		}
		regression_fit(_model, inputs.ToArray(), 2, inputs.Count, results.ToArray());
		foreach( var b in balls )
		{
			double[] wanted = new double[2] { Mathf.Abs(b.transform.position.x) + Mathf.Abs(b.transform.position.z), Mathf.Abs(b.transform.position.x * b.transform.position.z) };
			var coef = (float)perceptron_predict(_model, wanted, 2);
			b.renderer.material.color = new Color(Mathf.Max(0, coef * defaultColor.color.r), 0, Mathf.Max(0, defaultColor.color.b * -coef));
			//b.transform.position = new Vector3(b.transform.position.x, (float)perceptron_predict(_model, wanted, 2), b.transform.position.z);
		}
	}
	#endregion
	#region XOR
	public void StartProcessClassifyXOR()
	{
		SetIteration(iteration);
		SetAlpha(alpha);
		balls = new List<Ball>();
		exBalls = new List<Ball>();
		for( var i = 0 ; i < boules.transform.childCount ; ++i )
			balls.Add(boules.transform.GetChild(i).GetComponent<Ball>());
		for( var i = 0 ; i < examples.transform.childCount ; ++i )
		{
			for( var j = 0 ; j < examples.transform.GetChild(i).childCount ; ++j )
				exBalls.Add(examples.transform.GetChild(i).GetChild(j).GetComponent<Ball>());
		}
		_model = linear_create_model(1);

		List<double> inputs = new List<double>();
		List<double> results = new List<double>();
		foreach( var b in exBalls )
		{
			inputs.Add(b.transform.position.x * b.transform.position.z);
			results.Add(b.c == EColor.BLUE ? -1 : 1);
		}
		perceptron_fit(_model, inputs.ToArray(), 1, inputs.Count, results.ToArray());
		foreach( var b in balls )
		{
			double[] wanted = new double[1] { b.transform.position.x * b.transform.position.z };
			if( perceptron_classify(_model, wanted, 1) == -1 )
			{
				b.renderer.material.color = b.blue.color;
				b.transform.position = new Vector3(b.transform.position.x, -1, b.transform.position.z);
			}
			else
			{
				b.renderer.material.color = b.red.color;
				b.transform.position = new Vector3(b.transform.position.x, +1, b.transform.position.z);
			}
		}
	}
	public void StartProcessRegressionXOR()
	{
		SetIteration(iteration);
		SetAlpha(alpha);
		balls = new List<Ball>();
		exBalls = new List<Ball>();
		for( var i = 0 ; i < boules.transform.childCount ; ++i )
			balls.Add(boules.transform.GetChild(i).GetComponent<Ball>());
		for( var i = 0 ; i < examples.transform.childCount ; ++i )
		{
			for( var j = 0 ; j < examples.transform.GetChild(i).childCount ; ++j )
				exBalls.Add(examples.transform.GetChild(i).GetChild(j).GetComponent<Ball>());
		}
		_model = linear_create_model(2);

		List<double> inputs = new List<double>();
		List<double> results = new List<double>();
		foreach( var b in exBalls )
		{
			inputs.Add(b.transform.position.x * b.transform.position.z);
			results.Add(b.c == EColor.BLUE ? -1 : 1);
		}
		regression_fit(_model, inputs.ToArray(), 1, inputs.Count, results.ToArray());
		foreach( var b in balls )
		{
			double[] wanted = new double[1] { b.transform.position.x * b.transform.position.z };
			var coef = (float)perceptron_predict(_model, wanted, 1);
			b.renderer.material.color = new Color(Mathf.Max(0, coef * defaultColor.color.r), 0, Mathf.Max(0, defaultColor.color.b * -coef));
			//b.transform.position = new Vector3(b.transform.position.x, (float)perceptron_predict(_model, wanted, 2), b.transform.position.z);
		}
	}
	#endregion
	#region MULTICLASSE
	public void StartProcessCLassificationMulticlasse()
	{
		SetIteration(iteration);
		SetAlpha(alpha);
		balls = new List<Ball>();
		exBalls = new List<Ball>();
		List<EColor> types = new List<EColor>();
		for( var i = 0 ; i < boules.transform.childCount ; ++i )
			balls.Add(boules.transform.GetChild(i).GetComponent<Ball>());
		for( var i = 0 ; i < examples.transform.childCount ; ++i )
		{
			types.Add(examples.transform.GetChild(i).GetChild(0).GetComponent<Ball>().c);
			for( var j = 0 ; j < examples.transform.GetChild(i).childCount ; ++j )
				exBalls.Add(examples.transform.GetChild(i).GetChild(j).GetComponent<Ball>());
		}
		List<System.IntPtr> models = new List<System.IntPtr>();
		for( var i = 0 ; i < types.Count ; ++i ) //Pour toutes les classes
		{
			var currmodel = linear_create_model(2);
			List<double> inputs = new List<double>();
			List<double> results = new List<double>();
			foreach( var b in exBalls )
			{
				inputs.Add(b.transform.position.x);
				inputs.Add(b.transform.position.z);
				results.Add(b.c == types[i] ? -1 : 1);//Si la couleur de la boule est la couleur actuelle tu met -1, sinon 1
			}
			perceptron_fit(currmodel, inputs.ToArray(), 2, inputs.Count, results.ToArray());
			models.Add(currmodel);
		}
		foreach( var b in balls )
		{
			double[] wanted = new double[2] { b.transform.position.x, b.transform.position.z };
			for( var i = 0 ; i < models.Count ; ++i )
			{
				var coef = perceptron_classify(models[i], wanted, wanted.Length);
				if( coef == -1 )
				{
					b.renderer.material = b.GetColor(types[i]);
					b.transform.position = new Vector3(b.transform.position.x, b.GetPosByColor(types[i]), b.transform.position.z);
				}
			}
		}
	}
	public void StartProcessRegressionMulticlasse()
	{
		SetIteration(iteration);
		SetAlpha(alpha);
		balls = new List<Ball>();
		exBalls = new List<Ball>();
		List<EColor> types = new List<EColor>();
		for( var i = 0 ; i < boules.transform.childCount ; ++i )
			balls.Add(boules.transform.GetChild(i).GetComponent<Ball>());
		for( var i = 0 ; i < examples.transform.childCount ; ++i )
		{
			types.Add(examples.transform.GetChild(i).GetChild(0).GetComponent<Ball>().c);
			for( var j = 0 ; j < examples.transform.GetChild(i).childCount ; ++j )
				exBalls.Add(examples.transform.GetChild(i).GetChild(j).GetComponent<Ball>());
		}
		List<System.IntPtr> models = new List<System.IntPtr>();
		for( var i = 0 ; i < types.Count ; ++i ) //Pour toutes les classes
		{
			var currmodel = linear_create_model(2);
			List<double> inputs = new List<double>();
			List<double> results = new List<double>();
			foreach( var b in exBalls )
			{
				inputs.Add(b.transform.position.x);
				inputs.Add(b.transform.position.z);
				results.Add(b.c == types[i] ? -1 : 1);//Si la couleur de la boule est la couleur actuelle tu met -1, sinon 1
			}
			regression_fit(currmodel, inputs.ToArray(), 2, inputs.Count, results.ToArray());
			models.Add(currmodel);
		}
		foreach( var b in balls )
		{
			double[] wanted = new double[2] { b.transform.position.x, b.transform.position.z };
			double coef = 0;
			Color c = new Color();
			for( var i = 0 ; i < models.Count ; ++i )
			{
				coef = perceptron_predict(models[i], wanted, wanted.Length);
				c += ( ( 1f - (float)coef ) / 2f ) * b.GetColor(types[i]).color;
			}
			//coef /= models.Count;
			b.renderer.material.color = c;
			//b.transform.position = new Vector3(b.transform.position.x, (float)coef, b.transform.position.z);
		}
		for( var i = 0 ; i < models.Count ; ++i )
			linear_remove_model(models[i]);
	}
	#endregion
	void OnDestroy()
	{
		linear_remove_model(_model);
	}



	#region TEST_MNIST
	List<byte[][]> images;
	List<byte> labels;

	public void ReadMNIST()
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
					b = (byte)( b == 0 ? 0 : 255 );
					pixels[i][j] = b;
				}
			}
			images.Add(pixels);
			labels.Add(brLabels.ReadByte());
		} // each image

		ifsImages.Close();
		brImages.Close();
		ifsLabels.Close();
		brLabels.Close();
	}

	void GenerateRandomNumberMNIST()
	{
		int id = Random.Range(0, 10000);
		for( int x = 0 ; x < 28 ; ++x )
		{
			for( int y = 0 ; y < 28 ; ++y )
			{
				if (images[id][x][y] == 0)
				{

				}
				//inputs.Add((double)img[x][y]);
			}
		}
	}

	public void StartProcessCLassificationMultiCoucheMNIST()
	{
		ReadMNIST();
		SetIteration(iteration);
		SetAlpha(alpha);
		var currmodel = multilayer_create_model_bis(neuronByLayer.ToArray(), neuronByLayer.Count );
		List<double> inputs = new List<double>();
		List<double> results = new List<double>();
		int idx = 0;
		Debug.Log(images.Count);
		foreach( var img in images )
		{
			for( int x = 0 ; x < 28 ; ++x )
			{
				for( int y = 0 ; y < 28 ; ++y )
				{
					inputs.Add((double)img[x][y]);
				}
				for( var r = 0 ; r < 10 ; ++r )
					results.Add(labels[idx] == r ? 1 : -1);
			}
			idx++;
		}
		multilayer_classify_gradient_backpropagation(currmodel, inputs.ToArray(), neuronByLayer[0], images.Count, results.ToArray(), neuronByLayer.Count, true);
		double[] wanted = new double[28*28];
		for( var i = 1 ; i < 28 * 28 ; ++i )
		{
			wanted[i] = boules.transform.GetChild(i).GetComponent<BallMNISTInfo>().state;
		}
		var coef = multilayer_classify_perceptron(currmodel, wanted, neuronByLayer.Count, true);
		for( var i = 0 ; i < neuronByLayer[neuronByLayer.Count - 1] + 1 ; ++i )
		{
			Debug.Log(GetResultForIndex(currmodel, 3, i));
		}
	}
	public void StartProcessClassifyMultiCouche()
	{
		SetIteration(iteration);
		SetAlpha(alpha);
		balls = new List<Ball>();
		exBalls = new List<Ball>();
		for( var i = 0 ; i < boules.transform.childCount ; ++i )
		{
			balls.Add(boules.transform.GetChild(i).GetComponent<Ball>());
		}
		for( var i = 0 ; i < examples.transform.childCount ; ++i )
		{
			for( var j = 0 ; j < examples.transform.GetChild(i).childCount ; ++j )
			{
				exBalls.Add(examples.transform.GetChild(i).GetChild(j).GetComponent<Ball>());
			}
		}
		_model = multilayer_create_model_bis(neuronByLayer.ToArray(), neuronByLayer.Count);

		List<double> inputs = new List<double>();
		List<double> results = new List<double>();
		foreach( var b in exBalls )
		{
			inputs.Add(b.transform.position.x);
			inputs.Add(b.transform.position.z);
			results.Add(b.c == EColor.BLUE ? 1 : -1);
		}
		multilayer_classify_gradient_backpropagation(_model, inputs.ToArray(), neuronByLayer[0], exBalls.Count, results.ToArray(), neuronByLayer.Count, false);
		foreach( var b in balls )
		{
			double[] wanted = new double[2] { b.transform.position.x, b.transform.position.z };
			multilayer_classify_perceptron(_model, wanted, neuronByLayer.Count, false);
			for( var i = 1 ; i < neuronByLayer[neuronByLayer.Count - 1] + 1 ; ++i )
			{
				var coef = GetResultForIndex(_model, neuronByLayer.Count, i);
				if( coef > 0 )
				{
					b.renderer.material.color = b.blue.color;
					b.transform.position = new Vector3(b.transform.position.x, 1, b.transform.position.z);
				}
				else
				{
					b.renderer.material.color = b.red.color;
					b.transform.position = new Vector3(b.transform.position.x, -1, b.transform.position.z);
				}
			}

		}
	}

	#endregion



}
