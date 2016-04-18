#include "LinearModel.h"
#include <cstdlib>
#include <cstring>
#include <Eigen\Dense>
#include <iostream>

#include <cmath>
//fait le contenu des slides
//regression -> eijn lib matrice

extern "C"
{
    double* linear_create_model(int input_dimension) {
        double *model = static_cast<double*>(malloc(sizeof(double) * (input_dimension + 1)));
        memset(model, 0, sizeof(double) * (input_dimension + 1));
        return model;
    }

    void linear_remove_model(double* model) {
        if (model)
            delete model;
    }

    double perceptron_classify(double * model, double* input, int inputSize)//oracle
    {
        double out = model[0] * 1;//wo * x0
        for (int i = 0; i < inputSize; ++i)
            out += input[i] * model[i + 1];
        return out > 0.0 ? 1 : -1;//sign

    }

    double perceptron_classify_tanh(double * model, double* input, int inputSize)//oracle
    {
        double out = model[0] * 1;//wo * x0
        for (int i = 0; i < inputSize; ++i)
            out += input[i] * model[i + 1];
        return tanh(out);

    }

    double perceptron_predict(double * model, double* input, int inputSize)//oracle
    {
        double out = model[0] * 1;//wo * x0
        for (int i = 0; i < inputSize; ++i)
            out += input[i] * model[i + 1];
        return out;//sign

    }

    void perceptron_fit(double * model, double* inputs, int modelSize, int inputsSize, double* results) {
        double * input = static_cast<double*>(malloc(sizeof(double) * modelSize));
        for (int i = 0; i < iteration; ++i) {
            int ran = (rand() % (inputsSize / modelSize)) * modelSize;
            for (int j = ran, k = 0; j < ran + modelSize; ++j, ++k)
                input[k] = inputs[j];
            if (perceptron_classify(model, input, modelSize) != results[ran / modelSize]) {
                classification_hebb(model, input, modelSize, results[ran / modelSize]);
            }
        }
    }

    void classification_hebb(double * model, double* input, int inputSize, double result) {
        model[0] += alpha * result * 1;
        for (int i = 0; i < inputSize; ++i) {
            model[i + 1] += alpha * result * input[i];
        }
    }

    void regression_fit(double * model, double* inputs, int modelSize, int inputsSize, double* results) {
        Eigen::MatrixXd X(inputsSize / modelSize, modelSize + 1);
        Eigen::MatrixXd Y(inputsSize / modelSize, 1);
        Eigen::MatrixXd W(1, modelSize + 1);
        for (int i = 0, k = 0; i < inputsSize; i += modelSize, ++k) {
            for (int j = 0; j < modelSize + 1; ++j) {
                if (j == 0)
                    X(k, j) = 1;
                else
                    X(k, j) = inputs[i + j - 1];
            }
        }
        std::cout << "X : " << std::endl << X << std::endl;
        for (int i = 0; i < inputsSize / modelSize; ++i)
            Y(i, 0) = results[i];
        std::cout << "Y : " << std::endl << Y << std::endl;
        Eigen::MatrixXd XT = X;
        XT.transposeInPlace();
        W = XT*X;
        W = W.inverse();
        if (inputsSize / modelSize < modelSize + 1)
            W = XT * W;
        else
            W *= XT;
        W *= Y;
        for (int i = 0; i < modelSize + 1; ++i)
            model[i] = W(i, 0);
        std::cout << "W : " << std::endl << W << std::endl;
    }

    double GetAlpha() {
        return alpha;
    }

    void SetAlpha(double value) {
        alpha = value;
    }

    int GetIteration() {
        return iteration;
    }

    void SetIteration(int value) {
        iteration = value;
    }
}