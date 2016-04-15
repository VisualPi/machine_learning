#include <LinearModel.h>
#include <iostream>

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

	double inputs[8] = { 
		-1, -1,
		 1, -1,
		 1,  1,
		-1,  1};
	double results[4] = { -1, 1, -1, 1 };

	int pbl[3] = { 2, 3, 1 };
	int nbLayer = 3;

	double inputTest[2] = { 1, -1 }; //doit sortir 1

	MLP_bis *model = multilayer_create_model_bis( pbl, nbLayer);

	PrintModel(model, nbLayer);

	multilayer_classify_gradient_backpropagation(model, inputs, 2, 4, results, nbLayer, true);
	multilayer_classify_perceptron(model, inputTest, nbLayer, true);

	PrintModel(model, nbLayer);

	std::cout << model->X[2][1] << std::endl;

	double inputTest2[2] = { 1, 1 }; //doit sortir -1
	multilayer_classify_perceptron(model, inputTest2, nbLayer, true);

	std::cout << model->X[2][1] << std::endl;

	Destroy_MultiLayer_Perceptron(model, nbLayer);

	system("pause");

	return 0;
}