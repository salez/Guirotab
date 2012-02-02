<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Models.Controlador>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Controlador - Cadastro
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <div class="titulo">
        <h2>Controlador - Cadastro</h2>
    </div>
    
    <div class="conteudo-form">
    
        <%=Html.ValidationSummary() %>
        
    	<form method="post" action="">
            <ul>
        	    <li>
            	    <label for="Nome">Nome</label>
                    <%= Html.TextBoxFor(model => model.Nome) %>
                </li>
                <li>
            	    <label for="Descricao">Descrição</label>
                    <%= Html.TextBoxFor(model => model.Descricao) %>
                </li>
                <li class="liSubmit">
                    <input type="submit" class="submit" value="Cadastrar" />
                </li>
            </ul>
        </form>
    </div>

</asp:Content>