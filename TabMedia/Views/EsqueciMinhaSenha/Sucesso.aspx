<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Recuperação de Senha - <%=Util.Sistema.AppSettings.NomeSistema %></title>
    <% Html.RenderPartial("Header"); %>
    
    <script type="text/javascript">
        $('.login').corner("20px");
        $(function(){
            
            $('form').jqTransform();
        });
    </script>
</head>
<body class="login-body">
    <div>
        <form method="post" action="">
            <div class="login" style="height: 110px; width: 300px; padding: 20px">
                <h2>Esqueci Minha Senha</h2>
                <br /><br />
                <center>As instruções para recuperação de senha foram enviadas para seu email.</center>
                <div style="text-align: right; float: right; margin-top: 5px;">
                    <a href="<%=Url.Action("index","login") %>" >Voltar</a>
                </div>
            </div>
        </form>
    </div>
</body>
</html>
