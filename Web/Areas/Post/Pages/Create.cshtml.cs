using System.Diagnostics.CodeAnalysis;
using Application.Common.Interface;
using Application.Common.Security;
using Application.Features.Post;
using Ganss.Xss;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace Web.Areas.Post.Pages;

[CustomAuthorize]
public class MediatorCreatePostModel : PageModel
{
    [BindProperty]
    public CreatePostCommand Input { get; set; } = default!;
    
    [BindProperty]
    public string TagHolder { get; set; }

    public virtual Task OnGetAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? returnUrl = null) => throw new NotImplementedException();
    
    public virtual Task<IActionResult> OnPostAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? returnUrl = null) => throw new NotImplementedException();
}

internal sealed class MediatorCreatePostModel<TPost> : MediatorCreatePostModel where TPost : class
{
    private readonly IMediator _mediator;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHelperService _webHelper;
    
    public MediatorCreatePostModel(
        IHttpContextAccessor httpContextAccessor, 
        IMediator mediator, 
        IWebHelperService webHelper)
    {
        _httpContextAccessor = httpContextAccessor;
        _webHelper = webHelper;
        _mediator = mediator;
    }
    
    public override async Task OnGetAsync(string? returnUrl = null)
    {
        /*
        if (_httpContextAccessor.HttpContext == null)
        {
            Response.Redirect(_webHelper.GetPathBase());
            return;
        }

        var user = _httpContextAccessor.HttpContext.User;
        if (user.Identity != null && user.Identity.IsAuthenticated)
        {
            //Response.Redirect( _webHelper.GetPathBase() + "auth/login");
            LocalRedirect("~/auth/login");
        }
        */
    }
    
    public override async Task<IActionResult> OnPostAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? returnUrl = null)
    {
        if (ModelState.IsValid)
        {
            var sanitizer = new HtmlSanitizer();
            if (TagHolder != null!)
            {
                Input.Tags = JsonConvert.DeserializeObject<List<string>>(TagHolder.ToUpper());
            }

            Input.Title = sanitizer.Sanitize(Input.Title);
            Input.Body = sanitizer.Sanitize(Input.Body);
            
            var result = await _mediator.Send(Input);
            if (result)
            {
                Response.Redirect( _webHelper.GetPathBase() + "post/create/confirmation");
            }
        }
        return Page();
    }
}