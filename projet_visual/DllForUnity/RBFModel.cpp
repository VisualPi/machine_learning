#include "RBFModel.h"
#include <cstdlib>
#include <cstring>
#include <Eigen\Dense>
#include <iostream>

#include <cmath>

extern "C"
{
    /// <summary>Create model of type RBF.</summary>
    RBF* rbf_create_model(int nbExample, int nbParameter) {
        RBF* model;

        model = (RBF*) malloc(sizeof(RBF) * nbExample);
        for (int i = 0; i < nbExample; i++) {
            model[i].W = 0;
            model[i].X = (double*) malloc(sizeof(double) * nbParameter);
            for (int j = 0; j < nbParameter; j++)
                model[i].X[j] = 0;
            model[i].nb_parameter = nbParameter;
            model[i].nb_samples = nbExample;
        }

        return model;
    }

    /// <summary>Remove model of type RBF.</summary>
    void rbf_remove_model(RBF* model, int nbExample) {
        if (model) {
            for (int i = 0; i < nbExample; ++i)
                if (model[i].X)
                    delete[] model[i].X;

            delete[] model;
        }
    }

    /// <summary>Oracle of classify for RBF methode.</summary>
    double rbf_classify(RBF * model, double* input, int inputSize, double gamma) {
        return rbf_regression(model, input, inputSize, gamma) > 0.0 ? 1 : -1; // sign
    }

    /// <summary>Oracle of prediction for RBF methode.</summary>
    double rbf_regression(RBF * model, double* input, int inputSize, double gamma) {
        double out = 0;
        for (int i = 0; i < model->nb_samples; ++i)
            out += model[i].W * exp(-gamma*normeEuclidienne(input, model[i].X, inputSize));
        return out;
    }

    /// <summary>Traine model for RBF methode.</summary>
    void rbf_fit(RBF * model, double* inputs, int modelSize, int inputsSize, double* results, double gamma) {
        gamma = -gamma;
        int nbInput = inputsSize / modelSize;

        Eigen::MatrixXd Y(nbInput, 1);
        Eigen::MatrixXd W(nbInput, nbInput);

        // Init matrice Y
        for (int i = 0; i < nbInput; ++i)
            Y(i, 0) = results[i];

        // Init matrice W
        for (int i = 0; i < nbInput; ++i) {
            // All diagonal is set to : 1
            W(i, i) = 1;
            for (int j = i + 1; j < nbInput; ++j)
                // The matrice is symmetrical
                W(j, i) = W(i, j) = exp(gamma*normeEuclidienne(inputs + modelSize*i, inputs + modelSize*j, modelSize));
        }
        W = W.inverse()*Y;
        std::cout << W << std::endl;

        // Set model
        for (int i = 0; i < nbInput; ++i) {
            model[i].W = W(i, 0);
            for (int j = 0; j < modelSize; ++j)
                model[i].X[j] = inputs[i*modelSize + j];
        }
    }

    double normeEuclidienne(double* x, double* y, int modelSize) {
        double sum = 0;
        for (int i = 0; i < modelSize; ++i)
            sum += (y[i] - x[i])*(y[i] - x[i]);

        return sum;
    }
}