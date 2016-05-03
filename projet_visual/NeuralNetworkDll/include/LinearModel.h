#pragma once
#ifdef DLL_EXPORT
#define DLL_MODE __declspec(dllexport) 
#else
#define DLL_MODE __declspec(dllimport) 
#endif


extern "C" 
{
	double alpha = 0.1;
	int iteration = 4000;

	DLL_MODE double*    linear_create_model( int input_dimension );
	DLL_MODE void       linear_remove_model( double* model);
	DLL_MODE double     perceptron_classify( double * model, double* input, int inputSize);
	DLL_MODE double     perceptron_classify_tanh(double * model, double* input, int inputSize);
	DLL_MODE double     perceptron_predict( double * model, double* input, int inputSize );
	DLL_MODE void       perceptron_fit( double * model, double* inputs, int modelSize, int inputsSize, double* results );
	DLL_MODE void       regression_fit( double * model, double* inputs, int modelSize, int inputsSize, double* results );
	DLL_MODE void       classification_hebb( double * model, double* input, int inputSize, double result );

	DLL_MODE double GetAlpha();
	DLL_MODE void   SetAlpha( double value );
	DLL_MODE int    GetIteration();
	DLL_MODE void   SetIteration( int value );
}