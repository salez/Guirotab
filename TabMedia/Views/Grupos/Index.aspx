<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Models.Grupo>>" %>
<%@ Import Namespace="Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Grupos
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="titulo">
        <h2>Grupos</h2>
    </div>  
    
    <div class="botoes">
        <%=Html.ActionLink("Cadastrar","cadastro") %>
    </div>
    
    <div class="conteudo-baixo">
        <div class="tabela-container">
            <table class="tabela tablesorter">
                <thead>
                    <tr>
                        <th align="left">
                            id
                        </th>
                        <th align="left">
                            Nome
                        </th>
                        <th class="acao" style="width: 120px;">&nbsp;</th>
                    </tr>
                </thead>
                <tbody>
                <% foreach (Grupo item in Model) { %>
                
                    <tr>
                        <td>
                            <%= Html.Encode(item.Id) %>
                        </td>
                        <td>
                            <%= Html.Encode(item.Nome) %>
                        </td>
                        <td class="icones">
                            <%=Html.ActionLink("Permissões", "permissoes", new { id = item.Id })%>
                            <%=Html.ActionLink(" ", "alterar", new { id = item.Id }, new { @class = "lapis" })%>
                            <%=Html.ActionLink(" ", "excluir", new { id = item.Id }, new { @class = "lixo", onclick = "return confirm('Tem certeza de que deseja excluir este registro?');" })%>
                        </td>
                    </tr>
                
                <% } %>
                </tbody>
            </table>
        </div>
    </div>

</asp:Content>