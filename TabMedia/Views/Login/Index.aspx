<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<Models.Usuario>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

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
                <%--<h2>Guiropa</h2>--%>
            </div>
            <div class="login">
                <table class="tabela-login">
                    <tr style="height: 110px;">
                        <td>
                           <div class="titulo"><img alt="tabmedia" src="<%=Url.Content("~/images/logo_tabmedia_login.jpg") %>"/></div><br /><br />
                           <div class="nome"><label>Email:</label><%=Html.TextBoxFor(usuario => usuario.Email) %></div>
                           <div class="senha">
                               <label>Senha:</label><%=Html.PasswordFor(usuario => usuario.Senha) %>
                               <span><a href="<%=Url.Action("index","esqueciminhasenha") %>">esqueceu a senha?</a></span>
                           </div>
                            
                           <div class="submit" style="margin:0;"><input type="submit" value="Entrar" class="btnEntrar"/></div>
                           
                           <div class="divErro"><%=ViewData["erro"] %></div>
                        </td>
                    </tr>
                </table>
            </div>
        </form>
    </div>
</body>
</html>
