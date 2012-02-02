<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Models.Grupo>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Grupos - Cadastro
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	
	<div class="titulo">
		<h2>Grupos - Cadastro</h2>
	</div>
	
	<div class="conteudo-form">
	
		<%=Html.ValidationSummary() %>
		
		<form method="post" action="">
			<ul>
				
				<% if (this.GetActionName() == "cadastro") { %>
				<li>
					<label for="Id">ID</label>
					<%= Html.TextBoxFor(model => model.Id, new { maxlength = "3", @class = "validate[required,length[0,3]]" })%>
				</li>
				<%} %>

				<li>
					<label for="Nome">Nome</label>
					<%= Html.TextBoxFor(model => model.Nome, new { maxlength = "50", @class = "validate[required,length[0,50]]" })%>
				</li>
				
				<li class="liSubmit">
					<input class="submit" type="submit" value="Cadastrar" />
				</li>

			</ul>
		</form>
	</div>
	
</asp:Content>