#include "LinearModel.h"

extern "C"
{
    struct RBF {
        double  W;
        double* X;
        int     nb_parameter;
    };

    DLL_FOR_UNITY_API RBF*      rbf_create_model(int nb_parameter, int nb_example);
    DLL_FOR_UNITY_API double    rbf_classify(RBF * model, double* input, int inputSize, double gamma);
    DLL_FOR_UNITY_API double    rbf_regression(RBF * model, double* input, int inputSize, double gamma);
    DLL_FOR_UNITY_API void      rbf_fit(RBF * model, double* inputs, int modelSize, int inputsSize, double* results, double gamma);
    DLL_FOR_UNITY_API void      LLoyd(double * model, double* inputs, int modelSize, int inputsSize, double* results);
    
    double normeEuclidienne(double* x, double* y, int modelSize);
}