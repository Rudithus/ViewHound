using System.Linq;
using System.Web.Mvc;

namespace ViewHound.Mvc5
{
    public class HoundController : Controller
    {
        private readonly IHound _hound;

        public HoundController(IHound hound)
        {
            _hound = hound;
        }

        public ActionResult Used()
        {
            var views = _hound.GetUsedViewsLog();
            var content = views
                .Select(v => $"<p>View: {v.ViewPath}, Use count : {v.UseCount} </p>")
                .Aggregate((s1, s2) => $"{s1}{s2}");
            return Content(
                $"<html>{content}</html>");
        }
        public ActionResult UnUsed()
        {
            var views = _hound.GetUnUsedViewsLog();
            var content = views
                .Select(v => $"<p>View: {v.ViewPath}</p>")
                .Aggregate((s1, s2) => $"{s1}{s2}");
            return Content(
                $"<html>{content}</html>");
        }
        public ActionResult All()
        {
            var views = _hound.GetAllViewsLog();
            var content = views
                .Select(v => $"<p>View: {v.ViewPath}, Use count : {v.UseCount} </p>")
                .Aggregate((s1, s2) => $"{s1}{s2}");
            return Content(
                $"<html>{content}</html>");
        }
    }
}
