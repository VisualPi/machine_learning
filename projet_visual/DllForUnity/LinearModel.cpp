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

	//MLP* multilayer_create_model( int inputByLayer, int* perceptronsByLayer, int nbLayer )
	//{
	//	MLP* model = new MLP();
	//	model->ptr_layer_to_neuron = (double**) malloc( sizeof( double ) * nbLayer );
	//	model->ptr_input = (double**) malloc( sizeof( double ) * nbLayer );
	//	model->ptr_output = (double**) malloc( sizeof( double ) * nbLayer );
	//	model->ptr_delta = (double**) malloc( sizeof( double ) * nbLayer );
	//	model->input_by_layer = (int*) malloc( sizeof( int ) * nbLayer );
	//	int capacity = perceptronsByLayer[0] * ( inputByLayer + 1 );
	//	int input = inputByLayer + 1;
	//	for ( int i = 0 ; i < nbLayer ; ++i )
	//	{
	//		model->ptr_layer_to_neuron[i] = (double*) malloc( sizeof( double ) * capacity );
	//		memset( model->ptr_layer_to_neuron[i], 0, sizeof( double ) * ( capacity ) );
	//		model->ptr_input[i] = (double*) malloc( sizeof( double ) * input);
	//		memset(model->ptr_input[i], 0, sizeof(double) * (capacity));
	//		model->ptr_output[i] = (double*) malloc( sizeof( double ) * perceptronsByLayer[i] );
	//		memset(model->ptr_output[i], 0, sizeof(double) * perceptronsByLayer[i]);
	//		model->ptr_delta[i] = (double*) malloc( sizeof( double ) * perceptronsByLayer[i] );
	//		memset(model->ptr_delta[i], 0, sizeof(double) * perceptronsByLayer[i]);
	//		model->input_by_layer[i] = capacity;

	//		input = perceptronsByLayer[i];
	//		if(i < nbLayer -1 )
	//			capacity = perceptronsByLayer[i+1] * ( perceptronsByLayer[i] + 1 );
	//	}


	//	return model;
	//}

	double GetRandomPointInRange(double min, double max)
	{
		double f = (double)rand() / RAND_MAX;
		return min + f * (max - min);
	}

	MLP_bis* multilayer_create_model_bis(int* perceptronsByLayer, int nbLayer)
	{
		MLP_bis* model = (MLP_bis*)malloc(sizeof(MLP_bis));
		model->W = (double***) malloc( sizeof( double ) * (nbLayer));
		model->S = (double**) malloc( sizeof( double ) * nbLayer );
		model->Sigma = (double**)malloc(sizeof(double) * nbLayer);
		model->X = (double**) malloc( sizeof( double ) * nbLayer );
		//model->D = (int*)malloc(sizeof(double) * nbLayer);

		model->D = perceptronsByLayer;
		model->W[0] = nullptr;

		for (int i = 1; i < nbLayer; ++i)
		{
			model->W[i] = (double**)malloc(sizeof(double) * (model->D[i-1]+1));

			for (int j = 0; j <= model->D[i-1]; ++j)
			{
				model->W[i][j] = (double*)malloc(sizeof(double) * model->D[i]+1);
				for (int k = 0; k <= model->D[i]; ++k)
				{
					model->W[i][j][k] = GetRandomPointInRange(-1.0, 3.0);
				}
			}
		}

		for (int i = 0; i < nbLayer; ++i)
		{
			model->Sigma[i] = (double*)malloc(sizeof(double) * (model->D[i] + 1));
			memset(model->Sigma[i], 0, sizeof(double) * model->D[i]+1);

			model->S[i] = (double*)malloc(sizeof(double) * (model->D[i]+1));
			memset(model->S[i], 0, sizeof(double) * model->D[i] +1 );
			model->S[i][0] = 1;

			model->X[i] = (double*)malloc(sizeof(double) * (model->D[i]+1));
			memset(model->X[i], 0, sizeof(double) * model->D[i]+1);
			model->X[i][0] = 1;
		}

		return model;
	}

	double* multilayer_classify_perceptron( MLP_bis* model, double* inputs, int nbLayer, bool useClassify )
	{
		for (int i = 1; i <= model->D[0]; ++i)
			model->X[0][i] = inputs[i-1];
		
		for (int l = 1; l <= nbLayer; ++l)
		{
			for (int j = 1; j <= model->D[l]; ++j)
			{
				model->S[l][j] = 0;
				for (int k = 0; k <= model->D[l-1]; ++k)
					model->S[l][j] += model->W[l][k][j] * model->X[l - 1][k];

				if (useClassify || l < nbLayer - 1)
					model->X[l][j] = tanh(model->S[l][j]);
				else
					model->X[l][j] = model->S[l][j];
			}
		}

		return model->X[nbLayer - 1];
	}

	void multilayer_classify_gradient_backpropagation(MLP_bis* model, double* inputs, int inputSize, int exampleNumberCount, double* output, int nbLayer, bool useClassify)
	{
		for (int it = 0; it < iteration; ++it)
		{
			int exampleNumber = rand() % exampleNumberCount;

			multilayer_classify_perceptron(model, inputs + exampleNumber * inputSize, nbLayer, useClassify);

			for (int lastLayerNeuronNumber = 1; lastLayerNeuronNumber <= model->D[nbLayer - 1]; ++lastLayerNeuronNumber) //BIAS ?
			{
				double val = model->X[nbLayer - 1][lastLayerNeuronNumber];

				if (!useClassify)
					model->Sigma[nbLayer - 1][lastLayerNeuronNumber] = val - output[exampleNumber + (lastLayerNeuronNumber - 1)];
				else
					model->Sigma[nbLayer - 1][lastLayerNeuronNumber] = ((1.0 - (val * val)) * (val - output[exampleNumber + (lastLayerNeuronNumber-1)]));
			}

			for (int l = nbLayer - 2; l > 0; --l)
			{
				for (int i = 1; i <= model->D[l]; ++i) //BIAS ?
				{
					double sum = 0;
					for (int j = 1;j <= model->D[l+1]; ++j) //BIAS ?
					{
						sum += model->W[l+1][i][j] * model->Sigma[l+1][j];
					}
					model->Sigma[l][i] = (1.0 - (model->X[l][i] * model->X[l][i])) * sum;
				}
			}

			for (int layer = 1; layer < nbLayer; ++layer)
			{
				for (int start = 0; start <= model->D[layer-1]; ++start) //BIAS //I
				{
					for (int end = 1; end <= model->D[layer]; ++end) //BIAS //J
					{
						model->W[layer][start][end] -=  alpha * model->X[layer-1][start] * model->Sigma[layer][end];
					}
				}
			}
		}
	}
}
void Destroy_MultiLayer_Perceptron(MLP_bis* mlp, int nbLayer)
{

	for (int i = 0; i < nbLayer; ++i)
	{
		if(mlp->W[i] != nullptr)
		{ 
			for (int j = 0; j <= mlp->D[i]; j++)
			{
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