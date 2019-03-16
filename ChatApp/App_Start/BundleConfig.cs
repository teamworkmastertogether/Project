using System.Web;
using System.Web.Optimization;

namespace ChatApp
{
    public class BundleConfig
    {
   
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new ScriptBundle("~/comment/js").Include(
                     "~/Assets/Script/CommentPost.js"
                     ));
            bundles.Add(new ScriptBundle("~/messenger/js").Include(
                     "~/Assets/Script/Messenger.js",
                     "~/Assets/Script/Index.js"
                     ));
            bundles.Add(new StyleBundle("~/login/css").Include(
                     "~/Assets/Css/Login.css"
                     ));
            bundles.Add(new StyleBundle("~/comment/css").Include(
                    "~/Assets/Css/CommentPost.css"
                    ));
            bundles.Add(new StyleBundle("~/web/css").Include(
                   "~/Assets/Css/style.css"
                   ));

            BundleTable.EnableOptimizations = true;
        }
    }
}
