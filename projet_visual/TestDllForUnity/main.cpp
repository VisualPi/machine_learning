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

	double inputs[12] = { 1, 6, 5, 9, 3, 7, 6, 3, 8, 4, 9, 2 };
	double results[6] = { -1, -1, -1, 1, 1, 1 };

	int pbl[3] = { 2, 5, 1 };

	double inputTest[2] = { 0.5, 0.3 };

	MLP_bis *model = multilayer_create_model_bis( pbl, 3 );

	PrintModel(model);

	multilayer_classify_gradient_backpropagation(model, inputs, 2, 6, results, 3);

	PrintModel(model);

	system("pause");

	return 0;
}