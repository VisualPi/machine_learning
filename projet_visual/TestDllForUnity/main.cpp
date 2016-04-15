#include <LinearModel.h>
#include <iostream>

#include <vector>

void PrintModel(MLP_bis* model,int nbLayer)
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

int main()
{

	//double inputs[8] = { 
	//	-1, -1,
	//	 1, -1,
	//	 1,  1,
	//	-1,  1};
	/*double results[4] = { -1, 1, -1, 1 };*/
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

	system("pause");

	return 0;
}