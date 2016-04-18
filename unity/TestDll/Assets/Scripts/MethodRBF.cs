using UnityEngine;
using System.Collections.Generic;

public class MethodRBF : MonoBehaviour {
    [SerializeField]
    private GameObject _boules;
    [SerializeField]
    private GameObject _examples;
    [SerializeField]
    private Material _defaultColor;

    private List<Ball>      _balls;
    private List<Ball>      _exBalls;
    private List<EColor>    _types;

    [SerializeField]
    private double _gamma = 0.115;

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
        WrapperDllMachineLearning.RBF.DestroyModel(model, _exBalls.Count);
    }

    public void StartRegression() {
        _exBalls = new List<Ball>();

        foreach (var b in _balls)
            b.renderer.material.color = _defaultColor.color;

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

        WrapperDllMachineLearning.RBF.Fit(model, inputs.ToArray(), 2, inputs.Count, results.ToArray(), _gamma);

        foreach (var b in _balls) {
            var wanted = new double[] { b.transform.position.x, b.transform.position.z };

            var coef = (float)WrapperDllMachineLearning.RBF.Regression(model, wanted, wanted.Length, _gamma);
            b.renderer.material.color = new Color(Mathf.Max(0, coef * _defaultColor.color.r), 0, Mathf.Max(0, _defaultColor.color.b * -coef));
        }
        WrapperDllMachineLearning.RBF.DestroyModel(model, _exBalls.Count);
    }
}
