#include "PerceptronMulticoucheModel.h"
#include <cstdlib>
#include <cstring>

#include <cmath>

double GetRandomPointInRange(double min, double max) {
    return min + (static_cast<double>(rand()) / RAND_MAX) * (max - min);
}

MLP* MLP_CreateModel(int* perceptronsByLayer, int nbLayer) {
    MLP* model = static_cast<MLP*>     (malloc(sizeof(MLP)));
    model->W = static_cast<double***>(malloc(sizeof(double) * nbLayer));
    model->S = static_cast<double**> (malloc(sizeof(double) * nbLayer));
    model->Sigma = static_cast<double**> (malloc(sizeof(double) * nbLayer));
    model->X = static_cast<double**> (malloc(sizeof(double) * nbLayer));

    model->D = perceptronsByLayer;
    model->W[0] = nullptr;

    for (int i = 1; i < nbLayer; ++i) {
        model->W[i] = static_cast<double**>(malloc(sizeof(double) * (model->D[i - 1] + 1)));

        for (int j = 0; j <= model->D[i - 1]; ++j) {
            model->W[i][j] = static_cast<double*>(malloc(sizeof(double) * model->D[i] + 1));
            for (int k = 0; k <= model->D[i]; ++k) {
                model->W[i][j][k] = GetRandomPointInRange(-1.0, 3.0);
            }
        }
    }

    for (int i = 0; i < nbLayer; ++i) {
        model->Sigma[i] = static_cast<double*>(malloc(sizeof(double) * (model->D[i] + 1)));
        memset(model->Sigma[i], 0, sizeof(double) * model->D[i] + 1);

        model->S[i] = static_cast<double*>(malloc(sizeof(double) * (model->D[i] + 1)));
        memset(model->S[i], 0, sizeof(double) * model->D[i] + 1);
        model->S[i][0] = 1;

        model->X[i] = static_cast<double*>(malloc(sizeof(double) * (model->D[i] + 1)));
        memset(model->X[i], 0, sizeof(double) * model->D[i] + 1);
        model->X[i][0] = 1;
    }

    return model;
}

double* MLP_ClassifyPerceptron(MLP* model, double* inputs, int nbLayer, bool useClassify) {
    for (int i = 1; i <= model->D[0]; ++i)
        model->X[0][i] = inputs[i - 1];

    for (int l = 1; l <= nbLayer; ++l) {
        for (int j = 1; j <= model->D[l]; ++j) {
            model->S[l][j] = 0;
            for (int k = 0; k <= model->D[l - 1]; ++k)
                model->S[l][j] += model->W[l][k][j] * model->X[l - 1][k];

            if (useClassify || l < nbLayer - 1)
                model->X[l][j] = tanh(model->S[l][j]);
            else
                model->X[l][j] = model->S[l][j];
        }
    }

    return model->X[nbLayer - 1];
}

void MLP_ClassifyGradientBackpropagation(MLP* model, double* inputs, int inputSize, int exampleNumberCount, double* output, int nbLayer, bool useClassify) {
    for (int it = 0; it < MLP_iteration; ++it) {
        int exampleNumber = rand() % exampleNumberCount;

        MLP_ClassifyPerceptron(model, inputs + exampleNumber * inputSize, nbLayer, useClassify);

        for (int lastLayerNeuronNumber = 1; lastLayerNeuronNumber <= model->D[nbLayer - 1]; ++lastLayerNeuronNumber) //BIAS ?
        {
            double val = model->X[nbLayer - 1][lastLayerNeuronNumber];

            if (!useClassify)
                model->Sigma[nbLayer - 1][lastLayerNeuronNumber] = val - output[exampleNumber + (lastLayerNeuronNumber - 1)];
            else
                model->Sigma[nbLayer - 1][lastLayerNeuronNumber] = ((1.0 - (val * val)) * (val - output[exampleNumber + (lastLayerNeuronNumber - 1)]));
        }

        for (int l = nbLayer - 2; l > 0; --l) {
            for (int i = 1; i <= model->D[l]; ++i) //BIAS ?
            {
                double sum = 0;
                for (int j = 1; j <= model->D[l + 1]; ++j) //BIAS ?
                {
                    sum += model->W[l + 1][i][j] * model->Sigma[l + 1][j];
                }
                model->Sigma[l][i] = (1.0 - (model->X[l][i] * model->X[l][i])) * sum;
            }
        }

        for (int layer = 1; layer < nbLayer; ++layer) {
            for (int start = 0; start <= model->D[layer - 1]; ++start) //BIAS //I
            {
                for (int end = 1; end <= model->D[layer]; ++end) //BIAS //J
                {
                    model->W[layer][start][end] -= MLP_alpha * model->X[layer - 1][start] * model->Sigma[layer][end];
                }
            }
        }
    }
}

void MLP_DestroyModel(MLP* mlp, int nbLayer) {
    for (int i = 0; i < nbLayer; ++i) {
        if (mlp->W[i] != nullptr) {
            for (int j = 0; j <= mlp->D[i]; j++) {
                free(mlp->W[i][j]);
            }
        }
        free(mlp->W[i]);
        free(mlp->S[i]);
        free(mlp->X[i]);
        free(mlp->Sigma[i]);
    }

    free(mlp->W);
    free(mlp->S);
    free(mlp->X);
    free(mlp->D);
    free(mlp->Sigma);
    free(mlp);
}

double MLP_GetResultForIndex(MLP* model, int nbLayer, int index) {
    return model->X[nbLayer - 1][index];
}

double MLP_GetAlpha() {
    return MLP_alpha;
}

void MLP_SetAlpha(double value) {
    MLP_alpha = value;
}

int MLP_GetIteration() {
    return MLP_iteration;
}

void MLP_SetIteration(int value) {
    MLP_iteration = value;
}