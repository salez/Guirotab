<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Models.Erro>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Erros
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">

    <script type="text/javascript">
        function dialog(id) {
            var dialogImagens = $('#visualizar-'+id).dialog({
                title: "Visualizar Erro",
                width: 900,
                height: 600,
                modal: true
            });

            $('#visualizar-' + id).load('<%=Url.Action("ExibeInfo")%>' + '/' + id);
        }
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="titulo">
        <h2>Erros</h2>
    </div>
    
    <%Html.RenderPartial("Pager", String.Empty); %>
    
    <div class="conteudo-baixo">
        <div class="tabela-container">
            <table class="tabela tablesorter tabelapager">
                <thead>
                    <tr>
                        <th>
                            Id
                        </th>
                        <th>
                            Pagina
                        </th>
                        <th>
                            PaginaAnterior
                        </th>
                        <th>
                            InfoUsuario
                        </th>
                        <th>
                            Data de Inclusão
                        </th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                <% foreach (Models.Erro item in Model) { %>
                    <tr>
                        <td>
                            <%= Html.Encode(item.Id) %>
                        </td>
                        <td>
                            <a href="<%= item.Pagina %>" title="<%= item.Pagina %>" target="_blank"><%= Html.Encode(item.Pagina.Right(50)) %></a>
                        </td>
                        <td>
                            <a href="<%= item.PaginaAnterior %>" title="<%= item.PaginaAnterior %>" target="_blank"><%= Html.Encode(item.PaginaAnterior.Right(30)) %></a>
                        </td>
                        <td>
                            <%= item.InfoUsuario %>
                        </td>
                        <td>
                            <%= Html.Encode(item.DataInclusao.Formata(Util.Data.FormatoData.Completo)) %>
                        </td>
                        <td>
                            <div id="visualizar-<%=item.Id %>" class="modal" style="display: none;">
                                Carregando...
                            </div>
                            
                            <a href="javascript:dialog('<%=item.Id %>')">Visualizar&nbsp;Erro</a>
                        </td>
                    </tr>
                <% } %>
                </tbody>
            </table>
        </div>
    </div>
    
    <%Html.RenderPartial("Pager", String.Empty); %>

</asp:Content>