<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<link rel="shortcut icon" href="<%=Url.Content("~/favicon.ico")%>" type="image/x-icon" />

<%--CSS--%>

    <%=Util.Header.Css("~/Css/screen.css")%>
    <%=Util.Header.Css("~/Css/ipad.css", "only screen and (max-device-width: 1024px)")%>

    <%--PLUGINS--%>
    <%=Util.Header.Css("~/Css/jqtransform/jqtransform.css")%>
    <%=Util.Header.Css("~/Css/validation-engine/validationEngine.jquery.css")%>
    <%=Util.Header.Css("~/Css/jquery-rating/jquery.rating.css")%>
    <%=Util.Header.Css("~/Css/jquery-ui/jquery-ui-1.8.1.smoothness.css")%>
    <%=Util.Header.Css("~/css/uploadify/uploadify.css")%>
    <%=Util.Header.Css("~/css/anything-slider/slider.css") %>
    <%=Util.Header.Css("~/css/jquery-modal/jqModal.css") %>

    <%--CUSTOMIZACAO--%>
    <%=Util.Header.Css("~/Admin/customizacao/cssadmin.css")%>
    <%=Util.Header.Css("~/Customizacao/cssvalidationEngine.css")%>
    
<%--JAVASCRIPT--%>



    <%--JQUERY--%>
    <%=Util.Header.Javascript("~/Scripts/jquery/jquery-1.6.4.min.js")%>
    <%=Util.Header.Javascript("~/Scripts/jquery-ui/jquery-ui-1.8.4.custom.min.js")%>

        <%--PLUGINS--%>
        <%=Util.Header.Javascript("~/Scripts/jquery/jquery.maskedinput-1.2.2.min.js")%>
        <%=Util.Header.Javascript("~/Scripts/jquery/jquery.MetaData.js")%>
        <%=Util.Header.Javascript("~/Scripts/jquery/jquery.tablesorter.min.js")%>
        <%=Util.Header.Javascript("~/Scripts/jquery/jquery.tablesorter.pager.js")%>
        <%=Util.Header.Javascript("~/Scripts/jquery/jquery.pagination.js")%>
        <%=Util.Header.Javascript("~/Scripts/jquery/jquery.jqtransform.js")%>
        <%=Util.Header.Javascript("~/Scripts/jquery/jquery.rating.pack.js")%>
        <%=Util.Header.Javascript("~/Scripts/jquery/jquery.price_format.1.3.js")%>
        <%=Util.Header.Javascript("~/Scripts/jquery/jquery.limit-1.2.js")%>
        <%=Util.Header.Javascript("~/Scripts/jquery/jquery.corner.js")%>
        <%=Util.Header.Javascript("~/scripts/jquery/jquery.form.js")%>
        <%=Util.Header.Javascript("~/scripts/jquery/jquery.anythingslider.js")%>
        <%=Util.Header.Javascript("~/Scripts/uploadify/jquery.uploadify.v2.1.4.js")%>
        <%=Util.Header.Javascript("~/Scripts/jquery/jquery.address-1.4.js")%>
        <%=Util.Header.Javascript("~/Scripts/jquery/jquery.popupWindow.js")%>
        <%=Util.Header.Javascript("~/Scripts/jquery/jquery.validationEngine.js")%>
        <%=Util.Header.Javascript("~/Scripts/jquery/jquery.validationEngine-pt-br.js")%>
        <%=Util.Header.Javascript("~/Scripts/jquery/jqModal.js")%>

        <%--HIGHCHARTS--%>
        <%=Util.Header.Javascript("~/Scripts/highcharts/highcharts.js")%>
        <%=Util.Header.Javascript("~/Scripts/highcharts/exporting.js")%>

    <%--FLASH / SWFOBJECT--%>
    <%=Util.Header.Javascript("~/Scripts/swfobject.js")%>

    <%--EDITOR DE TEXTO--%>
    <%=Util.Header.Javascript("~/Scripts/ckeditor/ckeditor.js")%>
    <%=Util.Header.Javascript("~/Scripts/ckeditor/adapters/jquery.js")%>

