#ifndef __MNIST__HPP__
#define __MNIST__HPP__
#include <iostream>
#include <string>
#include <fstream>
#include <stdio.h>
#include <vector>

struct image
{
	image(int rows, int cols)
	{
		this->rows = rows;
		this->cols = cols;
		pixels = (double**)malloc(sizeof(double*) * rows);
		for (int i = 0; i < cols; ++i)
			pixels[i] = (double*)malloc(sizeof(double) * cols);
	}
	~image()
	{
		for (int i = 0; i < rows; ++i)
			free(pixels[i]);
		free(pixels);
	}
	double **pixels;
	int label;
	int rows;
	int cols;
};

class MNIST
{
public:
	std::vector<image*> images;
	int reverseInt(int i)
	{
		unsigned char c1, c2, c3, c4;

		c1 = i & 255;
		c2 = (i >> 8) & 255;
		c3 = (i >> 16) & 255;
		c4 = (i >> 24) & 255;

		return ((int)c1 << 24) + ((int)c2 << 16) + ((int)c3 << 8) + c4;
	}
	void read_mnist(std::string path, std::string images_file, std::string labels_file)
	{
		std::ifstream fimages(path + "\\" + images_file, std::ios::binary);
		std::ifstream flabels(path + "\\" + labels_file, std::ios::binary);
		if (fimages.is_open())
		{
			int magic_number = 0;
			fimages.read((char*)&magic_number, sizeof(magic_number));
			magic_number = reverseInt(magic_number);
			int number_of_images = 0;
			fimages.read((char*)&number_of_images, sizeof(number_of_images));
			number_of_images = reverseInt(number_of_images);
			int n_rows = 0;
			fimages.read((char*)&n_rows, sizeof(n_rows));
			n_rows = reverseInt(n_rows);
			int n_cols = 0;
			fimages.read((char*)&n_cols, sizeof(n_cols));
			n_cols = reverseInt(n_cols);
			int magic2 = 0;
			flabels.read((char*)&magic2, sizeof(magic2));
			magic2 = reverseInt(magic2);
			int numLabels = 0;
			flabels.read((char*)&numLabels, sizeof(numLabels));
			numLabels = reverseInt(numLabels);

			images = std::vector<image*>();
			images.reserve(number_of_images);
			
			for (int i = 0; i < number_of_images; ++i)
			{
				image* tmpImg = new image(n_rows, n_cols);
				for (int r = 0; r < n_rows; ++r)
				{
					for (int c = 0; c < n_cols; ++c)
					{
						unsigned char temp;
						fimages.read((char*)&temp, sizeof(temp));
						if (temp != '\0')
							tmpImg->pixels[r][c] = 1.0;
						else
							tmpImg->pixels[r][c] = 0.0;
					}
				}
				unsigned char lbl;
				flabels.read((char*)&lbl, sizeof(lbl));
				tmpImg->label = lbl;
				images.push_back(tmpImg);
			}
		}
		fimages.close();
		flabels.close();
	}
};

#endif //__MNIST__HPP__