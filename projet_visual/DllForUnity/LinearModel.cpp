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
	

	double* linear_create_model( int input_dimension )
	{
		double *model = (double*) malloc( sizeof( double ) * ( input_dimension + 1 ) );
		memset( model, 0, sizeof( double ) * ( input_dimension + 1 ) );
		return model;
	}
	void linear_remove_model( double* model )
	{
		if ( model )
			delete model;
	}
	double perceptron_classify(double * model, double* input, int inputSize)//oracle
	{
		double out = model[0] * 1;//wo * x0
		for (int i = 0; i < inputSize; ++i)
			out += input[i] * model[i + 1];
		return out > 0.0 ? 1 : -1;//sign

	}
	double perceptron_classify_tanh( double * model, double* input, int inputSize )//oracle
	{
		double out = model[0] * 1;//wo * x0
		for ( int i = 0 ; i < inputSize; ++i )
			out += input[i] * model[i + 1];
		return tanh(out);

	}
	double perceptron_predict( double * model, double* input, int inputSize )//oracle
	{
		double out = model[0] * 1;//wo * x0
		for ( int i = 0 ; i < inputSize; ++i )
			out += input[i] * model[i + 1];
		return out;//sign

	}
	void perceptron_fit( double * model, double* inputs, int modelSize, int inputsSize, double* results )
	{
		double * input = (double*) malloc( sizeof( double ) * modelSize );
		for ( int i = 0 ; i < iteration ; ++i )
		{
			int ran = ( rand() % ( inputsSize / modelSize ) ) * modelSize;
			for ( int j = ran, k = 0 ; j < ran + modelSize ; ++j, ++k )
				input[k] = inputs[j];
			if ( perceptron_classify( model, input, modelSize ) != results[ran / modelSize] )
			{
				classification_hebb( model, input, modelSize, results[ran / modelSize] );
			}
		}
	}
	void classification_hebb( double * model, double* input, int inputSize, double result )
	{
		model[0] += alpha * result * 1;
		for ( int i = 0 ; i < inputSize ; ++i )
		{
			model[i + 1] += alpha * result * input[i];
		}
	}
	void regression_fit( double * model, double* inputs, int modelSize, int inputsSize, double* results )
	{
		Eigen::MatrixXd X( inputsSize / modelSize, modelSize + 1 );
		Eigen::MatrixXd Y( inputsSize / modelSize, 1 );
		Eigen::MatrixXd W( 1, modelSize + 1 );
		for ( int i = 0, k = 0; i < inputsSize ; i += modelSize, ++k )
		{
			for ( int j = 0 ; j < modelSize + 1 ; ++j )
			{
				if ( j == 0 )
					X( k, j ) = 1;
				else
					X( k, j ) = inputs[i + j - 1];
			}
		}
		std::cout << "X : " << std::endl << X << std::endl;
		for ( int i = 0 ; i < inputsSize / modelSize ; ++i )
			Y( i, 0 ) = results[i];
		std::cout << "Y : " << std::endl << Y << std::endl;
		Eigen::MatrixXd XT = X;
		XT.transposeInPlace();
		W = XT*X;
		W = W.inverse();
		if ( inputsSize / modelSize < modelSize + 1 )
			W = XT * W;
		else
			W *= XT;
		W *= Y;
		for ( int i = 0; i < modelSize + 1 ; ++i )
			model[i] = W( i, 0 );
		std::cout << "W : " << std::endl << W << std::endl;
	}
	double linear_predict( double * model, double* inputs, int modelSize )
	{
		return 0;
	}

	//mutlicouche

	MLP* multilayer_create_model( int inputByLayer, int* perceptronsByLayer, int nbLayer )
	{
		MLP* model = new MLP();
		model->ptr_layer_to_neuron = (double**) malloc( sizeof( double ) * nbLayer );
		model->ptr_input = (double**) malloc( sizeof( double ) * nbLayer );
		model->ptr_output = (double**) malloc( sizeof( double ) * nbLayer );
		model->ptr_delta = (double**) malloc( sizeof( double ) * nbLayer );
		model->input_by_layer = (int*) malloc( sizeof( int ) * nbLayer );
		int capacity = perceptronsByLayer[0] * ( inputByLayer + 1 );
		int input = inputByLayer + 1;
		for ( int i = 0 ; i < nbLayer ; ++i )
		{
			model->ptr_layer_to_neuron[i] = (double*) malloc( sizeof( double ) * capacity );
			memset( model->ptr_layer_to_neuron[i], 0, sizeof( double ) * ( capacity ) );
			model->ptr_input[i] = (double*) malloc( sizeof( double ) * input);
			memset(model->ptr_input[i], 0, sizeof(double) * (capacity));
			model->ptr_output[i] = (double*) malloc( sizeof( double ) * perceptronsByLayer[i] );
			memset(model->ptr_output[i], 0, sizeof(double) * perceptronsByLayer[i]);
			model->ptr_delta[i] = (double*) malloc( sizeof( double ) * perceptronsByLayer[i] );
			memset(model->ptr_delta[i], 0, sizeof(double) * perceptronsByLayer[i]);
			model->input_by_layer[i] = capacity;

			input = perceptronsByLayer[i];
			if(i < nbLayer -1 )
				capacity = perceptronsByLayer[i+1] * ( perceptronsByLayer[i] + 1 );
		}


		return model;
	}

	//MLP_bis* multilayer_create_model(int inputByLayer, int* perceptronsByLayer, int nbLayer)
	//{
	//	MLP_bis* model = new MLP_bis();
	//	model->layer_weight = (double***) malloc( sizeof( double ) * nbLayer );
	//	model->ponderate_sum = (double**) malloc( sizeof( double ) * nbLayer );
	//	model->return_value = (double**) malloc( sizeof( double ) * nbLayer );
	//	model->neuron_number = (int**)malloc(sizeof(double) * nbLayer);

	//	int input_Number = inputByLayer +1 ;
	//	int numberPerceptron = perceptronsByLayer[1];

	//	for (int i = 0; i < nbLayer; ++i)
	//	{
	//		model->layer_weight[i] = (double**)malloc(sizeof(double) * input_Number);

	//		for (int j = 0; j < input_Number; ++j)
	//		{
	//			model->layer_weight[i][j] = (double*)malloc(sizeof(double) * numberPerceptron);
	//			memset(model->layer_weight[i][j], 0, sizeof(double) * numberPerceptron);
	//		}

	//		model->ponderate_sum[i] = (double*)malloc(sizeof(double) * perceptronsByLayer[i]);
	//		memset(model->ponderate_sum[i], 0, sizeof(double) * perceptronsByLayer[i]);

	//		model->return_value[i] = (double*)malloc(sizeof(double) * perceptronsByLayer[i]);
	//		memset(model->return_value[i], 0, sizeof(double) * perceptronsByLayer[i]);

	//		model->neuron_number[i] = (int*)malloc(sizeof(int) * perceptronsByLayer[i]);
	//		memset(model->neuron_number[i], 0, sizeof(double) * perceptronsByLayer[i]);

	//		input_Number = perceptronsByLayer[i] + 1;

	//		if(i+1 == nbLayer-1)
	//			numberPerceptron
	//	}

	//	return model;
	//}

	double multilayer_classify_perceptron( MLP* model, double* inputs, int inputSize, int nbLayer )
	{
		//Pour l allant de 0 a LMax
		//Pour chaque neurone de la couche L
		//Get Neurone j de la couche l
		//Classify du neurone avec pour input (si couche = 0 : input fonction)
		//sinon input de l-1
		//return tanh du dernier neurone

		for (int i = 0; i < model->input_by_layer[0]; ++i)
		{
			model->ptr_input[0][i] = inputs[i];
		}

		for (int i = 1; i < nbLayer; ++i)
		{
			for (int j = 0; j < model->input_by_layer[i]; ++j)
			{
				 model->ptr_output[i][j] = perceptron_classify_tanh(model->ptr_layer_to_neuron[i] + j, model->ptr_input[i] + j, inputSize);
				 if(i < nbLayer-1)
					model->ptr_input[i + 1][j] = model->ptr_output[i][j];
			}
		}

		int last_index = model->input_by_layer[nbLayer - 1];

		return model->ptr_input[nbLayer - 1][0];
	}

	void multilayer_classify_gradient_backpropagation(MLP* model, double* inputs, int inputSize, int inputNumber, int* output, int nbLayer)
	{
		double* input = new double[inputSize];
		for (int i = 0; i < iteration; ++i)
		{
			int ran = (rand() % (inputNumber / inputSize)) * inputSize;
			for (int j = ran, k = 0; j < ran + inputNumber; ++j, ++k)
				input[k] = inputs[j];

			int last_index = model->input_by_layer[nbLayer - 1];
			model->ptr_input[nbLayer - 1][last_index - 1] = multilayer_classify_perceptron(model, input, inputSize, true);

			if (model->ptr_input[nbLayer - 1][last_index - 1] != output[ran / inputSize])
			{
				//pour tout les neurones de la dernière couche : 
				//(( 1 - model->ptr_input[nbLayer - 1][last_index - 1] * model->ptr_input[nbLayer - 1][last_index - 1]) * (model->ptr_input[nbLayer - 1][last_index - 1] * output[ran / inputSize])

				//pour tout les autres neurones : 
				//
			}
		}
	}
}

double GetAlpha()
{
	return alpha;
}
void SetAlpha( double value )
{
	alpha = value;
}
int GetIteration()
{
	return iteration;
}
void SetIteration( int value )
{
	iteration = value;
}