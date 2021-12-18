using System.Threading.Tasks;
using CafeManager;
using HtmlAgilityPack;
using Utilities;

namespace Pipelines
{
    internal class FetchArticlePipeline : Pipeline
    {
        public FetchArticlePipeline(ParentPipeline pipeline) : base(pipeline)
        {
        }

        public override Task<object> DoFilter(int step, object preFiltered)
        {
            return new Task<object>(delegate
            {
                if (preFiltered is UrlPacker packer)
                {
                    var web = new HtmlWeb();
                    var urlPacker = new UrlPacker();
                    foreach (var packedUrl in packer.GetUrls())
                    {
                        var articlePage = web.Load(packedUrl.Url);
                        var node = articlePage.DocumentNode.SelectSingleNode("//a[class='on']");
                        var path = node.Attributes["href"].Value;
                        var pageUrl = UrlUtils.Join(Const.CafeDomain, path.Remove(path.Length - 1));
                        urlPacker.Add(new PackedUrl(packedUrl.Name, pageUrl));
                    }

                    return urlPacker;
                }

                return null;
            });
        }
    }
}