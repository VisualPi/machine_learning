#pragma once
#ifdef _WIN32
	#ifdef DLL_EXPORT
		#define DLL_MODE __declspec(dllexport) 
	#else
		#define DLL_MODE __declspec(dllimport) 
	#endif
#else
	#define DLL_MODE
#endif

extern "C"
{
    struct RBF {
        double  W;
        double* X; // Input value foreach parameter
        int     nb_parameter;
        int     nb_samples;
    };

    DLL_MODE RBF*   RBF_CreateModel(int nb_parameter, int nb_example);
    DLL_MODE void   RBF_DestroyModel(RBF* model, int nb_example);
    DLL_MODE double RBF_Classify(RBF * model, double* input, int inputSize, double gamma);
    DLL_MODE double RBF_Regression(RBF * model, double* input, int inputSize, double gamma);
    DLL_MODE void   RBF_Fit(RBF * model, double* inputs, int modelSize, int inputsSize, double* results, double gamma);
    DLL_MODE void   RBF_LLoyd(double * model, double* inputs, int modelSize, int inputsSize, double* results);
    
    double NormeEuclidienne(double* x, double* y, int modelSize);
}
