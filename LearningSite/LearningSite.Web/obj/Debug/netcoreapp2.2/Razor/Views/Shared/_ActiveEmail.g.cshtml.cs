#pragma checksum "C:\Users\hossein\source\repos\LearningSite\LearningSite.Web\Views\Shared\_ActiveEmail.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "b367ef665a2e2c5425d64164789425c628951bd4"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared__ActiveEmail), @"mvc.1.0.view", @"/Views/Shared/_ActiveEmail.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Shared/_ActiveEmail.cshtml", typeof(AspNetCore.Views_Shared__ActiveEmail))]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"b367ef665a2e2c5425d64164789425c628951bd4", @"/Views/Shared/_ActiveEmail.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a9af4978b9c2bfca24ef48e96efe5f8573634464", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared__ActiveEmail : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<LearningSite.DataLayer.Entities.User.User>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(50, 55, true);
            WriteLiteral("\r\n<div style=\"direction: rtl; padding: 20px\">\r\n    <h2>");
            EndContext();
            BeginContext(106, 14, false);
#line 4 "C:\Users\hossein\source\repos\LearningSite\LearningSite.Web\Views\Shared\_ActiveEmail.cshtml"
   Write(Model.UserName);

#line default
#line hidden
            EndContext();
            BeginContext(120, 132, true);
            WriteLiteral(" عزیز !</h2>\r\n    <p>با تشکر از ثبت نام شما در تاپ لرن ، جهت ادامه کار میبایست حساب کاربری خود را فعال کنید</p>\r\n    <p>\r\n        <a");
            EndContext();
            BeginWriteAttribute("href", " href=\"", 252, "\"", 322, 2);
            WriteAttributeValue("", 259, "https://localhost:44354/Account/ActiveAccount/", 259, 46, true);
#line 7 "C:\Users\hossein\source\repos\LearningSite\LearningSite.Web\Views\Shared\_ActiveEmail.cshtml"
WriteAttributeValue("", 305, Model.ActiveCode, 305, 17, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(323, 45, true);
            WriteLiteral(">فعالسازی حساب کاربری</a>\r\n    </p>\r\n</div>\r\n");
            EndContext();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<LearningSite.DataLayer.Entities.User.User> Html { get; private set; }
    }
}
#pragma warning restore 1591
