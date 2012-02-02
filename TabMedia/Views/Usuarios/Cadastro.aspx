<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Models.Usuario>" %>
<%@ Import Namespace="Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Usuários - Cadastro
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

	<div class="titulo">
		<h2>Usuários - Cadastro</h2>
	</div>
	
	<div class="conteudo-form">
	
		<%=Html.ValidationSummary() %>
		
		<form method="post" action="">
			<ul>
				<li>
					<label for="Nome">Nome</label>
					<%= Html.TextBoxFor(model => model.Nome, new { maxlength = "50", @class = "validate[required,length[0,50]]" })%>
				</li>
				<li>
					<label for="Email">Email</label>
					<%= Html.TextBoxFor(model => model.Email, new { maxlength = "50", @class = "validate[required,length[0,50]]" })%>
				</li>
				<%--<li>
					<label for="Login">Login</label>
					<%= Html.TextBoxFor(model => model.Login, new { maxlength = "50", @class = "validate[required,length[0,50]]" })%>
				</li>--%>
				<li>
					<label for="Senha">Senha</label>
					<% if ((Util.Cadastro.Tipo)ViewData["TipoCadastro"] == Util.Cadastro.Tipo.Inclusao){ %>
						<%= Html.PasswordFor(model => model.Senha, new { maxlength = "50", @class = "validate[required,length[0,50]]", id = "Senha" })%>
					<% }else{ %>
						<%= Html.PasswordFor(model => model.Senha, new { maxlength = "50", @class = "validate[length[0,50]]", id = "Senha" })%>
						<i><font color="gray">&nbsp;deixe em branco caso não queira alterá-la.</font></i>
					<% } %>
				</li>
				<li>
					<label for="Senha">Confirmar Senha</label>
					<input id="ConfirmarSenha" type="password" maxlength="50" class="validate[confirm[Senha]]">
				</li>
				<li class="liSelect">
					<label for="Status">Grupo</label>
					<%= Html.DropDownListFor(model => model.IdGrupo, (SelectList)ViewData["Grupos"], "Selecione", new { @class = "validate[required]" })%>
				</li>
				<li class="liSelect">
					<label for="Status">Status</label>
					<%= Html.DropDownListFor(model => model.Status, typeof(Usuario.EnumStatus).ToSelectList(Util.Dados.EnumHelper.TipoValor.Char), "Selecione", new { @class = "validate[required]" })%>
				</li>
				
				<li class="liSubmit">
					<input class="submit" type="submit" value="Cadastrar" />
				</li>
			</ul>
		</form>
	</div>

</asp:Content>


