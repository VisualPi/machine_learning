#ifdef DLL_EXPORT
#define DLL_FOR_UNITY_API __declspec(dllexport) 
#else
#define DLL_FOR_UNITY_API __declspec(dllimport) 
#endif

extern "C" 
{
	double alpha = 0.1;
	int iteration = 1000;

	struct MLP
	{
		double**	ptr_layer_to_neuron;
		double**	ptr_input;
		double**	ptr_output;
		double**	ptr_delta;
		int*		input_by_layer;
	};

	struct MLP_bis
	{
		double***	W; //w
		double**	S; //S
		double**	X; //x
		int*		D;//d
	};

	DLL_FOR_UNITY_API double* linear_create_model( int input_dimension );
	DLL_FOR_UNITY_API void linear_remove_model( double* model);
	DLL_FOR_UNITY_API double perceptron_classify( double * model, double* input, int inputSize);
	DLL_FOR_UNITY_API double perceptron_classify_tanh(double * model, double* input, int inputSize);
	DLL_FOR_UNITY_API double perceptron_predict( double * model, double* input, int inputSize );
	DLL_FOR_UNITY_API void perceptron_fit( double * model, double* inputs, int modelSize, int inputsSize, double* results );
	DLL_FOR_UNITY_API void regression_fit( double * model, double* inputs, int modelSize, int inputsSize, double* results );
	DLL_FOR_UNITY_API void classification_hebb( double * model, double* input, int inputSize, double result );


	DLL_FOR_UNITY_API MLP_bis* multilayer_create_model_bis(int* perceptronsByLayer, int nbLayer);
	DLL_FOR_UNITY_API void multilayer_classify_perceptron(MLP_bis* model, double* inputs, int nbLayer);
	DLL_FOR_UNITY_API void multilayer_classify_gradient_backpropagation(MLP_bis* model, double* inputs, int inputSize, int exampleNumber, double* output, int nbLayer, bool useClassify);


	DLL_FOR_UNITY_API double GetAlpha();
	DLL_FOR_UNITY_API void SetAlpha( double value );
	DLL_FOR_UNITY_API int GetIteration();
	DLL_FOR_UNITY_API void SetIteration( int value );
}