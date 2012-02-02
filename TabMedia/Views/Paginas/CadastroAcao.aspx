<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Models.Acao>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Ação - Cadastro
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="titulo">
        <h2><%=(Model != null) ? Model.Controlador.Nome: ViewData["Controlador"]%> - Ação - Cadastro</h2>
    </div>
    
    <div class="conteudo-form">
    
        <%=Html.ValidationSummary() %>
    
    	<form method="post" action="">
            <ul>
        	    <li>
            	    <label>Nome</label>
                    <%= Html.TextBoxFor(model => model.Nome) %>
                </li>
                <li>
            	    <label>Descricao</label>
                    <%= Html.TextBoxFor(model => model.Descricao) %>
                </li>
                <li class="liSubmit">
                    <input class="submit" type="submit" value="Cadastrar" />
                </li>
            </ul>
        </form>
    </div>

</asp:Content>