<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Models.Usuario>>" %>
<%@ Import Namespace="Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Usuários
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="titulo">
        <h2>Usuários</h2>
    </div>  
    
    <%Html.RenderPartial("submenu"); %>
    
    <div class="botoes">
        <%=Html.ActionLink("Cadastrar","cadastro") %>
    </div>
    
    <div class="filtros">
        <form method="post" action="">
            <span>
                <label>Usuário</label><br />
                <%= Html.TextBox("Usuario.Nome", "", new { maxlength = "50" })%>
            </span>
            
            <span>
                <label>Status</label><br />
                <%= Html.DropDownList("Usuario.Status", typeof(Models.Usuario.EnumStatus).ToSelectList(Util.Dados.EnumHelper.TipoValor.Char), "Todas")%>
            </span>
            
            <span>
                <label>Grupo</label><br />
                <%= Html.DropDownList("Usuario.IdGrupo", (SelectList)ViewData["grupos"], "Todos")%>
            </span>
            
            <div class="filtrar">
                <input type="submit" value="filtrar" />
            </div>
        </form>
    </div>

    <% if (Model.Count() > 0)
           { %>

    <%Html.RenderPartial("Pager", String.Empty); %>
    
    <div class="conteudo-baixo">

        <div class="tabela-container">
            <table class="tabela tablesorter tabelapager">
                <thead>
                    <tr>
                        <th>Territorio Simulado</th>
                        <th>
                            Nome
                        </th>
                        <th>
                            Login
                        </th>
                        <th>
                            Email
                        </th>
                        <th>
                            Status
                        </th>
                        <th>
                            Grupo
                        </th>
                        <th>
                            Data do Último Login
                        </th>
                        <th>
                            Data de Inclusão
                        </th>
                        <th class="acao">&nbsp;</th>
                    </tr>
                </thead>
                <tbody>
                <% foreach (Usuario item in Model)
                   { %>
                    <tr>
                        <td>
                            <%= Html.Encode(item.TerritorioSimulado)%>
                        </td>
                        <td>
                            <%= Html.Encode(item.Nome)%>
                        </td>
                        <td>
                            <%= Html.Encode(item.Login)%>
                        </td>
                        <td>
                            <%= Html.Encode(item.Email)%>
                        </td>
                        <td>
                            <%= Html.Encode((item.Status.HasValue) ? Util.Dados.EnumHelper.GetDescription((Usuario.EnumStatus)item.Status) : string.Empty)%>
                        </td>
                        <td>
                            <%= Html.Encode(item.Grupo.Nome)%>
                        </td>
                        <td>
                            <%= Html.Encode(Util.Data.Formata(item.DataUltimoLogin, new Util.Data.FormatoData[] { Util.Data.FormatoData.DiaMesAno, Util.Data.FormatoData.HoraMinuto }))%>
                        </td>
                        <td>
                            <%= Html.Encode(Util.Data.Formata(item.DataInclusao, new Util.Data.FormatoData[] { Util.Data.FormatoData.DiaMesAno, Util.Data.FormatoData.HoraMinuto }))%>
                        </td>
                        <td class="icones">
                            <%=Html.ActionLink(" ", "alterar", new { id = item.Id }, new { @class = "editar" })%>
                            <%=Html.ActionLink(" ", "excluir", new { id = item.Id }, new { @class = "excluir", onclick = "return confirm('Tem certeza de que deseja excluir este registro?');" })%>
                        </td>
                    </tr>
                <% } %>
                </tbody>
            </table>
        </div>

    </div>
    
    <%Html.RenderPartial("Pager", String.Empty); %>

    <%}
    else { %>
        <div class="conteudo-baixo" style="float: left; clear: both;">
            Nenhum usuário encontrado.
        </div>
    <%} %>
    
</asp:Content>