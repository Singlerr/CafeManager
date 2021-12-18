using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Conditions;
using HtmlAgilityPack;
using Utilities;

namespace Pipelines
{
    internal class FetchArticleDetailsPipeline : Pipeline
    {
        public FetchArticleDetailsPipeline(ParentPipeline pipeline) : base(pipeline)
        {
        }

        public override Task<object> DoFilter(int step, object preFiltered)
        {
            return new Task<object>(delegate
            {
                if (preFiltered is object[] arguments)
                    if (arguments[0] is ConditionParentPipeline cond && arguments[1] is UrlPacker packer)
                    {
                        var web = new HtmlWeb();
                        var packedArticleList = new List<PackedArticles>();
                        foreach (var packedUrls in packer.GetUrls())
                        {
                            var packedArticles = new PackedArticles();

                            var menuName = packedUrls.Name;
                            var basePageUrl = packedUrls.Url;
                            var k = 0;
                            HtmlNodeCollection nodes;

                            packedArticles.MenuName = menuName;

                            do
                            {
                                var pageUrl = UrlUtils.Join(basePageUrl, k.ToString());
                                var docs = web.Load(pageUrl);
                                nodes = docs.DocumentNode.SelectNodes("//tr[@*]");
                                foreach (var articleNode in nodes)
                                {
                                    var titleNode = articleNode.SelectSingleNode("//a[class='article']");
                                    var dateNode = articleNode.SelectSingleNode("//td[class='td_date']");
                                    var authorNode = articleNode.SelectSingleNode("//a[href='#']");
                                    var title = titleNode.InnerText;
                                    var date = dateNode.InnerText;
                                    var dateTime = DateTime.ParseExact(date, "yyyy.MM.dd",
                                        CultureInfo.InvariantCulture);
                                    var author = authorNode.InnerText;
                                    var condition = new Condition(title, dateTime
                                        , author,
                                        menuName);
                                    cond.Condition = condition;
                                    if (cond.CheckConditions())
                                    {
                                        var article = new Article();
                                        article.Author = author;
                                        article.Time = dateTime;
                                        article.Title = title;
                                        article.MenuName = menuName;
                                        article.PathUrl = titleNode.Attributes["href"].Value;
                                        packedArticles.Articles.Add(article);
                                    }
                                }

                                k++;
                            } while (nodes != null && nodes.Count > 0);

                            packedArticleList.Add(packedArticles);
                        }

                        return packedArticleList;
                    }

                return null;
            });
        }
    }
}