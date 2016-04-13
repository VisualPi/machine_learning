#ifdef DLL_EXPORT
#define DLL_FOR_UNITY_API __declspec(dllexport) 
#else
#define DLL_FOR_UNITY_API __declspec(dllimport) 
#endif

extern "C" 
{
	const double alpha = 0.1;
	const int iteration = 1000;

	struct MLP
	{
		double**	ptr_layer_to_neuron;
		double**	ptr_input;
		double**	ptr_output;
		double**	ptr_delta;
		int*		input_by_layer;
	};

	DLL_FOR_UNITY_API double* linear_create_model( int input_dimension );
	DLL_FOR_UNITY_API void linear_remove_model( double* model);
	DLL_FOR_UNITY_API int perceptron_classify( double * model, double* input, int inputSize );
	DLL_FOR_UNITY_API double perceptron_predict( double * model, double* input, int inputSize );
	DLL_FOR_UNITY_API void perceptron_fit( double * model, double* inputs, int modelSize, int inputsSize, double* results );
	DLL_FOR_UNITY_API void regression_fit( double * model, double* inputs, int modelSize, int inputsSize, double* results );
	DLL_FOR_UNITY_API void classification_hebb( double * model, double* input, int inputSize, double result );

	DLL_FOR_UNITY_API MLP* multilayer_create_model( int inputByLayer, int* perceptronsByLayer, int nbLayer );
	DLL_FOR_UNITY_API double* multilayer_classify_perceptron( double* model, double* inputs, int inputSize, int* perceptronsByLayer, int nbLayer);
}