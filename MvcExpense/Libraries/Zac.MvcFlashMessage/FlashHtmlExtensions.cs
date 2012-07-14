using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace Zac.MvcFlashMessage
{
    public static class FlashHtmlExtensions
    {
        public static IHtmlString Flash(this HtmlHelper instance, string tagName = "p", bool encoded = true)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            Func<string, XNode> content = message => encoded ? new XText(message) : XElement.Parse(message) as XNode;

            var messages = new FlashStorage(instance.ViewContext.TempData).Messages.ToList();

            var elements = messages.Select(pair => new XElement(tagName ?? "div", new XAttribute("class", "flash" + " " + pair.Key), content(pair.Value)));
            var html = string.Join(Environment.NewLine, elements.Select(e => e.ToString()));

            return instance.Raw(html);
        }
    }
}