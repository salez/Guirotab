<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Models.Controlador>>" %>
<%@ Import Namespace="Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="titulo">
        <h2>Sistema/Controladores</h2>
    </div>  
    <div class="botoes">
        <%=Html.ActionLink("Cadastrar Novo Controlador","cadastrocontrolador") %>
    </div>
    
    <% foreach (Controlador controlador in Model) { %>
    
    <div class="controlador-container">
        <div class="controlador">
            <div class="titulo-controlador">
                <h2>
                    <div style="display: block; float: left; margin-left: 16px;">
                        <%=controlador.Nome %>
                    </div>
                    
                </h2>
                <%=Html.ActionLink(" ", "alterarcontrolador", new { id = controlador.Id }, new { @class = "lapis" })%>
                <%=Html.ActionLink(" ", "excluircontrolador", new { id = controlador.Id }, new { @class = "lixo", onclick = "return confirm('Tem certeza de que deseja excluir este registro?');" })%>
             </div>
            
            <div class="botoes">
                <%=Html.ActionLink("Cadastrar Nova Acao","cadastroacao", new { id = controlador.Id }) %>
            </div>
            
            <div class="conteudo-baixo">
                <div class="tabela-container">
                    <table class="tabela tablesorter">
                        <thead>
                            <tr>
                                <th>
                                    Nome
                                </th>
                                <th>
                                    Descrição
                                </th>
                                <th class="acao">&nbsp;</th>
                            </tr>
                        </thead>
                        <tbody>
                        
                        <% foreach (Acao acao in controlador.Acaos) { %>

                            <tr>
                                <td>
                                    <%= Html.Encode(acao.Nome)%>
                                </td>
                                <td>
                                    <%= Html.Encode(acao.Descricao)%>
                                </td>
                                <td class="icones">
                                    <%=Html.ActionLink(" ", "alteraracao", new { id = acao.Id }, new { @class = "lapis" })%>
                                    <%=Html.ActionLink(" ", "excluiracao", new { id = acao.Id }, new { @class = "lixo", onclick = "return confirm('Tem certeza de que deseja excluir este registro?');" })%>
                                </td>
                            </tr>
                        
                        <% } %>
                        
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>
    <% } %>

</asp:Content>