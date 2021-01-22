using Microsoft.AspNetCore.Mvc.Controllers;
using System.Collections.Generic;

namespace Microsoft.AspNetCore.Mvc.Razor
{
    public class FeatureViewLocationExpander : IViewLocationExpander
    {
        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context,
  IEnumerable<string> viewLocations)
        {
            if (context.ActionContext.ActionDescriptor is ControllerActionDescriptor descriptor && !string.IsNullOrEmpty(context.ControllerName))
            {
                return ExpandPageHierarchy(descriptor);
            }


            // Not a action - just act natural.
            return viewLocations;

            IEnumerable<string> ExpandPageHierarchy(ControllerActionDescriptor controllerActionDescriptor)
            {
                var featureName = controllerActionDescriptor.Properties["feature"];
                var useCaseName = controllerActionDescriptor.Properties["usecase"];
                foreach (var location in viewLocations)
                {
                    yield return location.Replace("{3}", featureName.ToString()).Replace("{4}", useCaseName.ToString());
                }
            }
        }

        public static void SetRazorEngineConfig(RazorViewEngineOptions options)
        {
            // {0} - Action Name
            // {1} - Controller Name
            // {2} - Area Name
            // {3} - Feature Name
            // {4} - Use Case Name
            // Replace normal view location entirely
            options.ViewLocationFormats.Clear();
            options.ViewLocationFormats.Add("/Features/{3}/{4}/{0}.cshtml");
            options.ViewLocationFormats.Add("/Features/{3}/{0}.cshtml");
            options.ViewLocationFormats.Add("/Features/Shared/{0}.cshtml");
            options.ViewLocationExpanders.Add(new FeatureViewLocationExpander());
        }

        public void PopulateValues(ViewLocationExpanderContext context)
        {

        }
    }
}
