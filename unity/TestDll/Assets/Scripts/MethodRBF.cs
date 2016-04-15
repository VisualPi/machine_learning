using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class MethodRBF : MonoBehaviour {
    [SerializeField]
    private GameObject _boules;
    [SerializeField]
    private GameObject _examples;

    private List<Ball>      _balls;
    private List<Ball>      _exBalls;
    private List<EColor>    _types;
    private System.IntPtr   _model;

    [SerializeField]
    private double _gamma = 0.1;

    void Start() {
        _balls = new List<Ball>();

        for (var i = 0; i < _boules.transform.childCount; ++i)
            _balls.Add(_boules.transform.GetChild(i).GetComponent<Ball>());
    }

    public void StartClassify() {
        _exBalls = new List<Ball>();

        var types = new List<EColor>();
        for (var i = 0; i < _examples.transform.childCount; ++i) {
            types.Add(_examples.transform.GetChild(i).GetChild(0).GetComponent<Ball>().c);
            for (var j = 0; j < _examples.transform.GetChild(i).childCount; ++j)
                _exBalls.Add(_examples.transform.GetChild(i).GetChild(j).GetComponent<Ball>());
        }

        var model = WrapperDllMachineLearning.RBF.CreateModel(_exBalls.Count, 2);

        var inputs = new List<double>();
        var results = new List<double>();
        foreach (var b in _exBalls) {
            inputs.Add(b.transform.position.x);
            inputs.Add(b.transform.position.z);
            results.Add(b.c == types[0] ? -1 : 1);//Si la couleur de la boule est la couleur actuelle tu met -1, sinon 1
        }

        var str = "";
        inputs.ForEach(i => str += ", " + i.ToString());
        Debug.Log(str);
        str = "";
        results.ForEach(i => str += ", " + i.ToString());
        Debug.Log(str);

        WrapperDllMachineLearning.RBF.Fit(model, inputs.ToArray(), 2, inputs.Count, results.ToArray(), _gamma);


        foreach (var b in _balls) {
            var wanted = new double[] { b.transform.position.x, b.transform.position.z };

            var coef = WrapperDllMachineLearning.RBF.Classify(model, wanted, wanted.Length, _gamma);
            if (coef == -1) {
                b.renderer.material = b.GetColor(types[0]);
                b.transform.position = new Vector3(b.transform.position.x, b.GetPosByColor(types[0]),
                    b.transform.position.z);
            } else {
                b.renderer.material = b.GetColor(types[1]);
                b.transform.position = new Vector3(b.transform.position.x, b.GetPosByColor(types[1]),
                    b.transform.position.z);
            }
        }
    }

    public void StartRegression() {
    }

    void OnDestroy() {
        WrapperDllMachineLearning.RBF.DestroyModel(_model, 2);
    }
}
