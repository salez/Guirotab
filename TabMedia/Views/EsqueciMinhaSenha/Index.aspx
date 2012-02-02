<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Login - <%=Util.Sistema.AppSettings.NomeSistema %></title>
    <% Html.RenderPartial("Header"); %>
    
    <script type="text/javascript">
        $('.login').corner("20px");
        $(function () {
            //$('form').jqTransform();
            //$('.jqTransformSubmit').corner('5px');
        });
    </script>
    
</head>
<body class="login-body">
    <div>
    
        <%=Html.ValidationSummary() %>
        
        <%--<form method="post" action="">
            <div class="login" style="height: 110px; width: 300px; padding: 20px">
                <h2>Esqueci Minha Senha</h2>
                <br />
                <label>Email</label> 
                <br /><br />
                <div style="text-align: right; float: right;">
                    <input type="submit" value="Recuperar minha senha" />
                </div>
                
            </div>
        </form>--%>

        <form method="post" action="">
            <div class="topo_login">
                <h2>Guiropa</h2>
            </div>
            <div class="login">
                <table class="tabela-login">
                    <tr style="height: 110px;">
                        <td>
                           <div class="nome"><label>Email:</label><input type="text" name="email" /></div>
                            
                           <div class="botoes" style="float: right; margin-right: 20px;"><a value="Recuperar Senha" onclick="$('form').submit();" href="#" class="botao">Recuperar Senha</a></div>
                           
                           <div class="divErro"><%=ViewData["msg"] %></div>
                        </td>
                    </tr>
                </table>
            </div>
        </form>
    </div>
</body>
</html>
