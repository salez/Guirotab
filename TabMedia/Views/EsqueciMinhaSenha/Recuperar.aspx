<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Recuperação de Senha - <%=Util.Sistema.AppSettings.NomeSistema %></title>
    <% Html.RenderPartial("Header"); %>
    
    <script type="text/javascript">
        //$('.login').corner("20px");
        $(function(){

            $("form").validationEngine({
                inlineValidation: false,
                failure: function() {
                    $("form").validationEngine();
                }
            });
            
            //$('form').jqTransform();
        });
    </script>
</head>
<body class="login-body">
    <div>
        <form method="post" action="">
            
            <div class="topo_login">
                <h2>Recuperação de Senha</h2>
            </div>
            <div class="login">
                <table class="tabela-login">
                    <tr style="height: 110px;">
                        <td>
                           <div class="nome" style="height: auto;">
                                
                                <label>Nova Senha</label><br /> <input id="senha" type="password" name="senha" class="validate[required]" />
                                <br />
                                <label>Confirmar Nova Senha</label><br /> <input id="confirmarSenha" type="password" class="validate[confirm[senha]]" /><br />
                           
                           </div>
                            
                           <div class="botoes" style="float: right; margin-right: 20px;"><a onclick="$('form').submit();" href="#" class="botao">Salvar</a></div>
                           
                           <div class="divErro"><%=ViewData["msg"] %></div>
                        </td>
                    </tr>
                </table>
            </div>


        </form>
    </div>
</body>
</html>
