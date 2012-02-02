<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Models.Email>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Emails
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">

    <script type="text/javascript">
        function dialog(id) {
            var dialogEmail = $('#visualizar-'+id).dialog({
                title: "Visualizar Email",
                width: 750,
                height: 700,
                modal: true
            });

            $('#visualizar-' + id).load('<%=Url.Action("ExibeMail")%>' + '/' + id);
        }
    </script>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <div class="titulo">
        <h2>Emails</h2>
    </div>
    
    <div class="filtros">
        <form method="post" action="">
            <span>
                <label>De</label>
                <%= Html.TextBox("Email.De", "", new { maxlength = "50" })%>
            </span>
            
            <span>
                <label>Para</label><br />
                <%= Html.TextBox("Destinatario", "", new { maxlength = "50" })%>
            </span>
            
            <span>
                <label>Assunto</label><br />
                <%= Html.TextBox("Email.Assunto", "", new { maxlength = "50" })%>
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
            <table class="tabela tablesorter tabelapager">
                <thead>
                    <tr>
                        <th>
                            Id
                        </th>
                        <th>
                            De
                        </th>
                        <th>
                            Para
                        </th>
                        <th>
                            Assunto
                        </th>
                        <th>
                            Data
                        </th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    <% foreach (var item in Model) { %>
                    <tr>
                        <td>
                            <%= Html.Encode(item.Id) %>
                        </td>
                        <td>
                            <%= Html.Encode(item.De) %> (<%=item.DeEmail %>)
                        </td>
                        <td>
                            <% var destinatario = item.EmailDestinatarios.Where(d => d.Tipo == (Char)Models.Email.EnumTipo.Destinatario).FirstOrDefault(); %>
                            
                            <% if(destinatario != null) { %>
                                <%= Html.Encode(destinatario.Email)%> <%= (item.EmailDestinatarios.Count() > 1)?"... ("+ item.EmailDestinatarios.Count().ToString() + " destinatários" : ""%>
                            <% } %>
                        </td>
                        <td>
                            <%=item.Assunto %>
                        </td>
                        <td>
                            <%=item.DataInclusao.Formata(Util.Data.FormatoData.Completo) %>
                        </td>
                        <td>
                            
                            <div id="visualizar-<%=item.Id %>" class="modal" style="display: none;">
                            </div>
                            
                            <a href="javascript:dialog('<%=item.Id %>')">Visualizar&nbsp;Email</a>
                            
                        </td>
                    </tr>
                    <% } %>
                </tbody>
            </table>
        </div>
    </div>
    
    <%Html.RenderPartial("Pager",String.Empty); %>

</asp:Content>
