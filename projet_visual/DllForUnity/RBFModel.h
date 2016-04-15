#pragma once
#ifdef DLL_EXPORT
#define DLL_MODE __declspec(dllexport) 
#else
#define DLL_MODE __declspec(dllimport) 
#endif

extern "C"
{
    struct RBF {
        double  W;
        double* X;
        int     nb_parameter;
    };

    DLL_MODE RBF*      rbf_create_model(int nb_parameter, int nb_example);
    DLL_MODE void      rbf_remove_model(RBF*);
    DLL_MODE double    rbf_classify(RBF * model, double* input, int inputSize, double gamma);
    DLL_MODE double    rbf_regression(RBF * model, double* input, int inputSize, double gamma);
    DLL_MODE void      rbf_fit(RBF * model, double* inputs, int modelSize, int inputsSize, double* results, double gamma);
    DLL_MODE void      LLoyd(double * model, double* inputs, int modelSize, int inputsSize, double* results);
    
    double normeEuclidienne(double* x, double* y, int modelSize);
}