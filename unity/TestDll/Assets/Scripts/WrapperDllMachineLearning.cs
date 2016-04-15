using System.Runtime.InteropServices;

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

    public static class RBF {
        public static System.IntPtr CreateModel(int nbExample, int nbParameter) {
            return rbf_create_model(nbExample, nbParameter);
        }

        public static void DestroyModel(System.IntPtr model, int nbExample) {
            rbf_remove_model(model, nbExample);
        }

        public static double Classify(System.IntPtr model, double[] input, int inputSize, double gamma) {
            return rbf_classify(model, input, inputSize, gamma);
        }

        public static double Regression(System.IntPtr model, double[] input, int inputSize, double gamma) {
            return rbf_regression(model, input, inputSize, gamma);
        }

        public static void Fit(System.IntPtr model, double[] inputs, int modelSize, int inputsSize,
            double[] results, double gamma) {
            rbf_fit(model, inputs, modelSize, inputsSize, results, gamma);
        }

        [DllImport("DllForUnity")]
        private static extern System.IntPtr rbf_create_model(int nbParameter, int nbExample);

        [DllImport("DllForUnity")]
        private static extern System.IntPtr rbf_remove_model(System.IntPtr model, int nbExample);

        [DllImport("DllForUnity")]
        private static extern double rbf_classify(System.IntPtr model, double[] input, int inputSize, double gamma);

        [DllImport("DllForUnity")]
        private static extern double rbf_regression(System.IntPtr model, double[] input, int inputSize, double gamma);

        [DllImport("DllForUnity")]
        private static extern void rbf_fit(System.IntPtr model, double[] inputs, int modelSize, int inputsSize, double[] results, double gamma);

        [DllImport("DllForUnity")]
        public static extern void LLoyd(double[] model, double[] inputs, int modelSize, int inputsSize, double[] results);
    }
}
