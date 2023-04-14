//using Python.Runtime;
//public class DecisionTreeModel
//{
//    private dynamic _model;
//    public DecisionTreeModel(string modelFilePath)
//    {
//        using (Py.GIL())
//        {
//            dynamic pickle = Py.Import("pickle");
//            using (var file = new PyFile(modelFilePath, "rb"))
//            {
//                _model = pickle.load(file);
//            }
//        }
//    }
//    public string Predict(/* Add your feature parameters here */)
//    {
//        string prediction;
//        using (Py.GIL())
//        {
//            dynamic np = Py.Import("numpy");
//            // Replace 'YourFeatureArray' with a comma-separated list of your feature parameters
//            dynamic features = np.array(new[] { YourFeatureArray });
//            dynamic prediction = _model.predict(features);
//            prediction = prediction.ToString();
//        }
//        return prediction;
//    }
//}