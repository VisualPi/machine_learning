﻿using UnityEngine;
using System.Collections.Generic;
using System.Runtime.InteropServices;



public class Perceptron : MonoBehaviour
{
	public GameObject boules;
	public GameObject examples;
	private List<Ball> balls;
	private List<Ball> exBalls;

	public Material defaultColor;


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

	private System.IntPtr _model;

	void Start()
	{

	}
	public void StartProcessClassify()
	{
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
			double[] wanted = new double[2] { b.transform.position.x, b.transform.position.z};
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
		regression_fit(_model, inputs.ToArray(), 2, inputs.Count, results.ToArray());
		foreach( var b in balls )
		{
			double[] wanted = new double[2] { b.transform.position.x, b.transform.position.z};
			var coef = (float)perceptron_predict(_model, wanted, 2);
			b.renderer.material.color = new Color(Mathf.Max(0, coef * defaultColor.color.r), 0, Mathf.Max(0, defaultColor.color.b * -coef));
			b.transform.position = new Vector3(b.transform.position.x, (float)perceptron_predict(_model, wanted, 2), b.transform.position.z);
		}
	}

	//MULTICLASSE
	public void StartProcessCLassificationMulticlasse()
	{
		balls = new List<Ball>();
		exBalls = new List<Ball>();
		List<EColor> types = new List<EColor>();
		for( var i = 0 ; i < boules.transform.childCount ; ++i )
		{
			balls.Add(boules.transform.GetChild(i).GetComponent<Ball>());
		}
		for( var i = 0 ; i < examples.transform.childCount ; ++i )
		{
			types.Add(examples.transform.GetChild(i).GetChild(0).GetComponent<Ball>().c);
            for( var j = 0 ; j < examples.transform.GetChild(i).childCount ; ++j )
			{
				exBalls.Add(examples.transform.GetChild(i).GetChild(j).GetComponent<Ball>());

			}
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

		//a voir
		foreach( var b in balls )
		{
			double[] wanted = new double[2] { b.transform.position.x, b.transform.position.z};
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
		//regression moyenne des 3
	}
	public void StartProcessRegressionMulticlasse()
	{
		balls = new List<Ball>();
		exBalls = new List<Ball>();
		List<EColor> types = new List<EColor>();
		for( var i = 0 ; i < boules.transform.childCount ; ++i )
		{
			balls.Add(boules.transform.GetChild(i).GetComponent<Ball>());
		}
		for( var i = 0 ; i < examples.transform.childCount ; ++i )
		{
			types.Add(examples.transform.GetChild(i).GetChild(0).GetComponent<Ball>().c);
			for( var j = 0 ; j < examples.transform.GetChild(i).childCount ; ++j )
			{
				exBalls.Add(examples.transform.GetChild(i).GetChild(j).GetComponent<Ball>());

			}
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

		//a voir
		foreach( var b in balls )
		{
			double[] wanted = new double[2] { b.transform.position.x, b.transform.position.z};
			double coef = 0;
			Color c = new Color();
			//c = new Color (
			//	(1f-(float) perceptron_predict(models[1], wanted, wanted.Length))/2f,
			//	(1f-(float) perceptron_predict(models[2], wanted, wanted.Length))/2f,
			//	(1f-(float) perceptron_predict(models[0], wanted, wanted.Length))/2f
			//	);

			for( var i = 0 ; i < models.Count ; ++i )
			{
				coef = perceptron_predict(models[i], wanted, wanted.Length);
				c += ((1f - (float)coef)/2f) * b.GetColor(types[i]).color;
			}
			//coef /= models.Count;
			b.renderer.material.color = c;
            //b.transform.position = new Vector3(b.transform.position.x, (float)coef, b.transform.position.z);
		}
		//regression moyenne des 3
	}

	public void StartProcessClassificationMulticouche()
	{

	}

	void OnDestroy()
	{
		linear_remove_model(_model);
	}
}