#include <LinearModel.h>
#include <iostream>
int main()
{
	//double *model = linear_create_model(2);
	double inputs[12] = { 1, 6, 5, 9, 3, 7, 6, 3, 8, 4, 9, 2 };
	double results[6] = { -1, -1, -1, 1, 1, 1 };
	//perceptron_fit(model, inputs, 2, 12, results);
	//double wanted[2] = { 4, 5 };
	//int res = perceptron_classify(model, wanted, 2);
	//std::cout << "res : " << res << std::endl;

	int pbl[3] = { 3, 4, 1 };
	MLP *model = multilayer_create_model( 2, pbl, 3 );

	double inputTest[2] = { 0.5, 0.3 };
	std::cout << multilayer_classify_perceptron(model, inputTest, 2, 3) << std::endl;
	system("pause");
	//linear_remove_model( model );
	return 0;
}