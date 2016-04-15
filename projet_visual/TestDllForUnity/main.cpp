#include <LinearModel.h>
#include <iostream>

void PrintModel(MLP_bis* model)
{
	for (int layer = 1; layer < 3; ++layer)
	{
		for (int neuronBegin = 0; neuronBegin < model->D[layer-1]+1; ++neuronBegin)
		{
			for (int neuronEnd = 0; neuronEnd < model->D[layer]; ++neuronEnd)
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

	double inputs[12] = { 0.1, 0.3, 0.8, 0.9, 0.4, 0.7, 0.3, 0.3, 0.8, 0.4, 0.9, 0.2 };
	double results[6] = { -1, 1, 1, -1, 1, 1 };

	int pbl[3] = { 2, 3, 1 };

	double inputTest[2] = { 0.3, 0.3 };

	MLP_bis *model = multilayer_create_model_bis( pbl, 3 );

	PrintModel(model);

	multilayer_classify_gradient_backpropagation(model, inputs, 2, 6, results, 3, true);
	multilayer_classify_perceptron(model, inputTest, 3, true);

	PrintModel(model);

	std::cout << model->S[2][1] << std::endl;

	system("pause");

	return 0;
}