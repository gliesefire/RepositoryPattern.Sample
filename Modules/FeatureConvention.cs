using System.Reflection;

namespace Microsoft.AspNetCore.Mvc.ApplicationModels
{
    public class FeatureConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            controller.Properties.Add("feature",
              GetFeatureName(controller.ControllerType));

            controller.Properties.Add("usecase",
              GetUseCaseName(controller.ControllerType));
        }

        private string GetUseCaseName(TypeInfo controllerType)
        {
            string[] tokens = controllerType?.FullName?.Split('.') ?? new string[0];
            bool featureFound = false;
            for (byte i = 0; i < tokens.Length; ++i)
            {
                if (tokens[i].Equals("Features"))
                {
                    featureFound = true;
                    break;
                }
            }
            if (featureFound && tokens.Length > 3)
                return tokens[^2];
            else
                return string.Empty;
        }

        private string GetFeatureName(TypeInfo controllerType)
        {
            string[] tokens = controllerType?.FullName?.Split('.') ?? new string[0];
            string featureName = string.Empty;
            for (byte i = 0; i < tokens.Length; ++i)
            {
                if (tokens[i].Equals("Features"))
                {
                    if((i + 1) < tokens.Length)
                      featureName = tokens[i + 1];
                    break;
                }
            }
            return featureName;
        }
    }
}
