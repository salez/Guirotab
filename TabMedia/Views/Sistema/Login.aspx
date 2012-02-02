<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Sistema
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

     <div class="titulo">
     	<h4></h4>
        <h2>Sistema</h2>
        <h4></h4>
    </div>
    
    <div class="conteudo-form">
    
    <% using (Html.BeginForm()) { %>
    
        <ul>
            <li>
                <%= ViewData["erro"] %>
            </li>
            <li>
                <%= Html.Label("Senha")%>
                <%= Html.Password("senha", "", new { maxlength = "50", @class = "validate[required]" })%>
            </li>
            <li>
                <br />
                <input type="submit" value="Acessar" />
            </li>
        </ul>
    
    <% } %>

</asp:Content>