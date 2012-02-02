using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Models;
using System.Web.Mvc;

public class EmailAttribute : RegularExpressionAttribute
{
    public EmailAttribute()
        : base(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$") { }
}

public class ValidateOnlyFields : ActionFilterAttribute
{
    public String[] Campos { get; set; }

    public ValidateOnlyFields(String[] campos)
    {
        this.Campos = campos;
    }

    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        var modelState = filterContext.Controller.ViewData.ModelState;
        var valueProvider = filterContext.Controller.ValueProvider;

        var keys = modelState.Keys.Except(this.Campos);

        foreach (var key in keys)
            modelState[key].Errors.Clear();
    }
}

public class NotValidateFields : ActionFilterAttribute
{
    public String[] Campos { get; set; }

    public NotValidateFields(String[] campos)
    {
        this.Campos = campos;
    }

    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        var modelState = filterContext.Controller.ViewData.ModelState;
        var valueProvider = filterContext.Controller.ValueProvider;

        var keys = modelState.Keys.Where(x => this.Campos.Contains(x));

        foreach (var key in keys)
            modelState[key].Errors.Clear();
    }
}