using BlogEngine.Core;
using BlogEngine.Core.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

public class BlogRollController : ApiController
{
    public BlogRollController()
    {
    }

    public IEnumerable<BlogRollItem> Get()
    {
        var vm = new BlogRollVM();
        return vm.BlogRolls;
    }

    public HttpResponseMessage Get(string id)
    {
        Guid gId;
        if(Guid.TryParse(id, out gId))
        {
            return Request.CreateResponse(HttpStatusCode.NotFound);
        }
        else
        {
            var vm = new BlogRollVM();
            var result = vm.BlogRolls.Find(b => b.Id == gId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }

    public HttpResponseMessage Post(BlogRollItem item)
    {
        if (!Security.IsAdministrator)
            return Request.CreateResponse(HttpStatusCode.Unauthorized, item);

        BlogEngine.Core.Providers.BlogService.InsertBlogRoll(item);
        return Request.CreateResponse(HttpStatusCode.Created, item);
    }

    [HttpPut]
    public HttpResponseMessage Update([FromBody]BlogRollItem item)
    {
        if (!Security.IsAdministrator)
            return Request.CreateResponse(HttpStatusCode.Unauthorized, item);

        BlogEngine.Core.Providers.BlogService.UpdateBlogRoll(item);
        return Request.CreateResponse(HttpStatusCode.OK);
    }

    private string getUrl(string url)
    {
        if (!string.IsNullOrEmpty(url) && !url.Contains("://"))
            url = "http://" + url;
        return url;
    }
}