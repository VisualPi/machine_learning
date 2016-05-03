#include "LinearModel.h"
#include "RBFModel.h"
#include "PerceptronMulticoucheModel.h"
#include "MNIST.hpp"
#include <iostream>
#include <vector>

void PrintModelMLP(MLP* model,int nbLayer)
{
	for (int layer = 1; layer < nbLayer; ++layer)
	{
		for (int neuronBegin = 0; neuronBegin <= model->D[layer-1]; ++neuronBegin)
		{
			for (int neuronEnd = 1; neuronEnd <= model->D[layer]; ++neuronEnd)
			{
				std::cout << "Layer : " << layer << " Neuron Start : " << neuronBegin << " Neuron End : " << neuronEnd << " w value : " <<
					model->W[layer][neuronBegin][neuronEnd] << std::endl;
			}
		}
	}

	std::cout << std::endl;
	std::cout << std::endl;
}

void PrintModelRBF(RBF* model, int nbExample, int nbParameter) {
    for (int i = 0; i < nbExample; ++i) {
        std::cout << "model[" << i << "].W = " << model[i].W << ", model[" << i << "].X = {";
        for (int j = 0; j < nbParameter; ++j) { std::cout << model[i].X[j] << ","; }
        std::cout << "}" << std::endl;
    }
    std::cout << std::endl;
    std::cout << std::endl;
}

void testMLP() {
	// double inputs[8] = { 
	//	-1, -1,
	//	 1, -1,
	//	 1,  1,
	//	-1,  1};
	// double results[4] = { -1, 1, -1, 1 };
	double results[1] = { -1 };

	int pbl[5] = { 784, 10, 10  };
	int nbLayer = 3;

	std::vector<double> inputs;
	inputs.reserve(784);
	for (int i = 0; i < 784; ++i)
		inputs.push_back(rand() % 255);

	std::vector<double> output;
	output.reserve(10);
	for (int i = 0; i < 10; ++i)
		output.push_back(-1);

	int t = rand() % 10;
	output[t] = 1;

	//double inputTest[2] = { 1, -1 }; //doit sortir 1

	MLP *model = MLP_CreateModel( pbl, nbLayer);

	//PrintModelMLP(model, nbLayer);

    MLP_ClassifyGradientBackpropagation(model, &inputs.front(), 784, 1, &output.front(), nbLayer, false);
	double* coucou = MLP_ClassifyPerceptron(model, &inputs.front(), nbLayer, false);

	//PrintModelMLP(model, nbLayer);

	for (int i = 1; i < 10; ++i)
		std::cout << i << "="<< coucou[i] << std::endl;

    MLP_DestroyModel(model, nbLayer);
}

void testRBF() {
    double inputs[101] = { 2, 2, -1, 3, 1, -2, -2, 1, 0, 1, 3, 0, 6, -6, 0, 4, 3, 8, 6, 7, 6, 4, 3, 5, 1, 5, 4, 4, 5, 6, 2, 7, -6, 7, -3, 6, -4, 4, -7, 8, -5, 5, -2, 4, -2, 7, -5, 8, 6, -3, 3, -4, 4, -3, 5, -1, -8, -8, -8, -6, -6, -7, -7, 3, -3, 2, -4, -6, -4, -9, -2, -9, -3, -7, -3, -4, -4, 0, 2, -3, 0, -8, 0, -3, 4, -6, 5, -8, 6, -9, -6, -2, -8, -3, 0, -6, -3, -2, -8, -1 };
    double results[50] = { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
    //double inputs[8] = { 0, 0, 0, 1, 1, 0, 1, 1 };
    //double results[4] = { -1, 1, 1, -1 };
    RBF *model = RBF_CreateModel(51, 2);
    PrintModelRBF(model, 51, 2);

    RBF_Fit(model, inputs, 2, 102, results, 0.1);
    for (int i = 0; i < 51; i++)
        std::cout << RBF_Classify(model, &inputs[i * 2], 2, 0.1) << std::endl;

    PrintModelRBF(model, 51, 2);

    RBF_DestroyModel(model, 51);
}

void printDataSet(MNIST mnist)
{
	for (auto it = mnist.images.cbegin(); it != mnist.images.cend(); ++it)
	{
		for (int i = 0; i < (*it)->rows; ++i)
		{
			for (int j = 0; j < (*it)->cols; ++j)
				std::cout << (*it)->pixels[i][j];
			std::cout << std::endl;
		}
		std::cout << (*it)->label << std::endl;
		std::getchar();
	}
}

void testMNIST()
{
	MNIST mnist;
	mnist.read_mnist("D:\\ESGI\\5A\\machine_learning\\git\\trunk\\unity\\TestDll\\Assets\\MNIST",//path
		"t10k-images.idx3-ubyte", //nom fichier image
		"t10k-labels.idx1-ubyte" //nom fichier labels
	);
	//printDataSet(mnist);
	double results[1] = { -1 };

	int pbl[10] = { 784, 600, 500, 400, 300, 200 ,100 , 50, 25, 10 };
	//int pbl[5] = { 784, 50, 30, 10, 5 };
	SetIteration(4000);
	int nbLayer = 10;

	std::vector<double> inputs;
	inputs.reserve(mnist.images.size() * (mnist.images[0]->rows*mnist.images[0]->cols));
	for (int i = 0; i < mnist.images.size(); ++i)
		for (int j = 0; j < mnist.images[i]->rows; ++j)
			for (int k = 0; k < mnist.images[i]->cols; ++k)
				inputs.push_back(mnist.images[i]->pixels[j][k]);

	std::vector<double> output;
	output.reserve(mnist.images.size());
	for (int i = 0; i < mnist.images.size(); ++i)
		output.push_back(mnist.images[i]->label);

	MLP *model = MLP_CreateModel(pbl, nbLayer);

	//PrintModelMLP(model, nbLayer);
	MLP_ClassifyGradientBackpropagation(model, &inputs.front(), 784, 10000, &output.front(), nbLayer, true);
	std::vector<double> test
	{
		0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
		0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
		0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
		0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
		0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
		0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
		0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
		0,0,0,0,0,0,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
		0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,
		0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,
		0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,
		0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,
		0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,
		0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,
		0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,
		0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,
		0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,
		0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,
		0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0,
		0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,
		0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,
		0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,
		0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,
		0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,
		0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,
		0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,
		0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
		0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
	};
	double* coucou = MLP_ClassifyPerceptron(model, &test.front(), nbLayer, true);

	//PrintModelMLP(model, nbLayer);

	for (int i = 1; i < 10; ++i)
		std::cout << i << "=" << coucou[i] << std::endl;
}

int main() {
	testRBF();
	//testMNIST();
	std::getchar();
	return 0;
}