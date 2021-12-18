using System;
using System.Linq;
using System.Threading.Tasks;
using CafeManager;
using HtmlAgilityPack;
using Utilities;

namespace Pipelines
{
    internal class FetchDataPipeline : Pipeline
    {
        public FetchDataPipeline(ParentPipeline parentPipeline) : base(parentPipeline)
        {
        }

        public override Task<object> DoFilter(int step, object preFiltered)
        {
            return new Task<object>(delegate
            {
                var urlPacker = new UrlPacker();

                var input = preFiltered as string[];

                if (input == null)
                    throw new NullReferenceException();

                var url = input[0];

                var requestedMenus = new ArraySegment<string>(input, 1, input.Length - 1);

                var web = new HtmlWeb();
                var docs = web.Load(url);
                var node = docs.DocumentNode;
                var menuNodes = node.SelectNodes("//a[@target='cafe_main']");

                foreach (var menuNode in menuNodes)
                {
                    var menuName = menuNode.InnerText;
                    if (!requestedMenus.Contains(menuName))
                        continue;
                    var menuUrl = menuNode.Attributes["href"].Value;
                    urlPacker.Add(new PackedUrl(menuName, UrlUtils.Join(Const.CafeDomain, menuUrl)));
                }

                return urlPacker;
            });
        }
    }
}