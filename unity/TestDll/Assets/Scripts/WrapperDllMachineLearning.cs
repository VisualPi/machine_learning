using System.Runtime.InteropServices;

namespace Wrapper.Dll {
    public static class WrapperDllMachineLearning {
        [DllImport("DllForUnity")]
        public extern static void SetAlpha(double value);
        [DllImport("DllForUnity")]
        public extern static void SetIteration(int value);

        [DllImport("DllForUnity")]
        public extern static System.IntPtr linear_create_model(int input_model);
        [DllImport("DllForUnity")]
        public extern static void linear_remove_model(System.IntPtr model);
        [DllImport("DllForUnity")]
        public extern static int perceptron_classify(System.IntPtr model, double[] input, int inputSize);
        [DllImport("DllForUnity")]
        public extern static void perceptron_fit(System.IntPtr model, double[] inputs, int modelSize, int inputsSize, double[] results);
        [DllImport("DllForUnity")]
        public extern static void classification_hebb(System.IntPtr model, double[] input, int inputSize, double result);
        [DllImport("DllForUnity")]
        public extern static void regression_fit(System.IntPtr model, double[] inputs, int modelSize, int inputsSize, double[] results);
        [DllImport("DllForUnity")]
        public extern static double perceptron_predict(System.IntPtr model, double[] input, int inputSize);

        public static class MLP {
            #region DLL
            [DllImport("DllForUnity")]
            private static extern System.IntPtr[] MLP_CreateModel(int[] perceptronsByLayer, int nbLayer);

            [DllImport("DllForUnity")]
            private static extern double[] MLP_ClassifyPerceptron(System.IntPtr[] model, double[] inputs, int nbLayer, bool useClassify);

            [DllImport("DllForUnity")]
            private static extern void MLP_ClassifyGradientBackpropagation(System.IntPtr[] model, double[] inputs, int inputSize, int exampleNumber, double[] output, int nbLayer, bool useClassify);

            [DllImport("DllForUnity")]
            private static extern void MLP_DestroyModel(System.IntPtr[]  mlp, int nbLayer);


            [DllImport("DllForUnity")]
            private static extern double MLP_GetResultForIndex(System.IntPtr[] model, int nbLayer, int index);

            [DllImport("DllForUnity")]
            private static extern double MLP_GetAlpha();

            [DllImport("DllForUnity")]
            private static extern void MLP_SetAlpha(double value);

            [DllImport("DllForUnity")]
            private static extern int MLP_GetIteration();

            [DllImport("DllForUnity")]
            private static extern void MLP_SetIteration(int value);
            #endregion DLL

            public static System.IntPtr[] CreateModel(int[] perceptronsByLayer, int nbLayer) {
                return MLP_CreateModel(perceptronsByLayer, nbLayer);
            }

            public static double[] ClassifyPerceptron(System.IntPtr[] model, double[] inputs, int nbLayer, bool useClassify) {
                return MLP_ClassifyPerceptron(model, inputs, nbLayer, useClassify);
            }

            public static void ClassifyGradientBackpropagation(System.IntPtr[] model, double[] inputs, int inputSize,
                int exampleNumber, double[] output, int nbLayer, bool useClassify) {
                MLP_ClassifyGradientBackpropagation(model, inputs, inputSize, exampleNumber, output, nbLayer, useClassify);
            }

            public static void DestroyModel(System.IntPtr[] mlp, int nbLayer) {
                MLP_DestroyModel(mlp, nbLayer);
            }

            public static double GetResultForIndex(System.IntPtr[] model, int nbLayer, int index) {
                return MLP_GetResultForIndex(model, nbLayer, index);
            }

            public static double Alpha { get { return MLP_GetAlpha(); } set { MLP_SetAlpha(value); } }
            public static int Iteration { get { return MLP_GetIteration(); } set { MLP_SetIteration(value); } }
        }

        public static class RBF {
            #region DLL
            [DllImport("DllForUnity")]
            private static extern System.IntPtr RBF_CreateModel(int nbParameter, int nbExample);

            [DllImport("DllForUnity")]
            private static extern System.IntPtr RBF_DestroyModel(System.IntPtr model, int nbExample);

            [DllImport("DllForUnity")]
            private static extern double RBF_Classify(System.IntPtr model, double[] input, int inputSize, double gamma);

            [DllImport("DllForUnity")]
            private static extern double RBF_Regression(System.IntPtr model, double[] input, int inputSize, double gamma);

            [DllImport("DllForUnity")]
            private static extern void RBF_Fit(System.IntPtr model, double[] inputs, int modelSize, int inputsSize, double[] results, double gamma);

            [DllImport("DllForUnity")]
            public static extern void RBF_LLoyd(double[] model, double[] inputs, int modelSize, int inputsSize, double[] results);
            #endregion DLL

            public static System.IntPtr CreateModel(int nbExample, int nbParameter) {
                return RBF_CreateModel(nbExample, nbParameter);
            }

            public static void DestroyModel(System.IntPtr model, int nbExample) {
                RBF_DestroyModel(model, nbExample);
            }

            public static double Classify(System.IntPtr model, double[] input, int inputSize, double gamma) {
                return RBF_Classify(model, input, inputSize, gamma);
            }

            public static double Regression(System.IntPtr model, double[] input, int inputSize, double gamma) {
                return RBF_Regression(model, input, inputSize, gamma);
            }

            public static void Fit(System.IntPtr model, double[] inputs, int modelSize, int inputsSize,
                double[] results, double gamma) {
                RBF_Fit(model, inputs, modelSize, inputsSize, results, gamma);
            }
        }
    }
}

