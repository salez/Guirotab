<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<MakroCestas.Areas.Admin.Models.Oferta>>" %>
<%@ Import Namespace="MakroCestas.Areas.Admin.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Gerenciador de Ofertas
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <div class="titulo">
        <h2>Gerenciador de Ofertas</h2>
    </div>  
    
    <div class="sub-menu">
        <%=Html.ActionLink("Cadastrar nova oferta","cadastro") %>
    </div>

     <div class="conteudo-baixo">
        <div class="tabela-container">
        
            <%if (Model.Count() == 0)
              { %>
                
                <div style="height: 50px; line-height: 50px; font-size: 12px;">
                    <center><strong>Não há ofertas cadastradas</strong></center>
                </div>
            <%}
              else
              { %>
            
            <table width="996" border="0" cellpadding="0" cellspacing="3" class="tabela tablesorter">
                <thead>
                <tr>
                    <th style="text-align: center">
                        De
                    </th>
                    <th style="text-align: center">
                        Até
                    </th>
                    <th style="text-align: center">
                        Qtde de lojas
                    </th>
                    <th style="text-align: center">
                        Planilha
                    </th>
                    <th class="acao">&nbsp;</th>
                </tr>
                </thead>
                <tbody>
            <% foreach (var item in Model)
               {
                   int qtdeLojas = (from l in item.OfertaCestas
                                        select l.NumeroLoja).Distinct().Count();
                   %>
                
                <tr>
                    <td style="text-align: center">
                        <%= Html.Encode(item.DataInicio.Formata(Util.Data.FormatoData.DiaMesAno))%>
                    </td>
                    <td style="text-align: center">
                        <%= Html.Encode(item.DataFim.Formata(Util.Data.FormatoData.DiaMesAno))%>
                    </td>
                    <td style="text-align: center">
                        <%= qtdeLojas%>
                    </td>
                    <td style="text-align: center">
                        <a href="<%=Url.Content("~/Upload/Ofertas/"+item.Id+".xls") %>"><%=item.Id + ".xls"%></a>
                    </td>
                    <td style="width: 150px;">
                         <%=Html.ActionLink("Alterar", "alterar", new { id = item.Id })%> | <%=Html.ActionLink("Visualizar", "visualizar", new { id = item.Id })%> 
                         | <%=Html.ActionLink("Excluir", "excluir", new { id = item.Id }, new { onclick = "return confirm('Tem certeza de que deseja excluir este registro?');" })%>
                    </td>
                </tr>
            
            <% } %>
                </tbody>
            </table>
            
            <%} %>
            
        </div>
     </div>

</asp:Content>