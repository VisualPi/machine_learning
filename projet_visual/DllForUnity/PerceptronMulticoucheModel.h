#pragma once
#ifdef DLL_EXPORT
#define DLL_MODE __declspec(dllexport) 
#else
#define DLL_MODE __declspec(dllimport) 
#endif

extern "C"
{
    double  MLP_alpha = 0.1;
    int     MLP_iteration = 4000;

    struct MLP {
        double***	W; //w
        double**	S; //S
        double**	X; //x
        double**	Sigma;
        int*		D; //d
    };

    DLL_MODE MLP*       MLP_CreateModel(int* perceptronsByLayer, int nbLayer);
    DLL_MODE double*    MLP_ClassifyPerceptron(MLP* model, double* inputs, int nbLayer, bool useClassify);
    DLL_MODE void       MLP_ClassifyGradientBackpropagation(MLP* model, double* inputs, int inputSize, int exampleNumber, double* output, int nbLayer, bool useClassify);
    DLL_MODE void       MLP_DestroyModel(MLP* mlp, int nbLayer);

    DLL_MODE double     MLP_GetResultForIndex(MLP* model, int nbLayer, int index);
    DLL_MODE double     MLP_GetAlpha();
    DLL_MODE void       MLP_SetAlpha(double value);
    DLL_MODE int        MLP_GetIteration();
    DLL_MODE void       MLP_SetIteration(int value);
}