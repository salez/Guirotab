<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Models.Log>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Logs
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="titulo">
        <h2>Logs</h2>
    </div>
    
    <div class="filtros">
        <form method="post" action="">
            <span>
                <label>Usuário</label><br />
                <%= Html.TextBox("Usuario.Nome", "", new { maxlength = "50" })%>
            </span>
            
            <span>
                <label>Página</label><br />
                <%= Html.DropDownList("Pagina", typeof(Models.Log.EnumPagina).ToSelectList(Util.Dados.EnumHelper.TipoValor.String), "Todas")%>
            </span>
            
            <span>
                <label>Ação</label><br />
                <%= Html.DropDownList("TipoAcao", typeof(Models.Log.EnumTipo).ToSelectList(Util.Dados.EnumHelper.TipoValor.Char), "Todas")%>
            </span>
            
            <span>
                <label>Data Início</label><br />
                <%= Html.TextBox("DataDe", "", new { @class = "data" })%>
            </span>
            
            <span>
                <label>Data Fim</label><br />
                <%= Html.TextBox("DataAte", "", new { @class = "data" })%>
            </span>
            
            <div class="filtrar">
                <input type="submit" value="filtrar" />
            </div>
        </form>
    </div>
    
    <%Html.RenderPartial("Pager", String.Empty); %>
    
    <div class="conteudo-baixo">
        <div class="tabela-container">

            <% if (Model.Count() > 0)
               { %>

            <table class="tabela tablesorter tabelapager">
                <thead>
                    <tr>
                        <th>
                            Id
                        </th>
                        <th>
                            Usuário (ou Território)
                        </th>
                        <th>
                            Descricao
                        </th>
                        <th>
                            Ação
                        </th>
                        <th>
                            Página
                        </th>
                        <th>
                            Link
                        </th>
                        <th>
                            Data
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <% foreach (var item in Model)
                       { %>
                    <tr>
                        <td>
                            <%= Html.Encode(item.Id)%>
                        </td>
                        <td>
                            <%= (item.Usuario != null) ? Html.Encode(item.Usuario.Nome) : ""%>
                            <%= (item.Territorio != null) ? Html.Encode(item.Territorio.Id) : ""%>
                        </td>
                        <td>
                            <%= item.Descricao%>
                        </td>
                        <td>
                            <%= Html.Encode((Models.Log.EnumTipo)item.TipoAcao)%>
                        </td>
                        <td>
                            <%= Html.Encode(item.Pagina)%>
                        </td>
                        <td>
                            <a href="<%= item.Link %>">Acessar</a>
                        </td>
                        <td>
                            <%= Html.Encode(item.DataInclusao.Formata(Util.Data.FormatoData.Completo))%>
                        </td>
                    </tr>
                    <% } %>
                </tbody>
            </table>

            <%}
               else { %>
               
               Nenhum Log encontrado.

               <%} %>
        </div>
    </div>
    
    <%Html.RenderPartial("Pager", String.Empty); %>
    
</asp:Content>