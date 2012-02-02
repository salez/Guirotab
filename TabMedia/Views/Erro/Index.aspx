<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<Models.Usuario>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Erro - <%=Util.Sistema.AppSettings.NomeSistema %></title>
    <% Html.RenderPartial("Header"); %>
    
    <script type="text/javascript">
        $('.login').corner("20px");
        $(function () {
            //$('form').jqTransform();
            //$('.jqTransformSubmit').corner('5px');

            $('.submit').jqTransform();
        });
    </script>
</head>
<body class="login-body">
    <div>
        <form method="post" action="">
            <div class="topo_login">
                <%--<h2>Guiropa</h2>--%>
            </div>
            <div class="erro" style="position: absolute; left: 50%; margin-left: -400px; width: 800px; text-align: center; top: 50%; height: 300px; margin-top: -150px;">
                <img src="<%=Url.Content("~/images/logo_tabmedia_login.jpg") %>" />
                <br /><br />
                <h2>
                    Ops! Um erro ocorreu durante a requisição
                </h2>
                <br /><br />
                <p>
                    Um erro ocorreu durante o processamento de sua requisição, nossa equipe de desenvolvimento já foi informada e trabalharemos o mais breve possível para resolver este problema.
                </p>
                <br />
                <p>
                    Grato pela compreensão.
                </p>
            </div>
        </form>
    </div>
</body>
</html>


<%--<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="errorTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Ocorreu algum erro
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

<asp:Content ID="errorContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Desculpe, Um erro ocorreu durante a requisição
    </h2>
    
    <p>
        Um erro ocorreu durante o processamento de sua requisição, nossa equipe de desenvolvimento já foi informada e trabalharemos o mais breve possível para resolver este problema.
    </p>
    <p>
        Grato pela compreensão.
    </p>
</asp:Content>--%>