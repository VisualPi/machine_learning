#include "LinearModel.h"
#include "RBFModel.h"
#include <iostream>
#include <vector>

void PrintModelMLP(MLP_bis* model,int nbLayer)
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

	MLP_bis *model = multilayer_create_model_bis( pbl, nbLayer);

	//PrintModel(model, nbLayer);

	multilayer_classify_gradient_backpropagation(model, &inputs.front(), 784, 1, &output.front(), nbLayer, false);
	double* coucou = multilayer_classify_perceptron(model, &inputs.front(), nbLayer, false);

	//PrintModel(model, nbLayer);

	for (int i = 1; i < 10; ++i)
		std::cout << i << "="<< coucou[i] << std::endl;

	//Destroy_MultiLayer_Perceptron(model, nbLayer);
}

void testRBF() {
    double inputs[102] = { 2, 2, -1, 3, 1, -2, -2, 1, 0, 1, 3, 0, 6, -6, 0, 4, 3, 8, 6, 7, 6, 4, 3, 5, 1, 5, 4, 4, 5, 6, 2, 7, -6, 7, -3, 6, -4, 4, -7, 8, -5, 5, -2, 4, -2, 7, -5, 8, 6, -3, 3, -4, 4, -3, 5, -1, -8, -8, -8, -6, -6, -7, -7, 3, -3, 2, -4, -6, -4, -9, -2, -9, -3, -7, -3, -4, -4, 0, 2, -3, 0, -8, 0, -3, 4, -6, 5, -8, 6, -9, -6, -2, -8, -3, 0, -6, -3, -2, -8, -1, -8, -1 };
    double results[51] = { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
    //double inputs[8] = { 0, 0, 0, 1, 1, 0, 1, 1 };
    //double results[4] = { -1, 1, 1, -1 };
    RBF *model = rbf_create_model(51, 2);
    PrintModelRBF(model, 51, 2);

    rbf_fit(model, inputs, 2, 102, results, 0.1);
    for (int i = 0; i < 51; i++) {
        std::cout << rbf_classify(model, &inputs[i * 2], 2, 0.1)<< std::endl;
    }

    PrintModelRBF(model, 51, 2);

    rbf_remove_model(model, 51);
}

int main() {
    testRBF();
    system("pause");

    return 0;
}