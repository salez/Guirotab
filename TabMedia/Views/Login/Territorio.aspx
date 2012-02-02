<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<Models.Territorio>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<% Response.Redirect(Url.Action("index")); %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Login - <%=Util.Sistema.AppSettings.NomeSistema %></title>
    <% Html.RenderPartial("Header"); %>
    
    <script type="text/javascript">
        $('.login').corner("20px");
        $(function() {
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
            	<h4></h4>
                <h2>Guiropa</h2>
                <h4></h4>
            </div>
            <div class="login" >
                <table class="tabela-login">
                    <tr style="height: 110px;">
                        <td>
                           <div class="nome" style="width: 530px;"><label style="width: 120px;">Território:</label><%=Html.TextBox("IdTerritorio") %></div>
                           <div class="senha" style="width: 530px;">
                               <label style="width: 120px;">Senha:</label><%=Html.Password("Senha") %>
                               <%--<span><a href="<%=Url.Action("index","esqueciminhasenha") %>">esqueceu a senha?</a></span>--%>
                           </div>
                            
                           <div class="submit"><input type="submit" value="Entrar" class="btnEntrar"/></div>
                           
                           <div class="divErro"><%=ViewData["erro"] %></div>
                        </td>
                    </tr>
                </table>
            </div>
        </form>
    </div>
</body>
</html>
