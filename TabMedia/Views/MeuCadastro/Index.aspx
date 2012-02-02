<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Models.Usuario>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Meu Cadastro
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="titulo">
        <h2>Meu Cadastro</h2>
    </div>
    
    <div class="conteudo-form">
    
        <font color="red">
            <%=Html.ValidationSummary() %>
        </font>
        
	    <form method="post" action="">
            <ul>
    	        <li>
        	        <label for="Nome">Nome</label>
                    <%= Html.TextBoxFor(model => model.Nome) %>
                </li>
                <li>
        	        <label for="Email">Email</label>
                    <p><font color="gray"><%=Model.Email %></font></p>
                </li>
                <li>
        	        <label for="Senha">Senha</label>
        	        <%= Html.PasswordFor(model => model.Senha, new { maxlength = "50", @class = "validate[length[0,50]]", id ="Senha" })%> <p><font color="gray">Deixe em branco caso não queira alterá-la.</font></p>
                </li>
                <li>
        	        <label for="Senha">Confirmar Senha</label>
        	        <input id="ConfirmarSenha" type="password" maxlength="50" class="validate[confirm[Senha]]">
                </li>
                <li class="liSubmit">
                    <input class="submit" type="submit" value="Cadastrar" />
                </li>
            </ul>
        </form>
    </div>

</asp:Content>