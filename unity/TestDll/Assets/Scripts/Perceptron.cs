using UnityEngine;
using System.Collections.Generic;

public class Perceptron : MonoBehaviour
{
	public GameObject boules;
	public GameObject examples;
	private List<Ball> balls;
	private List<Ball> exBalls;

	public Material defaultColor;

	private System.IntPtr _model;

	public int iteration = 10000;
	public double alpha = 0.1;

	void Start()
	{
        WrapperDllMachineLearning.SetIteration(iteration);
        WrapperDllMachineLearning.SetAlpha(alpha);
	}

	#region SIMPLE
	public void StartProcessClassify()
	{
        WrapperDllMachineLearning.SetIteration(iteration);
        WrapperDllMachineLearning.SetAlpha(alpha);
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
        _model = WrapperDllMachineLearning.linear_create_model(2);

		List<double> inputs = new List<double>();
		List<double> results = new List<double>();
		foreach( var b in exBalls )
		{
			inputs.Add(b.transform.position.x);
			inputs.Add(b.transform.position.z);
			results.Add(b.c == EColor.BLUE ? -1 : 1);
		}
        WrapperDllMachineLearning.perceptron_fit(_model, inputs.ToArray(), 2, inputs.Count, results.ToArray());
		foreach( var b in balls )
		{
			double[] wanted = new double[2] { b.transform.position.x, b.transform.position.z };
            if (WrapperDllMachineLearning.perceptron_classify(_model, wanted, 2) == -1)
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
        WrapperDllMachineLearning.SetIteration(iteration);
        WrapperDllMachineLearning.SetAlpha(alpha);
		balls = new List<Ball>();
		exBalls = new List<Ball>();
		for( var i = 0 ; i < boules.transform.childCount ; ++i )
			balls.Add(boules.transform.GetChild(i).GetComponent<Ball>());
		for( var i = 0 ; i < examples.transform.childCount ; ++i )
		{
			for( var j = 0 ; j < examples.transform.GetChild(i).childCount ; ++j )
				exBalls.Add(examples.transform.GetChild(i).GetChild(j).GetComponent<Ball>());
		}
        _model = WrapperDllMachineLearning.linear_create_model(2);

		List<double> inputs = new List<double>();
		List<double> results = new List<double>();
		foreach( var b in exBalls )
		{
			inputs.Add(b.transform.position.x);
			inputs.Add(b.transform.position.z);
			results.Add(b.c == EColor.BLUE ? -1 : 1);
		}
        WrapperDllMachineLearning.regression_fit(_model, inputs.ToArray(), 2, inputs.Count, results.ToArray());
		foreach( var b in balls )
		{
			double[] wanted = new double[2] { b.transform.position.x, b.transform.position.z };
            var coef = (float)WrapperDllMachineLearning.perceptron_predict(_model, wanted, 2);
			b.renderer.material.color = new Color(Mathf.Max(0, coef * defaultColor.color.r), 0, Mathf.Max(0, defaultColor.color.b * -coef));
			//b.transform.position = new Vector3(b.transform.position.x, (float)perceptron_predict(_model, wanted, 2), b.transform.position.z);
		}
	}
	#endregion SIMPLE

	#region SQUARE
	public void StartProcessClassifySquare()
	{
        WrapperDllMachineLearning.SetIteration(iteration);
        WrapperDllMachineLearning.SetAlpha(alpha);
		balls = new List<Ball>();
		exBalls = new List<Ball>();
		for( var i = 0 ; i < boules.transform.childCount ; ++i )
			balls.Add(boules.transform.GetChild(i).GetComponent<Ball>());
		for( var i = 0 ; i < examples.transform.childCount ; ++i )
		{
			for( var j = 0 ; j < examples.transform.GetChild(i).childCount ; ++j )
				exBalls.Add(examples.transform.GetChild(i).GetChild(j).GetComponent<Ball>());
		}
        _model = WrapperDllMachineLearning.linear_create_model(2);

		List<double> inputs = new List<double>();
		List<double> results = new List<double>();
		foreach( var b in exBalls )
		{
			inputs.Add(b.transform.position.x * b.transform.position.x);
			inputs.Add(b.transform.position.z * b.transform.position.z);
			results.Add(b.c == EColor.BLUE ? -1 : 1);
		}
        WrapperDllMachineLearning.perceptron_fit(_model, inputs.ToArray(), 2, inputs.Count, results.ToArray());
		foreach( var b in balls )
		{
			double[] wanted = new double[2] { b.transform.position.x * b.transform.position.x, b.transform.position.z * b.transform.position.z };
            if (WrapperDllMachineLearning.perceptron_classify(_model, wanted, 2) == -1)
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
        WrapperDllMachineLearning.SetIteration(iteration);
        WrapperDllMachineLearning.SetAlpha(alpha);
		balls = new List<Ball>();
		exBalls = new List<Ball>();
		for( var i = 0 ; i < boules.transform.childCount ; ++i )
			balls.Add(boules.transform.GetChild(i).GetComponent<Ball>());
		for( var i = 0 ; i < examples.transform.childCount ; ++i )
		{
			for( var j = 0 ; j < examples.transform.GetChild(i).childCount ; ++j )
				exBalls.Add(examples.transform.GetChild(i).GetChild(j).GetComponent<Ball>());
		}
        _model = WrapperDllMachineLearning.linear_create_model(2);

		List<double> inputs = new List<double>();
		List<double> results = new List<double>();
		foreach( var b in exBalls )
		{
			inputs.Add(b.transform.position.x * b.transform.position.x);
			inputs.Add(b.transform.position.z * b.transform.position.z);
			results.Add(b.c == EColor.BLUE ? -1 : 1);
		}
        WrapperDllMachineLearning.regression_fit(_model, inputs.ToArray(), 2, inputs.Count, results.ToArray());
		foreach( var b in balls )
		{
			double[] wanted = new double[2] { b.transform.position.x * b.transform.position.x, b.transform.position.z * b.transform.position.z };
            var coef = (float)WrapperDllMachineLearning.perceptron_predict(_model, wanted, 2);
			b.renderer.material.color = new Color(Mathf.Max(0, coef * defaultColor.color.r), 0, Mathf.Max(0, defaultColor.color.b * -coef));
			//b.transform.position = new Vector3(b.transform.position.x, (float)perceptron_predict(_model, wanted, 2), b.transform.position.z);
		}
	}
	#endregion SQUARE

	#region CROSS
	public void StartProcessClassifyCross()
	{
        WrapperDllMachineLearning.SetIteration(iteration);
        WrapperDllMachineLearning.SetAlpha(alpha);
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
        _model = WrapperDllMachineLearning.linear_create_model(2);

		List<double> inputs = new List<double>();
		List<double> results = new List<double>();
		foreach( var b in exBalls )
		{
			inputs.Add(Mathf.Abs(b.transform.position.x) + Mathf.Abs(b.transform.position.z));
			inputs.Add(Mathf.Abs(b.transform.position.x * b.transform.position.z));
			results.Add(b.c == EColor.BLUE ? -1 : 1);
		}
        WrapperDllMachineLearning.perceptron_fit(_model, inputs.ToArray(), 2, inputs.Count, results.ToArray());
		foreach( var b in balls )
		{
			double[] wanted = new double[2] { Mathf.Abs(b.transform.position.x) + Mathf.Abs(b.transform.position.z), Mathf.Abs(b.transform.position.x * b.transform.position.z) };
            if (WrapperDllMachineLearning.perceptron_classify(_model, wanted, 2) == -1)
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
        WrapperDllMachineLearning.SetIteration(iteration);
        WrapperDllMachineLearning.SetAlpha(alpha);
		balls = new List<Ball>();
		exBalls = new List<Ball>();
		for( var i = 0 ; i < boules.transform.childCount ; ++i )
			balls.Add(boules.transform.GetChild(i).GetComponent<Ball>());
		for( var i = 0 ; i < examples.transform.childCount ; ++i )
		{
			for( var j = 0 ; j < examples.transform.GetChild(i).childCount ; ++j )
				exBalls.Add(examples.transform.GetChild(i).GetChild(j).GetComponent<Ball>());
		}
        _model = WrapperDllMachineLearning.linear_create_model(2);

		List<double> inputs = new List<double>();
		List<double> results = new List<double>();
		foreach( var b in exBalls )
		{
			inputs.Add(Mathf.Abs(b.transform.position.x) + Mathf.Abs(b.transform.position.z));
			inputs.Add(Mathf.Abs(b.transform.position.x * b.transform.position.z));
			results.Add(b.c == EColor.BLUE ? -1 : 1);
		}
        WrapperDllMachineLearning.regression_fit(_model, inputs.ToArray(), 2, inputs.Count, results.ToArray());
		foreach( var b in balls )
		{
			double[] wanted = new double[2] { Mathf.Abs(b.transform.position.x) + Mathf.Abs(b.transform.position.z), Mathf.Abs(b.transform.position.x * b.transform.position.z) };
            var coef = (float)WrapperDllMachineLearning.perceptron_predict(_model, wanted, 2);
			b.renderer.material.color = new Color(Mathf.Max(0, coef * defaultColor.color.r), 0, Mathf.Max(0, defaultColor.color.b * -coef));
			//b.transform.position = new Vector3(b.transform.position.x, (float)perceptron_predict(_model, wanted, 2), b.transform.position.z);
		}
	}
	#endregion CROSS

	#region XOR
	public void StartProcessClassifyXOR()
	{
        WrapperDllMachineLearning.SetIteration(iteration);
        WrapperDllMachineLearning.SetAlpha(alpha);
		balls = new List<Ball>();
		exBalls = new List<Ball>();
		for( var i = 0 ; i < boules.transform.childCount ; ++i )
			balls.Add(boules.transform.GetChild(i).GetComponent<Ball>());
		for( var i = 0 ; i < examples.transform.childCount ; ++i )
		{
			for( var j = 0 ; j < examples.transform.GetChild(i).childCount ; ++j )
				exBalls.Add(examples.transform.GetChild(i).GetChild(j).GetComponent<Ball>());
		}
        _model = WrapperDllMachineLearning.linear_create_model(1);

		List<double> inputs = new List<double>();
		List<double> results = new List<double>();
		foreach( var b in exBalls )
		{
			inputs.Add(b.transform.position.x * b.transform.position.z);
			results.Add(b.c == EColor.BLUE ? -1 : 1);
		}
        WrapperDllMachineLearning.perceptron_fit(_model, inputs.ToArray(), 1, inputs.Count, results.ToArray());
		foreach( var b in balls )
		{
			double[] wanted = new double[1] { b.transform.position.x * b.transform.position.z };
            if (WrapperDllMachineLearning.perceptron_classify(_model, wanted, 1) == -1)
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
        WrapperDllMachineLearning.SetIteration(iteration);
        WrapperDllMachineLearning.SetAlpha(alpha);
		balls = new List<Ball>();
		exBalls = new List<Ball>();
		for( var i = 0 ; i < boules.transform.childCount ; ++i )
			balls.Add(boules.transform.GetChild(i).GetComponent<Ball>());
		for( var i = 0 ; i < examples.transform.childCount ; ++i )
		{
			for( var j = 0 ; j < examples.transform.GetChild(i).childCount ; ++j )
				exBalls.Add(examples.transform.GetChild(i).GetChild(j).GetComponent<Ball>());
		}
        _model = WrapperDllMachineLearning.linear_create_model(2);

		List<double> inputs = new List<double>();
		List<double> results = new List<double>();
		foreach( var b in exBalls )
		{
			inputs.Add(b.transform.position.x * b.transform.position.z);
			results.Add(b.c == EColor.BLUE ? -1 : 1);
		}
        WrapperDllMachineLearning.regression_fit(_model, inputs.ToArray(), 1, inputs.Count, results.ToArray());
		foreach( var b in balls )
		{
			double[] wanted = new double[1] { b.transform.position.x * b.transform.position.z };
            var coef = (float)WrapperDllMachineLearning.perceptron_predict(_model, wanted, 1);
			b.renderer.material.color = new Color(Mathf.Max(0, coef * defaultColor.color.r), 0, Mathf.Max(0, defaultColor.color.b * -coef));
			//b.transform.position = new Vector3(b.transform.position.x, (float)perceptron_predict(_model, wanted, 2), b.transform.position.z);
		}
	}
	#endregion XOR

	#region MULTICLASSE
	public void StartProcessCLassificationMulticlasse()
	{
        WrapperDllMachineLearning.SetIteration(iteration);
        WrapperDllMachineLearning.SetAlpha(alpha);
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
            var currmodel = WrapperDllMachineLearning.linear_create_model(2);
			List<double> inputs = new List<double>();
			List<double> results = new List<double>();
			foreach( var b in exBalls )
			{
				inputs.Add(b.transform.position.x);
				inputs.Add(b.transform.position.z);
				results.Add(b.c == types[i] ? -1 : 1);//Si la couleur de la boule est la couleur actuelle tu met -1, sinon 1
			}
            WrapperDllMachineLearning.perceptron_fit(currmodel, inputs.ToArray(), 2, inputs.Count, results.ToArray());
			models.Add(currmodel);
		}
		foreach( var b in balls )
		{
			double[] wanted = new double[2] { b.transform.position.x, b.transform.position.z };
			for( var i = 0 ; i < models.Count ; ++i )
			{
                var coef = WrapperDllMachineLearning.perceptron_classify(models[i], wanted, wanted.Length);
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
        WrapperDllMachineLearning.SetIteration(iteration);
        WrapperDllMachineLearning.SetAlpha(alpha);
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
            var currmodel = WrapperDllMachineLearning.linear_create_model(2);
			List<double> inputs = new List<double>();
			List<double> results = new List<double>();
			foreach( var b in exBalls )
			{
				inputs.Add(b.transform.position.x);
				inputs.Add(b.transform.position.z);
				results.Add(b.c == types[i] ? -1 : 1);//Si la couleur de la boule est la couleur actuelle tu met -1, sinon 1
			}
            WrapperDllMachineLearning.regression_fit(currmodel, inputs.ToArray(), 2, inputs.Count, results.ToArray());
			models.Add(currmodel);
		}
		foreach( var b in balls )
		{
			double[] wanted = new double[2] { b.transform.position.x, b.transform.position.z };
			double coef = 0;
			Color c = new Color();
			for( var i = 0 ; i < models.Count ; ++i )
			{
                coef = WrapperDllMachineLearning.perceptron_predict(models[i], wanted, wanted.Length);
				c += ( ( 1f - (float)coef ) / 2f ) * b.GetColor(types[i]).color;
			}
			//coef /= models.Count;
			b.renderer.material.color = c;
			//b.transform.position = new Vector3(b.transform.position.x, (float)coef, b.transform.position.z);
		}
		for( var i = 0 ; i < models.Count ; ++i )
            WrapperDllMachineLearning.linear_remove_model(models[i]);
	}
	#endregion

	#region MULTICOUCHE
	public void StartProcessClassificationMulticouche()
	{
        WrapperDllMachineLearning.SetIteration(iteration);
        WrapperDllMachineLearning.SetAlpha(alpha);
	}
    #endregion MULTICOUCHE
    
	void OnDestroy()
	{
        WrapperDllMachineLearning.linear_remove_model(_model);
	}
}
